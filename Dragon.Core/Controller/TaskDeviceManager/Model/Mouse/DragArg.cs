
using System.Drawing;


namespace Dragon.Controller.TaskDeviceManager.Model.Mouse
{
    public class DragArg
    {
        public ControlMode ControlMode { get; set; }
        public List<Point> Points { get; set; } = new List<Point>();
        public int Duration { get; set; } = 2000; // ms, mặc định 2 giây
        public int DelayPerStep { get; set; }
        public int PixelsPerStep { get; set; }
    }


}
