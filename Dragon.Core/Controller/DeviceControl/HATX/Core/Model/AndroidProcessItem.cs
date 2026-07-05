using System;
using System.Collections.Generic;
using System.Text;

namespace Dragon.Controller.DeviceControl.HATX.Core.Model
{
    public class AndroidProcessItem
    {
        // luôn non-null, mặc định chuỗi rỗng
        public string User { get; set; } = string.Empty;

        // PID, mặc định -1 để phân biệt "chưa gán"
        public int Pid { get; set; } = -1;

        // tên tiến trình, luôn non-null
        public string Name { get; set; } = string.Empty;

        public AndroidProcessItem() { }

        public AndroidProcessItem(string user, int pid, string name)
        {
            User = user ?? string.Empty;
            Pid = pid;
            Name = name ?? string.Empty;
        }

        public override string ToString()
        {
            return $"{Pid} {Name} ({User})";
        }
    }
}
