namespace Dragon.Controller.DeviceControl.OTG.Model
{
    public class AoaOcr
    {
        public string Title { get; set; } = "";
        public string[] Keywords { get; set; } = Array.Empty<string>();
        public int TimeoutMs { get; set; } = 1500;
        public int IntervalMs { get; set; } = 500;
        public int MaxSwipes { get; set; } = 0;
        public int OffsetX { get; set; } = 0;
        public int OffsetY { get; set; } = 0;

        // ===== THÊM: Tọa độ swipe (percent) =====
        public float SwipeStartX { get; set; } = 50;   // % màn hình
        public float SwipeStartY { get; set; } = 80;   // % màn hình (dưới)
        public float SwipeEndX { get; set; } = 50;     // % màn hình
        public float SwipeEndY { get; set; } = 40;     // % màn hình (trên)
        public int SwipeDurationMs { get; set; } = 300; // thời gian vuốt
        public bool SwipeIsPercent { get; set; } = true; // dùng % hay pixel
    }
}