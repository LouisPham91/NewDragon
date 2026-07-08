using LibUsbDotNet;
using System;
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

                var cmd = Encoding.ASCII.GetBytes("screenshot\n");
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
        public async Task<Bitmap> ScreenshotBitmapAsync(int timeoutMs = 8000)
        {
            var data = await ScreenshotAsync(timeoutMs);
            using var ms = new MemoryStream(data);
            // Clone để không giữ lock trên MemoryStream
            return new Bitmap(ms);
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
