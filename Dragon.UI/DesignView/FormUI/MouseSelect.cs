using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dragon.DesignView.FormUI
{
    public partial class MouseSelect : Form
    {
        public MouseSelect()
        {
            this.AutoScaleMode = AutoScaleMode.Dpi;
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.Lime;
            this.TransparencyKey = Color.Lime;
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = false;
        }

        private void MouseSelect_Load(object sender, EventArgs e)
        {

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (var path = GetRoundedRectangle8Path(ClientRectangle, 15))
            {
                Region?.Dispose();
                Region = new Region(path);
            }
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
    }
}
