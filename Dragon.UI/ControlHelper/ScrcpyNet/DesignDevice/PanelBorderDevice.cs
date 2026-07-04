
using Dragon.DesignView.Public;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Dragon.ControlHelper.ScrcpyNet.DesignDevice
{
    public class PanelBorderDevice : Panel
    {
        #region Fields
        private int borderRadius = 15;
        private int borderThickness = 4;

        private Color borderColor = Color.FromArgb(61, 61, 61);

        public volatile bool UseGradientBorder = false;
        public Color GradientStart = Color.FromArgb(255, 71, 51);
        public Color Gradient2nd = Color.FromArgb(200, 120, 130);
        public Color Gradient3rd = Color.FromArgb(21, 115, 154);
        public Color GradientEnd = Color.FromArgb(32, 173, 232);

        public volatile DoubleBufferedPanel? renderPanel;
        public volatile PictureBox? PictureBox;

        private Size lastRegionSize;
        private int lastRegionRadius;
        #endregion

        #region Properties
        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int BorderRadius
        {
            get => borderRadius;
            set
            {
                if (borderRadius == value) return;
                borderRadius = value;
                UpdateRegionIfNeeded();
                if (IsHandleCreated)
                    Invalidate();
            }
        }

        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int BorderThickness
        {
            get => borderThickness;
            set
            {
                if (borderThickness == value) return;
                borderThickness = Math.Max(0, value);
                Padding = new Padding(borderThickness);
                UpdateRegionIfNeeded();
                if (IsHandleCreated)
                    Invalidate();
            }
        }

        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color BorderColor
        {
            get => borderColor;
            set
            {
                if (borderColor == value)
                    return;

                borderColor = value;

                BackColor = value;

                if (renderPanel != null)
                    renderPanel.BackColor = value;

                if (IsHandleCreated)
                    Invalidate();
            }
        }

        #endregion
        #region Constructor
        public PanelBorderDevice()
        {
            // Chống flicker
            DoubleBuffered = true;
            SetStyle(ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);


            Margin = new Padding(borderThickness / 2);
            Padding = new Padding(borderThickness);
        }
        // WS_EX_COMPOSITED chỉ ở đây
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // WS_EX_COMPOSITED
                return cp;
            }
        }
        #endregion



        #region Public Methods
        public void ChangeColorParent(int? BorderRadius = null, int? BorderThickness = null)
        {
            Action changeAction = () =>
            {
                
                BackColor = ThemeHelper.BorderNormal4th;
                if (renderPanel != null)
                {
                    renderPanel.BackColor = ThemeHelper.BorderNormal4th;
                }
                // ý là tự tôi sẽ Invalidate ở cuối code => nên ko cần phải dùng Properties
                borderColor = ThemeHelper.BorderColofulFist;

                if (ThemeHelper.CurrentMode == ThemeMode.Light)
                {
                    GradientStart = ThemeHelper.GradientStart_Light;
                    Gradient2nd = ThemeHelper.Gradient2ND_Light;
                    Gradient3rd = ThemeHelper.Gradient3RD_Light;
                    GradientEnd = ThemeHelper.GradientEnd_Light;
                }
                else
                {

                    GradientStart = ThemeHelper.GradientStart;
                    Gradient2nd = ThemeHelper.Gradient2ND;
                    Gradient3rd = ThemeHelper.Gradient3RD;
                    GradientEnd = ThemeHelper.GradientEnd;
                }

                if (BorderRadius.HasValue)
                    borderRadius = BorderRadius.Value;
                if (BorderThickness.HasValue)
                {
                    borderThickness = BorderThickness.Value;
                    Padding = new Padding(BorderThickness.Value);
                }

                Invalidate();
            };

            if (InvokeRequired) Invoke(changeAction);
            else changeAction();
        }
        #endregion



        #region Rendering & Region

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            UpdateRegionIfNeeded();
        }

        int count = 1;
        private void UpdateRegionIfNeeded()
        {
            //Debug.WriteLine("Check Update Region Lần " + count);
            count++;
            if (Width <= 0 || Height <= 0) return;
            if (lastRegionSize == Size && lastRegionRadius == borderRadius) return;

            //Debug.WriteLine("Thực thi Update Region Lần : " + (count - 1));

            lastRegionSize = Size;
            lastRegionRadius = borderRadius;

            // Region control cha
            var outerRect = ClientRectangle;
            using (GraphicsPath outerPath = GetRoundedRectanglePath(outerRect, borderRadius))
            {
                Region?.Dispose();
                Region = new Region(outerPath);
            }

            // Region cho panel con
            if (renderPanel != null && renderPanel.Width > 0 && renderPanel.Height > 0)
            {
                var innerRect = renderPanel.ClientRectangle;
                //int innerRadius = Math.Max(0, borderRadius - borderThickness);
                using (GraphicsPath innerPath = GetRoundedRectanglePath(innerRect, borderRadius - borderThickness))
                {
                    renderPanel.Region?.Dispose();
                    renderPanel.Region = new Region(innerPath);
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);
            base.OnPaint(e); // vẽ nội dung control sau cùng
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            var rect = new Rectangle(0, 0, Width, Height);
            if (UseGradientBorder)
            {
                BackColor = GradientStart;
                using (var brush = new LinearGradientBrush(rect, Color.Empty, Color.Empty, LinearGradientMode.Vertical))
                {
                    var blend = new ColorBlend
                    {
                        Colors = new[] { GradientStart, Gradient2nd, Gradient3rd, GradientEnd },
                        Positions = new[] { 0f, 0.33f, 0.66f, 1f }
                    };
                    brush.InterpolationColors = blend;
                    e.Graphics.FillRectangle(brush, rect);
                }
            }


        }

        public static GraphicsPath GetRoundedRectanglePath(RectangleF rect, float radius)
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
        #endregion

        #region Overrides
        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);

            if (e.Control == renderPanel)
            {
                e.Control?.SendToBack();
                return;
            }
        }

        #endregion
    }
}

