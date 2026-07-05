//using AdvancedSharpAdbClient;
//using Dragon.Controller.GlobalControl.Helper;
//using Dragon.Controller.DeviceControl.OTG;
//using System.Diagnostics;

//namespace Dragon.UI.DesignView.FormUI
//{
//    public partial class FormTestDGMode : Form
//    {
//        // Từ điển quản lý AoaHidManager cho từng thiết bị (key = Serial)

//        private AoaDevice? _selectedDevice;
//        private List<AoaDevice> _allDevices = new();
//        AdbClient AdbClient = new AdbClient();
//        private string adbPath = Path.Combine(AppContext.BaseDirectory, "Extension", "ScrcpyNet", "adb.exe");

//        public FormTestDGMode()
//        {
//            var (ok, log) = BCDEditHelper.EnableTestSigning();
//            Debug.WriteLine(log);
//            InitializeComponent();
//        }

//        private async void FormTestDGMode_Load(object sender, EventArgs e)
//        {
//            if (AdbServer.Instance.GetStatus().IsRunning == false)
//                await AdbServer.Instance.StartServerAsync(adbPath, false, CancellationToken.None);
//          //  AoaDeviceHelper.LoadAllAeoDevices(true);

//            Log("=== DRAGON AOA TEST STARTED ===");
//            RefreshDeviceList();
//        }

//        private void FormTestDGMode_FormClosing(object sender, FormClosingEventArgs e)
//        {
//            Log("=== CLEANUP ===");
//            // Giải phóng tất cả manager
//            foreach (var kvp in AoaDeviceHelper._hidManagers)
//            {
//                try { kvp.Value.Dispose(); } catch { }
//            }
//            AoaDeviceHelper._hidManagers.Clear();
//            AoaDeviceHelper.Cleanup();
//        }

//        private void Log(string msg)
//        {
//            Debug.WriteLine(msg);
//            if (rtbLog.InvokeRequired)
//            {
//                rtbLog.Invoke(() => Log(msg));
//                return;
//            }
//            string timestamp = DateTime.Now.ToString("HH:mm:ss");
//            rtbLog.AppendText($"[{timestamp}] {msg}\n");
//            rtbLog.ScrollToCaret();
//        }

//        private void BtnRefresh_Click(object? sender, EventArgs e)
//        {
//            RefreshDeviceList();
//        }

//        private void RefreshDeviceList()
//        {
//            var sw = Stopwatch.StartNew();
//            Log("Đang quét thiết bị USB...");
//            _allDevices = AoaDeviceHelper.GetAllDevices();
//            lstDevices.Items.Clear();
//            foreach (var dev in _allDevices)
//            {
//                string icon = dev.IsPoweredOn ? "🟢" : "🔴";
//                lstDevices.Items.Add($"{icon} [{dev.InstanceId}] {dev.Manufacturer} {dev.DriverName}");
//            }
//            var countOffline = _allDevices.Count(d => !d.IsPoweredOn);
//            var countOnline = _allDevices.Count(d => d.IsPoweredOn);
//            sw.Stop();
//            Log($"✅ Tìm thấy: {_allDevices.Count} thiết bị | 🟢 Bật: {countOnline} | 🔴 Offline : {countOffline} => Time {sw.ElapsedMilliseconds} ms");
//        }

//        private void BtnSelectFirstDevice_Click(object sender, EventArgs e)
//        {
//            var poweredOn = AoaDeviceHelper.GetStartedDevices();
//            if (poweredOn.Count > 0)
//            {
//                _selectedDevice = poweredOn[0];
//                UpdateStatus();
//                Log($"✅ Đã chọn: {_selectedDevice}");
//            }
//            else Log("❌ Không có thiết bị nào đang bật!");
//        }

//        private void BtnSelectBySerial_Click(object sender, EventArgs e)
//        {
//            string serial = txtSerial.Text.Trim();
//            if (string.IsNullOrEmpty(serial)) { Log("⚠️ Vui lòng nhập serial!"); return; }
//            _selectedDevice = AoaDeviceHelper.FindBySerial(serial);
//            UpdateStatus();
//            if (_selectedDevice != null)
//            {
//                Log($"✅ Đã chọn: {_selectedDevice}");
//            }
//            else
//            {
//                Log($"❌ Không tìm thấy: {serial}");
//            }
//        }

//        private void LstDevices_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            int index = lstDevices.SelectedIndex;
//            if (index >= 0 && index < _allDevices.Count)
//            {
//                _selectedDevice = _allDevices[index];
//                txtSerial.Text = _selectedDevice.DeviceId;
//                UpdateStatus();
//                Log($"▶ Đã chọn: {_selectedDevice}");
//            }
//        }

//        private void UpdateStatus()
//        {
//            if (_selectedDevice != null)
//            {
//                lblStatus.Text = $"Đã chọn: [{_selectedDevice.DeviceId}] {_selectedDevice.DriverName} | {_selectedDevice.Service}";
//                lblStatus.ForeColor = _selectedDevice.IsPoweredOn ? Color.Green : Color.OrangeRed;
//            }
//        }

//        private AoaHidManager? GetOrCreateHidManager()
//        {
//            if (_selectedDevice == null)
//            {
//                Log("❌ Chưa chọn thiết bị!");
//                return null;
//            }

//            string serial = _selectedDevice.DeviceId;
//            if (AoaDeviceHelper._hidManagers.TryGetValue(serial, out var existingManager))
//            {
//                return existingManager;
//            }

//            // Tạo mới manager cho thiết bị
//            var newManager = new AoaHidManager(_selectedDevice);
//            if (!newManager.Initialize(registerMouse: true, registerKeyboard: true))
//            {
//                Log("❌ Không thể khởi tạo AOA HID!");
//                newManager.Dispose();
//                return null;
//            }

//            if (AoaDeviceHelper._hidManagers.TryAdd(serial, newManager))
//            {
//                Log("✅ AOA HID đã sẵn sàng!");
//                return newManager;
//            }
//            else
//            {
//                newManager.Dispose();
//                return AoaDeviceHelper._hidManagers.GetOrAdd(serial, (key) => newManager); // thử lại
//            }
//        }

//        private void DisposeCurrentHidManager()
//        {
//            if (_selectedDevice == null) return;
//            string serial = _selectedDevice.DeviceId;
//            if (AoaDeviceHelper._hidManagers.TryRemove(serial, out var manager))
//            {
//                manager.Dispose();
//                Log($"✅ Đã đóng kết nối HID cho {serial}");
//            }
//        }
//        private async void BtnTestMouse_Click(object sender, EventArgs e)
//        {
//            if (!CheckDevice()) return;
//            var hid = GetOrCreateHidManager();
//            if (hid == null) return;

//            Log($"\n=== 🖱 TEST MOUSE trên {_selectedDevice!.DeviceId} ===");
//            try
//            {
//                // Hàm helper để click và delay có log
//                async Task ClickAndDelay(int x, int y, int delayMs)
//                {
//                    Log($"→ ClickAt({x}, {y})");
//                    hid.ClickAt(x, y);
//                    Log($"⏳ Delay {delayMs}ms...");
//                    await Task.Delay(delayMs);
//                    Log($"✓ Done delay {delayMs}ms");
//                }

//                await ClickAndDelay(100, 100, 300);
//                await ClickAndDelay(700, 400, 300);
//                await ClickAndDelay(700, 800, 300);
//                await ClickAndDelay(700, 1000, 300);
//                await ClickAndDelay(700, 1200, 300);
//                await ClickAndDelay(700, 1500, 300);

//                Log("✅ TEST MOUSE HOÀN THÀNH!");
//            }
//            catch (Exception ex) { Log($"❌ Lỗi: {ex.Message}"); }
//        }


//        private async void BtnTestKeyboard_Click(object sender, EventArgs e)
//        {
//            if (!CheckDevice()) return;
//            var hid = GetOrCreateHidManager();
//            if (hid == null) return;

//            Log($"\n=== ⌨ TEST KEYBOARD trên {_selectedDevice!.DeviceId} ===");
//            try
//            {
//                Log("→ Gõ 'Hello'...");
//                hid.KeyboardSendText("Hello");
//                await Task.Delay(300);
//                Log("→ Enter...");
//                hid.KeyboardEnter();
//                await Task.Delay(300);
//                Log("→ Gõ 'AOA Test'...");
//                hid.KeyboardSendText("AOA Test");
//                await Task.Delay(300);
//                Log("✅ TEST KEYBOARD HOÀN THÀNH!");
//            }
//            catch (Exception ex) { Log($"❌ Lỗi: {ex.Message}"); }
//        }

//        private async void BtnTestFull_Click(object sender, EventArgs e)
//        {
//            Log("=== 🚀 FULL TEST TẤT CẢ THIẾT BỊ ONLINE (CHẠY SONG SONG) ===");

//            // Lấy tất cả thiết bị đang bật
//            var onlineDevices = AoaDeviceHelper.GetStartedDevices();
//            Log($"Tìm thấy {onlineDevices.Count} thiết bị online");

//            // Tạo một danh sách các Task, mỗi Task xử lý một thiết bị
//            var tasks = new List<Task>();
//            foreach (var device in onlineDevices)
//            {
//                // Bắt đầu một tác vụ nền cho mỗi thiết bị
//                var task = Task.Run(async () =>
//                {
//                    Log($"[{device.DeviceId}] ▶ Bắt đầu test...");

//                    try
//                    {
//                        // Lấy hoặc tạo AoaHidManager cho thiết bị này
//                        var hid = AoaDeviceHelper.GetOrCreateHidManagerForDevice(device);
//                        if (hid == null)
//                        {
//                            Log($"[{device.DeviceId}] ❌ Không thể khởi tạo HID");
//                            return;
//                        }

//                        // === MOUSE TEST ===
//                        Log($"[{device.DeviceId}]   🖱 MOUSE TEST");
//                        async Task ClickAndDelay(int x, int y, int delayMs)
//                        {
//                            Log($"[{device.DeviceId}]     → ClickAt({x}, {y})");
//                            hid.ClickAt(x, y);
//                            Log($"[{device.DeviceId}]     ⏳ Delay {delayMs}ms...");
//                            await Task.Delay(delayMs);
//                        }

//                        await ClickAndDelay(200, 300, 200);
//                        await ClickAndDelay(400, 500, 200);
//                        await ClickAndDelay(600, 800, 200);
//                        Log($"[{device.DeviceId}]   ✅ MOUSE OK");

//                        // === KEYBOARD TEST ===
//                        Log($"[{device.DeviceId}]   ⌨ KEYBOARD TEST");
//                        Log($"[{device.DeviceId}]     → SendText 'Test'...");
//                        hid.KeyboardSendText("Test");
//                        await Task.Delay(200);
//                        Log($"[{device.DeviceId}]     → Enter...");
//                        hid.KeyboardEnter();
//                        await Task.Delay(200);
//                        Log($"[{device.DeviceId}]   ✅ KEYBOARD OK");

//                        Log($"[{device.DeviceId}] ✅ Hoàn thành test!");
//                    }
//                    catch (Exception ex)
//                    {
//                        Log($"[{device.DeviceId}] ❌ Lỗi: {ex.Message}");
//                    }
//                });

//                tasks.Add(task);
//            }

//            // Chờ tất cả các tác vụ hoàn thành
//            await Task.WhenAll(tasks);

//            Log("\n✅ FULL TEST TẤT CẢ THIẾT BỊ HOÀN THÀNH!");
//        }

//        private void BtnDispose_Click(object sender, EventArgs e)
//        {
//            DisposeCurrentHidManager();
//        }

//        private bool CheckDevice()
//        {
//            if (_selectedDevice == null) { Log("❌ Chưa chọn thiết bị!"); return false; }
//            if (!_selectedDevice.IsPoweredOn) { Log("❌ Thiết bị đang tắt/flash mode!"); return false; }
//            return true;
//        }

//        private void buttonRunADB_Click(object sender, EventArgs e)
//        {
//            if (!AdbServer.Instance.GetStatus().IsRunning)
//                AdbServer.Instance.StartServer(adbPath, restartServerIfNewer: true);
//            Log($"✅ Đã tạo sever adb");
//            var list = AdbClient.GetDevices();
//            foreach (var device in list)
//            {
//                Log($"🔹 {device.Serial} | {device.State}");
//            }
//        }

//        private void buttonStopADB_Click(object sender, EventArgs e)
//        {
//            if (AdbServer.Instance.GetStatus().IsRunning)
//                AdbServer.Instance.StopServer();

//            Log($"✅ Đã stop sever adb");
//        }

//        private void buttonSericeADB_Click(object sender, EventArgs e)
//        {
//            if (_selectedDevice != null)
//            {
//                AoaDeviceHelper.CloseDevice(_selectedDevice);
//            }

//        }
//    }
//}