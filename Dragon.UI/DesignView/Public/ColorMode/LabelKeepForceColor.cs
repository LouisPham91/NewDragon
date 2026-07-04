
using Dragon.ControlHelper;
using System.ComponentModel;
using System.Drawing.Drawing2D;


namespace Dragon.DesignView.Public.ColorMode
{
    public class LabelKeepForceColor : Label, IThemeable
    {
        private string _svgString = string.Empty;
        private bool _isClicked = false;
        private Image? _clickImage = null;
        private Image? _originalImage = null;

        private Color _textColorEnter = Color.FromArgb(255, 70, 70);
        private Color _textColorLeave = Color.Black;
        private Color _lineColorLeave = Color.FromArgb(168, 168, 168);
        private int _bottomLineBorderThickness = 1;



        [Category("Appearance")]
        [DefaultValue(typeof(Color), "255, 70, 70")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_TextColorEnter
        {
            get => _textColorEnter;
            set { if (_textColorEnter == value) return; _textColorEnter = value; UpdateSvgImages(); if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(Color), "Black")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_TextColorLeave
        {
            get => _textColorLeave;
            set { if (_textColorLeave == value) return; _textColorLeave = value; UpdateSvgImages(); if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(Color), "168, 168, 168")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_LineColorLeave
        {
            get => _lineColorLeave;
            set { if (_lineColorLeave == value) return; _lineColorLeave = value; if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(1)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int DG_BottomLine_BorderThickness
        {
            get => _bottomLineBorderThickness;
            set { if (_bottomLineBorderThickness == value) return; _bottomLineBorderThickness = Math.Max(0, value); if (IsHandleCreated) Invalidate(); }
        }

        [Category("Behavior")]
        [DefaultValue(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool DG_IsClicked
        {
            get => _isClicked;
            set { if (_isClicked == value) return; _isClicked = value; DrawLineBaseOnClick(); if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue("")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string DG_SVGString
        {
            get => _svgString;
            set { value ??= string.Empty; if (_svgString == value) return; _svgString = value; UpdateSvgImages(); if (IsHandleCreated) Invalidate(); }
        }

        private void UpdateSvgImages()
        {
            if (string.IsNullOrEmpty(_svgString)) return;

            var height = Font.Height - 2;
            _clickImage = SvgRenderer.RenderSvgFromString(_svgString, height, height, _textColorEnter);
            _originalImage = SvgRenderer.RenderSvgFromString(_svgString, height, height, _textColorLeave);
            Image = _isClicked ? _clickImage : _originalImage;
        }

        public LabelKeepForceColor()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);
            UpdateStyles();

            Padding = new Padding(0);
            Margin = new Padding(0);
            ImageAlign = ContentAlignment.MiddleLeft;
            TextAlign = ContentAlignment.MiddleRight;

            
            
            ApplyTheme();
        }

        public void ApplyTheme()
        {
            if (IsDisposed) return;

            BackColor = ThemeHelper.BackNormalFirst;
            _lineColorLeave = ThemeHelper.BorderNormalFist;
            _textColorLeave = ThemeHelper.ForeNormalFirst;
            ForeColor = _textColorLeave;

            if (ThemeHelper.CurrentMode == ThemeMode.Light)
            {
                _textColorEnter = ThemeHelper.CurrentStyle switch
                {
                    ThemeStyle.ThemeRed => ThemeHelper.ForceColoful4th,
                    ThemeStyle.ThemeGreen => ThemeHelper.ForceColofulFist,
                    ThemeStyle.ThemeGold => ThemeHelper.ForceColoful2nd,
                    _ => ThemeHelper.ForceColofulFist
                };
            }
            else
            {
                _textColorEnter = ThemeHelper.CurrentStyle switch
                {
                    ThemeStyle.ThemeRed => ThemeHelper.ForceColofulFist,
                    ThemeStyle.ThemeGreen => ThemeHelper.ForceColoful2nd,
                    ThemeStyle.ThemeGold => ThemeHelper.ForceColoful3rd,
                    _ => ThemeHelper.ForceColofulFist
                };
            }

            UpdateSvgImages();
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

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (IsDisposed) return;
            base.OnMouseClick(e);
            DrawLineBaseOnClick();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (IsDisposed) return;
            base.OnPaint(e);

            if (!DesignMode)
            {
                Image = _isClicked ? _clickImage : _originalImage;
                ForeColor = _isClicked ? _textColorEnter : _textColorLeave;
            }

            int y = ClientSize.Height - _bottomLineBorderThickness;
            Color lineColor = _isClicked ? _textColorEnter : _lineColorLeave;

            using (Pen pen = new Pen(lineColor, _bottomLineBorderThickness))
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.DrawLine(pen, 0, y, Width, y);
            }
        }

        public void DrawLineBaseOnClick()
        {
            if (IsDisposed) return;

            Control? parent = Parent;
            if (parent is null) return;

            foreach (Control ctrlObj in parent.Controls)
            {
                if (ctrlObj is LabelKeepForceColor ctrl)
                {
                    ctrl._isClicked = ctrl.Name == Name;
                    ctrl.Invalidate();
                }
            }
        }
    }
}