
using System.Drawing;
using System.Globalization;

namespace Dragon.Controller.GlobalControl.Helper
{
    public static class BitmapCropper
    {
        // Parse percent string: hỗ trợ 2 dạng:
        // 1) left,top,right,bottom  (giá trị % của left/top/right/bottom)
        // 2) x,y,w,h               (giá trị % của x,y,width,height)
        // Trả Rectangle ở image coords (pixel)
        public static Rectangle PercentStringToImageRect(string percentString, Bitmap source)
        {
            if (source == null || string.IsNullOrWhiteSpace(percentString))
                return Rectangle.Empty;

            var parts = percentString.Split(',');
            if (parts.Length != 4) return Rectangle.Empty;

            bool ok0 = float.TryParse(parts[0].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out var p0);
            bool ok1 = float.TryParse(parts[1].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out var p1);
            bool ok2 = float.TryParse(parts[2].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out var p2);
            bool ok3 = float.TryParse(parts[3].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out var p3);
            if (!ok0 || !ok1 || !ok2 || !ok3) return Rectangle.Empty;

            int imgW = source.Width;
            int imgH = source.Height;

            // Nếu p2 > p0 và p3 > p1 và p2<=100 && p3<=100 thì có thể là right,bottom
            bool looksLikeLTRB = (p2 > p0 && p3 > p1 && p0 >= 0 && p1 >= 0 && p2 <= 100 && p3 <= 100);

            int left, top, right, bottom;
            if (looksLikeLTRB)
            {
                left = (int)Math.Round(p0 / 100.0 * imgW);
                top = (int)Math.Round(p1 / 100.0 * imgH);
                right = (int)Math.Round(p2 / 100.0 * imgW);
                bottom = (int)Math.Round(p3 / 100.0 * imgH);
            }
            else
            {
                // treat as x,y,w,h percent
                left = (int)Math.Round(p0 / 100.0 * imgW);
                top = (int)Math.Round(p1 / 100.0 * imgH);
                int w = (int)Math.Round(p2 / 100.0 * imgW);
                int h = (int)Math.Round(p3 / 100.0 * imgH);
                right = left + Math.Max(1, w);
                bottom = top + Math.Max(1, h);
            }

            // Clamp to image bounds
            left = Math.Max(0, Math.Min(imgW - 1, left));
            top = Math.Max(0, Math.Min(imgH - 1, top));
            right = Math.Max(left + 1, Math.Min(imgW, right));
            bottom = Math.Max(top + 1, Math.Min(imgH, bottom));

            int width = right - left;
            int height = bottom - top;
            return new Rectangle(left, top, width, height);
        }

        // Convert image rect -> control (PictureBox) coords (dùng SizeMode = Zoom)
        public static Rectangle ConvertRectToPictureBox(string cropString, Size pb, Bitmap source)
        {
            if (source == null || string.IsNullOrEmpty(cropString))
                return Rectangle.Empty;

            var imgRect = PercentStringToImageRect(cropString, source);
            if (imgRect == Rectangle.Empty) return Rectangle.Empty;

            // Tính display rect
            int imgW = source.Width;
            int imgH = source.Height;
            int pbW = pb.Width;
            int pbH = pb.Height;

            float ratio = Math.Min((float)pbW / imgW, (float)pbH / imgH);
            int displayW = (int)Math.Round(imgW * ratio);
            int displayH = (int)Math.Round(imgH * ratio);
            int offsetX = (pbW - displayW) / 2;
            int offsetY = (pbH - displayH) / 2;

            int x = offsetX + (int)Math.Round(imgRect.X * ratio);
            int y = offsetY + (int)Math.Round(imgRect.Y * ratio);
            int w = Math.Max(1, (int)Math.Round(imgRect.Width * ratio));
            int h = Math.Max(1, (int)Math.Round(imgRect.Height * ratio));

            return new Rectangle(x, y, w, h);
        }

        // Crop bằng percent string (dùng PercentStringToImageRect)
        public static Bitmap? CropByString(Bitmap source, string cropString)
        {
            if (source == null || string.IsNullOrEmpty(cropString)) return null;
            var rect = PercentStringToImageRect(cropString, source);
            if (rect == Rectangle.Empty) return null;
            return CropByRectangleSafe(source, rect);
        }

        // Crop an toàn từ Rectangle (kiểm tra biên)
        public static Bitmap? CropByRectangleSafe(Bitmap source, Rectangle cropRect)
        {
            if (source == null) return null;
            // Clamp cropRect vào trong source
            int left = Math.Max(0, cropRect.X);
            int top = Math.Max(0, cropRect.Y);
            int right = Math.Min(source.Width, cropRect.X + cropRect.Width);
            int bottom = Math.Min(source.Height, cropRect.Y + cropRect.Height);

            int w = right - left;
            int h = bottom - top;
            if (w <= 0 || h <= 0) return null;

            Rectangle safeRect = new Rectangle(left, top, w, h);
            Bitmap cropped = new Bitmap(safeRect.Width, safeRect.Height);
            using (Graphics g = Graphics.FromImage(cropped))
            {
                g.DrawImage(source, new Rectangle(0, 0, cropped.Width, cropped.Height), safeRect, GraphicsUnit.Pixel);
            }
            return cropped;
        }

        // Giữ hàm cũ nhưng gọi bản safe
        public static Bitmap? CropByRectangle(Bitmap source, Rectangle cropRect)
        {
            return CropByRectangleSafe(source, cropRect);
        }

        public static Bitmap? CropByNum(Bitmap source, int ValueUpdown)
        {
            if (source == null) return null;

            // Tính toán kích thước mới
            int newWidth = source.Width - 2 * ValueUpdown;
            int newHeight = source.Height - 2 * ValueUpdown;

            // Kiểm tra giới hạn
            if (newWidth <= 0 || newHeight <= 0) return null;

            // Tạo vùng crop
            Rectangle cropRect = new Rectangle(ValueUpdown, ValueUpdown, newWidth, newHeight);

            // Clone bitmap theo vùng crop
            Bitmap cropped = new Bitmap(cropRect.Width, cropRect.Height);
            using (Graphics g = Graphics.FromImage(cropped))
            {
                g.DrawImage(source, new Rectangle(0, 0, cropped.Width, cropped.Height), cropRect, GraphicsUnit.Pixel);
            }

            return cropped;
        }
    }


}

