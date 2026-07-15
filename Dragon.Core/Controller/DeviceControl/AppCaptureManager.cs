using AdvancedSharpAdbClient;
using AdvancedSharpAdbClient.Models;
using AdvancedSharpAdbClient.Receivers;
using Dragon.Controller.Database.Services;
using Dragon.Controller.DeviceControl.HATX.Core.Model;
using Dragon.Database.Models;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;
using System.Xml;


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

        /// <summary>
        /// Đảm bảo AppCapture được khởi tạo và app Dragons đã được cài đặt + chạy trên thiết bị
        /// </summary>
        public async Task<AppCapture?> InitAppCaptureAsync(string deviceId, CancellationToken ct = default)
        {
            // 1. Kiểm tra xem đã có AppCapture chưa
            if (_byDevice.TryGetValue(deviceId, out var existing))
            {
                try
                {
                    using var testCts = new CancellationTokenSource(3000);
                    var testTask = existing.ScreenshotAsync(3000);
                    if (await Task.WhenAny(testTask, Task.Delay(3000, testCts.Token)) == testTask)
                    {
                        return existing;
                    }
                }
                catch { }

                existing.Dispose();
                _byDevice.TryRemove(deviceId, out _);
            }

            // 2. Lấy thông tin phone
            var phone = PhoneRepository.FindOneByDeviceID(deviceId);
            if (phone == null) return null;

            // 3. Kiểm tra IP WiFi
            if (string.IsNullOrWhiteSpace(phone.Ipv4) || !phone.IsPingWifi)
            {
                Debug.WriteLine($"[AppCapture] Device {deviceId} không có WiFi hoặc không ping được");
                return null;
            }

            // 4. Kết nối ADB
            var adbClient = new AdbClient();
            var devices = adbClient.GetDevices();
            var deviceData = devices.FirstOrDefault(d =>
                d.Serial == deviceId || d.Serial == phone.Ipv4 + ":5555");

            if (deviceData == null)
            {
                Debug.WriteLine($"[AppCapture] Device {deviceId} not found in ADB");
                return null;
            }

            string packageName = "abc.go.dragons";

            // 5. Kiểm tra & cài đặt app
            bool isInstalled = await IsAppInstalledAsync(deviceData, adbClient, packageName, ct);
            if (!isInstalled)
            {
                string apkPath = Path.Combine(AppContext.BaseDirectory, "Extension", "Apk", "Dragons.apk");
                if (!File.Exists(apkPath))
                {
                    Debug.WriteLine($"[AppCapture] APK not found: {apkPath}");
                    return null;
                }

                Debug.WriteLine($"[AppCapture] Installing {packageName}...");
                await InstallApkAsync(deviceData, adbClient, apkPath, ct);
                await Task.Delay(2000, ct);
            }

            // 6. Start app nếu chưa chạy
            bool isRunning = await IsAppRunningAsync(deviceData, adbClient, packageName, ct);
            if (!isRunning)
            {
                Debug.WriteLine($"[AppCapture] Starting {packageName}...");
                await StartAppAsync(deviceData, adbClient, packageName, ct);
                await Task.Delay(2000, ct); // Chờ app mở

                // 7. Xử lý màn hình welcome/permission (nếu có)
                await HandleWelcomeScreenAsync(deviceData, adbClient, ct);
            }

      
            // 8. Tạo AppCapture mới
            var capture = new AppCapture(deviceId, phone.Ipv4, 8888);

            try
            {
                using var testCts = new CancellationTokenSource(5000);
                var bit = await capture.ScreenshotBitmapAsync(3000);
                if (bit != null)
                {
                    _byDevice[deviceId] = capture;
                    Debug.WriteLine($"[AppCapture] Connected to {deviceId} at {phone.Ipv4}:8888");
                    return capture;
                }
            }
            catch { }

            capture.Dispose();
            Debug.WriteLine($"[AppCapture] Failed to connect to {deviceId} at {phone.Ipv4}:8888");
            return null;
        }
        /// <summary>
        /// Xử lý màn hình welcome/permission của app Dragons bằng UI Automator dump
        /// Flow: 
        ///   1. Tìm checkbox "Không hiển thị lại" → click
        ///   2. Tìm nút "Bắt đầu ngay" → click
        /// </summary>
        private async Task<bool> HandleWelcomeScreenAsync(DeviceData device, AdbClient adb, CancellationToken ct)
        {
            Debug.WriteLine("[AppCapture] Checking welcome screen...");

            int maxRetries = 10;
            for (int i = 0; i < maxRetries; i++)
            {
                if (ct.IsCancellationRequested) return false;

                try
                {
                    // 1. Dump UI
                    var receiver = new ConsoleOutputReceiver();
                    await adb.ExecuteRemoteCommandAsync("uiautomator dump /sdcard/window_dump.xml", device, receiver, ct);

                    receiver = new ConsoleOutputReceiver();
                    await adb.ExecuteRemoteCommandAsync("cat /sdcard/window_dump.xml", device, receiver, ct);
                    string xml = receiver.ToString();

                    if (string.IsNullOrWhiteSpace(xml))
                    {
                        Debug.WriteLine("[AppCapture] Empty XML, retrying...");
                        await Task.Delay(500, ct);
                        continue;
                    }

                    // 2. Parse XML thành List<NodeObj>
                    var doc = new XmlDocument();
                    doc.LoadXml(xml);

                    var allNodes = new List<NodeObj>();
                    FlattenNodes(doc.DocumentElement!, allNodes);

                    if (allNodes.Count == 0)
                    {
                        Debug.WriteLine("[AppCapture] No nodes found, retrying...");
                        await Task.Delay(500, ct);
                        continue;
                    }

                    if (!allNodes.Any(c => c.PackageName.Contains("abc.go.dragons", StringComparison.OrdinalIgnoreCase)))
                    {
                        return false;
                    }

                    // 3. Tìm các elements cần tương tác
                    var checkboxNode = FindNodeByText(allNodes, "Không hiển thị lại", checkable: true);
                    var startNowNode = FindNodeByText(allNodes, "Bắt đầu ngay", clickable: true);
                    var startServerNode = FindNodeByText(allNodes, "Bắt đầu Server", clickable: true)
                                          ?? FindNodeByText(allNodes, "Bắt đầu server", clickable: true);

                    bool foundAny = false;

                    // 4. Click checkbox "Không hiển thị lại" nếu tìm thấy
                    if (checkboxNode != null && checkboxNode.Checkable)
                    {
                        var center = checkboxNode.Bound.CenterPos;
                        Debug.WriteLine($"[AppCapture] Clicking checkbox: '{checkboxNode.Text}' at ({center.X},{center.Y})");
                        await TapAsync(adb, device, center.X, center.Y, ct);
                        await Task.Delay(300, ct);
                        foundAny = true;
                    }

                    // 5. Click nút "Bắt đầu ngay" nếu tìm thấy
                    if (startNowNode != null && startNowNode.Clickable)
                    {
                        var center = startNowNode.Bound.CenterPos;
                        Debug.WriteLine($"[AppCapture] Clicking start button: '{startNowNode.Text}' at ({center.X},{center.Y})");
                        await TapAsync(adb, device, center.X, center.Y, ct);
                        await Task.Delay(500, ct);
                        foundAny = true;
                    }

                    // 6. Click nút "Bắt đầu Server" nếu tìm thấy (fallback)
                    if (startServerNode != null && startServerNode.Clickable)
                    {
                        var center = startServerNode.Bound.CenterPos;
                        Debug.WriteLine($"[AppCapture] Clicking start server button: '{startServerNode.Text}' at ({center.X},{center.Y})");
                        await TapAsync(adb, device, center.X, center.Y, ct);
                        await Task.Delay(500, ct);
                        return true; // Thành công!
                    }

                    // 7. Nếu không tìm thấy element nào
                    if (!foundAny)
                    {
                        Debug.WriteLine("[AppCapture] No welcome elements found - screen might already be passed");
                        return false;
                    }
                }
                catch (XmlException ex)
                {
                    Debug.WriteLine($"[AppCapture] XML parse error: {ex.Message}");
                    await Task.Delay(500, ct);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[AppCapture] Welcome screen error: {ex.Message}");
                    return false;
                }

                await Task.Delay(500, ct);
            }

            Debug.WriteLine("[AppCapture] Welcome screen handling timeout");
            return false;
        }

        // ===== HELPER METHODS =====

        /// <summary>
        /// Flatten XML tree thành List<NodeObj>
        /// </summary>
        private void FlattenNodes(XmlNode node, List<NodeObj> list)
        {
            if (node == null) return;

            var obj = NodeObj.Create(node);
            if (obj != null) list.Add(obj);

            if (node.ChildNodes != null)
            {
                foreach (XmlNode child in node.ChildNodes)
                {
                    FlattenNodes(child, list);
                }
            }
        }

        /// <summary>
        /// Tìm node theo text (hỗ trợ tìm trong text, content-desc và hint)
        /// </summary>
        private NodeObj? FindNodeByText(
            List<NodeObj> nodes,
            string searchText,
            bool? clickable = null,
            bool? checkable = null,
            bool? enabled = null)
        {
            return nodes.FirstOrDefault(n =>
            {
                // Match text (case-insensitive, contains)
                bool textMatch =
                    n.Text.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                    n.ContentDescription.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                    n.Hint.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                    n.ResourceName.Contains(searchText, StringComparison.OrdinalIgnoreCase);

                if (!textMatch) return false;

                // Filter by properties nếu được chỉ định
                if (clickable.HasValue && n.Clickable != clickable.Value) return false;
                if (checkable.HasValue && n.Checkable != checkable.Value) return false;
                if (enabled.HasValue && n.Enabled != enabled.Value) return false;

                // Node phải có bounds hợp lệ
                if (n.Bound == null || n.Bound == NodeObj.Bounds.Empty) return false;
                if (n.Bound.Width <= 0 || n.Bound.Height <= 0) return false;

                return true;
            });
        }

        /// <summary>
        /// Thực hiện tap vào tọa độ
        /// </summary>
        private async Task TapAsync(AdbClient adb, DeviceData device, int x, int y, CancellationToken ct)
        {
            var receiver = new ConsoleOutputReceiver();
            await adb.ExecuteRemoteCommandAsync($"input tap {x} {y}", device, receiver, ct);
        }

       
        private static async Task<bool> IsAppInstalledAsync(DeviceData device, AdbClient adb, string packageName, CancellationToken ct)
        {
            var receiver = new ConsoleOutputReceiver();
            await adb.ExecuteRemoteCommandAsync($"pm list packages {packageName}", device, receiver, ct);
            return receiver.ToString().Contains(packageName);
        }

        private static async Task InstallApkAsync(DeviceData device, AdbClient adb, string apkPath, CancellationToken ct)
        {
            using var syncService = new SyncService(new AdbSocket(new IPEndPoint(IPAddress.Loopback, AdbClient.AdbServerPort)), device);
            using var stream = File.OpenRead(apkPath);

            // Push APK to temp
            await syncService.PushAsync(stream, "/data/local/tmp/dragons.apk", UnixFileStatus.UserRead | UnixFileStatus.UserWrite, DateTime.Now);

            // Install
            var receiver = new ConsoleOutputReceiver();
            await adb.ExecuteRemoteCommandAsync("pm install -r /data/local/tmp/dragons.apk", device, receiver, ct);

            // Cleanup
            await adb.ExecuteRemoteCommandAsync("rm /data/local/tmp/dragons.apk", device, new ConsoleOutputReceiver(), ct);
        }

        private static async Task<bool> IsAppRunningAsync(DeviceData device, AdbClient adb, string packageName, CancellationToken ct)
        {
            var receiver = new ConsoleOutputReceiver();
            await adb.ExecuteRemoteCommandAsync($"pidof {packageName}", device, receiver, ct);
            return !string.IsNullOrWhiteSpace(receiver.ToString());
        }

        private static async Task StartAppAsync(DeviceData device, AdbClient adb, string packageName, CancellationToken ct)
        {
            var receiver = new ConsoleOutputReceiver();
            // Thử mở app qua monkey
            await adb.ExecuteRemoteCommandAsync(
                $"monkey -p {packageName} -c android.intent.category.LAUNCHER 1",
                device, receiver, ct);
        }

        // MỚI: lấy thẳng AppCapture
        public async Task<AppCapture?> GetByDeviceId(string deviceId)
        {
            if (_byDevice.TryGetValue(deviceId, out var cap))
            {
                return cap;
            }
            else
            {
                var capt = await InitAppCaptureAsync(deviceId, CancellationToken.None);
                return capt;
            }
        }

        // Add - đảm bảo 1 device = 1 AppCapture
        public bool Add(Phone phone, int port = 8888)
        {
            if (phone == null || !phone.IsWifimode() || string.IsNullOrWhiteSpace(phone.Ipv4) || !phone.IsPingWifi)
                return false;

            var cap = new AppCapture(phone.DeviceID, phone.Ipv4, port);
            _byDevice.AddOrUpdate(phone.DeviceID, cap, (_, old) => { old.Dispose(); return cap; });
            return true;
        }
        public bool Add(string ip, int port = 8888)
        {
            var phone = PhoneRepository.FindOneByIp(ip);
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
