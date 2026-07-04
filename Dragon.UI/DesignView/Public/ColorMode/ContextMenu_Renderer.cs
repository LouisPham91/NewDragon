
using Dragon.ControlHelper;
using Dragon.DesignView.Public.NormalMode;
using System.ComponentModel;

namespace Dragon.DesignView.Public.ColorMode
{
    // ==================== MyLabeledToolStripOnlyCombox ====================
    public class MyLabeledToolStripOnlyCombox : ToolStripControlHost, IThemeable
    {
        private readonly int _inputWidth;
        private readonly int _controlHeight;
        private readonly Panel _panel;
        public ComboBoxCustomNC ComboBox { get; }

        // 🔥 Theme handler
        

        public MyLabeledToolStripOnlyCombox(string inputText, int inputWidth = 100, int controlHeight = 23,
            DockStyle dockStyle = DockStyle.Fill, int padding = 8, string SVG = "") : base(new Panel())
        {
            _inputWidth = inputWidth;
            _controlHeight = controlHeight;

            AutoSize = false;

            ComboBox = new ComboBoxCustomNC
            {
                Font = new Font("Segoe UI", 8.0F, FontStyle.Regular, GraphicsUnit.Point, 0),
                Dock = dockStyle,
                Width = _inputWidth,
                Height = _controlHeight,
                Text = inputText,
            };

            _panel = Control as Panel ?? throw new InvalidOperationException("Panel host không khởi tạo được.");
            _panel.Margin = Padding.Empty;
            _panel.Controls.Add(ComboBox);
            _panel.Height = _controlHeight;
            _panel.Width = _inputWidth + padding;

            // 🔥 Subscribe theme
            
            
            ApplyTheme();
        }

        public void SetText(string text, int minWidth = 50, int minHeight = 23)
        {
            if (IsDisposed) return;
            ComboBox.Text = text;

            using (Graphics g = ComboBox.CreateGraphics())
            {
                Size textSize = TextRenderer.MeasureText(g, text, ComboBox.Font,
                    new Size(int.MaxValue, int.MaxValue), TextFormatFlags.WordBreak);

                int width = Math.Max(textSize.Width + 10, minWidth);
                int height = Math.Max(textSize.Height + 4, minHeight);

                _panel.Width = width;
                _panel.Height = height;
                ComboBox.Width = width;
                ComboBox.Height = height;

                Size = _panel.Size;
            }
        }

        public void ApplyTheme()
        {
            if (IsDisposed) return;

            if (ThemeHelper.CurrentMode == ThemeMode.Light)
            {
                _panel.BackColor = ThemeHelper.BackNormalFirst;
                ComboBox.BackColor = ThemeHelper.BackNormalFirst;
                ComboBox.ForeColor = ThemeHelper.CurrentStyle switch
                {
                    ThemeStyle.ThemeRed => ThemeHelper.ForceColoful4th,
                    ThemeStyle.ThemeGreen => ThemeHelper.ForceColofulFist,
                    ThemeStyle.ThemeGold => ThemeHelper.ForceColoful2nd,
                    _ => ThemeHelper.ForceColofulFist
                };
            }
            else
            {
                _panel.BackColor = ThemeHelper.BackColofulFirst;
                ComboBox.BackColor = ThemeHelper.BackColofulFirst;
                ComboBox.ForeColor = ThemeHelper.CurrentStyle switch
                {
                    ThemeStyle.ThemeRed => ThemeHelper.ForceColofulFist,
                    ThemeStyle.ThemeGreen => ThemeHelper.ForceColoful2nd,
                    ThemeStyle.ThemeGold => ThemeHelper.ForceColoful3rd,
                    _ => ThemeHelper.ForceColofulFist
                };
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                
            base.Dispose(disposing);
        }
    }

    // ==================== MyLabeledToolStripOnlyTextBox ====================
    public class MyLabeledToolStripOnlyTextBox : ToolStripControlHost, IThemeable
    {
        private readonly int _inputWidth;
        private readonly int _controlHeight;
        private readonly Panel _panel;
        public TextBox TextBoxPart { get; }

        // 🔥 Theme handler
        

        public MyLabeledToolStripOnlyTextBox(string inputText, int inputWidth = 100, int controlHeight = 23) : base(new Panel())
        {
            _inputWidth = inputWidth;
            _controlHeight = controlHeight;

            AutoSize = false;
            Padding = Padding.Empty;
            Margin = Padding.Empty;

            TextBoxPart = new TextBox
            {
                BorderStyle = BorderStyle.None,
                Dock = DockStyle.Fill,
                Width = _inputWidth,
                Height = _controlHeight,
                Text = inputText,
                TextAlign = HorizontalAlignment.Left,
                WordWrap = true,
                Multiline = true,
            };

            _panel = Control as Panel ?? throw new InvalidOperationException("Panel host không khởi tạo được.");
            _panel.Margin = Padding.Empty;
            _panel.Controls.Add(TextBoxPart);
            _panel.Height = _controlHeight;

            // 🔥 Subscribe theme
            
            
            ApplyTheme();

            UpdateHostSize();
        }

        public void ApplyTheme()
        {
            if (IsDisposed) return;

            if (ThemeHelper.CurrentMode == ThemeMode.Light)
            {
                _panel.BackColor = ThemeHelper.BackNormalFirst;
                TextBoxPart.BackColor = ThemeHelper.BackNormalFirst;
                TextBoxPart.ForeColor = ThemeHelper.CurrentStyle switch
                {
                    ThemeStyle.ThemeRed => ThemeHelper.ForceColoful4th,
                    ThemeStyle.ThemeGreen => ThemeHelper.ForceColofulFist,
                    ThemeStyle.ThemeGold => ThemeHelper.ForceColoful2nd,
                    _ => ThemeHelper.ForceColofulFist
                };
            }
            else
            {
                _panel.BackColor = ThemeHelper.BackColofulFirst;
                TextBoxPart.BackColor = ThemeHelper.BackColofulFirst;
                TextBoxPart.ForeColor = ThemeHelper.CurrentStyle switch
                {
                    ThemeStyle.ThemeRed => ThemeHelper.ForceColofulFist,
                    ThemeStyle.ThemeGreen => ThemeHelper.ForceColoful2nd,
                    ThemeStyle.ThemeGold => ThemeHelper.ForceColoful3rd,
                    _ => ThemeHelper.ForceColofulFist
                };
            }
        }

        public void SetText(string text, int minWidth = 50, int minHeight = 23)
        {
            if (IsDisposed) return;
            TextBoxPart.Text = text;

            using (Graphics g = TextBoxPart.CreateGraphics())
            {
                Size textSize = TextRenderer.MeasureText(g, text, TextBoxPart.Font,
                    new Size(int.MaxValue, int.MaxValue), TextFormatFlags.WordBreak);

                int width = Math.Max(textSize.Width + 10, minWidth);
                int height = Math.Max(textSize.Height + 4, minHeight);

                _panel.Width = width;
                _panel.Height = height;
                TextBoxPart.Width = width;
                TextBoxPart.Height = height;

                Size = _panel.Size;
            }
        }

        private void UpdateHostSize()
        {
            var panel = Control as Panel;
            if (panel == null) return;

            int arrowGutter = 20;
            panel.Width = _inputWidth + arrowGutter;
            panel.Height = _controlHeight;
            Size = panel.Size;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                
            base.Dispose(disposing);
        }
    }

    // ==================== MyLabeledToolStripTextBox ====================
    public class MyLabeledToolStripTextBox : ToolStripControlHost, IThemeable
    {
        private readonly int _inputWidth;
        private readonly int _controlHeight;
       

        public Label LabelPart { get; }
        public TextBox TextBoxPart { get; }

        public MyLabeledToolStripTextBox(string initialLabel, int inputWidth = 100, int controlHeight = 23) : base(new Panel())
        {
            _inputWidth = inputWidth;
            _controlHeight = controlHeight;

            AutoSize = false;

            TextBoxPart = new TextBox
            {
                BorderStyle = BorderStyle.None,
                Dock = DockStyle.Fill,
                Width = _inputWidth,
                Height = _controlHeight,
                Text = "Test",
                TextAlign = HorizontalAlignment.Left,
            };

            var panelRight = new Panel
            {
                Dock = DockStyle.Right,
                Width = _inputWidth,
                Padding = new Padding(0, 4, 0, 0)
            };
            panelRight.Controls.Add(TextBoxPart);

            LabelPart = new Label
            {
                Text = initialLabel,
                AutoSize = false,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Height = _controlHeight
            };

            var panel = Control as Panel;
            if (panel == null) return;

            panel.Margin = Padding.Empty;
            panel.Controls.Add(panelRight);
            panel.Controls.Add(LabelPart);
            ApplyTheme();
            UpdateHostSize();
        }

        [Category("Appearance")]
        [DisplayName("DG_Text")]
        [Description("Text hiển thị trên LabelPart")]
        [DefaultValue("")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string DG_Text
        {
            get => LabelPart.Text;
            set
            {
                var v = value ?? string.Empty;
                if (LabelPart.Text == v) return;
                LabelPart.Text = v;
                UpdateHostSize();
            }
        }


        public void ApplyTheme()
        {
            if (IsDisposed) return;
            var panel = Control as Panel;
            if (panel == null) return;

            if (ThemeHelper.CurrentMode == ThemeMode.Light)
            {
                var backColor = ThemeHelper.BackNormalFirst;
                var foreColor = ThemeHelper.CurrentStyle switch
                {
                    ThemeStyle.ThemeRed => ThemeHelper.ForceColoful4th,
                    ThemeStyle.ThemeGreen => ThemeHelper.ForceColofulFist,
                    ThemeStyle.ThemeGold => ThemeHelper.ForceColoful2nd,
                    _ => ThemeHelper.ForceColofulFist
                };
                panel.BackColor = backColor;
                LabelPart.BackColor = backColor;
                LabelPart.ForeColor = foreColor;
                TextBoxPart.BackColor = backColor;
                TextBoxPart.ForeColor = foreColor;
            }
            else
            {
                var backColor = ThemeHelper.BackColofulFirst;
                var foreColor = ThemeHelper.CurrentStyle switch
                {
                    ThemeStyle.ThemeRed => ThemeHelper.ForceColofulFist,
                    ThemeStyle.ThemeGreen => ThemeHelper.ForceColoful2nd,
                    ThemeStyle.ThemeGold => ThemeHelper.ForceColoful3rd,
                    _ => ThemeHelper.ForceColofulFist
                };
                panel.BackColor = backColor;
                LabelPart.BackColor = backColor;
                LabelPart.ForeColor = foreColor;
                TextBoxPart.BackColor = backColor;
                TextBoxPart.ForeColor = foreColor;
            }
        }

        private void UpdateHostSize()
        {
            var panel = Control as Panel;
            if (panel == null) return;
            var labelSize = TextRenderer.MeasureText(LabelPart.Text, LabelPart.Font,
                new Size(int.MaxValue, _controlHeight), TextFormatFlags.WordBreak);
            panel.Width = _inputWidth + labelSize.Width + 8;
            panel.Height = _controlHeight;
            Size = panel.Size;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            base.Dispose(disposing);
        }
    }

    // ==================== MyToolStripMenuItem ====================
    public class MyToolStripMenuItem : ToolStripMenuItem, IThemeable
    {
        private string _svgString = string.Empty;
        private bool _isNormalToolstrip = false;

        [Category("Appearance")]
        [Description("SVG content trực tiếp")]
        [DefaultValue("")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string DG_SVGString
        {
            get => _svgString;
            set { value ??= string.Empty; if (_svgString == value) return; _svgString = value; UpdateImage(); }
        }

        [Category("Appearance")]
        [DefaultValue(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool DG_IsNormalToolstrip
        {
            get => _isNormalToolstrip;
            set { if (_isNormalToolstrip == value) return; _isNormalToolstrip = value; ApplyTheme(); }
        }

        public MyToolStripMenuItem(string? svgString = null) : base()
        {
            if (!string.IsNullOrEmpty(svgString)) _svgString = svgString;
            ApplyTheme();
        }

        public void ApplyTheme()
        {
            if (IsDisposed) return;

            if (_isNormalToolstrip)
            {
                BackColor = ThemeHelper.BackNormalFirst;
                ForeColor = ThemeHelper.ForeNormalFirst;
            }
            else
            {
                if (ThemeHelper.CurrentMode == ThemeMode.Light)
                {
                    BackColor = ThemeHelper.BackNormal3rd;
                    ForeColor = ThemeHelper.CurrentStyle switch
                    {
                        ThemeStyle.ThemeRed => ThemeHelper.ForceColoful4th,
                        ThemeStyle.ThemeGreen => ThemeHelper.ForceColofulFist,
                        ThemeStyle.ThemeGold => ThemeHelper.ForceColoful2nd,
                        _ => ThemeHelper.ForceColofulFist
                    };
                }
                else
                {
                    BackColor = ThemeHelper.BackColofulFirst;
                    ForeColor = ThemeHelper.CurrentStyle switch
                    {
                        ThemeStyle.ThemeRed => ThemeHelper.ForceColofulFist,
                        ThemeStyle.ThemeGreen => ThemeHelper.ForceColoful2nd,
                        ThemeStyle.ThemeGold => ThemeHelper.ForceColoful3rd,
                        _ => ThemeHelper.ForceColofulFist
                    };
                }
            }
            UpdateImage();
        }

        private void UpdateImage()
        {
            if (IsDisposed) return;
            var old = Image;
            if (string.IsNullOrWhiteSpace(_svgString)) Image = null;
            else
            {
                var height = Math.Max(Font.Height - 3, 16);
                Image = SvgRenderer.RenderSvgFromString(_svgString, height, height, ForeColor);
            }
            old?.Dispose();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Image?.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    // ==================== MyContextMenu ====================
    public class MyContextMenu : ContextMenuStrip
    {
        private int _padding = 10;

        [Category("Appearance")]
        [DefaultValue(10)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int DG_Padding
        {
            get => _padding;
            set { if (_padding == value) return; _padding = Math.Max(0, value); if (IsHandleCreated) Invalidate(); }
        }

        public MyContextMenu()
        {
            ShowImageMargin = false;
            ShowCheckMargin = false;
            RenderMode = ToolStripRenderMode.System;
            Font = new Font("Segoe UI", 9f);
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            using var g = CreateGraphics();
            float maxWidth = 0;
            foreach (ToolStripItem item in Items)
            {
                if (item is ToolStripSeparator) continue;
                if (string.IsNullOrEmpty(item.Text)) continue;
                var size = g.MeasureString(item.Text, item.Font);
                if (size.Width > maxWidth) maxWidth = size.Width;
            }
            int width = (int)Math.Ceiling(maxWidth) + _padding;
            var baseSize = base.GetPreferredSize(proposedSize);
            return new Size(width, baseSize.Height);
        }

        protected override void OnLayout(LayoutEventArgs e)
        {
            base.OnLayout(e);
            Invalidate();
        }
    }

    // ==================== ContextMenu_ColorTable ====================
    public class ContextMenu_ColorTable : ProfessionalColorTable
    {
        private Color _customBackColor = Color.FromArgb(97, 53, 49);

        public Color DG_CustomBackColor
        {
            get => _customBackColor;
            set => _customBackColor = value;  // 🔥 Bỏ dòng Invoke event
        }

        public override Color ToolStripDropDownBackground => _customBackColor;
        public override Color MenuItemSelected => _customBackColor;
        public override Color MenuItemBorder => _customBackColor;
        public override Color ImageMarginGradientBegin => _customBackColor;
        public override Color ImageMarginGradientMiddle => _customBackColor;
        public override Color ImageMarginGradientEnd => _customBackColor;
    }

    // ==================== ContextMenu_Renderer ====================
    public class ContextMenu_Renderer : ToolStripProfessionalRenderer, IThemeable
    {
        private bool _isNoImage = false;
        private float _lightenFactor = 0.2f;
        private Color _backColorLeave = Color.FromArgb(97, 53, 49);
        private Color _backColorEnter = Color.FromArgb(120, 53, 49);
        private Color _foreColorLeave = Color.FromArgb(207, 66, 51);
        private static ContextMenu_ColorTable _contextTable = new ContextMenu_ColorTable();

        // 🔥 Theme handler
        

        [Category("Appearance")]
        public bool DG_IsNoImage
        {
            get => _isNoImage;
            set => _isNoImage = value;
        }

        [Category("Appearance")]
        public float DG_LightenFactor
        {
            get => _lightenFactor;
            set
            {
                _lightenFactor = Math.Max(0, Math.Min(1, value));
                ApplyTheme();
            }
        }

        public ContextMenu_Renderer() : base(_contextTable)
        {                            
            ApplyTheme();
        }

        public void ApplyTheme()
        {
            if (ThemeHelper.CurrentMode == ThemeMode.Light)
            {
                _backColorLeave = ThemeHelper.BackNormalFirst;
                _foreColorLeave = ThemeHelper.CurrentStyle switch
                {
                    ThemeStyle.ThemeRed => ThemeHelper.ForceColoful4th,
                    ThemeStyle.ThemeGreen => ThemeHelper.ForceColofulFist,
                    ThemeStyle.ThemeGold => ThemeHelper.ForceColoful2nd,
                    _ => ThemeHelper.ForceColofulFist
                };
                _backColorEnter = ControlPaint.Dark(_backColorLeave, _lightenFactor);
            }
            else
            {
                _backColorLeave = ThemeHelper.BackColofulFirst;
                _foreColorLeave = ThemeHelper.CurrentStyle switch
                {
                    ThemeStyle.ThemeRed => ThemeHelper.ForceColofulFist,
                    ThemeStyle.ThemeGreen => ThemeHelper.ForceColoful2nd,
                    ThemeStyle.ThemeGold => ThemeHelper.ForceColoful3rd,
                    _ => ThemeHelper.ForceColofulFist
                };
                _backColorEnter = ControlPaint.Light(_backColorLeave, _lightenFactor);
            }

            _contextTable.DG_CustomBackColor = _backColorLeave;
        }

        // 🔥 ContextMenu_Renderer không phải Control nên không cần Dispose
        // Nếu cần cleanup, có thể thêm method Unsubscribe()
        public void Unsubscribe()
        {
            
        }

        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            e.Graphics.Clear(_backColorLeave);
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            using (Pen pen = new Pen(_backColorEnter))
            {
                Rectangle rect = new Rectangle(Point.Empty, e.ToolStrip.Size);
                rect.Width -= 1;
                rect.Height -= 1;
                e.Graphics.DrawRectangle(pen, rect);
            }
        }

        protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
        {
            if (_isNoImage)
            {
                e.Graphics.FillRectangle(new SolidBrush(e.ToolStrip.BackColor), e.AffectedBounds);
            }
            else
            {
                var rect = new Rectangle(
                    e.AffectedBounds.X,
                    e.AffectedBounds.Y,
                    e.AffectedBounds.Width,
                    e.AffectedBounds.Height);

                using (var brush = new SolidBrush(_backColorLeave))
                {
                    e.Graphics.FillRectangle(brush, rect);
                }
            }
        }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            Color backColor = e.Item.Selected ? _backColorEnter : _backColorLeave;
            using (SolidBrush brush = new SolidBrush(backColor))
            {
                e.Graphics.FillRectangle(brush, new Rectangle(Point.Empty, e.Item.Size));
            }
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            e.TextColor = _foreColorLeave;
            base.OnRenderItemText(e);
        }
    }
}