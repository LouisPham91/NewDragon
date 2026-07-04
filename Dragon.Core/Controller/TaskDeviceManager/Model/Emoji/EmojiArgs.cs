
using Dragon.Controller.TaskDeviceManager.Model.Mouse;
using Dragon.Controller.TaskDeviceManager.Model.Vision;


namespace Dragon.Controller.TaskDeviceManager.Model.Emoji
{
    public class EmojiArgs
    {
        public ControlMode ControlMode { get; set; }
        public string Name { get; set; } = string.Empty;
        public Reaction? Reactions { get; set; } = null;
        public VisionScanArgs? VisionScanArgs { get; set; } = null;
    }

}
