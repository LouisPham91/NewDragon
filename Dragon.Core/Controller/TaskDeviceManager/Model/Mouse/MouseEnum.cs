

namespace Dragon.Controller.TaskDeviceManager.Model.Mouse
{
    public enum ControlMode
    {
        ADB,
        ADBEvent,
        ATX,
        Scrcpy,
        HDI,
        OTG,
        ACC
    }

    public enum ClickMode
    {
        SingleClick,
        DoubleClick,
    }
    public enum KeyboardType
    {
        None,
        ADB,
        ACC,
    }
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    public enum SwipeMode
    {
        Fixed,
        Random
    }
}
