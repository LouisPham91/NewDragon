using DirectN;
using Dragon.Controller.Database.Services;
using Dragon.Controller.GlobalControl.Property;
using Dragon.Database.Models;
using FFmpeg.AutoGen;
using SDL3;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Dragon.ControlHelper.ScrcpyNet.Services
{
    public sealed class Sdl3RenderrEngine : IDisposable
    {
        public string DeviceID { get; }
        private readonly nint _panelHandle;
        private Phone? Phone;
        private nint _win, _renderer, _videoTex, _overlayTex;
        private SDL.FRect _dstRect, _overlayRect;
        private int _vidW, _vidH;
        private AVPixelFormat _pixFmt = AVPixelFormat.AV_PIX_FMT_NONE;
        public string Textnumber { get; private set; } = "";
        public string TextModel { get; private set; } = "";
        private int _numSize, _modelSize;
        IPDController _controller;
        CancellationToken token;
        private long _lastOverlayBuild = 0;

        public Sdl3RenderrEngine(string deviceId, nint panelHandle, IPDController controller)
        {
            DeviceID = deviceId;
            _panelHandle = panelHandle;
            _controller = controller;
            Phone = PhoneRepository.FindOneByDeviceID(DeviceID) ?? throw new InvalidOperationException($"Phone {DeviceID} not found");
        }

        public void InitSdl()
        {
            if (_win != nint.Zero) return;
            if ((SDL.WasInit(SDL.InitFlags.Video) & SDL.InitFlags.Video) == 0)
                SDL.Init(SDL.InitFlags.Video);

            uint props = SDL.CreateProperties();
            SDL.SetPointerProperty(props, "SDL.window.create.win32.hwnd", _panelHandle);
            SDL.SetBooleanProperty(props, "SDL.window.create.borderless", true);
            SDL.SetBooleanProperty(props, "SDL.window.create.resizable", false);
            _win = SDL.CreateWindowWithProperties(props);
            SDL.DestroyProperties(props);
            if (_win == nint.Zero)
                throw new Exception($"SDL_CreateWindow failed: {SDL.GetError()}");
        }
        public void SetToken(CancellationToken cancellation)
        {
            token = cancellation;
        }

        public void SetVideoSize(int w, int h) { _vidW = w; _vidH = h; }


        public void InitializeRenderer()
        {
            if (token.IsCancellationRequested) return;
            CleanupRender();
            if (_vidW == 0 || _vidH == 0) return;

            SDL.GetWindowSize(_win, out int ww, out int wh);
            _dstRect = new SDL.FRect { X = 0, Y = 0, W = ww, H = wh };
            _renderer = SDL.CreateRenderer(_win, null);

            var fmt = _pixFmt == AVPixelFormat.AV_PIX_FMT_NV12 ? SDL.PixelFormat.NV12 : SDL.PixelFormat.IYUV;
            _videoTex = SDL.CreateTexture(_renderer, fmt, SDL.TextureAccess.Streaming, _vidW, _vidH);
            SetOverlayText();
        }
        public void UpdateDisplaySizeV2(int w, int h)
        {
            if (token.IsCancellationRequested) return;
            if (w == _dstRect.W || w == _dstRect.H)
            {
                _dstRect.W = w;
                _dstRect.H = h;
            }
            else
            {
                if (Phone == null) return;
                var allSize = GetSettings.GetALLSize(Phone);
                if (h > w)
                {
                    _dstRect.W = allSize.min.w;
                    _dstRect.H = allSize.min.h;
                }
                else
                {
                    _dstRect.W = allSize.min.h;
                    _dstRect.H = allSize.min.w;
                }
            }

            SetOverlayText();
        }
        public void UpdateDisplaySize(int w, int h)
        {
            if (token.IsCancellationRequested) return;
            _dstRect.W = w;
            _dstRect.H = h;
            SetOverlayText();
        }
        public void ShowModelName()
        {
            if (GetSettings.GetDisplayTitle() == false)
            {
                if (_overlayTex != 0) SDL.DestroyTexture(_overlayTex);
                _overlayTex = 0;
            }
            else
            {
                SetOverlayText();
            }
        }

        public void SetOverlayText()
        {
            if (token.IsCancellationRequested) return;
            Stopwatch stopwatch = Stopwatch.StartNew();
            if (GetSettings.GetDisplayTitle() == false)
            {
                if (_overlayTex != 0) SDL.DestroyTexture(_overlayTex);
                _overlayTex = 0;
                return;
            }
            long now = Environment.TickCount64;
            if (now - _lastOverlayBuild < 120) return;
            _lastOverlayBuild = now;

            if (Phone == null || Phone.DeviceID != DeviceID)
            {
                Debug.WriteLine("Phone SetOverlayText - FindOneByDeviceID");
                Phone = PhoneRepository.FindOneByDeviceID(DeviceID);
            }
            var phone = Phone;
            if (phone == null) return;

            var tagNum = phone.PhoneTagNumber;

            var isModel = GetSettings.GetDisplayModelName();
            var isSerial = GetSettings.GetDisplaySerial();
            var isIP = GetSettings.GetDisplayIP();

            var serial = (phone.IsUSBmode()) ? DeviceID : (phone.Serial ?? "No IP");

            string displayline = "";
            if (isModel) displayline = phone.Model;
            if (isSerial) displayline = serial;
            if (isIP) displayline = !string.IsNullOrEmpty(phone.Ipv4) ? phone.Ipv4 : "No IP";

            Textnumber = (tagNum < 10) ? "0" + tagNum : tagNum.ToString();
            TextModel = displayline;

            if (_win == IntPtr.Zero) return;
            SDL.GetWindowSize(_win, out int w, out int h);
            if (w < 10) w = (int)_dstRect.W;
            if (h < 10) h = (int)_dstRect.H;
            if (w < 10 || h < 10) return;

            int refSize = Math.Min(w, h);

            bool smallPanel = w < 200;
            float numScale = smallPanel ? 0.16f : 0.22f;
            int minNumSize = smallPanel ? 9 : 14;
            int minModelSize = smallPanel ? 8 : 10;

            int numSize = Math.Max(minNumSize, (int)(refSize * numScale));
            numSize = (numSize / 2) * 2; // SỬA: ép về chẵn ngay từ đầu
            nint fNum = FontManagerSDL3.GetFont(numSize);

            nuint lenNum = (nuint)Encoding.UTF8.GetByteCount(Textnumber);
            TTF.GetStringSize(fNum, Textnumber, lenNum, out int twNum, out int thNum);

            int safety = 0;
            while (twNum > w * 0.75f && numSize > minNumSize && safety++ < 40)
            {
                numSize -= 2; // SỬA: luôn trừ 2 để giữ chẵn
                fNum = FontManagerSDL3.GetFont(numSize);
                TTF.GetStringSize(fNum, Textnumber, lenNum, out twNum, out thNum);
            }
            _numSize = Math.Max(minNumSize, (numSize / 2) * 2); // SỬA

            int modelSize = Math.Max(minModelSize, (int)(_numSize * 0.45f));
            modelSize = (modelSize / 2) * 2; // SỬA
            nint fMod = FontManagerSDL3.GetFont(modelSize);

            nuint lenMod = (nuint)Encoding.UTF8.GetByteCount(TextModel);
            TTF.GetStringSize(fMod, TextModel, lenMod, out int twMod, out int thMod);

            safety = 0;
            while (twMod > w * 0.90f && modelSize > minModelSize && safety++ < 50)
            {
                modelSize -= 2; // SỬA: trừ 2
                fMod = FontManagerSDL3.GetFont(modelSize);
                if (!TTF.GetStringSize(fMod, TextModel, lenMod, out twMod, out thMod)) break;
            }
            _modelSize = Math.Max(minModelSize, (modelSize / 2) * 2); // SỬA

            BuildOverlay(w, h);
            stopwatch.Stop();
            Debug.WriteLine("Build Overlay : " + stopwatch.ElapsedMilliseconds + "ms");
        }

        private void BuildOverlay(int width, int height)
        {
            if (_renderer == nint.Zero) return;
            if (token.IsCancellationRequested) return;

            nint fNum = FontManagerSDL3.GetFont(_numSize);
            nint fMod = FontManagerSDL3.GetFont(_modelSize);
            var white = new SDL.Color { R = 255, G = 255, B = 255, A = 255 };

            nint s1 = TTF.RenderTextBlended(fNum, Textnumber, 0, white);
            nint s2 = TTF.RenderTextBlended(fMod, TextModel, 0, white);
            if (s1 == 0 || s2 == 0) return;

            var r1 = Marshal.PtrToStructure<SDL.Surface>(s1);
            var r2 = Marshal.PtrToStructure<SDL.Surface>(s2);
            int cw = Math.Max(r1.Width, r2.Width) + 8;
            int ch = r1.Height + r2.Height + 4;

            nint surf = SDL.CreateSurface(cw, ch, SDL.PixelFormat.ARGB8888);
            SDL.Rect d1 = new() { X = (cw - r1.Width) / 2, Y = 0, W = r1.Width, H = r1.Height };
            SDL.BlitSurface(s1, nint.Zero, surf, in d1);
            SDL.Rect d2 = new() { X = (cw - r2.Width) / 2, Y = r1.Height + 2, W = r2.Width, H = r2.Height };
            SDL.BlitSurface(s2, nint.Zero, surf, in d2);

            // SỬA: bọc tạo/huỷ texture bằng lock để không đụng RenderFrame

            if (_overlayTex != nint.Zero) { SDL.DestroyTexture(_overlayTex); _overlayTex = nint.Zero; }

            _overlayTex = SDL.CreateTextureFromSurface(_renderer, surf);
            SDL.SetTextureBlendMode(_overlayTex, SDL.BlendMode.Blend);

            int baseAlpha = 200;
            int alpha = width <= 200 ? baseAlpha : baseAlpha - (int)((width - 200) * 0.12f);
            if (alpha < 90) alpha = 90;
            if (alpha > 255) alpha = 255;
            SDL.SetTextureAlphaMod(_overlayTex, (byte)alpha);

            _overlayRect = new SDL.FRect
            {
                X = (width - cw) / 2f,
                Y = height * 0.06f,
                W = cw,
                H = ch
            };


            SDL.DestroySurface(s1);
            SDL.DestroySurface(s2);
            SDL.DestroySurface(surf);
        }

        public unsafe void RenderFrame(AVFrame frame)
        {
            if (token.IsCancellationRequested) return;
            if (_renderer == nint.Zero) return;

            // --- debug ---
            //Debug.WriteLine($"[Render] frm {frame.width}x{frame.height} fmt={(AVPixelFormat)frame.format} d0={(frame.data[0] == null ? "null" : "ok")}");

            if (frame.width == 0 || frame.height == 0 || frame.data[0] == null)
            {
                Debug.WriteLine("[Render] skip invalid frame");
                return;
            }

            // resize nếu cần
            if (frame.width != _vidW || frame.height != _vidH || (AVPixelFormat)frame.format != _pixFmt)
            {
                Debug.WriteLine($"[Render] size change {_vidW}x{_vidH} -> {frame.width}x{frame.height}");
                _vidW = frame.width;
                _vidH = frame.height;
                _pixFmt = (AVPixelFormat)frame.format;

                if (_videoTex != 0)
                {
                    SDL.DestroyTexture(_videoTex);
                    _videoTex = 0;
                }

                var fmt = _pixFmt == AVPixelFormat.AV_PIX_FMT_NV12 ? SDL.PixelFormat.NV12 : SDL.PixelFormat.IYUV;
                _videoTex = SDL.CreateTexture(_renderer, fmt, SDL.TextureAccess.Streaming, _vidW, _vidH);

                if (_videoTex == 0)
                {
                    Debug.WriteLine($"[Render] CreateTexture failed: {SDL.GetError()}");
                    return;
                }

                SetOverlayText();
                // skip frame đầu sau resize – tránh update texture vừa tạo
                Debug.WriteLine("[Render] skip first frame after resize");
                return;
            }

            if (_videoTex == nint.Zero)
            {
                Debug.WriteLine("[Render] no texture");
                return;
            }

            try
            {
                if (_pixFmt == AVPixelFormat.AV_PIX_FMT_NV12)
                {
                    if (frame.data[1] == null) { Debug.WriteLine("[Render] skip nv12 no uv"); return; }
                    SDL.UpdateNVTexture(_videoTex, IntPtr.Zero, (nint)frame.data[0], frame.linesize[0], (nint)frame.data[1], frame.linesize[1]);
                }
                else
                {
                    if (frame.data[1] == null || frame.data[2] == null) { Debug.WriteLine("[Render] skip yuv no planes"); return; }
                    SDL.UpdateYUVTexture(_videoTex, IntPtr.Zero, (nint)frame.data[0], frame.linesize[0], (nint)frame.data[1], frame.linesize[1], (nint)frame.data[2], frame.linesize[2]);
                }

                SDL.RenderClear(_renderer);
                SDL.RenderTexture(_renderer, _videoTex, IntPtr.Zero, _dstRect);
                if (_overlayTex != nint.Zero)
                    SDL.RenderTexture(_renderer, _overlayTex, IntPtr.Zero, _overlayRect);
                SDL.RenderPresent(_renderer);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Render] EX {ex.Message}");
            }
        }

        private void CleanupRender()
        {
            if (_overlayTex != 0) SDL.DestroyTexture(_overlayTex);
            if (_videoTex != 0) SDL.DestroyTexture(_videoTex);
            if (_renderer != 0) SDL.DestroyRenderer(_renderer);
            _overlayTex = _videoTex = _renderer = 0;
        }

        public void Dispose()
        {
            CleanupRender();
            if (_win != 0) SDL.DestroyWindow(_win);
            _win = 0;
        }
    }
}