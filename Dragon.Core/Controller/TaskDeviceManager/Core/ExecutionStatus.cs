using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dragon.Controller.TaskDeviceManager.Core
{
    public enum ExecutionStatus
    {
        Continue,   // chạy tiếp (tên cũ Next)
        Stop,       // dừng cả cây
        Break,      // thoát vòng lặp hiện tại
        Skip,       // bỏ qua 1 node
        SkipTo,     // nhảy đến ID (thay SkipUntil)
        Retry       // thử lại
    }

}
