using System.Reflection;
using System.Runtime.InteropServices;

namespace Dragon.ControlHelper
{  
    public static partial class NativeWindowHelper
    {
        private const int WM_SETICON = 0x0080;
        private const int ICON_SMALL = 0;
        private const int ICON_BIG = 1;
        private const uint SWP_SHOWWINDOW = 0x0040;
        private const int SM_CXSCREEN = 0;
        private const int SM_CYSCREEN = 1;

        // --- AOT-friendly P/Invoke ---
        [LibraryImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [LibraryImport("user32.dll")]
        public static partial int GetSystemMetrics(int nIndex);

        [LibraryImport("user32.dll")]
        public static partial IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);


        [LibraryImport("user32.dll", EntryPoint = "SendMessageW")]
        public static partial IntPtr SendMessageW(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        // --- public API ---

        /// <summary>Resize và center window</summary>
        public static void ResizeAndCenter(IntPtr hWnd, int width, int height)
        {
            int screenW = GetSystemMetrics(SM_CXSCREEN);
            int screenH = GetSystemMetrics(SM_CYSCREEN);
            int x = (screenW - width) / 2;
            int y = (screenH - height) / 2;
            SetWindowPos(hWnd, IntPtr.Zero, x, y, width, height, SWP_SHOWWINDOW);
        }

    }
}
