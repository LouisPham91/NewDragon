
using System.ComponentModel;
using System.Drawing.Drawing2D;


namespace Dragon.DesignView.Public.NormalMode
{
    public class TableLayoutPanelBottomBorderN : TableLayoutPanel, IThemeable
    {
        private bool _isCellSingleBorder = false;
        private Color _borderColor = Color.FromArgb(61, 61, 61);
        private int _borderThickness = 1;

        // 🔥 Theme handler


        [Category("Dragon")]
        [DisplayName("DG_IsCellSingleBorder")]
        [Description("Vẽ viền đơn cho từng cell (DG_)")]
        [DefaultValue(false)]
        public bool DG_IsCellSingleBorder
        {
            get => _isCellSingleBorder;
            set
            {
                if (_isCellSingleBorder == value) return;
                _isCellSingleBorder = value;
                if (IsHandleCreated) Invalidate();
            }
        }

        [Category("Dragon")]
        [DisplayName("DG_BorderColor")]
        [Description("Màu viền (DG_)")]
        [DefaultValue(typeof(Color), "Gray")]
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
        [Description("Độ dày viền (DG_)")]
        [DefaultValue(1)]
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

        public TableLayoutPanelBottomBorderN()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |  ControlStyles.OptimizedDoubleBuffer, true);
            CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
            Padding = new Padding(0);
            Margin = new Padding(0);
            ApplyTheme();
        }

        public void ApplyTheme()
        {
            if (IsDisposed) return;

            // NormalMode: CHỈ dùng màu cơ bản
            BackColor = ThemeHelper.BackNormalFirst;
            _borderColor = ThemeHelper.BorderNormalFist;

            Invalidate();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                
            base.Dispose(disposing);
        }

        public void ChangeColor(Color? backColor = null, Color? borderColor = null)
        {
            if (backColor.HasValue)
                BackColor = backColor.Value;
            if (borderColor.HasValue)
                _borderColor = borderColor.Value;

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (IsDisposed) return;
            base.OnPaint(e);

            if (_borderThickness > 0)
            {
                using (Pen pen = new Pen(_borderColor, _borderThickness))
                {
                    e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                    e.Graphics.DrawLine(pen, -1, Height, Width + 1, Height);

                }
            }
        }

        protected override void OnCellPaint(TableLayoutCellPaintEventArgs e)
        {
            if (IsDisposed) return;
            base.OnCellPaint(e);

            if (!_isCellSingleBorder) return;

            using var pen = new Pen(_borderColor, _borderThickness)
            {
                Alignment = PenAlignment.Inset
            };
            var b = e.CellBounds;

            if (e.Column < ColumnCount - 1)
                e.Graphics.DrawLine(pen, b.Right - 1, b.Top, b.Right - 1, b.Bottom);

            if (e.Row < RowCount - 1)
                e.Graphics.DrawLine(pen, b.Left, b.Bottom - 1, b.Right, b.Bottom - 1);

            if (e.Row == RowCount - 1)
                e.Graphics.DrawLine(pen, b.Left, b.Bottom - 1, b.Right, b.Bottom - 1);

            if (e.Row == 0)
                e.Graphics.DrawLine(pen, b.Left, b.Top, b.Right, b.Top);

            //// --- THÊM 2 DÒNG NÀY THÔI ---
            //// 1. Vẽ viền TRÁI cho cột đầu tiên
            //if (e.Column == 0)
            //    e.Graphics.DrawLine(pen, b.Left, b.Top, b.Left, b.Bottom - 1);

            //// 2. Vẽ viền PHẢI cho cột cuối cùng (cái mà điều kiện cũ đã bỏ qua)
            //if (e.Column == ColumnCount - 1)
            //    e.Graphics.DrawLine(pen, b.Right - 1, b.Top, b.Right - 1, b.Bottom - 1);
        }
    }
}