

using Dragon.ControlHelper;
using System.ComponentModel;

namespace Dragon.DesignView.Public.NormalMode
{
    public class LabelHoverN : Label, IThemeable
    {
        private Image? _imageLight = null;
        private Image? _imageDark = null;
        private string _svgString = string.Empty;
        private Color _originalFore;
        private Color _originalBackGround;
        private float _lightenFactor = 0.2f;

        // 🔥 Theme handler


        [Category("Dragon")]
        [DisplayName("DG_LightenFactor")]
        [Description("Hệ số làm sáng (0-1)")]
        [DefaultValue(0.2f)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public float DG_LightenFactor
        {
            get => _lightenFactor;
            set
            {
                value = Math.Clamp(value, 0f, 1f);
                if (_lightenFactor == value) return;
                _lightenFactor = value;
                // nếu đổi factor cần vẽ lại
                UpdateSvgImage();
                if (IsHandleCreated) Invalidate();
            }
        }

        [Category("Dragon")]
        [DisplayName("DG_SVGString")]
        [Description("Nội dung SVG")]
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
                UpdateSvgImage();
                if (IsHandleCreated) Invalidate();
            }
        }

        private void UpdateSvgImage()
        {
            if (string.IsNullOrEmpty(_svgString)) return;

            var height = Font.Height - 2;
            _imageLight = SvgRenderer.RenderSvgFromString(_svgString, height, height, Color.FromArgb(245, 245, 245));
            _imageDark = SvgRenderer.RenderSvgFromString(_svgString, height, height, Color.FromArgb(40, 40, 40));

            Image = ThemeHelper.CurrentMode == ThemeMode.Light ? _imageDark : _imageLight;
        }

        public LabelHoverN()
        {
            DoubleBuffered = true;

            ApplyTheme();
        }

        public void ApplyTheme()
        {
            if (IsDisposed) return;

            // NormalMode: CHỈ dùng màu cơ bản
            if (ThemeHelper.CurrentMode == ThemeMode.Light)
            {
                _originalBackGround = ThemeHelper.BackNormalFirst;
                _originalFore = ThemeHelper.ForeNormalFirst;
                Image = _imageDark;
            }
            else
            {
                _originalBackGround = ThemeHelper.BackNormalFirst;
                _originalFore = ThemeHelper.ForeNormalFirst;
                Image = _imageLight;
            }

            BackColor = _originalBackGround;
            ForeColor = _originalFore;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                
            base.Dispose(disposing);
        }

        protected override void OnMouseHover(EventArgs e)
        {
            if (IsDisposed) return;
            base.OnMouseHover(e);

            if (ThemeHelper.CurrentMode == ThemeMode.Light)
                BackColor = ControlPaint.Dark(_originalBackGround, _lightenFactor);
            else
                BackColor = ControlPaint.Light(_originalBackGround, _lightenFactor);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (IsDisposed) return;
            base.OnMouseLeave(e);

            ForeColor = _originalFore;
            BackColor = _originalBackGround;
        }
    }
}