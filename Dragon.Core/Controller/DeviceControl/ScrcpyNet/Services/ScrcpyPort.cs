using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;

namespace Dragon.Controller.DeviceControl.ScrcpyNet.Services
{
    public class ScrcpyPort
    {
        /// <summary>
        /// Đối tượng khóa để đảm bảo thread-safe khi lấy port
        /// </summary>
        private static readonly object _portLock = new();

        /// <summary>
        /// Khoảng port từ 27183 đến 65535 để lấy port khả dụng
        /// </summary>
        private static int StartPort = 27183;
        /// <summary>
        /// Khoảng port từ 27183 đến 65535 để lấy port khả dụng
        /// </summary>
        private static readonly int EndPort = 65535;

        /// <summary>
        /// Lấy port khả dụng tiếp theo trong khoảng đã định từ 27183 đến 65535
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static int GetNextAvailablePort()
        {
            lock (_portLock)
            {
                for (int port = StartPort; port <= EndPort; port++)
                {
                    if (!IsPortInUse(port))
                    {
                        StartPort = port + 1;
                        return port;
                    }
                }
                throw new InvalidOperationException("No available ports in range.");
            }
        }

        /// <summary>
        /// Kiểm tra xem port có đang được sử dụng hay không
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static bool IsPortInUse(int port)
        {
            var ipProps = IPGlobalProperties.GetIPGlobalProperties();
            var usedPorts = ipProps.GetActiveTcpListeners().Concat(ipProps.GetActiveUdpListeners()).Select(p => p.Port);
            return usedPorts.Contains(port);
        }

        /// <summary>
        /// Chờ cho đến khi socket trên port cụ thể đóng
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static async Task WaitForSocketClose(int port)
        {
            var ipProps = IPGlobalProperties.GetIPGlobalProperties();
            while (ipProps.GetActiveTcpConnections().Any(c => c.LocalEndPoint.Port == port))
            {
                await Task.Delay(100); // Poll mỗi 100ms
                ipProps = IPGlobalProperties.GetIPGlobalProperties(); // refresh trạng thái
            }
        }
        /// <summary>
        /// Chạy lệnh CMD để kiểm tra port cụ thể
        /// </summary>
        /// <param name="Port"></param>
        /// <returns></returns>
        public static string CMDPort(int Port)
        {
            Process netstat = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/C netstat -a -n -o | findstr :" + Port,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            netstat.Start();
            string output = netstat.StandardOutput.ReadToEnd();
            netstat.WaitForExit();
            return output;
        }

        // regex cố định, không phụ thuộc Port
        private static readonly Regex PortRegex = new Regex(@"TCP\s+127\.0\.0\.1:(\d+)\s+[\d\.:]+\s+LISTENING\s+(\d+)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

        /// <summary>
        /// Kiểm tra port cụ thể có đang được sử dụng hay không
        /// </summary>
        /// <param name="Port"></param>
        /// <returns></returns>
        public static bool isPortRunInUse(int Port)
        {
            var output = CMDPort(Port);

            // duyệt tất cả dòng LISTENING trên 127.0.0.1
            foreach (Match m in PortRegex.Matches(output))
            {
                if (int.TryParse(m.Groups[1].Value, out int p) && p == Port)
                    return true;
            }
            return false;
        }

        private static readonly Regex NetstatRegex = new Regex(@"^\s*TCP\s+(?:\S+:)?(?<port>\d+)\s+\S+:\S+\s+LISTENING\s+(?<pid>\d+)", RegexOptions.CultureInvariant | RegexOptions.Multiline);

        /// <summary> Kill tiến trình đang LISTENING trên port cụ thể </summary>
        public static void KillPort(int port)
        {
            try
            {
                var psi = new ProcessStartInfo("cmd.exe", $"/c netstat -aon")
                {
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using var proc = Process.Start(psi)!;
                string output = proc.StandardOutput.ReadToEnd();
                proc.WaitForExit();

                if (string.IsNullOrWhiteSpace(output)) return;

                foreach (Match m in NetstatRegex.Matches(output))
                {
                    if (!int.TryParse(m.Groups["port"].Value, out int p) || p != port)
                        continue;

                    if (!int.TryParse(m.Groups["pid"].Value, out int pid))
                        continue;

                    Debug.WriteLine($"Kill PID {pid} trên port {port}...");

                    var killPsi = new ProcessStartInfo("cmd.exe", $"/c taskkill /F /PID {pid}")
                    {
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };
                    using var kill = Process.Start(killPsi)!;
                    Debug.WriteLine(kill.StandardOutput.ReadToEnd());
                    kill.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Lỗi kill port: " + ex.Message);
            }
        }
    }
}
