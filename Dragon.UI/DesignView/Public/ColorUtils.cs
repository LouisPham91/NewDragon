using System;

namespace Dragon.DesignView.Public
{
    public readonly struct RgbColor
    {
        public readonly byte R, G, B;
        public RgbColor(byte r, byte g, byte b) => (R, G, B) = (r, g, b);
        public uint ToArgb() => (uint)(0xFF << 24 | R << 16 | G << 8 | B);
    }

    public static class ColorUtils
    {
        public static RgbColor FromHSL(double h, double s, double l)
        {
            h = ((h % 360) + 360) % 360; // chuẩn hóa 0-360
            s = Math.Clamp(s, 0, 1);
            l = Math.Clamp(l, 0, 1);

            double c = (1 - Math.Abs(2 * l - 1)) * s;
            double hPrime = h / 60.0;
            double x = c * (1 - Math.Abs(hPrime % 2 - 1));
            double m = l - c / 2;

            double r1 = 0, g1 = 0, b1 = 0;
            if (hPrime < 1) { r1 = c; g1 = x; }
            else if (hPrime < 2) { r1 = x; g1 = c; }
            else if (hPrime < 3) { g1 = c; b1 = x; }
            else if (hPrime < 4) { g1 = x; b1 = c; }
            else if (hPrime < 5) { r1 = x; b1 = c; }
            else { r1 = c; b1 = x; }

            byte R = (byte)Math.Clamp(Math.Round((r1 + m) * 255), 0, 255);
            byte G = (byte)Math.Clamp(Math.Round((g1 + m) * 255), 0, 255);
            byte B = (byte)Math.Clamp(Math.Round((b1 + m) * 255), 0, 255);
            return new RgbColor(R, G, B);
        }

        public static double Luminance(RgbColor c)
        {
            double Rs = c.R / 255.0;
            double Gs = c.G / 255.0;
            double Bs = c.B / 255.0;

            double R = Rs <= 0.03928 ? Rs / 12.92 : Math.Pow((Rs + 0.055) / 1.055, 2.4);
            double G = Gs <= 0.03928 ? Gs / 12.92 : Math.Pow((Gs + 0.055) / 1.055, 2.4);
            double B = Bs <= 0.03928 ? Bs / 12.92 : Math.Pow((Bs + 0.055) / 1.055, 2.4);

            return 0.2126 * R + 0.7152 * G + 0.0722 * B;
        }

        public static double ContrastRatio(RgbColor a, RgbColor b)
        {
            double l1 = Luminance(a);
            double l2 = Luminance(b);
            return (Math.Max(l1, l2) + 0.05) / (Math.Min(l1, l2) + 0.05);
        }

        // tìm lightness thỏa contrast, AOT-safe
        public static double FindLightness(double h, double s, RgbColor fg, RgbColor bg,
            double minContrastText = 4.5, double minContrastBg = 3.0)
        {
            double best = 0.5;
            for (int i = 0; i < 21; i++) // 21 bước ~ đủ chính xác
            {
                double mid = i / 20.0;
                var trial = FromHSL(h, s, mid);
                if (ContrastRatio(trial, fg) >= minContrastText &&
                    ContrastRatio(trial, bg) >= minContrastBg)
                {
                    best = mid;
                    break;
                }
            }
            return best;
        }
    }
}