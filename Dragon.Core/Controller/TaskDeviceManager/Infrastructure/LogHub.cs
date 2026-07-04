using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace Dragon.Controller.TaskDeviceManager.Infrastructure
{
    public static class LogHub
    {
        private class LogEntry { public DateTime Time; public string Msg = ""; public Logger.Icon Icon; }

        private static readonly ConcurrentDictionary<string, ConcurrentQueue<LogEntry>> _buffers = new();
        // THAY RichTextBox bằng Action
        private static readonly ConcurrentDictionary<string, Action<DateTime, string, Logger.Icon>> _subscribers = new();

        public static void Write(string deviceId, string message, Logger.Icon icon)
        {
            var q = _buffers.GetOrAdd(deviceId, _ => new());
            var entry = new LogEntry { Time = DateTime.Now, Msg = message, Icon = icon };
            q.Enqueue(entry);
            while (q.Count > 500) q.TryDequeue(out _);

            if (_subscribers.TryGetValue(deviceId, out var handler))
                handler(entry.Time, message, icon);
        }

        // UI sẽ gọi hàm này
        public static void Attach(string deviceId, Action<DateTime, string, Logger.Icon> onLog)
        {
            _subscribers[deviceId] = onLog;
            // đẩy lại lịch sử
            if (_buffers.TryGetValue(deviceId, out var q))
                foreach (var e in q) onLog(e.Time, e.Msg, e.Icon);
        }

        public static void Detach(string deviceId) => _subscribers.TryRemove(deviceId, out _);
    }
}