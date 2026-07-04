using Dragon.Controller.TaskDeviceManager.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dragon.Controller.TaskDeviceManager.Infrastructure
{
    public static class DloopStore
    {
        // Form thêm/xóa/sửa bind trực tiếp vào đây
        public static ConcurrentDictionary<Guid, DLoop> Items { get; } = new();
    }
}
