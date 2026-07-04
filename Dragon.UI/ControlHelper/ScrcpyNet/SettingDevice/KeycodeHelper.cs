using Dragon.Controller.DeviceControl.ScrcpyNet.AndroidKeyBroad;
using System.Runtime.InteropServices;
using System.Text;

namespace Dragon.ControlHelper.ScrcpyNet.SettingDevice
{
    public static class KeycodeHelper
    {
        public static AndroidKeycode MapStringToAndroidKeycode(string key)
        {
            switch (key.ToLower())
            {
                case "home": return AndroidKeycode.AKEYCODE_HOME;
                case "back": return AndroidKeycode.AKEYCODE_BACK;
                case "left": return AndroidKeycode.AKEYCODE_DPAD_LEFT;
                case "right": return AndroidKeycode.AKEYCODE_DPAD_RIGHT;
                case "up": return AndroidKeycode.AKEYCODE_DPAD_UP;
                case "down": return AndroidKeycode.AKEYCODE_DPAD_DOWN;
                case "center": return AndroidKeycode.AKEYCODE_DPAD_CENTER;
                case "menu": return AndroidKeycode.AKEYCODE_MENU;
                case "search": return AndroidKeycode.AKEYCODE_SEARCH;
                case "enter": return AndroidKeycode.AKEYCODE_ENTER;
                case "delete": return AndroidKeycode.AKEYCODE_DEL;
                case "recent": return AndroidKeycode.AKEYCODE_APP_SWITCH;
                case "volumeup": return AndroidKeycode.AKEYCODE_VOLUME_UP;
                case "volumedown": return AndroidKeycode.AKEYCODE_VOLUME_DOWN;
                case "volumemute": return AndroidKeycode.AKEYCODE_VOLUME_MUTE;
                case "camera": return AndroidKeycode.AKEYCODE_CAMERA;
                case "power": return AndroidKeycode.AKEYCODE_POWER;
                default: return AndroidKeycode.AKEYCODE_UNKNOWN;
            }
        }

        public static Keys CharToKeys(char c)
        {
            if (char.IsLetter(c))
            {
                return (Keys)((int)Keys.A + (char.ToUpper(c) - 'A'));
            }
            if (char.IsDigit(c))
            {
                return (Keys)((int)Keys.D0 + (c - '0'));
            }

            switch (c)
            {
                case ' ': return Keys.Space;
                case '\n': return Keys.Enter;
                case '\t': return Keys.Tab;
                case ',': return Keys.Oemcomma;
                case '.': return Keys.OemPeriod;
                case ';': return Keys.OemSemicolon;
                case '\'': return Keys.OemQuotes;
                case '\\': return Keys.OemBackslash;
                case '|': return Keys.OemPipe;
                case '=': return Keys.Oemplus;
                case '+': return Keys.Add;
                case '-': return Keys.OemMinus;
                case '/': return Keys.OemQuestion;
                case '*': return Keys.Multiply;
                case '`': return Keys.Oemtilde;
                case '[': return Keys.OemOpenBrackets;
                case ']': return Keys.OemCloseBrackets;
                default: return Keys.None;
            }
        }


        private static readonly Dictionary<Keys, AndroidKeycode> keycodeDict = new Dictionary<Keys, AndroidKeycode>
        {


            { Keys.Space, AndroidKeycode.AKEYCODE_SPACE },
            { Keys.Back, AndroidKeycode.AKEYCODE_DEL },
            { Keys.Left, AndroidKeycode.AKEYCODE_DPAD_LEFT },
            { Keys.Up, AndroidKeycode.AKEYCODE_DPAD_UP },
            { Keys.Right, AndroidKeycode.AKEYCODE_DPAD_RIGHT },
            { Keys.Down, AndroidKeycode.AKEYCODE_DPAD_DOWN },
            { Keys.Delete, AndroidKeycode.AKEYCODE_FORWARD_DEL },
            { Keys.Tab, AndroidKeycode.AKEYCODE_TAB },
            { Keys.Enter, AndroidKeycode.AKEYCODE_ENTER },
            { Keys.OemOpenBrackets, AndroidKeycode.AKEYCODE_LEFT_BRACKET },
            { Keys.OemCloseBrackets, AndroidKeycode.AKEYCODE_RIGHT_BRACKET },
            { Keys.Oemcomma, AndroidKeycode.AKEYCODE_COMMA },
            { Keys.OemPeriod, AndroidKeycode.AKEYCODE_PERIOD },
            { Keys.OemSemicolon, AndroidKeycode.AKEYCODE_SEMICOLON },
            { Keys.OemQuotes, AndroidKeycode.AKEYCODE_APOSTROPHE },
            { Keys.OemBackslash, AndroidKeycode.AKEYCODE_BACKSLASH },
            { Keys.OemPipe, AndroidKeycode.AKEYCODE_BACKSLASH }, // Add for \ and |
            { Keys.Oemplus, AndroidKeycode.AKEYCODE_EQUALS }, // Map to = (unshifted)
            { Keys.OemMinus, AndroidKeycode.AKEYCODE_MINUS },
            { Keys.OemQuestion, AndroidKeycode.AKEYCODE_SLASH },
            { Keys.Oemtilde, AndroidKeycode.AKEYCODE_GRAVE },
            { Keys.Escape, AndroidKeycode.AKEYCODE_ESCAPE },
            { Keys.LControlKey, AndroidKeycode.AKEYCODE_CTRL_LEFT },
            { Keys.RControlKey, AndroidKeycode.AKEYCODE_CTRL_RIGHT },
            { Keys.LShiftKey, AndroidKeycode.AKEYCODE_SHIFT_LEFT },
            { Keys.RShiftKey, AndroidKeycode.AKEYCODE_SHIFT_RIGHT },
            { Keys.LMenu, AndroidKeycode.AKEYCODE_ALT_LEFT },
            { Keys.RMenu, AndroidKeycode.AKEYCODE_ALT_RIGHT },
             // Modifier keys - đảm bảo có đủ
           
        };

        public static AndroidKeycode ConvertKey(Keys key)
        {
            // A - Z
            if (key >= Keys.A && key <= Keys.Z)
            {
                int offset = (int)AndroidKeycode.AKEYCODE_A - (int)Keys.A;
                return (AndroidKeycode)((int)key + offset);
            }
            // Digits 0-9
            else if (key >= Keys.D0 && key <= Keys.D9)
            {
                int offset = (int)AndroidKeycode.AKEYCODE_0 - (int)Keys.D0;
                return (AndroidKeycode)((int)key + offset);
            }
            // NumPad 0-9
            else if (key >= Keys.NumPad0 && key <= Keys.NumPad9)
            {
                int offset = (int)AndroidKeycode.AKEYCODE_0 - (int)Keys.NumPad0; // Offset từ AKEYCODE_0 (7) thay vì AKEYCODE_NUMPAD_0 (144)
                return (AndroidKeycode)((int)key + offset);
            }
            // NumPad operators
            else if (key == Keys.Decimal) // Dấu chấm trên numpad
            {
                return AndroidKeycode.AKEYCODE_PERIOD; // Ánh xạ sang dấu chấm trên bàn phím chính
            }
            else if (key == Keys.Add) // Dấu cộng trên numpad
            {
                return AndroidKeycode.AKEYCODE_PLUS; // Ánh xạ sang dấu cộng trên bàn phím chính
            }
            else if (key == Keys.Subtract) // Dấu trừ trên numpad
            {
                return AndroidKeycode.AKEYCODE_MINUS; // Ánh xạ sang dấu trừ trên bàn phím chính
            }
            else if (key == Keys.Multiply) // Dấu nhân trên numpad
            {
                // Android không có keycode chuẩn cho "*" trên bàn phím chính, dùng AKEYCODE_ASTERISK (nếu đã thêm)
                return AndroidKeycode.AKEYCODE_ASTERISK; // Cần thêm AKEYCODE_ASTERISK = 292 vào AndroidKeycodes.cs
            }
            else if (key == Keys.Divide) // Dấu chia trên numpad
            {
                return AndroidKeycode.AKEYCODE_SLASH; // Ánh xạ sang dấu chia trên bàn phím chính
            }
            else if (key == Keys.Return || key == Keys.Enter)
            {
                // On most keyboards, NumPad Enter and main Enter both map to Keys.Enter.
                // If you need to distinguish, you may need to use KeyEventArgs.KeyCode and KeyEventArgs.Modifiers.
                return AndroidKeycode.AKEYCODE_NUMPAD_ENTER;
            }

            else if (keycodeDict.TryGetValue(key, out var androidKey))
            {
                return androidKey;
            }

            // Log.Warning("Unmapped key: {Key}", key);
            return AndroidKeycode.AKEYCODE_UNKNOWN;
        }

        [DllImport("user32.dll")]
        private static extern int ToUnicode(uint virtualKeyCode, uint scanCode, byte[] keyboardState,
            [Out, MarshalAs(UnmanagedType.LPWStr, SizeConst = 64)] StringBuilder receivingBuffer, int bufferSize, uint flags);

        public static string ConvertKeyToUnicode(Keys key)
        {
            byte[] keyboardState = new byte[256];
            StringBuilder receivingBuffer = new StringBuilder(64);
            int virtualKeyCode = (int)key;
            int scanCode = 0; // Scan code may need to be determined based on key
            int result = ToUnicode((uint)virtualKeyCode, (uint)scanCode, keyboardState, receivingBuffer, receivingBuffer.Capacity, 0);

            if (result > 0)
            {
                string unicodeChar = receivingBuffer.ToString();
                //Log.Debug("Converted key {Key} to Unicode: {Unicode}", key, unicodeChar);
                return unicodeChar;
            }

            //Log.Warning("Failed to convert key {Key} to Unicode", key);
            return string.Empty;
        }

        public static AndroidMetastate ConvertModifiers(Keys keyModifiers)
        {
            AndroidMetastate metastate = AndroidMetastate.AMETA_NONE;

            if ((keyModifiers & Keys.Shift) == Keys.Shift)
                metastate |= AndroidMetastate.AMETA_SHIFT_ON;

            if ((keyModifiers & Keys.Control) == Keys.Control)
                metastate |= AndroidMetastate.AMETA_CTRL_ON;

            if ((keyModifiers & Keys.Alt) == Keys.Alt)
                metastate |= AndroidMetastate.AMETA_ALT_ON;

            return metastate;
        }


    }

}
