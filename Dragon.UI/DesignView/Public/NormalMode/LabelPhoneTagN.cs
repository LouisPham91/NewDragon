
using System.ComponentModel;
using Dragon.ControlHelper;

namespace Dragon.DesignView.Public.NormalMode
{
   

    public class LabelPhoneTagN : Label, IThemeable
    {
        private string _svgString = string.Empty;
        private Color _foreColor = Color.White;


        [Category("Appearance")]
        [DisplayName("DG_SVGString")]
        [Description("SVG content trực tiếp")]
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
                UpdateImage();
            }
        }

        [Category("Appearance")]
        [DisplayName("DG_ForeColor")]
        [Description("Màu vẽ SVG (đồng bộ ForeColor)")]
        [DefaultValue(typeof(Color), "White")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_ForeColor
        {
            get => _foreColor;
            set
            {
                if (_foreColor == value) return;
                _foreColor = value;
                ForeColor = value;
                UpdateImage();
            }
        }

        public LabelPhoneTagN()
        {
            
            
            ApplyTheme();
        }

        private void UpdateImage()
        {
            if (IsDisposed) return;

            var old = Image;

            if (string.IsNullOrWhiteSpace(_svgString))
            {
                Image = null;
            }
            else
            {
                int height = Math.Max(Font.Height - 2, 16);
                Image = SvgRenderer.RenderSvgFromString(_svgString, height, height, _foreColor);
            }

            old?.Dispose();
        }

        public void ApplyTheme()
        {
            if (IsDisposed) return;
            BackColor = ThemeHelper.BackNormalFirst;
            _foreColor = ThemeHelper.ForeNormalFirst;
            ForeColor = _foreColor;
            UpdateImage();
            Invalidate();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                
                Image?.Dispose();
            }
            base.Dispose(disposing);
        }

        public void ChangeColor(string? svgString = null, Color? foreColor = null)
        {
            if (IsDisposed) return;
            if (!string.IsNullOrEmpty(svgString)) _svgString = svgString;
            if (foreColor.HasValue) { _foreColor = foreColor.Value; ForeColor = foreColor.Value; }
            UpdateImage();
        }
    }
}