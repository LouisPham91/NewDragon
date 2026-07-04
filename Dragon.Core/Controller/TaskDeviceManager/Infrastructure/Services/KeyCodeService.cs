using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dragon.Controller.TaskDeviceManager.Infrastructure.Services
{
    public static class KeyCodeService
    {
        public static readonly Dictionary<string, string> KeyIntentDict = new()
    {
        { "home",        "input keyevent 3" },
        { "back",        "input keyevent 4" },
        { "left",        "input keyevent 21" },
        { "right",       "input keyevent 22" },
        { "up",          "input keyevent 19" },
        { "down",        "input keyevent 20" },
        { "center",      "input keyevent 23" },
        { "menu",        "input keyevent 82" },
        { "search",      "input keyevent 84" },
        { "enter",       "input keyevent 66" },
        { "delete",      "input keyevent 67" },
        { "recent",      "input keyevent 187" },
        { "volume_up",   "input keyevent 24" },
        { "volume_down", "input keyevent 25" },
        { "volume_mute", "input keyevent 164" },
        { "camera",      "input keyevent 27" },
        { "power",       "input keyevent 26" }
    };

        // hàm mới — runner gọi cái này
        public static string GetCommand(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return "input keyevent 66"; // default ENTER

            var key = name.ToLower().Trim();
            return KeyIntentDict.TryGetValue(key, out var cmd)
                ? cmd
                : $"input keyevent {name}"; // nếu truyền số hoặc custom
        }
    }
}
