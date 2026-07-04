//using AdvancedSharpAdbClient;
//using AdvancedSharpAdbClient.Models;
//using Dragon.Controller.Controller.DeviceControl;
//using Dragon.Controller.Controller.DeviceControl.HATX.Core.Model;
//using Dragon.Controller.Controller.DeviceControl.OTG;
//using Dragon.Controller.Database.Models;
//using LibUsbDotNet.LibUsb;
//using Microsoft.Win32;
//using System.Collections.Concurrent;
//using System.Diagnostics;
//using System.Runtime.InteropServices;
//using System.Text;
//using System.Text.Json;
//using System.Text.Json.Serialization;
//using System.Text.RegularExpressions;
//using System.Xml;

//namespace Dragon.Controller.DeviceControl.OTG
//{

//    public class AoaDeviceHelper
//    {
//        private static readonly Regex SerialRegex = new Regex(@"^USB\\[^\\]+\\[A-Za-z0-9]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
//        private static readonly Regex VidRegex = new Regex(@"VID_([0-9A-F]{4})", RegexOptions.IgnoreCase);
//        private static readonly Regex PidRegex = new Regex(@"PID_([0-9A-F]{4})", RegexOptions.IgnoreCase);

//        [StructLayout(LayoutKind.Sequential)]
//        struct wdi_options_create_list
//        {
//            [MarshalAs(UnmanagedType.Bool)] public bool list_all;
//            [MarshalAs(UnmanagedType.Bool)] public bool list_hubs;
//            [MarshalAs(UnmanagedType.Bool)] public bool trim_whitespaces;
//        }

//        [DllImport("libwdi.dll")] static extern int wdi_create_list(out IntPtr list, ref wdi_options_create_list options);
//        [DllImport("libwdi.dll")] static extern int wdi_destroy_list(IntPtr list);


//        // ======== SETUPAPI / CFGMGR32 ========
//        const uint DIGCF_PRESENT = 0x2;
//        const uint DIGCF_ALLCLASSES = 0x4;
//        const uint SPDRP_DEVICEDESC = 0;
//        const uint SPDRP_SERVICE = 4;
//        const uint SPDRP_CLASS = 7;
//        const uint SPDRP_CLASSGUID = 8;
//        const uint SPDRP_DRIVER = 9;
//        const uint SPDRP_MFG = 11;

//        [StructLayout(LayoutKind.Sequential)]
//        struct SP_DEVINFO_DATA
//        {
//            public uint cbSize;
//            public Guid ClassGuid;
//            public uint DevInst;
//            public IntPtr Reserved;
//        }

//        [StructLayout(LayoutKind.Sequential)]
//        struct SP_PROPCHANGE_PARAMS
//        {
//            public SP_CLASSINSTALL_HEADER ClassInstallHeader;
//            public int StateChange;
//            public int Scope;
//            public int HwProfile;
//        }

//        [StructLayout(LayoutKind.Sequential)]
//        struct SP_CLASSINSTALL_HEADER
//        {
//            public int cbSize;
//            public int InstallFunction;
//        }
//        const int DIF_PROPERTYCHANGE = 0x12;
//        const int DICS_DISABLE = 1;
//        const int DICS_ENABLE = 2;
//        const int DICS_FLAG_GLOBAL = 1;

//        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Unicode)]
//        static extern IntPtr SetupDiGetClassDevs(ref Guid ClassGuid, string? Enumerator, IntPtr hwndParent, uint Flags);

//        [DllImport("setupapi.dll", SetLastError = true)]
//        static extern bool SetupDiEnumDeviceInfo(IntPtr DeviceInfoSet, uint MemberIndex, ref SP_DEVINFO_DATA DeviceInfoData);

//        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Unicode)]
//        static extern bool SetupDiGetDeviceInstanceId(IntPtr DeviceInfoSet, ref SP_DEVINFO_DATA DeviceInfoData,
//            StringBuilder DeviceInstanceId, int DeviceInstanceIdSize, out int RequiredSize);

//        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Unicode)]
//        static extern bool SetupDiGetDeviceRegistryProperty(IntPtr DeviceInfoSet, ref SP_DEVINFO_DATA DeviceInfoData,
//            uint Property, out uint PropertyRegDataType, StringBuilder PropertyBuffer, int PropertyBufferSize, out uint RequiredSize);

//        [DllImport("setupapi.dll", SetLastError = true)]
//        static extern bool SetupDiDestroyDeviceInfoList(IntPtr DeviceInfoSet);

//        [DllImport("cfgmgr32.dll", CharSet = CharSet.Unicode)] static extern int CM_Locate_DevNode(out uint pdnDevInst, string pDeviceID, uint ulFlags);
//        [DllImport("cfgmgr32.dll")] static extern int CM_Get_DevNode_Status(out uint pulStatus, out uint pulProblemNumber, uint dnDevInst, uint ulFlags);
//        [DllImport("cfgmgr32.dll")] static extern int CM_Get_Parent(out uint pdnDevInst, uint dnDevInst, uint ulFlags);
//        [DllImport("cfgmgr32.dll")] static extern int CM_Get_Child(out uint pdnDevInst, uint dnDevInst, uint ulFlags);
//        [DllImport("cfgmgr32.dll")] static extern int CM_Get_Sibling(out uint pdnDevInst, uint dnDevInst, uint ulFlags);
//        [DllImport("cfgmgr32.dll", CharSet = CharSet.Unicode)] static extern int CM_Get_Device_ID(uint dnDevInst, StringBuilder Buffer, int BufferLen, uint ulFlags);

//        [DllImport("setupapi.dll", SetLastError = true)]
//        static extern bool SetupDiSetClassInstallParams(IntPtr h, ref SP_DEVINFO_DATA d, ref SP_PROPCHANGE_PARAMS p, int size);

//        [DllImport("setupapi.dll", SetLastError = true)]
//        static extern bool SetupDiCallClassInstaller(int f, IntPtr h, ref SP_DEVINFO_DATA d);

//        [DllImport("setupapi.dll", SetLastError = true)]
//        static extern IntPtr SetupDiGetClassDevs(IntPtr ClassGuid, string? Enumerator, IntPtr hwndParent, uint Flags);


//        #region ------------------------- AOADevice -------------------------
//        public static AoaDevice? GetDeviceInfo(string instanceId, bool withRelations = true)
//        {
//            var usbGuid = new Guid("36fc9e60-c465-11cf-8056-444553540000");
//            IntPtr h = SetupDiGetClassDevs(ref usbGuid, null, IntPtr.Zero, DIGCF_PRESENT);
//            try
//            {
//                uint i = 0;
//                var data = new SP_DEVINFO_DATA { cbSize = (uint)Marshal.SizeOf<SP_DEVINFO_DATA>() };
//                while (SetupDiEnumDeviceInfo(h, i++, ref data))
//                {
//                    var sbId = new StringBuilder(512);
//                    SetupDiGetDeviceInstanceId(h, ref data, sbId, sbId.Capacity, out _);
//                    if (!sbId.ToString().Equals(instanceId, StringComparison.OrdinalIgnoreCase)) continue;
//                    return BuildInfo(h, data, sbId.ToString(), withRelations);
//                }
//            }
//            finally { SetupDiDestroyDeviceInfoList(h); }
//            return null;
//        }
//        public static ConcurrentDictionary<string, AoaDevice> AoaDeviceDict = new ConcurrentDictionary<string, AoaDevice>(StringComparer.OrdinalIgnoreCase);
//        public static void LoadAllAeoDevices(bool withRelations = true)
//        {
//            var usbGuid = new Guid("36fc9e60-c465-11cf-8056-444553540000");
//            var list = new List<AoaDevice>();
//            IntPtr h = SetupDiGetClassDevs(ref usbGuid, null, IntPtr.Zero, DIGCF_PRESENT);

//            try
//            {
//                uint i = 0;
//                var data = new SP_DEVINFO_DATA { cbSize = (uint)Marshal.SizeOf<SP_DEVINFO_DATA>() };
//                while (SetupDiEnumDeviceInfo(h, i++, ref data))
//                {
//                    var idSb = new StringBuilder(512);
//                    if (!SetupDiGetDeviceInstanceId(h, ref data, idSb, idSb.Capacity, out _)) continue;
//                    string instanceId = idSb.ToString();

//                    // --- lọc 1: phải là USB\VID_...&PID_...\serial ---
//                    if (!instanceId.StartsWith("USB\\", StringComparison.OrdinalIgnoreCase)) continue;

//                    if (!SerialRegex.IsMatch(instanceId)) continue;
//                    if (!instanceId.Contains("VID_") || !instanceId.Contains("PID_")) continue;

//                    // --- lọc 2: parse VID/PID ---
//                    if (!TryParseVidPid(instanceId, out ushort vid, out ushort pid)) continue;

//                    // --- lọc 3: VID phải nằm trong list Android ---
//                    if (!AndroidVID.Vids.Contains(vid)) continue;

//                    var info = new AoaDevice
//                    {
//                        InstanceId = instanceId,
//                        Vid = vid,
//                        Pid = pid
//                    };
//                    info.DeviceId = info.InstanceId.Split('\\').Last().ToLower();
//                    info.Description = GetProp(h, data, SPDRP_DEVICEDESC);
//                    info.ClassName = GetProp(h, data, SPDRP_CLASS);
//                    info.ClassGuid = GetProp(h, data, SPDRP_CLASSGUID);
//                    info.Manufacturer = GetProp(h, data, SPDRP_MFG);
//                    info.Service = GetProp(h, data, SPDRP_SERVICE);

//                    string driverKey = GetProp(h, data, SPDRP_DRIVER);
//                    if (!string.IsNullOrEmpty(driverKey))
//                    {
//                        using var k = Registry.LocalMachine.OpenSubKey(
//                            $@"SYSTEM\CurrentControlSet\Control\Class\{driverKey}");
//                        info.DriverName = k?.GetValue("InfPath") as string ?? "";
//                    }

//                    if (CM_Locate_DevNode(out uint dev, instanceId, 0) == 0)
//                    {
//                        CM_Get_DevNode_Status(out uint st, out _, dev, 0);
//                        info.Status = ((st & 0x8) != 0) ? "Started" : "Stopped";

//                        if (withRelations)
//                        {
//                            if (CM_Get_Parent(out uint p, dev, 0) == 0)
//                            {
//                                var pb = new StringBuilder(512);
//                                CM_Get_Device_ID(p, pb, pb.Capacity, 0);
//                                info.ParentId = pb.ToString();
//                            }
//                            if (CM_Get_Child(out uint c, dev, 0) == 0)
//                            {
//                                uint cur = c;
//                                do
//                                {
//                                    var cb = new StringBuilder(512);
//                                    CM_Get_Device_ID(cur, cb, cb.Capacity, 0);
//                                    info.Children.Add(cb.ToString());
//                                } while (CM_Get_Sibling(out uint n, cur, 0) == 0 && (cur = n) != 0);
//                            }
//                        }
//                    }

//                    AoaDeviceDict.AddOrUpdate(info.DeviceId, info, (key, existing) =>
//                    {
//                        existing.UpdateFromScan(info);
//                        return existing;
//                    });

//                }
//            }
//            finally { SetupDiDestroyDeviceInfoList(h); }

//        }
//        public static List<AoaDevice> GetStartedDevices()
//        {
//            LoadAllAeoDevices();
//            // Values của ConcurrentDictionary trả về snapshot an toàn
//            return AoaDeviceDict.Values.Where(d => d.Status.Equals("Started", StringComparison.OrdinalIgnoreCase)).ToList();
//        }
//        public static List<AoaDevice> GetAllDevices()
//        {
//            LoadAllAeoDevices();
//            return AoaDeviceDict.Values.ToList();
//        }
//        static AoaDevice BuildInfo(IntPtr h, SP_DEVINFO_DATA data, string instanceId, bool withRelations)
//        {
//            var info = new AoaDevice { InstanceId = instanceId };
//            info.DeviceId = info.InstanceId.Split('\\').Last().ToLower();
//            info.Description = GetProp(h, data, SPDRP_DEVICEDESC);
//            info.ClassName = GetProp(h, data, SPDRP_CLASS);
//            info.ClassGuid = GetProp(h, data, SPDRP_CLASSGUID);
//            info.Manufacturer = GetProp(h, data, SPDRP_MFG);
//            info.Service = GetProp(h, data, SPDRP_SERVICE);

//            string driverKey = GetProp(h, data, SPDRP_DRIVER);
//            if (!string.IsNullOrEmpty(driverKey))
//            {
//                using var k = Registry.LocalMachine.OpenSubKey($@"SYSTEM\CurrentControlSet\Control\Class\{driverKey}");
//                info.DriverName = k?.GetValue("InfPath") as string ?? "";
//            }

//            if (CM_Locate_DevNode(out uint dev, instanceId, 0) == 0)
//            {
//                CM_Get_DevNode_Status(out uint st, out _, dev, 0);
//                info.Status = ((st & 0x8) != 0) ? "Started" : "Stopped";

//                if (withRelations)
//                {
//                    if (CM_Get_Parent(out uint p, dev, 0) == 0)
//                    {
//                        var pb = new StringBuilder(512);
//                        CM_Get_Device_ID(p, pb, pb.Capacity, 0);
//                        info.ParentId = pb.ToString();
//                    }
//                    if (CM_Get_Child(out uint c, dev, 0) == 0)
//                    {
//                        uint cur = c;
//                        do
//                        {
//                            var cb = new StringBuilder(512);
//                            CM_Get_Device_ID(cur, cb, cb.Capacity, 0);
//                            info.Children.Add(cb.ToString());
//                        } while (CM_Get_Sibling(out uint n, cur, 0) == 0 && (cur = n) != 0);
//                    }
//                }
//            }
//            return info;
//        }
//        static string GetProp(IntPtr h, SP_DEVINFO_DATA d, uint prop)
//        {
//            var sb = new StringBuilder(1024);
//            if (SetupDiGetDeviceRegistryProperty(h, ref d, prop, out _, sb, sb.Capacity, out _))
//                return sb.ToString();
//            return string.Empty;
//        }
//        private static bool TryParseVidPid(string instanceId, out ushort vid, out ushort pid)
//        {
//            vid = pid = 0;
//            var mVid = VidRegex.Match(instanceId);
//            var mPid = PidRegex.Match(instanceId);
//            if (!mVid.Success || !mPid.Success) return false;
//            vid = Convert.ToUInt16(mVid.Groups[1].Value, 16);
//            pid = Convert.ToUInt16(mPid.Groups[1].Value, 16);
//            return true;
//        }
//        static bool ChangeState(IntPtr h, ref SP_DEVINFO_DATA dev, int state)
//        {
//            var p = new SP_PROPCHANGE_PARAMS
//            {
//                ClassInstallHeader = new SP_CLASSINSTALL_HEADER
//                {
//                    cbSize = Marshal.SizeOf<SP_CLASSINSTALL_HEADER>(),
//                    InstallFunction = DIF_PROPERTYCHANGE
//                },
//                StateChange = state,
//                Scope = DICS_FLAG_GLOBAL,
//                HwProfile = 0
//            };
//            return SetupDiSetClassInstallParams(h, ref dev, ref p, Marshal.SizeOf(p)) &&
//                   SetupDiCallClassInstaller(DIF_PROPERTYCHANGE, h, ref dev);
//        }
//        public static void CloseDevice(AoaDevice aoaDevice, bool removeFromDict = false)
//        {
//            if (aoaDevice == null) return;

//            // khóa nhẹ trên chính object để tránh 2 thread cùng close
//            lock (aoaDevice)
//            {
//                try
//                {
//                    var usb = aoaDevice.UsbDevice;
//                    if (usb != null)
//                    {
//                        // LibUsbDotNet: IUsbDevice có IsOpen, Close, Dispose
//                        if (usb.IsOpen)
//                        {
//                            usb.Close();   // ngắt kết nối USB
//                        }
//                        usb.Dispose();     // giải phóng handle
//                        aoaDevice.UsbDevice = null; // quan trọng: để lần sau biết là chưa mở
//                    }

//                    // cập nhật trạng thái trong dict cho dễ lọc
//                    aoaDevice.Status = "Stopped";
//                }
//                catch (Exception ex)
//                {
//                    // log nếu cần
//                    Console.WriteLine($"Close {aoaDevice.DeviceId} lỗi: {ex.Message}");
//                }
//            }

//            if (removeFromDict)
//            {
//                AoaDeviceDict.TryRemove(aoaDevice.DeviceId, out _);
//            }

//            if (_hidManagers.TryRemove(aoaDevice.DeviceId, out var existingManager))
//            {
//                try
//                {
//                    existingManager?.Dispose();
//                    existingManager = null;
//                }
//                catch (Exception ex)
//                {
//                    Debug.WriteLine($"[CloseUsbDevice] Error disposing HID manager for {aoaDevice.DeviceId}: {ex.Message}");
//                }
//            }
//        }


//        #endregion

//        #region ------------------------- BackUp & Restore -------------------------
//        private static readonly string BackupFilePath = Path.Combine(AppContext.BaseDirectory, "Extension", "Backup", "winusb_backup.json");
//        private static readonly string PsexePath = Path.Combine(AppContext.BaseDirectory, "Extension", "ScrcpyNet", "PsExec64.exe");
//        private static readonly object s_lock = new object();
//        private static Dictionary<string, DeviceBackup> LoadBackupDictionary()
//        {
//            try
//            {
//                if (File.Exists(BackupFilePath))
//                {
//                    string json = File.ReadAllText(BackupFilePath);
//                    var dict = JsonSerializer.Deserialize(json, DriverUsbBackupJsonContext.Default.DictionaryStringDeviceBackup);
//                    return dict ?? new Dictionary<string, DeviceBackup>(StringComparer.OrdinalIgnoreCase);
//                }
//            }
//            catch (Exception ex)
//            {
//                Debug.WriteLine($"[DriverUSBClass] Failed to load backup file: {ex.Message}");
//            }
//            return new Dictionary<string, DeviceBackup>(StringComparer.OrdinalIgnoreCase);
//        }
//        private static void SaveBackupDictionary(Dictionary<string, DeviceBackup> dict)
//        {
//            try
//            {
//                string? dir = Path.GetDirectoryName(BackupFilePath);
//                if (!string.IsNullOrEmpty(dir))
//                    Directory.CreateDirectory(dir);

//                // DÙNG TRỰC TIẾP JsonTypeInfo - 100% AOT safe
//                string json = JsonSerializer.Serialize(dict, DriverUsbBackupJsonContext.Default.DictionaryStringDeviceBackup);
//                // ghi atomic để tránh corrupt khi crash
//                string tempPath = BackupFilePath + ".tmp";
//                File.WriteAllText(tempPath, json);
//                File.Move(tempPath, BackupFilePath, overwrite: true);
//            }
//            catch (Exception ex)
//            {
//                Debug.WriteLine($"[DriverUSBClass] Failed to save backup file: {ex.Message}");
//            }
//        }
//        public static bool ForceInstallWinUsb(string instanceId)
//        {
//            if (string.IsNullOrEmpty(instanceId)) return false;

//            if (IsSystem())
//                return ForceInstallWinUsbInternal(instanceId);

//            // Thay vì RunViaPsExec, dùng Task Scheduler
//            return CreateAndRunSystemTask(instanceId, isRestore: false);
//        }

//        public static bool RestoreOriginalDriver(string instanceId)
//        {
//            if (string.IsNullOrEmpty(instanceId)) return false;

//            if (IsSystem())
//                return RestoreOriginalDriverInternal(instanceId);

//            return CreateAndRunSystemTask(instanceId, isRestore: true);
//        }
//        public static bool ForceInstallWinUsbInternal(string instanceId)
//        {
//            if (string.IsNullOrEmpty(instanceId))
//                return false;

//            string registryPath = $@"SYSTEM\CurrentControlSet\Enum\{instanceId}";

//            try
//            {
//                using (var deviceKey = Registry.LocalMachine.OpenSubKey(registryPath, writable: true))
//                {
//                    if (deviceKey == null)
//                    {
//                        Debug.WriteLine($"[ForceInstallWinUsb] Cannot open registry key: {registryPath}");
//                        return false;
//                    }

//                    string? originalService = deviceKey.GetValue("Service") as string;
//                    string[]? originalGuids = deviceKey.GetValue("DeviceInterfaceGUIDs") as string[];

//                    // Lưu backup vào file JSON (thread‑safe)
//                    lock (s_lock)
//                    {
//                        var backups = LoadBackupDictionary();
//                        if (!backups.ContainsKey(instanceId))
//                        {
//                            backups[instanceId] = new DeviceBackup
//                            {
//                                Service = originalService,
//                                DeviceInterfaceGuids = originalGuids
//                            };
//                            SaveBackupDictionary(backups);
//                            var guid = originalGuids != null ? string.Join(",", originalGuids) : "null";
//                            var service = originalService ?? "null";
//                            Debug.WriteLine($"[ForceInstallWinUsb] Backed up: Service='{service}', GUIDs='{guid}' for {instanceId}");
//                        }
//                    }

//                    // 1. Đặt Service = WinUSB
//                    deviceKey.SetValue("Service", "WinUSB", RegistryValueKind.String);
//                    Debug.WriteLine($"[ForceInstallWinUsb] Set Service=WinUSB for {instanceId}");

//                    // 2. Thêm DeviceInterfaceGUIDs nếu chưa có
//                    string[]? currentGuids = deviceKey.GetValue("DeviceInterfaceGUIDs") as string[];
//                    if (currentGuids == null || currentGuids.Length == 0)
//                    {
//                        const string winUsbGuid = "{DEE824EF-729B-4A0E-9C14-B7117D33A817}";
//                        deviceKey.SetValue("DeviceInterfaceGUIDs", new string[] { winUsbGuid }, RegistryValueKind.MultiString);

//                        Debug.WriteLine($"[ForceInstallWinUsb] Added DeviceInterfaceGUIDs for {instanceId}");
//                    }
//                }

//                RestartDevice(instanceId);
//                return true;
//            }
//            catch (Exception ex)
//            {
//                Debug.WriteLine($"[ForceInstallWinUsb] Error: {ex.Message}");
//                return false;
//            }
//        }
//        public static bool RestoreOriginalDriverInternal(string instanceId)
//        {
//            if (string.IsNullOrEmpty(instanceId))
//                return false;

//            DeviceBackup? backup;
//            lock (s_lock)
//            {
//                var backups = LoadBackupDictionary();
//                if (!backups.TryGetValue(instanceId, out backup))
//                {
//                    Debug.WriteLine($"[Restore] No backup found for {instanceId}");
//                    return false;
//                }
//                backups.Remove(instanceId);
//                SaveBackupDictionary(backups);
//            }

//            string registryPath = $@"SYSTEM\CurrentControlSet\Enum\{instanceId}";

//            try
//            {
//                using (var deviceKey = Registry.LocalMachine.OpenSubKey(registryPath, writable: true))
//                {
//                    if (deviceKey == null)
//                    {
//                        Debug.WriteLine($"[Restore] Cannot open registry key: {registryPath}");
//                        return false;
//                    }

//                    // Khôi phục Service
//                    if (backup.Service != null)
//                    {
//                        deviceKey.SetValue("Service", backup.Service, RegistryValueKind.String);
//                        Debug.WriteLine($"[Restore] Restored Service='{backup.Service}' for {instanceId}");
//                    }
//                    else
//                    {
//                        try
//                        {
//                            deviceKey.DeleteValue("Service", throwOnMissingValue: false);
//                            Debug.WriteLine($"[Restore] Deleted Service value for {instanceId}");
//                        }
//                        catch { }
//                    }

//                    // Khôi phục DeviceInterfaceGUIDs
//                    if (backup.DeviceInterfaceGuids != null)
//                    {
//                        deviceKey.SetValue("DeviceInterfaceGUIDs", backup.DeviceInterfaceGuids, RegistryValueKind.MultiString);
//                        Debug.WriteLine($"[Restore] Restored original DeviceInterfaceGUIDs for {instanceId}");
//                    }
//                    else
//                    {
//                        try
//                        {
//                            deviceKey.DeleteValue("DeviceInterfaceGUIDs", throwOnMissingValue: false);
//                            Debug.WriteLine($"[Restore] Deleted DeviceInterfaceGUIDs value for {instanceId}");
//                        }
//                        catch { }
//                    }
//                }

//                RestartDevice(instanceId);
//                Debug.WriteLine($"[Restore] Successfully restored driver for {instanceId}");
//                return true;
//            }
//            catch (Exception ex)
//            {
//                Debug.WriteLine($"[Restore] Error: {ex.Message}");
//                return false;
//            }
//        }
//        public static bool RestartDeviceNative(string instanceId)
//        {
//            IntPtr h = SetupDiGetClassDevs(IntPtr.Zero, null, IntPtr.Zero, DIGCF_PRESENT | DIGCF_ALLCLASSES);
//            if (h == IntPtr.Zero || h == new IntPtr(-1)) return false;

//            try
//            {
//                var dev = new SP_DEVINFO_DATA { cbSize = (uint)Marshal.SizeOf<SP_DEVINFO_DATA>() };
//                uint i = 0;
//                while (SetupDiEnumDeviceInfo(h, i++, ref dev))
//                {
//                    var sb = new StringBuilder(512);
//                    SetupDiGetDeviceInstanceId(h, ref dev, sb, sb.Capacity, out _);
//                    if (!sb.ToString().Equals(instanceId, StringComparison.OrdinalIgnoreCase)) continue;

//                    // 1. disable
//                    if (!ChangeState(h, ref dev, DICS_DISABLE)) return false;
//                    Thread.Sleep(800);
//                    // 2. enable
//                    if (!ChangeState(h, ref dev, DICS_ENABLE)) return false;
//                    return true;
//                }
//            }
//            finally { SetupDiDestroyDeviceInfoList(h); }
//            return false;
//        }
//        private static bool IsSystem()
//        {
//            var sid = System.Security.Principal.WindowsIdentity.GetCurrent().User?.Value;
//            return sid == "S-1-5-18";
//        }
//        private static bool CreateAndRunSystemTask(string instanceId, bool isRestore)
//        {
//            string exePath = Environment.ProcessPath!;
//            string action = isRestore ? "--do-restore" : "--do-install";
//            string taskName = "WinUsbSetup_" + Guid.NewGuid().ToString("N")[..8]; // tên duy nhất

//            // Lệnh tạo task:
//            //   /sc onstart: chạy khi khởi động (At startup)
//            //   /ru SYSTEM: tài khoản SYSTEM
//            //   /rl HIGHEST: mức quyền cao nhất
//            //   /tr "...": đường dẫn exe và tham số
//            //   /f: ghi đè không hỏi
//            string createArgs = $"/create /tn {taskName} " +
//                                $"/tr \"\\\"{exePath}\\\" {action} \\\"{instanceId}\\\"\" " +
//                                $"/sc onstart /ru SYSTEM /rl HIGHEST /f";

//            Run("schtasks", createArgs);

//            // Chạy task ngay bây giờ
//            string runArgs = $"/run /tn {taskName}";
//            Run("schtasks", runArgs);

//            // Chờ một chút để task hoàn thành (có thể dùng vòng lặp kiểm tra trạng thái)
//            System.Threading.Thread.Sleep(3000);

//            // (Tùy chọn) Xóa task sau khi chạy xong
//            string deleteArgs = $"/delete /tn {taskName} /f";
//            Run("schtasks", deleteArgs);

//            // Ở đây bạn có thể kiểm tra log hoặc trạng thái để trả về true/false
//            return true; // Hoặc dựa vào kết quả thực tế
//        }

//        static void Run(string file, string arg)
//        {
//            var p = new System.Diagnostics.Process();
//            p.StartInfo.FileName = file;
//            p.StartInfo.Arguments = arg;
//            p.StartInfo.CreateNoWindow = true;
//            p.StartInfo.UseShellExecute = false;
//            p.Start();
//            p.WaitForExit();
//        }
//        private static bool RunViaPsExec(string argsForSelf)
//        {
//            if (!File.Exists(PsexePath))
//            {
//                Debug.WriteLine("[PsExec] File not found: " + PsexePath);
//                return false;
//            }

//            string exePath = Environment.ProcessPath!;
//            string psArgs = $"-s -accepteula -h \"{exePath}\" {argsForSelf}";

//            var psi = new ProcessStartInfo
//            {
//                FileName = PsexePath,
//                Arguments = psArgs,
//                UseShellExecute = false,
//                CreateNoWindow = true,
//                RedirectStandardOutput = true,
//                RedirectStandardError = true
//            };

//            using var proc = new Process { StartInfo = psi, EnableRaisingEvents = true };
//            var stdout = new StringBuilder();
//            var stderr = new StringBuilder();

//            proc.OutputDataReceived += (s, e) => { if (e.Data != null) stdout.AppendLine(e.Data); };
//            proc.ErrorDataReceived += (s, e) => { if (e.Data != null) stderr.AppendLine(e.Data); };

//            proc.Start();
//            proc.BeginOutputReadLine();
//            proc.BeginErrorReadLine();

//            // chờ tối đa 30s, tránh treo
//            if (!proc.WaitForExit(30000))
//            {
//                try { proc.Kill(true); } catch { }
//                Debug.WriteLine("[PsExec] Timeout");
//                return false;
//            }

//            // đảm bảo đọc hết
//            proc.WaitForExit();

//            Debug.WriteLine("[PsExec stdout]: " + stdout.ToString());
//            Debug.WriteLine("[PsExec stderr]: " + stderr.ToString());

//            return proc.ExitCode == 0;
//        }

//        #endregion

//        #region ------------------------- Pnputil -------------------------
//        public static void RestartDevice(string instanceId)
//        {
//            RunPnputil($"/disable-device \"{instanceId}\"", 10000);
//            Thread.Sleep(1500);

//            RunPnputil($"/enable-device \"{instanceId}\"", 10000);
//            Thread.Sleep(1500);
//        }
//        public static void RunPnputil(string args, int wait = 10000)
//        {
//            try
//            {
//                var psi = new ProcessStartInfo("pnputil", args)
//                {
//                    CreateNoWindow = true,
//                    UseShellExecute = false,
//                    RedirectStandardOutput = true,
//                    Verb = "runas"
//                };
//                using var p = Process.Start(psi);
//                string output = p?.StandardOutput.ReadToEnd() ?? "";
//                p?.WaitForExit(wait);
//                Debug.WriteLine($"[PNP] pnputil {args} => {output.Trim()}");
//            }
//            catch (Exception ex)
//            {
//                Debug.WriteLine($"[PNP-ERR] {ex.Message}");
//            }
//        }

//        #endregion
//        #region ------------------------- UsbContext -------------------------
//        private static UsbContext? _sharedCtx;
//        public static UsbContext GetContext()
//        {
//            if (_sharedCtx == null || _sharedCtx.IsDisposed)
//                _sharedCtx = new UsbContext();
//            return _sharedCtx;
//        }
//        public static void Cleanup()
//        {
//            _sharedCtx?.Dispose();
//            _sharedCtx = null;
//        }
//        #endregion
//        #region ------------------------- ADB -------------------------

//        private const string TMP = "/data/local/tmp/";
//        private static AdbClient adbClient = new AdbClient();
//        private static readonly Regex PhysicalRegex = new(@"Physical size: (\d+)x(\d+)", RegexOptions.Compiled);
//        private static readonly Regex OverrideRegex = new(@"Override size: (\d+)x(\d+)", RegexOptions.Compiled);
//        static (int w, int h) GetPhysicalSize(DeviceData device, IAdbClient adb)
//        {
//            // 1. ưu tiên mCurrentDisplayRect – Samsung/Xiaomi/Oppo đều có
//            var out1 = Shell(device, adb, "wm size");
//            // ví dụ: mCurrentDisplayRect=Rect(0, 0 - 1080, 2400)
//            var m = OverrideRegex.Match(out1);
//            if (m.Success) return (int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value));

//            m = PhysicalRegex.Match(out1);
//            if (m.Success) return (int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value));
//            return (0, 0);
//        }

//        private static readonly Regex XmlFromDumpRegex = new Regex(@"<\?xml.*</hierarchy>", RegexOptions.Singleline | RegexOptions.Compiled);
//        static string Shell(DeviceData device, IAdbClient adb, string command)
//        {
//            var sb = new StringBuilder();
//            try
//            {
//                // thêm 2>&1 để gộp stderr vào stdout, và dùng predicate để đọc từng dòng
//                adb.ExecuteRemoteCommand(command + " 2>&1", device, line =>
//                {
//                    sb.AppendLine(line);
//                    return true; // tiếp tục đọc
//                });
//                return sb.ToString().Trim();
//            }
//            catch (OperationCanceledException)
//            {
//                Debug.WriteLine($"[{device.Serial}] shell timeout: {command}");
//                return string.Empty;
//            }
//            catch (Exception ex)
//            {
//                Debug.WriteLine($"[{device.Serial}] shell error: {ex.Message}");
//                return string.Empty;
//            }
//        }

//        static List<string> ShellLong(DeviceData device, IAdbClient adb, string command)
//        {
//            // gộp stderr luôn cho chắc
//            var cmd = command + " 2>&1";
//            try
//            {
//                // đọc stream, không chờ hết
//                return adb.ExecuteRemoteEnumerable(cmd, device).ToList();
//            }
//            catch (Exception ex)
//            {
//                Debug.WriteLine($"[{device.Serial}] {ex.Message}");
//                return new List<string>();
//            }
//        }
//        public static List<NodeObj>? DumpUiNodes(DeviceData deviceData)
//        {
//            try
//            {
//                var serial = deviceData.Serial;

//                //CMD.ExecuteAdbV2($"adb -s {serial} shell pkill -f minitouch; pkill -f minicap; pkill -f atx-agent", 3000);
//                //CMD.ExecuteAdbV2($"adb -s {serial} shell pkill -f tonghopbaitap; pkill -f ttap2; pkill -f bincapTap1", 3000);
//                //CMD.ExecuteAdbV2($"adb -s {serial} shell {TMP}tonghopbaitap server --stop", 3000);
//                //CMD.ExecuteAdbV2($"adb -s {serial} shell {TMP}atx-agent server --stop", 3000);

//                Shell(deviceData, adbClient, "pkill -f minitouch; pkill -f minicap; pkill -f atx-agent");
//                Shell(deviceData, adbClient, "pkill -f tonghopbaitap; pkill -f ttap2; pkill -f bincapTap1");
//                Shell(deviceData, adbClient, $"{TMP}tonghopbaitap server --stop");
//                Shell(deviceData, adbClient, $"{TMP}atx-agent server --stop");

//                //CMD.ExecuteAdbV2($"adb -s {serial} shell pkill -f uiautomator", 2000); // dọ         
//                //CMD.ExecuteAdbV2($"adb -s {serial} shell input keyevent 224", 1000); // wake

//                Shell(deviceData, adbClient, "pkill -f uiautomator"); // dọn
//                Shell(deviceData, adbClient, "input keyevent 224"); // wake

//                //var dump = CMD.ExecuteAdbV2($"adb -s {serial} shell uiautomator dump", 30000); // chạy mới
//                //string raw = CMD.ExecuteAdbV2($"adb -s {serial} shell cat /sdcard/window_dump.xml", 5000);
//                //CMD.ExecuteAdbV2($"adb -s {serial} shell rm /sdcard/window_dump.xml", 2000);

//                var dump = Shell(deviceData, adbClient, "uiautomator dump"); // chạy mới
//                //Thread.Sleep(800); // chờ ghi xong                         
//                string raw = Shell(deviceData, adbClient, "cat /sdcard/window_dump.xml"); // đọc mới   
//                Shell(deviceData, adbClient, "rm /sdcard/window_dump.xml"); // xóa mới
//                if (string.IsNullOrEmpty(raw))
//                    return null;

//                // 2. tách phần XML sạch
//                var match = XmlFromDumpRegex.Match(raw);
//                if (!match.Success)
//                {
//                    Debug.WriteLine("Không tìm thấy XML trong output");
//                    return null;
//                }

//                string cleanXml = match.Value.Trim();
//                var doc = new XmlDocument();
//                doc.LoadXml(cleanXml);

//                // 3. đi qua toàn bộ node
//                var list = new List<NodeObj>();

//                void Walk(XmlNode n)
//                {
//                    var obj = NodeObj.Create(n);
//                    if (obj != null) list.Add(obj);
//                    foreach (XmlNode child in n.ChildNodes) Walk(child);
//                }

//                Walk(doc.DocumentElement!);
//                return list;
//            }
//            catch (Exception ex)
//            {
//                Debug.WriteLine($"DumpUiNodes lỗi: {ex.Message}");
//                return null;
//            }
//        }



//        #endregion

//        public static ConcurrentDictionary<string, AoaHidManager> _hidManagers = new(StringComparer.OrdinalIgnoreCase);
//        public static AoaDevice? FindBySerial(string deviceID)
//        {
//            if (string.IsNullOrWhiteSpace(deviceID))
//                return null;
//            var key = deviceID.Trim().ToLowerInvariant();
//            return AoaDeviceDict.TryGetValue(key, out var dev) ? dev : null;
//        }
//        public static IUsbDevice? OpenByDeviceID(ushort vid, ushort pid, string deviceID)
//        {
//            Debug.WriteLine($"[OpenBySerial] DeviceID ='{deviceID}' VID:{vid:X4} PID:{pid:X4}");

//            var listcontext = GetContext().List();
//            var candidates = listcontext.Where(x => x.VendorId == vid && x.ProductId == pid).ToList();
//            Debug.WriteLine($"[OpenBySerial] {candidates.Count} candidate(s)");

//            var aoaDevice = FindBySerial(deviceID);
//            if (aoaDevice == null)
//            {
//                Debug.WriteLine($"[OpenBySerial] ❌ Device with Serial='{deviceID}' not found in WMI");
//                return null;
//            }

//            // Thử mở lần đầu
//            foreach (var device in candidates)
//            {
//                try
//                {
//                    if (device.IsOpen) continue;

//                    if (device.TryOpen())
//                    {

//                        string serial = device.Info?.SerialNumber ?? "";
//                        if (serial.Equals(deviceID, StringComparison.OrdinalIgnoreCase))
//                        {
//                            aoaDevice.Service = "WinUSB";
//                            Debug.WriteLine("[OpenBySerial] ✅ MATCHED (first try)!");
//                            // Đóng các thiết bị khác
//                            foreach (var other in candidates)
//                                if (other != device) { try { other.Close(); } catch { } other.Dispose(); }
//                            return device;
//                        }

//                        device.Close();
//                        device.Dispose();
//                    }
//                    else
//                    {
//                        // Không mở được → có thể không phải driver WinUSB
//                        string serial = device.Info?.SerialNumber ?? "";
//                        if (string.IsNullOrEmpty(serial))
//                        {
//                            Debug.WriteLine("[OpenBySerial] ⚠️ Need to install WinUSB driver...");
//                            bool driverInstalled = ForceInstallWinUsbInternal(aoaDevice.InstanceId);
//                        }
//                        device.Dispose();
//                    }
//                }
//                catch (Exception ex)
//                {
//                    Debug.WriteLine($"[OpenBySerial] Error: {ex.Message}");
//                    device.Dispose();
//                }
//            }

//            return null;
//        }
//        public static IUsbDevice? OpenDevice(AoaDevice aoaDevice)
//        {
//            if (aoaDevice.UsbDevice != null && aoaDevice.UsbDevice.IsOpen)
//                return aoaDevice.UsbDevice;

//            ushort vid = aoaDevice.Vid;
//            ushort pid = aoaDevice.Pid;

//            Debug.WriteLine($"[OpenDevice] Opening device with Serial='{aoaDevice.DeviceId}' VID:{vid:X4} PID:{pid:X4}");

//            Debug.WriteLine($"[OpenDevice] Checking ADB devices...");
//            var deviceDatas = adbClient.GetDevices();

//            if (deviceDatas.Any() && deviceDatas.Any(c => c.Serial.Equals(aoaDevice.DeviceId, StringComparison.OrdinalIgnoreCase)))
//            {
//                var deviceData = deviceDatas.First(c => c.Serial.Equals(aoaDevice.DeviceId, StringComparison.OrdinalIgnoreCase));

//                bool IsScreenOff(string powerDump) =>
//                         powerDump.Contains("mWakefulness=Dozing") ||
//                         powerDump.Contains("mWakefulness=Asleep") ||
//                         powerDump.Contains("Display Power: state=OFF");

//                bool IsLocked(string policyDump) =>
//                    policyDump.Contains("mIsShowing=true") ||
//                    policyDump.Contains("showing=true") && policyDump.Contains("mInputRestricted=true");

//                var powerDump = Shell(deviceData, adbClient, "dumpsys power");
//                var policyDump = Shell(deviceData, adbClient, "dumpsys window policy");

//                var size = GetPhysicalSize(deviceData, adbClient);
//                int w = size.w;
//                int h = size.h;

//                if (IsScreenOff(powerDump))
//                {
//                    Shell(deviceData, adbClient, "input keyevent 26"); // power
//                    Thread.Sleep(800);

//                    Shell(deviceData, adbClient, $"input tap {w / 2},{h / 2} "); // power
//                    Thread.Sleep(300);

//                    Shell(deviceData, adbClient, $"input swipe {w / 2} {(int)(h * 0.86)} {w / 2} {(int)(h * 0.2)} 1000");
//                    Thread.Sleep(300);

//                    Shell(deviceData, adbClient, $"input swipe {w / 2} {(int)(h * 0.84)} {w / 2} {(int)(h * 0.2)} 1000");
//                    Thread.Sleep(200);
//                }

//                if (IsLocked(policyDump))
//                {

//                    var key = new KeypadString();
//                    string keycode = "123789";

//                    foreach (char c in keycode)
//                    {
//                        int digit = c - '0'; // '1' -> 1
//                        var (x, y) = key.GetClick(digit, w, h);

//                        Shell(deviceData, adbClient, $"input tap {x} {y}");
//                        Thread.Sleep(200); // chờ animation phím, 150-200ms là đẹp
//                    }
//                    var (xOk, yOK) = key.GetOk(w, h);
//                    Shell(deviceData, adbClient, $"input tap {xOk} {yOK}");
//                    Thread.Sleep(2000);
//                }


//                var IsOpen = OpenDeveloperOptions(deviceData, adbClient);
//                if (!IsOpen)
//                {
//                    Debug.WriteLine("Không thể mở deverlop options (nhà phát triển)");
//                    return null;
//                }
//                //Thread.Sleep(2000);

//                var list = DumpUiNodes(deviceData);
//                if (list == null) return null;
//                var checks = list.Where(c => c.ResourceName == "com.android.settings:id/switch_widget").ToList();
//                var enableButton = list.FirstOrDefault(node => node.ResourceName == "com.android.settings:id/switch_widget"
//                && (node.Text.Contains("phát triển", StringComparison.OrdinalIgnoreCase) || node.Text.Contains("Developer options", StringComparison.OrdinalIgnoreCase)));

//                if (enableButton != null)
//                {
//                    var center = enableButton.Bound.CenterPos;
//                    Shell(deviceData, adbClient, $"input tap {center.X} {center.Y}");
//                }
//                else
//                {

//                    Debug.WriteLine("Tạo hàm tắt Usb debugging trước ");
//                    return null;
//                }
//            }
//            static bool OpenDeveloperOptions(DeviceData device, IAdbClient adb)
//            {
//                // 1. Lấy SDK để chọn lệnh ưu tiên
//                int sdk = 0;
//                var sdkStr = Shell(device, adb, "getprop ro.build.version.sdk");
//                int.TryParse(sdkStr, out sdk);

//                // 2. Danh sách lệnh theo độ ưu tiên (mới -> cũ)
//                var cmds = new List<string>();

//                if (sdk >= 29) // Android 10+
//                {
//                    cmds.Add("am start -n com.android.settings/.Settings$DevelopmentSettingsDashboardActivity");
//                }
//                if (sdk >= 26) // Android 8+
//                {
//                    cmds.Add("am start -n com.android.settings/com.android.settings.DevelopmentSettings");
//                }
//                // lệnh gốc từ Android 3 tới nay, chạy được trên mọi máy
//                cmds.Add("am start -n com.android.settings/.DevelopmentSettings");
//                cmds.Add("am start -a android.settings.APPLICATION_DEVELOPMENT_SETTINGS");

//                // 3. Thử từng lệnh
//                foreach (var cmd in cmds)
//                {
//                    var outp = Shell(device, adb, cmd);
//                    // thành công nếu có "Starting:" và không có "Error" / "not found"
//                    if (outp.Contains("Starting:") && !outp.ToLower().Contains("error") && !outp.Contains("not found"))
//                    {
//                        Debug.WriteLine($"[{device.Serial}] OK với: {cmd}");
//                        return true;
//                    }
//                }
//                return false;
//            }

//            // 1. Cố gắng mở chính xác bằng Serial Number trước
//            var usb = OpenByDeviceID(vid, pid, aoaDevice.DeviceId);
//            if (usb != null)
//            {
//                Debug.WriteLine("[OpenDevice] ✅ Opened by Serial");
//                aoaDevice.UsbDevice = usb;
//                return usb;
//            }

//            // 3. Phương án cuối cùng: Kill ADB server để giải phóng thiết bị và thử lại
//            Debug.WriteLine("[OpenDevice] Failed to open by VID/PID, killing ADB server as last resort...");


//            usb = OpenByDeviceID(vid, pid, aoaDevice.DeviceId);
//            if (usb != null)
//            {
//                Debug.WriteLine("[OpenDevice] ✅ Opened after killing ADB");
//                aoaDevice.UsbDevice = usb;
//                return usb;
//            }

//            Debug.WriteLine("[OpenDevice] ❌ Failed to open device");
//            return null;
//        }

//        public static void CloseAllAoaDevice()
//        {
//            foreach (var kvp in AoaDeviceDict)
//            {
//                try { CloseDevice(kvp.Value); } catch { }
//            }

//            Cleanup();
//        }

//        public static AoaHidManager? GetOrCreateHidManagerForDevice(AoaDevice device)
//        {
//            string serial = device.DeviceId;
//            if (_hidManagers.TryGetValue(serial, out var existingManager))
//                return existingManager;

//            var newManager = new AoaHidManager(device);
//            if (!newManager.Initialize(registerMouse: true, registerKeyboard: true))
//            {
//                Debug.WriteLine($"❌ Khởi tạo HID thất bại cho {serial}");
//                newManager.Dispose();
//                return null;
//            }

//            if (_hidManagers.TryAdd(serial, newManager))
//            {
//                Debug.WriteLine($"✅ Đã tạo HID Manager cho {serial}");
//                return newManager;
//            }
//            else
//            {
//                newManager.Dispose();
//                return _hidManagers.GetOrAdd(serial, (key) => newManager);
//            }
//        }

//    }

//}
