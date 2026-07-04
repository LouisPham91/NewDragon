using Dragon.Controller.TaskDeviceManager.Model.Mouse;

namespace Dragon.Controller.TaskDeviceManager.Model.File
{
    public class FileArgs
    {
        public bool hasSu { get; set; } = false;
        // Đường dẫn nguồn
        public string SourcePath { get; set; } = string.Empty;
        // Đường dẫn đích
        public string DestinationPath { get; set; } = string.Empty;

        public bool isArmBroadCast { get; set; } = false;
        public string ArmBroadCommand { get; set; } = string.Empty;
        // push or pull
        public CopyFileOperation CopyFileOperation { get; set; }
        public ControlMode ControlMode { get; set; }
    }

    public enum CopyFileOperation
    {
        Pull,
        Push,
        FileInAndroid
    }

}
