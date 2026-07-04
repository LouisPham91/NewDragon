
using Dragon.ControlHelper.UIController;
using Dragon.Controller.GlobalControl.Property;
using Dragon.DesignView.Public;
using Dragon.DesignView.Public.ColorMode;
using Dragon.DesignView.Public.NormalMode;
using System.Diagnostics;

namespace Dragon.DesignView.ControlUI.Private
{
    public partial class UserSettings : UserControl
    {
        // 🔥 Theme handler - protected để Designer file truy cập được


        public UserSettings()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.ResizeRedraw, true);

            this.AutoScaleMode = AutoScaleMode.Dpi;
            InitializeComponent();


            ApplyTheme();

            toggleDisplayTitle.Toggled += ToggleDisplayTitle_Toggled;
            toggleDisplayIP.Toggled += ToggleDisplayIP_Toggled;
            toggleDisplayModel.Toggled += ToggleDisplayModel_Toggled;
            toggleSerial.Toggled += ToggleSerial_Toggled;
            LoadAllTogglesFromSettings();
        }

        public void ApplyTheme()
        {
            if (IsDisposed) return;

            BackColor = ThemeHelper.BackNormalFirst;


            //toggleDisplayTitle.DG_BackgroundColor = Color.Transparent;
            //toggleDisplayIP.DG_BackgroundColor = Color.Transparent;
            //toggleDisplayModel.DG_BackgroundColor = Color.Transparent;
            //toggleSerial.DG_BackgroundColor = Color.Transparent;

            //AlabelNormaln16.ForeColor = ThemeHelper.ForceColofulFist;
            //AlabelNormaln2.ForeColor = ThemeHelper.ForceColofulFist;
            //AlabelNormaln3.ForeColor = ThemeHelper.ForceColofulFist;
            //AlabelNormaln19.ForeColor = ThemeHelper.ForceColofulFist;
            ApplyThemeToControls(this);
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
                    toggleWithText.DG_BackgroundColor = ThemeHelper.BackNormalFirst;

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


        // ⚠️ KHÔNG CÓ method Dispose ở đây - để Designer file xử lý
    }
}