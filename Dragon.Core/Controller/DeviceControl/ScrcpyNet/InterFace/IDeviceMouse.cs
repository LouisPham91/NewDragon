using Dragon.Controller.TaskDeviceManager.Core;
using Dragon.Controller.TaskDeviceManager.Model.Mouse;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dragon.Controller.DeviceControl.ScrcpyNet.InterFace
{
    public interface IDeviceMouse
    {
        Task<NodeResult> ClickAsync(DLoopContext ctx, ClickArg arg);
        Task<NodeResult> SwipeAsync(DLoopContext ctx, SwipeArg arg);
        Task<NodeResult> LongPressAsync(DLoopContext ctx, LongPressArg arg);
        Task<NodeResult> DragDropAsync(DLoopContext ctx, DragArg arg);

        Size renderSize { get; }
    }
}
