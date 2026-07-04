using Microsoft.Win32;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Dragon.Controller.DeviceControl.OTG
{
    public static class AoaDeviceScanner
    {
        private static readonly Regex SerialRegex = new Regex(@"^USB\\[^\\]+\\[A-Za-z0-9]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex VidRegex = new Regex(@"VID_([0-9A-F]{4})", RegexOptions.IgnoreCase);
        private static readonly Regex PidRegex = new Regex(@"PID_([0-9A-F]{4})", RegexOptions.IgnoreCase);

        //[StructLayout(LayoutKind.Sequential)]
        //struct wdi_options_create_list
        //{
        //    [MarshalAs(UnmanagedType.Bool)] public bool list_all;
        //    [MarshalAs(UnmanagedType.Bool)] public bool list_hubs;
        //    [MarshalAs(UnmanagedType.Bool)] public bool trim_whitespaces;
        //}

        //[DllImport("libwdi.dll")] static extern int wdi_create_list(out IntPtr list, ref wdi_options_create_list options);
        //[DllImport("libwdi.dll")] static extern int wdi_destroy_list(IntPtr list);


        // ======== SETUPAPI / CFGMGR32 ========
        const uint DIGCF_PRESENT = 0x2;
        //const uint DIGCF_ALLCLASSES = 0x4;
        const uint SPDRP_DEVICEDESC = 0;
        const uint SPDRP_SERVICE = 4;
        const uint SPDRP_CLASS = 7;
        const uint SPDRP_CLASSGUID = 8;
        const uint SPDRP_DRIVER = 9;
        const uint SPDRP_MFG = 11;

        [StructLayout(LayoutKind.Sequential)]
        struct SP_DEVINFO_DATA
        {
            public uint cbSize;
            public Guid ClassGuid;
            public uint DevInst;
            public IntPtr Reserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct SP_PROPCHANGE_PARAMS
        {
            public SP_CLASSINSTALL_HEADER ClassInstallHeader;
            public int StateChange;
            public int Scope;
            public int HwProfile;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct SP_CLASSINSTALL_HEADER
        {
            public int cbSize;
            public int InstallFunction;
        }
        //const int DIF_PROPERTYCHANGE = 0x12;
        //const int DICS_DISABLE = 1;
        //const int DICS_ENABLE = 2;
        //const int DICS_FLAG_GLOBAL = 1;

        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern IntPtr SetupDiGetClassDevs(ref Guid ClassGuid, string? Enumerator, IntPtr hwndParent, uint Flags);

        [DllImport("setupapi.dll", SetLastError = true)]
        static extern bool SetupDiEnumDeviceInfo(IntPtr DeviceInfoSet, uint MemberIndex, ref SP_DEVINFO_DATA DeviceInfoData);

        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern bool SetupDiGetDeviceInstanceId(IntPtr DeviceInfoSet, ref SP_DEVINFO_DATA DeviceInfoData,
            StringBuilder DeviceInstanceId, int DeviceInstanceIdSize, out int RequiredSize);

        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern bool SetupDiGetDeviceRegistryProperty(IntPtr DeviceInfoSet, ref SP_DEVINFO_DATA DeviceInfoData,
            uint Property, out uint PropertyRegDataType, StringBuilder PropertyBuffer, int PropertyBufferSize, out uint RequiredSize);

        [DllImport("setupapi.dll", SetLastError = true)]
        static extern bool SetupDiDestroyDeviceInfoList(IntPtr DeviceInfoSet);

        [DllImport("cfgmgr32.dll", CharSet = CharSet.Unicode)] static extern int CM_Locate_DevNode(out uint pdnDevInst, string pDeviceID, uint ulFlags);
        [DllImport("cfgmgr32.dll")] static extern int CM_Get_DevNode_Status(out uint pulStatus, out uint pulProblemNumber, uint dnDevInst, uint ulFlags);
        [DllImport("cfgmgr32.dll")] static extern int CM_Get_Parent(out uint pdnDevInst, uint dnDevInst, uint ulFlags);
        [DllImport("cfgmgr32.dll")] static extern int CM_Get_Child(out uint pdnDevInst, uint dnDevInst, uint ulFlags);
        [DllImport("cfgmgr32.dll")] static extern int CM_Get_Sibling(out uint pdnDevInst, uint dnDevInst, uint ulFlags);
        [DllImport("cfgmgr32.dll", CharSet = CharSet.Unicode)] static extern int CM_Get_Device_ID(uint dnDevInst, StringBuilder Buffer, int BufferLen, uint ulFlags);

        [DllImport("setupapi.dll", SetLastError = true)]
        static extern bool SetupDiSetClassInstallParams(IntPtr h, ref SP_DEVINFO_DATA d, ref SP_PROPCHANGE_PARAMS p, int size);

        [DllImport("setupapi.dll", SetLastError = true)]
        static extern bool SetupDiCallClassInstaller(int f, IntPtr h, ref SP_DEVINFO_DATA d);

        [DllImport("setupapi.dll", SetLastError = true)]
        static extern IntPtr SetupDiGetClassDevs(IntPtr ClassGuid, string? Enumerator, IntPtr hwndParent, uint Flags);

        public static AoaDevice? GetDeviceInfo(string instanceId, bool withRelations = true)
        {
            var usbGuid = new Guid("36fc9e60-c465-11cf-8056-444553540000");
            IntPtr h = SetupDiGetClassDevs(ref usbGuid, null, IntPtr.Zero, DIGCF_PRESENT);
            try
            {
                uint i = 0;
                var data = new SP_DEVINFO_DATA { cbSize = (uint)Marshal.SizeOf<SP_DEVINFO_DATA>() };
                while (SetupDiEnumDeviceInfo(h, i++, ref data))
                {
                    var sbId = new StringBuilder(512);
                    SetupDiGetDeviceInstanceId(h, ref data, sbId, sbId.Capacity, out _);
                    if (!sbId.ToString().Equals(instanceId, StringComparison.OrdinalIgnoreCase)) continue;
                    return BuildInfo(h, data, sbId.ToString(), withRelations);
                }
            }
            finally { SetupDiDestroyDeviceInfoList(h); }
            return null;
        }
        public static ConcurrentDictionary<string, AoaDevice> AoaDeviceDict = new ConcurrentDictionary<string, AoaDevice>(StringComparer.OrdinalIgnoreCase);
        public static AoaDevice? FindByDeviceID(string deviceID, bool autoScanMissing = true, bool refreshExisting = true, bool withRelations = true)
        {
            if (string.IsNullOrWhiteSpace(deviceID)) return null;
            var key = deviceID.Trim().ToLowerInvariant();

            // 1. thử lấy từ cache
            if (AoaDeviceDict.TryGetValue(key, out var cached))
            {
                if (refreshExisting && !string.IsNullOrEmpty(cached.InstanceId))
                {
                    // lấy thông tin tươi từ hệ thống, giống như lúc scan
                    var fresh = GetDeviceInfo(cached.InstanceId, withRelations);
                    if (fresh != null)
                    {
                        // giữ VID/PID cũ nếu GetDeviceInfo không set
                        fresh.Vid = cached.Vid;
                        fresh.Pid = cached.Pid;
                        cached.UpdateFromScan(fresh);
                    }
                }
                return cached;
            }

            // 2. chưa có trong cache
            if (autoScanMissing)
            {
                // scan nhanh toàn bộ thiết bị Android (chính là logic của LoadAllAeoDevices)
                LoadAllAeoDevices(withRelations);
                // thử lại, nhưng không scan tiếp để tránh vòng lặp
                return FindByDeviceID(deviceID, autoScanMissing: false, refreshExisting: false);
            }

            return null;
        }
        public static void LoadAllAeoDevices(bool withRelations = true)
        {
            var usbGuid = new Guid("36fc9e60-c465-11cf-8056-444553540000");
            var list = new List<AoaDevice>();
            IntPtr h = SetupDiGetClassDevs(ref usbGuid, null, IntPtr.Zero, DIGCF_PRESENT);

            try
            {
                uint i = 0;
                var data = new SP_DEVINFO_DATA { cbSize = (uint)Marshal.SizeOf<SP_DEVINFO_DATA>() };
                while (SetupDiEnumDeviceInfo(h, i++, ref data))
                {
                    var idSb = new StringBuilder(512);
                    if (!SetupDiGetDeviceInstanceId(h, ref data, idSb, idSb.Capacity, out _)) continue;
                    string instanceId = idSb.ToString();

                    // --- lọc 1: phải là USB\VID_...&PID_...\serial ---
                    if (!instanceId.StartsWith("USB\\", StringComparison.OrdinalIgnoreCase)) continue;

                    if (!SerialRegex.IsMatch(instanceId)) continue;
                    if (!instanceId.Contains("VID_") || !instanceId.Contains("PID_")) continue;

                    // --- lọc 2: parse VID/PID ---
                    if (!TryParseVidPid(instanceId, out ushort vid, out ushort pid)) continue;

                    // --- lọc 3: VID phải nằm trong list Android ---
                    if (!AndroidVID.Vids.Contains(vid)) continue;

                    var info = new AoaDevice
                    {
                        InstanceId = instanceId,
                        Vid = vid,
                        Pid = pid
                    };
                    info.DeviceId = info.InstanceId.Split('\\').Last().ToLower();
                    info.Description = GetProp(h, data, SPDRP_DEVICEDESC);
                    info.ClassName = GetProp(h, data, SPDRP_CLASS);
                    info.ClassGuid = GetProp(h, data, SPDRP_CLASSGUID);
                    info.Manufacturer = GetProp(h, data, SPDRP_MFG);
                    info.Service = GetProp(h, data, SPDRP_SERVICE);

                    string driverKey = GetProp(h, data, SPDRP_DRIVER);
                    if (!string.IsNullOrEmpty(driverKey))
                    {
                        using var k = Registry.LocalMachine.OpenSubKey(
                            $@"SYSTEM\CurrentControlSet\Control\Class\{driverKey}");
                        info.DriverName = k?.GetValue("InfPath") as string ?? "";
                    }

                    if (CM_Locate_DevNode(out uint dev, instanceId, 0) == 0)
                    {
                        CM_Get_DevNode_Status(out uint st, out _, dev, 0);
                        info.Status = ((st & 0x8) != 0) ? "Started" : "Stopped";

                        if (withRelations)
                        {
                            if (CM_Get_Parent(out uint p, dev, 0) == 0)
                            {
                                var pb = new StringBuilder(512);
                                CM_Get_Device_ID(p, pb, pb.Capacity, 0);
                                info.ParentId = pb.ToString();
                            }
                            if (CM_Get_Child(out uint c, dev, 0) == 0)
                            {
                                uint cur = c;
                                do
                                {
                                    var cb = new StringBuilder(512);
                                    CM_Get_Device_ID(cur, cb, cb.Capacity, 0);
                                    info.Children.Add(cb.ToString());
                                } while (CM_Get_Sibling(out uint n, cur, 0) == 0 && (cur = n) != 0);
                            }
                        }
                    }

                    AoaDeviceDict.AddOrUpdate(info.DeviceId, info, (key, existing) =>
                    {
                        existing.UpdateFromScan(info);
                        return existing;
                    });

                }
            }
            finally { SetupDiDestroyDeviceInfoList(h); }

        }
        public static List<AoaDevice> GetStartedDevices()
        {
            LoadAllAeoDevices();
            // Values của ConcurrentDictionary trả về snapshot an toàn
            return AoaDeviceDict.Values.Where(d => d.Status.Equals("Started", StringComparison.OrdinalIgnoreCase)).ToList();
        }
        public static List<AoaDevice> GetAllDevices()
        {
            LoadAllAeoDevices();
            return AoaDeviceDict.Values.ToList();
        }
        static AoaDevice BuildInfo(IntPtr h, SP_DEVINFO_DATA data, string instanceId, bool withRelations)
        {
            var info = new AoaDevice { InstanceId = instanceId };
            info.DeviceId = info.InstanceId.Split('\\').Last().ToLower();
            info.Description = GetProp(h, data, SPDRP_DEVICEDESC);
            info.ClassName = GetProp(h, data, SPDRP_CLASS);
            info.ClassGuid = GetProp(h, data, SPDRP_CLASSGUID);
            info.Manufacturer = GetProp(h, data, SPDRP_MFG);
            info.Service = GetProp(h, data, SPDRP_SERVICE);

            string driverKey = GetProp(h, data, SPDRP_DRIVER);
            if (!string.IsNullOrEmpty(driverKey))
            {
                using var k = Registry.LocalMachine.OpenSubKey($@"SYSTEM\CurrentControlSet\Control\Class\{driverKey}");
                info.DriverName = k?.GetValue("InfPath") as string ?? "";
            }

            if (CM_Locate_DevNode(out uint dev, instanceId, 0) == 0)
            {
                CM_Get_DevNode_Status(out uint st, out _, dev, 0);
                info.Status = ((st & 0x8) != 0) ? "Started" : "Stopped";

                if (withRelations)
                {
                    if (CM_Get_Parent(out uint p, dev, 0) == 0)
                    {
                        var pb = new StringBuilder(512);
                        CM_Get_Device_ID(p, pb, pb.Capacity, 0);
                        info.ParentId = pb.ToString();
                    }
                    if (CM_Get_Child(out uint c, dev, 0) == 0)
                    {
                        uint cur = c;
                        do
                        {
                            var cb = new StringBuilder(512);
                            CM_Get_Device_ID(cur, cb, cb.Capacity, 0);
                            info.Children.Add(cb.ToString());
                        } while (CM_Get_Sibling(out uint n, cur, 0) == 0 && (cur = n) != 0);
                    }
                }
            }
            return info;
        }
        static string GetProp(IntPtr h, SP_DEVINFO_DATA d, uint prop)
        {
            var sb = new StringBuilder(1024);
            if (SetupDiGetDeviceRegistryProperty(h, ref d, prop, out _, sb, sb.Capacity, out _))
                return sb.ToString();
            return string.Empty;
        }
        private static bool TryParseVidPid(string instanceId, out ushort vid, out ushort pid)
        {
            vid = pid = 0;
            var mVid = VidRegex.Match(instanceId);
            var mPid = PidRegex.Match(instanceId);
            if (!mVid.Success || !mPid.Success) return false;
            vid = Convert.ToUInt16(mVid.Groups[1].Value, 16);
            pid = Convert.ToUInt16(mPid.Groups[1].Value, 16);
            return true;
        }
        public static List<AoaDevice> Scan(bool withRelations = true)
        {
            var list = new List<AoaDevice>();
            var usbGuid = new Guid("36fc9e60-c465-11cf-8056-444553540000");
            IntPtr h = SetupDiGetClassDevs(ref usbGuid, null, IntPtr.Zero, DIGCF_PRESENT);
            try
            {
                uint i = 0;
                var data = new SP_DEVINFO_DATA { cbSize = (uint)Marshal.SizeOf<SP_DEVINFO_DATA>() };
                while (SetupDiEnumDeviceInfo(h, i++, ref data))
                {
                    var idSb = new StringBuilder(512);
                    if (!SetupDiGetDeviceInstanceId(h, ref data, idSb, idSb.Capacity, out _)) continue;
                    string instanceId = idSb.ToString();
                    if (!instanceId.StartsWith("USB\\", StringComparison.OrdinalIgnoreCase)) continue;
                    if (!SerialRegex.IsMatch(instanceId)) continue;
                    if (!instanceId.Contains("VID_") || !instanceId.Contains("PID_")) continue;
                    if (!TryParseVidPid(instanceId, out ushort vid, out ushort pid)) continue;
                    if (!AndroidVID.Vids.Contains(vid)) continue;

                    var info = BuildInfo(h, data, instanceId, withRelations);
                    info.Vid = vid;
                    info.Pid = pid;
                    info.InstanceId = instanceId;
                    info.DeviceId = instanceId.Split('\\').Last().ToLower();
                    list.Add(info);
                }
            }
            finally { SetupDiDestroyDeviceInfoList(h); }
            return list;
        }

    }
}
