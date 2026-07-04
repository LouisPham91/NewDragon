// File: DoubleBufferedPanel.cs
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using static AntdUI.Input;

namespace Dragon.ControlHelper.ScrcpyNet.DesignDevice
{
    public class DoubleBufferedPanel : Panel
    {
        public DoubleBufferedPanel()
        {
            // Đây là cách DUY NHẤT để bật đầy đủ và an toàn
            DoubleBuffered = true;
            SetStyle(ControlStyles.UserPaint |
                    ControlStyles.AllPaintingInWmPaint |
                    ControlStyles.OptimizedDoubleBuffer |
                    ControlStyles.ResizeRedraw |
                    ControlStyles.Selectable, true);  // QUAN TRỌNG: để nhận focus + keyboard


            TabStop = true;          
            BackColor = Color.FromArgb(40,40,40);
        }
        public void SetRegion(int BorderRadius, int BorderThickness)
        {
            if (this != null && Width > 0 && Height > 0)
            {
                var innerRect = ClientRectangle;
                using (GraphicsPath innerPath = GetRoundedRectanglePath(innerRect, BorderRadius - BorderThickness))
                {
                    Region?.Dispose();
                    Region = new Region(innerPath);
                }
            }
        }
        protected override bool IsInputKey(Keys keyData) => true;

        public static GraphicsPath GetRoundedRectanglePath(RectangleF rect, float radius)
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
    }
}