

using Dragon.ControlHelper;
using System.ComponentModel;

namespace Dragon.DesignView.Public.NormalMode
{
    public class LabelNormalN : Label, IThemeable
    { 
        private string _svgString = string.Empty;
        private bool _isBrightBack = false;
        private bool _isGrayImage = false;
        private bool _isColorMode = false;  // 🔥 THÊM: IsColorMode
        private Color _originalBackColor = Color.Transparent;
        private Color _imageColor = Color.Transparent;
        private int _lightenPercent = 80;

        // 🔥 Theme handler


        [Category("Dragon")]
        [DisplayName("DG_SVGString")]
        [DefaultValue("")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string DG_SVGString
        {
            get => _svgString;
            set
            {
                value ??= string.Empty;
                if (_svgString == value) return;
                _svgString = value;
                UpdateLabelSVGString();
            }
        }

        [Category("Dragon")]
        [DisplayName("DG_IsBrightBack")]
        [DefaultValue(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool DG_IsBrightBack
        {
            get => _isBrightBack;
            set { if (_isBrightBack == value) return; _isBrightBack = value; }
        }

        [Category("Dragon")]
        [DisplayName("DG_IsGrayImage")]
        [DefaultValue(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool DG_IsGrayImage
        {
            get => _isGrayImage;
            set { if (_isGrayImage == value) return; _isGrayImage = value; ApplyTheme(); }
        }

        [Category("Dragon")]
        [DisplayName("DG_IsColorMode")]
        [DefaultValue(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool DG_IsColorMode
        {
            get => _isColorMode;
            set { if (_isColorMode == value) return; _isColorMode = value; ApplyTheme(); }
        }

        [Category("Dragon")]
        [DisplayName("DG_BackColor")]
        [DefaultValue(typeof(Color), "Transparent")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_BackColor
        {
            get => _originalBackColor;
            set
            {
                if (_originalBackColor == value) return;
                _originalBackColor = value;
                BackColor = value;
            }
        }

        [Category("Dragon")]
        [DisplayName("DG_SVGImageColor")]
        [DefaultValue(typeof(Color), "Transparent")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_SVGImageColor
        {
            get => _imageColor;
            set
            {
                if (_imageColor == value) return;
                _imageColor = value;
                UpdateLabelSVGString(_imageColor);
            }
        }

        [Category("Dragon")]
        [DisplayName("DG_LightenPercent")]
        [DefaultValue(80)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int DG_LightenPercent
        {
            get => _lightenPercent;
            set
            {
                value = Math.Clamp(value, 0, 255);
                if (_lightenPercent == value) return;
                _lightenPercent = value;
            }
        }

        public LabelNormalN()
        {
            // 🔥 Khởi tạo ngay để tránh warning CS8618
            
            
            ApplyTheme();
        }

        private void UpdateLabelSVGString(Color? color = null)
        {
            if (IsDisposed || string.IsNullOrEmpty(_svgString)) return;

            var height = Font.Height - 2;
            Image = SvgRenderer.RenderSvgFromString(_svgString, height, height, color ?? ForeColor);
        }

        public void ApplyTheme()
        {
            if (IsDisposed) return;

            BackColor = Color.Transparent;

            // 🔥 THÊM: Xử lý IsColorMode
            if (_isColorMode)
            {
                ForeColor = ThemeHelper.ForceColofulFist;
            }
            else
            {
                ForeColor = ThemeHelper.ForeNormalFirst;
            }

            if (_isGrayImage)
            {
                UpdateLabelSVGString(ThemeHelper.BorderNormal3rd);
                ForeColor = ThemeHelper.BorderNormal3rd;
            }
            else
            {
                UpdateLabelSVGString();
            }

            Invalidate();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                
            base.Dispose(disposing);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            if (IsDisposed || !_isBrightBack) return;
            base.OnMouseEnter(e);

            if (ThemeHelper.CurrentMode == ThemeMode.Light)
            {
                BackColor = ControlPaint.Light(ForeColor, _lightenPercent / 255f);
            }
            else
            {
                BackColor = ControlPaint.Dark(ForeColor, _lightenPercent / 255f);
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (IsDisposed || !_isBrightBack) return;
            base.OnMouseLeave(e);
            BackColor = _originalBackColor;
        }
    }
}