
using Dragon.ControlHelper;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Dragon.DesignView.Public.ColorMode
{
    public class LabelKeepLeaveC : Label, IThemeable
    {
        private Color _textColorLeave = Color.FromArgb(240, 240, 240);
        private Color _textColorEnter = Color.FromArgb(207, 66, 51);
        private int _bottomLineBorderThickness = 2;
        private Color _bottomLineColorLeave = Color.FromArgb(240, 240, 240);
        private Color _bottomLineColorEnter = Color.FromArgb(207, 66, 51);

        private bool _isClicked = false;
        private string _svgString = string.Empty; // <-- đổi
        private Size _imageSize = new Size(16, 16);
        private Image? _originalImage;
        private Image? _clickImage;



        [Category("Appearance")]
        [DefaultValue(typeof(Color), "240, 240, 240")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_TextColorLeave
        {
            get => _textColorLeave;
            set { if (_textColorLeave == value) return; _textColorLeave = value; UpdateImages(); if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(Color), "207, 66, 51")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_TextColorEnter
        {
            get => _textColorEnter;
            set { if (_textColorEnter == value) return; _textColorEnter = value; UpdateImages(); if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(2)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int DG_BottomLine_BorderThickness
        {
            get => _bottomLineBorderThickness;
            set { if (_bottomLineBorderThickness == value) return; _bottomLineBorderThickness = Math.Max(0, value); if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(Color), "240, 240, 240")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_BottomLine_BorderColor_Leave
        {
            get => _bottomLineColorLeave;
            set { if (_bottomLineColorLeave == value) return; _bottomLineColorLeave = value; if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(Color), "207, 66, 51")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_BottomLine_BorderColor_Enter
        {
            get => _bottomLineColorEnter;
            set { if (_bottomLineColorEnter == value) return; _bottomLineColorEnter = value; if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool DG_IsClicked
        {
            get => _isClicked;
            set { if (_isClicked == value) return; _isClicked = value; DrawLineBaseOnClick(); if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [Description("SVG content trực tiếp")]
        [DefaultValue("")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string DG_SVGString
        {
            get => _svgString;
            set { value ??= string.Empty; if (_svgString == value) return; _svgString = value; UpdateImages(); if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(Size), "16, 16")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Size DG_ImageSize
        {
            get => _imageSize;
            set { if (_imageSize == value) return; _imageSize = value; UpdateImages(); if (IsHandleCreated) Invalidate(); }
        }
        private void UpdateImages()
        {
            // dispose ảnh cũ
            _originalImage?.Dispose();
            _clickImage?.Dispose();
            _originalImage = null;
            _clickImage = null;

            if (string.IsNullOrWhiteSpace(_svgString)) { Image = null; return; }

            _originalImage = SvgRenderer.RenderSvgFromString(_svgString, _imageSize.Width, _imageSize.Height, _textColorLeave);
            _clickImage = SvgRenderer.RenderSvgFromString(_svgString, _imageSize.Width, _imageSize.Height, _textColorEnter);
            Image = _isClicked ? _clickImage : _originalImage;
        }

        public LabelKeepLeaveC()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint |  ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            UpdateStyles();

            Padding = new Padding(0);
            Margin = new Padding(0);
            ImageAlign = ContentAlignment.MiddleLeft;
            TextAlign = ContentAlignment.MiddleCenter;

            var height = Font.Height;
            _imageSize = new Size(height, height);

            
            
            ApplyTheme();
        }

        public void ApplyTheme()
        {
            if (IsDisposed) return;

            BackColor = ThemeHelper.BackNormalFirst;
            _textColorLeave = ThemeHelper.ForeNormalFirst;
            _bottomLineColorLeave = ThemeHelper.BorderNormalFist;

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
            _bottomLineColorEnter = _textColorEnter;

            ForeColor = _isClicked ? _textColorEnter : _textColorLeave;
            UpdateImages();
            Invalidate();
        }

        protected override void OnHandleDestroyed(EventArgs e) {  base.OnHandleDestroyed(e); }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                
                _originalImage?.Dispose();
                _clickImage?.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            if (IsDisposed) return; base.OnMouseEnter(e);
            if (!_isClicked) { ForeColor = _textColorEnter; Image = _clickImage; Invalidate(); }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (IsDisposed) return; base.OnMouseLeave(e);
            if (!_isClicked) { ForeColor = _textColorLeave; Image = _originalImage; Invalidate(); }
        }

        protected override void OnMouseClick(MouseEventArgs e) { if (IsDisposed) return; base.OnMouseClick(e); DrawLineBaseOnClick(); }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (IsDisposed) return; base.OnPaint(e);

            if (!DesignMode)
            {
                Image = _isClicked ? _clickImage : _originalImage;
                ForeColor = _isClicked ? _textColorEnter : _textColorLeave;
            }

            int y = Height - 1;
            Color lineColor = _isClicked ? _bottomLineColorEnter : _bottomLineColorLeave;
            using Pen pen = new(lineColor, _bottomLineBorderThickness);
            e.Graphics.SmoothingMode = SmoothingMode.None;
            e.Graphics.DrawLine(pen, 0, y, Width, y);
        }

        public void DrawLineBaseOnClick()
        {
            if (IsDisposed || Parent == null) return;
            foreach (Control ctrl in Parent.Controls)
            {
                if (ctrl is LabelKeepLeaveC lbl)
                {
                    lbl._isClicked = lbl.Name == Name;
                    lbl.ForeColor = lbl._isClicked ? lbl._textColorEnter : lbl._textColorLeave;
                    lbl.Image = lbl._isClicked ? lbl._clickImage : lbl._originalImage;
                    lbl.Invalidate();
                }
            }
        }
    }
}