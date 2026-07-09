using Dragon.Controller.Database.Services;
using Dragon.Controller.DeviceControl.OTG.Loop;
using System.Collections.Concurrent;
using System.Diagnostics;


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

            // 1. Lấy thông tin tươi từ scanner
            var dev = AoaDeviceScanner.FindByDeviceID(deviceId,
                autoScanMissing: true,
                refreshExisting: forceRefresh,
                withRelations: true);

            if (dev == null || string.IsNullOrEmpty(dev.InstanceId))
                return null;

            // 2. Tạo hoặc lấy session hiện có
            var session = _sessions.GetOrAdd(dev.DeviceId, id => new AoaDeviceSession(dev));

            // 3. Update thông tin mới nhất
            session.Device.UpdateFromScan(dev);

            // 4. Mở nếu chưa ready
            if (!session.IsReady)
            {
                var ok = await session.Open();
                if (!ok)
                {
                    _sessions.TryRemove(dev.DeviceId, out _);
                    session.Dispose();
                    return null;
                }
            }

            // ===== THÊM: Cập nhật size từ nhiều nguồn =====
            _ = Task.Run(async () =>
            {
                // Ưu tiên 1: Thử update từ ADB
                bool updatedFromADB = await session.TryUpdateSizeFromADB();

                // Ưu tiên 2: Nếu không có ADB, thử từ Phone database
                if (!updatedFromADB)
                {
                    var phone = PhoneRepository.FindOneByDeviceID(deviceId);
                    if (phone != null && phone.PhysicalWidth > 0 && phone.PhysicalHeight > 0)
                    {
                        session.UpdateSizeFromPhone(phone.PhysicalWidth, phone.PhysicalHeight);
                    }
                }
            });

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



        // Trong AoaDeviceManager, thêm method này:
        public async Task<AoaDeviceSession?> StartWithAoaLoopAsync(string deviceId)
        {
            var session = await StartByDeviceIdAsync(deviceId);
            if (session == null) return null;

            var phone = PhoneRepository.FindOneByDeviceID(deviceId);
            if (phone == null) return session;

            // Lấy capture (có thể null)
            var capture = AppCaptureManager.Instance.GetByDeviceId(deviceId);
            bool hasCapture = capture != null;

            // Tìm loop phù hợp với trạng thái capture
            var loop = await AoaLoopMatcher.FindBestMatchAsync(phone, hasCapture);
            if (loop == null) return session;

            _ = Task.Run(async () =>
            {
                try
                {
                    using var cts = new CancellationTokenSource(TimeSpan.FromMinutes(5));
                    await AoaLoopRunner.RunAsync(loop, session.ctrl, capture, cts.Token);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[AoaLoop] Error for {deviceId}: {ex.Message}");
                }
            });

            return session;
        }
    }
}