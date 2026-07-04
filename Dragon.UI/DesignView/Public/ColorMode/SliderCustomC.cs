using System.ComponentModel;

namespace Dragon.DesignView.Public.ColorMode
{
    public class SliderCustomC : Control, IThemeable
    {
        private readonly Font _valueFont;

        private int _value = 50;
        private bool _isDragging = false;
        private bool _isDraggingEnabled = true;
        private const int TextPaddingRight = 40;

        private bool _isModeWhiteBlack = false;
        private int _minimum = 0;
        private int _maximum = 100;
        private int _step = 1;

        private Color _trackColor = Color.LightGray;
        private Color _filledColor = Color.Orange;
        private Color _thumbColor = Color.Red;
        private Color _textValueColor = Color.White;

        public event EventHandler? DragCompleted;
        public event EventHandler? ValueChanged;
        public event EventHandler? DraggingEnabledChanged;

        // ===== DG_ properties =====
        [Category("Appearance")]
        [DefaultValue(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool DG_IsModeWhiteBlack
        {
            get => _isModeWhiteBlack;
            set { if (_isModeWhiteBlack == value) return; _isModeWhiteBlack = value; ApplyTheme(); if (IsHandleCreated) Invalidate(); }
        }

        [Category("Behavior")]
        [DefaultValue(0)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int DG_Minimum
        {
            get => _minimum;
            set { if (_minimum == value) return; _minimum = value; if (_value < _minimum) Value = _minimum; }
        }

        [Category("Behavior")]
        [DefaultValue(100)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int DG_Maximum
        {
            get => _maximum;
            set { if (_maximum == value) return; _maximum = value; if (_value > _maximum) Value = _maximum; }
        }

        [Category("Behavior")]
        [DefaultValue(1)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int DG_Step
        {
            get => _step;
            set { value = Math.Max(1, value); if (_step == value) return; _step = value; }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(Color), "LightGray")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_TrackColor
        {
            get => _trackColor;
            set { if (_trackColor == value) return; _trackColor = value; if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(Color), "Orange")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_FilledColor
        {
            get => _filledColor;
            set { if (_filledColor == value) return; _filledColor = value; if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(Color), "Red")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_ThumbColor
        {
            get => _thumbColor;
            set { if (_thumbColor == value) return; _thumbColor = value; if (IsHandleCreated) Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(Color), "White")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_TextValueColor
        {
            get => _textValueColor;
            set { if (_textValueColor == value) return; _textValueColor = value; if (IsHandleCreated) Invalidate(); }
        }

        [Category("Behavior")]
        [DefaultValue(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool DG_IsDraggingEnabled
        {
            get => _isDraggingEnabled;
            set
            {
                if (_isDraggingEnabled == value) return;
                _isDraggingEnabled = value;
                DraggingEnabledChanged?.Invoke(this, EventArgs.Empty);
                if (IsHandleCreated) Invalidate();
            }
        }

        // ===== Value chuẩn (fix lỗi inaccessible) =====
        [Category("Behavior")]
        [Description("Current slider value")]
        [DefaultValue(50)]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Value
        {
            get => _value;
            set
            {
                int clamped = Math.Max(_minimum, Math.Min(_maximum, value));
                if (_value != clamped)
                {
                    _value = clamped;
                    if (IsHandleCreated) Invalidate();
                    ValueChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public SliderCustomC()
        {
            DoubleBuffered = true;
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor,
                true);

            Height = 40;
            Width = 250;
            BackColor = Color.White;
            TabStop = true;

            _valueFont = new Font("Segoe UI", 9, FontStyle.Bold);
            ApplyTheme();
        }

        public void ApplyTheme()
        {
            if (IsDisposed) return;

            if (ThemeHelper.CurrentMode == ThemeMode.Light)
            {
                if (!_isModeWhiteBlack)
                {
                    BackColor = ThemeHelper.BackNormalFirst;
                    _trackColor = ThemeHelper.BackNormal3rd;
                    _textValueColor = ThemeHelper.ForeNormalFirst;

                    if (ThemeHelper.CurrentStyle == ThemeStyle.ThemeRed)
                        _filledColor = _thumbColor = ThemeHelper.ForceColoful2nd;
                    else if (ThemeHelper.CurrentStyle == ThemeStyle.ThemeGreen)
                        _filledColor = _thumbColor = ThemeHelper.ForceColoful4th;
                    else if (ThemeHelper.CurrentStyle == ThemeStyle.ThemeGold)
                        _filledColor = _thumbColor = ThemeHelper.ForceColoful2nd;
                }
            }
            else
            {
                if (!_isModeWhiteBlack)
                {
                    BackColor = ThemeHelper.BackNormalFirst;
                    _textValueColor = ThemeHelper.ForeNormalFirst;

                    if (ThemeHelper.CurrentStyle == ThemeStyle.ThemeRed)
                    {
                        _filledColor = _thumbColor = ThemeHelper.ForceColofulFist;
                        _trackColor = ThemeHelper.ForeNormal3rd;
                    }
                    else if (ThemeHelper.CurrentStyle == ThemeStyle.ThemeGreen)
                    {
                        _filledColor = _thumbColor = ThemeHelper.ForceColoful4th;
                        _trackColor = ThemeHelper.BackNormal3rd;
                    }
                    else if (ThemeHelper.CurrentStyle == ThemeStyle.ThemeGold)
                    {
                        _filledColor = _thumbColor = ThemeHelper.BorderColoful2nd;
                        _trackColor = ThemeHelper.BackNormal3rd;
                    }
                }
            }
            Invalidate();
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            if (IsDisposed) return;
            using var brush = new SolidBrush(BackColor);
            pevent.Graphics.FillRectangle(brush, ClientRectangle);
        }

        protected override bool IsInputKey(Keys keyData) =>
            keyData is Keys.Left or Keys.Right or Keys.Up or Keys.Down || base.IsInputKey(keyData);

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (IsDisposed || !_isDraggingEnabled) return;
            base.OnKeyDown(e);
            switch (e.KeyCode)
            {
                case Keys.Left:
                case Keys.Down: Value -= _step; e.Handled = true; break;
                case Keys.Right:
                case Keys.Up: Value += _step; e.Handled = true; break;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (IsDisposed) return;
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            int trackY = Height / 2 - 2;
            int trackWidth = Width - 20 - TextPaddingRight;
            var trackRect = new Rectangle(10, trackY, trackWidth, 4);
            using (var b = new SolidBrush(_trackColor))
                g.FillRectangle(b, trackRect);

            float percent = (_maximum == _minimum) ? 0 : (_value - _minimum) / (float)(_maximum - _minimum);
            int filledWidth = (int)(percent * trackWidth);
            var filledRect = new Rectangle(10, trackY, filledWidth, 4);
            using (var b = new SolidBrush(_filledColor))
                g.FillRectangle(b, filledRect);

            int thumbX = 10 + filledWidth;
            var thumbRect = new Rectangle(thumbX - 7, trackY - 6, 14, 14);
            using (var b = new SolidBrush(_thumbColor))
                g.FillEllipse(b, thumbRect);

            string text = FormatShortNumber(_value);
            using var brush = new SolidBrush(_textValueColor);
            var textSize = g.MeasureString(text, ThemeHelper.SharedValueFont);
            float textX = Width - textSize.Width;
            float textY = Height / 2 - textSize.Height / 2;
            g.DrawString(text, ThemeHelper.SharedValueFont, brush, textX, textY);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (IsDisposed || !_isDraggingEnabled) return;
            _isDragging = true;
            Capture = true;
            SetValueFromMouse(e.X);
            Focus();
            Invalidate();
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (IsDisposed || !_isDraggingEnabled || !_isDragging || e.Button != MouseButtons.Left) return;
            SetValueFromMouse(e.X);
            Invalidate();
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (IsDisposed) return;
            if (_isDragging)
            {
                _isDragging = false;
                Capture = false;
                SetValueFromMouse(e.X);
                DragCompleted?.Invoke(this, EventArgs.Empty);
            }
            Invalidate();
            base.OnMouseUp(e);
        }

        protected override void OnMouseCaptureChanged(EventArgs e)
        {
            if (IsDisposed) return;
            if (_isDragging)
            {
                _isDragging = false;
                Capture = false;
                DragCompleted?.Invoke(this, EventArgs.Empty);
            }
            base.OnMouseCaptureChanged(e);
        }

        private void SetValueFromMouse(int mouseX)
        {
            int usableWidth = Width - 20 - TextPaddingRight;
            int x = mouseX - 10;
            float pct = Math.Max(0, Math.Min(1, x / (float)usableWidth));

            if (_maximum == _minimum || _step <= 0)
            {
                Value = _minimum;
                return;
            }

            int raw = _minimum + (int)(pct * (_maximum - _minimum));
            int stepV = (int)Math.Round(raw / (double)_step) * _step;
            Value = Math.Max(_minimum, Math.Min(_maximum, stepV));
        }

        private string FormatShortNumber(int value)
        {
            if (value >= 1_000_000) return (value / 1_000_000.0).ToString("0.#") + "M";
            if (value >= 1_000) return (value / 1_000.0).ToString("0.#") + "K";
            return value.ToString();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _valueFont?.Dispose();
            base.Dispose(disposing);
        }
    }
}