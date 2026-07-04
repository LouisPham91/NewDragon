using System.Collections.Concurrent;


namespace Dragon.Controller.DeviceControl.OTG
{
    public sealed class AoaDeviceManager
    {
        private static readonly Lazy<AoaDeviceManager> _lazy = new(() => new AoaDeviceManager());
        public static AoaDeviceManager Instance => _lazy.Value;
        private AoaDeviceManager() { }

        private readonly ConcurrentDictionary<string, AoaDeviceSession> _sessions = new(StringComparer.OrdinalIgnoreCase);
        public IReadOnlyCollection<AoaDeviceSession> All => _sessions.Values.ToArray();
        public IReadOnlyCollection<string> AllIds => _sessions.Keys.ToArray();

        // ---- API MỚI: start theo DeviceId (serial) ----
        public async Task<AoaDeviceSession?> StartByDeviceIdAsync(string deviceId, bool forceRefresh = true)
        {
            if (string.IsNullOrWhiteSpace(deviceId)) return null;

            // 1. Lấy thông tin tươi từ scanner (chính là FindByDeviceID bạn đã có)
            var dev = AoaDeviceScanner.FindByDeviceID(deviceId,
                autoScanMissing: true,
                refreshExisting: forceRefresh,
                withRelations: true);

            if (dev == null || string.IsNullOrEmpty(dev.InstanceId))
                return null; // không cắm hoặc không phải Android

            // 2. Tạo hoặc lấy session hiện có
            var session = _sessions.GetOrAdd(dev.DeviceId, id => new AoaDeviceSession(dev));

            // 3. Update thông tin mới nhất (tránh mất UsbDevice đang mở)
            session.Device.UpdateFromScan(dev);

            // 4. Mở nếu chưa ready
            if (!session.IsReady)
            {
                var ok = await session.Open();
                if (!ok)
                {
                    // mở lỗi -> dọn để lần sau thử lại sạch
                    _sessions.TryRemove(dev.DeviceId, out _);
                    session.Dispose();
                    return null;
                }
            }
            return session;
        }

        // ---- stop riêng ----
        public bool StopByDeviceId(string deviceId)
        {
            if (_sessions.TryRemove(deviceId, out var s))
            {
                s.Dispose();
                return true;
            }
            return false;
        }

        // ---- restart nhanh ----
        public async Task<AoaDeviceSession?> RestartByDeviceIdAsync(string deviceId)
        {
            StopByDeviceId(deviceId);
            await Task.Delay(300); // chờ Windows release handle
            return await StartByDeviceIdAsync(deviceId, true);
        }

        // ---- giữ lại ScanAndSync nhưng KHÔNG tự mở ----
        public async Task ScanAndSync(bool autoOpen = false)
        {
            var fresh = AoaDeviceScanner.Scan(true);
            var freshIds = fresh.Select(d => d.DeviceId).ToHashSet(StringComparer.OrdinalIgnoreCase);

            foreach (var dev in fresh)
            {
                var session = _sessions.GetOrAdd(dev.DeviceId, id => new AoaDeviceSession(dev));
                session.Device.UpdateFromScan(dev);
                if (autoOpen && !session.IsReady)
                    await session.Open();
            }

            // xóa device đã rút
            foreach (var id in _sessions.Keys.Except(freshIds).ToList())
            {
                if (_sessions.TryRemove(id, out var s)) s.Dispose();
            }
        }

        public void Shutdown()
        {
            foreach (var s in _sessions.Values) s.Dispose();
            _sessions.Clear();
        }

        public AoaDeviceSession? Get(string deviceId) =>
            _sessions.TryGetValue(deviceId, out var s) ? s : null;
    }
}