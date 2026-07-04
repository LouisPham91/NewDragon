using Dragon.Controller.TaskDeviceManager.Model.Mouse;


namespace Dragon.Controller.TaskDeviceManager.Model.Input
{
    public class KeyPressArgs
    {

        public string Command { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public ControlMode ControlMode { get; set; }
        public bool IsCustom { get; set; }

    }
}
