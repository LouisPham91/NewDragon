
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Dragon.DesignView.Public.ColorMode
{
    public class PanelLineBorderNC : Panel, IThemeable
    {
        private bool _isUseBackNormal = false;
        private bool _isSameLineColor = false;

        private bool _drawLeft = false;
        private Color _drawLeftColor = Color.FromArgb(61, 61, 61);
        private bool _drawRight = false;
        private Color _drawRightColor = Color.FromArgb(61, 61, 61);
        private bool _drawTop = false;
        private Color _drawTopColor = Color.FromArgb(61, 61, 61);
        private bool _drawBottom = false;
        private Color _drawBottomColor = Color.FromArgb(61, 61, 61);

        private int _borderThickness = 2;
        private PenAlignment _borderAlignment = PenAlignment.Inset;

        private bool _isBackGroundGradient = false;
        private Color _backGroundColor1 = Color.FromArgb(31, 31, 31);
        private Color _backGroundColor2 = Color.FromArgb(31, 31, 31);
        private LinearGradientMode _backGroundLinearGradientMode = LinearGradientMode.Vertical;


        [Category("Appearance")]
        [DefaultValue(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool DG_IsUseBackNormal
        {
            get => _isUseBackNormal;
            set { if (_isUseBackNormal == value) return; _isUseBackNormal = value; if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool DG_IsSameLineColor
        {
            get => _isSameLineColor;
            set { if (_isSameLineColor == value) return; _isSameLineColor = value; if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool DG_DrawLeft
        {
            get => _drawLeft;
            set { if (_drawLeft == value) return; _drawLeft = value; if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(Color), "61, 61, 61")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_DrawLeftColor
        {
            get => _drawLeftColor;
            set { if (_drawLeftColor == value) return; _drawLeftColor = value; if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool DG_DrawRight
        {
            get => _drawRight;
            set { if (_drawRight == value) return; _drawRight = value; if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(Color), "61, 61, 61")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_DrawRightColor
        {
            get => _drawRightColor;
            set { if (_drawRightColor == value) return; _drawRightColor = value; if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool DG_DrawTop
        {
            get => _drawTop;
            set { if (_drawTop == value) return; _drawTop = value; if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(Color), "61, 61, 61")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_DrawTopColor
        {
            get => _drawTopColor;
            set { if (_drawTopColor == value) return; _drawTopColor = value; if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool DG_DrawBottom
        {
            get => _drawBottom;
            set { if (_drawBottom == value) return; _drawBottom = value; if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(Color), "61, 61, 61")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_DrawBottomColor
        {
            get => _drawBottomColor;
            set { if (_drawBottomColor == value) return; _drawBottomColor = value; if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(2)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int DG_BorderThickness
        {
            get => _borderThickness;
            set { if (_borderThickness == value) return; _borderThickness = Math.Max(0, value); Padding = new Padding(_borderThickness); if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(PenAlignment), "Inset")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public PenAlignment DG_BorderAlignment
        {
            get => _borderAlignment;
            set { if (_borderAlignment == value) return; _borderAlignment = value; if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool DG_BackGroundGradientBool
        {
            get => _isBackGroundGradient;
            set { if (_isBackGroundGradient == value) return; _isBackGroundGradient = value; if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(Color), "31, 31, 31")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_BackGroundColor1
        {
            get => _backGroundColor1;
            set { if (_backGroundColor1 == value) return; _backGroundColor1 = value; if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(Color), "31, 31, 31")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_BackGroundColor2
        {
            get => _backGroundColor2;
            set { if (_backGroundColor2 == value) return; _backGroundColor2 = value; if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(LinearGradientMode), "Vertical")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public LinearGradientMode DG_BackGroundLinearGradientMode
        {
            get => _backGroundLinearGradientMode;
            set { if (_backGroundLinearGradientMode == value) return; _backGroundLinearGradientMode = value; if (IsHandleCreated) Invalidate(); }
        }

        public PanelLineBorderNC()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);
            UpdateStyles();

            
            
            ApplyTheme();
        }

        public void ApplyTheme()
        {
            if (IsDisposed) return;

            if (_isSameLineColor)
            {
                _drawLeftColor = ThemeHelper.BorderNormalFist;
                _drawRightColor = ThemeHelper.BorderNormalFist;
                _drawTopColor = ThemeHelper.BorderNormalFist;
                _drawBottomColor = ThemeHelper.BorderNormalFist;
            }

            if (_isUseBackNormal)
            {
                BackColor = ThemeHelper.BackNormalFirst;
                ForeColor = ThemeHelper.ForeNormalFirst;
            }

            if (_isBackGroundGradient)
            {
                _backGroundColor1 = ThemeHelper.GradientStart;
                _backGroundColor2 = ThemeHelper.GradientEnd;
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

            e.Graphics.Clear(BackColor);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            if (_isBackGroundGradient && Width > 0 && Height > 0)
            {
                using (var brush = new LinearGradientBrush(ClientRectangle, _backGroundColor1, _backGroundColor2, _backGroundLinearGradientMode))
                {
                    e.Graphics.FillRectangle(brush, ClientRectangle);
                }
            }

            if (_drawLeft) DrawBorder(e, LineBorder.Left, _drawLeftColor);
            if (_drawRight) DrawBorder(e, LineBorder.Right, _drawRightColor);
            if (_drawTop) DrawBorder(e, LineBorder.Top, _drawTopColor);
            if (_drawBottom) DrawBorder(e, LineBorder.Bottom, _drawBottomColor);
        }

        private void DrawBorder(PaintEventArgs e, LineBorder borderType, Color borderColor)
        {
            int w = ClientSize.Width;
            int h = ClientSize.Height;

            using (var pen = new Pen(borderColor, _borderThickness) { Alignment = _borderAlignment })
            {
                switch (borderType)
                {
                    case LineBorder.Top:
                        e.Graphics.DrawLine(pen, 0, 0, w, 0);
                        break;
                    case LineBorder.Bottom:
                        e.Graphics.DrawLine(pen, 0, h, w, h);
                        break;
                    case LineBorder.Left:
                        e.Graphics.DrawLine(pen, 0, 0, 0, h);
                        break;
                    case LineBorder.Right:
                        e.Graphics.DrawLine(pen, w, 0, w, h);
                        break;
                }
            }
        }
    }
}