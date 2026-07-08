namespace Dragon.Controller.DeviceControl.OTG.Model
{
    public class AoaOcr
    {
        public string[] Keywords { get; set; } = Array.Empty<string>();
        public int TimeoutMs { get; set; } = 1500;   // chờ tối đa
        public int IntervalMs { get; set; } = 500;   // chụp mỗi lần
        public int MaxSwipes { get; set; } = 0;      // 0 = không swipe, >0 = kéo tới khi thấy
        public int OffsetX { get; set; } = 0;        // click lệch X
        public int OffsetY { get; set; } = 0;        // click lệch Y
    }
}