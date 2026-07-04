using Dragon.Controller.DeviceControl.ScrcpyNet.AndroidKeyBroad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Dragon.ControlHelper.ScrcpyNet.SettingDevice
{
    public static class KeycodeUhidHelper
    {
        #region HID Keycode Dictionary (KHÔNG DUPLICATE)

        private static Dictionary<Keys, byte> _hidKeycodeDict = new();

        private static Dictionary<Keys, byte> HidKeycodeDict
        {
            get
            {
                if (_hidKeycodeDict == null || _hidKeycodeDict.Count == 0)
                {
                    _hidKeycodeDict = new Dictionary<Keys, byte>();
                    InitializeDictionary();
                }
                return _hidKeycodeDict;
            }
        }

        private static void InitializeDictionary()
        {
            // Add từng cặp một - đảm bảo không duplicate
            AddSafe(Keys.A, 0x04); AddSafe(Keys.B, 0x05); AddSafe(Keys.C, 0x06); AddSafe(Keys.D, 0x07); AddSafe(Keys.E, 0x08);
            AddSafe(Keys.F, 0x09); AddSafe(Keys.G, 0x0A); AddSafe(Keys.H, 0x0B); AddSafe(Keys.I, 0x0C); AddSafe(Keys.J, 0x0D);
            AddSafe(Keys.K, 0x0E); AddSafe(Keys.L, 0x0F); AddSafe(Keys.M, 0x10); AddSafe(Keys.N, 0x11); AddSafe(Keys.O, 0x12);
            AddSafe(Keys.P, 0x13); AddSafe(Keys.Q, 0x14); AddSafe(Keys.R, 0x15); AddSafe(Keys.S, 0x16); AddSafe(Keys.T, 0x17);
            AddSafe(Keys.U, 0x18); AddSafe(Keys.V, 0x19); AddSafe(Keys.W, 0x1A); AddSafe(Keys.X, 0x1B); AddSafe(Keys.Y, 0x1C);
            AddSafe(Keys.Z, 0x1D);

            AddSafe(Keys.D1, 0x1E); AddSafe(Keys.D2, 0x1F); AddSafe(Keys.D3, 0x20); AddSafe(Keys.D4, 0x21); AddSafe(Keys.D5, 0x22);
            AddSafe(Keys.D6, 0x23); AddSafe(Keys.D7, 0x24); AddSafe(Keys.D8, 0x25); AddSafe(Keys.D9, 0x26); AddSafe(Keys.D0, 0x27);

            AddSafe(Keys.Enter, 0x28);
            AddSafe(Keys.Escape, 0x29);
            AddSafe(Keys.Back, 0x2A);
            AddSafe(Keys.Tab, 0x2B);
            AddSafe(Keys.Space, 0x2C);

            AddSafe(Keys.OemMinus, 0x2D);
            AddSafe(Keys.Oemplus, 0x2E);
            AddSafe(Keys.OemOpenBrackets, 0x2F);
            AddSafe(Keys.OemCloseBrackets, 0x30);
            AddSafe(Keys.OemBackslash, 0x31);
            AddSafe(Keys.OemSemicolon, 0x33);
            AddSafe(Keys.OemQuotes, 0x34);
            AddSafe(Keys.Oemtilde, 0x35);
            AddSafe(Keys.Oemcomma, 0x36);
            AddSafe(Keys.OemPeriod, 0x37);
            AddSafe(Keys.OemQuestion, 0x38);

            AddSafe(Keys.CapsLock, 0x39);
            AddSafe(Keys.NumLock, 0x53);
            AddSafe(Keys.Scroll, 0x47);

            AddSafe(Keys.F1, 0x3A); AddSafe(Keys.F2, 0x3B); AddSafe(Keys.F3, 0x3C); AddSafe(Keys.F4, 0x3D);
            AddSafe(Keys.F5, 0x3E); AddSafe(Keys.F6, 0x3F); AddSafe(Keys.F7, 0x40); AddSafe(Keys.F8, 0x41);
            AddSafe(Keys.F9, 0x42); AddSafe(Keys.F10, 0x43); AddSafe(Keys.F11, 0x44); AddSafe(Keys.F12, 0x45);

            AddSafe(Keys.PrintScreen, 0x46);
            AddSafe(Keys.Insert, 0x49);
            AddSafe(Keys.Home, 0x4A);
            AddSafe(Keys.PageUp, 0x4B);
            AddSafe(Keys.Delete, 0x4C);
            AddSafe(Keys.End, 0x4D);
            AddSafe(Keys.PageDown, 0x4E);

            AddSafe(Keys.Right, 0x4F);
            AddSafe(Keys.Left, 0x50);
            AddSafe(Keys.Down, 0x51);
            AddSafe(Keys.Up, 0x52);

            AddSafe(Keys.NumPad0, 0x62); AddSafe(Keys.NumPad1, 0x59); AddSafe(Keys.NumPad2, 0x5A);
            AddSafe(Keys.NumPad3, 0x5B); AddSafe(Keys.NumPad4, 0x5C); AddSafe(Keys.NumPad5, 0x5D);
            AddSafe(Keys.NumPad6, 0x5E); AddSafe(Keys.NumPad7, 0x5F); AddSafe(Keys.NumPad8, 0x60);
            AddSafe(Keys.NumPad9, 0x61);
            AddSafe(Keys.Decimal, 0x63);
            AddSafe(Keys.Divide, 0x54);
            AddSafe(Keys.Multiply, 0x55);
            AddSafe(Keys.Subtract, 0x56);
            AddSafe(Keys.Add, 0x57);

            AddSafe(Keys.LControlKey, 0xE0);
            AddSafe(Keys.LShiftKey, 0xE1);
            AddSafe(Keys.LMenu, 0xE2);
            AddSafe(Keys.LWin, 0xE3);
            AddSafe(Keys.RControlKey, 0xE4);
            AddSafe(Keys.RShiftKey, 0xE5);
            AddSafe(Keys.RMenu, 0xE6);
            AddSafe(Keys.RWin, 0xE7);

            AddSafe(Keys.VolumeMute, 0x7F);
            AddSafe(Keys.VolumeUp, 0x80);
            AddSafe(Keys.VolumeDown, 0x81);
            AddSafe(Keys.MediaNextTrack, 0xB5);
            AddSafe(Keys.MediaPreviousTrack, 0xB6);
            AddSafe(Keys.MediaStop, 0xB7);
            AddSafe(Keys.MediaPlayPause, 0xCD);

            AddSafe(Keys.Apps, 0x65);
        }

        private static void AddSafe(Keys key, byte value)
        {
            if (!_hidKeycodeDict.ContainsKey(key))
            {
                _hidKeycodeDict.Add(key, value);
            }
            // Nếu duplicate, bỏ qua (hoặc log warning)
        }

        public static byte ConvertToHidKeycode(Keys key)
        {
            // A - Z
            if (key >= Keys.A && key <= Keys.Z)
                return (byte)(0x04 + ((int)key - (int)Keys.A));

            // NumPad 0-9 → Map về số trên bàn phím chính
            if (key >= Keys.NumPad0 && key <= Keys.NumPad9)
            {
                if (key == Keys.NumPad0)
                    return 0x27; // Số 0 chính
                else
                    return (byte)(0x1E + ((int)key - (int)Keys.NumPad1)); // Số 1-9 chính
            }

            // NumPad operators → Map về phím chính
            if (key == Keys.Decimal) return 0x37;   // . (OemPeriod)
            if (key == Keys.Add) return 0x2E;       // + (cần Shift với =) → sẽ xử lý trong KeyDown
            if (key == Keys.Subtract) return 0x2D;  // - (OemMinus)
            if (key == Keys.Multiply) return 0x25;  // * (Shift + 8)
            if (key == Keys.Divide) return 0x38;    // / (OemQuestion - cần Shift)

            // Escape
            if (key == Keys.Escape) return 0x29;

            // Dictionary
            if (HidKeycodeDict.TryGetValue(key, out var hidCode))
                return hidCode;

            return 0x00;
        }


        #endregion

        #region ConvertKeyToUnicode

        [DllImport("user32.dll")]
        private static extern int ToUnicode(uint vk, uint sc, byte[] ks, StringBuilder sb, int sz, uint fl);

        [DllImport("user32.dll")]
        private static extern uint MapVirtualKey(uint code, uint type);

        public static string ConvertKeyToUnicode(Keys key, Keys modifiers)
        {
            byte[] kbState = new byte[256];
            if ((modifiers & Keys.Shift) == Keys.Shift) kbState[0x10] = 0x80;
            if ((modifiers & Keys.Control) == Keys.Control) kbState[0x11] = 0x80;
            if ((modifiers & Keys.Alt) == Keys.Alt) kbState[0x12] = 0x80;
            if (Control.IsKeyLocked(Keys.CapsLock)) kbState[0x14] = 0x01;

            var sb = new StringBuilder(64);
            uint vk = (uint)key;
            uint sc = MapVirtualKey(vk, 0);

            int result = ToUnicode(vk, sc, kbState, sb, sb.Capacity, 0);
            return result > 0 ? sb.ToString() : string.Empty;
        }

        #endregion

        #region ConvertToHidModifiers

        public static byte ConvertToHidModifiers(Keys modifiers)
        {
            byte hid = 0;
            if ((modifiers & Keys.Control) == Keys.Control) hid |= 0x01;
            if ((modifiers & Keys.Shift) == Keys.Shift) hid |= 0x02;
            if ((modifiers & Keys.Alt) == Keys.Alt) hid |= 0x04;
            if ((modifiers & Keys.LWin) == Keys.LWin || (modifiers & Keys.RWin) == Keys.RWin) hid |= 0x08;
            return hid;
        }

        #endregion

        #region ConvertHidLedToMods

        public static (bool capsLock, bool numLock) ConvertHidLedToMods(byte led)
        {
            return ((led & 0x02) != 0, (led & 0x01) != 0);
        }

        #endregion

        #region ConvertAndroidKeycodeToHid

        public static byte ConvertAndroidKeycodeToHid(AndroidKeycode code)
        {
            return code switch
            {
                AndroidKeycode.AKEYCODE_A => 0x04,
                AndroidKeycode.AKEYCODE_B => 0x05,
                AndroidKeycode.AKEYCODE_C => 0x06,
                AndroidKeycode.AKEYCODE_D => 0x07,
                AndroidKeycode.AKEYCODE_E => 0x08,
                AndroidKeycode.AKEYCODE_F => 0x09,
                AndroidKeycode.AKEYCODE_G => 0x0A,
                AndroidKeycode.AKEYCODE_H => 0x0B,
                AndroidKeycode.AKEYCODE_I => 0x0C,
                AndroidKeycode.AKEYCODE_J => 0x0D,
                AndroidKeycode.AKEYCODE_K => 0x0E,
                AndroidKeycode.AKEYCODE_L => 0x0F,
                AndroidKeycode.AKEYCODE_M => 0x10,
                AndroidKeycode.AKEYCODE_N => 0x11,
                AndroidKeycode.AKEYCODE_O => 0x12,
                AndroidKeycode.AKEYCODE_P => 0x13,
                AndroidKeycode.AKEYCODE_Q => 0x14,
                AndroidKeycode.AKEYCODE_R => 0x15,
                AndroidKeycode.AKEYCODE_S => 0x16,
                AndroidKeycode.AKEYCODE_T => 0x17,
                AndroidKeycode.AKEYCODE_U => 0x18,
                AndroidKeycode.AKEYCODE_V => 0x19,
                AndroidKeycode.AKEYCODE_W => 0x1A,
                AndroidKeycode.AKEYCODE_X => 0x1B,
                AndroidKeycode.AKEYCODE_Y => 0x1C,
                AndroidKeycode.AKEYCODE_Z => 0x1D,

                AndroidKeycode.AKEYCODE_1 => 0x1E,
                AndroidKeycode.AKEYCODE_2 => 0x1F,
                AndroidKeycode.AKEYCODE_3 => 0x20,
                AndroidKeycode.AKEYCODE_4 => 0x21,
                AndroidKeycode.AKEYCODE_5 => 0x22,
                AndroidKeycode.AKEYCODE_6 => 0x23,
                AndroidKeycode.AKEYCODE_7 => 0x24,
                AndroidKeycode.AKEYCODE_8 => 0x25,
                AndroidKeycode.AKEYCODE_9 => 0x26,
                AndroidKeycode.AKEYCODE_0 => 0x27,

                AndroidKeycode.AKEYCODE_ENTER => 0x28,
                AndroidKeycode.AKEYCODE_ESCAPE => 0x29,
                AndroidKeycode.AKEYCODE_DEL => 0x2A,
                AndroidKeycode.AKEYCODE_TAB => 0x2B,
                AndroidKeycode.AKEYCODE_SPACE => 0x2C,

                AndroidKeycode.AKEYCODE_MINUS => 0x2D,
                AndroidKeycode.AKEYCODE_EQUALS => 0x2E,
                AndroidKeycode.AKEYCODE_LEFT_BRACKET => 0x2F,
                AndroidKeycode.AKEYCODE_RIGHT_BRACKET => 0x30,
                AndroidKeycode.AKEYCODE_BACKSLASH => 0x31,
                AndroidKeycode.AKEYCODE_SEMICOLON => 0x33,
                AndroidKeycode.AKEYCODE_APOSTROPHE => 0x34,
                AndroidKeycode.AKEYCODE_GRAVE => 0x35,
                AndroidKeycode.AKEYCODE_COMMA => 0x36,
                AndroidKeycode.AKEYCODE_PERIOD => 0x37,
                AndroidKeycode.AKEYCODE_SLASH => 0x38,

                AndroidKeycode.AKEYCODE_CAPS_LOCK => 0x39,
                AndroidKeycode.AKEYCODE_NUM_LOCK => 0x53,

                AndroidKeycode.AKEYCODE_DPAD_RIGHT => 0x4F,
                AndroidKeycode.AKEYCODE_DPAD_LEFT => 0x50,
                AndroidKeycode.AKEYCODE_DPAD_DOWN => 0x51,
                AndroidKeycode.AKEYCODE_DPAD_UP => 0x52,

                AndroidKeycode.AKEYCODE_F1 => 0x3A,
                AndroidKeycode.AKEYCODE_F2 => 0x3B,
                AndroidKeycode.AKEYCODE_F3 => 0x3C,
                AndroidKeycode.AKEYCODE_F4 => 0x3D,
                AndroidKeycode.AKEYCODE_F5 => 0x3E,
                AndroidKeycode.AKEYCODE_F6 => 0x3F,
                AndroidKeycode.AKEYCODE_F7 => 0x40,
                AndroidKeycode.AKEYCODE_F8 => 0x41,
                AndroidKeycode.AKEYCODE_F9 => 0x42,
                AndroidKeycode.AKEYCODE_F10 => 0x43,
                AndroidKeycode.AKEYCODE_F11 => 0x44,
                AndroidKeycode.AKEYCODE_F12 => 0x45,

                _ => 0x00
            };
        }

        #endregion
    }
}
