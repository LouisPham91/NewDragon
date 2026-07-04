
using System.ComponentModel;
using System.Drawing.Drawing2D;
namespace Dragon.DesignView.Public.ColorMode
{
    public class PanelRoundCustomC : Panel, IThemeable
    {
        private bool _isBackGroundGradient = false;
        private Color _backGroundColor1 = Color.FromArgb(31, 31, 31);
        private Color _backGroundColor2 = Color.FromArgb(31, 31, 31);
        private LinearGradientMode _backGroundLinearGradientMode = LinearGradientMode.Vertical;

        private float _borderRadius = 15;
        private Color _borderColor = Color.FromArgb(108, 108, 108);
        private int _borderThickness = 2;
        private LineBorder _borderLine = LineBorder.Top;
        private PenAlignment _borderAlignment = PenAlignment.Inset;

        private bool _addLineBool = false;
        private LineBorder _addLine = LineBorder.Bottom;
        private Color _addLineColor = Color.Red;
        private PenAlignment _addLineAlignment = PenAlignment.Inset;
        [Category("Appearance")]
        [DefaultValue(typeof(LinearGradientMode), "Vertical")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public LinearGradientMode DG_BackGroundLinearGradientMode
        {
            get => _backGroundLinearGradientMode;
            set { if (_backGroundLinearGradientMode == value) return; _backGroundLinearGradientMode = value; DrawRegionPath(); if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool DG_BackGroundGradientBool
        {
            get => _isBackGroundGradient;
            set { if (_isBackGroundGradient == value) return; _isBackGroundGradient = value; DrawRegionPath(); if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(Color), "31, 31, 31")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_BackGroundColor1
        {
            get => _backGroundColor1;
            set { if (_backGroundColor1 == value) return; _backGroundColor1 = value; DrawRegionPath(); if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(Color), "31, 31, 31")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_BackGroundColor2
        {
            get => _backGroundColor2;
            set { if (_backGroundColor2 == value) return; _backGroundColor2 = value; DrawRegionPath(); if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(15f)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public float DG_Radius
        {
            get => _borderRadius;
            set { if (_borderRadius == value) return; _borderRadius = Math.Max(0, value); DrawRegionPath(); if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(Color), "108, 108, 108")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_BorderColor
        {
            get => _borderColor;
            set { if (_borderColor == value) return; _borderColor = value; DrawRegionPath(); if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(2)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int DG_BorderThickness
        {
            get => _borderThickness;
            set { if (_borderThickness == value) return; _borderThickness = Math.Max(0, value); Padding = new Padding(_borderThickness); DrawRegionPath(); if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(LineBorder), "Top")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public LineBorder DG_BorderLine
        {
            get => _borderLine;
            set { if (_borderLine == value) return; _borderLine = value; DrawRegionPath(); if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(PenAlignment), "Inset")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public PenAlignment DG_BorderAlignment
        {
            get => _borderAlignment;
            set { if (_borderAlignment == value) return; _borderAlignment = value; DrawRegionPath(); if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(LineBorder), "Bottom")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public LineBorder DG_AddLine
        {
            get => _addLine;
            set { if (_addLine == value) return; _addLine = value; DrawRegionPath(); if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(Color), "Red")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_AddLineColor
        {
            get => _addLineColor;
            set { if (_addLineColor == value) return; _addLineColor = value; DrawRegionPath(); if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(PenAlignment), "Inset")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public PenAlignment DG_AddLineAlignment
        {
            get => _addLineAlignment;
            set { if (_addLineAlignment == value) return; _addLineAlignment = value; DrawRegionPath(); if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool DG_AddLineBool
        {
            get => _addLineBool;
            set { if (_addLineBool == value) return; _addLineBool = value; DrawRegionPath(); if (IsHandleCreated) Invalidate(); }
        }

        public PanelRoundCustomC()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);
            UpdateStyles();
            Margin = new Padding(0);

            
            
            ApplyTheme();
        }

        public void ApplyTheme()
        {
            if (IsDisposed) return;

            BackColor = ThemeHelper.BackNormalFirst;
            _borderColor = ThemeHelper.BorderColofulFist;
            _addLineColor = ThemeHelper.ForceColofulFist;

            if (_isBackGroundGradient)
            {
                _backGroundColor1 = ThemeHelper.GradientStart;
                _backGroundColor2 = ThemeHelper.GradientEnd;
            }

            DrawRegionPath();
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

            e.Graphics.Clear(BackColor);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (var path = GetPartialRoundedRect(ClientRectangle, _borderRadius, _borderLine, _borderThickness))
            {
                if (_isBackGroundGradient)
                {
                    using (var brush = new LinearGradientBrush(ClientRectangle, _backGroundColor1, _backGroundColor2, _backGroundLinearGradientMode))
                    {
                        e.Graphics.FillPath(brush, path);
                    }
                }

                using (Pen pen = new Pen(_borderColor, _borderThickness) { Alignment = _borderAlignment })
                {
                    e.Graphics.DrawPath(pen, path);
                }
            }

            if (!_addLineBool) return;

            using (Pen pen = new Pen(_addLineColor, _borderThickness) { Alignment = _addLineAlignment })
            {
                switch (_addLine)
                {
                    case LineBorder.Top:
                        e.Graphics.DrawLine(pen, _borderThickness, _borderThickness, ClientSize.Width - _borderThickness, _borderThickness);
                        break;
                    case LineBorder.Left:
                        e.Graphics.DrawLine(pen, _borderThickness, _borderThickness, _borderThickness, ClientSize.Height - _borderThickness);
                        break;
                    case LineBorder.Right:
                        e.Graphics.DrawLine(pen, ClientSize.Width - _borderThickness, _borderThickness, ClientSize.Width - _borderThickness, ClientSize.Height - _borderThickness);
                        break;
                    case LineBorder.Bottom:
                        e.Graphics.DrawLine(pen, _borderThickness, ClientSize.Height - _borderThickness, ClientSize.Width - _borderThickness, ClientSize.Height - _borderThickness);
                        break;
                }
            }
        }

        public void DrawRegionPath()
        {
            if (IsDisposed) return;

            using (var path = GetPartialRoundedRect(ClientRectangle, _borderRadius, _borderLine, _borderThickness))
            {
                Region?.Dispose();
                Region = new Region(path);
            }
            Margin = new Padding(0);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            if (IsDisposed) return;
            base.OnSizeChanged(e);
            DrawRegionPath();
            Invalidate();
        }

        private GraphicsPath GetPartialRoundedRect(RectangleF rect, float radius, LineBorder border, int borderThickness)
        {
            var path = new GraphicsPath();
            float d = radius * 2f;

            if (radius <= 0 || borderThickness <= 0)
            {
                path.AddRectangle(rect);
                return path;
            }

            if (border == LineBorder.Top)
            {
                path.AddArc(rect.X, rect.Y, d, d, 180, 90);
                path.AddLine(rect.X + radius, rect.Y, rect.Right - radius, rect.Y);
                path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
                path.AddLine(rect.Right, rect.Y + radius, rect.Right, rect.Bottom - radius);
                path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
                path.AddLine(rect.Right - radius, rect.Bottom, rect.X + radius, rect.Bottom);
                path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
                path.AddLine(rect.X, rect.Bottom - radius, rect.X, rect.Y + radius);
                path.CloseFigure();
            }
            else if (border == LineBorder.Bottom)
            {
                path.AddLine(rect.X, rect.Y, rect.Right, rect.Y);
                path.AddLine(rect.Right, rect.Y, rect.Right, rect.Bottom - radius);
                path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
                path.AddLine(rect.Right - radius, rect.Bottom, rect.X + radius, rect.Bottom);
                path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
                path.CloseFigure();
            }
            else if (border == LineBorder.Left)
            {
                path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
                path.AddLine(rect.X, rect.Bottom - radius, rect.X, rect.Y + radius);
                path.AddArc(rect.X, rect.Y, d, d, 180, 90);
                path.AddLine(rect.X + radius, rect.Y, rect.Right, rect.Y);
                path.AddLine(rect.Right, rect.Y, rect.Right, rect.Bottom);
                path.AddLine(rect.Right, rect.Bottom, rect.X + radius, rect.Bottom);
                path.CloseFigure();
            }
            else // LineBorder.Right
            {
                path.AddLine(rect.X, rect.Y, rect.Right - radius, rect.Y);
                path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
                path.AddLine(rect.Right, rect.Y + radius, rect.Right, rect.Bottom - radius);
                path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
                path.AddLine(rect.Right - radius, rect.Bottom, rect.X, rect.Bottom);
                path.AddLine(rect.X, rect.Bottom, rect.X, rect.Y);
                path.CloseFigure();
            }

            return path;
        }
    }
}