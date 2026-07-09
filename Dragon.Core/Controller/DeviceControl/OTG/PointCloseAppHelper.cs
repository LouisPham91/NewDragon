// File: DeviceControl\OTG\PointCloseAppHelper.cs
namespace Dragon.Controller.DeviceControl.OTG
{
    public static class PointCloseAppHelper
    {
        /// <summary>
        /// Parse string "x,y" percent thành (float x, float y)
        /// </summary>
        public static (float x, float y) Parse(string pointStr)
        {
            var parts = pointStr.Split(',');
            if (parts.Length == 2 &&
                float.TryParse(parts[0].Trim(), out var x) &&
                float.TryParse(parts[1].Trim(), out var y))
                return (x, y);
            return (50, 90); // default
        }

        /// <summary>
        /// Convert percent -> pixel
        /// </summary>
        public static (int x, int y) ToPixel(string pointStr, int physicalWidth, int physicalHeight)
        {
            var (px, py) = Parse(pointStr);
            return ((int)(physicalWidth * px / 100f), (int)(physicalHeight * py / 100f));
        }
    }
}