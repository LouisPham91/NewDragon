

using System.ComponentModel;

namespace Dragon.DesignView.Public.NormalMode
{
    public class ComboBoxCustomFlatN : ComboBox, IThemeable
    {
        private Color _borderColor = Color.Black;
        private Color _arrowColor = Color.White;
        private int _borderSize = 1;
        private float _arrowWidth = 3f;
        private float _arrowHeight = 1.5f;



        [Category("Dragon")]
        [DisplayName("DG_BorderColor")]
        [DefaultValue(typeof(Color), "Black")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_BorderColor
        {
            get => _borderColor;
            set { if (_borderColor == value) return; _borderColor = value; if (IsHandleCreated) Invalidate(); }
        }

        [Category("Dragon")]
        [DisplayName("DG_ArrowColor")]
        [DefaultValue(typeof(Color), "White")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_ArrowColor
        {
            get => _arrowColor;
            set { if (_arrowColor == value) return; _arrowColor = value; if (IsHandleCreated) Invalidate(); }
        }

        [Category("Dragon")]
        [DisplayName("DG_BorderSize")]
        [DefaultValue(1)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int DG_BorderSize
        {
            get => _borderSize;
            set { value = Math.Max(0, value); if (_borderSize == value) return; _borderSize = value; if (IsHandleCreated) Invalidate(); }
        }

        [Category("Dragon")]
        [DisplayName("DG_ArrowWidth")]
        [DefaultValue(3f)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public float DG_ArrowWidth
        {
            get => _arrowWidth;
            set { if (_arrowWidth == value) return; _arrowWidth = value; if (IsHandleCreated) Invalidate(); }
        }

        [Category("Dragon")]
        [DisplayName("DG_ArrowHeight")]
        [DefaultValue(1.5f)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public float DG_ArrowHeight
        {
            get => _arrowHeight;
            set { if (_arrowHeight == value) return; _arrowHeight = value; if (IsHandleCreated) Invalidate(); }
        }

        public ComboBoxCustomFlatN()
        {
            // 🔥 QUAN TRỌNG: Phải dùng OwnerDrawFixed mới set được màu nền!
            DrawMode = DrawMode.OwnerDrawFixed;
            DropDownStyle = ComboBoxStyle.DropDownList;
            FlatStyle = FlatStyle.Flat;

            
            
            ApplyTheme();
        }

        public void ApplyTheme()
        {
            if (IsDisposed) return;

            BackColor = ThemeHelper.BackNormalFirst;   // Ví dụ: trắng
            ForeColor = ThemeHelper.ForeNormalFirst;   // Ví dụ: đen
            _borderColor = ThemeHelper.BorderNormalFist;
            _arrowColor = ThemeHelper.ForeNormalFirst; // hoặc chọn màu khác để nổi bật


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

            // Vẽ nền item
            e.DrawBackground();

            // Vẽ text item
            using (Brush textBrush = new SolidBrush(ForeColor))
            {
                if (e.Font != null)
                    e.Graphics.DrawString(Items[e.Index]?.ToString(), e.Font, textBrush, e.Bounds);
            }

            // Vẽ focus rectangle nếu cần
            e.DrawFocusRectangle();
        }


        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == 0xF) // WM_PAINT
            {
                using (Graphics g = Graphics.FromHwnd(Handle))
                {
                    Rectangle rect = new Rectangle(0, 0, Width, Height);

                    // Nếu borderSize > 0 thì mới vẽ border
                    if (_borderSize > 0)
                    {
                        using (Pen pen = new Pen(_borderColor, _borderSize))
                        {
                            g.DrawRectangle(pen, rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
                        }
                    }
                    else
                    {
                        // Nếu borderSize = 0 thì fill lại bằng màu nền để che border mặc định
                        using (Brush b = new SolidBrush(BackColor))
                        {
                            g.FillRectangle(b, rect);
                        }
                    }

                    int buttonWidth = SystemInformation.VerticalScrollBarWidth;
                    Rectangle dropDownRect = new Rectangle(
                        Width - buttonWidth - 1,
                        1,
                        buttonWidth,
                        Height - 2
                    );

                    using (Brush b = new SolidBrush(BackColor))
                    {
                        g.FillRectangle(b, dropDownRect);
                    }

                    float midX = rect.Right - rect.Height / 2f;
                    float midY = rect.Top + rect.Height / 2f;

                    PointF[] arrow = new PointF[]
                    {
                new PointF(midX - _arrowWidth, midY - _arrowHeight),
                new PointF(midX + _arrowWidth, midY - _arrowHeight),
                new PointF(midX, midY + _arrowHeight)
                    };

                    using (Brush brush = new SolidBrush(_arrowColor))
                    {
                        g.FillPolygon(brush, arrow);
                    }
                }
            }
        }

    }
}