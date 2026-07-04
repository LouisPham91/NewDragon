
using AdvancedSharpAdbClient.Models;
using Dragon.Controller.DeviceControl.OTG;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace Dragon.Database.Models
{
    public class PhoneBox
    {
        public int Id { get; set; }
        public override string ToString()
        {
            return Id.ToString();
        }
    }
    public class PhoneTemplate
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Product { get; set; } = string.Empty;
        public int PhysicalWidth { get; set; } = 0;
        public int PhysicalHeight { get; set; } = 0;
        public int API { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }

    public class Phone
    {
        // trong class Phone
        public bool MatchesDevice(DeviceData d)
        {
            if (IsWifimode())
                return !string.IsNullOrEmpty(Ipv4) &&
                      !string.IsNullOrEmpty(d.Serial) &&
                       GetIp(d.Serial).Equals(Ipv4, StringComparison.Ordinal);

            if (IsUSBmode())
                return !string.IsNullOrEmpty(d.Serial) &&
                       d.Serial.Equals(DeviceID, StringComparison.Ordinal) &&
                      !d.Serial.Contains(':');

            return false;
        }

        private static string GetIp(string serial) => serial?.Split(':')[0] ?? "";

        // ===== Helpers dùng DeviceState thật =====
        public bool IsOnline(IEnumerable<DeviceData> devices)
        {
            if (PhoneMode == PhoneMode.UOTG)
            {
                var aoa = AoaDeviceScanner.FindByDeviceID(DeviceID);
                if (aoa != null && aoa.Status.Equals("Started", StringComparison.OrdinalIgnoreCase))
                {
                    Online = true;
                }
                else
                {
                    Online = false;
                }

            }
            else
            {
                Online = devices.Any(d => MatchesDevice(d) && d.State == DeviceState.Online);
            }
            return Online;
        }

        public DeviceData? GetDeviceData(IEnumerable<DeviceData> devices)
        {
            return devices.FirstOrDefault(d => (d.Serial == DeviceID || GetIp(d.Serial) == Ipv4) && d.State == DeviceState.Online);
        }

        public bool IsUSBmode()
        {
            return (PhoneMode is PhoneMode.USB or PhoneMode.UATX or PhoneMode.UHDI) ? true : false;
        }
        public bool IsWifimode()
        {
            return (PhoneMode is PhoneMode.WHDI or PhoneMode.WIFI or PhoneMode.WATX) ? true : false;
        }
        public bool IsUHDImode()
        {
            return PhoneMode == PhoneMode.WHDI || PhoneMode == PhoneMode.UHDI ? true : false;
        }
        public bool IsUHDIok()
        {
            return (IsUHDImode() && IsUHDI) ? true : false;
        }

        // ===== Properties =====
        public int Id { get; set; }

        public int PhoneTagNumber { get; set; }
        public int? PhoneBoxId { get; set; }

        public string DeviceID { get; set; } = string.Empty;   // ro.serialno
        public string Serial { get; set; } = string.Empty;     // adb serial

        // <-- DÙNG THẲNG DeviceState, KHÔNG CAST
        public DeviceState DeviceState { get; set; } = DeviceState.Unknown;

        public PhoneMode PhoneMode { get; set; }

        public string Model { get; set; } = string.Empty;
        public string Product { get; set; } = string.Empty;
        public string Usb { get; set; } = string.Empty;
        public string TransportId { get; set; } = string.Empty;

        public string Ipv4 { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

        public bool IsUHDI { get; set; }
        public int PhysicalWidth { get; set; }
        public int PhysicalHeight { get; set; }

        public bool IsRunning { get; set; }
        public string AndroidVersion { get; set; } = string.Empty;
        public int API { get; set; }
        public bool IsUseUSB { get; set; }

        public string ProcVersion { get; set; } = string.Empty;
        public string ProcCpuInfo { get; set; } = string.Empty;

        public bool IsAccessibleAppInstall { get; set; }
        public bool IsPingWifi { get; set; }
        public bool IsRooted { get; set; }
        public bool IsMagisk { get; set; }
        public bool IsKernelSu { get; set; }

        public string PhoneHash { get; set; } = string.Empty;

        [NotMapped]
        public Bitmap? Im { get; set; }

        [NotMapped]
        public bool Online { get; set; } = false;

        [NotMapped]
        public DeviceData? DeviceData { get; set; }

        public override string ToString() => PhoneTagNumber.ToString();

        public void HasValueCompute()
        {
            string input = $"{DeviceID}|{Model}|{AndroidVersion}|{API}|{ProcVersion}|{ProcCpuInfo}";
            PhoneHash = Controller.GlobalControl.Security.HashValue.ComputeStringSHA256(input);
        }
    }

    public enum PhoneMode
    {
        USB,
        WIFI,
        UATX,
        WATX,
        UHDI,
        WHDI,
        UOTG,
        ACC,
        ALL
    }

}
