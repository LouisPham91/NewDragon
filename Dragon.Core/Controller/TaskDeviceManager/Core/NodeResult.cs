using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dragon.Controller.TaskDeviceManager.Core
{
    public class NodeResult
    {
        public ExecutionStatus Status { get; set; } = ExecutionStatus.Continue;
        public object? Value { get; set; }
        public string? SkipToId { get; set; }
        public string? Error { get; set; }

        public static NodeResult Ok(object? val = null) => new() { Status = ExecutionStatus.Continue, Value = val };
        public static NodeResult Stop() => new() { Status = ExecutionStatus.Stop };
        public static NodeResult Fail(string error) => new() { Status = ExecutionStatus.Stop, Error = error };
    }
}
