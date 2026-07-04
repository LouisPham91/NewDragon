using Dragon.Controller.GlobalControl.Helper;
using FFmpeg.AutoGen;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Dragon.Controller.Controller.DeviceControl.HATX.Core
{
    public static class AtxInit
    {


        private const string TMP = "/data/local/tmp/";
        private const int TARGET_H = 960;
        private static readonly string CACHE_PATH = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Extension", "ATX");

        private static readonly Regex OverrideSizeRegex = new Regex(@"Override size: (\d+)x(\d+)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        private static readonly Regex PhysicalSizeRegex = new Regex(@"Physical size: (\d+)x(\d+)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        private static readonly Regex RectRegex = new(@"Rect\(\s*\d+\s*,\s*\d+\s*-\s*(\d+)\s*,\s*(\d+)\s*\)", RegexOptions.Compiled);
        private static readonly Regex CurRegex = new(@"cur=(\d+)x(\d+)", RegexOptions.Compiled);

        private static readonly Dictionary<string, string> ATX_AGENT_FILE_DICT = new()
        {
            { "armeabi-v7a", "atx-agent_0.10.1_linux_armv7" },
            { "arm64-v8a", "atx-agent_0.10.1_linux_arm64" },
            { "armeabi", "atx-agent_0.10.1_linux_armv6" },
            { "x86", "atx-agent_0.10.1_linux_386" },
            { "x86_64", "atx-agent_0.10.1_linux_amd64" },
        };

        private static string GetTonghopPath(string abi)
        {
            if (!ATX_AGENT_FILE_DICT.TryGetValue(abi, out var fileName))
                throw new Exception($"Unsupported ABI: {abi}");
            return Path.Combine(CACHE_PATH, fileName);
        }
        private static string GetTtap2Path(string abi) => Path.Combine(CACHE_PATH, abi, "minitouch");
        private static string GetBincapPath(string abi) => Path.Combine(CACHE_PATH, abi, "bin", "minicap");
        private static string GetLibcapPath(string abi) => Path.Combine(CACHE_PATH, abi, "lib", "minicap.so");

        private static Task<string> Adb(string deviceId, string args, int timeoutMs = 3000, CancellationToken ct = default)
        {
            return CMD.ExecuteAdbAsync($"-s {deviceId} {args}", timeoutMs, ct);
        }

        public static async Task StartMiniCap(string deviceId, bool enable, CancellationToken ct = default)
        {
            await Adb(deviceId, "shell pkill -f bincapTap1", 2000, ct);
            if (!enable) return;

            // LẤY TỪ DUMPSYS, không phải wm size
            var (w, h, real) = await GetRotateAsync(deviceId, ct); // w,h đã đúng chiều xoay

            // luôn scale theo cạnh dài = 960
            int scaledW, scaledH;
            if (w > h) // ngang
            {
                scaledW = TARGET_H; // 960
                scaledH = (int)Math.Round(h * (double)TARGET_H / w);
            }
            else // dọc
            {
                scaledH = TARGET_H;
                scaledW = (int)Math.Round(w * (double)TARGET_H / h);
            }
            if (scaledW % 2 != 0) scaledW--;
            if (scaledH % 2 != 0) scaledH--;

            string launcher = await GetDaemonLauncher(deviceId, ct);
            string cmd = $"shell LD_LIBRARY_PATH={TMP} {launcher} {TMP}bincapTap1 -P {real}@{scaledW}x{scaledH}/0 -Q 70 > /dev/null 2>&1 &";
            await Adb(deviceId, cmd, 5000, ct);
        }

        private static readonly ConcurrentDictionary<string, bool> _initialized = new();
        private static readonly ConcurrentDictionary<string, int> _forwardPorts = new();
        private static readonly object _portLock = new();
        private static readonly HashSet<int> _usedPorts = new HashSet<int>();
        public static async Task EnsureInitializedAsync(string deviceId, bool isMiniCap = false, CancellationToken ct = default)
        {
            if (_initialized.ContainsKey(deviceId))
                return;

            var psTtap2 = await Adb(deviceId, "shell pidof ttap2", 800, ct);
            var psAtx = await Adb(deviceId, "shell pidof tonghopbaitap", 800, ct);
            var psCap = isMiniCap ? await Adb(deviceId, "shell pidof bincapTap1", 800, ct) : "ok";
            var net1888 = await Adb(deviceId, "shell netstat -an | grep :1888", 800, ct);

            // 4 & 5: check apk đã cài chưa (không cần pidof, vì uiautomator2 không chạy liên tục)
            var pkgServer = await Adb(deviceId, "shell pm list packages | grep com.github.uiautomator", 1000, ct);
            var pkgTest = await Adb(deviceId, "shell pm list packages | grep com.github.uiautomator.test", 1000, ct);

            bool hasTtap2 = !string.IsNullOrWhiteSpace(psTtap2);
            bool hasAtx = !string.IsNullOrWhiteSpace(psAtx);
            bool hasCap = !isMiniCap || !string.IsNullOrWhiteSpace(psCap);
            bool hasNet = net1888.Contains("1888");
            bool hasApk1 = pkgServer.Contains("com.github.uiautomator");
            bool hasApk2 = pkgTest.Contains("com.github.uiautomator.test");

            if (hasTtap2 && hasAtx && hasCap && hasNet && hasApk1 && hasApk2)
            {
                _initialized[deviceId] = true;
                return;
            }

            // 3. nếu thiếu thì mới chạy full init (đoạn code bạn vừa dán)
            var log = new StringBuilder();

            log.AppendLine($"===== Khởi tạo ATX lần đầu cho [{deviceId}] =====");
            await Adb(deviceId, "wait-for-device", 30000, ct);

            string abi = (await Adb(deviceId, "shell getprop ro.product.cpu.abi", 3000, ct)).Trim();
            log.AppendLine($"ABI: {abi}");

            string launcher = await GetDaemonLauncher(deviceId, ct);

            log.AppendLine("===== Push file =====");
            await PushFile(deviceId, GetTonghopPath(abi), "tonghopbaitap", ct, log);
            await PushFile(deviceId, GetTtap2Path(abi), "ttap2", ct, log);
            await PushFile(deviceId, GetBincapPath(abi), "bincapTap1", ct, log);
            await PushFile(deviceId, GetLibcapPath(abi), "libcapTap1.so", ct, log);

            log.AppendLine("===== Phân quyền =====");
            await Adb(deviceId, $"shell chmod 755 {TMP}tonghopbaitap {TMP}ttap2 {TMP}bincapTap1", 5000, ct);
            await Adb(deviceId, $"shell chmod 644 {TMP}libcapTap1.so", 5000, ct);

            log.AppendLine("===== Tạo symlink =====");
            await Adb(deviceId, $"shell ln -sf {TMP}ttap2 {TMP}minitouch", 3000, ct);
            await Adb(deviceId, $"shell ln -sf {TMP}bincapTap1 {TMP}minicap", 3000, ct);
            await Adb(deviceId, $"shell ln -sf {TMP}libcapTap1.so {TMP}minicap.so", 3000, ct);

            log.AppendLine("===== Kill tiến trình cũ =====");
            await Adb(deviceId, "shell pkill -f tonghopbaitap; pkill -f ttap2; pkill -f bincapTap1", 3000, ct);
            await Adb(deviceId, $"shell {TMP}tonghopbaitap server --stop", 3000, ct);


            log.AppendLine("===== Cài uiautomator2 =====");
            await Adb(deviceId, $"install -r \"{Path.Combine(CACHE_PATH, "apk", "app-uiautomator.apk")}\"", 30000, ct);
            await Adb(deviceId, $"install -r \"{Path.Combine(CACHE_PATH, "apk", "app-uiautomator-test.apk")}\"", 30000, ct);
            await Adb(deviceId, "shell am instrument -w io.appium.uiautomator2.server.test/androidx.test.runner.AndroidJUnitRunner > /dev/null 2>&1 &", 5000, ct);

            await Adb(deviceId, "shell cmd appops set io.appium.uiautomator2.server RUN_IN_BACKGROUND allow", 2000, ct);
            await Adb(deviceId, "shell cmd appops set io.appium.uiautomator2.server.test RUN_IN_BACKGROUND allow", 2000, ct);
            log.AppendLine("[OK] cho phép chạy nền");

            // Reset override size nếu cần
            //await ResetOverrideIfNeeded(deviceId, ct);

            log.AppendLine("===== Chạy minitouch và atx-agent =====");
            await Adb(deviceId, $"shell {launcher} {TMP}ttap2 > /dev/null 2>&1 &", 3000, ct);
            await StartMiniCap(deviceId, isMiniCap, ct);
            await Task.Delay(1000, ct);
            await Adb(deviceId, $"shell cd {TMP} && ./tonghopbaitap server --addr 0.0.0.0:1888 -d > /dev/null 2>&1", 5000, ct);

            log.AppendLine("===== Kiểm tra =====");
            string ps = await Adb(deviceId, "shell ps | grep tonghopbaitap", 3000, ct);
            string net = await Adb(deviceId, "shell netstat -an | grep 1888", 3000, ct);
            log.AppendLine(ps);
            log.AppendLine(net);

            _initialized[deviceId] = true;
            Debug.WriteLine(log.ToString());
        }
        private static async Task<int> GetFreeForwardPortAsync(string deviceId, CancellationToken ct)
        {
            if (_forwardPorts.TryGetValue(deviceId, out int existingPort))
                return existingPort;

            lock (_portLock)
            {
                for (int port = 1888; port <= 1999; port++)
                {
                    if (!_usedPorts.Contains(port))
                    {
                        _usedPorts.Add(port);
                        _forwardPorts[deviceId] = port;
                        return port;
                    }
                }
            }
            throw new Exception("Không còn port trống (1888-1999) để forward");
        }


        public static async Task<(string AtxUrl, int ForwardPort, bool IsWiFi)> GetAtxUrlAsync(string deviceId, CancellationToken ct = default)
        {
            string wifiIp = await GetIpAsync(deviceId, ct);
            if (!string.IsNullOrEmpty(wifiIp))
            {
                // Chuyển sang WiFi: xoá forward nếu có
                if (_forwardPorts.TryRemove(deviceId, out int oldPort))
                {
                    lock (_portLock) _usedPorts.Remove(oldPort);
                    await Adb(deviceId, $"forward --remove tcp:{oldPort}", 2000, ct);
                }
                return ($"http://{wifiIp}:1888", 0, true);
            }

            // USB mode: lấy hoặc tạo port mới
            int port = await GetFreeForwardPortAsync(deviceId, ct);
            await Adb(deviceId, $"forward tcp:{port} tcp:1888", 5000, ct);
            return ($"http://127.0.0.1:{port}", port, false);
        }
        public static async Task StopAsync(string deviceId, CancellationToken ct = default)
        {
            var log = new StringBuilder();
            log.AppendLine($"===== STOP [{deviceId}] =====");

            string stop = await Adb(deviceId, $"shell {TMP}tonghopbaitap server --stop", 3000, ct);
            log.AppendLine(string.IsNullOrWhiteSpace(stop) ? "[OK] server --stop" : stop.Trim());

            await Adb(deviceId, "shell pkill -9 -f tonghopbaitap; pkill -9 -f ttap2; pkill -9 -f bincapTap1", 2000, ct);
            log.AppendLine("[OK] pkill ...");

            if (_forwardPorts.TryRemove(deviceId, out int port))
            {
                lock (_portLock) _usedPorts.Remove(port);
                await Adb(deviceId, $"forward --remove tcp:{port}", 2000, ct);
            }
            _initialized.TryRemove(deviceId, out _);

            string net = await Adb(deviceId, "shell netstat -an | grep 1888", 2000, ct);
            log.AppendLine(string.IsNullOrWhiteSpace(net) ? "[OK] port 1888 closed" : "[WARN] port 1888 still open");

            Debug.WriteLine(log.ToString());
        }

        // them helper nay vao class
        private static async Task<string> GetDaemonLauncher(string deviceId, CancellationToken ct)
        {
            // API < 23 thuong khong co nohup
            string which = await Adb(deviceId, "shell which nohup", 2000, ct);
            if (!string.IsNullOrWhiteSpace(which) && which.Contains("nohup"))
                return "nohup";

            // fallback an toan cho moi Android
            return "setsid";
        }

        private static async Task PushFile(string deviceId, string localPath, string remoteName, CancellationToken ct, StringBuilder log)
        {
            if (!File.Exists(localPath))
            {
                log.AppendLine($"[MISS] {localPath}");
                return;
            }
            string remote = TMP + remoteName;
            var res = await Adb(deviceId, $"push \"{localPath}\" {remote}", 30000, ct);
            bool fail = res.Contains("error", StringComparison.OrdinalIgnoreCase) || res.Contains("failed", StringComparison.OrdinalIgnoreCase);
            log.AppendLine(fail ? $"[FAIL] {remoteName}" : $"[OK] {remoteName} <- {Path.GetFileName(localPath)}");
        }

        private static async Task<(int w, int h, string raw)> GetRotateAsync(string deviceId, CancellationToken ct)
        {
            foreach (var cmd in new[] {
                "shell dumpsys display | grep mCurrentDisplayRect",
                "shell dumpsys window displays | grep cur=",
                "shell dumpsys display | grep mCurrentLayerStackRect",
                "shell dumpsys display | grep physicalFrame"
            })
            {
                string o = await Adb(deviceId, cmd, 2000, ct);
                var m = RectRegex.Match(o);
                if (!m.Success) m = CurRegex.Match(o);
                if (m.Success) return (int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value), $"{m.Groups[1].Value}x{m.Groups[2].Value}");
            }
            return await GetPhysicalSizeAsync(deviceId, ct);
        }
        private static async Task<(int w, int h, string raw)> GetPhysicalSizeAsync(string deviceId, CancellationToken ct)
        {
            string output = await Adb(deviceId, "shell wm size", 3000, ct);

            // 1. ưu tiên Override size nếu có
            var m = OverrideSizeRegex.Match(output);
            if (!m.Success)
            {
                // 2. không có override thì lấy Physical size
                m = PhysicalSizeRegex.Match(output);
            }

            if (!m.Success)
                throw new Exception($"Không parse được wm size: {output}");

            int w = int.Parse(m.Groups[1].Value);
            int h = int.Parse(m.Groups[2].Value);
            return (w, h, $"{w}x{h}");
        }

        private static async Task<string> GetIpAsync(string deviceId, CancellationToken ct)
        {
            string ip = await Adb(deviceId, "shell ip -f inet addr show wlan0 | awk '/inet /{print $2}' | cut -d/ -f1", 3000, ct);
            ip = ip.Trim();
            if (string.IsNullOrWhiteSpace(ip))
                ip = await Adb(deviceId, "shell getprop dhcp.wlan0.ipaddress", 3000, ct);
            return ip.Trim();
        }

        //+ Trên Android, lệnh wm size và wm density có thể override độ phân giải tạm thời:
        //   - Farm hay dùng để test, nhưng nếu quên reset, lần sau:
        //   - GetPhysicalSizeAsync sẽ đọc ra Override size: 720x1280 thay vì 1080x2400 thật
        //   - minicap tính scale sai → hình méo
        //   - minitouch tap sai tọa độ

    }
}