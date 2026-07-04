using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Dragon.ControlHelper.Interrop
{
    public static partial class Win32   // <--- QUAN TRỌNG: phải có partial
    {
        const string USER32 = "user32.dll";

        [LibraryImport(USER32)]
        public static partial int GetSystemMetrics(int nIndex);

        [LibraryImport(USER32)]
        public static partial uint MapVirtualKey(uint uCode, uint uMapType);

        [LibraryImport(USER32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetWindowPos(nint hWnd, nint hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [LibraryImport(USER32, SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetWindowText(nint hWnd, string lpString);

        [LibraryImport(USER32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetForegroundWindow(nint hWnd);

        [LibraryImport(USER32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool ReleaseCapture();

        [LibraryImport(USER32)]
        public static partial nint SendMessage(nint hWnd, uint Msg, nint wParam, nint lParam);

        [LibraryImport(USER32)]
        public static partial void keybd_event(byte bVk, byte bScan, uint dwFlags, nuint dwExtraInfo);

        // --- 4 hàm phức tạp giữ DllImport ---
        [StructLayout(LayoutKind.Sequential)]
        public struct ICONINFO { public int fIcon; public int xHotspot; public int yHotspot; public nint hbmMask; public nint hbmColor; }

        [DllImport(USER32, SetLastError = true)]
        public static extern nint CreateIconIndirect(ref ICONINFO icon);

        [DllImport(USER32, SetLastError = true)]
        public static extern bool GetIconInfo(nint hIcon, out ICONINFO piconinfo);

        [DllImport(USER32, SetLastError = true)]
        public static extern bool DestroyIcon(nint hIcon);

        [DllImport(USER32, CharSet = CharSet.Unicode)]
        public static extern int ToUnicode(uint wVirtKey, uint wScanCode, byte[] lpKeyState, StringBuilder pwszBuff, int cchBuff, uint wFlags);
    }
}
