using AdvancedSharpAdbClient;
using AdvancedSharpAdbClient.Models;
using AdvancedSharpAdbClient.Receivers;
using Dragon.ControlHelper.ScrcpyNet.DesignDevice;
using Dragon.ControlHelper.ScrcpyNet.Services;
using Dragon.Controller.Database.Services;
using Dragon.Controller.GlobalControl.Extensions;
using Dragon.Controller.GlobalControl.Helper;
using Dragon.Controller.GlobalControl.Property;
using Dragon.Database.Models;
using Dragon.DesignView.FormUI;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;

namespace Dragon.ControlHelper.UIController
{
    public class DeviceManager
    {

        #region ======================= Properties =======================

        public static volatile bool IsDrawSerialOnScreen = false;
        private static readonly Lazy<DeviceManager> instance = new(() => new DeviceManager { });
        public static DeviceManager Instance => instance.Value;
        public ConcurrentDictionary<string, PanelDevice> DeviceDict { get; } = new();
        //public MouseSelect? MouseSelect;
        private readonly object _lockPhoneAdd = new object();
        private readonly object _lockTag = new object();
        private int _lastTagNumber = -1; // để tránh load nhiều lần


        private static readonly Regex OverrideSizeRegex = new Regex(@"Override size: (\d+)x(\d+)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        private static readonly Regex PhysicalSizeRegex = new Regex(@"Physical size: (\d+)x(\d+)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        private static readonly Regex Ipv4Regex = new Regex(@"(\d+\.\d+\.\d+\.\d+)/", RegexOptions.CultureInvariant);

        #endregion



        #region =======================  Phone vs Device =======================

        public void UpdatePhoneOffline(List<Phone> listOffline)
        {
            foreach (var phoneOff in listOffline)
            {
                phoneOff.DeviceState = DeviceState.Offline;
                phoneOff.Message = "Device Offline";
                var checkbool = PhoneRepository.Update(phoneOff);
                Debug.WriteLine($"Update Phone Offline {phoneOff.DeviceID} : {checkbool}");
            }
        }
        private void InitLastPhoneTagNumber()
        {
            var listPhone = PhoneRepository.LoadAll();
            lock (_lockTag) { _lastTagNumber = listPhone?.Any() == true ? listPhone.Max(p => p.PhoneTagNumber) : 0; }
        }

        public (int, int) GetPhysicalSize(AdbClient _adb, DeviceData _device)
        {
            var output = new ConsoleOutputReceiver();
            _adb.ExecuteRemoteCommand("wm size", _device, output);
            var value = output.ToString();
            if (string.IsNullOrWhiteSpace(value)) return (0, 0);

            var m1 = OverrideSizeRegex.Match(value);
            if (m1.Success && int.TryParse(m1.Groups[1].Value, out int w1) && int.TryParse(m1.Groups[2].Value, out int h1))
                return (w1, h1);

            var m2 = PhysicalSizeRegex.Match(value);
            if (m2.Success && int.TryParse(m2.Groups[1].Value, out int w2) && int.TryParse(m2.Groups[2].Value, out int h2))
                return (w2, h2);

            return (0, 0);
        }

        public string? GetIpv4(DeviceData deviceData)
        {
            string ipv4 = string.Empty;
            try // Kiểm tra kết nối wifi và ipv4
            {

                string? receiver2 = CMD.ExecuteAdb($"adb -s {deviceData.Serial} shell ip addr show wlan0");
                if (!string.IsNullOrEmpty(receiver2))
                {
                    var match = Ipv4Regex.Match(receiver2);
                    if (match.Success)
                    {
                        return match.Groups[1].Value;
                    }
                }

            }
            catch (Exception) { }

            return null;
        }


        public async Task CreateNewPhoneOrUpdate(List<DeviceData> deviceList)
        {
            if (deviceList == null || deviceList.Count == 0) return;

            InitLastPhoneTagNumber();
            var tasks = new List<Task>();

            foreach (var deviceData in deviceList)
            {
                if (string.IsNullOrEmpty(deviceData.Serial) || string.IsNullOrEmpty(deviceData.Model) || string.IsNullOrEmpty(deviceData.Product))
                    continue;

                var dd = deviceData; // tránh closure
                tasks.Add(Task.Run(async () =>
                {
                    var adb = new AdbClient();
                    using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));
                    var ct = cts.Token;

                    async Task<string> Adb(string cmd)
                    {
                        try
                        {
                            var r = new ConsoleOutputReceiver();
                            await adb.ExecuteRemoteCommandAsync(cmd, dd, r, ct);
                            return r.ToString().Trim();
                        }
                        catch { return string.Empty; }
                    }

                    async Task<string> Shell(string cmd) => await CMD.ExecuteAdbAsync($"adb -s {dd.Serial} shell {cmd}");


                    // chạy song song
                    var serialTask = Adb("getprop ro.serialno");
                    var sizeTask = Task.Run(() => GetPhysicalSize(adb, dd));
                    var ipTask = Adb("ip addr show wlan0");
                    var uhidTask = Adb("test -e /dev/uhid && echo exists || echo notfound");
                    var sdkTask = Adb("getprop ro.build.version.sdk");
                    var procTask = Shell("cat /proc/version");
                    var cpuTask = Shell("cat /proc/cpuinfo");
                    var suTask = Adb("su -c id");
                    var magiskTask = Adb("pm list packages | grep magisk");
                    var ksuTask = Adb("pm list packages | grep kernelsu");

                    await Task.WhenAll(serialTask, sizeTask, ipTask, uhidTask, sdkTask, procTask, cpuTask);

                    string serialNo = await serialTask;
                    var (w, h) = await sizeTask;
                    string ipv4 = Ipv4Regex.Match(await ipTask).Groups[1].Value;
                    bool isUhdi = (await uhidTask).Contains("exists");
                    int sdk = int.TryParse(await sdkTask, out var v) ? v : 0;
                    bool isRoot = (await suTask).Contains("uid=0");
                    bool isMagisk = (await magiskTask).Contains("magisk");
                    bool isKsu = (await ksuTask).Contains("kernelsu");
                    bool isWifi = dd.Serial.Contains('.');

                    string procVer = await procTask;
                    string cpuInfo = await cpuTask;

                    // chỉ lock khi ghi DB
                    lock (_lockPhoneAdd)
                    {
                        if (string.IsNullOrEmpty(serialNo) || w <= 0 || h <= 0 || sdk <= 0)
                            return;

                        var phone = PhoneRepository.FindOneByDeviceID(serialNo);
                        bool isNew = phone == null;

                        if (isNew)
                        {
                            _lastTagNumber++;
                            phone = new Phone
                            {
                                DeviceID = serialNo,
                                PhoneTagNumber = _lastTagNumber,
                                IsRunning = false,
                                PhoneMode = isWifi ? PhoneMode.WIFI : PhoneMode.USB
                            };
                        }


                        phone!.DeviceState = dd.State;
                        phone.Serial = dd.Serial;
                        phone.Model = dd.Model;
                        phone.Product = dd.Product;
                        phone.Usb = dd.Usb;
                        phone.TransportId = dd.TransportId;
                        phone.Ipv4 = ipv4;
                        phone.IsUHDI = isUhdi;
                        phone.PhysicalWidth = w;
                        phone.PhysicalHeight = h;
                        phone.IsUseUSB = isWifi;
                        phone.AndroidVersion = AndroidVersionMapper.GetAndroidVersion(sdk);
                        phone.API = sdk;
                        phone.Message = "Device Online";
                        phone.ProcVersion = procVer;
                        phone.ProcCpuInfo = cpuInfo;
                        phone.IsPingWifi = !string.IsNullOrEmpty(ipv4);
                        phone.IsRooted = isRoot;
                        phone.IsMagisk = isMagisk;
                        phone.IsKernelSu = isKsu;

                        phone.HasValueCompute();

                        if (isNew) PhoneRepository.Add(phone);
                        else PhoneRepository.UpdateByDeviceID(phone.DeviceID, phone);
                    }
                }));
            }

            await Task.WhenAll(tasks);
        }

        public async Task LoadDeviceList()
        {
            if (!AdbServer.Instance.GetStatus().IsRunning) return;

            var allPhones = PhoneRepository.LoadAll();
            var allPhoneNotShows = allPhones.Where(c => DeviceDict.Keys.Contains(c.DeviceID) == false).ToList();
            var allPhoneShowButNotConnect = allPhones.Where(c => DeviceDict.TryGetValue(c.DeviceID, out var device) && device._PDcontroller != null && !device._PDcontroller.IsConnecting).ToList();

            var adbClient = new AdbClient();

            var deviceList = adbClient.GetDevices()?.ToList() ?? new List<DeviceData>();

            // 1. tìm WiFi nào chưa có trong adb
            var wifiPhones = allPhoneNotShows.Where(p => p.IsWifimode()).ToList();
            var missingWifi = wifiPhones.Where(p => !deviceList.Any(d => d.Serial.StartsWith(p.Ipv4 + ":"))).ToList();

            // 2. reconnect wifi nếu có
            if (missingWifi.Any())
            {
                await Task.WhenAll(missingWifi.Select(p => ReconnectAsync(p)));
                // refresh lại sau khi reconnect
                deviceList = adbClient.GetDevices()?.ToList() ?? new List<DeviceData>();
            }
            // 3. chưa có phone nào đc cắm vào devices
            if (!deviceList.Any())
            {
                UpdatePhoneOffline(allPhoneNotShows);
                MessageBox.Show("There Are No Phone Connect To PC");
                return;
            }

            // tách IP ra khỏi serial "ip:port"
            string GetIp(string serial) => serial?.Split(':')[0] ?? "";

            bool IsSame(Phone v, DeviceData c) => v.DeviceID == c.Serial || (!string.IsNullOrEmpty(v.Ipv4) && v.Ipv4 == GetIp(c.Serial));

            bool IsSameByMode(Phone v, DeviceData c)
            {
                if (v.IsWifimode())
                    return !string.IsNullOrEmpty(v.Ipv4) &&
                          !string.IsNullOrEmpty(c.Serial) &&
                           GetIp(c.Serial).Equals(v.Ipv4, StringComparison.Ordinal);

                if (v.IsUSBmode())
                    return !string.IsNullOrEmpty(c.Serial) &&
                           c.Serial.Equals(v.DeviceID, StringComparison.Ordinal) &&
                          !c.Serial.Contains(':'); // USB serial không có port

                return false;
            }


            // lấy device mới (không tồn tại trong allPhones) | get devicedata
            var newPhones = deviceList.Where(c => !allPhones.Any(v => IsSame(v, c))).ToList();

            var notShows = deviceList.Where(c => allPhoneNotShows.Any(v => IsSameByMode(v, c))).ToList();

            var notConnect = deviceList.Where(c => allPhoneShowButNotConnect.Any(v => IsSameByMode(v, c))).ToList();

            bool IsUsable(DeviceState s) => s == DeviceState.Online;

            var allTargetDevices = newPhones.Concat(notShows).Concat(notConnect).Where(d => IsUsable(d.State))
                                         .GroupBy(d => d.Serial) // loại trùng serial (vì 1 phone có thể xuất hiện ở 2 list)
                                         .Select(g => g.First()) // giữ cái đầu tiên gặp
                                         .ToList();

            // 8. tìm phone offline để cập nhật
            var onlineSerials = new HashSet<string>(allTargetDevices.Select(d => d.Serial));

            var offlinePhones = allPhones.Where(p =>
                !onlineSerials.Contains(p.DeviceID) &&
                !onlineSerials.Contains($"{p.Ipv4}:5555") &&
                !onlineSerials.Any(s => s.StartsWith(p.Ipv4 + ":"))
            ).ToList();

            if (offlinePhones.Any())
            {
                UpdatePhoneOffline(offlinePhones); // hoặc set status trong DB
            }

            await CreateNewPhoneOrUpdate(allTargetDevices);

        }
        public async Task TurnOn_All_WIFI_TCP_Service()
        {
            if (!AdbServer.Instance.GetStatus().IsRunning)
                return;
            var stopWatch = Stopwatch.StartNew();
            List<Task> tasks = new List<Task>();
            var adbClientServer = new AdbClient();
            var deviceList = adbClientServer.GetDevices();
            var listphone = PhoneRepository.FindManyByPhoneModes(PhoneMode.WIFI, PhoneMode.WHDI, PhoneMode.WATX);
            if (deviceList == null || deviceList.Count() == 0) return;

            foreach (var phone in listphone)
            {
                if (phone == null || string.IsNullOrEmpty(phone.Serial) || string.IsNullOrEmpty(phone.Ipv4)) continue;

                tasks.Add(Task.Run(async () =>
                {
                    DeviceData? deviceExit = deviceList.FirstOrDefault(d => d.Serial == $"{phone.Ipv4}:5555");
                    if (deviceExit != null)
                    {
                        if (deviceExit.State == DeviceState.Offline)
                        {
                            await DisconnectAsync(phone);
                            await Task.Delay(1000);
                            var log = await ReconnectAsync(phone);
                            Debug.WriteLine($"Reconnect Offline {phone.Ipv4}:5555 Result: {log}");
                        }
                    }
                    else
                    {
                        var log = await ReconnectAsync(phone);
                        Debug.WriteLine($"Reconnect {phone.Ipv4}:5555 Result: {log}");
                    }
                }));
            }
            await Task.WhenAll(tasks);
            stopWatch.Stop();
            Debug.WriteLine($"TurnOn_All_WIFI_TCP_Service completed in {stopWatch.ElapsedMilliseconds} ms");
        }

        private async Task<string?> ReconnectAsync(Phone phone)
        {
            return await CMD.ExecuteAdbAsync($"adb connect {phone.Ipv4}:5555");
        }
        private async Task<string?> DisconnectAsync(Phone phone)
        {
            return await CMD.ExecuteAdbAsync($"adb disconnect {phone.Ipv4}:5555");
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

        #endregion


        #region ======================= DevicePanel : Create, Update, Delete | Included Database |  Mode Changed =======================

        public void CreateDevicePanel(string deviceID, PanelDevice panel)
        {
            if (DeviceDict.TryAdd(deviceID, panel))
            {
                Debug.WriteLine($"Create new device : {panel.DeviceID}");
            }
            else
            {
                Debug.WriteLine($"Cant Create New. Device {panel.DeviceID} Exiting !");
            }
        }
        public void UpdateDevicePanel(string deviceID, PanelDevice panel)
        {
            DeviceDict.TryGetValue(deviceID, out var device);
            if (device == null)
            {
                Debug.WriteLine($"Device {deviceID} not found for update.");
                return;
            }
            else
            {
                DeviceDict.TryUpdate(deviceID, device, panel);
                Debug.WriteLine($"Update Device : {panel.DeviceID}");
            }
        }
        public void UpdateCreateManyDevicePanel(List<PanelDevice> panels)
        {
            foreach (var panel in panels)
            {
                if (DeviceDict.ContainsKey(panel.DeviceID))
                {
                    UpdateDevicePanel(panel.DeviceID, panel);
                }
                else
                {
                    CreateDevicePanel(panel.DeviceID, panel);
                }
            }
        }
        public void DeleteDevicePanel(string deviceID)
        {
            _ = DeviceDict.TryRemove(deviceID, out var panel);
            if (panel != null)
            {
                Debug.WriteLine($"Delete Device : {panel.DeviceID} Susscess!");
            }

        }
        public void DeleteManyDevicePanel(List<PanelDevice> panels)
        {
            foreach (var panel in panels)
            {
                if (DeviceDict.ContainsKey(panel.DeviceID))
                {
                    DeleteDevicePanel(panel.DeviceID);
                }
                else
                {
                    Debug.WriteLine($"Device {panel.DeviceID} not found for deletion.");
                }
            }
        }
        public bool IsAnyDevicePanel(string deviceID)
        {
            return DeviceDict.ContainsKey(deviceID);
        }
        public PanelDevice? FindOneDevicePanel(string deviceID)
        {
            _ = DeviceDict.TryGetValue(deviceID, out var panel);
            if (panel != null)
            {
                return panel;
            }
            else
            {
                Debug.WriteLine($"Device {deviceID} not found.");
                return null;
            }
        }
        public List<PanelDevice> LoadAllDevicePanel()
        {
            return DeviceDict.Values.ToList();
        }
        public List<PanelDevice> FindManyDevicePanel(List<Phone> phones)
        {
            List<PanelDevice> panelDevices = new List<PanelDevice>();
            if (phones == null || phones.Count == 0) return panelDevices;
            foreach (var phone in phones)
            {
                var panel = FindOneDevicePanel(phone.DeviceID);
                if (panel != null)
                {
                    panelDevices.Add(panel);
                }
            }
            return panelDevices;
        }
        public async Task StartPanelDevice(DeviceData deviceData, Phone phone, int maxSize, long bitrate, int fps, int minsize)
        {
            if (deviceData == null || phone == null) return;
            var virtualFlowHelper = FormManager.Instance.virtualFlowHelper;
            if (virtualFlowHelper == null) return;
            var _panel = virtualFlowHelper.flowLayoutPanel;
            if (_panel == null) return;


            var panel = new PanelDevice(phone.DeviceID);
            _panel.Controls.Add(panel);
            DeviceDict.TryAdd(phone.DeviceID, panel);

            var pdcontroler = new PDController(phone.DeviceID, deviceData, panel);

            await pdcontroler.StartAsync();
        }
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

        public async Task ShowDevicePanel(List<Phone>? selectedPhones = null)
        {
            selectedPhones = (selectedPhones == null) ? PhoneRepository.LoadAll() : selectedPhones;
            if (selectedPhones == null || !selectedPhones.Any()) return;

            // 1. Lấy dữ liệu ADB + chuẩn bị trạng thái offline trên background thread
            var deviceDataList = AdbClient.Instance.GetDevices();
            if (deviceDataList == null || !deviceDataList.Any()) return;

            var virtualFlowHelper = FormManager.Instance.virtualFlowHelper;
            if (virtualFlowHelper == null) return;

            var _panel = virtualFlowHelper.flowLayoutPanel;
            if (_panel == null) return;

            var allDevicePanels = _panel.Controls.OfType<PanelDevice>().ToArray();
            foreach (var item in allDevicePanels)
            {
                _panel.Controls.Remove(item);
            }

            var ordered = selectedPhones.OrderBy(p => p.PhoneTagNumber).ToList();

            // 3. Cấu hình chung
            int maxSize = GetSettings.GetMaxScreenSize();
            int minSize = GetSettings.GetMinScreenSize();
            long bitrate = GetSettings.GetBitrate();
            int fps = GetSettings.GetFps();

            var listPhoneWorks = new List<(Phone, DeviceData)>();

            foreach (var phone in ordered)
            {
                if (phone.IsRunning) continue;

                if (phone.IsUSBmode() && (phone.IsUHDIok() || phone.PhoneMode == PhoneMode.USB))
                {
                    var deviceDataExit = phone.GetDeviceData(deviceDataList);
                    if (deviceDataExit == null) continue;
                    listPhoneWorks.Add((phone, deviceDataExit));
                }

                if (phone.IsWifimode())
                {
                    bool CheckIp = (!string.IsNullOrEmpty(phone.Ipv4) || phone.Ipv4.Contains('.')) ? true : false;
                    if (CheckIp && phone.Serial.Contains('.'))
                    {
                        var deviceDataExit = phone.GetDeviceData(deviceDataList);
                        if (deviceDataExit == null) continue;
                        listPhoneWorks.Add((phone, deviceDataExit));
                    }
                    else if (CheckIp && !phone.Serial.Contains('.'))
                    {
                        var deviceDataExit = phone.GetDeviceData(deviceDataList);
                        if (deviceDataExit == null) continue;
                        var ipv4 = GetIpv4(deviceDataExit);
                        if (!string.IsNullOrEmpty(ipv4))
                        {
                            phone.Ipv4 = ipv4;
                            phone.Serial = $"{phone.Ipv4}:5555";
                            PhoneRepository.UpdateByDeviceID(phone.DeviceID, phone);
                            var checkOff = deviceDataList.Any(c => c.Serial == phone.Serial && c.State == DeviceState.Offline);
                            ChangeTCPIP_USB_To_Wifi(phone, checkOff);

                            deviceDataList = AdbClient.Instance.GetDevices();
                            var newDeviceExit = deviceDataList.FirstOrDefault(c => c.Serial == $"{phone.Ipv4}:5555" && c.State == DeviceState.Online);
                            if (newDeviceExit == null) continue;
                            listPhoneWorks.Add((phone, newDeviceExit));
                        }
                    }
                }
            }
            var adb = new AdbClient();
            foreach (var (phone, data) in listPhoneWorks)
            {
                bool exists = DeviceDict.ContainsKey(phone.DeviceID);
                if (!exists)
                {
                    _panel.BeginInvoke(async () =>
                    {
                        Debug.WriteLine($"Start Phone {phone.PhoneTagNumber}, Mode: {phone.PhoneMode}, DeviceID: {phone.DeviceID}, Serial: {phone.Serial}");
                        // chạy ở thread hiện tại, không ép lên UI
                        await StartPanelDevice(data, phone, maxSize, bitrate, fps, minSize).ConfigureAwait(false);

                    });
                }
                else
                {
                    _panel.BeginInvoke(async () =>
                    {
                        var panelDevice = allDevicePanels.FirstOrDefault(c => c.DeviceID == phone.DeviceID);
                        _panel.Controls.Add(panelDevice);
                        if (panelDevice?._PDcontroller?.IsConnecting == false && data.State == DeviceState.Online)
                        {
                            try
                            {
                                async Task<string> Adb(string cmd)
                                {
                                    try
                                    {
                                        var r = new ConsoleOutputReceiver();
                                        await adb.ExecuteRemoteCommandAsync(cmd, data, r, CancellationToken.None);
                                        return r.ToString().Trim();
                                    }
                                    catch { return string.Empty; }
                                }

                                var serialTask = await Adb("getprop ro.serialno");
                                if(serialTask == phone.DeviceID)
                                {
                                    await panelDevice._PDcontroller.StopAsync();
                                    await panelDevice._PDcontroller.StartAsync();
                                }    
                                
                            }
                            catch (Exception){}
                        }
                    });
                }
            }

        }


        public async Task ChangeDeviceMode(PhoneMode selectedMode, List<Phone> listPhoneSeleteds)
        {
            var maxSize = GetSettings.GetMaxScreenSize();
            var minSize = GetSettings.GetMinScreenSize();
            var bitrate = GetSettings.GetBitrate();
            var fps = GetSettings.GetFps();
            var deviceDatacheck = AdbClient.Instance.GetDevices();
            if (deviceDatacheck == null || deviceDatacheck.Count() <= 0) return;

            List<Task> tasks = new List<Task>();
            foreach (var phone in listPhoneSeleteds)
            {
                tasks.Add(Task.Run(() =>
                {
                    var deviceDatas = AdbClient.Instance.GetDevices();
                    bool isChangeMode = false;


                    if (selectedMode == PhoneMode.WIFI || selectedMode == PhoneMode.WHDI || selectedMode == PhoneMode.WATX)
                    {

                        bool IsModeWHDI = selectedMode == PhoneMode.WATX || selectedMode == PhoneMode.WHDI ? true : false;
                        isChangeMode = deviceDatas.Any(c => c.Serial == $"{phone.Ipv4}:5555" && c.State == DeviceState.Online);
                        if (!isChangeMode && !IsModeWHDI || IsModeWHDI && phone.IsUHDI && !isChangeMode)
                        {
                            if (deviceDatas.Any(c => c.Serial == phone.DeviceID && c.State == DeviceState.Online))
                            {
                                var dataUSB = deviceDatas.Single(c => c.Serial == phone.DeviceID && c.State == DeviceState.Online);
                                var ipv4 = GetIpv4(dataUSB);
                                if (!string.IsNullOrEmpty(ipv4))
                                {
                                    phone.Ipv4 = ipv4;
                                    PhoneRepository.UpdateByDeviceID(phone.DeviceID, phone);

                                    var devicePanel = FindOneDevicePanel(phone.DeviceID);
                                    if (devicePanel != null && FormManager.Instance.virtualFlowHelper != null)
                                    {
                                        FormManager.Instance.virtualFlowHelper.RunInvoke(new Action(() =>
                                        {
                                            FormManager.Instance.virtualFlowHelper.RemoveOnePanelDevice(devicePanel);
                                            //devicePanel.StopDevicePanel();
                                            DeleteDevicePanel(phone.DeviceID);

                                        }));
                                    }
                                    var checkoff = deviceDatas.Any(c => c.Serial == $"{phone.Ipv4}:5555" && c.State == DeviceState.Offline);
                                    ChangeTCPIP_USB_To_Wifi(phone, checkoff);

                                    deviceDatas = AdbClient.Instance.GetDevices();
                                    isChangeMode = deviceDatas.Any(c => c.Serial == $"{phone.Ipv4}:5555" && c.State == DeviceState.Online);
                                }
                            }

                        }
                        else if (IsModeWHDI && !phone.IsUHDI)
                        {
                            Debug.WriteLine($"This Device {phone.DeviceID} Do Not Support Mode WOTP or  WHDI");
                            isChangeMode = false;
                        }
                    }
                    else if (selectedMode == PhoneMode.USB || selectedMode == PhoneMode.UATX || selectedMode == PhoneMode.UHDI)
                    {
                        bool IsModeWHDI = selectedMode == PhoneMode.UATX || selectedMode == PhoneMode.UATX ? true : false;
                        if (IsModeWHDI && phone.IsUHDI)
                        {
                            isChangeMode = deviceDatas.Any(c => c.Serial == phone.DeviceID && c.State == DeviceState.Online);
                        }
                        else if (!IsModeWHDI)
                        {
                            isChangeMode = deviceDatas.Any(c => c.Serial == phone.DeviceID && c.State == DeviceState.Online);
                        }
                        else if (IsModeWHDI && !phone.IsUHDI)
                        {
                            Debug.WriteLine($"This Device {phone.DeviceID} Do Not Support Mode UOTP or UHDI ");
                            isChangeMode = false;
                        }

                    }
                    else if (selectedMode == PhoneMode.ACC)
                    {
                        isChangeMode = deviceDatas.Any(c => c.Serial == phone.DeviceID && c.State == DeviceState.Online || c.Serial == $"{phone.Ipv4}:5555" && c.State == DeviceState.Online);
                    }

                    Debug.WriteLine($"[ChangeDeviceMode] : {phone.DeviceID} IsChangeMode = {isChangeMode} ");
                    if (isChangeMode && phone.PhoneMode != selectedMode)
                    {
                        phone.PhoneMode = selectedMode;
                        PhoneRepository.UpdateByDeviceID(phone.DeviceID, phone);
                        deviceDatas = AdbClient.Instance.GetDevices();
                        // Change Khi DevicePanel đang tồn tại
                        var devicePanel = FindOneDevicePanel(phone.DeviceID);
                        if (devicePanel != null && FormManager.Instance.virtualFlowHelper != null)
                        {

                            FormManager.Instance.virtualFlowHelper.RunInvoke(() =>
                            {
                                if (selectedMode == PhoneMode.WIFI || selectedMode == PhoneMode.WHDI || selectedMode == PhoneMode.WATX)
                                {
                                    //devicePanel.Change_SizeMax_DeviceMode(phone, bitrate, fps, maxSize, minSize);
                                }
                                else if (selectedMode == PhoneMode.USB || selectedMode == PhoneMode.UATX || selectedMode == PhoneMode.UHDI)
                                {
                                    //devicePanel.Change_SizeMax_DeviceMode(phone, bitrate, fps, maxSize, minSize);
                                }
                                else if (selectedMode == PhoneMode.ACC)
                                {
                                    DeviceData deviceDataACC = deviceDatas.First(c => (c.Serial == phone.DeviceID && c.State == DeviceState.Online) || (c.Serial == $"{phone.Ipv4}:5555" && c.State == DeviceState.Online));

                                }
                            });


                        }
                        else
                        {
                            if (selectedMode == PhoneMode.WIFI || selectedMode == PhoneMode.WHDI || selectedMode == PhoneMode.WATX)
                            {

                                var chkAny = deviceDatas.Any(c => c.Serial == $"{phone.Ipv4}:5555" && c.State == DeviceState.Online);
                                if (chkAny)
                                {
                                    var deviceDataWifi = deviceDatas.FirstOrDefault(c => c.Serial == $"{phone.Ipv4}:5555" && c.State == DeviceState.Online);
                                    if (deviceDataWifi != null && FormManager.Instance.virtualFlowHelper != null)
                                    {
                                        FormManager.Instance.virtualFlowHelper.RunInvoke(async () =>
                                        {
                                            await StartPanelDevice(deviceDataWifi, phone, maxSize, bitrate, fps, minSize);
                                        });

                                    }
                                }


                            }
                            else if (selectedMode == PhoneMode.USB || selectedMode == PhoneMode.UATX || selectedMode == PhoneMode.UHDI)
                            {
                                var chkAny = deviceDatas.Any(c => c.Serial == phone.DeviceID && c.State == DeviceState.Online);
                                if (chkAny)
                                {
                                    var deviceDataUSB = deviceDatas.FirstOrDefault(c => c.Serial == phone.DeviceID && c.State == DeviceState.Online);
                                    if (deviceDataUSB != null && FormManager.Instance.virtualFlowHelper != null)
                                    {
                                        FormManager.Instance.virtualFlowHelper.RunInvoke(async () =>
                                        {
                                            await StartPanelDevice(deviceDataUSB, phone, maxSize, bitrate, fps, minSize);
                                        });
                                    }
                                }

                            }
                            else if (selectedMode == PhoneMode.ACC)
                            {

                                DeviceData deviceDataACC = deviceDatas.First(c => (c.Serial == phone.DeviceID && c.State == DeviceState.Online) || (c.Serial == $"{phone.Ipv4}:5555" && c.State == DeviceState.Online));

                            }
                        }
                    }

                }));
            }
            await Task.WhenAll(tasks);
        }

        public void ReNameBoxId_PanelDeviceUpdatePhone()
        {
            if (DeviceDict.Values == null || DeviceDict.Values.Count() == 0) return;

            foreach (var panelDevice in DeviceDict.Values)
            {
                if (panelDevice.phone != null)
                {
                    var phone = PhoneRepository.FindOneByDeviceID(panelDevice.phone.DeviceID);
                    if (phone != null)
                    {
                        panelDevice.phone.PhoneBoxId = phone.PhoneBoxId;
                    }
                }
            }
        }
        public int CountDevicePanel()
        {
            return DeviceDict.Count;
        }

        public void ClearAllDevicePanel()
        {
            foreach (var kv in DeviceDict)
            {
                kv.Value?.Dispose();
            }
            DeviceDict.Clear();
            Debug.WriteLine("Cleared all device panels.");
        }
        public async Task<bool> FastPingAsync(string ip, int timeoutMs = 500)
        {
            using var ping = new Ping();
            try
            {
                var reply = await ping.SendPingAsync(ip, timeoutMs);
                return reply.Status == IPStatus.Success;
            }
            catch
            {
                return false;
            }
        }

        public void ChangeAllMaxSizePanelDevice()
        {
            var listDevices = LoadAllDevicePanel();
            if (!listDevices.Any()) return;

            foreach (var devicePanel in listDevices)
            {
                if (devicePanel != null && devicePanel._PDcontroller?.ScrcpyService?.Connected == true)
                {
                    devicePanel.ResizeVideoMaxSize();
                }
            }

        }
        public void ChangeAllMinSizePanelDevice()
        {
            var listdatabasePhone = PhoneRepository.LoadAll();
            if (listdatabasePhone == null || listdatabasePhone.Count <= 0) return;
            foreach (var phone in listdatabasePhone)
            {
                var panelDevice = FindOneDevicePanel(phone.DeviceID);
                if (panelDevice != null)
                {
                    var newSize = GetSettings.getSize(phone);

                    if (panelDevice.Parent is FlowLayoutPanel)
                    {
                        var pad = panelDevice.BorderThickness * 2;
                        var size = new Size(newSize.minSize.Width + pad, newSize.minSize.Height + pad);
                        panelDevice.Size = new Size(size.Width, size.Height);
                    }
                    else
                    {
                        var pad = panelDevice.BorderThickness * 2;
                        var size = new Size(newSize.maxSize.Width + pad, newSize.maxSize.Height + pad);
                        panelDevice.Size = new Size(size.Width, size.Height);
                    }
                }
            }



        }
        public void ChangePanelDevice(PanelDevice panelDevice, FormAutoRecord formAutoRecord)
        {
            //panelUnderBack


            Size maxSize = GetSettings.ComputeVideoSize(panelDevice.phone.PhysicalWidth, panelDevice.phone.PhysicalHeight, GetSettings.GetMaxScreenSize());
            var newSize = GetSettings.ComputePanelSize(maxSize, formAutoRecord.panelPhone.Height);

            var pad = panelDevice.BorderThickness * 2;
            var size = new Size(newSize.Width + pad, newSize.Height + pad);

            panelDevice.Size = new Size(size.Width, size.Height);

            formAutoRecord.panelPhone.Size = newSize;
            formAutoRecord.panelUnderBack.Width = newSize.Width + 4;
            formAutoRecord.picboxScreenShot.Size = newSize;


        }

        public Bitmap? ScreenShotPanelDevice(string deviceID)
        {
            if (string.IsNullOrEmpty(deviceID)) return null;

            var panelDevice = FindOneDevicePanel(deviceID);
            if (panelDevice == null) return null;

            var bitmap = panelDevice?._PDcontroller?.ScreenShot();
            if (bitmap == null) return null;

            return bitmap;
        }

        #endregion

    }
}
