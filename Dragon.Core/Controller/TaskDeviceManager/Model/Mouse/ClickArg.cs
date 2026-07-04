

namespace Dragon.Controller.TaskDeviceManager.Model.Mouse
{
    public class ClickArg
    {
        public ControlMode ControlMode { get; set; }
        public ClickMode ClickMode { get; set; } = ClickMode.SingleClick;
        public float x { get; set; }
        public float y { get; set; }
    }
}
