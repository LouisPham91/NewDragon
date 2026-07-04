

using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Dragon.DesignView.Public.NormalMode
{
    public class ComboBoxCustomNC : ComboBox, IThemeable
    {
        private Color _hoverBackColor = Color.LightSalmon;
        private Color _selectedBackColor = Color.IndianRed;
        private Color _borderColor = Color.FromArgb(207, 66, 51);
        private Color _arrowButtonColor = Color.FromArgb(207, 66, 51);
        private Color _arrowColor = Color.FromArgb(40, 40, 40);
        private float _arrowWidth = 3f;
        private float _arrowHeight = 1.5f;



        [Category("Dragon")]
        [DefaultValue(typeof(Color), "LightSalmon")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_HoverBackColor
        {
            get => _hoverBackColor;
            set { if (_hoverBackColor == value) return; _hoverBackColor = value; if (IsHandleCreated) Invalidate(); }
        }

        [Category("Dragon")]
        [DefaultValue(typeof(Color), "IndianRed")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_SelectedBackColor
        {
            get => _selectedBackColor;
            set { if (_selectedBackColor == value) return; _selectedBackColor = value; if (IsHandleCreated) Invalidate(); }
        }

        [Category("Dragon")]
        [DefaultValue(typeof(Color), "207, 66, 51")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_BorderColor
        {
            get => _borderColor;
            set { if (_borderColor == value) return; _borderColor = value; if (IsHandleCreated) Invalidate(); }
        }

        [Category("Dragon")]
        [DefaultValue(typeof(Color), "207, 66, 51")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_ArrowButtonColor
        {
            get => _arrowButtonColor;
            set { if (_arrowButtonColor == value) return; _arrowButtonColor = value; if (IsHandleCreated) Invalidate(); }
        }

        [Category("Dragon")]
        [DefaultValue(typeof(Color), "40, 40, 40")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_ArrowColor
        {
            get => _arrowColor;
            set { if (_arrowColor == value) return; _arrowColor = value; if (IsHandleCreated) Invalidate(); }
        }

        [Category("Dragon")]
        [DefaultValue(3f)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public float DG_ArrowWidth
        {
            get => _arrowWidth;
            set { if (_arrowWidth == value) return; _arrowWidth = Math.Max(0, value); if (IsHandleCreated) Invalidate(); }
        }

        [Category("Dragon")]
        [DefaultValue(1.5f)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public float DG_ArrowHeight
        {
            get => _arrowHeight;
            set { if (_arrowHeight == value) return; _arrowHeight = Math.Max(0, value); if (IsHandleCreated) Invalidate(); }
        }

        public ComboBoxCustomNC()
        {
            DrawMode = DrawMode.OwnerDrawFixed;
            DropDownStyle = ComboBoxStyle.DropDownList;
            FlatStyle = FlatStyle.Flat;

            // AOT: không dùng UserPaint trên ComboBox
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.ResizeRedraw, true); 
            
            ApplyTheme();
        }

        public void ApplyTheme()
        {
            if (IsDisposed) return;
            BackColor = ThemeHelper.BackNormalFirst;
            ForeColor = ThemeHelper.ForeNormalFirst;
            _borderColor = ThemeHelper.BorderNormalFist;
            _arrowColor = ThemeHelper.ForeNormalFirst;
            _selectedBackColor = ThemeHelper.CurrentMode == ThemeMode.Light
                ? Color.FromArgb(230, 230, 230)
                : Color.FromArgb(70, 70, 70);
            Invalidate();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) 
            base.Dispose(disposing);
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            Color back = selected ? _selectedBackColor : BackColor;

            using var bg = new SolidBrush(back);
            e.Graphics.FillRectangle(bg, e.Bounds);

            string text = Items[e.Index]?.ToString() ?? string.Empty;
            TextRenderer.DrawText(e.Graphics, text, Font, e.Bounds, ForeColor,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);

            if ((e.State & DrawItemState.Focus) == DrawItemState.Focus)
                e.DrawFocusRectangle();
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == 0x000F) // WM_PAINT
            {
                using var g = Graphics.FromHwnd(Handle);
                g.SmoothingMode = SmoothingMode.AntiAlias;

                // viền
                using (var pen = new Pen(_borderColor, 1))
                    g.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);

                // nền nút mũi tên
                int btnW = SystemInformation.VerticalScrollBarWidth;
                var btnRect = new Rectangle(Width - btnW, 1, btnW - 1, Height - 2);
                using (var bg = new SolidBrush(BackColor))
                    g.FillRectangle(bg, btnRect);

                // mũi tên
                float midX = Width - btnW / 2f;
                float midY = Height / 2f;
                PointF[] arrow = {
                    new PointF(midX - _arrowWidth, midY - _arrowHeight),
                    new PointF(midX + _arrowWidth, midY - _arrowHeight),
                    new PointF(midX, midY + _arrowHeight)
                };
                using var arrowBrush = new SolidBrush(_arrowColor);
                g.FillPolygon(arrowBrush, arrow);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Height = PreferredHeight;
            Invalidate();
        }
    }
}