
using System.Drawing;
using System.Text.Json.Serialization;

namespace Dragon.Controller.TaskDeviceManager.Model.Vision
{
    public class ATXNode
    {
        [JsonPropertyName("className")]
        public string ClassName { get; set; } = string.Empty;

        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;

        [JsonPropertyName("resourceName")]
        public string ResourceName { get; set; } = string.Empty;

        [JsonPropertyName("packageName")]
        public string PackageName { get; set; } = string.Empty;

        [JsonPropertyName("contentDescription")]
        public string ContentDescription { get; set; } = string.Empty;
        public string SecialClickPoint { get; set; } = ""; // "x%,y%" click riêng cho ATX
        public PointCenter? PotisonClick { get; set; }
        public VisionAction VisionAction { get; set; } = VisionAction.DetectOnly;

        public Point? GetTapPoint(int width, int height)
        {
            if (PotisonClick == null) return null;
            if (width <= 0 || height <= 0) return null;

            // Nếu giá trị phần trăm nằm ngoài 0..100 thì clamp
            float pxPercent = PotisonClick.X;
            float pyPercent = PotisonClick.Y;

            if (float.IsNaN(pxPercent) || float.IsNaN(pyPercent)) return null;

            pxPercent = Math.Max(0f, Math.Min(100f, pxPercent));
            pyPercent = Math.Max(0f, Math.Min(100f, pyPercent));

            // Chuyển percent -> pixel, làm tròn
            int x = (int)Math.Round(pxPercent * width / 100.0);
            int y = (int)Math.Round(pyPercent * height / 100.0);

            // Clamp vào khung màn hình (0..width, 0..height)
            x = Math.Max(0, Math.Min(width, x));
            y = Math.Max(0, Math.Min(height, y));

            return new Point(x, y);
        }
    }

    public class PointCenter
    {
        public float X { get; set; }
        public float Y { get; set; }
    }
}
