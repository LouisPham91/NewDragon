
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Dragon.DesignView.Public.ColorMode
{
    public class PanelRoundedWithBorder : Panel
    {
        private int _borderRadius = 20;
        private Color _borderColor = Color.Gray;
        private int _borderThickness = 2;
        private DashStyle _borderDashStyle = DashStyle.Solid;

        [Category("Dragon")]
        [DefaultValue(20)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int DG_BorderRadius
        {
            get => _borderRadius;
            set { if (_borderRadius == value) return; _borderRadius = Math.Max(0, value); UpdateRegion(); if (IsHandleCreated) Invalidate(); }
        }

        [Category("Dragon")]
        [DefaultValue(typeof(Color), "Gray")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_BorderColor
        {
            get => _borderColor;
            set { if (_borderColor == value) return; _borderColor = value; if (IsHandleCreated) Invalidate(); }
        }

        [Category("Dragon")]
        [DefaultValue(2)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int DG_BorderThickness
        {
            get => _borderThickness;
            set { if (_borderThickness == value) return; _borderThickness = Math.Max(0, value); UpdateRegion(); if (IsHandleCreated) Invalidate(); }
        }

        [Category("Dragon")]
        [DefaultValue(typeof(DashStyle), "Solid")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public DashStyle DG_BorderDashStyle
        {
            get => _borderDashStyle;
            set { if (_borderDashStyle == value) return; _borderDashStyle = value; if (IsHandleCreated) Invalidate(); }
        }

        public PanelRoundedWithBorder()
        {
            DoubleBuffered = true;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateRegion();
        }

        private void UpdateRegion()
        {
            using var path = GetRoundedRectanglePath(ClientRectangle, _borderRadius);
            var old = Region;
            Region = new Region(path);
            old?.Dispose();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = ClientRectangle;

            using (GraphicsPath path = GetRoundedRectanglePath(rect, _borderRadius))
            using (Pen pen = new Pen(_borderColor, _borderThickness))
            using (SolidBrush brush = new SolidBrush(BackColor))
            {
                pen.Alignment = PenAlignment.Inset;
                pen.DashStyle = _borderDashStyle;

                g.FillPath(brush, path);
                g.DrawPath(pen, path);
            }
        }

        private GraphicsPath GetRoundedRectanglePath(RectangleF rect, float radius)
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