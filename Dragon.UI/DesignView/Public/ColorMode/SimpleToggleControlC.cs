
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Dragon.DesignView.Public.ColorMode
{
    public class SimpleToggleControlC : UserControl, IThemeable
    {
        private bool _isToggled;
        private float _backGroundRadius = 8f;
        private Color _toggleColorNormal = Color.FromArgb(40, 40, 40);
        private Color _toggleColorClick = Color.FromArgb(83, 60, 61);
        private Color _knobColor = Color.FromArgb(207, 66, 51);



        [Category("Appearance")]
        [DefaultValue(8f)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public float DG_BackGroundRadius
        {
            get => _backGroundRadius;
            set { if (_backGroundRadius == value) return; _backGroundRadius = value; if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(Color), "40, 40, 40")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_ToggleColorNormal
        {
            get => _toggleColorNormal;
            set { if (_toggleColorNormal == value) return; _toggleColorNormal = value; if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(Color), "83, 60, 61")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_ToggleColorClick
        {
            get => _toggleColorClick;
            set { if (_toggleColorClick == value) return; _toggleColorClick = value; if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(Color), "207, 66, 51")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_KnobColor
        {
            get => _knobColor;
            set { if (_knobColor == value) return; _knobColor = value; if (IsHandleCreated) Invalidate(); }
        }

        [Category("Behavior")]
        [DefaultValue(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool DG_IsToggled
        {
            get => _isToggled;
            set
            {
                if (_isToggled == value) return;
                _isToggled = value;
                if (IsHandleCreated) Invalidate();
                Toggled?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler? Toggled;

        public SimpleToggleControlC()
        {
            DoubleBuffered = true;
            Cursor = Cursors.Hand;
            BackColor = Color.Transparent;
            Size = new Size(60, 30);

            
            
            ApplyTheme();
        }

        public void ApplyTheme()
        {
            if (IsDisposed) return;

            if (ThemeHelper.CurrentMode == ThemeMode.Light)
            {
                _toggleColorNormal = ThemeHelper.BackNormal3rd;
                _toggleColorClick = ThemeHelper.BorderNormal3rd;
                _knobColor = ThemeHelper.CurrentStyle switch
                {
                    ThemeStyle.ThemeRed => ThemeHelper.ForceColoful4th,
                    ThemeStyle.ThemeGreen => ThemeHelper.ForceColofulFist,
                    ThemeStyle.ThemeGold => ThemeHelper.ForceColoful2nd,
                    _ => ThemeHelper.ForceColofulFist
                };
            }
            else
            {
                _toggleColorNormal = ThemeHelper.BackNormalFirst;
                _toggleColorClick = ThemeHelper.BorderNormal2nd;
                _knobColor = ThemeHelper.CurrentStyle switch
                {
                    ThemeStyle.ThemeRed => ThemeHelper.ForceColofulFist,
                    ThemeStyle.ThemeGreen => ThemeHelper.ForceColoful2nd,
                    ThemeStyle.ThemeGold => ThemeHelper.ForceColoful3rd,
                    _ => ThemeHelper.ForceColofulFist
                };
            }

            Invalidate();
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            
            base.OnHandleDestroyed(e);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                
            base.Dispose(disposing);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (IsDisposed) return;

            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var trackRect = new RectangleF(0, Height * 0.25f, Width, Height * 0.5f);

            using (var brush = new SolidBrush(_isToggled ? _toggleColorClick : _toggleColorNormal))
            using (var path = GetRoundedRectanglePath(trackRect, _backGroundRadius))
            {
                g.FillPath(brush, path);
            }

            float diameter = trackRect.Height - 4;
            float knobX = _isToggled ? trackRect.Right - diameter - 2 : trackRect.X + 2;
            float knobY = trackRect.Y + 2;

            using (var brush = new SolidBrush(_knobColor))
            {
                g.FillEllipse(brush, knobX, knobY, diameter, diameter);
            }
        }

        protected override void OnClick(EventArgs e)
        {
            if (IsDisposed) return;
            base.OnClick(e);
            DG_IsToggled = !DG_IsToggled;
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            if (IsDisposed) return;
            base.OnSizeChanged(e);
            Invalidate();
        }

        private GraphicsPath GetRoundedRectanglePath(RectangleF r, float radius)
        {
            float maxR = Math.Min(r.Width, r.Height) / 2;
            radius = Math.Min(radius, maxR);
            float dia = radius * 2;

            var path = new GraphicsPath();
            path.AddArc(r.X, r.Y, dia, dia, 180, 90);
            path.AddArc(r.Right - dia, r.Y, dia, dia, 270, 90);
            path.AddArc(r.Right - dia, r.Bottom - dia, dia, dia, 0, 90);
            path.AddArc(r.X, r.Bottom - dia, dia, dia, 90, 90);
            path.CloseFigure();

            return path;
        }
    }
}