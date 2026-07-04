

using Dragon.Controller.DeviceControl.ScrcpyNet.Services;

namespace Dragon.Controller.DeviceControl.ScrcpyNet.InterFace
{
    public interface IEventDongBo
    {
        void HandleEvent(IControlMessage msg);
    }
}
