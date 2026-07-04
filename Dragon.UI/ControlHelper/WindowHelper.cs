using System.Runtime.InteropServices;

namespace Dragon.ControlHelper
{

    public static partial class WindowHelper
    {
        private const int SW_RESTORE = 9;

        [LibraryImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetForegroundWindow(IntPtr hWnd);

        [LibraryImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public static void BringToFront(Form form)
        {
            if (form.WindowState == FormWindowState.Minimized)
            {
                ShowWindow(form.Handle, SW_RESTORE);
            }
            SetForegroundWindow(form.Handle);
        }
    }

}
