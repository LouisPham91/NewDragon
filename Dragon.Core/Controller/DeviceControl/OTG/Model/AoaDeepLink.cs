using System;
using System.Collections.Generic;
using System.Text;

namespace Dragon.Controller.DeviceControl.OTG.Model
{
    public class AoaDeeplink
    {
        public string Url { get; set; } = ""; // ví dụ "settings"
        public int WaitAfterMs { get; set; } = 1500;
    }
}
