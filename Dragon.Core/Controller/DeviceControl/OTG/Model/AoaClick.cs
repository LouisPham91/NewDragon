using System;
using System.Collections.Generic;
using System.Text;

namespace Dragon.Controller.DeviceControl.OTG.Model
{
    public class AoaClick
    {
        public int NumClicks { get; set; } = 1;
        public float X {  get; set; }
        public float Y { get; set; }
        public int DelayBetweenMs { get; set; } = 300; // <-- thêm
        public bool IsPerCent { get; set; } = true;
    }
}
