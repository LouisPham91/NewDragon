


using System.ComponentModel;

namespace Dragon.DesignView.Public.NormalMode
{
    public class LabelPhones : Label, IThemeable
    {
        private bool _isDrawingLine = true;

        // 🔥 Theme handler


        // Action không serialize được -> ẩn khỏi Designer
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Action? OnSelectAllPhones { get; set; }

        [Category("Dragon")]
        [DisplayName("DG_IsDrawingLine")]
        [Description("Vẽ đường gạch chân")]
        [DefaultValue(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool DG_IsDrawingLine
        {
            get => _isDrawingLine;
            set
            {
                if (_isDrawingLine == value) return;
                _isDrawingLine = value;
                if (IsHandleCreated) Invalidate();
            }
        }

        public LabelPhones()
        {
            SetStyle(ControlStyles.Selectable, true);
            TabStop = true;

            // 🔥 Khởi tạo ngay để tránh warning CS8618
            
            
            ApplyTheme();
        }

        public void ApplyTheme()
        {
            if (IsDisposed) return;

            // NormalMode: CHỈ dùng màu cơ bản
            if (ThemeHelper.CurrentMode == ThemeMode.Light)
            {
                BackColor = ThemeHelper.BackNormal3rd;
                ForeColor = ThemeHelper.ForeNormalFirst;
            }
            else
            {
                BackColor = ThemeHelper.BackColofulFirst;
                ForeColor = ThemeHelper.ForeNormalFirst;
            }

            Invalidate();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                
            base.Dispose(disposing);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.A))
            {
                OnSelectAllPhones?.Invoke();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (IsDisposed) return;
            Focus();
            base.OnMouseDown(e);
        }

        public void IsConnect(bool isOnline)
        {
            _isDrawingLine = isOnline;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (IsDisposed) return;
            base.OnPaint(e);

            if (_isDrawingLine) return;

            using (Pen pen = new Pen(ForeColor, 1))
            {
                Point center = new Point(Width - Width / 8, Height - Height / 8);
                int length = Math.Min(Width, Height) / 8;

                for (int i = 0; i < 8; i++)
                {
                    double angle = i * Math.PI / 4;
                    int x = center.X + (int)(length * Math.Cos(angle));
                    int y = center.Y + (int)(length * Math.Sin(angle));
                    e.Graphics.DrawLine(pen, center, new Point(x, y));
                }
            }
        }
    }
}