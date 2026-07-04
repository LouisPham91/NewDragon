

using System.ComponentModel;
using Dragon.ControlHelper;

namespace Dragon.DesignView.Public.NormalMode
{
    public class PictureBoxBrightN : PictureBox, IThemeable
    {
        private Color _originalBackColor;
        private int _lightenPercent = 20;
        private Image? _svgImage = null; // chỉ giữ ảnh do SVG tạo ra
        private Color _originalImageColor = Color.White;
        private Size _imageSize = new(20, 20);
        private bool _isBrightBack = true;
        private bool _isGrayImage = false;
        private bool _isWhiteImage = false;
        private bool _isBackTransparent = false;
        private UseMode _useMode = UseMode.ThemeMode;
        private string _svgString = string.Empty;

        [Category("Dragon")]
        [DefaultValue(UseMode.ThemeMode)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public UseMode DG_UseMode
        {
            get => _useMode;
            set { if (_useMode == value) return; _useMode = value; ApplyTheme(); }
        }

        [Category("Dragon")]
        [DefaultValue("")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string DG_SVGString
        {
            get => _svgString;
            set { value ??= ""; if (_svgString == value) return; _svgString = value; UpdateSvgImage(); }
        }

        [Category("Dragon")]
        [DefaultValue(20)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int DG_LightenPercent
        {
            get => _lightenPercent;
            set { value = Math.Clamp(value, 0, 255); if (_lightenPercent == value) return; _lightenPercent = value; }
        }

        [Category("Dragon")]
        [DefaultValue(typeof(Size), "20, 20")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Size DG_ImageSize
        {
            get => _imageSize;
            set { if (_imageSize == value) return; _imageSize = value; UpdateSvgImage(); }
        }

        [Category("Dragon")]
        [DefaultValue(typeof(Color), "White")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_ImageColor
        {
            get => _originalImageColor;
            set { if (_originalImageColor == value) return; _originalImageColor = value; UpdateSvgImage(); }
        }

        [Category("Dragon")]
        [DefaultValue(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool DG_IsBrightBack
        {
            get => _isBrightBack;
            set { if (_isBrightBack == value) return; _isBrightBack = value; }
        }

        [Category("Dragon")]
        [DefaultValue(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool DG_IsGrayImage
        {
            get => _isGrayImage;
            set { if (_isGrayImage == value) return; _isGrayImage = value; ApplyTheme(); }
        }

        [Category("Dragon")]
        [DefaultValue(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool DG_IsWhiteImage
        {
            get => _isWhiteImage;
            set { if (_isWhiteImage == value) return; _isWhiteImage = value; ApplyTheme(); }
        }

        [Category("Dragon")]
        [DefaultValue(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool DG_IsBackTransparent
        {
            get => _isBackTransparent;
            set { if (_isBackTransparent == value) return; _isBackTransparent = value; ApplyTheme(); }
        }

        public PictureBoxBrightN()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            ApplyTheme();
        }

        public void ApplyTheme()
        {
            if (IsDisposed) return;

            _originalBackColor = _isBackTransparent ? Color.Transparent : ThemeHelper.BackNormalFirst;
            BackColor = _originalBackColor;
            ForeColor = ThemeHelper.ForeNormalFirst;

            // chỉ đổi màu SVG, KHÔNG đụng vào Image do user gán
            if (!string.IsNullOrWhiteSpace(_svgString))
            {
                if (_isGrayImage) DG_ImageColor = ThemeHelper.BorderNormal3rd;
                else if (_isWhiteImage) DG_ImageColor = Color.FromArgb(245, 245, 245);
                else DG_ImageColor = ForeColor;
            }
            Invalidate();
        }

        private void UpdateSvgImage()
        {
            if (IsDisposed || DesignMode) return;

            // nếu không dùng SVG thì THÔI, giữ nguyên Image user đã gán
            if (string.IsNullOrWhiteSpace(_svgString))
                return;

            var old = _svgImage;
            _svgImage = SvgRenderer.RenderSvgFromString(_svgString, _imageSize.Width, _imageSize.Height, _originalImageColor);
            Image = _svgImage; // gán ảnh mới
            old?.Dispose();    // chỉ dispose ảnh SVG cũ
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _svgImage?.Dispose();
            base.Dispose(disposing);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            if (IsDisposed || !_isBrightBack) return;
            base.OnMouseEnter(e);
            BackColor = Color.FromArgb(168, 168, 168);
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            if (IsDisposed) return;
            base.OnMouseLeave(e);
            BackColor = _originalBackColor;
        }
    }
}