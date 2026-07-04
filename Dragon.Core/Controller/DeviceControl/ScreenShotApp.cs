using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Dragon.Controller.Controller.DeviceControl
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
    public sealed class ScreenShotApp
    {
        private ScreenShotApp() { } // chặn new từ ngoài

        private static readonly Lazy<ScreenShotApp> _lazy = new(() => new ScreenShotApp());
        public static ScreenShotApp Instance => _lazy.Value;

        private readonly ConcurrentDictionary<string, PhoneConnection> _map = new();
        public void Add(string ip, int port = 8888) => _map[ip] = new PhoneConnection(ip, port);
        public PhoneConnection Get(string ip) => _map[ip];
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
