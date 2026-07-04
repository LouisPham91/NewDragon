using AdvancedSharpAdbClient;
using AdvancedSharpAdbClient.Receivers;
using Dragon.Controller.Controller.DeviceControl.HATX.Core.Model;
using Dragon.Controller.TaskDeviceManager.Core;
using Dragon.Controller.TaskDeviceManager.Infrastructure;
using Dragon.Controller.TaskDeviceManager.Model.App;
using Dragon.Controller.TaskDeviceManager.Model.Mouse;
using Dragon.Database.Models;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Dragon.Controller.TaskDeviceManager.Runners
{
    public static class AppRunner
    {
        public static async Task<NodeResult> RunAsync(DLoopContext ctx, AppArgs args)
        {
            if (ctx.Token.IsCancellationRequested) return NodeResult.Stop();

            return args.Action switch
            {
                AppAction.Start => await StartAsync(ctx, args),
                AppAction.Stop => await StopAsync(ctx, args),
                AppAction.Clear => await ClearAsync(ctx, args),
                AppAction.Uninstall => await UninstallAsync(ctx, args),
                AppAction.Install => await InstallAsync(ctx, args),
                _ => NodeResult.Fail("Unsupported AppAction")
            };
        }

        private static async Task<NodeResult> StartAsync(DLoopContext ctx, AppArgs a)
        {
            if (a.ControlMode == ControlMode.ATX)
            {
                if (ctx.Atx == null) return NodeResult.Fail("ATX null");
                await ctx.Atx.AppStartAsync(a.Package, a.Monkey, a.Stop, a.Wait, a.Activity, a.WaitTime, a.Front);
                Logger.Notify(ctx.LogKey, $"[ATX] Start {a.Package}", Logger.Icon.None);
                return NodeResult.Ok();
            }
            else if (a.ControlMode == ControlMode.Scrcpy || a.ControlMode == ControlMode.ADB || a.ControlMode == ControlMode.ADBEvent || a.ControlMode == ControlMode.HDI)
            {
                // gọi đúng hàm AppStart ADB đã chuẩn hóa
                return await AppStart(ctx, a.Package, a.Monkey, a.Stop, a.Wait, a.Activity, a.WaitTime, a.Front, ctx.Token);
            }
            else if (a.ControlMode == ControlMode.OTG)
            {
                return NodeResult.Fail("ACC mode chưa triển khai");
            }
            else if (a.ControlMode == ControlMode.ACC)
            {
                return NodeResult.Fail("ACC mode chưa triển khai");
            }
            return NodeResult.Fail("Unknown ControlMode");
        }

        private static async Task<NodeResult> StopAsync(DLoopContext ctx, AppArgs a)
        {
            if (a.ControlMode == ControlMode.ATX)
            {
                if (ctx.Atx == null) return NodeResult.Fail("ATX null");
                await ctx.Atx.AppStopAsync(a.Package);
                return NodeResult.Ok();
            }
            else if (a.ControlMode == ControlMode.Scrcpy || a.ControlMode == ControlMode.ADB || a.ControlMode == ControlMode.ADBEvent || a.ControlMode == ControlMode.HDI)
            {
                return await AppStop(ctx, a.Package, ctx.Token);
            }
            else if (a.ControlMode == ControlMode.ACC)
            {
                return NodeResult.Fail("ACC mode chưa triển khai");
            }
            return NodeResult.Fail("Unknown ControlMode");
        }

        private static async Task<NodeResult> ClearAsync(DLoopContext ctx, AppArgs a)
        {
            if (a.ControlMode == ControlMode.ATX)
            {
                if (ctx.Atx == null) return NodeResult.Fail("ATX null");
                await ctx.Atx.AppClearAsync(a.Package);
                return NodeResult.Ok();
            }
            else if (a.ControlMode == ControlMode.Scrcpy || a.ControlMode == ControlMode.ADB || a.ControlMode == ControlMode.ADBEvent || a.ControlMode == ControlMode.HDI)
            {
                return await AppClear(ctx, a.Package, ctx.Token);
            }
            else if (a.ControlMode == ControlMode.ACC)
            {
                return NodeResult.Fail("ACC mode chưa triển khai");
            }
            return NodeResult.Fail("Unknown ControlMode");
        }

        private static async Task<NodeResult> UninstallAsync(DLoopContext ctx, AppArgs a)
        {
            if (a.ControlMode == ControlMode.ATX)
            {
                if (ctx.Atx == null) return NodeResult.Fail("ATX null");
                await ctx.Atx.AppUninstallAsync(a.Package);
                return NodeResult.Ok();
            }
            else if (a.ControlMode == ControlMode.Scrcpy || a.ControlMode == ControlMode.ADB || a.ControlMode == ControlMode.ADBEvent || a.ControlMode == ControlMode.HDI)
            {
                return await AppUninstall(ctx, a.Package, ctx.Token);
            }
            else if (a.ControlMode == ControlMode.ACC)
            {
                return NodeResult.Fail("ACC mode chưa triển khai");
            }
            return NodeResult.Fail("Unknown ControlMode");
        }

        private static async Task<NodeResult> InstallAsync(DLoopContext ctx, AppArgs a)
        {

            if (a.ControlMode == ControlMode.ATX || a.ControlMode == ControlMode.Scrcpy || a.ControlMode == ControlMode.ADB || a.ControlMode == ControlMode.ADBEvent || a.ControlMode == ControlMode.HDI)
            {
                return await AppInstall(ctx, a.PathAPk, ctx.Token);
            }
            else if (a.ControlMode == ControlMode.ACC)
            {
                return NodeResult.Fail("ACC mode chưa triển khai");
            }
            return NodeResult.Fail("Unknown ControlMode");
        }

        // ---- ADB helpers (NodeResult) ----
        public static async Task<NodeResult> AppStop(DLoopContext ctx, string pkg, CancellationToken token)
        {
            await ctx.AdbClient.ExecuteRemoteCommandAsync($"am force-stop {pkg}", ctx.DeviceData, new ConsoleOutputReceiver(), token);
            Logger.Notify(ctx.LogKey, $"[ADB] Stop {pkg}", Logger.Icon.None);
            return NodeResult.Ok();
        }

        public static async Task<NodeResult> AppClear(DLoopContext ctx, string pkg, CancellationToken token)
        {
            await ctx.AdbClient.ExecuteRemoteCommandAsync($"pm clear {pkg}", ctx.DeviceData, new ConsoleOutputReceiver(), token);
            Logger.Notify(ctx.LogKey, $"[ADB] Clear {pkg}", Logger.Icon.None);
            return NodeResult.Ok();
        }

        public static async Task<NodeResult> AppUninstall(DLoopContext ctx, string pkg, CancellationToken token)
        {
            await ctx.AdbClient.ExecuteRemoteCommandAsync($"pm uninstall {pkg}", ctx.DeviceData, new ConsoleOutputReceiver(), token);
            Logger.Notify(ctx.LogKey, $"[ADB] Uninstall {pkg}", Logger.Icon.None);
            return NodeResult.Ok();
        }

        public static async Task<NodeResult> AppInstall(DLoopContext ctx, string apk, CancellationToken token)
        {
            var r = new ConsoleOutputReceiver();
            await ctx.AdbClient.ExecuteRemoteCommandAsync($"pm install -r -t \"{apk}\"", ctx.DeviceData, r, token);
            var ok = r.ToString().Contains("Success");
            Logger.Notify(ctx.LogKey, $"[ADB] Install {apk}: {r}", Logger.Icon.Information);
            return ok ? NodeResult.Ok() : NodeResult.Fail("Install failed");
        }

        #region For ADB

        public static async Task<List<AndroidProcessItem>> GetProcessList(DLoopContext ctx, CancellationToken token)
        {
            var list = new List<AndroidProcessItem>();
            var receiver = new ConsoleOutputReceiver();
            await ctx.AdbClient.ExecuteRemoteCommandAsync("ps -A", ctx.DeviceData, receiver, token);
            string result = receiver.ToString();
            if (string.IsNullOrWhiteSpace(result)) return list;

            var pids = new HashSet<string>();
            string[] sp = result.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in sp)
            {
                string[] info = s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (info.Length < 3) continue;
                if (info[0] == "USER") continue;
                if (!pids.Add(info[1])) continue;
                list.Add(new AndroidProcessItem()
                {
                    User = info[0],
                    Pid = int.TryParse(info[1], out int pid) ? pid : -1,
                    Name = info[info.Length - 1]
                });
            }
            return list;
        }

        public static async Task<List<string>> AppRunningList(DLoopContext ctx)
        {
            List<string> apps = await AppList(ctx);
            List<AndroidProcessItem> process = await GetProcessList(ctx, CancellationToken.None);
            List<string> list = new();
            foreach (string app in apps)
            {
                foreach (var item in process)
                {
                    if (item.Name.Contains(app))
                    {
                        list.Add(app);
                        break;
                    }
                }
            }
            return list;
        }

        public static async Task<List<string>> AppList(DLoopContext ctx)
        {
            var output = new ConsoleOutputReceiver();
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            try
            {
                await ctx.AdbClient.ExecuteRemoteCommandAsync("pm list package -3", ctx.DeviceData, output, cts.Token);
                var packages = output.ToString()
                  .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                  .Select(line => line.Replace("package:", "").Trim())
                  .ToList();
                return packages;
            }
            catch (OperationCanceledException ex)
            {
                Logger.Notify(ctx.LogKey, $"Client or ExecuteRemote Error {ex.Message}", Logger.Icon.Error);
                return new List<string>();
            }
        }

        private static readonly Regex FocusRegex = new(
        @"mCurrentFocus=Window\{.*?\s+(?<package>[^\s]+)/(?<activity>[^\s]+)\}",
        RegexOptions.CultureInvariant);

        private static readonly Regex ResumedRegex = new(
        @"mResumedActivity: ActivityRecord\{.*?\s+(?<package>[^\s]+)/(?<activity>[^\s]+)\s.*?\}",
        RegexOptions.CultureInvariant);

        private static readonly Regex TopActivityRegex = new(
        @"ACTIVITY (?<package>[^\s]+)/(?<activity>[^/\s]+) \w+ pid=(?<pid>\d+)",
        RegexOptions.CultureInvariant);

        public static async Task<AppCurrentInfo?> AppCurrent(DLoopContext ctx, CancellationToken token)
        {
            var receiver = new ConsoleOutputReceiver();
            await ctx.AdbClient.ExecuteRemoteCommandAsync("dumpsys window windows", ctx.DeviceData, receiver, token);
            var result = receiver.ToString();

            var match = FocusRegex.Match(result);
            if (match.Success)
            {
                return new AppCurrentInfo
                {
                    Package = match.Groups["package"].Value,
                    Activity = match.Groups["activity"].Value
                };
            }

            // fallback 1: mResumedActivity
            receiver = new ConsoleOutputReceiver();
            await ctx.AdbClient.ExecuteRemoteCommandAsync("dumpsys activity activities", ctx.DeviceData, receiver, token);
            result = receiver.ToString();

            match = ResumedRegex.Match(result);
            if (!match.Success) return null;

            var info = new AppCurrentInfo
            {
                Package = match.Groups["package"].Value
            };

            // fallback 2: lấy pid + activity chính xác
            receiver = new ConsoleOutputReceiver();
            await ctx.AdbClient.ExecuteRemoteCommandAsync("dumpsys activity top", ctx.DeviceData, receiver, token);
            result = receiver.ToString();

            foreach (Match m in TopActivityRegex.Matches(result))
            {
                if (m.Groups["package"].Value == info.Package)
                {
                    info.Activity = m.Groups["activity"].Value;
                    info.Pid = int.TryParse(m.Groups["pid"].Value, out var pid) ? pid : 0;
                    return info;
                }
            }

            // nếu không tìm thấy ở top, trả về package thôi
            info.Activity = match.Groups["activity"].Value;
            return info;
        }

        public static async Task<bool> AppWait(DLoopContext ctx, string package, int timeout = 20000, string? activity = null, bool front = false)
        {
            long deadline = DateTimeOffset.Now.ToUnixTimeMilliseconds() + timeout;
            while (DateTimeOffset.Now.ToUnixTimeMilliseconds() < deadline)
            {
                try
                {
                    if (ctx.Token.IsCancellationRequested) return false;
                    if (front)
                    {
                        var info = await AppCurrent(ctx, ctx.Token);
                        if (info != null && info.Package == package)
                        {
                            if (!string.IsNullOrWhiteSpace(activity))
                            {
                                if (activity == info.Activity) return true;
                            }
                            else return true;
                        }
                    }
                    else
                    {
                        var list = await AppRunningList(ctx);
                        if (list.Contains(package)) return true;
                    }
                }
                catch { }
                await Logger.DelayAsync(1000, ctx.Token, ctx.Session.Phone.DeviceID);
            }
            return false;
        }


        public static async Task<NodeResult> AppStart(DLoopContext ctx, string package, bool monkey = false, bool stop = false, bool wait = false, string? activity = null, int timeout = 20000, bool front = false, CancellationToken token = default)
        {
            if (stop) await AppStop(ctx, package, token);

            if (monkey)
            {
                await ctx.AdbClient.ExecuteRemoteCommandAsync($"monkey -p {package} -c android.intent.category.LAUNCHER 1", ctx.DeviceData, new ConsoleOutputReceiver(), token);
                if (wait) await AppWait(ctx, package, timeout, activity, front);
                Logger.Notify(ctx.LogKey, $"[ADB] Start {package} via monkey", Logger.Icon.None);
                return NodeResult.Ok();
            }

            if (string.IsNullOrWhiteSpace(activity))
            {
                Logger.Notify(ctx.LogKey, $"Không xác định được activity của {package}, sử dụng monkey", Logger.Icon.Warning);
                return NodeResult.Fail("Missing activity");
            }

            Debug.WriteLine($"Khởi động APP: {package}/{activity}");
            await ctx.AdbClient.ExecuteRemoteCommandAsync($"am start -a android.intent.action.MAIN -c android.intent.category.LAUNCHER -n {package}/{activity}", ctx.DeviceData, new ConsoleOutputReceiver(), token);

            if (wait)
            {
                Logger.Notify(ctx.LogKey, $"Wait {timeout}", Logger.Icon.None);
                await AppWait(ctx, package, timeout, activity, front);
            }

            Logger.Notify(ctx.LogKey, $"Success Start App {package}", Logger.Icon.None);
            return NodeResult.Ok();
        }

        #endregion

    }

}
