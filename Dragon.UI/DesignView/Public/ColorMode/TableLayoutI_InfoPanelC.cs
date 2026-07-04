using System.ComponentModel;


namespace Dragon.DesignView.Public.ColorMode
{
    public class TableLayoutI_InfoPanelC : TableLayoutPanel, IThemeable
    {
        private bool _isNormalBack = false;
        // thêm field
        [Category("Dragon")]
        [DefaultValue(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool DG_IsNormalBack
        {
            get => _isNormalBack;
            set { if (_isNormalBack == value) return; _isNormalBack = value; ApplyTheme(); }
        }
        public TableLayoutI_InfoPanelC()
        {
            AutoSize = true;
            ColumnCount = 2;
            RowCount = 0;
            Dock = DockStyle.Fill;

            ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            CellBorderStyle = TableLayoutPanelCellBorderStyle.None;

            
             // dùng field
            ApplyTheme();
        }

        public Label AddLabel(string title, string value)
        {
            var lblTitle = CreateTitleLabel(title);
            var lblValue = CreateValueLabel(value);
            AddRow(lblTitle, lblValue);
            return lblValue;
        }

        public void AddTextBox(string title, string defaultValue = "")
        {
            var lblTitle = CreateTitleLabel(title);
            var txtValue = new TextBox
            {
                Text = defaultValue,
                BorderStyle = BorderStyle.None,
                Dock = DockStyle.Fill
            };
            AddRow(lblTitle, txtValue);
        }

        public AntdUI.Dropdown AddComboBox(string title, IEnumerable<object> items, string? selectedItem = null)
        {
            var lblTitle = CreateTitleLabel(title);
            var comboValue = new AntdUI.Dropdown
            {
                AutoSizeMode = AntdUI.TAutoSize.Auto,
                Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point, 0),
                IsLink = true,
                ShowArrow = true,
                Dock = DockStyle.Fill,
                Width = 100,
                Type = AntdUI.TTypeMini.Primary
            };
            comboValue.Items.AddRange(items.ToArray());

            AddRow(lblTitle, comboValue);

            return comboValue;
        }

        private void AddRow(Control title, Control value)
        {
            ApplyTheme(title);
            ApplyTheme(value);

            value.Click += (e, s) =>
            {
                Clipboard.SetText(value.Text);
                MessageBox.Show("Copied to clipboard", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            RowCount++;
            RowStyles.Add(new RowStyle(SizeType.AutoSize));
            Controls.Add(title, 0, RowCount - 1);
            Controls.Add(value, 1, RowCount - 1);
        }

        public void ApplyTheme()
        {
            ApplyThemeToAllControls();
        }
        public void ApplyTheme(Control control)
        {
            if (_isNormalBack)
            {
                control.ForeColor = ThemeHelper.ForeNormalFirst;
                control.BackColor = ThemeHelper.BackNormalFirst;
            }
            else
            {
                if (ThemeHelper.CurrentMode == ThemeMode.Light)
                {
                    control.BackColor = ThemeHelper.BackNormalFirst;
                    control.ForeColor = ThemeHelper.CurrentStyle switch
                    {
                        ThemeStyle.ThemeRed => ThemeHelper.ForceColoful4th,
                        ThemeStyle.ThemeGreen => ThemeHelper.ForceColofulFist,
                        ThemeStyle.ThemeGold => ThemeHelper.ForceColoful2nd,
                        _ => ThemeHelper.ForceColofulFist
                    };
                }
                else
                {
                    control.BackColor = ThemeHelper.BackColofulFirst;
                    control.ForeColor = ThemeHelper.CurrentStyle switch
                    {
                        ThemeStyle.ThemeRed => ThemeHelper.ForceColofulFist,
                        ThemeStyle.ThemeGreen => ThemeHelper.ForceColoful2nd,
                        ThemeStyle.ThemeGold => ThemeHelper.ForceColoful3rd,
                        _ => ThemeHelper.ForceColofulFist
                    };
                }
            }
        }
        private void ApplyThemeToAllControls()
        {
            foreach (Control control in Controls)
            {
                ApplyTheme(control);
            }
            Invalidate();
        }

        private Label CreateTitleLabel(string text) => new Label
        {
            Text = text,
            AutoSize = true,
            TextAlign = ContentAlignment.MiddleLeft,
            Dock = DockStyle.Fill,
            Padding = new Padding(0, 3, 0, 3)
        };

        private Label CreateValueLabel(string text) => new Label
        {
            Text = text,
            AutoSize = true,
            TextAlign = ContentAlignment.MiddleLeft,
            Dock = DockStyle.Fill,
            Padding = new Padding(0, 3, 0, 3)
        };

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            base.Dispose(disposing);
        }
    }
}