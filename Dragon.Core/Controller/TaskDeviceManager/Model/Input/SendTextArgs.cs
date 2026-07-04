using Dragon.Controller.TaskDeviceManager.Model.Mouse;

namespace Dragon.Controller.TaskDeviceManager.Model.Input
{
    public class SendTextArgs
    {
        public ControlMode ControlMode { get; set; }
        public string Text { get; set; } = string.Empty;
        public InputTextType InputTextType { get; set; }
        public TypeOption TypeOption { get; set; }

    }
}
