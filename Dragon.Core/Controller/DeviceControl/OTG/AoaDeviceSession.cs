using AdvancedSharpAdbClient;
using AdvancedSharpAdbClient.Models;
using Dragon.Controller.Database;
using Dragon.Controller.Database.Models;
using Dragon.Controller.Database.Services;
using Dragon.Controller.DeviceControl.HATX;
using Dragon.Controller.DeviceControl.HATX.Core.Model;
using Dragon.Controller.DeviceControl.Orc;
using Dragon.Controller.DeviceControl.OTG.Loop;
using LibUsbDotNet.LibUsb;
using LibUsbDotNet.Main;
using Microsoft.Win32;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace Dragon.Controller.DeviceControl.OTG
{
    public sealed class AoaDeviceSession : IDisposable
    {
        public AoaDevice Device { get; }
        public AoaController ctrl { get; }
        private IUsbDevice? _usb;
        private bool _disposed;

        //public event Action<AoaDeviceSession>? Connected;
        //public event Action<AoaDeviceSession, Exception>? Error;
        public bool IsReady => _usb != null && _usb.IsOpen && !_disposed;
        private const string TMP = "/data/local/tmp/";
        private readonly Regex XmlFromDumpRegex = new Regex(@"<\?xml.*</hierarchy>", RegexOptions.Singleline | RegexOptions.Compiled);
        private readonly Regex PhysicalRegex = new(@"Physical size: (\d+)x(\d+)", RegexOptions.Compiled);
        private readonly Regex OverrideRegex = new(@"Override size: (\d+)x(\d+)", RegexOptions.Compiled);
        public AoaDeviceSession(AoaDevice device)
        {
            Device = device ?? throw new ArgumentNullException(nameof(device));
            ctrl = new AoaController(device);
        }

        // ==== MỞ THIẾT BỊ - DÙNG FIND THAY VÌ LIST ====
        public static IUsbDevice? OpenByDeviceID(ushort vid, ushort pid, string serial)
        {
            Debug.WriteLine($"[OpenByDeviceID] VID:{vid:X4} PID:{pid:X4} Serial:{serial}");
            using var ctx = new UsbContext();
            var finder = new UsbDeviceFinder { Vid = vid, Pid = pid, SerialNumber = serial };
            var dev = ctx.Find(finder);
            if (dev == null) return null;
            try
            {
                if (!dev.IsOpen)
                    dev.Open();

                return dev;
            }
            catch
            {
                dev.Dispose();
                return null;
            }
        }

        public async Task<bool> Open()
        {
            try
            {

                // 2. Cài WinUSB nếu chưa
                if (!await EnsureWinUsb())
                    return false;

                // 3. Mở bằng Find
                _usb = OpenByDeviceID(Device.Vid, Device.Pid, Device.DeviceId);
                if (_usb == null) return false;

                Device.UsbDevice = _usb;
                ctrl.Initialize(_usb, true, true);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static readonly DeviceBackupRepository _repo = new();

        public static async Task<bool> ForceInstallWinUsbInternal(string instanceId)
        {
            if (string.IsNullOrEmpty(instanceId)) return false;
            string regPath = $@"SYSTEM\CurrentControlSet\Enum\{instanceId}";
            try
            {
                using var key = Registry.LocalMachine.OpenSubKey(regPath, true);
                if (key == null) return false;

                string? svc = key.GetValue("Service") as string;
                string[]? guids = key.GetValue("DeviceInterfaceGUIDs") as string[];

                var existing = await _repo.FindOneAsync(instanceId);
                if (existing == null)
                {
                    await _repo.SaveAsync(new DeviceBackup
                    {
                        DeviceId = instanceId,
                        Service = svc,
                        DeviceInterfaceGuids = guids
                    });
                }

                key.SetValue("Service", "WinUSB", RegistryValueKind.String);
                if (guids == null || guids.Length == 0)
                {
                    key.SetValue("DeviceInterfaceGUIDs",
                        new[] { "{DEE824EF-729B-4A0E-9C14-B7117D33A817}" },
                        RegistryValueKind.MultiString);
                }
                await RestartDevice(instanceId);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ForceInstallWinUsb] {ex.Message}");
                return false;
            }
        }



        public async Task<bool> RestoreOriginalDriverInternal(bool enableDevelop = true, AppCapture? cap = null)
        {
            var backup = await _repo.FindOneAsync(Device.DeviceId);
            if (backup == null) return false;

            // ===== PHẦN MỚI: Dùng AoaLoop để enable USB debugging =====
            if (enableDevelop)
            {
                // Lấy phone info từ database
                var phone = PhoneRepository.FindOneByDeviceID(Device.DeviceId);

                if (phone != null && cap != null && ctrl != null)
                {
                    // Tìm AoaLoop phù hợp với phone này
                    var loop = await AoaLoopMatcher.FindBestMatchAsync(phone);

                    if (loop != null)
                    {
                        try
                        {
                            Debug.WriteLine($"[{Device.DeviceId}] Running AoaLoop to enable USB debugging...");

                            using var cts = new CancellationTokenSource(TimeSpan.FromMinutes(3));
                            await AoaLoopRunner.RunAsync(loop, ctrl, cap, cts.Token);

                            Debug.WriteLine($"[{Device.DeviceId}] AoaLoop completed");
                        }
                        catch (OperationCanceledException)
                        {
                            Debug.WriteLine($"[{Device.DeviceId}] AoaLoop timeout");
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"[{Device.DeviceId}] AoaLoop error: {ex.Message}");
                            // Không return false, vẫn tiếp tục restore driver
                        }
                    }
                    else
                    {
                        Debug.WriteLine($"[{Device.DeviceId}] No AoaLoop found for this phone model");
                    }
                }
            }

            // ===== PHẦN GIỮ NGUYÊN: Restore driver =====
            try
            {
                using var key = Registry.LocalMachine.OpenSubKey(
                    $@"SYSTEM\CurrentControlSet\Enum\{Device.InstanceId}", true);

                if (key == null) return false;

                if (backup.Service != null)
                    key.SetValue("Service", backup.Service);
                else
                    key.DeleteValue("Service", false);

                if (backup.DeviceInterfaceGuids?.Length > 0)
                    key.SetValue("DeviceInterfaceGUIDs", backup.DeviceInterfaceGuids, RegistryValueKind.MultiString);
                else
                    key.DeleteValue("DeviceInterfaceGUIDs", false);

                await _repo.RemoveAsync(Device.DeviceId);
                await RestartDevice(Device.InstanceId);

                Debug.WriteLine($"[{Device.DeviceId}] Driver restored to: {backup.Service ?? "default"}");
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[{Device.DeviceId}] Restore driver error: {ex.Message}");
                return false;
            }
        }

        public static async Task RestartDevice(string instanceId)
        {
            RunPnputil($"/disable-device \"{instanceId}\"");
            await Task.Delay(800);
            RunPnputil($"/enable-device \"{instanceId}\"");
            await Task.Delay(800);
        }

        private static void RunPnputil(string args)
        {
            try
            {
                var psi = new ProcessStartInfo("pnputil", args)
                {
                    CreateNoWindow = true,
                    UseShellExecute = false
                };
                Process.Start(psi)?.WaitForExit(10000);
            }
            catch { }
        }

        public void Dispose()
        {
            if (_disposed) return;
            try { _usb?.Close(); _usb?.Dispose(); } catch { }
            ctrl?.Dispose();
            _disposed = true;
        }

        (int w, int h) GetPhysicalSize(DeviceData device, IAdbClient adb)
        {
            // 1. ưu tiên mCurrentDisplayRect – Samsung/Xiaomi/Oppo đều có
            var out1 = Shell(device, adb, "wm size");
            // ví dụ: mCurrentDisplayRect=Rect(0, 0 - 1080, 2400)
            var m = OverrideRegex.Match(out1);
            if (m.Success) return (int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value));

            m = PhysicalRegex.Match(out1);
            if (m.Success) return (int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value));
            return (0, 0);
        }
        private async Task<bool> EnsureWinUsb()
        {
            if (Device == null || string.IsNullOrEmpty(Device.InstanceId)) return false;
            if (Device.Vid == 0 || Device.Pid == 0) return false;
            if (Device.DeviceId == null) return false;
            if (Device.Status?.Equals("Started", StringComparison.OrdinalIgnoreCase) == false) return false;

            // 1. Đã ở chế độ WinUSB rồi thì thôi
            if (Device.Service?.Equals("WinUSB", StringComparison.OrdinalIgnoreCase) == true)
            {
                Debug.WriteLine($"[EnsureWinUsb] {Device.DeviceId} already WinUSB, skip");
                return true;
            }

            var adbClient = new AdbClient();

            // 2. Kiểm tra ADB còn dính không
            var devices = adbClient.GetDevices();
            var deviceData = devices.FirstOrDefault(x => x.Serial.Equals(Device.DeviceId, StringComparison.OrdinalIgnoreCase));
            if (deviceData != null)
            {
                bool IsScreenOff(string powerDump) =>
                         powerDump.Contains("mWakefulness=Dozing") ||
                         powerDump.Contains("mWakefulness=Asleep") ||
                         powerDump.Contains("Display Power: state=OFF");

                bool IsLocked(string policyDump) =>
                    policyDump.Contains("mIsShowing=true") ||
                    policyDump.Contains("showing=true") && policyDump.Contains("mInputRestricted=true");

                var powerDump = Shell(deviceData, adbClient, "dumpsys power");
                var policyDump = Shell(deviceData, adbClient, "dumpsys window policy");

                var size = GetPhysicalSize(deviceData, adbClient);
                int w = size.w;
                int h = size.h;

                if (IsScreenOff(powerDump))
                {
                    Shell(deviceData, adbClient, "input keyevent 26"); // power
                    await Task.Delay(700);

                    Shell(deviceData, adbClient, $"input tap {w / 2},{h / 2} "); // power
                    await Task.Delay(300);

                    Shell(deviceData, adbClient, $"input swipe {w / 2} {(int)(h * 0.86)} {w / 2} {(int)(h * 0.2)} 1000");
                    await Task.Delay(300);

                    Shell(deviceData, adbClient, $"input swipe {w / 2} {(int)(h * 0.8)} {w / 2} {(int)(h * 0.2)} 1000");
                    await Task.Delay(200);
                }

                if (IsLocked(policyDump))
                {
                    var key = await KeypadStringRepository.FindOneAsync(Device.DeviceId);
                    if (key == null) return false;

                    string keycode = key.PassCoce;

                    foreach (char c in keycode)
                    {
                        int digit = c - '0'; // '1' -> 1
                        var (x, y) = key.GetClick(digit, w, h);

                        Shell(deviceData, adbClient, $"input tap {x} {y}");
                        await Task.Delay(200); // chờ animation phím, 150-200ms là đẹp
                    }
                    var (xOk, yOK) = key.GetOk(w, h);
                    Shell(deviceData, adbClient, $"input tap {xOk} {yOK}");
                    await Task.Delay(2000);
                }


                var IsOpen = OpenDeveloperOptions(deviceData, adbClient);
                if (!IsOpen)
                {
                    Debug.WriteLine("Không thể mở deverlop options (nhà phát triển)");
                    return false;
                }
                var list = DumpUiNodes(deviceData, adbClient);
                if (list == null) return false;
                var checks = list.Where(c => c.ResourceName == "com.android.settings:id/switch_widget").ToList();
                var enableButton = list.FirstOrDefault(node => node.ResourceName == "com.android.settings:id/switch_widget"
                && (node.Text.Contains("phát triển", StringComparison.OrdinalIgnoreCase) || node.Text.Contains("Developer options", StringComparison.OrdinalIgnoreCase)));

                if (enableButton != null)
                {
                    var center = enableButton.Bound.CenterPos;
                    Shell(deviceData, adbClient, $"input tap {center.X} {center.Y}");
                }
                else
                {

                    Debug.WriteLine("Tạo hàm tắt Usb debugging trước ");
                    return false;
                }
            }

            // 3. Giờ mới an toàn cài WinUSB
            bool ok = await ForceInstallWinUsbInternal(Device.InstanceId);
            if (ok)
            {
                Device.Service = "WinUSB";
                await Task.Delay(1500);
            }
            return ok;
        }

        public List<NodeObj>? DumpUiNodes(DeviceData deviceData, AdbClient adbClient)
        {
            try
            {
                var serial = deviceData.Serial;

                //CMD.ExecuteAdbV2($"adb -s {serial} shell pkill -f minitouch; pkill -f minicap; pkill -f atx-agent", 3000);
                //CMD.ExecuteAdbV2($"adb -s {serial} shell pkill -f tonghopbaitap; pkill -f ttap2; pkill -f bincapTap1", 3000);
                //CMD.ExecuteAdbV2($"adb -s {serial} shell {TMP}tonghopbaitap server --stop", 3000);
                //CMD.ExecuteAdbV2($"adb -s {serial} shell {TMP}atx-agent server --stop", 3000);

                Shell(deviceData, adbClient, "pkill -f minitouch; pkill -f minicap; pkill -f atx-agent");
                Shell(deviceData, adbClient, "pkill -f tonghopbaitap; pkill -f ttap2; pkill -f bincapTap1");
                Shell(deviceData, adbClient, $"{TMP}tonghopbaitap server --stop");
                Shell(deviceData, adbClient, $"{TMP}atx-agent server --stop");

                //CMD.ExecuteAdbV2($"adb -s {serial} shell pkill -f uiautomator", 2000); // dọ         
                //CMD.ExecuteAdbV2($"adb -s {serial} shell input keyevent 224", 1000); // wake

                Shell(deviceData, adbClient, "pkill -f uiautomator"); // dọn
                Shell(deviceData, adbClient, "input keyevent 224"); // wake

                //var dump = CMD.ExecuteAdbV2($"adb -s {serial} shell uiautomator dump", 30000); // chạy mới
                //string raw = CMD.ExecuteAdbV2($"adb -s {serial} shell cat /sdcard/window_dump.xml", 5000);
                //CMD.ExecuteAdbV2($"adb -s {serial} shell rm /sdcard/window_dump.xml", 2000);

                var dump = Shell(deviceData, adbClient, "uiautomator dump"); // chạy mới
                                                                             //Task.Delay(800); // chờ ghi xong                         
                string raw = Shell(deviceData, adbClient, "cat /sdcard/window_dump.xml"); // đọc mới   
                Shell(deviceData, adbClient, "rm /sdcard/window_dump.xml"); // xóa mới
                if (string.IsNullOrEmpty(raw))
                    return null;

                // 2. tách phần XML sạch
                var match = XmlFromDumpRegex.Match(raw);
                if (!match.Success)
                {
                    Debug.WriteLine("Không tìm thấy XML trong output");
                    return null;
                }

                string cleanXml = match.Value.Trim();
                var doc = new XmlDocument();
                doc.LoadXml(cleanXml);

                // 3. đi qua toàn bộ node
                var list = new List<NodeObj>();

                void Walk(XmlNode n)
                {
                    var obj = NodeObj.Create(n);
                    if (obj != null) list.Add(obj);
                    foreach (XmlNode child in n.ChildNodes) Walk(child);
                }

                Walk(doc.DocumentElement!);
                return list;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"DumpUiNodes lỗi: {ex.Message}");
                return null;
            }
        }
        string Shell(DeviceData device, IAdbClient adb, string command)
        {
            var sb = new StringBuilder();
            try
            {
                // thêm 2>&1 để gộp stderr vào stdout, và dùng predicate để đọc từng dòng
                adb.ExecuteRemoteCommand(command + " 2>&1", device, line =>
                {
                    sb.AppendLine(line);
                    return true; // tiếp tục đọc
                });
                return sb.ToString().Trim();
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine($"[{device.Serial}] shell timeout: {command}");
                return string.Empty;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[{device.Serial}] shell error: {ex.Message}");
                return string.Empty;
            }
        }
        bool OpenDeveloperOptions(DeviceData device, IAdbClient adb)
        {
            // 1. Lấy SDK để chọn lệnh ưu tiên
            int sdk = 0;
            var sdkStr = Shell(device, adb, "getprop ro.build.version.sdk");
            int.TryParse(sdkStr, out sdk);

            // 2. Danh sách lệnh theo độ ưu tiên (mới -> cũ)
            var cmds = new List<string>();

            if (sdk >= 29) // Android 10+
            {
                cmds.Add("am start -n com.android.settings/.Settings$DevelopmentSettingsDashboardActivity");
            }
            if (sdk >= 26) // Android 8+
            {
                cmds.Add("am start -n com.android.settings/com.android.settings.DevelopmentSettings");
            }
            // lệnh gốc từ Android 3 tới nay, chạy được trên mọi máy
            cmds.Add("am start -n com.android.settings/.DevelopmentSettings");
            cmds.Add("am start -a android.settings.APPLICATION_DEVELOPMENT_SETTINGS");

            // 3. Thử từng lệnh
            foreach (var cmd in cmds)
            {
                var outp = Shell(device, adb, cmd);
                // thành công nếu có "Starting:" và không có "Error" / "not found"
                if (outp.Contains("Starting:") && !outp.ToLower().Contains("error") && !outp.Contains("not found"))
                {
                    Debug.WriteLine($"[{device.Serial}] OK với: {cmd}");
                    return true;
                }
            }
            return false;
        }

    }
}