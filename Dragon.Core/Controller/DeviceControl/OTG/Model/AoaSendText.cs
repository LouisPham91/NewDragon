using System;
using System.Collections.Generic;
using System.Text;

namespace Dragon.Controller.DeviceControl.OTG.Model
{
    public class AoaSendText
    {
        public string Text { get; set; } = "";
        public int DelayPerCharMs { get; set; } = 50;
    }
}
