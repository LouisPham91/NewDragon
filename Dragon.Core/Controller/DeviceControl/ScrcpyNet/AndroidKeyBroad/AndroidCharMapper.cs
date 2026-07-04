using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Dragon.Controller.DeviceControl.ScrcpyNet.AndroidKeyBroad
{
  
    public static class AndroidCharMapper
    {
        private static readonly Dictionary<char, AndroidKeycode> charToKeycode = new()
    {
        // Chữ số
        { '0', AndroidKeycode.AKEYCODE_0 },
        { '1', AndroidKeycode.AKEYCODE_1 },
        { '2', AndroidKeycode.AKEYCODE_2 },
        { '3', AndroidKeycode.AKEYCODE_3 },
        { '4', AndroidKeycode.AKEYCODE_4 },
        { '5', AndroidKeycode.AKEYCODE_5 },
        { '6', AndroidKeycode.AKEYCODE_6 },
        { '7', AndroidKeycode.AKEYCODE_7 },
        { '8', AndroidKeycode.AKEYCODE_8 },
        { '9', AndroidKeycode.AKEYCODE_9 },

        // Chữ thường a-z
        { 'a', AndroidKeycode.AKEYCODE_A },
        { 'b', AndroidKeycode.AKEYCODE_B },
        { 'c', AndroidKeycode.AKEYCODE_C },
        { 'd', AndroidKeycode.AKEYCODE_D },
        { 'e', AndroidKeycode.AKEYCODE_E },
        { 'f', AndroidKeycode.AKEYCODE_F },
        { 'g', AndroidKeycode.AKEYCODE_G },
        { 'h', AndroidKeycode.AKEYCODE_H },
        { 'i', AndroidKeycode.AKEYCODE_I },
        { 'j', AndroidKeycode.AKEYCODE_J },
        { 'k', AndroidKeycode.AKEYCODE_K },
        { 'l', AndroidKeycode.AKEYCODE_L },
        { 'm', AndroidKeycode.AKEYCODE_M },
        { 'n', AndroidKeycode.AKEYCODE_N },
        { 'o', AndroidKeycode.AKEYCODE_O },
        { 'p', AndroidKeycode.AKEYCODE_P },
        { 'q', AndroidKeycode.AKEYCODE_Q },
        { 'r', AndroidKeycode.AKEYCODE_R },
        { 's', AndroidKeycode.AKEYCODE_S },
        { 't', AndroidKeycode.AKEYCODE_T },
        { 'u', AndroidKeycode.AKEYCODE_U },
        { 'v', AndroidKeycode.AKEYCODE_V },
        { 'w', AndroidKeycode.AKEYCODE_W },
        { 'x', AndroidKeycode.AKEYCODE_X },
        { 'y', AndroidKeycode.AKEYCODE_Y },
        { 'z', AndroidKeycode.AKEYCODE_Z },

        // Chữ hoa A-Z (dùng chung keycode với chữ thường, cần kết hợp modifier SHIFT khi gửi phím)
        { 'A', AndroidKeycode.AKEYCODE_A },
        { 'B', AndroidKeycode.AKEYCODE_B },
        { 'C', AndroidKeycode.AKEYCODE_C },
        { 'D', AndroidKeycode.AKEYCODE_D },
        { 'E', AndroidKeycode.AKEYCODE_E },
        { 'F', AndroidKeycode.AKEYCODE_F },
        { 'G', AndroidKeycode.AKEYCODE_G },
        { 'H', AndroidKeycode.AKEYCODE_H },
        { 'I', AndroidKeycode.AKEYCODE_I },
        { 'J', AndroidKeycode.AKEYCODE_J },
        { 'K', AndroidKeycode.AKEYCODE_K },
        { 'L', AndroidKeycode.AKEYCODE_L },
        { 'M', AndroidKeycode.AKEYCODE_M },
        { 'N', AndroidKeycode.AKEYCODE_N },
        { 'O', AndroidKeycode.AKEYCODE_O },
        { 'P', AndroidKeycode.AKEYCODE_P },
        { 'Q', AndroidKeycode.AKEYCODE_Q },
        { 'R', AndroidKeycode.AKEYCODE_R },
        { 'S', AndroidKeycode.AKEYCODE_S },
        { 'T', AndroidKeycode.AKEYCODE_T },
        { 'U', AndroidKeycode.AKEYCODE_U },
        { 'V', AndroidKeycode.AKEYCODE_V },
        { 'W', AndroidKeycode.AKEYCODE_W },
        { 'X', AndroidKeycode.AKEYCODE_X },
        { 'Y', AndroidKeycode.AKEYCODE_Y },
        { 'Z', AndroidKeycode.AKEYCODE_Z },

        // Ký tự đặc biệt
        { ' ', AndroidKeycode.AKEYCODE_SPACE },
        { '\n', AndroidKeycode.AKEYCODE_ENTER },
        { '\r', AndroidKeycode.AKEYCODE_ENTER },        // Carriage return cũng map vào ENTER
        { '\t', AndroidKeycode.AKEYCODE_TAB },
        { '\b', AndroidKeycode.AKEYCODE_DEL },           // Backspace
        { ',', AndroidKeycode.AKEYCODE_COMMA },
        { '.', AndroidKeycode.AKEYCODE_PERIOD },
        { ';', AndroidKeycode.AKEYCODE_SEMICOLON },
        { '\'', AndroidKeycode.AKEYCODE_APOSTROPHE },
        { '\\', AndroidKeycode.AKEYCODE_BACKSLASH },
        { '=', AndroidKeycode.AKEYCODE_EQUALS },
        { '+', AndroidKeycode.AKEYCODE_PLUS },
        { '-', AndroidKeycode.AKEYCODE_MINUS },
        { '/', AndroidKeycode.AKEYCODE_SLASH },
        { '*', AndroidKeycode.AKEYCODE_STAR },
        { '`', AndroidKeycode.AKEYCODE_GRAVE },
        { '[', AndroidKeycode.AKEYCODE_LEFT_BRACKET },
        { ']', AndroidKeycode.AKEYCODE_RIGHT_BRACKET },
        { '@', AndroidKeycode.AKEYCODE_AT },
        { '#', AndroidKeycode.AKEYCODE_POUND },

        // Thêm các ký tự mở rộng
        { '!', AndroidKeycode.AKEYCODE_1 },              // Shift + 1
        { '"', AndroidKeycode.AKEYCODE_APOSTROPHE },     // Shift + '
        { '£', AndroidKeycode.AKEYCODE_POUND },          // Có thể cần xử lý khác tùy layout
        { '$', AndroidKeycode.AKEYCODE_4 },              // Shift + 4
        { '%', AndroidKeycode.AKEYCODE_5 },              // Shift + 5
        { '^', AndroidKeycode.AKEYCODE_6 },              // Shift + 6
        { '&', AndroidKeycode.AKEYCODE_7 },              // Shift + 7
        { '(', AndroidKeycode.AKEYCODE_9 },              // Shift + 9
        { ')', AndroidKeycode.AKEYCODE_0 },              // Shift + 0
        { '_', AndroidKeycode.AKEYCODE_MINUS },          // Shift + -
        { '<', AndroidKeycode.AKEYCODE_COMMA },          // Shift + ,
        { '>', AndroidKeycode.AKEYCODE_PERIOD },         // Shift + .
        { '?', AndroidKeycode.AKEYCODE_SLASH },          // Shift + /
        { ':', AndroidKeycode.AKEYCODE_SEMICOLON },      // Shift + ;
        { '{', AndroidKeycode.AKEYCODE_LEFT_BRACKET },   // Shift + [
        { '}', AndroidKeycode.AKEYCODE_RIGHT_BRACKET },  // Shift + ]
        { '|', AndroidKeycode.AKEYCODE_BACKSLASH },      // Shift + \
        { '~', AndroidKeycode.AKEYCODE_GRAVE },          // Shift + `



    };

        public static AndroidKeycode CharToAndroidKeycode(char c)
        {
            if (charToKeycode.TryGetValue(c, out var code))
                return code;
            return AndroidKeycode.AKEYCODE_UNKNOWN;
        }

        /// <summary>
        /// Kiểm tra xem ký tự có cần phím Shift hay không
        /// </summary>
        public static bool RequiresShift(char c)
        {
            return c switch
            {
                >= 'A' and <= 'Z' => true,
                '!' or '@' or '#' or '$' or '%' or '^' or '&' or '*' or '(' or ')' => true,
                '_' or '+' or '{' or '}' or '|' or ':' or '"' or '<' or '>' or '?' or '~' => true,
                _ => false
            };
        }
    }

    [Flags]
    public enum AndroidMetaState : int
    {
        AMETA_NONE = 0,
        AMETA_ALT_ON = 0x02,
        AMETA_ALT_LEFT_ON = 0x10,
        AMETA_ALT_RIGHT_ON = 0x20,
        AMETA_SHIFT_ON = 0x01,
        AMETA_SHIFT_LEFT_ON = 0x40,
        AMETA_SHIFT_RIGHT_ON = 0x80,
        AMETA_SYM_ON = 0x04,
        AMETA_FUNCTION_ON = 0x08,
        AMETA_CTRL_ON = 0x1000,
        AMETA_CTRL_LEFT_ON = 0x2000,
        AMETA_CTRL_RIGHT_ON = 0x4000,
        AMETA_META_ON = 0x10000,
        AMETA_META_LEFT_ON = 0x20000,
        AMETA_META_RIGHT_ON = 0x40000,
        AMETA_CAPS_LOCK_ON = 0x100000,
        AMETA_NUM_LOCK_ON = 0x200000,
        AMETA_SCROLL_LOCK_ON = 0x400000
    }
}