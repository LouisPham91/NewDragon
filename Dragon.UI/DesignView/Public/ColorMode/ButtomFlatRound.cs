using Dragon.ControlHelper;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Dragon.DesignView.Public.ColorMode
{
    public class ButtomFlatRound : Button, IThemeable
    {
        private Color _backColor = Color.FromArgb(97, 53, 49);
        private Color _foreColor = Color.FromArgb(207, 66, 51);
        private int _padding = 6;
        private bool _isHovered = false;
        private string _svgString = string.Empty; // <-- đổi từ imageName



        [Category("Appearance")]
        [DefaultValue(0)] // đổi thành giá trị khởi tạo thực tế của _padding
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int DG_Padding
        {
            get => _padding;
            set { if (_padding == value) return; _padding = Math.Max(0, value); if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(Color), "Empty")] // đổi nếu _backColor có default khác
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_BackColor
        {
            get => _backColor;
            set { if (_backColor == value) return; _backColor = value; BackColor = value; if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(Color), "Empty")] // đổi nếu _foreColor có default khác
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_ForeColor
        {
            get => _foreColor;
            set { if (_foreColor == value) return; _foreColor = value; ForeColor = value; UpdateImage(); if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [Description("SVG content trực tiếp, không cần file")]
        [DefaultValue("")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string DG_SVGString
        {
            get => _svgString;
            set { value ??= string.Empty; if (_svgString == value) return; _svgString = value; UpdateImage(); if (IsHandleCreated) Invalidate(); }
        }

        public ButtomFlatRound()
        {
            FlatStyle = FlatStyle.Flat;
            DoubleBuffered = true;
            FlatAppearance.BorderSize = 0;
            FlatAppearance.MouseDownBackColor = Color.Transparent;
            FlatAppearance.MouseOverBackColor = Color.Transparent;

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);

            
            
            ApplyTheme();
        }

        private void UpdateImage()
        {
            if (IsDisposed) return;

            // dispose ảnh cũ tránh leak GDI
            var old = Image;

            if (string.IsNullOrWhiteSpace(_svgString))
            {
                Image = null;
            }
            else
            {
                var height = Math.Max(Font.Height - 2, 16);
                Image = SvgRenderer.RenderSvgFromString(_svgString, height, height, _foreColor);
            }

            old?.Dispose();
        }

        public void ApplyTheme()
        {
            if (IsDisposed) return;

            if (ThemeHelper.CurrentMode == ThemeMode.Light)
            {
                _backColor = ThemeHelper.BackNormalFirst;
                _foreColor = ThemeHelper.CurrentStyle switch
                {
                    ThemeStyle.ThemeRed => ThemeHelper.ForceColoful4th,
                    ThemeStyle.ThemeGreen => ThemeHelper.ForceColofulFist,
                    ThemeStyle.ThemeGold => ThemeHelper.ForceColoful2nd,
                    _ => ThemeHelper.ForceColofulFist
                };
            }
            else
            {
                _backColor = ThemeHelper.BackColofulFirst;
                _foreColor = ThemeHelper.CurrentStyle switch
                {
                    ThemeStyle.ThemeRed => ThemeHelper.ForceColofulFist,
                    ThemeStyle.ThemeGreen => ThemeHelper.ForceColoful2nd,
                    ThemeStyle.ThemeGold => ThemeHelper.ForceColoful3rd,
                    _ => ThemeHelper.ForceColofulFist
                };
            }

            BackColor = _backColor;
            ForeColor = _foreColor;
            UpdateImage();
            Invalidate();
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            
            base.OnHandleDestroyed(e);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                
                Image?.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            if (IsDisposed) return;
            base.OnMouseEnter(e);
            _isHovered = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (IsDisposed) return;
            base.OnMouseLeave(e);
            _isHovered = false;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (IsDisposed) return;
            e.Graphics.Clear(_backColor);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            RectangleF rect = new RectangleF(0.5f, 0.5f, Width - 1f, Height - 1f);
            Color fillColor = _isHovered ? ControlPaint.Light(_backColor, 0.2f) : _backColor;

            using (GraphicsPath path = GetRoundedRectanglePath(rect, 5f))
            using (SolidBrush brush = new SolidBrush(fillColor))
            {
                e.Graphics.FillPath(brush, path);
                var oldRegion = Region;
                Region = new Region(path);
                oldRegion?.Dispose();
            }

            int imageSize = Font.Height - 2;
            Size textSize = TextRenderer.MeasureText(Text, Font);
            int totalContentWidth = (Image != null ? imageSize + _padding : 0) + textSize.Width;
            int leftStart = Math.Max((Width - totalContentWidth) / 2, 5);

            if (Image != null)
            {
                Rectangle imageRect = new Rectangle(leftStart, (Height - imageSize) / 2, imageSize, imageSize);
                e.Graphics.DrawImage(Image, imageRect);

                Rectangle textRect = new Rectangle(imageRect.Right + _padding, 0, Width - imageRect.Right - _padding, Height);
                TextRenderer.DrawText(e.Graphics, Text, Font, textRect, _foreColor,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
            }
            else
            {
                Rectangle textRect = new Rectangle(leftStart, 0, Width - leftStart, Height);
                TextRenderer.DrawText(e.Graphics, Text, Font, textRect, _foreColor,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
            }
        }

        private GraphicsPath GetRoundedRectanglePath(RectangleF rect, float radius)
        {
            float d = radius * 2f;
            GraphicsPath path = new GraphicsPath();

            if (radius <= 0f)
            {
                path.AddRectangle(rect);
                return path;
            }

            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddLine(rect.X + radius, rect.Y, rect.Right - radius, rect.Y);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddLine(rect.Right, rect.Y + radius, rect.Right, rect.Bottom - radius);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddLine(rect.Right - radius, rect.Bottom, rect.X + radius, rect.Bottom);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.AddLine(rect.X, rect.Bottom - radius, rect.X, rect.Y + radius);
            path.CloseFigure();

            return path;
        }
    }
}