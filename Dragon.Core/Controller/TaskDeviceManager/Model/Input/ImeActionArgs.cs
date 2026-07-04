using Dragon.Controller.TaskDeviceManager.Model.Mouse;


namespace Dragon.Controller.TaskDeviceManager.Model.Input
{
    
    public class ImeActionArgs
    {
        public ControlMode ControlMode { get; set; }

        // Loại hành động (ClearText, SmartEnter, Keycode, EditorCode, Show, Hide)
        public ImeActionType ImeActionType { get; set; }

        // Nếu ActionType là Keycode hoặc EditorCode thì cần Code
        public int Code { get; set; } = 0;

        // Text có thể dùng cho log hoặc mở rộng sau này
        public string? Text { get; set; }

        // Timeout mặc định
        public int Timeout { get; set; } = 5000;
    }

}
