
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Dragon.DesignView.Public.ColorMode
{
    public class PanelColorC : Panel, IThemeable
    {
        private bool _isClick = false;
        private Color _bottomLineColor = Color.White;

        [Category("Dragon")]
        [DefaultValue(typeof(Color), "White")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_BottomLineColor
        {
            get => _bottomLineColor;
            set { if (_bottomLineColor == value) return; _bottomLineColor = value; if (IsHandleCreated) Invalidate(); }
        }

        [Category("Dragon")]
        [DefaultValue(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool DG_IsClick
        {
            get => _isClick;
            set { if (_isClick == value) return; _isClick = value; if (IsHandleCreated) Invalidate(); }
        }

        public PanelColorC()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);
            UpdateStyles();

            
            
            ApplyTheme();
        }

        public void ApplyTheme()
        {
            if (IsDisposed) return;

            BackColor = ThemeHelper.BackNormalFirst;

            if (ThemeHelper.CurrentMode == ThemeMode.Light)
            {
                _bottomLineColor = ThemeHelper.CurrentStyle switch
                {
                    ThemeStyle.ThemeRed => ThemeHelper.ForceColoful4th,
                    ThemeStyle.ThemeGreen => ThemeHelper.ForceColofulFist,
                    ThemeStyle.ThemeGold => ThemeHelper.ForceColoful2nd,
                    _ => ThemeHelper.ForceColofulFist
                };
            }
            else
            {
                _bottomLineColor = ThemeHelper.CurrentStyle switch
                {
                    ThemeStyle.ThemeRed => ThemeHelper.ForceColofulFist,
                    ThemeStyle.ThemeGreen => ThemeHelper.ForceColoful2nd,
                    ThemeStyle.ThemeGold => ThemeHelper.ForceColoful3rd,
                    _ => ThemeHelper.ForceColofulFist
                };
            }

            Invalidate();
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            
            base.OnHandleDestroyed(e);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                
            base.Dispose(disposing);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (IsDisposed) return;
            base.OnPaint(e);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            if (_isClick)
            {
                Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);
                using (GraphicsPath path = GetRectanglePath(rect))
                using (Pen pen = new Pen(_bottomLineColor, 2))
                {
                    e.Graphics.DrawPath(pen, path);
                }
            }
        }

        private GraphicsPath GetRectanglePath(Rectangle rect)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddRectangle(rect);
            return path;
        }

        public void DrawLineBaseOnClick()
        {
            if (IsDisposed) return;

            Control? parent = Parent;
            if (parent is null) return;

            foreach (Control control in parent.Controls)
            {
                if (control is PanelColorC ctrl)
                {
                    ctrl._isClick = ctrl.Name == Name;
                    ctrl.Invalidate();
                }
            }
        }
    }
}