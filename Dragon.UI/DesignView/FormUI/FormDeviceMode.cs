

using Dragon.ControlHelper.UIController;
using Dragon.Controller.GlobalControl.Property;
using Dragon.DesignView.Public;
using Dragon.DesignView.Public.ColorMode;
using Dragon.DesignView.Public.NormalMode;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;


namespace Dragon.DesignView.FormUI
{
    public partial class FormDeviceMode : Form
    {
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private float _roundRadius = 15f;
        private Color _borderColor = Color.FromArgb(168, 168, 168);
        public Size originalSize;
        public Point originalLocation;
        public bool isMaximized = false;

        public FormDeviceMode()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);
            UpdateStyles();

            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            // Dock panelMain để tránh resize liên tục
            panelMain.Dock = DockStyle.Fill;

            EnableFormDrag(panel1);
            ThemeHelper.ThemeChanged += OnGlobalThemeChanged;
            toggleDisplayTitle.Toggled += ToggleDisplayTitle_Toggled;
            toggleDisplayIP.Toggled += ToggleDisplayIP_Toggled;
            toggleDisplayModel.Toggled += ToggleDisplayModel_Toggled;
            toggleSerial.Toggled += ToggleSerial_Toggled;
            LoadAllTogglesFromSettings();
        }


        private void OnGlobalThemeChanged(object? s, EventArgs e)
        {
            if (IsDisposed || !IsHandleCreated) return;
            BeginInvoke(new Action(ApplyTheme)); // tránh cross-thread
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ApplyTheme();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            // Chỉ set Region sau khi form đã hiển thị
            SetRoundedRegion();
            SetPanelMainRoundedRegion();
        }

        public void ApplyTheme()
        {
            SuspendLayout();
           
            BackColor = ThemeHelper.BorderNormalFist;

            if (ThemeHelper.CurrentMode == ThemeMode.Light)
            {
                ForeColor = ThemeHelper.ForeNormalFirst;
                panelMain.ForeColor = ThemeHelper.ForeNormalFirst;
                labelLightScreen1.Text = "       Light Screen";
                labelLightScreen2.Text = "       Light Screen";
            }
            else
            {
                ForeColor = ThemeHelper.ForeNormal3rd;
                panelMain.ForeColor = ThemeHelper.ForeNormal3rd;
                labelLightScreen1.Text = "       Dark Screen";
                labelLightScreen2.Text = "       Dark Screen";
            }

            panelMain.BackColor = ThemeHelper.BackNormalFirst;
            panel1.BackColor = ThemeHelper.BackNormalFirst;

            PictureBoxCloseForm.ApplyTheme();

            ApplyThemeToControls(this);

            ResumeLayout();
        }

        private void ApplyThemeToControls(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                if (control is LabelNormalN labelNormal)
                {
                    labelNormal.ForeColor = ThemeHelper.ForeNormalFirst;
                    labelNormal.DG_SVGImageColor = ThemeHelper.ForceColofulFist;
                }
                else if (control is ToggleWithTextControl toggleWithText)
                {
                    if (ThemeHelper.CurrentMode == ThemeMode.Light)
                    {
                        toggleWithText.DG_KnobColor = ThemeHelper.ForceColofulFist;
                        toggleWithText.ForeColor = ThemeHelper.ForeNormalFirst;
                        toggleWithText.DG_LabelColor = ThemeHelper.ForeNormalFirst;
                        toggleWithText.DG_ToggleColorClick = Color.FromArgb(165, 165, 165);
                        toggleWithText.DG_ToggleColorNormal = Color.FromArgb(200, 200, 200);
                    }
                    else
                    {
                        toggleWithText.DG_KnobColor = Color.White;
                        toggleWithText.ForeColor = ThemeHelper.ForeNormalFirst;
                        toggleWithText.DG_LabelColor = ThemeHelper.ForeNormalFirst;
                        toggleWithText.DG_ToggleColorClick = ThemeHelper.ForceColofulFist;
                        toggleWithText.DG_ToggleColorNormal = Color.FromArgb(120, 120, 120);
                    }
                }

                if (control.HasChildren)
                    ApplyThemeToControls(control);
            }

           
        }
        public void LoadAllTogglesFromSettings()
        {
            _isUpdating = true; // chặn tất cả Toggled
            try
            {
                // 1. gán đúng giá trị từ setting
                toggleDisplayTitle.DG_IsToggled = GetSettings.GetDisplayTitle();
                toggleDisplayIP.DG_IsToggled = GetSettings.GetDisplayIP();
                toggleDisplayModel.DG_IsToggled = GetSettings.GetDisplayModelName();
                toggleSerial.DG_IsToggled = GetSettings.GetDisplaySerial();

                // 2. bật/tắt nhóm con theo Title (giống code trong ToggleDisplayTitle)
                bool titleOn = toggleDisplayTitle.DG_IsToggled;
                toggleDisplayIP.Enabled = titleOn;
                toggleDisplayModel.Enabled = titleOn;
                toggleSerial.Enabled = titleOn;
            }
            finally
            {
                _isUpdating = false;
            }
        }
        private void ToggleDisplayTitle_Toggled(object? sender, EventArgs e)
        {
            if (_isUpdating) return; // <--- thêm dòng này

            var isTitle = toggleDisplayTitle.DG_IsToggled;

            _isUpdating = true;
            try
            {
                toggleDisplayIP.Enabled = isTitle;
                toggleDisplayModel.Enabled = isTitle;
                toggleSerial.Enabled = isTitle;

                if (!isTitle) // tắt Title thì tắt luôn 3 con
                {
                    toggleDisplayIP.DG_IsToggled = false;
                    toggleDisplayModel.DG_IsToggled = false;
                    toggleSerial.DG_IsToggled = false;
                }
            }
            finally { _isUpdating = false; }

            GetSettings.SetDisplayTitle(isTitle);

            var devices = DeviceManager.Instance.LoadAllDevicePanel();
            foreach (var device in devices)
            {
                device._PDcontroller?.ShowModelName();
            }
        }

        private bool _isUpdating = false;

        private void ToggleSerial_Toggled(object? sender, EventArgs e)
        {
            if (_isUpdating) return;
            SetToggles(toggleSerial);
        }

        private void ToggleDisplayModel_Toggled(object? sender, EventArgs e)
        {
            if (_isUpdating) return;
            SetToggles(toggleDisplayModel);
        }

        private void ToggleDisplayIP_Toggled(object? sender, EventArgs e)
        {
            if (_isUpdating) return;
            SetToggles(toggleDisplayIP);
        }

        // hàm trung tâm – hoạt động như radio button
        private void SetToggles(Control? activeToggle)
        {
            _isUpdating = true;
            try
            {
                bool serialOn = activeToggle == toggleSerial && toggleSerial.DG_IsToggled;
                bool modelOn = activeToggle == toggleDisplayModel && toggleDisplayModel.DG_IsToggled;
                bool ipOn = activeToggle == toggleDisplayIP && toggleDisplayIP.DG_IsToggled;

                toggleSerial.DG_IsToggled = serialOn;
                toggleDisplayModel.DG_IsToggled = modelOn;
                toggleDisplayIP.DG_IsToggled = ipOn;

                // lưu luôn
                GetSettings.SetDisplaySerial(serialOn);
                GetSettings.SetDisplayModelName(modelOn);
                GetSettings.SetDisplayIP(ipOn);

                var devices = DeviceManager.Instance.LoadAllDevicePanel();
                foreach (var device in devices)
                {
                    device._PDcontroller?.ShowModelName();
                }
            }
            finally { _isUpdating = false; }
        }

        #region Setting Begin
        private void EnableFormDrag(Control ctrl)
        {
            if (ctrl == null) return;

            ctrl.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    ReleaseCapture();
                    SendMessage(this.Handle, 0xA1, 2, 0); // WM_NCLBUTTONDOWN
                }
            };
        }

        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_BorderColor
        {
            get => _borderColor;
            set
            {
                _borderColor = value;
                this.BackColor = value;
            }
        }

        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public float DG_RoundRadius
        {
            get => _roundRadius;
            set
            {
                _roundRadius = value;
                SetRoundedRegion();
                SetPanelMainRoundedRegion();
            }
        }

        private void SetRoundedRegion()
        {
            using (var path = GetRoundedRectanglePath(ClientRectangle, _roundRadius))
            {
                var region = new Region(path);
                var old = Region;
                Region = region;
                old?.Dispose();
            }
        }

        private void SetPanelMainRoundedRegion()
        {
            using (var path = GetRoundedRectanglePath(panelMain.ClientRectangle, _roundRadius))
            {
                var region = new Region(path);
                var old = panelMain.Region;
                panelMain.Region = region;
                old?.Dispose();
            }
        }

        public static GraphicsPath GetRoundedRectanglePath(RectangleF rect, float radius)
        {
            float d = radius * 2f;
            var path = new GraphicsPath();
            if (radius <= 0) { path.AddRectangle(rect); return path; }

            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddLine(rect.X + radius, rect.Y, rect.Right - radius, rect.Y);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddLine(rect.Right, rect.Y + radius, rect.Right, rect.Bottom - radius);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddLine(rect.Right - radius, rect.Bottom, rect.X + radius, rect.Bottom);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.AddLine(rect.X, rect.Bottom - radius, rect.X, rect.Y + radius);
            path.CloseFigure();
            return path;
        }

        public void UpdateOriginalState()
        {
            originalSize = this.Size;
            originalLocation = this.Location;
        }

        protected override void OnLocationChanged(EventArgs e)
        {
            base.OnLocationChanged(e);
            if (!isMaximized && this.WindowState == FormWindowState.Normal)
            {
                UpdateOriginalState();
            }
        }

        private void PictureBoxCloseForm_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
    }
}
