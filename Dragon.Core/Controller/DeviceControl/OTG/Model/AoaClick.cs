using System;
using System.Collections.Generic;
using System.Text;

namespace Dragon.Controller.DeviceControl.OTG.Model
{
    public class AoaClick
    {
        public int NumClicks { get; set; } = 1;
        public int X {  get; set; }
        public int Y { get; set; }
        public int DelayBetweenMs { get; set; } = 300; // <-- thêm
    }
}
