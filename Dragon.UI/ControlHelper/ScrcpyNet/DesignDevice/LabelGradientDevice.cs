
using Dragon.DesignView.Public;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dragon.ControlHelper.ScrcpyNet.DesignDevice
{
    public class LabelGradientDevice : Label
    {
        public int borderRadius = 4;
        public int borderThickness = 1;
        public Color borderColor = Color.FromArgb(255, 71, 51);

        public Color LabelGradientDevice_StartCorlor = Color.FromArgb(255, 71, 51);
        public Color LabelGradientDevice_EnndCorlor = Color.FromArgb(32, 173, 232);

        public LabelGradientDevice()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
        }

        public void ChangeColor(int? BorderRadius = null, int? BorderThickness = null)
        {
            if (ThemeHelper.CurrentMode == ThemeMode.Light)
            {
                borderColor = ThemeHelper.BorderColofulFist;
                LabelGradientDevice_StartCorlor = ThemeHelper.GradientStart;
                LabelGradientDevice_EnndCorlor = ThemeHelper.GradientEnd;
            }
            else
            {
                borderColor = ThemeHelper.BorderColofulFist;
                LabelGradientDevice_StartCorlor = ThemeHelper.GradientStart;
                LabelGradientDevice_EnndCorlor = ThemeHelper.GradientEnd;
            }

            if (BorderRadius.HasValue)
                borderRadius = BorderRadius.Value;
            if (BorderThickness.HasValue)
                borderThickness = BorderThickness.Value;
            Invalidate();
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            BackColor = Color.Transparent;
            ForeColor = Color.White;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = ClientRectangle;

            // Tạo GraphicsPath với các góc bo tròn
            using (GraphicsPath path = GetRoundedRectanglePath(rect, borderRadius))
            {
                // Clip control theo hình bo tròn
                Region = new Region(path);

                // Xoá nền mặc định bằng màu nền của Parent (để tránh hiện nền vuông)
                // Nếu không có Parent thì dùng Color.Transparent
                e.Graphics.Clear(Parent != null ? Parent.BackColor : Color.Transparent);

                // Vẽ nền gradient (ở đây dùng hướng ngang từ xanh lục sang xanh lam)
                using (LinearGradientBrush brush = new LinearGradientBrush(
                    rect, LabelGradientDevice_StartCorlor, LabelGradientDevice_EnndCorlor, LinearGradientMode.Horizontal))
                {
                    e.Graphics.FillPath(brush, path);
                }

                // Vẽ chữ (màu trắng) ở trung tâm control
                using (SolidBrush textBrush = new SolidBrush(ForeColor))
                {
                    StringFormat sf = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };
                    e.Graphics.DrawString(Text, Font, textBrush, rect, sf);
                }
            }
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            // Không gọi base.OnPaintBackground để tránh vẽ nền mặc định (hình chữ nhật)
            // Điều này giúp chỉ hiển thị phần vẽ bên OnPaint (vùng gradient trong path)
        }

        // Tạo GraphicsPath cho hình chữ nhật với các góc bo tròn
        private GraphicsPath GetRoundedRectanglePath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;
            Rectangle arcRect = new Rectangle(rect.Location, new Size(diameter, diameter));

            // Góc trên trái
            path.AddArc(arcRect, 180, 90);
            // Góc trên phải
            arcRect.X = rect.Right - diameter;
            path.AddArc(arcRect, 270, 90);
            // Góc dưới phải
            arcRect.Y = rect.Bottom - diameter;
            path.AddArc(arcRect, 0, 90);
            // Góc dưới trái
            arcRect.X = rect.Left;
            path.AddArc(arcRect, 90, 90);

            path.CloseFigure();
            return path;
        }
    }
}
