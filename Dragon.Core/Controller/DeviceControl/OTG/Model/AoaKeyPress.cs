using System;
using System.Collections.Generic;
using System.Text;

namespace Dragon.Controller.DeviceControl.OTG.Model
{
    public class AoaKeyPress
    {
        public string Key { get; set; } = "Home"; // Home, Back, AppSwitch, Enter, VolumeUp...
        public int Repeat { get; set; } = 1;
        public int DelayBetweenMs { get; set; } = 200;
    }
}
