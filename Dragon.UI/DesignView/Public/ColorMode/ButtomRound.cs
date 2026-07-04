using Dragon.ControlHelper;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Dragon.DesignView.Public.ColorMode
{
    public class ButtomRound : Button, IThemeable
    {
        public Color _foceColor = Color.FromArgb(207, 66, 51);
        public float borderRadius = 15f;
        public float _lightenPercent = 0.2f; // <— giữ nguyên
        public string _svgString = string.Empty;
        Color _originalBackColor = Color.FromArgb(240, 240, 240);
        public bool isChangeText = false;

        private bool _isHover = false;
        private bool _isPressed = false;

        protected override bool ShowFocusCues => false;

        // --- PROPERTY ---
        [Category("Dragon")]
        [DefaultValue(typeof(Color), "207, 66, 51")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_ForceColor
        {
            get => _foceColor;
            set { if (_foceColor == value) return; _foceColor = value; ForeColor = value; updateImageSVG(); if (IsHandleCreated) Invalidate(); }
        }

        [Category("Dragon")]
        [DefaultValue(15f)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public float GD_Radius
        {
            get => borderRadius;
            set { if (borderRadius == value) return; borderRadius = Math.Max(0, value); if (IsHandleCreated) Invalidate(); }
        }

        [Category("Dragon")]
        [DefaultValue("")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string DG_SVGString
        {
            get => _svgString;
            set { value ??= string.Empty; if (_svgString == value) return; _svgString = value; updateImageSVG(); if (IsHandleCreated) Invalidate(); }
        }

        // giữ y nguyên logic gốc
        [Category("Dragon")]
        [DefaultValue(0.2f)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public float DG_LightenPercent
        {
            get => _lightenPercent;
            set { var v = Math.Max(0, Math.Min(255, value)); if (_lightenPercent == v) return; _lightenPercent = v; if (IsHandleCreated) Invalidate(); }
        }

        [Category("Dragon")]
        [DefaultValue(typeof(Color), "240, 240, 240")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_BackColor
        {
            get => _originalBackColor;
            set { if (_originalBackColor == value) return; _originalBackColor = value; if (IsHandleCreated) Invalidate(); }
        }
        public ButtomRound()
        {
            SetStyle(ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.Selectable, true);
            SetStyle(ControlStyles.Selectable, false);

            TabStop = false;
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            UseVisualStyleBackColor = false;
            BackColor = Color.Transparent;

            ApplyTheme();
        }

        public void updateImageSVG()
        {
            if (IsDisposed) return;
            if (!string.IsNullOrEmpty(_svgString))
            {
                int h = Math.Max(16, Font.Height);
                var old = Image;
                Image = SvgRenderer.RenderSvgFromString(_svgString, h, h, ForeColor);
                old?.Dispose();
            }
        }

        public void ApplyTheme()
        {
            if (IsDisposed) return;
            // giữ nguyên logic theme của bạn
            if (ThemeHelper.CurrentMode == ThemeMode.Light)
            {
                if (ThemeHelper.CurrentStyle == ThemeStyle.ThemeRed) { DG_BackColor = ThemeHelper.BackNormal3rd; DG_ForceColor = ThemeHelper.ForceColoful4th; }
                else if (ThemeHelper.CurrentStyle == ThemeStyle.ThemeGreen) { DG_BackColor = ThemeHelper.BackNormal3rd; DG_ForceColor = ThemeHelper.ForceColofulFist; }
                else if (ThemeHelper.CurrentStyle == ThemeStyle.ThemeGold) { DG_BackColor = ThemeHelper.BackNormal3rd; DG_ForceColor = ThemeHelper.ForceColoful2nd; }
            }
            else
            {
                if (ThemeHelper.CurrentStyle == ThemeStyle.ThemeRed) { DG_BackColor = ThemeHelper.BackColofulFirst; DG_ForceColor = ThemeHelper.ForceColofulFist; }
                else if (ThemeHelper.CurrentStyle == ThemeStyle.ThemeGreen) { DG_BackColor = ThemeHelper.BackColofulFirst; DG_ForceColor = ThemeHelper.ForceColoful2nd; }
                else if (ThemeHelper.CurrentStyle == ThemeStyle.ThemeGold) { DG_BackColor = ThemeHelper.BackColofulFirst; DG_ForceColor = ThemeHelper.ForceColoful3rd; }
            }

            if (isChangeText && Name == "buttomDarkMode")
                Text = ThemeHelper.CurrentMode == ThemeMode.Light ? "Light Mode" : "Dark Mode";

            Invalidate();
        }

        protected override void OnMouseEnter(EventArgs e) { _isHover = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { _isHover = false; _isPressed = false; Invalidate(); base.OnMouseLeave(e); }
        protected override void OnMouseDown(MouseEventArgs e) { if (e.Button == MouseButtons.Left) { _isPressed = true; Invalidate(); } base.OnMouseDown(e); }
        protected override void OnMouseUp(MouseEventArgs e) { _isPressed = false; Invalidate(); base.OnMouseUp(e); }

        protected override void OnPaintBackground(PaintEventArgs pevent) { }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;

            // 1. Vẽ nền bo góc – vẫn mượt
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.Clear(Parent?.BackColor ?? SystemColors.Control);

            var rect = new Rectangle(0, 0, Width - 1, Height - 1); // -1 để không bị tràn pixel
            using var path = GetRoundedPath(rect, borderRadius);

            Color back = _originalBackColor;
            if (_isPressed) back = ControlPaint.Dark(back, 0.08f);
            else if (_isHover) back = ControlPaint.Light(back, 0.04f);

            using (var b = new SolidBrush(back))
                g.FillPath(b, path);

            // 2. TẮT anti-alias trước khi vẽ text/icon -> sắc nét
            g.SmoothingMode = SmoothingMode.None;
            g.InterpolationMode = InterpolationMode.NearestNeighbor; // icon SVG giữ nét
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            // 3. Tính toán căn giữa bằng số NGUYÊN
            var img = Image;
            var textSize = TextRenderer.MeasureText(Text, Font);
            int totalW = textSize.Width + (img != null ? img.Width + 6 : 0);
            int startX = (Width - totalW) / 2;   // chia nguyên, không .5
            int startY = (Height - Math.Max(img?.Height ?? 0, textSize.Height)) / 2;

            // vẽ icon
            if (img != null)
            {
                g.DrawImage(img, startX, startY + (textSize.Height - img.Height) / 2, img.Width, img.Height);
                startX += img.Width + 6;
            }

            // 4. Dùng TextRenderer – đây là bí kíp hết mờ
            TextRenderer.DrawText(g, Text, Font,
                new Point(startX, (Height - textSize.Height) / 2),
                ForeColor,
                TextFormatFlags.Left | TextFormatFlags.NoPadding | TextFormatFlags.SingleLine);
        }

        private GraphicsPath GetRoundedPath(Rectangle r, float radius)
        {
            float d = radius * 2;
            var p = new GraphicsPath();
            if (radius <= 0) { p.AddRectangle(r); return p; }
            p.AddArc(r.X, r.Y, d, d, 180, 90);
            p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            p.CloseFigure();
            return p;
        }

        protected override void OnHandleCreated(EventArgs e) { base.OnHandleCreated(e); Region = null; }
        protected override void OnSizeChanged(EventArgs e) { base.OnSizeChanged(e); Region = null; Invalidate(); }
    }
}