
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Dragon.DesignView.Public.ColorMode
{
    public class ToggleWithTextControl : UserControl, IThemeable
    {
        private bool _isModeWhiteBlack = false;
        private bool _isToggled;
        private string _labelText = "Test Click";
        private float _backGroundRadius = 8;
        private Color _backgroundColor = Color.FromArgb(97, 53, 49);
        private Color _toggleColorClick = Color.FromArgb(83, 60, 61);
        private Color _toggleColorNormal = Color.FromArgb(40, 40, 40);
        private Color _knobColor = Color.FromArgb(207, 66, 51);
        private Color _labelColor = Color.FromArgb(207, 66, 51);
        private int _toggleWidth = 35;

        [Category("Appearance")]
        [DisplayName("DG_IsModeWhiteBlack")]
        [DefaultValue(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool DG_IsModeWhiteBlack
        {
            get => _isModeWhiteBlack;
            set { if (_isModeWhiteBlack == value) return; _isModeWhiteBlack = value; ApplyTheme(); if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [DisplayName("DG_BackGroundRadius")]
        [DefaultValue(8f)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public float DG_BackGroundRadius
        {
            get => _backGroundRadius;
            set { if (_backGroundRadius == value) return; _backGroundRadius = value; if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [DisplayName("DG_BackgroundColor")]
        [DefaultValue(typeof(Color), "97, 53, 49")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_BackgroundColor
        {
            get => _backgroundColor;
            set { if (_backgroundColor == value) return; _backgroundColor = value; if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [DisplayName("DG_ToggleColorClick")]
        [DefaultValue(typeof(Color), "83, 60, 61")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_ToggleColorClick
        {
            get => _toggleColorClick;
            set { if (_toggleColorClick == value) return; _toggleColorClick = value; if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [DisplayName("DG_ToggleColorNormal")]
        [DefaultValue(typeof(Color), "40, 40, 40")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_ToggleColorNormal
        {
            get => _toggleColorNormal;
            set { if (_toggleColorNormal == value) return; _toggleColorNormal = value; if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [DisplayName("DG_KnobColor")]
        [DefaultValue(typeof(Color), "207, 66, 51")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_KnobColor
        {
            get => _knobColor;
            set { if (_knobColor == value) return; _knobColor = value; if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [DisplayName("DG_LabelColor")]
        [DefaultValue(typeof(Color), "207, 66, 51")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_LabelColor
        {
            get => _labelColor;
            set { if (_labelColor == value) return; _labelColor = value; if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [DisplayName("DG_ToggleWidth")]
        [DefaultValue(35)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int DG_ToggleWidth
        {
            get => _toggleWidth;
            set { if (_toggleWidth == value) return; _toggleWidth = Math.Max(10, value); if (IsHandleCreated) Invalidate(); }
        }

        [Category("Behavior")]
        [Description("Gets or sets whether the toggle is ON")]
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

        [Category("Appearance")]
        [Description("Label text beside toggle")]
        [DefaultValue("Test Click")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string DG_LabelText
        {
            get => _labelText;
            set { value ??= ""; if (_labelText == value) return; _labelText = value; if (IsHandleCreated) Invalidate(); }
        }

        public event EventHandler? Toggled;

        public ToggleWithTextControl()
        {
            Size = new Size(150, 40);
            Cursor = Cursors.Hand;
            DoubleBuffered = true;
            SetStyle(ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);

            // 🔥 Subscribe theme đúng cách
            
            
            ApplyTheme();
        }

        public void ApplyTheme()
        {
            if (IsDisposed) return;

            if (ThemeHelper.CurrentMode == ThemeMode.Light)
            {
                if (_isModeWhiteBlack)
                {
                    _backgroundColor = ThemeHelper.BackNormalFirst;
                    _knobColor = Color.FromArgb(136, 136, 136);
                    _labelColor = ThemeHelper.ForceColofulFist;
                    _toggleColorClick = Color.FromArgb(185, 185, 185);
                    _toggleColorNormal = ThemeHelper.BackNormal3rd;
                }
                else if (ThemeHelper.CurrentStyle == ThemeStyle.ThemeRed)
                {
                    _backgroundColor = ThemeHelper.BackNormal3rd;
                    _knobColor = ThemeHelper.ForceColofulFist;
                    _labelColor = ThemeHelper.ForceColoful4th;
                    _toggleColorClick = ThemeHelper.BorderNormal3rd;
                    _toggleColorNormal = ThemeHelper.BackNormal2nd;
                }
                else if (ThemeHelper.CurrentStyle == ThemeStyle.ThemeGreen)
                {
                    _backgroundColor = ThemeHelper.BackNormal3rd;
                    _knobColor = ThemeHelper.BorderColofulFist;
                    _labelColor = ThemeHelper.ForceColofulFist;
                    _toggleColorClick = ThemeHelper.BorderNormal3rd;
                    _toggleColorNormal = ThemeHelper.BackNormalFirst;
                }
                else if (ThemeHelper.CurrentStyle == ThemeStyle.ThemeGold)
                {
                    _backgroundColor = ThemeHelper.BackNormal3rd;
                    _knobColor = ThemeHelper.ForceColoful2nd;
                    _labelColor = ThemeHelper.ForceColoful2nd;
                    _toggleColorClick = ThemeHelper.BorderNormal3rd;
                    _toggleColorNormal = ThemeHelper.BackNormalFirst;
                }
            }
            else // Dark mode
            {
                if (_isModeWhiteBlack)
                {
                    _backgroundColor = ThemeHelper.BackNormalFirst;
                    _knobColor = Color.FromArgb(245, 245, 245);
                    _labelColor = ThemeHelper.ForceColofulFist;
                    _toggleColorClick = Color.FromArgb(101, 101, 101);
                    _toggleColorNormal = Color.FromArgb(50, 50, 50);
                }
                else if (ThemeHelper.CurrentStyle == ThemeStyle.ThemeRed)
                {
                    _backgroundColor = ThemeHelper.BackColofulFirst;
                    _knobColor = ThemeHelper.ForceColofulFist;
                    _labelColor = ThemeHelper.ForceColofulFist;
                    _toggleColorClick = ThemeHelper.BorderNormal2nd;
                    _toggleColorNormal = ThemeHelper.BackNormalFirst;
                }
                else if (ThemeHelper.CurrentStyle == ThemeStyle.ThemeGreen)
                {
                    _backgroundColor = ThemeHelper.BackColofulFirst;
                    _knobColor = ThemeHelper.ForceColoful3rd;
                    _labelColor = ThemeHelper.ForceColoful2nd;
                    _toggleColorClick = ThemeHelper.BorderNormal2nd;
                    _toggleColorNormal = ThemeHelper.BackNormalFirst;
                }
                else if (ThemeHelper.CurrentStyle == ThemeStyle.ThemeGold)
                {
                    _backgroundColor = ThemeHelper.BackColofulFirst;
                    _knobColor = ThemeHelper.ForceColoful3rd;
                    _labelColor = ThemeHelper.ForceColoful3rd;
                    _toggleColorClick = ThemeHelper.BorderNormal2nd;
                    _toggleColorNormal = ThemeHelper.BackNormalFirst;
                }
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

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var toggleColor = _isToggled ? _toggleColorClick : _toggleColorNormal;

            // Draw outer container rounded
            using (Brush bg = new SolidBrush(_backgroundColor))
            using (GraphicsPath bgPath = GetRoundedRectanglePath(new Rectangle(0, 0, Width - 1, Height - 1), _backGroundRadius))
            {
                g.FillPath(bg, bgPath);
            }

            // Toggle area
            Rectangle toggleBounds = new Rectangle(5, 5, _toggleWidth, Height - 10);
            using (Brush toggleBrush = new SolidBrush(toggleColor))
            using (GraphicsPath togglePath = GetRoundedRectanglePath(toggleBounds, toggleBounds.Height / 2))
            {
                g.FillPath(toggleBrush, togglePath);
            }

            // Toggle knob
            int knobSize = toggleBounds.Height - 4;
            int knobX = _isToggled ? toggleBounds.Right - knobSize - 2 : toggleBounds.Left + 2;
            using (Brush knobBrush = new SolidBrush(_knobColor))
            {
                g.FillEllipse(knobBrush, knobX, toggleBounds.Top + 2, knobSize, knobSize);
            }

            // Draw label
            Rectangle labelRect = new Rectangle(toggleBounds.Right + 10, 0, Width - toggleBounds.Right - 10, Height);
            TextFormatFlags flags = TextFormatFlags.VerticalCenter
                                    | TextFormatFlags.Left
                                    | TextFormatFlags.SingleLine
                                    | TextFormatFlags.NoPadding
                                    | TextFormatFlags.NoClipping;

            TextRenderer.DrawText(g, _labelText, Font, labelRect, _labelColor, flags);
        }

        protected override void OnClick(EventArgs e)
        {
            if (IsDisposed) return;
            base.OnClick(e);
            DG_IsToggled = !DG_IsToggled;
        }

        private GraphicsPath GetRoundedRectanglePath(RectangleF bounds, float radius)
        {
            float maxRadius = Math.Min(bounds.Width, bounds.Height) / 2f;
            radius = Math.Min(radius, maxRadius);

            float diameter = radius * 2;
            GraphicsPath path = new GraphicsPath();

            path.AddArc(bounds.X, bounds.Y, diameter, diameter, 180, 90);
            path.AddLine(bounds.X + radius, bounds.Y, bounds.Right - radius, bounds.Y);
            path.AddArc(bounds.Right - diameter, bounds.Y, diameter, diameter, 270, 90);
            path.AddLine(bounds.Right, bounds.Y + radius, bounds.Right, bounds.Bottom - radius);
            path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddLine(bounds.Right - radius, bounds.Bottom, bounds.X + radius, bounds.Bottom);
            path.AddArc(bounds.X, bounds.Bottom - diameter, diameter, diameter, 90, 90);
            path.AddLine(bounds.X, bounds.Bottom - radius, bounds.X, bounds.Y + radius);

            path.CloseFigure();
            return path;
        }
    }
}