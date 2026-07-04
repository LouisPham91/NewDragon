using Dragon.Controller.TaskDeviceManager.Core;
using Dragon.Controller.TaskDeviceManager.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dragon.Controller.TaskDeviceManager.Infrastructure
{
    public class Logger
    {
        public enum Icon { None, Hand, Stop, Error, Question, Exclamation, Warning, Asterisk, Information }

        public static void Notify(string deviceId, string message, Icon icon)
            => LogHub.Write(deviceId, message, icon);

        public static async Task<ExecutionStatus> DelayAsync(int ms, CancellationToken token, string deviceId)
        {
            if (ms <= 0) return ExecutionStatus.Continue;
            Notify(deviceId, $"Đang chờ {ms} ms", Icon.Information);
            int remain = ms;
            while (remain > 0)
            {
                if (token.IsCancellationRequested) { Notify(deviceId, "Bị hủy bởi token", Icon.Warning); return ExecutionStatus.Stop; }
                int step = remain >= 100 ? 100 : remain;
                try { await Task.Delay(step, token); } catch { Notify(deviceId, "Bị hủy bởi token", Icon.Warning); return ExecutionStatus.Stop; }
                remain -= step;
            }
            return ExecutionStatus.Continue;
        }
    }
}
