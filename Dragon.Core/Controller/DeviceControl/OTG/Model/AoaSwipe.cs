using System;
using System.Collections.Generic;
using System.Text;

namespace Dragon.Controller.DeviceControl.OTG.Model
{
    public class AoaSwipe
    {
        public int NumSwipe { get; set; } = 1;
        public int X1 { get; set; }
        public int Y1 { get; set; }
        public int X2 { get; set; }
        public int Y2 { get; set; }
        public int DurationMs { get; set; } = 100; // <-- thêm
    }
}
