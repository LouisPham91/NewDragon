using Dragon.Controller.TaskDeviceManager.Model.Mouse;


namespace Dragon.Controller.TaskDeviceManager.Model.App
{
    public class AppArgs
    {
        public string Name { get; set; } = string.Empty;
        public ControlMode ControlMode { get; set; }  
        public AppAction Action { get; set; }  
        public string PathAPk { get; set; } = string.Empty;
        public string Package { get; set; } = string.Empty;
        public bool Monkey { get; set; } = false;
        public bool Stop { get; set; } = false;
        public bool Wait { get; set; } = false;
        public int WaitTime { get; set; } = 20000;
        public bool Front { get; set; } = false;
        public string? Activity { get; set; } = null;
    }

    public enum AppAction
    {
        Start,
        Stop,
        Clear,
        Uninstall,
        Install
    }

}
