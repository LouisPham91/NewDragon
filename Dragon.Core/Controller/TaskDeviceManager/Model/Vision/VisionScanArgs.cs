using Dragon.Controller.TaskDeviceManager.Model.Mouse;

namespace Dragon.Controller.TaskDeviceManager.Model.Vision
{
    public class VisionScanArgs
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public VisionMode VisionMode { get; set; }
        public VisionAction VisionAction { get; set; }
        public ControlMode ControlMode { get; set; }
        public ATXNode ATXNode { get; set; } = new ATXNode();
        public List<ImageOrcText> ImageOrcTexts { get; set; } = new List<ImageOrcText>();

        public int RetryCount { get; set; } = 3;      // thử tối đa 3 lần
        public int RetryDelayMs { get; set; } = 800;  // nghỉ 800ms giữa lần
        public bool StopOnFirstFound { get; set; } = true; // tìm thấy là dừng
    }

    public enum VisionAction
    {
        None,              // không làm gì
        DetectOnly,        // trước là CheckOnly
        DetectAndClick,    // trước là CheckAndClick
        ClickAtPoint,      // trước là ClickOnPoint
        ExtractText        // trước là GetText
    }

    public enum VisionMode
    {
        ByAtxNode,         // trước là ATXNode
        ByImageTemplate,   // trước là ImageExiting (sửa typo ImageExisting)
        ByOcrText          // trước là TextExitInImage (sửa typo TextExists)
    }
}
