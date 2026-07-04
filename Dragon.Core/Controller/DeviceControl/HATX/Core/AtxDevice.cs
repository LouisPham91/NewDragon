using Dragon.Controller.Controller.DeviceControl.HATX.Core;
using Dragon.Controller.Controller.DeviceControl.HATX.Core.Model;
using Dragon.Controller.TaskDeviceManager.Core;
using Dragon.Controller.TaskDeviceManager.Infrastructure;
using Dragon.Controller.TaskDeviceManager.Infrastructure.Services;
using Dragon.Controller.TaskDeviceManager.Model.DatabaseColumn;
using Dragon.Controller.TaskDeviceManager.Model.Mouse;
using Dragon.Controller.TaskDeviceManager.Model.Vision;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml;

namespace Dragon.Controller.DeviceControl.HATX
{
    public sealed partial class AtxDevice : IAsyncDisposable
    {
        public AtxInfo? _AtxInfo { get; private set; }
        private readonly HttpClient _http;
        private ClientWebSocket? _wsCap;
        private ClientWebSocket? _wsTouch;
        private readonly CancellationTokenSource _cts = new();
        public bool IsMiniTouchAvailable { get; private set; }
        public string Host { get; }
        public int Port { get; }
        public bool IsUsbMode { get; private set; }
        public string DeviceId { get; private set; } = string.Empty;
        public int Rotation { get; private set; }

        // Constructor dùng cho USB (từ AtxInit)
        public AtxDevice(string deviceId, string atxUrl, int forwardPort)
        {
            DeviceId = deviceId;
            IsUsbMode = forwardPort != 0;
            var uri = new Uri(atxUrl);
            Host = uri.Host;
            Port = uri.Port;
            _http = new HttpClient { BaseAddress = uri };
        }

        // Factory method khởi tạo thông minh (tự động WiFi/USB)
        public static async Task<AtxDevice> CreateAsync(string deviceId, bool isMiniCap = true, CancellationToken ct = default)
        {
            await AtxInit.EnsureInitializedAsync(deviceId, isMiniCap, ct);
            var (atxUrl, forwardPort, isWiFi) = await AtxInit.GetAtxUrlAsync(deviceId, ct);
            var device = new AtxDevice(deviceId, atxUrl, forwardPort);
            await device.ConnectAsync();
            return device;
        }

        public async Task ConnectAsync()
        {
            _AtxInfo = await GetInfoAsync();
            _wsCap?.Dispose();
            _wsTouch?.Dispose();

            // Minicap
            _wsCap = new ClientWebSocket();
            await _wsCap.ConnectAsync(new Uri($"ws://{Host}:{Port}/minicap"), CancellationToken.None);
            _ = ReceiveCapLoop(_cts.Token);

            // Minitouch
            try
            {
                _wsTouch = new ClientWebSocket();
                await _wsTouch.ConnectAsync(new Uri($"ws://{Host}:{Port}/minitouch"), CancellationToken.None);
                var reset = Encoding.UTF8.GetBytes("{\"operation\":\"r\"}");
                await _wsTouch.SendAsync(reset, WebSocketMessageType.Text, true, CancellationToken.None);
                await Task.Delay(150);
                IsMiniTouchAvailable = _wsTouch.State == WebSocketState.Open;
            }
            catch
            {
                IsMiniTouchAvailable = false;
                _wsTouch?.Dispose();
                _wsTouch = null;
            }
            Debug.WriteLine($"[ATX {Host}:{Port}] minitouch = {(IsMiniTouchAvailable ? "OK" : "CLOSED → dùng shell")}");
        }

        public async Task DisconnectAsync()
        {
            _cts.Cancel();
            if (_wsCap != null && _wsCap.State == WebSocketState.Open)
                await _wsCap.CloseAsync(WebSocketCloseStatus.NormalClosure, "disconnect", CancellationToken.None);
            _wsCap?.Dispose();
            if (_wsTouch != null && _wsTouch.State == WebSocketState.Open)
                await _wsTouch.CloseAsync(WebSocketCloseStatus.NormalClosure, "disconnect", CancellationToken.None);
            _wsTouch?.Dispose();
            _wsCap = null;
            _wsTouch = null;
        }

        public async ValueTask DisposeAsync()
        {
            await DisconnectAsync();
            _http.Dispose();
            _cts.Dispose();
            if (IsUsbMode && !string.IsNullOrEmpty(DeviceId))
            {
                await AtxInit.StopAsync(DeviceId, CancellationToken.None);
            }
        }

        public event Action<Bitmap>? FrameReceived;
        private async Task ReceiveCapLoop(CancellationToken ct)
        {
            var buf = new byte[256 * 1024];
            try
            {
                while (_wsCap != null && _wsCap.State == WebSocketState.Open && !ct.IsCancellationRequested)
                {
                    var res = await _wsCap.ReceiveAsync(buf, ct);
                    if (res.MessageType == WebSocketMessageType.Binary)
                    {
                        using var ms = new MemoryStream(buf, 0, res.Count);
                        var bmp = new Bitmap(ms);
                        FrameReceived?.Invoke(bmp);
                    }
                    else if (res.MessageType == WebSocketMessageType.Text)
                    {
                        var txt = Encoding.UTF8.GetString(buf, 0, res.Count);
                        if (txt.StartsWith("rotation "))
                            Rotation = int.Parse(txt[9..]);
                    }
                }
            }
            catch { }
        }

        public async Task<AtxInfo?> GetInfoAsync()
        {
            var json = await _http.GetStringAsync("info");
            return JsonSerializer.Deserialize(json, AtxJsonContext.Default.AtxInfo);
        }

        public async Task<string> ShellAsync(string cmd)
        {
            var resp = await _http.PostAsync("shell", new FormUrlEncodedContent([new("command", cmd)]));
            return await resp.Content.ReadAsStringAsync();
        }

        public async Task<List<NodeObj>> DumpHierarchyAsync()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            var json = await _http.GetStringAsync("dump/hierarchy");
            var resp = JsonSerializer.Deserialize(json, AtxJsonContext.Default.HierarchyResponse);
            var doc = new XmlDocument();
            if (string.IsNullOrEmpty(resp?.Result)) return [];
            doc.LoadXml(resp.Result);
            var list = new List<NodeObj>();
            void Walk(XmlNode n)
            {
                var obj = NodeObj.Create(n);
                if (obj != null) list.Add(obj);
                foreach (XmlNode child in n.ChildNodes) Walk(child);
            }
            Walk(doc.DocumentElement!);
            stopwatch.Stop();
            Debug.WriteLine($"[ATX {Host}:{Port}] DumpHierarchyAsync: {list.Count} nodes, took {stopwatch.ElapsedMilliseconds}ms");
            return list;
        }

        // ========== TOUCH METHODS ==========
        private async Task SendTouchRawAsync(string op, float xP, float yP, float pressure = 0.5f, int index = 0)
        {
            if (!IsMiniTouchAvailable || _wsTouch == null)
                throw new InvalidOperationException("minitouch unavailable");
            var cmd = new TouchCmd(op, index, pressure, xP, yP);
            var json = JsonSerializer.Serialize(cmd, AtxJsonContext.Default.TouchCmd);
            await _wsTouch.SendAsync(Encoding.UTF8.GetBytes(json), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public async Task ClickAsync(float xPercent, float yPercent)
        {
            if (IsMiniTouchAvailable)
            {
                await SendTouchRawAsync("d", xPercent, yPercent);
                await Task.Delay(30);
                await SendTouchRawAsync("u", xPercent, yPercent);
            }
            else if (_AtxInfo?.Display != null)
            {
                int x = (int)(xPercent * _AtxInfo.Display.Width);
                int y = (int)(yPercent * _AtxInfo.Display.Height);
                await ShellAsync($"input tap {x} {y}");
            }
        }

        public async Task LongPressAsync(float xPercent, float yPercent, int durationMs = 1000)
        {
            if (IsMiniTouchAvailable)
            {
                await SendTouchRawAsync("d", xPercent, yPercent);
                await Task.Delay(durationMs);
                await SendTouchRawAsync("u", xPercent, yPercent);
            }
            else if (_AtxInfo?.Display != null)
            {
                int x = (int)(xPercent * _AtxInfo.Display.Width);
                int y = (int)(yPercent * _AtxInfo.Display.Height);
                await ShellAsync($"input swipe {x} {y} {x} {y} {durationMs}");
            }
        }

        public async Task SwipeAsync(float startX, float startY, float endX, float endY, int durationMs = 300, int steps = 10)
        {
            if (IsMiniTouchAvailable)
            {
                await SendTouchRawAsync("d", startX, startY);
                var stepDelay = durationMs / steps;
                for (int i = 1; i <= steps; i++)
                {
                    float progress = i / (float)steps;
                    float currentX = startX + (endX - startX) * progress;
                    float currentY = startY + (endY - startY) * progress;
                    await SendTouchRawAsync("m", currentX, currentY);
                    await Task.Delay(stepDelay);
                }
                await SendTouchRawAsync("u", endX, endY);
            }
            else if (_AtxInfo?.Display != null)
            {
                int x1 = (int)(startX * _AtxInfo.Display.Width);
                int y1 = (int)(startY * _AtxInfo.Display.Height);
                int x2 = (int)(endX * _AtxInfo.Display.Width);
                int y2 = (int)(endY * _AtxInfo.Display.Height);
                await ShellAsync($"input swipe {x1} {y1} {x2} {y2} {durationMs}");
            }
        }

        // Drag and drop với sendevent (chính xác)
        public async Task<bool> DragDropAbsoluteAsync(int startX, int startY, int endX, int endY, int durationMs = 2000, int steps = 15)
        {
            if (_AtxInfo?.Display == null) return false;
            var ev = await GetEventDeviceAsync();
            if (string.IsNullOrEmpty(ev))
            {
                await ShellAsync($"input swipe {startX} {startY} {endX} {endY} {durationMs}");
                return true;
            }

            var cmds = new List<string>();
            cmds.AddRange(new[]
            {
                $"sendevent {ev} 3 57 123", $"sendevent {ev} 3 53 {startX}", $"sendevent {ev} 3 54 {startY}",
                $"sendevent {ev} 3 58 50", $"sendevent {ev} 1 330 1", $"sendevent {ev} 0 0 0"
            });
            int stepDelay = durationMs / steps;
            for (int i = 1; i <= steps; i++)
            {
                float progress = i / (float)steps;
                int curX = startX + (int)((endX - startX) * progress);
                int curY = startY + (int)((endY - startY) * progress);
                cmds.Add($"sendevent {ev} 3 53 {curX}");
                cmds.Add($"sendevent {ev} 3 54 {curY}");
                cmds.Add($"sendevent {ev} 0 0 0");
                cmds.Add($"sleep {(stepDelay / 1000.0).ToString("0.000", CultureInfo.InvariantCulture)}");
            }
            cmds.AddRange(new[]
            {
                $"sendevent {ev} 3 57 -1", $"sendevent {ev} 1 330 0", $"sendevent {ev} 0 0 0"
            });
            for (int i = 0; i < cmds.Count; i += 20)
            {
                var batch = string.Join(" && ", cmds.Skip(i).Take(20));
                await ShellAsync(batch);
            }
            return true;
        }

        private string? _cachedEventDevice;
        private async Task<string?> GetEventDeviceAsync()
        {
            if (!string.IsNullOrEmpty(_cachedEventDevice)) return _cachedEventDevice;
            try
            {
                var result = await ShellAsync("ls -la /dev/input/ | grep -E 'event[0-9]+' | awk '{print $9}'");
                var eventDevices = result.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                foreach (var ev in eventDevices)
                {
                    var info = await ShellAsync($"getevent -i /dev/input/{ev} | grep -E 'ABS_MT_POSITION|ABS_X'");
                    if (!string.IsNullOrEmpty(info))
                    {
                        _cachedEventDevice = $"/dev/input/{ev}";
                        return _cachedEventDevice;
                    }
                }
            }
            catch { }
            return null;
        }

        // ========== APP CONTROL ==========
        private static readonly Regex FocusRegex = MyFocusRegex();
        private static readonly Regex ActivityTopRegex = MyActivityTopRegex();
        [GeneratedRegex(@"mCurrentFocus=Window\{.*?\s+(?<package>[^\s]+)/(?<activity>[^\s]+)\}", RegexOptions.Singleline)]
        private static partial Regex MyFocusRegex();
        [GeneratedRegex(@"ACTIVITY (?<package>[^\s]+)/(?<activity>[^/\s]+) \w+ pid=(?<pid>\d+)", RegexOptions.Singleline)]
        private static partial Regex MyActivityTopRegex();

        public async Task AppStartAsync(string package, bool monkey = false, bool stop = false,
            bool wait = false, string? activity = null, int timeout = 20000, bool front = false, CancellationToken token = default)
        {
            if (token.IsCancellationRequested) return;
            if (stop) await AppStopAsync(package);
            if (monkey)
            {
                await ShellAsync($"monkey -p {package} -c android.intent.category.LAUNCHER 1");
                if (wait) await AppWaitAsync(package, timeout, activity, front, token);
                return;
            }
            if (string.IsNullOrWhiteSpace(activity))
                activity = await GetMainActivityAsync(package);
            string cmd = string.IsNullOrEmpty(activity)
                ? $"monkey -p {package} -c android.intent.category.LAUNCHER 1"
                : $"am start -a android.intent.action.MAIN -c android.intent.category.LAUNCHER -n {package}/{activity}";
            await ShellAsync(cmd);
            if (wait) await AppWaitAsync(package, timeout, activity, front, token);
        }

        public async Task AppStopAsync(string package) => await ShellAsync($"am force-stop {package}");
        public async Task AppClearAsync(string package) => await ShellAsync($"pm clear {package}");
        public async Task AppUninstallAsync(string package) => await ShellAsync($"pm uninstall {package}");

        public async Task<bool> AppWaitAsync(string package, int timeout = 20000, string? activity = null, bool front = false, CancellationToken token = default)
        {
            long deadline = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + timeout;
            while (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() < deadline)
            {
                if (token.IsCancellationRequested) return false;
                if (front)
                {
                    var cur = await AppCurrentAsync();
                    if (cur?.Package == package)
                    {
                        if (!string.IsNullOrWhiteSpace(activity)) return activity == cur.Activity;
                        return true;
                    }
                }
                else
                {
                    var result = await ShellAsync($"pidof {package}");
                    if (!string.IsNullOrWhiteSpace(result) && result.Trim() != "") return true;
                }
                await Task.Delay(1000, token);
            }
            return false;
        }

        public async Task<AppCurrentInfo?> AppCurrentAsync()
        {
            try
            {
                var result = await ShellAsync("dumpsys window windows");
                var match = FocusRegex.Match(result);
                if (match.Success)
                    return new AppCurrentInfo { Package = match.Groups["package"].Value, Activity = match.Groups["activity"].Value };
                result = await ShellAsync("dumpsys activity top");
                var matches = ActivityTopRegex.Matches(result);
                foreach (Match m in matches)
                    return new AppCurrentInfo
                    {
                        Package = m.Groups["package"].Value,
                        Activity = m.Groups["activity"].Value,
                        Pid = int.TryParse(m.Groups["pid"].Value, out int pid) ? pid : -1
                    };
                var hierarchy = await DumpHierarchyAsync();
                var root = hierarchy.FirstOrDefault();
                if (root?.PackageName != null)
                    return new AppCurrentInfo { Package = root.PackageName, Activity = root.ClassName };
            }
            catch (Exception ex) { Debug.WriteLine($"AppCurrent error: {ex.Message}"); }
            return null;
        }

        private async Task<string?> GetMainActivityAsync(string package)
        {
            try
            {
                var result = await ShellAsync($"cmd package resolve-activity --brief {package} | tail -n 1");
                var activity = result.Trim();
                if (!string.IsNullOrEmpty(activity) && activity.Contains('/'))
                {
                    var parts = activity.Split('/');
                    if (parts.Length > 1)
                    {
                        var act = parts[1];
                        if (act.StartsWith('.')) return package + act;
                        return act;
                    }
                }
                result = await ShellAsync($"pm dump {package} | grep -A 1 'LAUNCHER' | grep -oE '([a-zA-Z0-9_\\.]+)/\\.?([a-zA-Z0-9_\\.]+)' | head -1");
                if (!string.IsNullOrEmpty(result))
                {
                    var parts = result.Trim().Split('/');
                    if (parts.Length > 1)
                    {
                        var act = parts[1];
                        if (act.StartsWith('.')) return package + act;
                        return act;
                    }
                }
            }
            catch (Exception ex) { Debug.WriteLine($"GetMainActivity error: {ex.Message}"); }
            return null;
        }

        public async Task PressBackAsync() => await ShellAsync("input keyevent KEYCODE_BACK");
        public async Task PressHomeAsync() => await ShellAsync("input keyevent KEYCODE_HOME");
        public async Task PressRecentAsync() => await ShellAsync("input keyevent KEYCODE_APP_SWITCH");
        public async Task<bool> IsAppInstalledAsync(string package) => (await ShellAsync($"pm list packages {package}")).Contains(package);
        public async Task OpenUriAsync(string uri) => await ShellAsync($"am start -a android.intent.action.VIEW -d \"{uri}\"");

        // ========== NODE TEXT EXTRACTION ==========
        private static readonly ConcurrentDictionary<string, Regex> _regexCache = new(StringComparer.OrdinalIgnoreCase);
        public async Task<string> FindNodeSaveTextsAsync(SetColumnDataArgs readTextData)
        {
            _regexCache.Clear();
            if (readTextData == null || string.IsNullOrWhiteSpace(readTextData.NodeRegexParttern)) return string.Empty;
            var nodes = await DumpHierarchyAsync();
            if (nodes == null || nodes.Count == 0) return string.Empty;
            var regex = _regexCache.GetOrAdd(readTextData.NodeRegexParttern,
                p => new Regex(p, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant));
            foreach (var node in nodes)
            {
                bool matchAll = true;
                if (readTextData.IsNodeResourceName && !string.IsNullOrWhiteSpace(readTextData.NodeResourceName))
                    matchAll &= node.ResourceName?.Equals(readTextData.NodeResourceName, StringComparison.OrdinalIgnoreCase) == true;
                if (readTextData.IsNodeClassName && !string.IsNullOrWhiteSpace(readTextData.NodeClassName))
                    matchAll &= node.ClassName?.Equals(readTextData.NodeClassName, StringComparison.OrdinalIgnoreCase) == true;
                if (readTextData.IsNodePackageName && !string.IsNullOrWhiteSpace(readTextData.NodePackageName))
                    matchAll &= node.PackageName?.Equals(readTextData.NodePackageName, StringComparison.OrdinalIgnoreCase) == true;
                if (!matchAll) continue;
                foreach (var source in new[] { node.Text ?? "", node.ContentDescription ?? "" })
                {
                    if (string.IsNullOrEmpty(source)) continue;
                    var match = regex.Match(source);
                    if (match.Success)
                        return readTextData.IsNodeGetFullText ? source : (match.Groups.Count > 1 ? match.Groups[1].Value : match.Value);
                }
            }
            return string.Empty;
        }

        // ========== FILE OPERATIONS ==========
        public async Task<bool> PullAsync(string sourcePath, string destinationPath, string? logKey = null)
        {
            try
            {
                var content = await ShellAsync($"cat \"{sourcePath}\"");
                if (string.IsNullOrEmpty(content)) return false;
                await File.WriteAllBytesAsync(destinationPath, Encoding.UTF8.GetBytes(content));
                return true;
            }
            catch { return false; }
        }

        public async Task<bool> PushAsync(string sourcePath, string destinationPath, int mode = 493, string? logKey = null)
        {
            try
            {
                if (!File.Exists(sourcePath)) return false;
                var bytes = await File.ReadAllBytesAsync(sourcePath);
                var base64 = Convert.ToBase64String(bytes);
                await ShellAsync($"echo '{base64}' | base64 -d > {destinationPath}");
                if (mode > 0) await ShellAsync($"chmod {Convert.ToString(mode, 8)} {destinationPath}");
                var check = await ShellAsync($"ls -la {destinationPath}");
                return !check.Contains("No such file");
            }
            catch { return false; }
        }

        public async Task<bool> CopyAsync(string sourcePath, string destinationPath, string? logKey = null)
        {
            try
            {
                await ShellAsync($"cp \"{sourcePath}\" \"{destinationPath}\"");
                var check = await ShellAsync($"ls -la {destinationPath}");
                return !check.Contains("No such file");
            }
            catch { return false; }
        }

        public async Task<bool> FileExistsAsync(string path) => (await ShellAsync($"test -f \"{path}\" && echo 'exists' || echo 'not exists'")).Trim() == "exists";
        public async Task<bool> DeleteFileAsync(string path) { await ShellAsync($"rm -f \"{path}\""); return !await FileExistsAsync(path); }
        public async Task<string?> ReadTextFileAsync(string path) { try { return await ShellAsync($"cat \"{path}\""); } catch { return null; } }
        public async Task<bool> WriteTextFileAsync(string path, string content) { var escaped = content.Replace("'", "'\\''"); await ShellAsync($"echo '{escaped}' > \"{path}\""); return await FileExistsAsync(path); }

        // ========== BROADCAST ==========
        public async Task<bool> ArmBroadCastAsync(string command, string? logKey = null)
        {
            try
            {
                var result = await ShellAsync($"am broadcast -a {command}");
                return result.Contains("Broadcast completed: result=0") || (!result.Contains("Error") && !result.Contains("Permission denied"));
            }
            catch { return false; }
        }

        // ========== DLoopContext METHODS (tương thích code cũ) ==========
        public async Task<NodeResult> ClickAsync(DLoopContext ctx, ClickArg click)
        {
            if (ctx.Token.IsCancellationRequested) return NodeResult.Stop();
            if (_AtxInfo?.Display == null) return NodeResult.Fail("Display info not available");
            float x = click.x / 100f, y = click.y / 100f;
            if (IsMiniTouchAvailable)
            {
                await SendTouchRawAsync("d", x, y);
                await Task.Delay(30);
                await SendTouchRawAsync("u", x, y);
            }
            else
            {
                int absX = (int)(x * _AtxInfo.Display.Width);
                int absY = (int)(y * _AtxInfo.Display.Height);
                await ShellAsync($"input tap {absX} {absY}");
            }
            if (click.ClickMode == ClickMode.DoubleClick)
            {
                if (await Logger.DelayAsync(120, ctx.Token, ctx.LogKey) == ExecutionStatus.Stop) return NodeResult.Stop();
                if (IsMiniTouchAvailable)
                {
                    await SendTouchRawAsync("d", x, y);
                    await Task.Delay(30);
                    await SendTouchRawAsync("u", x, y);
                }
                else
                {
                    int absX = (int)(x * _AtxInfo.Display.Width);
                    int absY = (int)(y * _AtxInfo.Display.Height);
                    await ShellAsync($"input tap {absX} {absY}");
                }
            }
            Logger.Notify(ctx.LogKey, $"ATX {click.ClickMode} ({click.x}%,{click.y}%)", Logger.Icon.Information);
            return NodeResult.Ok();
        }

        public async Task<NodeResult> LongPressAsync(DLoopContext ctx, LongPressArg press)
        {
            if (ctx.Token.IsCancellationRequested) return NodeResult.Stop();
            if (_AtxInfo?.Display == null) return NodeResult.Fail("Display info not available");
            float x = press.x / 100f, y = press.y / 100f;
            if (IsMiniTouchAvailable)
            {
                await SendTouchRawAsync("d", x, y);
                if (await Logger.DelayAsync(press.Duration, ctx.Token, ctx.LogKey) == ExecutionStatus.Stop)
                {
                    await SendTouchRawAsync("u", x, y);
                    return NodeResult.Stop();
                }
                await SendTouchRawAsync("u", x, y);
            }
            else
            {
                int absX = (int)(x * _AtxInfo.Display.Width);
                int absY = (int)(y * _AtxInfo.Display.Height);
                await ShellAsync($"input swipe {absX} {absY} {absX} {absY} {press.Duration}");
            }
            Logger.Notify(ctx.LogKey, $"ATX LongPress ({press.x}%,{press.y}%) {press.Duration}ms", Logger.Icon.Information);
            return NodeResult.Ok();
        }

        public async Task<NodeResult> SwipeAsync(DLoopContext ctx, SwipeArg swipe)
        {
            if (ctx.Token.IsCancellationRequested) return NodeResult.Stop();
            if (_AtxInfo?.Display == null) return NodeResult.Fail("Display info not available");
            int loop = swipe.loopTime;
            if (swipe.SwipeMode == SwipeMode.Random && swipe.randMax >= swipe.randMin)
                loop = Random.Shared.Next(swipe.randMin, swipe.randMax + 1);
            for (int i = 0; i < loop; i++)
            {
                if (ctx.Token.IsCancellationRequested) return NodeResult.Stop();
                int x1, y1, x2, y2;
                var r = Random.Shared;
                int left = (int)(swipe.Left / 100f * _AtxInfo.Display.Width);
                int right = (int)(swipe.Right / 100f * _AtxInfo.Display.Width);
                int top = (int)(swipe.Top / 100f * _AtxInfo.Display.Height);
                int bottom = (int)(swipe.Bottom / 100f * _AtxInfo.Display.Height);
                switch (swipe.Direction)
                {
                    case Direction.Down: x1 = x2 = r.Next(left, right + 1); y1 = bottom; y2 = top; break;
                    case Direction.Up: x1 = x2 = r.Next(left, right + 1); y1 = top; y2 = bottom; break;
                    case Direction.Right: y1 = y2 = r.Next(top, bottom + 1); x1 = right; x2 = left; break;
                    case Direction.Left: y1 = y2 = r.Next(top, bottom + 1); x1 = left; x2 = right; break;
                    default: return NodeResult.Fail("Invalid direction");
                }
                float sx = x1 / (float)_AtxInfo.Display.Width, sy = y1 / (float)_AtxInfo.Display.Height;
                float ex = x2 / (float)_AtxInfo.Display.Width, ey = y2 / (float)_AtxInfo.Display.Height;
                if (IsMiniTouchAvailable)
                {
                    await SendTouchRawAsync("d", sx, sy);
                    int steps = Math.Max(5, swipe.duration / 20);
                    int stepDelay = swipe.duration / steps;
                    for (int s = 1; s <= steps; s++)
                    {
                        float p = s / (float)steps;
                        float cx = sx + (ex - sx) * p;
                        float cy = sy + (ey - sy) * p;
                        await SendTouchRawAsync("m", cx, cy);
                        await Task.Delay(stepDelay);
                    }
                    await SendTouchRawAsync("u", ex, ey);
                }
                else await ShellAsync($"input swipe {x1} {y1} {x2} {y2} {swipe.duration}");
                Logger.Notify(ctx.LogKey, $"ATX Swipe {swipe.Direction} {i + 1}/{loop}", Logger.Icon.Information);
                if (swipe.DelayPerLoop > 0 && await Logger.DelayAsync(swipe.DelayPerLoop, ctx.Token, ctx.LogKey) == ExecutionStatus.Stop)
                    return NodeResult.Stop();
            }
            return NodeResult.Ok();
        }

        public async Task<NodeResult> DragAndDropAsync(DLoopContext ctx, DragArg drag)
        {
            if (ctx.Token.IsCancellationRequested) return NodeResult.Stop();
            if (_AtxInfo?.Display == null || drag.Points == null || drag.Points.Count < 2)
                return NodeResult.Fail("Invalid drag data");
            try
            {
                var ev = await GetEventDeviceAsync();
                if (string.IsNullOrEmpty(ev))
                    return await DragAndDropFallbackAsync(ctx, drag);
                var cmds = new List<string>();
                var start = drag.Points[0];
                int xStart = (int)(_AtxInfo.Display.Width * start.X / 100.0);
                int yStart = (int)(_AtxInfo.Display.Height * start.Y / 100.0);
                cmds.AddRange(new[]
                {
                    $"sendevent {ev} 3 57 123", $"sendevent {ev} 3 53 {xStart}", $"sendevent {ev} 3 54 {yStart}",
                    $"sendevent {ev} 3 58 50", $"sendevent {ev} 1 330 1", $"sendevent {ev} 0 0 0", "sleep 0.5"
                });
                int segCount = drag.Points.Count - 1;
                int durPerSeg = drag.Duration / Math.Max(1, segCount);
                int steps = 15;
                for (int seg = 0; seg < segCount; seg++)
                {
                    var p1 = drag.Points[seg];
                    var p2 = drag.Points[seg + 1];
                    int x1 = (int)(_AtxInfo.Display.Width * p1.X / 100.0);
                    int y1 = (int)(_AtxInfo.Display.Height * p1.Y / 100.0);
                    int x2 = (int)(_AtxInfo.Display.Width * p2.X / 100.0);
                    int y2 = (int)(_AtxInfo.Display.Height * p2.Y / 100.0);
                    int sleepPerStep = durPerSeg / steps;
                    float sleepSec = sleepPerStep / 1000f;
                    for (int i = 1; i <= steps; i++)
                    {
                        if (ctx.Token.IsCancellationRequested) return NodeResult.Stop();
                        int xi = x1 + (x2 - x1) * i / steps;
                        int yi = y1 + (y2 - y1) * i / steps;
                        cmds.Add($"sendevent {ev} 3 53 {xi}");
                        cmds.Add($"sendevent {ev} 3 54 {yi}");
                        cmds.Add($"sendevent {ev} 0 0 0");
                        cmds.Add($"sleep {sleepSec.ToString("0.000", CultureInfo.InvariantCulture)}");
                    }
                }
                var end = drag.Points.Last();
                int xEnd = (int)(_AtxInfo.Display.Width * end.X / 100.0);
                int yEnd = (int)(_AtxInfo.Display.Height * end.Y / 100.0);
                cmds.Add($"sendevent {ev} 3 53 {xEnd}");
                cmds.Add($"sendevent {ev} 3 54 {yEnd}");
                cmds.Add($"sendevent {ev} 0 0 0");
                cmds.Add("sleep 0.5");
                cmds.AddRange(new[] { $"sendevent {ev} 3 57 -1", $"sendevent {ev} 1 330 0", $"sendevent {ev} 0 0 0" });
                for (int i = 0; i < cmds.Count; i += 20)
                {
                    if (ctx.Token.IsCancellationRequested) return NodeResult.Stop();
                    await ShellAsync(string.Join(" && ", cmds.Skip(i).Take(20)));
                }
                Logger.Notify(ctx.LogKey, $"ATX Drag {drag.Points.Count} điểm", Logger.Icon.Information);
                return NodeResult.Ok();
            }
            catch (Exception ex) { Logger.Notify(ctx.LogKey, $"Lỗi Drag: {ex.Message}", Logger.Icon.Error); return NodeResult.Fail(ex.Message); }
        }

        private async Task<NodeResult> DragAndDropFallbackAsync(DLoopContext ctx, DragArg drag)
        {
            try
            {
                var sw = Stopwatch.StartNew();
                int segCount = drag.Points.Count - 1;
                int durPerSeg = drag.Duration / Math.Max(1, segCount);
                int steps = 15;
                var start = drag.Points[0];
                float xs = start.X / 100f, ys = start.Y / 100f;
                if (IsMiniTouchAvailable)
                {
                    await SendTouchRawAsync("d", xs, ys);
                    if (await Logger.DelayAsync(500, ctx.Token, ctx.LogKey) == ExecutionStatus.Stop) return NodeResult.Stop();
                    for (int seg = 0; seg < segCount; seg++)
                    {
                        var p1 = drag.Points[seg];
                        var p2 = drag.Points[seg + 1];
                        float x1 = p1.X / 100f, y1 = p1.Y / 100f;
                        float x2 = p2.X / 100f, y2 = p2.Y / 100f;
                        int sleepPerStep = durPerSeg / steps;
                        for (int i = 1; i <= steps; i++)
                        {
                            if (ctx.Token.IsCancellationRequested) return NodeResult.Stop();
                            float progress = i / (float)steps;
                            float cx = x1 + (x2 - x1) * progress;
                            float cy = y1 + (y2 - y1) * progress;
                            await SendTouchRawAsync("m", cx, cy);
                            await Task.Delay(sleepPerStep);
                        }
                    }
                    var end = drag.Points.Last();
                    await SendTouchRawAsync("m", end.X / 100f, end.Y / 100f);
                    await Task.Delay(500);
                    await SendTouchRawAsync("u", end.X / 100f, end.Y / 100f);
                }
                else
                {
                    for (int seg = 0; seg < segCount; seg++)
                    {
                        var p1 = drag.Points[seg];
                        var p2 = drag.Points[seg + 1];
                        int x1 = (int)(_AtxInfo!.Display!.Width * p1.X / 100.0);
                        int y1 = (int)(_AtxInfo.Display.Height * p1.Y / 100.0);
                        int x2 = (int)(_AtxInfo.Display.Width * p2.X / 100.0);
                        int y2 = (int)(_AtxInfo.Display.Height * p2.Y / 100.0);
                        await ShellAsync($"input swipe {x1} {y1} {x2} {y2} {durPerSeg}");
                    }
                }
                Logger.Notify(ctx.LogKey, $"ATX Drag (fallback) {drag.Points.Count} điểm", Logger.Icon.Information);
                return NodeResult.Ok();
            }
            catch (Exception ex) { Logger.Notify(ctx.LogKey, $"Lỗi Drag fallback: {ex.Message}", Logger.Icon.Error); return NodeResult.Fail(ex.Message); }
        }

        public async Task<List<NodeObj>> FindNodesWithDuplicates(ATXNode nodeQuery, int physicalWidth, int physicalHeight)
        {
            if (nodeQuery == null)
                return new List<NodeObj>();

            var nodes = await DumpHierarchyAsync();
            if (nodes == null || nodes.Count == 0)
                return new List<NodeObj>();

            var results = new List<NodeObj>();

            foreach (var node in nodes)
            {
                bool match = true;

                // Kiểm tra ClassName
                if (!string.IsNullOrEmpty(nodeQuery.ClassName))
                    match &= node.ClassName?.Equals(nodeQuery.ClassName, StringComparison.OrdinalIgnoreCase) == true;

                // Kiểm tra Text
                if (!string.IsNullOrEmpty(nodeQuery.Text))
                    match &= node.Text?.Equals(nodeQuery.Text, StringComparison.OrdinalIgnoreCase) == true;

                // Kiểm tra ResourceName
                if (!string.IsNullOrEmpty(nodeQuery.ResourceName))
                    match &= node.ResourceName?.Equals(nodeQuery.ResourceName, StringComparison.OrdinalIgnoreCase) == true;

                // Kiểm tra PackageName
                if (!string.IsNullOrEmpty(nodeQuery.PackageName))
                    match &= node.PackageName?.Equals(nodeQuery.PackageName, StringComparison.OrdinalIgnoreCase) == true;

                // Kiểm tra ContentDescription
                if (!string.IsNullOrEmpty(nodeQuery.ContentDescription))
                    match &= node.ContentDescription?.Equals(nodeQuery.ContentDescription, StringComparison.OrdinalIgnoreCase) == true;

                if (match)
                    results.Add(node);
            }

            return results;
        }

        public async Task<NodeResult> KeyPress(DLoopContext ctx, string keyName)
        {
            if (ctx.Token.IsCancellationRequested)
                return NodeResult.Stop();

            // Lấy command từ KeyCodeService (tương tự logic trong ExecutePressKeyAsync)
            string command = KeyCodeService.GetCommand(keyName);
            await ShellAsync(command);

            Logger.Notify(ctx.LogKey, $"ATX KeyPress: {keyName} => {command}", Logger.Icon.Information);
            return NodeResult.Ok();
        }

        private async Task<bool> SendLongTextAsync(string text, CancellationToken token)
        {
            const int chunk = 50;
            var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var sb = new StringBuilder();
            int count = 0;

            foreach (var word in words)
            {
                if (count >= chunk)
                {
                    await ShellAsync($"input text {EncodeForAdb(sb.ToString())}");
                    await Task.Delay(50, token);
                    sb.Clear();
                    count = 0;
                }
                if (sb.Length > 0)
                    sb.Append(' ');
                sb.Append(word);
                count++;
            }

            if (sb.Length > 0)
                await ShellAsync($"input text {EncodeForAdb(sb.ToString())}");

            return true;
        }
        private static string EncodeForAdb(string text)
        {
            var sb = new StringBuilder();
            foreach (char c in text)
            {
                if (c == ' ')
                    sb.Append("%s");
                else if (char.IsLetterOrDigit(c))
                    sb.Append(c);
                else
                    sb.Append('\\').Append(c);
            }
            return sb.ToString();
        }
        public async Task<NodeResult> SendTextAsync(DLoopContext ctx, string text)
        {
            if (ctx.Token.IsCancellationRequested)
                return NodeResult.Stop();
            try
            {
                var success = await SendLongTextAsync(text, ctx.Token);
                if (success)
                {
                    Logger.Notify(ctx.LogKey, $"Đã gửi text: {text}", Logger.Icon.Information);
                    return NodeResult.Ok();
                }
                return NodeResult.Fail("Send text failed");
            }
            catch (Exception ex)
            {
                Logger.Notify(ctx.LogKey, $"Lỗi SendText: {ex.Message}", Logger.Icon.Error);
                return NodeResult.Fail(ex.Message);
            }
        }

        public async Task<NodeObj?> FindNodesByPointSmallest(int x, int y, CancellationToken ct = default)
        {
            var nodes = await DumpHierarchyAsync();
            return nodes
                .Where(n => n.Bound != null && n.Bound.Contains(x, y))
                .OrderBy(n => n.Bound.Width * n.Bound.Height) // diện tích nhỏ nhất
                .FirstOrDefault();
        }
        public async Task<NodeObj?> FindNodeByTextOrDescriptionSmallest(string query, bool exact = false, CancellationToken ct = default)
        {
            var nodes = await DumpHierarchyAsync();

            // 1. tìm theo Text trước
            var byText = nodes
                .Where(n => !string.IsNullOrEmpty(n.Text) &&
                    (exact ? n.Text.Equals(query, StringComparison.Ordinal)
                           : n.Text.Contains(query, StringComparison.OrdinalIgnoreCase)))
                .Where(n => n.Bound != null);

            var smallestByText = byText
                .OrderBy(n => n.Bound.Width * n.Bound.Height)
                .FirstOrDefault();

            if (smallestByText != null) return smallestByText;

            // 2. fallback sang ContentDescription
            var byDesc = nodes
                .Where(n => !string.IsNullOrEmpty(n.ContentDescription) &&
                    (exact ? n.ContentDescription.Equals(query, StringComparison.Ordinal)
                           : n.ContentDescription.Contains(query, StringComparison.OrdinalIgnoreCase)))
                .Where(n => n.Bound != null);

            return byDesc
                .OrderBy(n => n.Bound.Width * n.Bound.Height)
                .FirstOrDefault();
        }
        public async Task<IEnumerable<NodeObj>?> FindNodeByTextOrDescription(string query, bool exact = false, CancellationToken ct = default)
        {
            var nodes = await DumpHierarchyAsync();

            // 1. tìm theo Text trước
            var byText = nodes
                .Where(n => !string.IsNullOrEmpty(n.Text) &&
                    (exact ? n.Text.Equals(query, StringComparison.Ordinal)
                           : n.Text.Contains(query, StringComparison.OrdinalIgnoreCase)))
                .Where(n => n.Bound != null);

            if (byText != null && byText.Count() > 0)
                return byText;


            // 2. fallback sang ContentDescription
            var byDesc = nodes
                .Where(n => !string.IsNullOrEmpty(n.ContentDescription) &&
                    (exact ? n.ContentDescription.Equals(query, StringComparison.Ordinal)
                           : n.ContentDescription.Contains(query, StringComparison.OrdinalIgnoreCase)))
                .Where(n => n.Bound != null);

            if (byDesc != null && byDesc.Count() > 0)
                return byDesc;

            return null;
        }
    }
}