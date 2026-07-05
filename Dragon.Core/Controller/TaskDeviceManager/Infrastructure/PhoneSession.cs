using AdvancedSharpAdbClient;
using AdvancedSharpAdbClient.Models;
using Dragon.Controller.DeviceControl;
using Dragon.Controller.DeviceControl.HATX;
using Dragon.Controller.DeviceControl.OTG;
using Dragon.Controller.DeviceControl.ScrcpyNet.InterFace;
using Dragon.Database.Models;

namespace Dragon.Controller.TaskDeviceManager.Infrastructure
{
    public class PhoneSession 
    {
        public Phone Phone { get; }
        public AdbClient? AdbClient { get; set; } = null;
        public DeviceData? DeviceData { get; set; }
        public AtxDevice? Atx { get; set; }
        public CancellationTokenSource Cts { get; } = new();

        // property tính toán — mỗi lần đọc là tìm lại panel
        public IDeviceScreen? Screen { get; set; }
        public IDeviceInput? Input { get; set; }
        public IDeviceInput? InputUhid { get; set; }

        public IDeviceMouse? Mouse { get; set; }        // scrcpy
        public IDeviceMouse? MouseUhid { get; set; }    // uhid
        public AoaDeviceSession? Aoa { get; set; }
        public AppCapture? AppCapture { get; set; }

        public PhoneSession(Phone phone, AdbClient? adbClient = null, DeviceData? deviceData = null, AtxDevice? hAtx = null)
        {
            Phone = phone;
            AdbClient = adbClient;
            Atx = hAtx;
            DeviceData = deviceData;
        }
    }
}