
namespace Dragon.Controller.TaskDeviceManager.Model.Mouse
{
    public class LongPressArg
    {
        public ControlMode ControlMode { get; set; }
        public float x { get; set; }
        public float y { get; set; }
        public int Duration { get; set; } = 1000; // ms
    }
}
