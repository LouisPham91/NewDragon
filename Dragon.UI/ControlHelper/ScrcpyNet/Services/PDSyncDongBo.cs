using Dragon.ControlHelper.UIController;
using Dragon.Controller.DeviceControl.ScrcpyNet.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dragon.ControlHelper.ScrcpyNet.Services
{
    public class PDSyncDongBo
    {
        private static readonly Lazy<PDSyncDongBo> instance = new(() => new PDSyncDongBo { });
        public static PDSyncDongBo Instance => instance.Value;
        public bool IsDongBo = false;
        public static event Action<IControlMessage>? DeviceEvent;
        public void SendEventTodevice(IControlMessage msg)
        {
            DeviceEvent?.Invoke(msg); // Gọi event cho tất cả thiết bị
        }

    }
}
