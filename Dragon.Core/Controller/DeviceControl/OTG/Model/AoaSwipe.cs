using System;
using System.Collections.Generic;
using System.Text;

namespace Dragon.Controller.DeviceControl.OTG.Model
{
    public class AoaSwipe
    {
        public int NumSwipe { get; set; } = 1;
        public float X1 { get; set; }
        public float Y1 { get; set; }
        public float X2 { get; set; }
        public float Y2 { get; set; }
        public int DurationMs { get; set; } = 100; // <-- thêm
        public bool IsPerCent { get; set; } = true;
    }
}
