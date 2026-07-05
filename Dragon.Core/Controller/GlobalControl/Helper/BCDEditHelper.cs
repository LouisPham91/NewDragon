using System;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace Dragon.Controller.GlobalControl.Helper
{
    public class BCDEditHelper
    {
        // chạy lệnh và lấy log thô, không ép "completed successfully"
        private static (int ExitCode, string Log) Execute(string command)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = "/c " + command,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8
            };

            using var proc = Process.Start(psi);
            if (proc is null)
                return (-1, "Không khởi chạy được process");

            string output = proc.StandardOutput.ReadToEnd();
            string error = proc.StandardError.ReadToEnd();
            proc.WaitForExit();

            return (proc.ExitCode, (output + error).Trim());
        }

        public static (bool Success, string Log) RunCmd(string command)
        {
            var (code, log) = Execute(command);
            bool success = code == 0 &&
                           log.Contains("completed successfully", StringComparison.OrdinalIgnoreCase);
            return (success, log);
        }

        public static bool? IsTestSigningOn(out string rawLog)
        {
            var (code, log) = Execute("bcdedit /enum {current}");
            rawLog = log;

            if (code != 0) return null; 

            var m = Regex.Match(log, @"testsigning\s+(\w+)", RegexOptions.IgnoreCase);
            if (m.Success)
                return m.Groups[1].Value.Equals("Yes", StringComparison.OrdinalIgnoreCase);

            return null; 
        }

        public static (bool Success, string Log) EnableTestSigning()
        {
            var state = IsTestSigningOn(out var current);
            if (state == true)
                return (true, "testsigning đã ON, bỏ qua set.");

            if (state == null)
                return (false, "Không đọc được trạng thái hiện tại:\n" + current);

            var (ok, log) = RunCmd("bcdedit /set testsigning on");
            if (ok) log += "\nCần reboot để áp dụng.";
            return (ok, log);
        }

        public static (bool Success, string Log) DisableTestSigning()
        {
            var state = IsTestSigningOn(out var current);
            if (state == false)
                return (true, "testsigning đã OFF, bỏ qua set.");

            if (state == null)
                return (false, "Không đọc được trạng thái hiện tại:\n" + current);

            var (ok, log) = RunCmd("bcdedit /set testsigning off");
            if (ok) log += "\nCần reboot để áp dụng.";
            return (ok, log);
        }
    }
}