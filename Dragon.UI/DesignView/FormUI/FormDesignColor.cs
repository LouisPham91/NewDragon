
using Dragon.DesignView.Public;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dragon.DesignView.FormUI
{
    public partial class FormDesignColor : Form
    {
        public FormDesignColor()
        {
            InitializeComponent();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panelColorc1_BackColorChanged(object sender, EventArgs e)
        {
            this.panel1.BackColor = this.panelColorc1.BackColor;
            TinhToanMau(this.panelColorc1.BackColor, this.panelColorc2.BackColor);
        }

        private void panelColorc2_BackColorChanged(object sender, EventArgs e)
        {
            this.panel2.BackColor = this.panelColorc2.BackColor;
            TinhToanMau(this.panelColorc1.BackColor, this.panelColorc2.BackColor);
        }

        public void TinhToanMau(Color light, Color Dark)
        {
            Color lightBg = Color.White;
            Color darkBg = Color.FromArgb(40, 40, 40);
            lightBg = light;
            darkBg = Dark;

            double hue = 0;    // thuần đỏ
            double sat = 0.65;   // độ bão hòa

            // 1. Tạo màu nền từ ThemeHelper (nếu bạn vẫn dùng System.Drawing.Color)
            var lightRGB = new RgbColor(ThemeHelper.BackNormalFirst.R,
                                       ThemeHelper.BackNormalFirst.G,
                                       ThemeHelper.BackNormalFirst.B);

            var darkRGB = new RgbColor(ThemeHelper.BackNormalFirst.R,
                                      ThemeHelper.BackNormalFirst.G,
                                      ThemeHelper.BackNormalFirst.B);

            var white = new RgbColor(255, 255, 255); // thay cho Color.White

            // 2. Tìm lightness
            double L_light = ColorUtils.FindLightness(hue, sat, white, lightRGB);
            double L_dark = ColorUtils.FindLightness(hue, sat, white, darkRGB);

            // 3. Lấy màu HSL
            RgbColor btnLightRgb = ColorUtils.FromHSL(hue, sat, L_light);
            RgbColor btnDarkRgb = ColorUtils.FromHSL(hue, sat, L_dark);

            // 4. Nếu control WinForms vẫn cần System.Drawing.Color thì convert lại:
            Color btnColorLight = Color.FromArgb(btnLightRgb.R, btnLightRgb.G, btnLightRgb.B);
            Color btnColorDark = Color.FromArgb(btnDarkRgb.R, btnDarkRgb.G, btnDarkRgb.B);

            ConfigureButton(button1, btnColorLight, $"L={Math.Round(L_light, 2)}");
            ConfigureButton(button2, btnColorDark, $"L={Math.Round(L_dark, 2)}");
        }

        private void ConfigureButton(Button btn, Color bg, string text)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = bg;
            btn.ForeColor = Color.White;
            btn.Text = text;
            btn.Size = new Size(120, 40);
            btn.Click += (s, e) =>
                MessageBox.Show($"Button color: {bg}");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void FormDesignColor_Load(object sender, EventArgs e)
        {

        }
    }
}
