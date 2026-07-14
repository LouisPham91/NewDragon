using LibUsbDotNet;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Dragon.Controller.DeviceControl
{

    public class AppCapture : IAsyncDisposable
    {
        public string DeviceId { get; }
        public string Ip { get; }
        private readonly int _port;
        private TcpClient? _client;
        private NetworkStream? _stream;
        private readonly SemaphoreSlim _lock = new(1, 1); // chỉ 1 lệnh/lúc
        private readonly SemaphoreSlim _connectLock = new(1, 1);
        private DateTime _lastUse = DateTime.UtcNow;

        public AppCapture(string deviceId, string ip, int port = 8888)
        {
            DeviceId = deviceId;
            Ip = ip;
            _port = port;
        }

        private async Task EnsureConnectedAsync()
        {
            if (_client?.Connected == true) return;
            await _connectLock.WaitAsync();
            try
            {
                if (_client?.Connected == true) return;
                _client?.Dispose();
                _client = new TcpClient { NoDelay = true };
                using var cts = new CancellationTokenSource(3000);
                await _client.ConnectAsync(Ip, _port, cts.Token);
                _stream = _client.GetStream();
                _stream.ReadTimeout = 120_000;
                _stream.WriteTimeout = 5000;
            }
            finally { _connectLock.Release(); }
        }

        public async Task<byte[]> ScreenshotAsync(int timeoutMs = 8000)
        {
            await _lock.WaitAsync();
            try
            {
                await EnsureConnectedAsync();
                _lastUse = DateTime.UtcNow;

                var cmd = Encoding.ASCII.GetBytes("screenshot\n"); // ✅ Thêm \n
                await _stream!.WriteAsync(cmd, 0, cmd.Length);

                var sizeBuf = new byte[4];
                await ReadExactAsync(sizeBuf, timeoutMs);
                if (BitConverter.IsLittleEndian) Array.Reverse(sizeBuf);
                int size = BitConverter.ToInt32(sizeBuf, 0);
                if (size <= 0 || size > 10_000_000) throw new Exception("size lỗi");

                var img = new byte[size];
                await ReadExactAsync(img, timeoutMs);
                return img;
            }
            catch
            {
                Dispose(); // đóng để lần sau reconnect
                throw;
            }
            finally { _lock.Release(); }
        }
        public async Task<Bitmap?> ScreenshotBitmapAsync(int timeoutMs = 8000)
        {
            try
            {
                var data = await ScreenshotAsync(timeoutMs);

                // Check null hoặc empty
                if (data == null || data.Length == 0)
                {
                    Debug.WriteLine("Screenshot data is null or empty");
                    return null;
                }

                // Check minimum JPEG size (header + minimal data)
                if (data.Length < 100) // JPEG ít nhất phải có header
                {
                    Debug.WriteLine($"Screenshot data too small: {data.Length} bytes");
                    return null;
                }

                try
                {
                    using var ms = new MemoryStream(data);

                    // Validate đây là ảnh hợp lệ trước khi tạo Bitmap
                    if (!IsValidImage(ms))
                    {
                        Debug.WriteLine("Invalid image data received");
                        return null;
                    }

                    ms.Position = 0; // Reset position sau khi validate

                    // Clone để không giữ lock trên MemoryStream
                    // Dùng Image.FromStream thay vì new Bitmap(ms) để có error handling tốt hơn
                    using var tempBitmap = Image.FromStream(ms, false, false);
                    var bitmap = new Bitmap(tempBitmap); // Clone
                    return bitmap;
                }
                catch (OutOfMemoryException ex)
                {
                    Debug.WriteLine($"Out of memory loading screenshot: {ex.Message}");
                    // Force GC để cleanup
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    return null;
                }
                catch (ArgumentException ex) // Invalid parameters (corrupt image)
                {
                    Debug.WriteLine($"Invalid image data: {ex.Message}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ScreenshotBitmapAsync error: {ex.Message}");
                return null; // Return null thay vì throw để caller dễ handle
            }
        }

        // Helper method để validate ảnh
        private bool IsValidImage(Stream stream)
        {
            try
            {
                long originalPosition = stream.Position;
                stream.Position = 0;

                // Check JPEG header (FF D8)
                byte[] header = new byte[2];
                if (stream.Read(header, 0, 2) != 2)
                    return false;

                bool isValid = header[0] == 0xFF && header[1] == 0xD8;
                stream.Position = originalPosition;
                return isValid;
            }
            catch
            {
                return false;
            }
        }
       
        public async Task SendAsync(string command)
        {
            await _lock.WaitAsync();
            try
            {
                await EnsureConnectedAsync();
                var cmd = Encoding.ASCII.GetBytes(command.Trim() + "\n");
                await _stream!.WriteAsync(cmd, 0, cmd.Length);
                await _stream.FlushAsync();
            }
            catch { Dispose(); throw; }
            finally { _lock.Release(); }
        }

        private async Task ReadExactAsync(byte[] buffer, int timeoutMs)
        {
            int offset = 0;
            using var cts = new CancellationTokenSource(timeoutMs);
            while (offset < buffer.Length)
            {
                int read = await _stream!.ReadAsync(buffer, offset, buffer.Length - offset, cts.Token);
                if (read == 0) throw new IOException("disconnect");
                offset += read;
            }
        }
        public Task DeeplinkAsync(string url)
        {
            // tự thêm https:// nếu thiếu scheme
            if (!url.Contains("://")) url = "https://" + url;
            return SendAsync($"deeplink {url}");
        }
        public void Dispose()
        {
            try { _stream?.Close(); } catch { }
            try { _client?.Close(); } catch { }
            _stream = null; _client = null;
        }
        public ValueTask DisposeAsync() { Dispose(); return ValueTask.CompletedTask; }
    }

}
