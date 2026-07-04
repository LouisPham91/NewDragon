
using System.ComponentModel;
using Dragon.ControlHelper;

namespace Dragon.DesignView.Public.ColorMode
{

    public class PictureBoxBrightC : PictureBox, IThemeable
    {
        private Color? _brightFormColor = Color.FromArgb(200, 200, 200);
        private Color? _brightPanelColor = Color.FromArgb(207, 66, 51);
        private Image? _originalImage = null;
        private string _svgString = string.Empty;
        private Size _imageSize = new(24, 24);
        private bool _isFormPanel = true;



        [Category("Appearance")]
        [DisplayName("DG_NormalBrightnessFactor")]
        [DefaultValue(1f)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public float DG_NormalBrightnessFactor
        {
            get; set;
        } = 1.0f;

        [Category("Appearance")]
        [DisplayName("DG_NormalOffset")]
        [DefaultValue(0f)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public float DG_NormalOffset
        {
            get; set;
        } = 0f;

        [Category("Appearance")]
        [DisplayName("DG_BrightBrightnessFactor")]
        [DefaultValue(2f)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public float DG_BrightBrightnessFactor
        {
            get; set;
        } = 2.0f;

        [Category("Appearance")]
        [DisplayName("DG_BrightOffset")]
        [DefaultValue(20f)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public float DG_BrightOffset
        {
            get; set;
        } = 20f;

        [Category("Appearance")]
        [Description("SVG content trực tiếp")]
        [DefaultValue("")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string DG_SVGString
        {
            get => _svgString;
            set { value ??= string.Empty; if (_svgString == value) return; _svgString = value; UpdateImage(); }
        }

        [Category("Appearance")]
        [DisplayName("DG_ImageSize")]
        [DefaultValue(typeof(Size), "24, 24")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Size DG_ImageSize
        {
            get => _imageSize;
            set { if (_imageSize == value) return; _imageSize = value; UpdateImage(); }
        }

        [Category("Appearance")]
        [DisplayName("DG_IsFormPanel")]
        [DefaultValue(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool DG_IsFormPanel
        {
            get => _isFormPanel;
            set { if (_isFormPanel == value) return; _isFormPanel = value; UpdateImage(); }
        }

        [Category("Appearance")]
        [DisplayName("DG_BrightFormColor")]
        [DefaultValue(typeof(Color), "200, 200, 200")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color? DG_BrightFormColor
        {
            get => _brightFormColor;
            set { if (_brightFormColor == value) return; _brightFormColor = value; UpdateImage(); }
        }

        [Category("Appearance")]
        [DisplayName("DG_BrightPanelColor")]
        [DefaultValue(typeof(Color), "207, 66, 51")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color? DG_BrightPanelColor
        {
            get => _brightPanelColor;
            set { if (_brightPanelColor == value) return; _brightPanelColor = value; UpdateImage(); }
        }

        public PictureBoxBrightC()
        {
            BackColor = Color.Transparent;
            DoubleBuffered = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();

            
            
            ApplyTheme();
        }

        public void ApplyTheme()
        {
            if (IsDisposed) return;
            _brightFormColor = ThemeHelper.ForeNormalFirst;
            _brightPanelColor = ThemeHelper.ForceColofulFist;
            UpdateImage();
        }

        private void UpdateImage()
        {
            if (IsDisposed) return;

            // dispose ảnh cũ
            var old = _originalImage;
            Image?.Dispose();

            if (string.IsNullOrWhiteSpace(_svgString))
            {
                _originalImage = null;
                Image = null;
                old?.Dispose();
                return;
            }

            Color color = _isFormPanel && _brightFormColor.HasValue ? _brightFormColor.Value :
                         !_isFormPanel && _brightPanelColor.HasValue ? _brightPanelColor.Value : ForeColor;

            _originalImage = SvgRenderer.RenderSvgFromString(_svgString, _imageSize.Width, _imageSize.Height, color);
            Image = _originalImage;
            old?.Dispose();
        }

        protected override void OnHandleDestroyed(EventArgs e) {  base.OnHandleDestroyed(e); }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                
                _originalImage?.Dispose();
                Image?.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            if (IsDisposed) return; base.OnMouseEnter(e);
            if (_originalImage != null)
            {
                var bright = DesignHelper.CreateBrightImageWithOffset(_originalImage, DG_BrightBrightnessFactor, DG_BrightOffset);
                Image?.Dispose();
                Image = bright;
            }
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (IsDisposed) return; base.OnMouseLeave(e);
            if (_originalImage != null)
            {
                var normal = DesignHelper.CreateBrightImageWithOffset(_originalImage, DG_NormalBrightnessFactor, DG_NormalOffset);
                Image?.Dispose();
                Image = normal;
            }
            Invalidate();
        }
    }
}