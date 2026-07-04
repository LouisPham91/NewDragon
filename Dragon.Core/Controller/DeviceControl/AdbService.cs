using AdvancedSharpAdbClient;
using AdvancedSharpAdbClient.Models;
using AdvancedSharpAdbClient.Receivers;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Dragon.Controller.Controller.DeviceControl
{
    public partial class AdbService
    {
        [GeneratedRegex(@"(\d)", RegexOptions.CultureInvariant)]
        private static partial Regex DigitRegex();

        public static async Task<int> GetUserRotationAsync(DeviceData device, IAdbClient adb, CancellationToken token = default)
        {
            var output = await ShellAsync(device.Serial, device, adb, "settings get system user_rotation", token);
            return int.TryParse(output.Trim(), out var v) ? v : -1;
        }

        // === HÀM MỚI: lấy size thật, chạy tốt Android 6+ ===
        public static async Task<(int w, int h)> GetPhysicalSizeAsync(DeviceData device, IAdbClient adb, CancellationToken token = default)
        {
            // 1. ưu tiên mCurrentDisplayRect – Samsung/Xiaomi/Oppo đều có
            var out1 = await ShellAsync(device.Serial, device, adb, "dumpsys display | grep mCurrentDisplayRect", token);
            // ví dụ: mCurrentDisplayRect=Rect(0, 0 - 1080, 2400)
            var m = Regex.Match(out1, @"Rect\(\d+,\s*\d+\s*-\s*(\d+),\s*(\d+)\)");
            if (m.Success) return (int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value));

            // 2. fallback AOSP cũ
            var out2 = await ShellAsync(device.Serial, device, adb, "dumpsys display | grep cur=", token);
            m = Regex.Match(out2, @"cur=(\d+)x(\d+)");
            if (m.Success) return (int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value));

            return (0, 0);
        }

        // === HÀM MỚI: biết ngang/dọc thật ===
        public static async Task<bool> IsLandscapeAsync(DeviceData device, IAdbClient adb, CancellationToken token = default)
        {
            var (w, h) = await GetPhysicalSizeAsync(device, adb, token);
            if (w > 0 && h > 0) return w > h; // cách chính xác nhất

            // fallback nếu wm size lỗi
            int rot = await GetUserRotationAsync(device, adb, token);
            return rot == 1 || rot == 3;
        }

  
        // giữ nguyên hàm cũ của cậu (đã sửa lỗi biến v)
        public static async Task<int> GetRotationAsync(string DeviceID, DeviceData Device, IAdbClient _adb, CancellationToken token = default)
        {
            var output = await ShellAsync(DeviceID, Device, _adb, "settings get system user_rotation", token);
            var m = DigitRegex().Match(output);
            return m.Success && int.TryParse(m.Groups[1].Value, out var v) ? v : -1;
        }
        public static async Task RemoveADBForwardsAsync(DeviceData device, IAdbClient adb, CancellationToken ct = default)
        {
            var forwards = await adb.ListForwardAsync(device, ct);

            foreach (var f in forwards)
            {
                // forward atx luôn là: PC tcp:xxxxx -> Android tcp:7726
                if (f.RemoteSpec.Protocol == ForwardProtocol.Tcp && f.RemoteSpec.Port == 7726)
                {
                    await adb.RemoveForwardAsync(device, f.LocalSpec.Port, ct);
                    Debug.WriteLine($"[{device.Serial}] adb forward removed: {f.Local} -> {f.Remote}");
                }
            }
        }
        public static async Task RemoveAtxForwardsAsync(DeviceData device, IAdbClient adb, CancellationToken ct = default)
        {
            var forwards = await adb.ListForwardAsync(device, ct);

            foreach (var f in forwards)
            {
                // forward atx luôn là: PC tcp:xxxxx -> Android tcp:7912
                if (f.RemoteSpec.Protocol == ForwardProtocol.Tcp && f.RemoteSpec.Port == 7912)
                {
                    await adb.RemoveForwardAsync(device, f.LocalSpec.Port, ct);
                    Debug.WriteLine($"[{device.Serial}] ATX forward removed: {f.Local} -> {f.Remote}");
                }
            }
        }

        // 2. Xóa mọi forward của scrcpy (nếu có) – thường là localabstract
        public static async Task RemoveScrcpyForwardsAsync(DeviceData device, IAdbClient adb, CancellationToken ct = default)
        {
            var forwards = await adb.ListForwardAsync(device, ct);

            foreach (var f in forwards)
            {
                bool isScrcpy = f.RemoteSpec.Protocol == ForwardProtocol.LocalAbstract &&
                                f.RemoteSpec.SocketName?.StartsWith("scrcpy", StringComparison.OrdinalIgnoreCase) == true;

                if (!isScrcpy) continue;

                // forward dạng tcp:xxxx -> localabstract:scrcpy_xxx
                if (f.LocalSpec.Protocol == ForwardProtocol.Tcp)
                {
                    await adb.RemoveForwardAsync(device, f.LocalSpec.Port, ct);
                }
                else
                {
                    // forward dạng localabstract -> localabstract (hiếm) – dùng lệnh thô
                    await adb.ExecuteServerCommandAsync(
                        $"host-serial:{device.Serial}",
                        $"killforward:{f.LocalSpec}", ct);
                }

                Debug.WriteLine($"[{device.Serial}] SCRCPY forward removed: {f.Local} -> {f.Remote}");
            }
        }

        // 3. (bonus) Xóa reverse của scrcpy – scrcpy thật ra dùng cái này nhiều hơn
        public static async Task RemoveScrcpyReversesAsync(DeviceData device, IAdbClient adb, CancellationToken ct = default)
        {
            var reverses = await adb.ListReverseForwardAsync(device, ct);

            foreach (var r in reverses)
            {
                if (r.LocalSpec.Protocol == ForwardProtocol.LocalAbstract &&
                    r.LocalSpec.SocketName?.StartsWith("scrcpy", StringComparison.OrdinalIgnoreCase) == true)
                {
                    await adb.RemoveReverseForwardAsync(device, r.Local, ct);
                    Debug.WriteLine($"[{device.Serial}] SCRCPY reverse removed: {r.Local} -> {r.Remote}");
                }
            }
        }

        public static async Task<string> ShellAsync(string DeviceID, DeviceData Device, IAdbClient _adb, string command, CancellationToken ct = default)
        {
            var receiver = new ConsoleOutputReceiver();
            try
            {
                await _adb.ExecuteRemoteCommandAsync(command, Device, receiver, ct);
                return receiver.ToString().Trim();
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine($"[{DeviceID}] shell timeout: {command}");
                return string.Empty;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[{DeviceID}] shell error: {ex.Message}");
                return string.Empty;
            }
        }
    }
}