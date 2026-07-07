using AdvancedSharpAdbClient;
using AdvancedSharpAdbClient.Models;
using Dragon.Controller.Database.Services;
using Dragon.Controller.DeviceControl;
using Dragon.Controller.DeviceControl.HATX;
using Dragon.Controller.DeviceControl.OTG;
using Dragon.Controller.GlobalControl.Helper;
using Dragon.Controller.TaskDeviceManager.Core;
using Dragon.Controller.TaskDeviceManager.Model.Mouse;
using Dragon.Controller.TaskDeviceManager.Model.Vision;
using Dragon.Database.Models;
using LibUsbDotNet;
using LibUsbDotNet.LibUsb;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Dragon.Controller.TaskDeviceManager.Infrastructure
{
    public class DragonPhoneManager
    {
        private static readonly Lazy<DragonPhoneManager> _inst = new(() => new());
        public static DragonPhoneManager Instance => _inst.Value;

        private readonly PhoneFarmHost _host = new();
        private readonly ConcurrentDictionary<string, PhoneSession> _sessions = new();
        public bool CheckBeforeRun(Phone phone, DLoop root)
        {
            root.Hydrate();

            // 1. Xác định trạng thái vật lý
            var aoa = AoaDeviceScanner.FindByDeviceID(phone.DeviceID);
            bool isUsbPlugged = aoa != null && aoa.IsPoweredOn;


            // kiểm tra adb có serial IP không
            var adb = new AdbClient();
            var dev = adb.GetDevices().FirstOrDefault(d => d.Serial == phone.DeviceID || d.Serial == phone.Ipv4);
            bool hasWifiSerial = dev != null && dev.Serial.Contains(":");

            // 2. Áp ConnectionType
            string chosenConnection = null;

            switch (root.connectionType)
            {
                case ConnectionType.UsbOnly:
                    if (!isUsbPlugged) return false;
                    chosenConnection = "USB";
                    break;

                case ConnectionType.WifiOnly:
                    if (hasWifiSerial) chosenConnection = "WIFI";
                    else if (isUsbPlugged && aoa != null)
                    {

                        if (dev != null && string.IsNullOrEmpty(phone.Ipv4) && aoa.Service?.Equals("WinUSB", StringComparison.OrdinalIgnoreCase) == false)
                        {
                            var checkOff = adb.GetDevices().Any(c => c.Serial == phone.Serial && c.State == DeviceState.Offline);
                            ChangeTCPIP_USB_To_Wifi(phone, checkOff);
                            chosenConnection = "WIFI";

                        }
                        else if (dev == null && aoa.Service?.Equals("WinUSB", StringComparison.OrdinalIgnoreCase) == true)
                        {
                            var exit = AppCaptureManager.Instance.GetIpByDeviceId(phone.DeviceID);
                            if (exit)
                            {
                                // Xây dựng đc hàm bật adb debugging lên rồi sử dụng 2 dòng code dưới để chuyển sang wifi mode
                                var checkOff = adb.GetDevices().Any(c => c.Serial == phone.Serial && c.State == DeviceState.Offline);
                                ChangeTCPIP_USB_To_Wifi(phone, checkOff);
                            }
                            
                        }
                        return false;
                    }
                    else return false; // 2b và 3 như bạn liệt kê
                    break;

                case ConnectionType.Auto:
                    if (isUsbPlugged) chosenConnection = "USB";
                    else if (hasWifiSerial) chosenConnection = "WIFI";
                    else return false;
                    break;
            }

            // 3. Gom nhu cầu từ cây DLoop
            bool needHDI = false, needOTG = false, needATX = false, needScrcpy = false;
            bool needVisionScreen = false;

            void Walk(DLoop n)
            {
                if (!n.Enabled) return;
                switch (n.ControlMode)
                {
                    case ControlMode.HDI: needHDI = true; break;
                    case ControlMode.OTG: needOTG = true; break;
                    case ControlMode.ATX: needATX = true; break;
                    case ControlMode.Scrcpy: needScrcpy = true; break;
                }
                if (n.Type == NodeType.VisionScanArgs)
                {
                    var v = n.GetArgs<VisionScanArgs>();
                    if (v.VisionMode != VisionMode.ByAtxNode) needVisionScreen = true;
                }
                foreach (var c in n.Children) Walk(c);
            }
            Walk(root);

            // 4. Kiểm tra theo nhóm USB/WiFi
            if (!isUsbPlugged)
            {
                // nhóm WiFi
                if (needHDI)
                {
                    Logger.Notify(phone.DeviceID, "HDI cần USB, hiện đang WiFi", Logger.Icon.Error);
                    return false;
                }
                if (needOTG)
                {
                    Logger.Notify(phone.DeviceID, "OTG cần cáp AOA, hiện đang WiFi", Logger.Icon.Error);
                    return false;
                }
                if (needVisionScreen && !hasWifiSerial)
                {
                    Logger.Notify(phone.DeviceID, "Vision cần ảnh, WiFi chưa kết nối", Logger.Icon.Error);
                    return false;
                }
                // ADB, ADBEvent, ATX(WATX), Scrcpy, ACC → ok
            }
            else
            {
                // nhóm USB — cho phép tất cả, có thể tự chuyển PhoneMode
                if (needOTG && (aoa == null || !aoa.IsPoweredOn))
                {
                    Logger.Notify(phone.DeviceID, "OTG yêu cầu AOA PowerOn", Logger.Icon.Error);
                    return false;
                }
                // không chặn gì thêm
            }

            return true;
        }
        // 1. Tạo session DỰA VÀO root.ControlMode
        private async Task<PhoneSession?> BuildSessionAsync(Phone phone, DLoop root)
        {
            var adb = new AdbClient();
            DeviceData? dd;

            // ưu tiên wifi nếu phone đang ở wifi mode
            if (phone.IsWifimode())
                dd = adb.GetDevices().FirstOrDefault(d => d.Serial.Contains(phone.Ipv4));
            else
                dd = adb.GetDevices().FirstOrDefault(d => d.Serial == phone.DeviceID);

            if (dd == null)
            {
                Logger.Notify(phone.DeviceID, "Không tìm thấy DeviceData", Logger.Icon.Error);
                return null;
            }

            var session = new PhoneSession(phone, adb, dd);

            // khởi tạo đúng 1 loại input theo root
            switch (root.ControlMode)
            {
                case ControlMode.ATX:
                    Logger.Notify(phone.DeviceID, "Khởi tạo ATX...", Logger.Icon.Information);
                    session.Atx = await AtxDevice.CreateAsync(dd.Serial, false, session.Cts.Token);
                    break;

                case ControlMode.Scrcpy:
                    // giả sử bạn có factory scrcpy
                    var scrcpy = await ScrcpyServiceFactory.CreateAsync(dd.Serial);
                    session.Input = scrcpy.Input;
                    session.Screen = scrcpy.Screen;
                    break;

                case ControlMode.HDI:
                    session.InputUhid = await UhidServiceFactory.CreateAsync(dd.Serial);
                    break;

                case ControlMode.OTG:
                    session.Aoa = await AoaDeviceManager.Instance.StartByDeviceIdAsync(phone.DeviceID);
                    if (session.Aoa == null)
                    {
                        Logger.Notify(phone.DeviceID, "Không khởi tạo được AOA", Logger.Icon.Error);
                        return null;
                    }
                    break;

                case ControlMode.ACC:
                    // chưa hỗ trợ
                    Logger.Notify(phone.DeviceID, "ACC chưa hỗ trợ", Logger.Icon.Error);
                    return null;

                    // ADB và ADBEvent không cần khởi tạo thêm
            }

            // nếu là wifi thì chuẩn bị ScreenShotApp cho Vision
            if (phone.IsWifimode())
                AppCaptureManager.Instance.Add(phone);

            return session;
        }

        public async Task StartAsync(string deviceId, DLoop dLoop)
        {
            Stop(deviceId);
            await Task.Delay(100);

            var phone = PhoneRepository.FindOneByDeviceID(deviceId);
            if (phone == null) { Logger.Notify(deviceId, "Không tìm thấy Phone", Logger.Icon.Error); return; }
            if (phone.IsRunning) { Logger.Notify(deviceId, "Phone đang chạy task khác", Logger.Icon.Error); return; }

            // --- DÙNG THẲNG MODE TỪ DLOOP GỐC ---
            if (dLoop.ControlMode == default) dLoop.ControlMode = ControlMode.ADB;
            if (dLoop.VisionMode == default) dLoop.VisionMode = VisionMode.ByAtxNode;

            // đẩy xuống toàn bộ cây
            dLoop.Hydrate();

            var session = await BuildSessionAsync(phone, dLoop);
            if (session == null) return;

            phone.IsRunning = true;
            PhoneRepository.Update(phone);
            _sessions[deviceId] = session;

            _ = Task.Run(async () =>
            {
                try
                {
                    var result = await _host.ExecuteAsync(dLoop, session);
                    Logger.Notify(deviceId, $"Task kết thúc: {result.Status}", Logger.Icon.Information);
                }
                finally
                {
                    Stop(deviceId);
                }
            });
        }

        public void Stop(string deviceId)
        {
            if (_sessions.TryRemove(deviceId, out var session))
            {
                try { session.Cts.Cancel(); } catch { }
                try { AppCaptureManager.Instance.Remove(session.Phone); } catch { }
                try { session.Aoa?.Dispose(); } catch { }

                var phone = session.Phone;
                phone.IsRunning = false;

                var dev = AdbClient.Instance.GetDevices().FirstOrDefault(d => d.Serial == deviceId || d.Serial == phone.Ipv4);
                if (dev != null) phone.DeviceState = dev.State;
                phone.IsOnline(AdbClient.Instance.GetDevices());

                PhoneRepository.Update(phone);
                Logger.Notify(deviceId, "Đã dừng và reset IsRunning", Logger.Icon.Warning);
            }
        }

        public async Task RestartAsync(string deviceId, DLoop task)
        {
            Stop(deviceId);
            await Task.Delay(500);
            await StartAsync(deviceId, task);
        }

        public bool IsRunning(string deviceId) => _sessions.ContainsKey(deviceId);



        public bool ChangeTCPIP_USB_To_Wifi(Phone phone, bool offline = false)
        {
            //var restartUSBMode = CMD.ExecuteAdb($"-s {phone.DeviceID} usb");
            //Debug.WriteLine($"Restart USB Mode : {restartUSBMode}");
            //Thread.Sleep(500);
            if (offline)
            {
                var disChek = Disconnect(phone);
                Debug.WriteLine($"Offline DisCheck:{disChek}");
                Thread.Sleep(700);
            }
            bool ChkTcpip = false;
            bool ChkRec = false;
            var tcpipCheck = Tcpip(phone);
            Debug.WriteLine($"TcpipCheck : {tcpipCheck}");
            if (!string.IsNullOrEmpty(tcpipCheck) && tcpipCheck.Contains($"restarting in TCP mode port"))
            {
                ChkTcpip = true;
            }
            Thread.Sleep(900);
            var reconnectCheck = Reconnect(phone);
            Debug.WriteLine($"reconnectCheck: {reconnectCheck}");
            if (!string.IsNullOrEmpty(tcpipCheck) && tcpipCheck.Contains($"connected"))
            {
                ChkRec = true;
            }
            var deviceDatas = AdbClient.Instance.GetDevices();
            if (deviceDatas.Any(c => c.Serial.Contains(phone.Ipv4) && c.State == DeviceState.Online) && ChkRec && ChkTcpip)
            {
                return true;
            }

            return false;
        }
        private string? Tcpip(Phone phone)
        {
            return CMD.ExecuteAdb($"-s {phone.DeviceID} tcpip 5555");
        }
        private string? Reconnect(Phone phone)
        {
            return CMD.ExecuteAdb($"adb connect {phone.Ipv4}:5555");
        }
        private string? Disconnect(Phone phone)
        {
            return CMD.ExecuteAdb($"disconnect {phone.Ipv4}:5555");
        }

    }
}