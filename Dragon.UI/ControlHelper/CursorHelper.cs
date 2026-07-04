using System.Drawing;
using System.Runtime.InteropServices;

namespace Dragon.ControlHelper
{
    public static class CursorHelper
    {
        // SVG gốc - giữ nguyên
        private const string HandCloseSvg = @"<?xml version=""1.0"" encoding=""utf-8""?><svg fill=""#000000"" width=""800px"" height=""800px"" viewBox=""0 0 512 512"" xmlns=""http://www.w3.org/2000/svg""><path d=""M464.8 80c-26.9-.4-48.8 21.2-48.8 48h-8V96.8c0-26.3-20.9-48.3-47.2-48.8-26.9-.4-48.8 21.2-48.8 48v32h-8V80.8c0-26.3-20.9-48.3-47.2-48.8-26.9-.4-48.8 21.2-48.8 48v48h-8V96.8c0-26.3-20.9-48.3-47.2-48.8-26.9-.4-48.8 21.2-48.8 48v136l-8-7.1v-48.1c0-26.3-20.9-48.3-47.2-48.8C21.9 127.6 0 149.2 0 176v66.4c0 27.4 11.7 53.5 32.2 71.8l111.7 99.3c10.2 9.1 16.1 22.2 16.1 35.9v6.7c0 13.3 10.7 24 24h240c13.3 0 24-10.7 24-24v-2.9c0-12.8 2.6-25.5 7.5-37.3l49-116.3c5-11.8 7.5-24.5 7.5-37.3V128.8c0-26.3-20.9-48.4-47.2-48.8z""/></svg>";

        private static Cursor? _handClose;
        public static Cursor HandClose => _handClose ??= CreateCursorFromSvg(HandCloseSvg, 22, 22, Color.White, 11, 11);

        // --- Tạo cursor từ SVG string (dùng SvgRenderer đã AOT-safe) ---
        private static Cursor CreateCursorFromSvg(string svg, int w, int h, Color color, int hotX, int hotY)
        {
            // 1. Render bằng SvgRenderer (đã replace màu bằng regex, không reflection)
            using var bmp = SvgRenderer.RenderSvgFromString(svg, w, h, color);

            // 2. Tạo cursor từ bitmap
            return CreateCursorFromBitmap(bmp, hotX, hotY);
        }

        private static Cursor CreateCursorFromBitmap(Bitmap bmp, int hotX, int hotY)
        {
            // GetHicon tạo HICON, phải destroy sau khi dùng
            IntPtr hIcon = bmp.GetHicon();
            try
            {
                if (!GetIconInfo(hIcon, out ICONINFO info))
                    return Cursors.Default;

                info.fIcon = false; // cursor, không phải icon
                info.xHotspot = hotX;
                info.yHotspot = hotY;

                IntPtr hCursor = CreateIconIndirect(ref info);

                // dọn bitmap mask/color của GetIconInfo
                if (info.hbmMask != IntPtr.Zero) DeleteObject(info.hbmMask);
                if (info.hbmColor != IntPtr.Zero) DeleteObject(info.hbmColor);

                return hCursor != IntPtr.Zero ? new Cursor(hCursor) : Cursors.Default;
            }
            finally
            {
                DestroyIcon(hIcon); // <-- quan trọng cho AOT, tránh leak handle
            }
        }

        // --- P/Invoke ---
        [StructLayout(LayoutKind.Sequential)]
        private struct ICONINFO
        {
            public bool fIcon;
            public int xHotspot;
            public int yHotspot;
            public IntPtr hbmMask;
            public IntPtr hbmColor;
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr CreateIconIndirect(ref ICONINFO icon);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool GetIconInfo(IntPtr hIcon, out ICONINFO piconinfo);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool DestroyIcon(IntPtr hIcon);

        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern bool DeleteObject(IntPtr hObject);
    }
}