using Svg;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Text.RegularExpressions;

namespace Dragon.ControlHelper
{
    public static partial class SvgRenderer
    {
        // --- Regex biên dịch sẵn cho AOT ---
        [GeneratedRegex("""(fill|stroke)\s*=\s*(['"])(?!none|transparent)[^'"]*?\2""", RegexOptions.IgnoreCase)]
        private static partial Regex RxAttr();

        [GeneratedRegex("""(fill|stroke)\s*:\s*(?!none|transparent|currentColor)[^;'""]+""", RegexOptions.IgnoreCase)]
        private static partial Regex RxStyle();

        [GeneratedRegex("""<svg[^>]*\s(fill|stroke)\s*=""", RegexOptions.IgnoreCase)]
        private static partial Regex RxHasColor();

        private static string ColorizeSvg(string svg, string hex)
        {
            // 1) đổi attribute fill="..." / stroke='...'
            svg = RxAttr().Replace(svg, m => $"{m.Groups[1].Value}=\"{hex}\"");
            // 2) đổi trong style="fill:...;stroke:..."
            svg = RxStyle().Replace(svg, m => $"{m.Groups[1].Value}:{hex}");
            // 3) đổi currentColor
            svg = svg.Replace("currentColor", hex, StringComparison.OrdinalIgnoreCase);
            // 4) nếu chưa có màu nào -> inject vào <svg>
            if (!RxHasColor().IsMatch(svg))
                svg = Regex.Replace(svg, "<svg([^>]*?)>", $"<svg$1 fill=\"{hex}\">", RegexOptions.IgnoreCase);
            //svg = Regex.Replace(svg, "<svg([^>]*?)>", $"<svg$1 fill=\"{hex}\" stroke=\"{hex}\">", RegexOptions.IgnoreCase);
            return svg;
        }

        // --- HÀM DUY NHẤT BẠN DÙNG ---
        public static Bitmap RenderSvgFromString(string svgContent, int width, int height, Color forceColor)
        {
            if (string.IsNullOrWhiteSpace(svgContent))
                throw new ArgumentException("SVG content empty");

            string hex = $"#{forceColor.R:X2}{forceColor.G:X2}{forceColor.B:X2}";
            string colored = ColorizeSvg(svgContent, hex);

            // Svg.NET 3.4+ đã đánh dấu trim-compatible, dùng Open(stream) là AOT-safe
            using var ms = new MemoryStream(Encoding.UTF8.GetBytes(colored));
            var doc = SvgDocument.Open<SvgDocument>(ms);

            doc.Width = width;
            doc.Height = height;

            var bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            using var g = Graphics.FromImage(bmp);
            g.Clear(Color.Transparent);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            doc.Draw(g);

            return bmp;
        }
    }
}