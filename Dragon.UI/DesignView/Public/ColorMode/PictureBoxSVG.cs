
using Dragon.ControlHelper;
using System.ComponentModel;

namespace Dragon.DesignView.Public.ColorMode
{
    public class PictureBoxSVG : PictureBox 
    {
        private string _svgString = string.Empty;
        private float _zoom = 1.0f;
        private Color? _svgColor = null;

        [Browsable(true)]
        [Category("DragonUI")]
        [Description("SVG string để render thành hình ảnh")]
        [DefaultValue("")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string DG_SVGString
        {
            get => _svgString;
            set { value ??= ""; if (_svgString == value) return; _svgString = value; RenderSvg(); if (IsHandleCreated) Invalidate(); }
        }

        [Browsable(true)]
        [Category("DragonUI")]
        [Description("Tỉ lệ zoom (1.0 = bình thường, <1 thu nhỏ, >1 phóng to)")]
        [DefaultValue(1f)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public float DG_Zoom
        {
            get => _zoom;
            set { if (value <= 0 || _zoom == value) return; _zoom = value; RenderSvg(); if (IsHandleCreated) Invalidate(); }
        }

        [Browsable(true)]
        [Category("DragonUI")]
        [Description("Màu ép cho SVG. Nếu null thì dùng màu gốc của SVG.")]
        [DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color? DG_SVGColor
        {
            get => _svgColor;
            set { if (_svgColor == value) return; _svgColor = value; RenderSvg(); if (IsHandleCreated) Invalidate(); }
        }

        public void RenderSvg()
        {
            if (string.IsNullOrWhiteSpace(_svgString))
            {
                ReplaceImage(null);
                return;
            }

            // Kích thước mục tiêu dựa trên zoom
            int targetW = Math.Max(1, (int)(Width * _zoom));
            int targetH = Math.Max(1, (int)(Height * _zoom));

            try
            {
                if (_svgColor == null) return;
                // Giả sử RenderSvgFromString trả về Bitmap đã render đúng kích thước targetW x targetH
                using (var renderedBmp = SvgRenderer.RenderSvgFromString(_svgString, targetW, targetH, _svgColor.Value))
                {
                    if (renderedBmp == null)
                    {
                        ReplaceImage(null);
                        return;
                    }

                    // Tạo final bitmap có kích thước control và vẽ renderedBmp vào giữa
                    var finalBmp = new Bitmap(Width, Height);
                    using (Graphics g = Graphics.FromImage(finalBmp))
                    {
                        g.Clear(BackColor);
                        g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

                        int x = (Width - targetW) / 2;
                        int y = (Height - targetH) / 2;
                        g.DrawImage(renderedBmp, x, y, targetW, targetH);
                    }

                    // Gán finalBmp lên PictureBox một cách an toàn
                    ReplaceImage(finalBmp);
                }
            }
            catch
            {
                ReplaceImage(null);
            }
        }

        private void ReplaceImage(Image? newImage)
        {
            var old = Image;
            if (old != null && old != newImage)
            {
                try { old.Dispose(); } catch { }
            }
            Image = newImage;
            Invalidate();
        }


        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            RenderSvg();
        }
    }
}