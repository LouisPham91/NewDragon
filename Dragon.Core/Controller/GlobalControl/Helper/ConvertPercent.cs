
using System.Drawing;


namespace Dragon.Controller.GlobalControl.Helper
{
    public class ConvertPercent
    {
        public static string PointToPercentString(Point p, Bitmap bmp)
        {
            if (bmp == null) return string.Empty;

            double percentX = (double)p.X / bmp.Width * 100.0;
            double percentY = (double)p.Y / bmp.Height * 100.0;

            // format với 2 chữ số thập phân
            return $"{percentX:F2},{percentY:F2}";
        }

        public static Point PercentStringToPoint(string text, Bitmap bmp)
        {
            if (bmp == null) return Point.Empty;
            if (string.IsNullOrWhiteSpace(text)) return Point.Empty;

            var parts = text.Split(',');
            if (parts.Length != 2) return Point.Empty;

            if (double.TryParse(parts[0], out double percentX) && double.TryParse(parts[1], out double percentY))
            {
                int x = (int)(percentX / 100.0 * bmp.Width);
                int y = (int)(percentY / 100.0 * bmp.Height);
                return new Point(x, y);
            }
            return Point.Empty;
        }

        public static Point PercentStringToPoint(string text, int with, int height)
        {
            if (string.IsNullOrWhiteSpace(text)) return Point.Empty;

            var parts = text.Split(',');
            if (parts.Length != 2) return Point.Empty;

            if (double.TryParse(parts[0], out double percentX) && double.TryParse(parts[1], out double percentY))
            {
                int x = (int)(percentX / 100.0 * with);
                int y = (int)(percentY / 100.0 * height);
                return new Point(x, y);
            }
            return Point.Empty;
        }

        public static string RectToPercentString(Rectangle rect, Bitmap bmp)
        {
            if (bmp == null) return string.Empty;

            double px = (double)rect.X / bmp.Width * 100.0;
            double py = (double)rect.Y / bmp.Height * 100.0;
            double pw = (double)rect.Width / bmp.Width * 100.0;
            double ph = (double)rect.Height / bmp.Height * 100.0;

            return $"{px:F2},{py:F2},{pw:F2},{ph:F2}";
        }

        public static Rectangle PercentStringToRect(string text, Bitmap bmp)
        {
            if (bmp == null) return Rectangle.Empty;
            if (string.IsNullOrWhiteSpace(text)) return Rectangle.Empty;

            var parts = text.Split(',');
            if (parts.Length != 4) return Rectangle.Empty;

            if (double.TryParse(parts[0], out double px) &&
                double.TryParse(parts[1], out double py) &&
                double.TryParse(parts[2], out double pw) &&
                double.TryParse(parts[3], out double ph))
            {
                int x = (int)(px / 100.0 * bmp.Width);
                int y = (int)(py / 100.0 * bmp.Height);
                int w = (int)(pw / 100.0 * bmp.Width);
                int h = (int)(ph / 100.0 * bmp.Height);

                return new Rectangle(x, y, w, h);
            }
            return Rectangle.Empty;
        }
        public static Rectangle PhysicalRectToBitmapRect(Rectangle rectPhysical, int physicalWidth, int physicalHeight, Bitmap bmp)
        {
            if (bmp == null) return Rectangle.Empty;

            // Tính phần trăm theo physical
            double px = (double)rectPhysical.X / physicalWidth;
            double py = (double)rectPhysical.Y / physicalHeight;
            double pw = (double)rectPhysical.Width / physicalWidth;
            double ph = (double)rectPhysical.Height / physicalHeight;

            // Convert sang bitmap
            int x = (int)(px * bmp.Width);
            int y = (int)(py * bmp.Height);
            int w = (int)(pw * bmp.Width);
            int h = (int)(ph * bmp.Height);

            return new Rectangle(x, y, w, h);
        }

    }
}
