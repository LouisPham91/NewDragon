
using Dragon.Controller.GlobalControl.Property;
using Dragon.Controller.GlobalControl.Setting;

namespace Dragon.DesignView.Public
{
    public enum UseMode { ThemeMode, ThemeStyle }
    public enum ThemeMode { Light, Dark }
    public enum ThemeStyle { ThemeRed, ThemeGreen, ThemeGold }
    public interface IThemeable
    {
        void ApplyTheme();
    }
    public static class DarkMode
    {
        public static readonly Color BackNormalFirst = Color.FromArgb(40, 40, 40);
        public static readonly Color BackNormal2nd = Color.FromArgb(31, 31, 31);
        public static readonly Color BackNormal3rd = Color.FromArgb(225, 225, 225);

        public static readonly Color BorderNormalFist = Color.FromArgb(61, 61, 61);
        public static readonly Color BorderNormal2nd = Color.FromArgb(61, 61, 61);
        public static readonly Color BorderNormal3rd = Color.FromArgb(165, 165, 165);
        public static readonly Color BorderNormal4th = Color.FromArgb(108, 108, 108);

        public static readonly Color ForeNormalFirst = Color.White;
        public static readonly Color ForeNormal2nd = Color.FromArgb(240, 240, 240);
        public static readonly Color ForeNormal3rd = Color.FromArgb(200, 200, 200);
    }

    public static class LightMode
    {
        public static readonly Color BackNormalFirst = Color.FromArgb(245, 245, 245);
        public static readonly Color BackNormal2nd = Color.FromArgb(205, 205, 205);
        public static readonly Color BackNormal3rd = Color.FromArgb(225, 225, 225);

        public static readonly Color BorderNormalFist = Color.FromArgb(168, 168, 168);
        public static readonly Color BorderNormal2nd = Color.FromArgb(61, 61, 61);
        public static readonly Color BorderNormal3rd = Color.FromArgb(165, 165, 165);
        public static readonly Color BorderNormal4th = Color.FromArgb(108, 108, 108);

        public static readonly Color ForeNormalFirst = Color.Black;
        public static readonly Color ForeNormal2nd = Color.FromArgb(31, 31, 31);
        public static readonly Color ForeNormal3rd = Color.FromArgb(50, 50, 50);
    }

    public static class ThemeRed
    {
        public static readonly Color BackColofulFirst = Color.FromArgb(97, 53, 49);
        public static readonly Color BackColoful2nd = Color.FromArgb(127, 53, 49);

        public static readonly Color BorderColofulFist = Color.FromArgb(207, 66, 51);
        public static readonly Color BorderColoful2nd = Color.FromArgb(83, 60, 61);

        public static readonly Color ForceColofulFist = Color.FromArgb(207, 66, 51);
        public static readonly Color ForceColoful2nd = Color.FromArgb(227, 66, 51);
        public static readonly Color ForceColoful3rd = Color.FromArgb(255, 66, 51);
        public static readonly Color ForceColoful4th = Color.FromArgb(255, 70, 70);

        public static readonly Color GradientStart = Color.FromArgb(0, 173, 232);
        public static readonly Color Gradient2ND = Color.FromArgb(21, 115, 154);
        public static readonly Color Gradient3RD = Color.FromArgb(200, 120, 130);
        public static readonly Color GradientEnd = Color.FromArgb(255, 71, 51);

        public static readonly Color GradientStart_Light = Color.FromArgb(0, 173, 232);
        public static readonly Color Gradient2ND_Light = Color.FromArgb(21, 115, 154);
        public static readonly Color Gradient3RD_Light = Color.FromArgb(180, 120, 130);
        public static readonly Color GradientEnd_Light = Color.FromArgb(225, 71, 51);
    }

    public static class ThemeGreen
    {
        public static readonly Color BackColofulFirst = Color.FromArgb(60, 100, 60);
        public static readonly Color BackColoful2nd = Color.FromArgb(60, 150, 60);

        public static readonly Color BorderColofulFist = Color.FromArgb(35, 160, 35);
        public static readonly Color BorderColoful2nd = Color.FromArgb(35, 250, 35);

        public static readonly Color ForceColofulFist = Color.FromArgb(35, 130, 35);
        public static readonly Color ForceColoful2nd = Color.FromArgb(35, 200, 35);
        public static readonly Color ForceColoful3rd = Color.FromArgb(35, 190, 35);
        public static readonly Color ForceColoful4th = Color.FromArgb(35, 160, 35);

        public static readonly Color GradientStart = Color.FromArgb(141, 251, 141);
        public static readonly Color Gradient2ND = Color.FromArgb(121, 230, 130);
        public static readonly Color Gradient3RD = Color.FromArgb(119, 208, 108);
        public static readonly Color GradientEnd = Color.FromArgb(53, 130, 53);

        public static readonly Color GradientStart_Light = Color.FromArgb(119, 208, 108);
        public static readonly Color Gradient2ND_Light = Color.FromArgb(100, 180, 108);
        public static readonly Color Gradient3RD_Light = Color.FromArgb(100, 158, 108);
        public static readonly Color GradientEnd_Light = Color.FromArgb(53, 130, 53);
    }

    public static class ThemeGold
    {
        public static readonly Color BackColofulFirst = Color.FromArgb(154, 104, 11);
        public static readonly Color BackColoful2nd = Color.FromArgb(97, 53, 49);

        public static readonly Color BorderColofulFist = Color.FromArgb(184, 134, 11);
        public static readonly Color BorderColoful2nd = Color.FromArgb(228, 165, 32);

        public static readonly Color ForceColofulFist = Color.FromArgb(184, 134, 11);
        public static readonly Color ForceColoful2nd = Color.FromArgb(214, 164, 11);
        public static readonly Color ForceColoful3rd = Color.FromArgb(248, 205, 32);
        public static readonly Color ForceColoful4th = Color.FromArgb(255, 226, 32);

        public static readonly Color GradientStart = Color.FromArgb(255, 225, 11);
        public static readonly Color Gradient2ND = Color.FromArgb(234, 195, 11);
        public static readonly Color Gradient3RD = Color.FromArgb(214, 164, 11);
        public static readonly Color GradientEnd = Color.FromArgb(184, 134, 11);

        public static readonly Color GradientStart_Light = Color.FromArgb(214, 164, 11);
        public static readonly Color Gradient2ND_Light = Color.FromArgb(184, 134, 11);
        public static readonly Color Gradient3RD_Light = Color.FromArgb(164, 114, 11);
        public static readonly Color GradientEnd_Light = Color.FromArgb(144, 94, 11);
    }

    // 🔥 ThemeHelper được giữ nguyên, chỉ thêm comment hướng dẫn
    public static class ThemeHelper
    {
        public static event EventHandler? ThemeChanged;

        public static readonly Font SharedValueFont = new Font("Segoe UI", 9, FontStyle.Bold);



        public static ThemeMode CurrentMode { get; private set; }
        public static ThemeStyle CurrentStyle { get; private set; }

        // Normal Colors
        public static Color BackNormalFirst { get; private set; }
        public static Color BackNormal2nd { get; private set; }
        public static Color BackNormal3rd { get; private set; }

        public static Color BorderNormalFist { get; private set; }
        public static Color BorderNormal2nd { get; private set; }
        public static Color BorderNormal3rd { get; private set; }
        public static Color BorderNormal4th { get; private set; }

        public static Color ForeNormalFirst { get; private set; }
        public static Color ForeNormal2nd { get; private set; }
        public static Color ForeNormal3rd { get; private set; }

        // Colorful colors
        public static Color BackColofulFirst { get; private set; }
        public static Color BackColoful2nd { get; private set; }

        public static Color BorderColofulFist { get; private set; }
        public static Color BorderColoful2nd { get; private set; }

        public static Color ForceColofulFist { get; private set; }
        public static Color ForceColoful2nd { get; private set; }
        public static Color ForceColoful3rd { get; private set; }
        public static Color ForceColoful4th { get; private set; }

        public static Color GradientStart { get; private set; }
        public static Color Gradient2ND { get; private set; }
        public static Color Gradient3RD { get; private set; }
        public static Color GradientEnd { get; private set; }

        public static Color GradientStart_Light { get; private set; }
        public static Color Gradient2ND_Light { get; private set; }
        public static Color Gradient3RD_Light { get; private set; }
        public static Color GradientEnd_Light { get; private set; }

        static ThemeHelper()
        {
            var mode = (ThemeMode)GetSettings.GetThemeMode(); // 0=Light, 1=Dark
            var style = (ThemeStyle)GetSettings.GetThemeStyle(); // 0=Red...
            SetTheme(mode, style);
        }

        public static void SetTheme(ThemeMode mode, ThemeStyle style)
        {
            CurrentMode = mode;
            CurrentStyle = style;

            // Light / Dark
            if (mode == ThemeMode.Light)
            {
                BackNormalFirst = LightMode.BackNormalFirst;
                BackNormal2nd = LightMode.BackNormal2nd;
                BackNormal3rd = LightMode.BackNormal3rd;

                BorderNormalFist = LightMode.BorderNormalFist;
                BorderNormal2nd = LightMode.BorderNormal2nd;
                BorderNormal3rd = LightMode.BorderNormal3rd;
                BorderNormal4th = LightMode.BorderNormal4th;

                ForeNormalFirst = LightMode.ForeNormalFirst;
                ForeNormal2nd = LightMode.ForeNormal2nd;
                ForeNormal3rd = LightMode.ForeNormal3rd;
            }
            else
            {
                BackNormalFirst = DarkMode.BackNormalFirst;
                BackNormal2nd = DarkMode.BackNormal2nd;
                BackNormal3rd = DarkMode.BackNormal3rd;

                BorderNormalFist = DarkMode.BorderNormalFist;
                BorderNormal2nd = DarkMode.BorderNormal2nd;
                BorderNormal3rd = DarkMode.BorderNormal3rd;
                BorderNormal4th = DarkMode.BorderNormal4th;

                ForeNormalFirst = DarkMode.ForeNormalFirst;
                ForeNormal2nd = DarkMode.ForeNormal2nd;
                ForeNormal3rd = DarkMode.ForeNormal3rd;
            }

            // Style accent
            BackColofulFirst = style switch
            {
                ThemeStyle.ThemeRed => ThemeRed.BackColofulFirst,
                ThemeStyle.ThemeGreen => ThemeGreen.BackColofulFirst,
                _ => ThemeGold.BackColofulFirst
            };
            BackColoful2nd = style switch
            {
                ThemeStyle.ThemeRed => ThemeRed.BackColoful2nd,
                ThemeStyle.ThemeGreen => ThemeGreen.BackColoful2nd,
                _ => ThemeGold.BackColoful2nd
            };
            BorderColofulFist = style switch
            {
                ThemeStyle.ThemeRed => ThemeRed.BorderColofulFist,
                ThemeStyle.ThemeGreen => ThemeGreen.BorderColofulFist,
                _ => ThemeGold.BorderColofulFist
            };
            BorderColoful2nd = style switch
            {
                ThemeStyle.ThemeRed => ThemeRed.BorderColoful2nd,
                ThemeStyle.ThemeGreen => ThemeGreen.BorderColoful2nd,
                _ => ThemeGold.BorderColoful2nd
            };
            ForceColofulFist = style switch
            {
                ThemeStyle.ThemeRed => ThemeRed.ForceColofulFist,
                ThemeStyle.ThemeGreen => ThemeGreen.ForceColofulFist,
                _ => ThemeGold.ForceColofulFist
            };
            ForceColoful2nd = style switch
            {
                ThemeStyle.ThemeRed => ThemeRed.ForceColoful2nd,
                ThemeStyle.ThemeGreen => ThemeGreen.ForceColoful2nd,
                _ => ThemeGold.ForceColoful2nd
            };
            ForceColoful3rd = style switch
            {
                ThemeStyle.ThemeRed => ThemeRed.ForceColoful3rd,
                ThemeStyle.ThemeGreen => ThemeGreen.ForceColoful3rd,
                _ => ThemeGold.ForceColoful3rd
            };
            ForceColoful4th = style switch
            {
                ThemeStyle.ThemeRed => ThemeRed.ForceColoful4th,
                ThemeStyle.ThemeGreen => ThemeGreen.ForceColoful4th,
                _ => ThemeGold.ForceColoful4th
            };
            GradientStart = style switch
            {
                ThemeStyle.ThemeRed => ThemeRed.GradientStart,
                ThemeStyle.ThemeGreen => ThemeGreen.GradientStart,
                _ => ThemeGold.GradientStart
            };
            Gradient2ND = style switch
            {
                ThemeStyle.ThemeRed => ThemeRed.Gradient2ND,
                ThemeStyle.ThemeGreen => ThemeGreen.Gradient2ND,
                _ => ThemeGold.Gradient2ND
            };
            Gradient3RD = style switch
            {
                ThemeStyle.ThemeRed => ThemeRed.Gradient3RD,
                ThemeStyle.ThemeGreen => ThemeGreen.Gradient3RD,
                _ => ThemeGold.Gradient3RD
            };
            GradientEnd = style switch
            {
                ThemeStyle.ThemeRed => ThemeRed.GradientEnd,
                ThemeStyle.ThemeGreen => ThemeGreen.GradientEnd,
                _ => ThemeGold.GradientEnd
            };
            GradientStart_Light = style switch
            {
                ThemeStyle.ThemeRed => ThemeRed.GradientStart_Light,
                ThemeStyle.ThemeGreen => ThemeGreen.GradientStart_Light,
                _ => ThemeGold.GradientStart_Light
            };
            Gradient2ND_Light = style switch
            {
                ThemeStyle.ThemeRed => ThemeRed.Gradient2ND_Light,
                ThemeStyle.ThemeGreen => ThemeGreen.Gradient2ND_Light,
                _ => ThemeGold.Gradient2ND_Light
            };
            Gradient3RD_Light = style switch
            {
                ThemeStyle.ThemeRed => ThemeRed.Gradient3RD_Light,
                ThemeStyle.ThemeGreen => ThemeGreen.Gradient3RD_Light,
                _ => ThemeGold.Gradient3RD_Light
            };
            GradientEnd_Light = style switch
            {
                ThemeStyle.ThemeRed => ThemeRed.GradientEnd_Light,
                ThemeStyle.ThemeGreen => ThemeGreen.GradientEnd_Light,
                _ => ThemeGold.GradientEnd_Light
            };

            ThemeChanged?.Invoke(null, EventArgs.Empty); 
        }

    }
}