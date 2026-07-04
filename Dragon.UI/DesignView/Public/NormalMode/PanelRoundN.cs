
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Dragon.DesignView.Public.NormalMode
{
    public class PanelRoundN : Panel, IThemeable
    {
        private float _borderRadius = 15f;

        [Category("Appearance")]
        [DisplayName("GD_Radius")]
        [Description("Bán kính bo góc")]
        [DefaultValue(0f)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public float GD_Radius
        {
            get => _borderRadius;
            set
            {
                if (_borderRadius == value) return;
                _borderRadius = Math.Max(0, value);
                UpdateRegion();
            }
        }

        public PanelRoundN()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);
            UpdateStyles();
            
             // dùng field
            ApplyTheme();
        }
        public void ApplyTheme()
        {
            if (IsDisposed) return;

            // NormalMode: CHỈ dùng màu cơ bản
            BackColor = ThemeHelper.BackNormalFirst;
            ForeColor = ThemeHelper.ForeNormalFirst;

            if (Name == "panelLeftMainBack") BackColor = ThemeHelper.BorderNormalFist;
            if (Name == "panelMenu") BackColor = ThemeHelper.BackNormalFirst;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            UpdateRegion();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            UpdateRegion();
        }

        public void ChangeColor(Color? backColor = null)
        {
            if (backColor.HasValue)
                BackColor = backColor.Value;
            UpdateRegion();
        }

        public void UpdateRegion()
        {
            using (var path = GetRoundedRectanglePath(ClientRectangle, _borderRadius))
            {
                var newRegion = new Region(path);
                var oldRegion = Region;
                Region = newRegion;
                oldRegion?.Dispose();
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
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            base.Dispose(disposing);
        }
    }
}