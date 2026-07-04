
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Dragon.Controller.GlobalControl.Setting;
using System.Diagnostics;

namespace Dragon.DesignView.Public
{
    public static class DesignHelper
    {

        public static int GetTextWidth(string text, Font font)
        {
            Size textSize = TextRenderer.MeasureText(text, font);
            return textSize.Width;
        }
        public static IEnumerable<Control> GetAllControls(Control parent)
        {
            foreach (Control child in parent.Controls)
            {
                yield return child; // trả về chính nó

                // nếu nó còn chứa con nữa thì đi tiếp
                if (child.HasChildren)
                {
                    foreach (var grandChild in GetAllControls(child))
                        yield return grandChild;
                }
            }
        }
        public static int GetTextHeight(Control control)
        {
            // Nếu không có text, chiều cao bằng 0.
            if (string.IsNullOrEmpty(control.Text))
                return 0;

            // Xác định kích thước giới hạn: chiều rộng hiện tại của label, chiều cao lớn (int.MaxValue) để tính toàn bộ cao của text.
            Size proposedSize = new Size(control.Width, int.MaxValue);

            // Cờ WordBreak giúp tính số dòng khi text tự động xuống dòng.
            TextFormatFlags flags = TextFormatFlags.WordBreak;

            // Sử dụng TextRenderer để đo kích thước của text
            Size textSize = TextRenderer.MeasureText(control.Text, control.Font, proposedSize, flags);

            return textSize.Height;
        }

        public static void SapXepPanelNamGiuaPanelKhac(Control controlCha, Control controlCon)
        {
            // Fix for CS1612: Create a new Point object and assign it to controlCon.Location  
            controlCon.Location = new Point(
                (controlCha.Width - controlCon.Width) / 2,
                (controlCha.Height - controlCon.Height) / 2
            );
            controlCon.BringToFront();
        }

        public static Bitmap? GetRandomLogo()
        {
            var image = GetResource.GetRandomImage("Dragon");
            if (image == null)
            {
                Debug.WriteLine("⚠️ Không tìm thấy ảnh ngẫu nhiên nào.");
                return null;
            }
            return (Bitmap?)image;
        }
        public static float GetDpiScaleFactor(Control control)
        {
            using (Graphics g = control.CreateGraphics())
            {
                return g.DpiX / 96f;
            }
        }
        public static GraphicsPath GetRoundedRectangleFloatPath(RectangleF rect, float radius)
        {
            float d = radius * 2f;
            GraphicsPath path = new GraphicsPath();

            if (radius <= 0f)
            {
                path.AddRectangle(rect);
                return path;
            }

            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddLine(rect.X + radius, rect.Y, rect.Right - radius, rect.Y);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddLine(rect.Right, rect.Y + radius, rect.Right, rect.Bottom - radius);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddLine(rect.Right - radius, rect.Bottom, rect.X + radius, rect.Bottom);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.AddLine(rect.X, rect.Bottom - radius, rect.X, rect.Y + radius);
            path.CloseFigure();

            return path;
        }
        public static GraphicsPath GetRoundedRectangle8Path(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int d = radius * 2;

            if (radius <= 0)
            {
                path.AddRectangle(rect);
                return path;
            }

            // Góc trên-trái
            path.AddArc(rect.Left, rect.Top, d, d, 180, 90);

            // Cạnh trên
            path.AddLine(rect.Left + radius, rect.Top, rect.Right - radius, rect.Top);

            // Góc trên-phải
            path.AddArc(rect.Right - d, rect.Top, d, d, 270, 90);

            // Cạnh phải
            path.AddLine(rect.Right, rect.Top + radius, rect.Right, rect.Bottom - radius);

            // Góc dưới-phải
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);

            // Cạnh dưới
            path.AddLine(rect.Right - radius, rect.Bottom, rect.Left + radius, rect.Bottom);

            // Góc dưới-trái
            path.AddArc(rect.Left, rect.Bottom - d, d, d, 90, 90);

            // Cạnh trái
            path.AddLine(rect.Left, rect.Bottom - radius, rect.Left, rect.Top + radius);

            path.CloseFigure();
            return path;
        }

        /// <summary>
        /// Tạo GraphicsPath cho hình chữ nhật với các góc bo tròn chuẩn, 
        /// giống như cách của BorderedPanel.
        /// </summary>
        public static GraphicsPath GetRoundedRectanglePath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int d = radius * 2;

            // Nếu bán kính <= 0, bỏ qua bo tròn
            if (radius <= 0)
            {
                path.AddRectangle(rect);
                return path;
            }
            // Các góc theo thứ tự: Top-left, Top-right, Bottom-right, Bottom-left
            path.AddArc(rect.Left, rect.Top, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Top, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.Left, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        public static void OnPaint(Control control, PaintEventArgs e, int borderRadius_Cha, int borderThickness_Cha, Color BorderColorCha)
        {
            Rectangle rect = new Rectangle(0, 0, control.Width - 1, control.Height - 1);

            using (GraphicsPath path = GetRoundedRectanglePath(rect, borderRadius_Cha))
            using (Pen pen = new Pen(BorderColorCha, borderThickness_Cha) { Alignment = PenAlignment.Inset })
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.DrawPath(pen, path);
            }
        }

        /// <summary>
        /// hàm này sẽ đc vẽ lại path mỗi khi OnSizeChanged
        /// protected override void OnSizeChanged(EventArgs e)
        /// </summary>
        /// <param name="control"></param>
        /// <param name="borderRadius"></param>
        public static void SetRoundedRegion(Control control, int borderRadius)
        {
            int adjustedRadius = (int)(borderRadius * GetDpiScaleFactor(control));
            Rectangle rect = new Rectangle(0, 0, control.Width, control.Height);
            using (GraphicsPath path = GetRoundedRectanglePath(rect, adjustedRadius))
            {
                if (control.Region != null)
                    control.Region.Dispose();
                control.Region = new Region(path);
            }
        }

        public static void UpdateInnerPanelRegion(Control innerPanel, int borderRadius_Cha, int borderThickness_Cha)
        {
            if (innerPanel.Width <= 0 || innerPanel.Height <= 0)
                return;

            int innerRadius = Math.Max(0, borderRadius_Cha - borderThickness_Cha);
            Rectangle rect = new Rectangle(0, 0, innerPanel.Width, innerPanel.Height);
            using (GraphicsPath path = GetRoundedRectanglePath(rect, innerRadius))
            {
                if (innerPanel.Region != null)
                    innerPanel.Region.Dispose();
                innerPanel.Region = new Region(path);
            }
        }



        public static Image CreateBrightImage(Image original, float brightnessFactor)
        {
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);
            float[][] ptsArray ={
                             new float[] { brightnessFactor, 0, 0, 0, 0 },
                             new float[] { 0, brightnessFactor, 0, 0, 0 },
                             new float[] { 0, 0, brightnessFactor, 0, 0 },
                             new float[] { 0, 0, 0, 1, 0 },
                             new float[] { 0, 0, 0, 0, 1 }
                        };
            ColorMatrix cm = new ColorMatrix(ptsArray);

            using (ImageAttributes ia = new ImageAttributes())
            {
                ia.SetColorMatrix(cm);
                using (Graphics g = Graphics.FromImage(newBitmap))
                {
                    g.DrawImage(original,
                                new Rectangle(0, 0, original.Width, original.Height),
                                0, 0, original.Width, original.Height,
                                GraphicsUnit.Pixel, ia);
                }
            }
            return newBitmap;
        }

        public static Image CreateBrightImageWithOffset(Image original, float brightnessFactor, float offset)
        {
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);
            // Tạo ma trận gồm việc nhân và cộng offset cho R, G, B
            float[][] ptsArray ={
         new float[] { brightnessFactor, 0, 0, 0, offset },
         new float[] { 0, brightnessFactor, 0, 0, offset },
         new float[] { 0, 0, brightnessFactor, 0, offset },
         new float[] { 0, 0, 0, 1, 0 },
         new float[] { 0, 0, 0, 0, 1 }
    };
            ColorMatrix cm = new ColorMatrix(ptsArray);

            using (ImageAttributes ia = new ImageAttributes())
            {
                ia.SetColorMatrix(cm);
                using (Graphics g = Graphics.FromImage(newBitmap))
                {
                    g.DrawImage(original,
                                new Rectangle(0, 0, original.Width, original.Height),
                                0, 0, original.Width, original.Height,
                                GraphicsUnit.Pixel, ia);
                }
            }
            return newBitmap;
        }


    }


    public static class BorderRenderer
    {
        public static void DrawContinuousGradientBorder(Graphics graphics, Rectangle rect, int borderRadius, int borderThickness, Color[] gradientColors)
        {
            if (gradientColors == null || gradientColors.Length < 2) return;

            using (GraphicsPath path = GetRoundedRectanglePath(rect, borderRadius))
            {
                // Tạo LinearGradientBrush với hướng dọc (Vertical)
                using (LinearGradientBrush brush = new LinearGradientBrush(
                    new Point(rect.Left, rect.Top),
                    new Point(rect.Left, rect.Bottom),
                    Color.Black, Color.Black)) // Màu khởi tạo không quan trọng
                {
                    // Tạo ColorBlend để phân bố bốn màu
                    ColorBlend colorBlend = new ColorBlend
                    {
                        Colors = gradientColors,
                        Positions = new float[] { 0.0f, 0.45f, 0.76f, 1.0f },
                    };
                    brush.InterpolationColors = colorBlend;

                    // Lấp đầy toàn bộ vùng với gradient
                    graphics.FillPath(brush, path);
                }

                //// (Tùy chọn) Vẽ viền ngoài cùng để làm nổi gradient, nếu cần
                //using (Pen outlinePen = new Pen(gradientColors[0], 1)) // Màu viền ngoài cùng với độ dày nhỏ
                //{
                //    graphics.DrawPath(outlinePen, path);
                //}
            }
        }

        private static GraphicsPath GetRoundedRectanglePath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int d = radius * 2;

            path.AddArc(rect.Left, rect.Top, d, d, 180, 90); // Top-left
            path.AddArc(rect.Right - d, rect.Top, d, d, 270, 90); // Top-right
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90); // Bottom-right
            path.AddArc(rect.Left, rect.Bottom - d, d, d, 90, 90); // Bottom-left
            path.CloseFigure();

            return path;
        }

    }
}
