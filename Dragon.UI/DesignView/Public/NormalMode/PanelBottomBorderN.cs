
using System.ComponentModel;
using System.Drawing.Drawing2D;


namespace Dragon.DesignView.Public.NormalMode
{
    public class PanelBottomBorderN : Panel, IThemeable
    {
        private Color _backColor = Color.FromArgb(40, 40, 40);
        private Color _borderColor = Color.FromArgb(61, 61, 61);
        private int _borderThickness = 1;

        // 🔥 Theme handler


        [Category("Dragon")]
        [DisplayName("DG_BackColor")]
        [Description("Màu nền panel")]
        [DefaultValue(typeof(Color), "40, 40, 40")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_BackColor
        {
            get => _backColor;
            set
            {
                if (_backColor == value) return;
                _backColor = value;
                BackColor = value;
                if (IsHandleCreated) Invalidate();
            }
        }

        [Category("Dragon")]
        [DisplayName("DG_BorderColor")]
        [Description("Màu viền dưới")]
        [DefaultValue(typeof(Color), "61, 61, 61")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_BorderColor
        {
            get => _borderColor;
            set
            {
                if (_borderColor == value) return;
                _borderColor = value;
                if (IsHandleCreated) Invalidate();
            }
        }

        [Category("Dragon")]
        [DisplayName("DG_BorderThickness")]
        [Description("Độ dày viền dưới")]
        [DefaultValue(1)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int DG_BorderThickness
        {
            get => _borderThickness;
            set
            {
                value = Math.Max(0, value);
                if (_borderThickness == value) return;
                _borderThickness = value;
                if (IsHandleCreated) Invalidate();
            }
        }

        public PanelBottomBorderN()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);
            UpdateStyles();

            // 🔥 Khởi tạo ngay để tránh warning CS8618
            
            
            ApplyTheme();
        }

        public void ApplyTheme()
        {
            if (IsDisposed) return;

            // NormalMode: CHỈ dùng màu cơ bản
            _backColor = ThemeHelper.BackNormalFirst;
            _borderColor = ThemeHelper.BorderNormalFist;
            ForeColor = ThemeHelper.ForeNormalFirst;
            BackColor = _backColor;

            Invalidate();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                
            base.Dispose(disposing);
        }

        public void ChangeColor(Color? backColor = null, Color? borderColor = null, int? borderThickness = null)
        {
            if (backColor.HasValue)
            {
                _backColor = backColor.Value;
                BackColor = backColor.Value;
            }
            if (borderColor.HasValue)
                _borderColor = borderColor.Value;
            if (borderThickness.HasValue)
                _borderThickness = borderThickness.Value;

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (IsDisposed) return;
            base.OnPaint(e);

            BackColor = _backColor;

            if (_borderThickness > 0)
            {
                int y = ClientSize.Height - _borderThickness;
                using (Pen pen = new Pen(_borderColor, _borderThickness))
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.DrawLine(pen, 0, y, ClientSize.Width, y);
                }
            }
        }
    }
}