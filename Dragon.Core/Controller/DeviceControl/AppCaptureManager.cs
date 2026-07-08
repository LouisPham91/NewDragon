using Dragon.Database.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Dragon.Controller.DeviceControl
{
    public sealed class AppCaptureManager
    {
        // --- singleton ---
        private AppCaptureManager() { }
        private static readonly Lazy<AppCaptureManager> _lazy = new(() => new AppCaptureManager());
        public static AppCaptureManager Instance => _lazy.Value;

        // --- chỉ 1 map: deviceId -> AppCapture ---
        private readonly ConcurrentDictionary<string, AppCapture> _byDevice = new(StringComparer.OrdinalIgnoreCase);

        // MỚI: lấy thẳng AppCapture
        public AppCapture? GetByDeviceId(string deviceId)
        {
            _byDevice.TryGetValue(deviceId, out var cap);
            return cap;
        }

        public bool TryGet(string deviceId, out AppCapture? capture)
            => _byDevice.TryGetValue(deviceId, out capture);

        // Add - đảm bảo 1 device = 1 AppCapture
        public bool Add(Phone phone, int port = 8888)
        {
            if (phone == null || !phone.IsWifimode() || string.IsNullOrWhiteSpace(phone.Ipv4) || !phone.IsPingWifi)
                return false;

            var cap = new AppCapture(phone.DeviceID, phone.Ipv4, port);
            _byDevice.AddOrUpdate(phone.DeviceID, cap, (_, old) => { old.Dispose(); return cap; });
            return true;
        }

        // API dùng deviceId trực tiếp
        public Task<byte[]> ScreenshotAsync(string deviceId) => _byDevice[deviceId].ScreenshotAsync();
        public Task<byte[]> ScreenshotAsync(Phone phone) => ScreenshotAsync(phone.DeviceID);

        public Task HomeAsync(string deviceId) => _byDevice[deviceId].SendAsync("home");
        public Task SettingsAsync(string deviceId) => _byDevice[deviceId].SendAsync("settings");
        public Task WifiAsync(string deviceId) => _byDevice[deviceId].SendAsync("wifi");
        public Task DeeplinkAsync(string deviceId, string url) => _byDevice[deviceId].DeeplinkAsync(url);

        public bool Remove(string deviceId)
        {
            if (_byDevice.TryRemove(deviceId, out var cap))
            {
                cap.Dispose();
                return true;
            }
            return false;
        }
        public bool Remove(Phone phone) => Remove(phone.DeviceID);

        public void Clear()
        {
            foreach (var kv in _byDevice) kv.Value.Dispose();
            _byDevice.Clear();
        }

        public IEnumerable<string> AllDeviceIds => _byDevice.Keys;
    }
}
