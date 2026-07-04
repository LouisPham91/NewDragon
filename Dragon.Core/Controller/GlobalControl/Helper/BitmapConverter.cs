
using System.Drawing;

namespace Dragon.Controller.GlobalControl.Helper
{

    public static class BitmapConverter
    {
        public static string ToPercent(Rectangle rect, int width, int height)
        {
            double left = (double)rect.Left / width * 100.0;
            double top = (double)rect.Top / height * 100.0;
            double right = (double)rect.Right / width * 100.0;
            double bottom = (double)rect.Bottom / height * 100.0;

            return $"{left:F2},{top:F2},{right:F2},{bottom:F2}";
        }

        public static Rectangle PercentToRect(string percent, int width, int height)
        {
            // tách "9.26,10.42,46.30,18.75"
            var p = percent.Split(',').Select(double.Parse).ToArray();

            int left = (int)Math.Round(p[0] / 100.0 * width);
            int top = (int)Math.Round(p[1] / 100.0 * height);
            int right = (int)Math.Round(p[2] / 100.0 * width);
            int bottom = (int)Math.Round(p[3] / 100.0 * height);

            return Rectangle.FromLTRB(left, top, right, bottom);
        }

        public static Rectangle ConvertRect(Rectangle r, Size from, Size to)
        {
            float sx = (float)to.Width / from.Width;
            float sy = (float)to.Height / from.Height;
            return Rectangle.FromLTRB(
                (int)(r.Left * sx),
                (int)(r.Top * sy),
                (int)(r.Right * sx),
                (int)(r.Bottom * sy)
            );
        }

        // Convert Bitmap -> byte[]
        public static byte[] BitmapToByteArray(Bitmap bitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Chọn định dạng ảnh, ví dụ PNG
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
        }

        // Convert byte[] -> Bitmap
        public static Bitmap? ByteArrayToBitmap(byte[] byteArray)
        {
            if (byteArray == null || byteArray.Length == 0) return null;

            using (MemoryStream ms = new MemoryStream(byteArray))
            {
                return new Bitmap(ms);
            }
        }
    }

}
