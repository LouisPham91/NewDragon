using Dragon.Database.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Dragon.Controller.DeviceControl
{
    /// <summary>
    /// Cách dùng nhanh:
    /// <code>
    /// ScreenShotApp.Instance.Add("192.168.1.101");
    /// var img = await ScreenShotApp.Instance.ScreenshotAsync("192.168.1.101");
    /// await ScreenShotApp.Instance.DeeplinkAsync("192.168.1.101", "fb://page/123");
    /// ScreenShotApp.Instance.Remove("192.168.1.101");
    /// </code>
    /// </summary>
    public sealed class AppCaptureManager
    {
        private AppCaptureManager() { }

        private static readonly Lazy<AppCaptureManager> _lazy = new(() => new AppCaptureManager());
        public static AppCaptureManager Instance => _lazy.Value;

        private readonly ConcurrentDictionary<string, AppCapture> _map = new();
        private readonly ConcurrentDictionary<string, string> _deviceToIp = new(StringComparer.OrdinalIgnoreCase);

        public bool GetIpByDeviceId(string deviceId)
        {
            return _deviceToIp.TryGetValue(deviceId, out var ip);
        }
        // ---- API MỚI cho DLoop ----
        public bool Add(Phone phone, int port = 8888)
        {
            if (phone == null) return false;
            if (!phone.IsWifimode()) return false; // chỉ wifi
            if (string.IsNullOrWhiteSpace(phone.Ipv4)) return false;
            if (!phone.IsPingWifi) return false; // ping ok mới dùng

            _deviceToIp[phone.DeviceID] = phone.Ipv4;
            _map[phone.Ipv4] = new AppCapture(phone.Ipv4, port);
            return true;
        }

        public Task<byte[]> ScreenshotAsync(Phone phone)
        {
            if (!_deviceToIp.TryGetValue(phone.DeviceID, out var ip))
                throw new InvalidOperationException($"Phone {phone.DeviceID} chưa Add vào ScreenShotApp");
            return ScreenshotAsync(ip);
        }

        public async Task<Bitmap> ScreenShotByPhone(Phone phone)
        {
            var data = await ScreenshotAsync(phone);
            using var ms = new MemoryStream(data);
            return (Bitmap)Image.FromStream(ms);
        }

        public bool Remove(Phone phone)
        {
            if (_deviceToIp.TryRemove(phone.DeviceID, out var ip))
                return Remove(ip);
            return false;
        }

        // --- thêm 3 hàm này ---
        public bool Remove(string ip)
        {
            if (_map.TryRemove(ip, out var conn))
            {
                conn.Dispose(); // đóng socket, giải phóng
                return true;
            }
            return false;
        }
        public void Clear()
        {
            foreach (var kv in _map) kv.Value.Dispose();
            _map.Clear();
        }
        public Task DeeplinkAsync(string ip, string url) => _map[ip].DeeplinkAsync(url);
        public IEnumerable<string> AllIps => _map.Keys;
        public Task<byte[]> ScreenshotAsync(string ip) => _map[ip].ScreenshotAsync();
        public Task HomeAsync(string ip) => _map[ip].SendAsync("home");
        public Task WifiAsync(string ip) => _map[ip].SendAsync("wifi");
        public Task SettingsAsync(string ip) => _map[ip].SendAsync("settings");
    }
}
