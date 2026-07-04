
using Dragon.DesignView.ControlUI.Private;
using Dragon.DesignView.Public;



namespace Dragon.DesignView.FormUI
{
    public partial class FormSettings : FormOriginal
    {
        private UserSettingColor userSettingColor = new();
        private UserLanguage userLanguage = new();
        private UserSettings userSettings = new UserSettings();

        public FormSettings()
        {
            InitializeComponent();
            userSettingColor.CreateControl();
            userLanguage.CreateControl();
            userSettings.CreateControl();
            ThemeHelper.ThemeChanged += OnGlobalThemeChanged;
            ApplyTheme();
        }
        private void OnGlobalThemeChanged(object? s, EventArgs e)
        {
            if (IsDisposed || !IsHandleCreated) return;
            BeginInvoke(new Action(ApplyTheme)); // tránh cross-thread
        }
        public void ApplyTheme()
        {
            SuspendLayout();

            foreach (var ctrl in DesignHelper.GetAllControls(this))
            {
                if (ctrl is IThemeable themeable)
                    themeable.ApplyTheme();
            }

            userSettingColor.ApplyTheme();
            userLanguage.ApplyTheme();
            userSettings.ApplyTheme();
            panelRightMain.BackColor = ThemeHelper.BackNormalFirst;
            DG_BackColor = ThemeHelper.BackNormalFirst;
            DG_BorderColor = ThemeHelper.BorderNormalFist;
            ForeColor = ThemeHelper.ForeNormalFirst;
            panelLineBordernc1.ApplyTheme();
            panelRightMain.ApplyTheme();
            
            ResumeLayout();
        }
        private void FormSettings_Load(object sender, EventArgs e)
        {

            panelRightMain.Controls.Add(userLanguage);
            userLanguage.Dock = DockStyle.Fill;
            userLanguage.Visible = false;

            panelRightMain.Controls.Add(userSettingColor);
            userSettingColor.Dock = DockStyle.Fill;

            panelRightMain.Controls.Add(userSettings);
            userSettings.Dock = DockStyle.Fill;
            userSettings.Visible = false;

            buttomLanguage.ApplyTheme();
            buttomThemeColor.ApplyTheme();
            buttomThemeColor.DrawLineBaseOnClick();
        }
        public void SetAllControlVisibleFalse()
        {
            userLanguage.Visible = false;
            userSettingColor.Visible = false;
            userSettings.Visible = false;
        }

        private void buttomThemeColor_Click(object sender, EventArgs e)
        {
            SetAllControlVisibleFalse();
            userSettingColor.Visible = true;
        }

        private void buttomLanguage_Click(object sender, EventArgs e)
        {
            SetAllControlVisibleFalse();
            userLanguage.Visible = true;
        }

        private void buttomSetting_Click(object sender, EventArgs e)
        {
            SetAllControlVisibleFalse();
            userSettings.Visible = true;

        }
    }
}
