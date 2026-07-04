

namespace Dragon.Controller.TaskDeviceManager.Model.Mouse
{
    public class SwipeArg
    {
        public ControlMode ControlMode { get; set; }
        public SwipeMode SwipeMode { get; set; } = SwipeMode.Fixed;
        public int randMin { get; set; } = 1;
        public int randMax { get; set; } = 5;
        public int loopTime { get; set; } = 1;
        public Direction Direction { get; set; }
        public float Left { get; set; } = 5.0F;    // 5% chiều ngang
        public float Right { get; set; } = 95.0F;  // 95% chiều ngang
        public float Top { get; set; } = 20.0F;    // 20% chiều dọc
        public float Bottom { get; set; } = 95.0F; // 95% chiều dọc
        public int DelayPerLoop { get; set; }
        public int PixelPerStep { get; set; } = 50;
        public int duration { get; set; } = 55;
        public int DelayStep { get; set; }
        public int UHDIMoveDelay { get; set; }

    }
}
