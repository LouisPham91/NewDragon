
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Dragon.DesignView.Public.NormalMode
{
    public class NumericUpDownCustomN : Control, IThemeable
    {
        private int _value = 0;
        private int _minimum = 0;
        private int _maximum = 100;
        private TextBox? _inputBox;
        private bool _mouseDownUp = false;
        private bool _mouseDownDown = false;

        private Color _borderColor = Color.FromArgb(61, 61, 61);
        private Color _buttonColor = Color.FromArgb(61, 61, 61);
        private Color _boxColor = Color.FromArgb(61, 61, 61);
        private Color _textColor = Color.White;
        private Color _arrowColor = Color.White;
        private Color _arrowPressedColor = Color.FromArgb(224, 224, 224);

        // 🔥 Theme handler


        [Category("Behavior")]
        [DefaultValue(0)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int Value
        {
            get => _value;
            set
            {
                int clamped = Math.Max(_minimum, Math.Min(_maximum, value));
                if (_value == clamped) return;
                _value = clamped;
                if (_inputBox != null) _inputBox.Text = _value.ToString();
                if (IsHandleCreated) Invalidate();
            }
        }

        [Category("Behavior")]
        [DisplayName("DG_Minimum")]
        [DefaultValue(0)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int DG_Minimum
        {
            get => _minimum;
            set { if (_minimum == value) return; _minimum = value; if (_value < _minimum) Value = _minimum; }
        }

        [Category("Behavior")]
        [DisplayName("DG_Maximum")]
        [DefaultValue(100)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int DG_Maximum
        {
            get => _maximum;
            set { if (_maximum == value) return; _maximum = value; if (_value > _maximum) Value = _maximum; }
        }

        [Category("Appearance")]
        [DisplayName("DG_BorderColor")]
        [DefaultValue(typeof(Color), "61, 61, 61")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_BorderColor
        {
            get => _borderColor;
            set { if (_borderColor == value) return; _borderColor = value; if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [DisplayName("DG_ButtonColor")]
        [DefaultValue(typeof(Color), "61, 61, 61")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_ButtonColor
        {
            get => _buttonColor;
            set { if (_buttonColor == value) return; _buttonColor = value; if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [DisplayName("DG_BoxColor")]
        [DefaultValue(typeof(Color), "61, 61, 61")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_BoxColor
        {
            get => _boxColor;
            set { if (_boxColor == value) return; _boxColor = value; if (_inputBox != null) _inputBox.BackColor = value; if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [DisplayName("DG_TextColor")]
        [DefaultValue(typeof(Color), "White")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_TextColor
        {
            get => _textColor;
            set { if (_textColor == value) return; _textColor = value; if (_inputBox != null) _inputBox.ForeColor = value; if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [DisplayName("DG_ArrowColor")]
        [DefaultValue(typeof(Color), "White")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_ArrowColor
        {
            get => _arrowColor;
            set { if (_arrowColor == value) return; _arrowColor = value; if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [DisplayName("DG_ArrowPressedColor")]
        [DefaultValue(typeof(Color), "224, 224, 224")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_ArrowPressedColor
        {
            get => _arrowPressedColor;
            set { if (_arrowPressedColor == value) return; _arrowPressedColor = value; if (IsHandleCreated) Invalidate(); }
        }

        public NumericUpDownCustomN()
        {
            Size = new Size(100, 30);

            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     
                     ControlStyles.SupportsTransparentBackColor, true);

            _inputBox = new TextBox
            {
                BorderStyle = BorderStyle.None,
                TextAlign = HorizontalAlignment.Center,
                Font = Font,
                TabStop = true
            };
            _inputBox.Text = _value.ToString();
            _inputBox.Leave += InputBox_Leave;
            _inputBox.KeyPress += InputBox_KeyPress;

            Controls.Add(_inputBox);
            LayoutInputBox();

            // 🔥 Khởi tạo ngay để tránh warning CS8618
            
            
            ApplyTheme();
        }

        public void ApplyTheme()
        {
            if (IsDisposed) return;

            // dùng đúng tên trong ThemeHelper
            _borderColor = ThemeHelper.BorderNormalFist;   // 61 dark / 168 light
            _buttonColor = ThemeHelper.BackNormal2nd;     // 31 dark / 205 light
            _boxColor = ThemeHelper.BackNormalFirst;   // 40 dark / 245 light
            _textColor = ThemeHelper.ForeNormalFirst;   // White / Black
            _arrowColor = ThemeHelper.ForeNormalFirst;
            _arrowPressedColor = ThemeHelper.ForeNormal2nd;     // 240 dark / 31 light
            BackColor = ThemeHelper.BackNormalFirst;

            if (_inputBox != null)
            {
                _inputBox.BackColor = _boxColor;
                _inputBox.ForeColor = _textColor;
                _inputBox.BorderStyle = BorderStyle.None;
            }
            Invalidate();
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
                
            base.Dispose(disposing);
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (_inputBox != null)
            {
                _inputBox.BackColor = _boxColor;
                _inputBox.ForeColor = _textColor;
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            LayoutInputBox();
            Invalidate();
        }

        private void LayoutInputBox()
        {
            if (_inputBox == null) return;

            int padding = 4;
            int buttonWidth = 20;
            _inputBox.Location = new Point(padding, padding);
            _inputBox.Size = new Size(Width - buttonWidth - 2 * padding, Height - 2 * padding);
        }

        private void InputBox_Leave(object? sender, EventArgs e)
        {
            if (int.TryParse(_inputBox?.Text, out int v))
            {
                Value = v;
                _inputBox!.Text = _value.ToString();
            }
            else
            {
                _inputBox!.Text = _value.ToString();
            }
        }

        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);
            if (_inputBox != null) _inputBox.BackColor = _boxColor;
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);
            if (_inputBox != null) _inputBox.ForeColor = _textColor;
        }

        private void InputBox_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;

            if (e.KeyChar == (char)Keys.Enter)
            {
                InputBox_Leave(_inputBox, EventArgs.Empty);
                e.Handled = true;
                Focus();
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (IsDisposed) return;
            base.OnMouseDown(e);

            if (GetUpButtonRect().Contains(e.Location))
            {
                _mouseDownUp = true;
                Value++;
            }
            else if (GetDownButtonRect().Contains(e.Location))
            {
                _mouseDownDown = true;
                Value--;
            }

            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (IsDisposed) return;
            base.OnMouseUp(e);
            _mouseDownUp = false;
            _mouseDownDown = false;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (IsDisposed) return;
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.Clear(_boxColor);

            using (Pen pen = new Pen(_borderColor, 1))
            {
                g.DrawRectangle(pen, new Rectangle(0, 0, Width - 1, Height - 1));
            }

            DrawArrowButton(g, GetUpButtonRect(), true, _mouseDownUp);
            DrawArrowButton(g, GetDownButtonRect(), false, _mouseDownDown);
        }

        private Rectangle GetUpButtonRect()
        {
            return new Rectangle(Width - 20, 1, 19, Height / 2 - 1);
        }

        private Rectangle GetDownButtonRect()
        {
            return new Rectangle(Width - 20, Height / 2 - 1, 19, Height / 2 - 1);
        }

        private void DrawArrowButton(Graphics g, Rectangle rect, bool isUp, bool isPressed)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.FillRectangle(new SolidBrush(BackColor), rect);

            Point[] triangle = GetTrianglePoints(rect, isUp);
            using (SolidBrush arrowBrush = new SolidBrush(isPressed ? _arrowPressedColor : _arrowColor))
            {
                g.FillPolygon(arrowBrush, triangle);
            }
        }

        private Point[] GetTrianglePoints(Rectangle rect, bool isUp)
        {
            int cx = rect.X + rect.Width / 2;
            int cy = rect.Y + rect.Height / 2;
            int size = Math.Min(rect.Width, rect.Height) / 3;

            if (isUp)
            {
                return new Point[]
                {
                    new Point(cx - size, cy + size / 2),
                    new Point(cx + size, cy + size / 2),
                    new Point(cx, cy - size)
                };
            }
            else
            {
                return new Point[]
                {
                    new Point(cx - size, cy - size / 2),
                    new Point(cx + size, cy - size / 2),
                    new Point(cx, cy + size)
                };
            }
        }
    }
}