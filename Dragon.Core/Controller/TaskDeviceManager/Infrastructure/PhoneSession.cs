using AdvancedSharpAdbClient;
using AdvancedSharpAdbClient.Models;
using Dragon.Controller.DeviceControl.HATX;
using Dragon.Controller.DeviceControl.ScrcpyNet.InterFace;
using Dragon.Database.Models;

namespace Dragon.Controller.TaskDeviceManager.Infrastructure
{
    public class PhoneSession 
    {
        public Phone Phone { get; }
        public AdbClient AdbClient { get; init; } = null!;
        public DeviceData? DeviceData { get; private set; }
        public AtxDevice? Atx { get; set; }
        public CancellationTokenSource Cts { get; } = new();

        // property tính toán — mỗi lần đọc là tìm lại panel
        public IDeviceScreen? Screen { get; set; }
        public IDeviceInput? Input { get; set; }
        public IDeviceInput? InputUhid { get; set; }

        public IDeviceMouse? Mouse { get; set; }        // scrcpy
        public IDeviceMouse? MouseUhid { get; set; }    // uhid

        public PhoneSession(Phone phone, AdbClient adbClient, DeviceData deviceData, AtxDevice hAtx)
        {
            Phone = phone;
            AdbClient = adbClient;
            Atx = hAtx;
            DeviceData = deviceData;
        }
    }
}