using AdvancedSharpAdbClient;
using AdvancedSharpAdbClient.Models;
using AdvancedSharpAdbClient.Receivers;
using Dragon.Controller.TaskDeviceManager.Core;
using Dragon.Controller.TaskDeviceManager.Infrastructure;
using Dragon.Controller.TaskDeviceManager.Model.Mouse;
using Dragon.Database.Models;
using System.Diagnostics;

namespace Dragon.Controller.TaskDeviceManager.Infrastructure.Services
{
    public static class ADBMouseServices
    {
        public static async Task<NodeResult> Click(DLoopContext ctx, ClickArg click)
        {
            var phone = ctx.Session.Phone;
            var adb = ctx.AdbClient;
            var dd = ctx.DeviceData;
            var token = ctx.Token;
            var logKey = ctx.LogKey;

            if (phone == null || adb == null || dd == null || string.IsNullOrEmpty(dd.Serial))
            {
                Logger.Notify(logKey, "Thiếu tham số Click", Logger.Icon.Warning);
                return NodeResult.Stop();
            }
            if (token.IsCancellationRequested) return NodeResult.Stop();

            int x = (int)(phone.PhysicalWidth * click.x / 100.0);
            int y = (int)(phone.PhysicalHeight * click.y / 100.0);
            string cmd = $"input tap {x} {y}";

            try
            {
                var receiver = new ConsoleOutputReceiver();
                await adb.ExecuteRemoteCommandAsync(cmd, dd, receiver, token);

                if (click.ClickMode == ClickMode.DoubleClick)
                {
                    var d = await Logger.DelayAsync(120, token, logKey);
                    if (d == ExecutionStatus.Stop) return NodeResult.Stop();
                    await adb.ExecuteRemoteCommandAsync(cmd, dd, receiver, token);
                }

                Logger.Notify(logKey, $"ADB Click {click.ClickMode} ({x},{y})", Logger.Icon.Information);
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
                return NodeResult.Fail(ex.Message);
            }
        }

        public static async Task<NodeResult> Swipe(DLoopContext ctx, SwipeArg swipe)
        {
            var phone = ctx.Session.Phone;
            var adb = ctx.AdbClient;
            var dd = ctx.DeviceData;
            var token = ctx.Token;
            var logKey = ctx.LogKey;

            if (phone == null || adb == null || dd == null)
            {
                Logger.Notify(logKey, "Thiếu tham số Swipe", Logger.Icon.Warning);
                return NodeResult.Stop();
            }
            if (token.IsCancellationRequested) return NodeResult.Stop();

            var sw = Stopwatch.StartNew();
            int loopCount = swipe.loopTime;
            if (swipe.SwipeMode == SwipeMode.Random && swipe.randMax >= swipe.randMin)
                loopCount = Random.Shared.Next(swipe.randMin, swipe.randMax + 1);

            var rand = Random.Shared;

            try
            {
                for (int i = 0; i < loopCount; i++)
                {
                    if (token.IsCancellationRequested) return NodeResult.Stop();

                    int x1, y1, x2, y2;
                    switch (swipe.Direction)
                    {
                        case Direction.Down:
                            x1 = x2 = (int)(phone.PhysicalWidth * rand.Next((int)swipe.Left, (int)swipe.Right + 1) / 100.0);
                            y1 = (int)(phone.PhysicalHeight * swipe.Bottom / 100.0);
                            y2 = (int)(phone.PhysicalHeight * swipe.Top / 100.0);
                            break;
                        case Direction.Up:
                            x1 = x2 = (int)(phone.PhysicalWidth * rand.Next((int)swipe.Left, (int)swipe.Right + 1) / 100.0);
                            y1 = (int)(phone.PhysicalHeight * swipe.Top / 100.0);
                            y2 = (int)(phone.PhysicalHeight * swipe.Bottom / 100.0);
                            break;
                        case Direction.Right:
                            y1 = y2 = (int)(phone.PhysicalHeight * rand.Next((int)swipe.Top, (int)swipe.Bottom + 1) / 100.0);
                            x1 = (int)(phone.PhysicalWidth * swipe.Right / 100.0);
                            x2 = (int)(phone.PhysicalWidth * swipe.Left / 100.0);
                            break;
                        case Direction.Left:
                            y1 = y2 = (int)(phone.PhysicalHeight * rand.Next((int)swipe.Top, (int)swipe.Bottom + 1) / 100.0);
                            x1 = (int)(phone.PhysicalWidth * swipe.Left / 100.0);
                            x2 = (int)(phone.PhysicalWidth * swipe.Right / 100.0);
                            break;
                        default:
                            Logger.Notify(logKey, "Direction không hợp lệ", Logger.Icon.Warning);
                            return NodeResult.Stop();
                    }

                    var receiver = new ConsoleOutputReceiver();
                    string cmd = $"input swipe {x1} {y1} {x2} {y2} {swipe.duration}";
                    await adb.ExecuteRemoteCommandAsync(cmd, dd, receiver, token);

                    Logger.Notify(logKey, $"Swipe {swipe.Direction} {i + 1}/{loopCount}", Logger.Icon.Information);

                    if (swipe.DelayPerLoop > 0)
                    {
                        var d = await Logger.DelayAsync(swipe.DelayPerLoop, token, logKey);
                        if (d == ExecutionStatus.Stop) return NodeResult.Stop();
                    }

                }

                sw.Stop();
                Logger.Notify(logKey, $"Swipe xong {swipe.Direction} {sw.ElapsedMilliseconds}ms", Logger.Icon.Information);
                return NodeResult.Ok();
            }
            catch (OperationCanceledException)
            {
                Logger.Notify(logKey, "Swipe bị hủy", Logger.Icon.Warning);
                return NodeResult.Stop();
            }
            catch (Exception ex)
            {
                Logger.Notify(logKey, $"Lỗi Swipe: {ex.Message}", Logger.Icon.Error);
                return NodeResult.Fail(ex.Message);
            }
        }

        public static async Task<NodeResult> LongPress(DLoopContext ctx, LongPressArg press)
        {
            var phone = ctx.Session.Phone;
            var adb = ctx.AdbClient;
            var dd = ctx.DeviceData;
            var token = ctx.Token;
            var logKey = ctx.LogKey;

            if (phone == null || adb == null || dd == null)
            {
                Logger.Notify(logKey, "Thiếu tham số LongPress", Logger.Icon.Warning);
                return NodeResult.Stop();
            }
            if (token.IsCancellationRequested) return NodeResult.Stop();

            int x = (int)(phone.PhysicalWidth * press.x / 100.0);
            int y = (int)(phone.PhysicalHeight * press.y / 100.0);
            string cmd = $"input swipe {x} {y} {x} {y} {press.Duration}";

            try
            {
                var receiver = new ConsoleOutputReceiver();
                await adb.ExecuteRemoteCommandAsync(cmd, dd, receiver, token);
                Logger.Notify(logKey, $"LongPress ({x},{y}) {press.Duration}ms", Logger.Icon.Information);
                return NodeResult.Ok();
            }
            catch (OperationCanceledException)
            {
                Logger.Notify(logKey, "LongPress bị hủy", Logger.Icon.Warning);
                return NodeResult.Stop();
            }
            catch (Exception ex)
            {
                Logger.Notify(logKey, $"Lỗi LongPress: {ex.Message}", Logger.Icon.Error);
                return NodeResult.Fail(ex.Message);
            }
        }

    }
}