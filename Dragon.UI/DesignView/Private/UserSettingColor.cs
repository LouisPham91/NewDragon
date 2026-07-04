
using Dragon.Controller.GlobalControl;
using Dragon.Controller.GlobalControl.Property;
using Dragon.DesignView.Public;
using System;
using System.Windows.Forms;

namespace Dragon.DesignView.ControlUI.Private
{
    public partial class UserSettingColor : UserControl
    {
        // 🔥 Theme handler - protected để Designer file truy cập được
        

        public UserSettingColor()
        {
            this.AutoScaleMode = AutoScaleMode.Dpi;
            InitializeComponent();

            // 🔥 Khởi tạo theme handler NGAY ĐẦU
            
            
            ApplyTheme();
        }

        public void ApplyTheme()
        {
            if (IsDisposed) return;

            BackColor = ThemeHelper.BackNormalFirst;
            // Cập nhật màu đặc biệt cho label
            label1.ForeColor = ThemeHelper.ForeNormalFirst;

            // Cập nhật màu cho các panel màu (giữ màu cố định)
            panelRed.BackColor = Color.FromArgb(255, 71, 51);
            panelGreen.BackColor = Color.FromArgb(90, 151, 90);
            panelGold.BackColor = Color.DarkGoldenrod;
        }

        // ⚠️ KHÔNG CÓ method Dispose ở đây - để Designer file xử lý

        private void UserSettingColor_Load(object sender, EventArgs e)
        {
            if (IsDisposed) return;

            panelRed.MouseClick += PanelRed_MouseClick;
            panelGreen.MouseClick += PanelGreen_MouseClick;
            panelGold.MouseClick += PanelGold_MouseClick;

            panelRed.DrawLineBaseOnClick();
        }

        private void PanelRed_MouseClick(object? sender, MouseEventArgs e)
        {
            if (IsDisposed) return;

            var thememode = (ThemeMode)GetSettings.GetThemeMode();
            var style = ThemeStyle.ThemeRed;
            ThemeHelper.SetTheme(thememode, style);
            GetSettings.SetThemeMode((int)thememode);
            GetSettings.SetThemeStyle((int)style);
            panelRed.DrawLineBaseOnClick();
        }

        private void PanelGreen_MouseClick(object? sender, MouseEventArgs e)
        {
            if (IsDisposed) return;

            var thememode = (ThemeMode)GetSettings.GetThemeMode();
            var style = ThemeStyle.ThemeGreen;
            ThemeHelper.SetTheme(thememode, style);
            GetSettings.SetThemeMode((int)thememode);
            GetSettings.SetThemeStyle((int)style);
            panelGreen.DrawLineBaseOnClick();
        }

        private void PanelGold_MouseClick(object? sender, MouseEventArgs e)
        {
            if (IsDisposed) return;

            var thememode = (ThemeMode)GetSettings.GetThemeMode();
            var style = ThemeStyle.ThemeGold;
            ThemeHelper.SetTheme(thememode, style);
            GetSettings.SetThemeMode((int)thememode);
            GetSettings.SetThemeStyle((int)style);
            panelGold.DrawLineBaseOnClick();
        }
    }
}