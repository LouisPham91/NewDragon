using Dragon.Controller.TaskDeviceManager.Model.Input;
using Dragon.Controller.TaskDeviceManager.Model.Mouse;


namespace Dragon.Controller.TaskDeviceManager.Model.DatabaseColumn
{
    public class GetColumnDataArgs
    {
        public ControlMode ControlMode { get; set; }
        public TypeOption TypeOption { get; set; }
        public WriteAction WriteAction { get; set; }
        public string ColumnName { get; set; } = string.Empty;
        public string SocialNetworkName { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
    }
    public enum WriteAction
    {
        None = 0,
        OTP,
        Column
    }

}
