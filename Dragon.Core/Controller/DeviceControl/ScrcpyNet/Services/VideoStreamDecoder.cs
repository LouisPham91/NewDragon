using FFmpeg.AutoGen;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;

namespace Dragon.Controller.DeviceControl.ScrcpyNet.Services
{
    public unsafe class FrameData : IDisposable
    {
        private bool disposed;

        public ReadOnlySpan<byte> Data
        {
            get
            {
                if (disposed) throw new ObjectDisposedException(nameof(FrameData));
                return new ReadOnlySpan<byte>(data, length);
            }
        }

        public int Width { get; }
        public int Height { get; }
        public int FrameNumber { get; }
        public AVPixelFormat PixelFormat { get; }

        private readonly byte* data;
        private readonly int length;

        public FrameData(byte* data, int length, int width, int height, int frameNumber, AVPixelFormat pixelFormat)
        {
            this.data = data;
            this.length = length;
            Width = width;
            Height = height;
            FrameNumber = frameNumber;
            PixelFormat = pixelFormat;
        }

        ~FrameData()
        {
            Dispose(disposing: false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing) { }
                ffmpeg.av_free(data);
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

    public unsafe class VideoStreamDecoder : IDisposable
    {
        private readonly AVCodec* _codec;
        private readonly AVCodecParserContext* _parser;
        private readonly AVCodecContext* _ctx;
        private readonly AVFrame* _frame;
        private bool _disposed;
        private volatile bool _isScreenshot = false;
        private volatile Bitmap? _bitmapScreenShot;
        private CancellationToken _cts;
        public event Action<AVFrame>? NewFrameEvent;
        public volatile bool _IsStopRender = false;
        public VideoStreamDecoder(AVCodecID codecId = AVCodecID.AV_CODEC_ID_H264)
        {
            _codec = ffmpeg.avcodec_find_decoder(codecId);
            if (_codec == null)
                throw new InvalidOperationException($"Couldn't find AVCodec for {codecId}.");

            _parser = ffmpeg.av_parser_init((int)_codec->id);
            if (_parser == null)
                throw new InvalidOperationException("Couldn't initialize AVCodecParserContext.");

            _ctx = ffmpeg.avcodec_alloc_context3(_codec);
            if (_ctx == null)
                throw new InvalidOperationException("Couldn't allocate AVCodecContext.");

            int ret = ffmpeg.avcodec_open2(_ctx, _codec, null);
            if (ret < 0)
                throw new InvalidOperationException($"Couldn't open AVCodecContext: {GetFFmpegError(ret)}");

            _frame = ffmpeg.av_frame_alloc();
            if (_frame == null)
                throw new InvalidOperationException("Couldn't allocate AVFrame.");
        }
     
        public void Initialize(int width, int height)
        {
            if (_cts.IsCancellationRequested) return;
            if (_ctx == null)
                throw new InvalidOperationException("Codec context is not initialized.");

            _ctx->width = width;
            _ctx->height = height;
            // Không ép pix_fmt, để decoder tự chọn dựa trên codec
            _ctx->time_base = new AVRational { num = 1, den = 60 };
            Debug.WriteLine($"[VideoStreamDecoder] Initialized: {width}x{height}");
        }
        public void Flush()
        {
            if (_ctx == null) return;
            // Xóa toàn bộ frame/packet đang dở trong decoder
            ffmpeg.avcodec_flush_buffers(_ctx);
            Debug.WriteLine("[VideoStreamDecoder] flushed buffers");
        }

        private readonly object _decodeLock = new();

        public void SetToken(CancellationToken token)
        {
            _cts = token;
        }

        public void PauseRender()
        {
            _IsStopRender = true;
        }
        public void ResumeRender()
        {
            _IsStopRender = false;
        }
        public List<AVPacket> Decode(byte[] data, int length, long pts = -1)
        {
            var packets = new List<AVPacket>();
            if (_cts.IsCancellationRequested) throw new Exception();
            fixed (byte* dataPtr = data)
            {
                byte* ptr = dataPtr;
                int remaining = length;

                while (remaining > 0)
                {
                    AVPacket* packet = ffmpeg.av_packet_alloc();
                    try
                    {
                        int ret = ffmpeg.av_parser_parse2(_parser, _ctx,
                            &packet->data, &packet->size,
                            ptr, remaining,
                            pts != -1 ? pts : ffmpeg.AV_NOPTS_VALUE,
                            ffmpeg.AV_NOPTS_VALUE, 0);
                        if (ret < 0)
                        {
                            Debug.WriteLine($"[VideoStreamDecoder] Parser error: {GetFFmpegError(ret)}");
                            continue;
                        }

                        ptr += ret;
                        remaining -= ret;

                        if (packet->size > 0)
                        {
                            var cloned = *ffmpeg.av_packet_clone(packet);
                            packets.Add(cloned);
                        }
                    }
                    finally
                    {
                        ffmpeg.av_packet_free(&packet);
                    }
                }
            }
            return packets;
        }

        public void DecodePacket(AVPacket packet)
        {

            try
            {
                if (_cts.IsCancellationRequested || _ctx == null)
                {
                    ffmpeg.av_packet_unref(&packet);
                    return;
                }

                int ret = ffmpeg.avcodec_send_packet(_ctx, &packet);
                if (ret < 0 && ret != ffmpeg.AVERROR(ffmpeg.EAGAIN))
                {
                    Debug.WriteLine($"[Decode] send_packet err: {GetFFmpegError(ret)}");
                    ffmpeg.av_packet_unref(&packet);
                    return;
                }

                while (ret >= 0)
                {
                    ret = ffmpeg.avcodec_receive_frame(_ctx, _frame);
                    if (ret == ffmpeg.AVERROR(ffmpeg.EAGAIN) || ret == ffmpeg.AVERROR_EOF)
                        break;

                    if (ret < 0)
                    {
                        Debug.WriteLine($"[Decode] receive_frame err: {GetFFmpegError(ret)}");
                        break;
                    }

                    NewFrameEvent?.Invoke(*_frame);

                    if (_isScreenshot)
                    {
                        var bmp = ConvertToBitmap(*_frame); // vẫn nặng, xem dưới
                        lock (_shotLock)
                        {
                            _bitmapScreenShot?.Dispose();
                            _bitmapScreenShot = bmp;
                        }
                        _shotReady.Set();
                        _isScreenshot = false; // chỉ chụp 1 frame
                    }

                    ffmpeg.av_frame_unref(_frame);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[DecodePacket] exception: {ex.Message}");
            }
            finally
            {
                ffmpeg.av_packet_unref(&packet);
            }

        }

        private readonly ManualResetEventSlim _shotReady = new(false);
        private readonly object _shotLock = new();

        public Bitmap? ScreenShot(int timeoutMs = 500)
        {
            if (_cts.IsCancellationRequested) return null;

            lock (_shotLock)
            {
                _bitmapScreenShot?.Dispose();
                _bitmapScreenShot = null;
            }
            _shotReady.Reset();
            _isScreenshot = true;

            // chờ tối đa 500ms, check mỗi 5ms
            bool ok = _shotReady.Wait(timeoutMs);

            _isScreenshot = false;

            lock (_shotLock)
            {
                var bmp = _bitmapScreenShot;
                _bitmapScreenShot = null;
                return bmp; // trả thẳng, không new Bitmap() nữa
            }
        }



        private SwsContext* _swsCtx = null;
        private int _swsW, _swsH;
        private AVPixelFormat _swsSrcFmt;
        public Bitmap? ConvertToBitmap(AVFrame frame)
        {
            int w = frame.width, h = frame.height;
            var srcFmt = (AVPixelFormat)frame.format;

            // reuse context
            if (_swsCtx == null || w != _swsW || h != _swsH || srcFmt != _swsSrcFmt)
            {
                ffmpeg.sws_freeContext(_swsCtx);
                _swsCtx = ffmpeg.sws_getContext(w, h, srcFmt, w, h,
                    AVPixelFormat.AV_PIX_FMT_BGRA, 2, null, null, null);
                _swsW = w; _swsH = h; _swsSrcFmt = srcFmt;
            }

            byte_ptrArray4 dst = new(); int_array4 lines = new();
            ffmpeg.av_image_alloc(ref dst, ref lines, w, h, AVPixelFormat.AV_PIX_FMT_BGRA, 1);

            ffmpeg.sws_scale(_swsCtx, frame.data, frame.linesize, 0, h, dst, lines);

            var bmp = new Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var data = bmp.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.WriteOnly, bmp.PixelFormat);
            // copy 1 lần, nhanh hơn vòng for
            Buffer.MemoryCopy(dst[0], (void*)data.Scan0, data.Stride * h, lines[0] * h);
            bmp.UnlockBits(data);

            ffmpeg.av_free(dst[0]);
            return bmp;
        }


        private string GetFFmpegError(int errorCode)
        {
            byte[] buffer = new byte[512];
            fixed (byte* ptr = buffer)
            {
                ffmpeg.av_strerror(errorCode, ptr, (ulong)buffer.Length);
                return Encoding.ASCII.GetString(buffer, 0, buffer.Length).TrimEnd('\0');
            }
        }

        ~VideoStreamDecoder()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                // managed
                _shotReady?.Dispose();
                lock (_shotLock)
                {
                    _bitmapScreenShot?.Dispose();
                    _bitmapScreenShot = null;
                }
            }

            // unmanaged - luôn free dù disposing = false
            if (_swsCtx != null)
            {
                ffmpeg.sws_freeContext(_swsCtx);
                _swsCtx = null;
            }
            if (_parser != null)
            {
                ffmpeg.av_parser_close(_parser);
            }
            if (_ctx != null)
            {
                var ctx = _ctx;
                ffmpeg.avcodec_free_context(&ctx);
            }
            if (_frame != null)
            {
                var f = _frame;
                ffmpeg.av_frame_free(&f);
            }

            _disposed = true;
        }

    }
}