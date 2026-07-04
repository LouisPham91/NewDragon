using AdvancedSharpAdbClient;
using AdvancedSharpAdbClient.Models;
using AdvancedSharpAdbClient.Receivers;
using Dragon.Controller.TaskDeviceManager.Core;
using Dragon.Controller.TaskDeviceManager.Infrastructure;
using Dragon.Controller.TaskDeviceManager.Model.Mouse;
using Dragon.Database.Models;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;

namespace Dragon.Controller.TaskDeviceManager.Infrastructure.Services
{
    public static class ADBEventServices
    {
        public static ConcurrentDictionary<string, string> InputEventDict { get; } = new();
        private const string DefaultEventDevice = "/dev/input/event1";

        // ===== PUBLIC API - chỉ nhận DLoopContext =====
        public static async Task<NodeResult> Click(DLoopContext ctx, ClickArg click)
        {
            var phone = ctx.Session.Phone;
            var adb = ctx.AdbClient;
            var dd = ctx.DeviceData;
            var token = ctx.Token;
            var logKey = ctx.LogKey;

            if (phone == null || adb == null || dd == null)
            {
                Logger.Notify(logKey, "Thiếu tham số Click", Logger.Icon.Warning);
                return NodeResult.Stop();
            }
            if (token.IsCancellationRequested) return NodeResult.Stop();

            try
            {
                string ev = await EnsureEventDeviceAsync(logKey, dd, adb, token);
                int x = (int)(phone.PhysicalWidth * click.x / 100.0);
                int y = (int)(phone.PhysicalHeight * click.y / 100.0);

                string cmd = $"sendevent {ev} 3 57 123 && sendevent {ev} 3 53 {x} && sendevent {ev} 3 54 {y} && sendevent {ev} 3 58 50 && sendevent {ev} 1 330 1 && sendevent {ev} 0 0 0 && sleep 0.100 && sendevent {ev} 3 57 -1 && sendevent {ev} 1 330 0 && sendevent {ev} 0 0 0";

                var rcv = new ConsoleOutputReceiver();
                await adb.ExecuteRemoteCommandAsync(cmd, dd, rcv, token);

                if (click.ClickMode == ClickMode.DoubleClick)
                {
                    var d = await Logger.DelayAsync(120, token, logKey);
                    if (d == ExecutionStatus.Stop) return NodeResult.Stop();
                    await adb.ExecuteRemoteCommandAsync(cmd, dd, rcv, token);
                }

                Logger.Notify(logKey, $"Event Click {click.ClickMode} ({x},{y})", Logger.Icon.Information);
                return NodeResult.Ok();
            }
            catch (OperationCanceledException)
            {
                Logger.Notify(logKey, "Click bị hủy", Logger.Icon.Warning);
                return NodeResult.Stop();
            }
            catch (Exception ex)
            {
                Logger.Notify(logKey, $"Lỗi Click: {ex.Message}", Logger.Icon.Error);
                return NodeResult.Stop();
            }
        }

        public static async Task<NodeResult> LongPress(DLoopContext ctx, LongPressArg press)
        {
            var phone = ctx.Session.Phone;
            var adb = ctx.AdbClient;
            var dd = ctx.DeviceData;
            var token = ctx.Token;
            var logKey = ctx.LogKey;

            if (phone == null || adb == null || dd == null) return NodeResult.Stop();
            if (token.IsCancellationRequested) return NodeResult.Stop();

            try
            {
                string ev = await EnsureEventDeviceAsync(logKey, dd, adb, token);
                int x = (int)(phone.PhysicalWidth * press.x / 100.0);
                int y = (int)(phone.PhysicalHeight * press.y / 100.0);
                double sec = press.Duration / 1000.0;

                string cmd = $"sendevent {ev} 3 57 123 && sendevent {ev} 3 53 {x} && sendevent {ev} 3 54 {y} && sendevent {ev} 3 58 50 && sendevent {ev} 1 330 1 && sendevent {ev} 0 0 0 && sleep {sec.ToString("0.000", CultureInfo.InvariantCulture)} && sendevent {ev} 3 57 -1 && sendevent {ev} 1 330 0 && sendevent {ev} 0 0 0";

                await adb.ExecuteRemoteCommandAsync(cmd, dd, new ConsoleOutputReceiver(), token);
                Logger.Notify(logKey, $"Event LongPress ({x},{y}) {press.Duration}ms", Logger.Icon.Information);
                return NodeResult.Ok();
            }
            catch (OperationCanceledException)
            {
                return NodeResult.Stop();
            }
            catch (Exception ex)
            {
                Logger.Notify(logKey, $"Lỗi LongPress: {ex.Message}", Logger.Icon.Error);
                return NodeResult.Stop();
            }
        }

        public static async Task<NodeResult> Swipe(DLoopContext ctx, SwipeArg swipe)
        {
            var phone = ctx.Session.Phone;
            var adb = ctx.AdbClient;
            var dd = ctx.DeviceData;
            var token = ctx.Token;
            var logKey = ctx.LogKey;

            if (phone == null || adb == null || dd == null) return NodeResult.Stop();

            var sw = Stopwatch.StartNew();
            int loopCount = swipe.loopTime;
            if (swipe.SwipeMode == SwipeMode.Random && swipe.randMax >= swipe.randMin)
                loopCount = Random.Shared.Next(swipe.randMin, swipe.randMax + 1);

            try
            {
                string ev = await EnsureEventDeviceAsync(logKey, dd, adb, token);

                for (int i = 0; i < loopCount; i++)
                {
                    if (token.IsCancellationRequested) return NodeResult.Stop();

                    int x1, y1, x2, y2;
                    var r = Random.Shared;
                    switch (swipe.Direction)
                    {
                        case Direction.Down:
                            x1 = x2 = (int)(phone.PhysicalWidth * r.Next((int)swipe.Left, (int)swipe.Right + 1) / 100.0);
                            y1 = (int)(phone.PhysicalHeight * swipe.Bottom / 100.0);
                            y2 = (int)(phone.PhysicalHeight * swipe.Top / 100.0); break;
                        case Direction.Up:
                            x1 = x2 = (int)(phone.PhysicalWidth * r.Next((int)swipe.Left, (int)swipe.Right + 1) / 100.0);
                            y1 = (int)(phone.PhysicalHeight * swipe.Top / 100.0);
                            y2 = (int)(phone.PhysicalHeight * swipe.Bottom / 100.0); break;
                        case Direction.Right:
                            y1 = y2 = (int)(phone.PhysicalHeight * r.Next((int)swipe.Top, (int)swipe.Bottom + 1) / 100.0);
                            x1 = (int)(phone.PhysicalWidth * swipe.Right / 100.0);
                            x2 = (int)(phone.PhysicalWidth * swipe.Left / 100.0); break;
                        case Direction.Left:
                            y1 = y2 = (int)(phone.PhysicalHeight * r.Next((int)swipe.Top, (int)swipe.Bottom + 1) / 100.0);
                            x1 = (int)(phone.PhysicalWidth * swipe.Left / 100.0);
                            x2 = (int)(phone.PhysicalWidth * swipe.Right / 100.0); break;
                        default: return NodeResult.Stop();
                    }

                    await DoSwipe(ev, adb, dd, x1, y1, x2, y2, swipe.duration, 8, token);
                    Logger.Notify(logKey, $"Event Swipe {swipe.Direction} {i + 1}/{loopCount}", Logger.Icon.Information);

                    if (swipe.DelayPerLoop > 0)
                    {
                        var d = await Logger.DelayAsync(swipe.DelayPerLoop, token, logKey);
                        if (d == ExecutionStatus.Stop) return NodeResult.Stop();
                    }

                }

                sw.Stop();
                return NodeResult.Ok();
            }
            catch (OperationCanceledException)
            {
                return NodeResult.Stop();
            }
            catch (Exception ex)
            {
                Logger.Notify(logKey, $"Lỗi Swipe: {ex.Message}", Logger.Icon.Error);
                return NodeResult.Stop();
            }
        }

        public static async Task<NodeResult> DragAndDrop(DLoopContext ctx, DragArg drag)
        {
            var phone = ctx.Session.Phone;
            var adb = ctx.AdbClient;
            var dd = ctx.DeviceData;
            var token = ctx.Token;
            var logKey = ctx.LogKey;

            if (phone == null || adb == null || dd == null || drag.Points == null || drag.Points.Count < 2)
                return NodeResult.Stop();

            try
            {
                string ev = await EnsureEventDeviceAsync(logKey, dd, adb, token);
                var cmds = new List<string>();
                var start = drag.Points[0];
                int xs = (int)(phone.PhysicalWidth * start.X / 100.0);
                int ys = (int)(phone.PhysicalHeight * start.Y / 100.0);
                cmds.AddRange(new[] { $"sendevent {ev} 3 57 123", $"sendevent {ev} 3 53 {xs}", $"sendevent {ev} 3 54 {ys}", $"sendevent {ev} 3 58 50", $"sendevent {ev} 1 330 1", $"sendevent {ev} 0 0 0" });

                int seg = drag.Points.Count - 1;
                int dur = Math.Max(1, drag.Duration / Math.Max(1, seg));
                for (int s = 0; s < seg; s++)
                {
                    var p1 = drag.Points[s]; var p2 = drag.Points[s + 1];
                    int x1 = (int)(phone.PhysicalWidth * p1.X / 100.0);
                    int y1 = (int)(phone.PhysicalHeight * p1.Y / 100.0);
                    int x2 = (int)(phone.PhysicalWidth * p2.X / 100.0);
                    int y2 = (int)(phone.PhysicalHeight * p2.Y / 100.0);
                    for (int i = 1; i <= 15; i++)
                    {
                        if (token.IsCancellationRequested) return NodeResult.Stop();
                        int xi = x1 + (x2 - x1) * i / 15;
                        int yi = y1 + (y2 - y1) * i / 15;
                        cmds.Add($"sendevent {ev} 3 53 {xi}");
                        cmds.Add($"sendevent {ev} 3 54 {yi}");
                        cmds.Add($"sendevent {ev} 0 0 0");
                    }
                    cmds.Add($"sleep {(dur / 1000.0).ToString("0.000", CultureInfo.InvariantCulture)}");
                }

                cmds.AddRange(new[] { $"sendevent {ev} 3 57 -1", $"sendevent {ev} 1 330 0", $"sendevent {ev} 0 0 0" });

                for (int i = 0; i < cmds.Count; i += 20)
                {
                    if (token.IsCancellationRequested) return NodeResult.Stop();
                    await adb.ExecuteRemoteCommandAsync(string.Join(" && ", cmds.Skip(i).Take(20)), dd, new ConsoleOutputReceiver(), token);
                }

                Logger.Notify(logKey, $"Event Drag {drag.Points.Count} điểm xong", Logger.Icon.Information);
                return NodeResult.Ok();
            }
            catch (OperationCanceledException)
            {
                return NodeResult.Stop();
            }
            catch (Exception ex)
            {
                Logger.Notify(logKey, $"Lỗi Drag: {ex.Message}", Logger.Icon.Error);
                return NodeResult.Stop();
            }
        }

        // ===== Helpers =====
        private static async Task<string> EnsureEventDeviceAsync(string deviceId, DeviceData dd, AdbClient adb, CancellationToken token)
        {
            if (InputEventDict.TryGetValue(deviceId, out var existing) && !string.IsNullOrWhiteSpace(existing))
                return existing;

            try
            {
                var detected = await DetectTouchEventDevice(dd, adb, token);
                if (!string.IsNullOrEmpty(detected))
                {
                    InputEventDict[deviceId] = detected;
                    return detected;
                }
            }
            catch { }
            InputEventDict[deviceId] = DefaultEventDevice;
            return DefaultEventDevice;
        }

        public static async Task<string?> DetectTouchEventDevice(DeviceData dd, AdbClient adb, CancellationToken token)
        {
            var output = new ConsoleOutputReceiver();
            await adb.ExecuteRemoteCommandAsync("getevent -lp", dd, output, token);
            string? current = null;
            foreach (var raw in output.ToString().Split('\n'))
            {
                var line = raw.Trim();
                if (line.Contains("/dev/input/event"))
                    current = line.Split(' ').LastOrDefault(p => p.StartsWith("/dev/input/event"));
                if (current != null && (line.Contains("ABS_MT_POSITION_X") || line.Contains("BTN_TOUCH")))
                    return current;
            }
            return null;
        }

        private static async Task DoSwipe(string ev, AdbClient adb, DeviceData dd, int x1, int y1, int x2, int y2, int duration, int pixelsPerStep, CancellationToken token)
        {
            int distance = (int)Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
            int steps = Math.Max(10, distance / Math.Max(1, pixelsPerStep));
            var cmds = new List<string>
            {
                $"sendevent {ev} 3 57 123",
                $"sendevent {ev} 3 53 {x1}",
                $"sendevent {ev} 3 54 {y1}",
                $"sendevent {ev} 3 58 50",
                $"sendevent {ev} 1 330 1",
                $"sendevent {ev} 0 0 0"
            };
            for (int i = 1; i <= steps; i++)
            {
                int xi = x1 + (x2 - x1) * i / steps;
                int yi = y1 + (y2 - y1) * i / steps;
                cmds.Add($"sendevent {ev} 3 53 {xi}");
                cmds.Add($"sendevent {ev} 3 54 {yi}");
                cmds.Add($"sendevent {ev} 0 0 0");
            }
            cmds.Add($"sleep {(duration / 1000.0).ToString("0.000", CultureInfo.InvariantCulture)}");
            cmds.Add($"sendevent {ev} 3 57 -1");
            cmds.Add($"sendevent {ev} 1 330 0");
            cmds.Add($"sendevent {ev} 0 0 0");

            for (int i = 0; i < cmds.Count; i += 20)
            {
                if (token.IsCancellationRequested) return;
                await adb.ExecuteRemoteCommandAsync(string.Join(" && ", cmds.Skip(i).Take(20)), dd, new ConsoleOutputReceiver(), token);
            }
        }
    }
}