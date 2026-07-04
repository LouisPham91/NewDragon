using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Net.Sockets;
using System.Drawing;

namespace Dragon.Controller.GlobalControl.Helper
{

    public static class CMD
    {
        // adb.exe bạn để sẵn trong project
        public static readonly string ADBPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "Extension", "ScrcpyNet", "adb.exe");

        // Thư mục portable - nằm ngay trong app, copy đi đâu cũng được
        private static readonly string PortableHome;
        private static readonly string AndroidDir;

        // Chạy 1 lần duy nhất khi app khởi động
        static CMD()
        {
            PortableHome = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "adb_data");
            AndroidDir = Path.Combine(PortableHome, ".android");
            Directory.CreateDirectory(AndroidDir);

            // Ép adb dùng thư mục này thay vì C:\Users\...\ .android
            Environment.SetEnvironmentVariable("ANDROID_SDK_HOME", PortableHome);
            Environment.SetEnvironmentVariable("HOME", PortableHome);
            Environment.SetEnvironmentVariable("ADB_VENDOR_KEYS", AndroidDir);

        }

        private static void ApplyEnv(ProcessStartInfo psi)
        {
            psi.Environment["ANDROID_SDK_HOME"] = PortableHome;
            psi.Environment["HOME"] = PortableHome;
            psi.Environment["USERPROFILE"] = PortableHome;   // <-- quan trọng nhất
            psi.Environment["APPDATA"] = Path.Combine(PortableHome, "AppData");
            psi.Environment["LOCALAPPDATA"] = Path.Combine(PortableHome, "AppData");
            psi.Environment["ADB_VENDOR_KEYS"] = AndroidDir;
        }

        public static async Task<string> ExecuteAdbAsync(string args, int timeoutMs = 1500, CancellationToken ct = default)
        {
            if (args.StartsWith("adb ", StringComparison.OrdinalIgnoreCase))
                args = args[4..].Trim();

            if (!File.Exists(ADBPath))
                return $"[ADB] not found: {ADBPath}";

            using var proc = new Process();
            proc.StartInfo = new ProcessStartInfo
            {
                FileName = ADBPath,
                Arguments = args,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8
            };
            ApplyEnv(proc.StartInfo);

            proc.Start();

            var outTask = proc.StandardOutput.ReadToEndAsync(ct);
            var errTask = proc.StandardError.ReadToEndAsync(ct);
            var exitTask = proc.WaitForExitAsync(ct);

            using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            cts.CancelAfter(timeoutMs);

            try
            {
                await exitTask.WaitAsync(cts.Token);
            }
            catch
            {
                try { proc.Kill(true); } catch { }
                return "[ADB] Timeout";
            }

            string output = await outTask;
            string error = await errTask;
            return (output + "\n" + error).Trim();
        }

        public static string ExecuteAdb(string args, int timeoutMs = 2000)
        {
            if (args.Contains("start-server") || args.Contains("kill-server"))
                timeoutMs = 15000;

            if (args.StartsWith("adb ", StringComparison.OrdinalIgnoreCase))
                args = args[4..].Trim();

            if (!File.Exists(ADBPath))
                return $"[ADB] adb.exe not found at: {ADBPath}";

            using var p = new Process();
            var sb = new StringBuilder();
            p.StartInfo = new ProcessStartInfo
            {
                FileName = ADBPath,
                Arguments = args,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8
            };
            ApplyEnv(p.StartInfo);

            p.OutputDataReceived += (s, e) => { if (e.Data != null) sb.AppendLine(e.Data); };
            p.ErrorDataReceived += (s, e) => { if (e.Data != null) sb.AppendLine(e.Data); };

            p.Start();
            p.BeginOutputReadLine();
            p.BeginErrorReadLine();

            if (!p.WaitForExit(timeoutMs)) { try { p.Kill(); } catch { } return "[ADB] Timeout"; }
            p.WaitForExit();

            return sb.ToString().Trim();
        }

        public static string? ExecuteAaptCMD(string fileName, string arguments, int timeoutMs = 20000)
        {
            try
            {
                using (Process process = new Process())
                {
                    var psi = new ProcessStartInfo
                    {
                        FileName = fileName,              // full path tới aapt.exe
                        Arguments = arguments,            // ví dụ: $"dump badging \"{apkPath}\""
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    };

                    process.StartInfo = psi;
                    process.Start();

                    // đọc output trước
                    string stdout = process.StandardOutput.ReadToEnd();
                    string stderr = process.StandardError.ReadToEnd();

                    if (!process.WaitForExit(timeoutMs))
                    {
                        try { process.Kill(true); } catch { }
                        return "[TIMEOUT]";
                    }

                    string combined = string.IsNullOrWhiteSpace(stdout) ? stderr : stdout;
                    return combined.Trim();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
    //public class CMD
    //{
    //    public static readonly string ADBPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Extension", "ScrcpyNet", "adb.exe");
    //    public static async Task<string> ExecuteAdbAsync(string args, int timeoutMs = 1500, CancellationToken ct = default)
    //    {
    //        if (args.StartsWith("adb ", StringComparison.OrdinalIgnoreCase))
    //            args = args[4..].Trim();

    //        if (!File.Exists(ADBPath))
    //            return $"[ADB] not found: {ADBPath}";

    //        using var proc = new Process();
    //        proc.StartInfo = new ProcessStartInfo
    //        {
    //            FileName = ADBPath,
    //            Arguments = args,
    //            UseShellExecute = false,
    //            RedirectStandardOutput = true,
    //            RedirectStandardError = true,
    //            CreateNoWindow = true,
    //            StandardOutputEncoding = Encoding.UTF8,
    //            StandardErrorEncoding = Encoding.UTF8
    //        };

    //        proc.Start();

    //        var outTask = proc.StandardOutput.ReadToEndAsync(ct);
    //        var errTask = proc.StandardError.ReadToEndAsync(ct);
    //        var exitTask = proc.WaitForExitAsync(ct);

    //        // tạo timeout 2s
    //        using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
    //        cts.CancelAfter(timeoutMs);

    //        try
    //        {
    //            //// chờ exit HOẶC hết 2s
    //            await exitTask.WaitAsync(cts.Token);

    //        }
    //        catch (TimeoutException)
    //        {
    //            try { proc.Kill(true); } catch { }
    //            return "[ADB] Timeout";
    //        }
    //        catch (OperationCanceledException)
    //        {
    //            try { proc.Kill(true); } catch { }
    //            return "[ADB] Timeout";
    //        }

    //        // lấy output (đã xong vì process đã thoát)
    //        string output = await outTask;
    //        string error = await errTask;
    //        return (output + "\n" + error).Trim();
    //    }


    //    public static string ExecuteAdb(string args, int timeoutMs = 2000) // tăng timeout
    //    {
    //        if (args.StartsWith("adb ", StringComparison.OrdinalIgnoreCase))
    //            args = args[4..].Trim();

    //        if (!File.Exists(ADBPath))
    //            return $"[ADB] adb.exe not found at: {ADBPath}";

    //        using var p = new Process();
    //        var sb = new StringBuilder();
    //        p.StartInfo = new ProcessStartInfo
    //        {
    //            FileName = ADBPath,
    //            Arguments = args,
    //            UseShellExecute = false,
    //            RedirectStandardOutput = true,
    //            RedirectStandardError = true,
    //            CreateNoWindow = true,
    //            StandardOutputEncoding = Encoding.UTF8,
    //            StandardErrorEncoding = Encoding.UTF8
    //        };
    //        p.OutputDataReceived += (s, e) => { if (e.Data != null) sb.AppendLine(e.Data); };
    //        p.ErrorDataReceived += (s, e) => { if (e.Data != null) sb.AppendLine(e.Data); };

    //        p.Start();
    //        p.BeginOutputReadLine();
    //        p.BeginErrorReadLine();

    //        if (!p.WaitForExit(timeoutMs)) { try { p.Kill(); } catch { } return "[ADB] Timeout"; }
    //        p.WaitForExit(); // đợi flush hết stream

    //        return sb.ToString().Trim();
    //    }
    //    public static string? ExecuteAaptCMD(string fileName, string arguments, int timeoutMs = 20000)
    //    {
    //        try
    //        {
    //            using (Process process = new Process())
    //            {
    //                var psi = new ProcessStartInfo
    //                {
    //                    FileName = fileName,              // full path tới aapt.exe
    //                    Arguments = arguments,            // ví dụ: $"dump badging \"{apkPath}\""
    //                    CreateNoWindow = true,
    //                    UseShellExecute = false,
    //                    RedirectStandardOutput = true,
    //                    RedirectStandardError = true
    //                };

    //                process.StartInfo = psi;
    //                process.Start();

    //                // đọc output trước
    //                string stdout = process.StandardOutput.ReadToEnd();
    //                string stderr = process.StandardError.ReadToEnd();

    //                if (!process.WaitForExit(timeoutMs))
    //                {
    //                    try { process.Kill(true); } catch { }
    //                    return "[TIMEOUT]";
    //                }

    //                string combined = string.IsNullOrWhiteSpace(stdout) ? stderr : stdout;
    //                return combined.Trim();
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            return ex.Message;
    //        }
    //    }
    //}
    public class AdbHelper
    {
        public static async Task<string?> InstallApkAsync(string deviceID, string apkPath)
        {
            if (!string.IsNullOrEmpty(deviceID))
            {
                // Tạo lệnh ADB
                var dir = Directory.GetCurrentDirectory();
                var filepath = apkPath.Contains(":") ? apkPath : $"{dir}\\{apkPath}";
                string cmdCommand = $"adb -s {deviceID} install \"{filepath}\"";

                // Thực thi lệnh
                string? result = await CMD.ExecuteAdbAsync(cmdCommand);

                return result;
            }
            else
            {
                Debug.WriteLine("Bạn chưa chọn DeviceID để chạy code ");
                return null;
            }

        }
        public static string? InstallApk(string deviceID, string apkPath)
        {
            if (!string.IsNullOrEmpty(deviceID))
            {
                // Tạo lệnh ADB
                var dir = Directory.GetCurrentDirectory();
                var filepath = apkPath.Contains(":") ? apkPath : $"{dir}\\{apkPath}";
                string cmdCommand = $"adb -s {deviceID} install \"{filepath}\"";

                // Thực thi lệnh
                string? result = CMD.ExecuteAdb(cmdCommand);

                return result;
            }
            else
            {
                Debug.WriteLine("Bạn chưa chọn DeviceID để chạy code ");
                return null;
            }

        }
        public static async Task<string?> ReInstallApkAsync(string deviceID, string apkPath)
        {
            if (!string.IsNullOrEmpty(deviceID))
            {
                // Tạo lệnh ADB
                var dir = Directory.GetCurrentDirectory();
                var filepath = apkPath.Contains(":") ? apkPath : $"{dir}\\{apkPath}";
                string cmdCommand = $"adb -s {deviceID} install -r \"{filepath}\"";

                // Thực thi lệnh
                string? result = await CMD.ExecuteAdbAsync(cmdCommand);
                return result;
            }
            else
            {
                Debug.WriteLine("Bạn chưa chọn DeviceID để chạy code ");
                return null;
            }

        }
        public static async Task<string?> UninstallApkAsync(string deviceID, string apkPath)
        {
            if (!string.IsNullOrEmpty(deviceID))
            {
                //adb shell pm uninstall --user 0 com.android.chrome
                // Tạo lệnh ADB
                string cmdCommand = $"adb -s {deviceID} uninstall \"{apkPath}\"";

                // Thực thi lệnh
                string? result = await CMD.ExecuteAdbAsync(cmdCommand);
                return result;
            }
            else
            {
                Debug.WriteLine("Bạn chưa chọn DeviceID để chạy code ");
                return null;
            }

        }
        public static async Task VietChuTiengViet(string deviceID, string text)
        {
            if (!string.IsNullOrEmpty(deviceID))
            {
                bool ok = false;
                // Bước 1: Kiểm tra xem com.android.adbkeyboard có tồn tại không
                //string checkAdbKeyboard = await ExecuteCMDAsync($"adb -s {deviceID} shell pm list packages | grep com.android.adbkeyboard");
                string? checkAdbKeyboard = await CMD.ExecuteAdbAsync($"adb -s {deviceID} shell pm list packages | findstr com.android.adbkeyboard");
                if (checkAdbKeyboard == null || checkAdbKeyboard == string.Empty)
                {
                    // Nếu com.android.adbkeyboard không tồn tại, cài đặt APK tại đây
                    string? ketqua = await InstallApkAsync(deviceID, Directory.GetCurrentDirectory() + "\\App\\ADBKeyBoard.apk");
                    if (!string.IsNullOrEmpty(ketqua) && ketqua.Contains("Success"))
                    {
                        ok = true;
                        // Bước 2: Kích hoạt ADBKeyBoard từ adb
                        await CMD.ExecuteAdbAsync($"adb -s {deviceID} shell ime enable com.android.adbkeyboard/.AdbIME");
                    }
                }
                else if (checkAdbKeyboard.Contains("com.android.adbkeyboard"))
                {
                    ok = true;
                }
                if (ok)
                {
                    string input_text_shell = Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
                    // Bước 3: Chuyển sang ADBKeyBoard từ adb
                    await CMD.ExecuteAdbAsync($"adb -s {deviceID} shell ime set com.android.adbkeyboard/.AdbIME");
                    string cmdCommand = $"adb -s {deviceID} shell am broadcast -a ADB_INPUT_B64 --es msg {input_text_shell}";

                    await CMD.ExecuteAdbAsync(cmdCommand);
                }
            }
            else
            {
                Debug.WriteLine("Bạn chưa chọn DeviceID để chạy code ");
            }


        }
        public static void SetupTiengViet(string deviceID)
        {
            if (!string.IsNullOrEmpty(deviceID))
            {
                bool ok = false;
                // Bước 1: Kiểm tra xem com.android.adbkeyboard có tồn tại không
                //string checkAdbKeyboard = await ExecuteCMDAsync($"adb -s {deviceID} shell pm list packages | grep com.android.adbkeyboard");
                string? checkAdbKeyboard = CMD.ExecuteAdb($"adb -s {deviceID} shell pm list packages | findstr com.android.adbkeyboard");
                if (checkAdbKeyboard == null || checkAdbKeyboard == string.Empty)
                {
                    // Nếu com.android.adbkeyboard không tồn tại, cài đặt APK tại đây
                    string? ketqua = InstallApk(deviceID, Directory.GetCurrentDirectory() + "\\App\\ADBKeyBoard.apk");
                    if (!string.IsNullOrEmpty(ketqua) && ketqua.Contains("Success"))
                    {
                        ok = true;
                        // Bước 2: Kích hoạt ADBKeyBoard từ adb
                        CMD.ExecuteAdb($"adb -s {deviceID} shell ime enable com.android.adbkeyboard/.AdbIME");
                    }
                }
                else if (checkAdbKeyboard.Contains("com.android.adbkeyboard"))
                {
                    ok = true;
                }
                if (ok)
                {
                    //string input_text_shell = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(text));
                    // Bước 3: Chuyển sang ADBKeyBoard từ adb
                    CMD.ExecuteAdb($"adb -s {deviceID} shell ime set com.android.adbkeyboard/.AdbIME");
                    //string cmdCommand = $"adb -s {deviceID} shell am broadcast -a ADB_INPUT_B64 --es msg {input_text_shell}";

                    //await CMD.ExecuteAdbAsync(cmdCommand);
                }
            }
            else
            {
                Debug.WriteLine("Bạn chưa chọn DeviceID để chạy code ");
            }


        }

        private static readonly Regex PackageRegex = new Regex(@"^package:(?<name>\S+)", RegexOptions.CultureInvariant);
        public static async Task<List<string>?> TimAppName(string deviceID, string AppName)
        {
            if (string.IsNullOrWhiteSpace(deviceID))
                return null;

            // pm list packages đã có filter sẵn, không cần findstr
            string cmd = $"adb -s {deviceID} shell pm list packages {AppName}";
            string? output = await CMD.ExecuteAdbAsync(cmd);

            if (string.IsNullOrWhiteSpace(output))
                return null;

            var result = new List<string>();

            foreach (var line in output.Split('\n', StringSplitOptions.RemoveEmptyEntries))
            {
                var m = PackageRegex.Match(line.Trim());
                if (m.Success)
                    result.Add(m.Groups["name"].Value);
            }

            return result;
        }

        public static async Task<List<string>?> FinAllAppName(string deviceID)
        {
            if (!string.IsNullOrEmpty(deviceID))
            {
                string cmdCommand = $"adb -s {deviceID} shell pm list package";

                string? stringpakage = await CMD.ExecuteAdbAsync(cmdCommand);

                if (!string.IsNullOrEmpty(stringpakage))
                {
                    var packages = stringpakage.Split("\n");
                    List<string> appNames = new List<string>();

                    foreach (string package in packages)
                    {
                        Match match = PackageRegex.Match(package);
                        if (match.Success)
                        {
                            string appName = match.Groups[1].Value;
                            appNames.Add(appName);
                        }
                    }

                    return appNames;
                }
            }
            return null;
        }

        public static async Task<bool> CheckExitingDeviceID(string deviceID)
        {
            if (!string.IsNullOrEmpty(deviceID))
            {
                for (int i = 0; i < 80; i++)
                {
                    try
                    {
                        var check = await CMD.ExecuteAdbAsync($"adb devices");

                        if (check != null && check != string.Empty)
                        {
                            if (check.Contains(deviceID))
                            {
                                return true;
                            }

                        }
                    }
                    catch (Exception)
                    { }
                    Thread.Sleep(1000);
                }

            }

            return false;
        }
        public static async Task PowerOnD6603(string deviceID)
        {
            if (!string.IsNullOrEmpty(deviceID))
            {

                var check = await CMD.ExecuteAdbAsync($"adb -s {deviceID} shell dumpsys power | findstr \"mWakefulness=\"");
                if (!string.IsNullOrEmpty(check) && check.Contains("Asleep"))
                {
                    string cmdCommand = $"adb -s {deviceID} shell input keyevent KEYCODE_POWER";
                    await CMD.ExecuteAdbAsync(cmdCommand);
                }

                string cmdCommandv1 = $"adb -s {deviceID} shell dumpsys window | findstr mDreamingLockscreen";
                var value = await CMD.ExecuteAdbAsync(cmdCommandv1);
                if (!string.IsNullOrEmpty(value) && value.Contains("mDreamingLockscreen=true"))
                {
                    await CMD.ExecuteAdbAsync($"adb -s {deviceID} shell input swipe 120 720 120 220 100");
                }

            }
        }
        public static async Task Power(string deviceID)
        {
            if (!string.IsNullOrEmpty(deviceID))
            {
                string cmdCommand = $"adb -s {deviceID} shell input keyevent KEYCODE_POWER";
                await CMD.ExecuteAdbAsync(cmdCommand);
            }
        }
        private static readonly Regex SettingsRegex = new Regex(@"\b[\w\.]+SETTINGS_[\w]+|[\w\.]+SETTINGS\b", RegexOptions.CultureInvariant);


        public static async Task<List<string>?> getActionIntentAndroiSetting(string deviceID)
        {
            if (!string.IsNullOrEmpty(deviceID))
            {
                var chkString = await CMD.ExecuteAdbAsync($"adb -s {deviceID} shell dumpsys | findstr \"SETTINGS\"");
                if (!string.IsNullOrEmpty(chkString))
                {
                    MatchCollection matches = SettingsRegex.Matches(chkString);
                    List<string> result = new List<string>();
                    foreach (Match match in matches)
                    {
                        if (!match.Value.Contains("WRITE") && !match.Value.Contains("INPUT_METHOD_SETTINGS") && !match.Value.Contains("permission"))
                        {
                            result.Add(match.Value);
                        }

                    }
                    // In ra kết quả
                    foreach (string s in result)
                    {
                        Debug.WriteLine(s);
                    }
                    return result;

                }
            }

            return null;
        }
        public static async Task<bool> OpenDeepLink(string deviceID, string deeplink)
        {
            if (!string.IsNullOrEmpty(deviceID))
            {
                string cmdCommand = $"adb -s {deviceID} shell am start -a {deeplink}";
                var value = await CMD.ExecuteAdbAsync(cmdCommand);
                if (!string.IsNullOrEmpty(value) && !value.Contains("Error"))
                {
                    return true;
                }
            }
            return false;
        }
        public static async Task<bool> adbMove(string deviceID, int pointAx, int PointAy, int PointBx, int PointBy, int Touch)
        {
            if (!string.IsNullOrEmpty(deviceID))
            {

                string cmdCommand = $"adb -s {deviceID} shell input swipe {pointAx} {PointAy} {PointBx} {PointBy} {Touch}";
                var value = await CMD.ExecuteAdbAsync(cmdCommand);
                if (!string.IsNullOrEmpty(value) && !value.Contains("Error"))
                {
                    return true;
                }
            }
            return false;
        }
        public static async Task<bool> OpenApp(string deviceID, string appname)
        {
            if (!string.IsNullOrEmpty(deviceID))
            {
                string cmdCommand = $"adb -s {deviceID} shell monkey -p {appname} -c android.intent.category.LAUNCHER 1";
                var value = await CMD.ExecuteAdbAsync(cmdCommand);
                if (!string.IsNullOrEmpty(value) && !value.Contains("No activities"))
                {
                    return true;
                }
            }
            return false;
        }
        public static async Task<bool> AreAllAppsLoaded(string deviceID)
        {
            string cmdCommand = $"adb -s {deviceID} shell dumpsys activity activities";
            string? output = await CMD.ExecuteAdbAsync(cmdCommand);
            if (!string.IsNullOrEmpty(output))
            {
                return output.Contains("state=RESUMED");
            }
            else
            {
                return false;
            }
        }

        public static async Task<string?> GetPropSerialno(string deviceID)
        {
            string cmdCommandv2 = $"adb -s {deviceID} shell getprop ro.serialno";
            var listvalue = await CMD.ExecuteAdbAsync(cmdCommandv2);
            if (!string.IsNullOrEmpty(listvalue))
            {
                return listvalue.Replace("\n", "").Replace("\r", "");
            }
            else
            {
                return null;
            }

        }

        public static async Task CoDinhManHinhAndroid(string deviceID, int doc_ngang = 0)
        {
            //adb shell settings put system accelerometer_rotation 0
            //adb shell settings put system user_rotation 0
            //adb shell settings put system user_rotation 1

            string cmdCommandv1 = $"adb -s {deviceID} shell settings put system accelerometer_rotation 0";
            await CMD.ExecuteAdbAsync(cmdCommandv1);

            string cmdCommandv2 = $"adb -s {deviceID} shell settings put system user_rotation {doc_ngang}";
            await CMD.ExecuteAdbAsync(cmdCommandv2);
        }

        //public static async Task<bool> DumpyHelpClick(DeviceData device, List<string> TextNote, string TenFile, bool click = true, string InnerText = "text")
        //{
        //    if (TextNote != null && TextNote.Count() > 0)
        //    {
        //        TextNote = TextNote.Select(s => s.ToLower()).ToList();
        //        var getPropSerialno = await GetPropSerialno(device.Serial);
        //        getPropSerialno = getPropSerialno.Replace("\r\n", "");
        //        string folderPath = $"CheckHandle\\{getPropSerialno}";
        //        string filePath = Path.Combine(folderPath, "getSerial.txt");

        //        Directory.CreateDirectory(folderPath);

        //        var tagertXmlPath = Directory.GetCurrentDirectory() + "\\" + folderPath + $"\\{TenFile}.xml";
        //        await CMD.ExecuteAdbAsync($"adb -s {device.Serial} shell rm /sdcard/{TenFile}.xml");
        //        await Task.Delay(500);
        //        await CMD.ExecuteAdbAsync($"adb -s {device.Serial} shell uiautomator dump");
        //        await Task.Delay(500);
        //        File.Delete(tagertXmlPath);

        //        await Task.Delay(500);
        //        var file = await PullFileFromDevice(device.Serial, $"sdcard/window_dump.xml", folderPath + $"\\{TenFile}.xml");
        //        if (file != null)
        //        {
        //            await Task.Delay(500);
        //            if (File.Exists(tagertXmlPath))
        //            {
        //                XmlDocument doc = new XmlDocument();
        //                doc.Load(tagertXmlPath); // Replace xmlString with your XML string

        //                // Select all 'node' elements
        //                XmlNodeList nodes = doc.SelectNodes("//node");

        //                // Loop through the nodes and find the one with text "ALLOW"
        //                foreach (XmlNode node in nodes)
        //                {
        //                    var textnote = node.Attributes[InnerText]?.InnerText;
        //                    if (textnote != string.Empty)
        //                    {
        //                        foreach (var item in TextNote)
        //                        {
        //                            Debug.WriteLine($"Value Textnote : {textnote.ToLower()} , Value so sánh : {item} ,  Value Bool = {TextNote.Any(c => textnote.ToLower().ToString() == c.ToLower().ToString())}");
        //                        }

        //                    }

        //                    if (!string.IsNullOrEmpty(textnote) && TextNote.Any(c => textnote.ToLower().ToString() == c.ToLower().ToString()))
        //                    {
        //                        string bounds = node.Attributes["bounds"]?.InnerText; // Giả sử bounds = "[x1,y1][x2,y2]"
        //                        if (!string.IsNullOrEmpty(bounds))
        //                        {
        //                            string[] points = bounds.Replace("][", ",").Replace("]", "").Replace("[", "").Split(','); // Tách chuỗi để lấy các giá trị x và y

        //                            int x1 = int.Parse(points[0]);
        //                            int y1 = int.Parse(points[1]);
        //                            int x2 = int.Parse(points[2]);
        //                            int y2 = int.Parse(points[3]);

        //                            int centerX = (x1 + x2) / 2;
        //                            int centerY = (y1 + y2) / 2;
        //                            if (click)
        //                            {
        //                                Debug.WriteLine($"DumpyHelpClick: {centerX},{centerY}");
        //                                device.Click(new Point(centerX, centerY));
        //                                await Task.Delay(300);
        //                                return true;
        //                            }
        //                            else
        //                            {
        //                                await Task.Delay(300);
        //                                return true;
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return false;
        //}
        //public static async Task CloseAllAppEmulator(Device device)
        //{
        //    if (!string.IsNullOrEmpty(device.Serial))
        //    {

        //        if (device.EmulatorType == EmulatorType.MEmu)
        //        {
        //            await Task.Delay(1000);
        //            string cmdCommand = $"adb -s {device.Serial} shell input keyevent KEYCODE_APP_SWITCH";
        //            await CMD.ExecuteAdbAsync(cmdCommand);
        //            await Task.Delay(1000);

        //            var mid = (device.ScreenWidth - 41) / 2;
        //            var x1 = mid - 60;
        //            var x2 = (device.ScreenWidth - 41) - 60;
        //            int y1 = (device.ScreenHeight - 100);
        //            device.Swipe(new Point(x1, y1), new Point(x2, y1), 150);
        //            await Task.Delay(1000);
        //        }
        //        else
        //        {
        //            //adb shell input keyevent KEYCODE_APP_SWITCH
        //            string cmdCommand = $"adb -s {device.Serial} shell input keyevent KEYCODE_APP_SWITCH";
        //            await CMD.ExecuteAdbAsync(cmdCommand);
        //            await Task.Delay(1000);

        //            await DumpyHelpClick(device, new List<string> { "CLEAR ALL", "xóa tất cả" }, "CloseAppBlueStack", true);
        //            await Task.Delay(500);
        //        }


        //        //await Helper.HOME(device.Serial);
        //        //await Task.Delay(500);

        //    }
        //}
        //public static async Task CloseAllApp(string deviceID, string numapp = "2")
        //{
        //    if (!string.IsNullOrEmpty(deviceID))
        //    {

        //        string cmdCommand = $"adb -s {deviceID} shell input keyevent KEYCODE_APP_SWITCH";
        //        await CMD.ExecuteAdbAsync(cmdCommand);
        //        await NamedPipeServerInstance.Delay(deviceID, 1000);

        //        ////await PHNAuto.DoWork(deviceID, model,"TimAnhClick:CloseAppD6603|CatNgang|1/3|3|280,199,568,526|20|300:tieptuc:DungHam");
        //        //// RealMouseKeybroad.Click_Potion(deviceID, 945, 1437, 1);
        //        //await CMD.ExecuteAdbAsync($"adb -s {deviceID} shell input tap 945 1437");
        //        //await NamedPipeServerInstance.Delay(deviceID, 300);

        //        string cmdCommandv2 = $"adb -s {deviceID} shell dumpsys activity recents | findstr \"TaskRecord\"";
        //        var listvalue = await CMD.ExecuteAdbAsync(cmdCommandv2);
        //        for (int i = 0; i < listvalue.Count() - Convert.ToInt32(numapp); i++)
        //        {
        //            string cmdCommandv3 = $"adb -s {deviceID} shell input keyevent KEYCODE_DPAD_DOWN";
        //            await CMD.ExecuteAdbAsync(cmdCommandv3);
        //            await NamedPipeServerInstance.Delay(deviceID, 1000);

        //            string cmdCommandv4 = $"adb -s {deviceID} shell input keyevent DEL";
        //            await CMD.ExecuteAdbAsync(cmdCommandv4);
        //            await NamedPipeServerInstance.Delay(deviceID, 1000);
        //        }

        //        await HOME(deviceID);
        //    }
        //}
        //public static async Task ForceStopApp(string deviceID, string appname)
        //{
        //    if (!string.IsNullOrEmpty(deviceID))
        //    {
        //        string cmdCommand = $"adb -s {deviceID} shell am force-stop {appname}";
        //        await CMD.ExecuteAdbAsync(cmdCommand);
        //    }

        //}
        public static async Task<string?> ClearCacheApp(string deviceID, string appname)
        {
            if (!string.IsNullOrEmpty(deviceID))
            {
                string cmdCommand = $"adb -s {deviceID} shell su -c 'rm -rf /data/data/{appname}/cache/*'";
                return await CMD.ExecuteAdbAsync(cmdCommand);
            }
            return null;
        }
        public static async Task<string?> ClearApp(string deviceID, string appname)
        {
            if (!string.IsNullOrEmpty(deviceID))
            {
                string cmdCommand = $"adb -s {deviceID} shell pm clear {appname}";
                return await CMD.ExecuteAdbAsync(cmdCommand);
            }
            return null;
        }
        public static async Task<string?> CheckAppVersion(string deviceID, string appname)
        {
            if (!string.IsNullOrEmpty(deviceID))
            {
                string cmdCommand = $"adb -s {deviceID} shell dumpsys package {appname} | findstr versionName";
                var stringVersion = await CMD.ExecuteAdbAsync(cmdCommand);
                return stringVersion;
            }
            return null;
        }
        public static async Task PowerOff(string deviceID)
        {
            if (!string.IsNullOrEmpty(deviceID))
            {
                string cmdCommand = $"adb -s {deviceID} shell reboot -p";
                await CMD.ExecuteAdbAsync(cmdCommand);


            }
        }
        public static async Task RebootRecovery(string deviceID)
        {
            if (!string.IsNullOrEmpty(deviceID))
            {
                string cmdCommand = $"adb -s {deviceID} reboot recovery";
                await CMD.ExecuteAdbAsync(cmdCommand);
            }
        }
        public static async Task HOME(string deviceID)
        {
            if (!string.IsNullOrEmpty(deviceID))
            {
                string cmdCommand = $"adb -s {deviceID} shell input keyevent KEYCODE_HOME";
                await CMD.ExecuteAdbAsync(cmdCommand);
            }
        }
        public static async Task APP_SWITCH(string deviceID)
        {
            if (!string.IsNullOrEmpty(deviceID))
            {
                string cmdCommand = $"adb -s {deviceID} shell input keyevent KEYCODE_APP_SWITCH";
                await CMD.ExecuteAdbAsync(cmdCommand);
            }
        }
        public static async Task BACK(string deviceID)
        {
            if (!string.IsNullOrEmpty(deviceID))
            {
                string cmdCommand = $"adb -s {deviceID} shell input keyevent KEYCODE_BACK";
                await CMD.ExecuteAdbAsync(cmdCommand);
            }
        }
        public static async Task VOLUME_UP(string deviceID)
        {
            if (!string.IsNullOrEmpty(deviceID))
            {
                string cmdCommand = $"adb -s {deviceID} shell input keyevent KEYCODE_VOLUME_UP";
                await CMD.ExecuteAdbAsync(cmdCommand);
            }
        }
        public static async Task VOLUME_DOWN(string deviceID)
        {
            if (!string.IsNullOrEmpty(deviceID))
            {
                string cmdCommand = $"adb -s {deviceID} shell input keyevent KEYCODE_VOLUME_DOWN";
                await CMD.ExecuteAdbAsync(cmdCommand);
            }
        }
        public static async Task<List<string>?> getFilesDevice(string deviceID, string path)
        {
            if (!string.IsNullOrEmpty(deviceID))
            {
                string cmdCommand = $"adb -s {deviceID} shell su -c ls";

                cmdCommand = !string.IsNullOrEmpty(path) ? cmdCommand += $" {path}" : cmdCommand;
                try
                {
                    var stringlist = await CMD.ExecuteAdbAsync(cmdCommand);
                    if (stringlist != null && stringlist.Length > 0)
                    {
                        var filess = stringlist.Split("\r\n");
                        List<string> list = new List<string>();
                        foreach (string file in filess)
                        {
                            if (!string.IsNullOrEmpty(file) && !file.Contains("adb"))
                            {
                                list.Add(file);
                            }
                        }

                        return list;

                    }
                }
                catch (Exception)
                { }
            }
            return null;
        }
        public static async Task<List<string>?> TimfileByName(string deviceID, string tenFile)
        {
            if (!string.IsNullOrEmpty(deviceID))
            {
                string cmdCommand = $"adb -s {deviceID} shell su -c find . -name \"*{tenFile}*\"";
                try
                {
                    var stringlist = await CMD.ExecuteAdbAsync(cmdCommand);
                    if (stringlist != null && stringlist.Length > 0)
                    {
                        var filess = stringlist.Split("\r\n");
                        List<string> list = new List<string>();
                        foreach (string file in filess)
                        {
                            if (!string.IsNullOrEmpty(file) && !file.Contains("find:"))
                            {
                                list.Add(file);
                            }
                        }

                        return list;

                    }
                }
                catch (Exception)
                { }
            }
            return null;
        }
        public static async Task<bool> PushFileToDevice(string deviceID, string localFilePath, string remoteFilePath)
        {
            var dir = Directory.GetCurrentDirectory();
            var filepath = localFilePath.Contains(":") ? localFilePath : $"{dir}\\{localFilePath}";

            string cmdCommand = $"adb -s {deviceID} push \"{filepath}\" \"{remoteFilePath}\"";
            var value = await CMD.ExecuteAdbAsync(cmdCommand);
            if (!string.IsNullOrEmpty(value) && value.ToLower().Contains("error"))
            {
                return false;
            }
            return true;
        }
        public static async Task<string?> CopyFileAndroidRoot(string deviceID, string localFilePath, string remoteFilePath)
        {
            string cmdCommand = $"adb -s {deviceID} shell \"su -c cp {localFilePath} {remoteFilePath}\"";
            return await CMD.ExecuteAdbAsync(cmdCommand);
        }
        public static async Task<string?> CopyFileAndroid(string deviceID, string localFilePath, string remoteFilePath)
        {
            // Escape spaces in the file paths
            string escapedLocalFilePath = localFilePath.Replace(" ", "\\ ");
            string escapedRemoteFilePath = remoteFilePath.Replace(" ", "\\ ");

            string cmdCommand = $"adb -s {deviceID} shell \"cp -f {escapedLocalFilePath} {escapedRemoteFilePath}\"";
            return await CMD.ExecuteAdbAsync(cmdCommand);
        }
        public static async Task<bool> PushImage(string deviceID, string localFilePath)
        {
            if (File.Exists(localFilePath))
            {
                var dir = Directory.GetCurrentDirectory();
                await CMD.ExecuteAdbAsync($"adb -s {deviceID} shell rm -f /sdcard/Pictures/Images/*");

                await CMD.ExecuteAdbAsync($"adb -s {deviceID} shell mkdir /sdcard/Picture/");
                await CMD.ExecuteAdbAsync($"adb -s {deviceID} shell mkdir /sdcard/Picture/Images");

                //sdcard/DCIM/Camera/
                string imageName = Path.GetFileName(localFilePath);
                var remoteFilePath = $"sdcard/Picture/Images/{imageName}";

                var filepath = localFilePath.Contains(":") ? localFilePath : $"{dir}\\{localFilePath}";
                string cmdCommand = $"adb -s {deviceID} push \"{filepath}\" \"{remoteFilePath}\"";
                var valk = await CMD.ExecuteAdbAsync(cmdCommand);
                if (valk != null && valk != string.Empty && valk.ToLower().Contains("error"))
                {
                    return false;
                }
                else
                {
                    string path = remoteFilePath;
                    if (path[0] == '/')
                    {
                        path = path.TrimStart('/');
                    }
                    await CMD.ExecuteAdbAsync($"adb -s {deviceID} shell am broadcast -a android.intent.action.MEDIA_SCANNER_SCAN_FILE -d file:///{path}");

                    return true;
                }

            }
            return false;

        }
        public static async Task<bool> PushImageRandom(string deviceID, string localPath)
        {
            if (Directory.Exists(localPath))
            {
                var imageFormats = new string[] { "*.jpg", "*.jpeg", "*.png", "*.gif", "*.tif", "*.tiff" };
                var listFiles = imageFormats.SelectMany(format => Directory.GetFiles(localPath, format)).ToArray();
                if (listFiles != null && listFiles.Count() > 0)
                {
                    var index = new Random().Next(0, listFiles.Count());
                    string localFilePath = listFiles[index];

                    var dir = Directory.GetCurrentDirectory();
                    await CMD.ExecuteAdbAsync($"adb -s {deviceID} shell rm -f /sdcard/Pictures/Images/*");
                    await CMD.ExecuteAdbAsync($"adb -s {deviceID} shell mkdir /sdcard/Picture/");
                    await CMD.ExecuteAdbAsync($"adb -s {deviceID} shell mkdir /sdcard/Picture/Images");

                    //sdcard/DCIM/Camera/
                    string imageName = Path.GetFileName(localFilePath);
                    var remoteFilePath = $"sdcard/Picture/Images/{imageName}";

                    var filepath = localFilePath.Contains(":") ? localFilePath : $"{dir}\\{localFilePath}";
                    string cmdCommand = $"adb -s {deviceID} push \"{filepath}\" \"{remoteFilePath}\"";
                    var valk = await CMD.ExecuteAdbAsync(cmdCommand);
                    if (valk != null && valk != string.Empty && valk.ToLower().Contains("error"))
                    {
                        return false;
                    }
                    else
                    {
                        string path = remoteFilePath;
                        if (path[0] == '/')
                        {
                            path = path.TrimStart('/');
                        }
                        await CMD.ExecuteAdbAsync($"adb -s {deviceID} shell am broadcast -a android.intent.action.MEDIA_SCANNER_SCAN_FILE -d file:///{path}");

                        return true;
                    }
                }
            }

            return false;
        }
        public static async Task PushImageOrVideo(string deviceID, string localFilePath)
        {
            var dir = Directory.GetCurrentDirectory();
            await CMD.ExecuteAdbAsync($"adb -s {deviceID} shell su -c mkdir -p /storage/emulated/0/DCIM/Camera/");
            //sdcard/DCIM/Camera/
            string imageName = Path.GetFileNameWithoutExtension(localFilePath);
            var remoteFilePath = $"sdcard/DCIM/Camera/{imageName}.png";

            var filepath = localFilePath.Contains(":") ? localFilePath : $"{dir}\\{localFilePath}";
            string cmdCommand = $"adb -s {deviceID} push \"{filepath}\" \"{remoteFilePath}\"";
            await CMD.ExecuteAdbAsync(cmdCommand);

            string path = remoteFilePath;
            if (path[0] == '/')
            {
                path = path.TrimStart('/');
            }
            await CMD.ExecuteAdbAsync($"adb -s {deviceID} shell am broadcast -a android.intent.action.MEDIA_SCANNER_SCAN_FILE -d file:///{path}");
        }
        public static async Task<string?> PullFileFromDevice(string deviceID, string remoteFilePath, string localFilePath)
        {
            var dir = Directory.GetCurrentDirectory();
            var filepath = localFilePath.Contains(":") ? localFilePath : $"{dir}\\{localFilePath}";

            string cmdCommand = $"adb -s {deviceID} pull \"{remoteFilePath}\" \"{filepath}\"";
            return await CMD.ExecuteAdbAsync(cmdCommand);
        }

        public static Bitmap ReplaceNonBlackWithWhite(Bitmap bitmap)
        {
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    Color pixelColor = bitmap.GetPixel(x, y);
                    if (pixelColor.R > 46 || pixelColor.G > 46 || pixelColor.B > 46)
                    {
                        bitmap.SetPixel(x, y, Color.White);
                    }
                }
            }

            return bitmap;
        }
        public static Bitmap Replace_Non_White_With_Black(Bitmap bitmap)
        {
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    Color pixelColor = bitmap.GetPixel(x, y);
                    if (pixelColor.R < 160 || pixelColor.G < 160 || pixelColor.B < 160)
                    {
                        bitmap.SetPixel(x, y, Color.White);
                    }
                }
            }

            return bitmap;
        }

        public static async Task TurnOnWifiRoot(string deviceID)
        {
            if (!string.IsNullOrEmpty(deviceID))
            {
                string cmdCommand = $"adb -s {deviceID} shell su -c 'svc wifi enable'";
                await CMD.ExecuteAdbAsync(cmdCommand);
            }
        }
        public static async Task TurnOffWifiRoot(string deviceID)
        {
            if (!string.IsNullOrEmpty(deviceID))
            {
                string cmdCommand = $"adb -s {deviceID} shell su -c 'svc wifi disable'";
                await CMD.ExecuteAdbAsync(cmdCommand);
            }
        }
        public static async Task BatteryRandom(string deviceID)
        {
            if (!string.IsNullOrEmpty(deviceID))
            {
                Random random = new Random();
                string cmdCommand = $"adb -s {deviceID} battery set level {random.Next(30, 99)}";
                await CMD.ExecuteAdbAsync(cmdCommand);
            }
        }
        public static async Task Reboot(string deviceID)
        {
            if (!string.IsNullOrEmpty(deviceID))
            {
                Random random = new Random();
                string cmdCommand = $"adb -s {deviceID} reboot";
                await CMD.ExecuteAdbAsync(cmdCommand);

                await WaitForDeviceBootCompletedAsync(deviceID);
            }
        }
        public static async Task<bool> IsDeviceBootCompletedAsync(string deviceId)
        {
            try
            {
                string cmdCommand = $"adb -s {deviceId} shell getprop sys.boot_completed";
                string? result = await CMD.ExecuteAdbAsync(cmdCommand);
                if (!string.IsNullOrEmpty(result))
                {
                    return result.Trim() == "1";
                }
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error checking boot status for device {deviceId}: {ex.Message}");
                return false;
            }
        }
        public static async Task WaitForDeviceBootCompletedAsync(string deviceId)
        {
            while (!await IsDeviceBootCompletedAsync(deviceId))
            {
                await Task.Delay(1000);
            }
        }
        public static async Task RebootToRecovery(string deviceID)
        {
            if (!string.IsNullOrEmpty(deviceID))
            {
                string cmdCommand = $"adb -s {deviceID} reboot recovery";
                await CMD.ExecuteAdbAsync(cmdCommand);
            }
        }
        public static async Task ShutDown(string deviceID)
        {
            if (!string.IsNullOrEmpty(deviceID))
            {
                string cmdCommand = $"adb -s {deviceID} shell reboot -p";
                await CMD.ExecuteAdbAsync(cmdCommand);
            }
        }
        public static async Task<bool> SetClipBroad(string deviceID, string text)
        {
            if (!string.IsNullOrEmpty(deviceID))
            {
                // Thoát tất cả các ký tự đặc biệt trong `text`
                string escapedText = EscapeSpecialCharacters(text);
                var value = await CMD.ExecuteAdbAsync($"adb -s {deviceID} shell am broadcast -a clipper.set -e text '{escapedText}'");
                if (value != null && value != string.Empty && value.ToLower().Contains("is copied into clipboard"))
                {
                    return true;
                }
            }

            return false;
        }
        private static string EscapeSpecialCharacters(string input)
        {
            // Thay thế các ký tự đặc biệt bằng cách sử dụng các chuỗi thoát
            var stringBuilder = new StringBuilder(input);
            stringBuilder.Replace("\\", "\\\\");
            stringBuilder.Replace("\"", "\\\"");
            stringBuilder.Replace("'", "\\'");
            stringBuilder.Replace("&", "\\&");
            stringBuilder.Replace("|", "\\|");
            stringBuilder.Replace(";", "\\;");
            stringBuilder.Replace("<", "\\<");
            stringBuilder.Replace(">", "\\>");
            stringBuilder.Replace("*", "\\*");
            stringBuilder.Replace("?", "\\?");
            stringBuilder.Replace("$", "\\$");
            stringBuilder.Replace("`", "\\`");
            stringBuilder.Replace("!", "\\!");
            stringBuilder.Replace("@", "\\@");
            stringBuilder.Replace("#", "\\#");
            stringBuilder.Replace("%", "\\%");
            stringBuilder.Replace("^", "\\^");
            stringBuilder.Replace("(", "\\(");
            stringBuilder.Replace(")", "\\)");
            stringBuilder.Replace("[", "\\[");
            stringBuilder.Replace("]", "\\]");
            stringBuilder.Replace("{", "\\{");
            stringBuilder.Replace("}", "\\}");
            stringBuilder.Replace(":", "\\:");
            stringBuilder.Replace(",", "\\,");
            stringBuilder.Replace("=", "\\=");
            stringBuilder.Replace("+", "\\+");
            stringBuilder.Replace("-", "\\-");
            stringBuilder.Replace(".", "\\.");
            stringBuilder.Replace("/", "\\/");
            // Thêm các ký tự đặc biệt khác cần thoát ở đây

            return stringBuilder.ToString();
        }

        public static async Task PlanModeON(string deviceID)
        {
            // //adb shell settings put global airplane_mode_on 1
            string cmdClearShoppe = "adb -s " + deviceID + " shell settings put global airplane_mode_on 1";
            cmdClearShoppe += Environment.NewLine + "adb -s " + deviceID + " shell am broadcast -a android.intent.action.AIRPLANE_MODE";
            await CMD.ExecuteAdbAsync(cmdClearShoppe);
        }
        public static async Task PlanModeOFF(string deviceID)
        {
            string cmdClearShoppe = "adb -s " + deviceID + " shell settings put global airplane_mode_on 0";
            cmdClearShoppe += Environment.NewLine + "adb -s " + deviceID + " shell am broadcast -a android.intent.action.AIRPLANE_MODE";
            await CMD.ExecuteAdbAsync(cmdClearShoppe);
        }

    }
    public class FileHelper
    {
        // Danh sách extension mặc định (ignore case)
        private static readonly HashSet<string> FileExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            // Gói ứng dụng & code
            ".aab", ".apk", ".class", ".dex", ".jar", ".oat", ".odex",

            // Thư viện & native code
            ".elf", ".ko", ".o", ".obj", ".so",

            // Hệ thống & firmware
            ".bin", ".dat", ".fstab", ".img", ".mbn",

            // Script & cấu hình
            ".conf", ".ini", ".json", ".prop", ".rc", ".sh", ".xml", ".yaml", ".yml",

            // Dữ liệu & database
            ".db", ".csv", ".json", ".kv", ".log", ".sqlite", ".txt", ".xml",

            // Nén & lưu trữ
            ".7z", ".jar", ".rar", ".tar", ".tar.bz2", ".tar.gz", ".tgz", ".xz", ".zip", ".lzma",

            // Media - Ảnh
            ".bmp", ".gif", ".jpg", ".jpeg", ".png", ".raw", ".tif", ".webp",

            // Media - Âm thanh
            ".aac", ".flac", ".m4a", ".mp3", ".ogg", ".wav",

            // Media - Video
            ".3gp", ".avi", ".mkv", ".mov", ".mp4", ".mpeg", ".webm",

            // Media - Subtitle
            ".ass", ".srt", ".vtt",

            // Phông chữ & chứng chỉ
            ".cer", ".crt", ".key", ".otf", ".pem", ".ttf",

            // Khác
            ".avb", ".dtbo", ".obb", ".vbmeta"
        };

        // Trả về true nếu tên có extension nằm trong danh sách
        public static bool LooksLikeFile(string name)
        {
            var ext = Path.GetExtension(name);
            return !string.IsNullOrEmpty(ext) && FileExtensions.Contains(ext);
        }
        public static bool IsApkFile(string filePath)
        {
            return Path.GetExtension(filePath).Equals(".apk", StringComparison.OrdinalIgnoreCase);
        }
        public static bool IsImageFile(string filePath)
        {
            string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".tiff" };
            string fileExtension = Path.GetExtension(filePath).ToLower();
            return Array.Exists(imageExtensions, ext => ext == fileExtension);
        }

        public static bool IsMediaFile(string filePath)
        {
            string[] videoExtensions = { ".mp4", ".avi", ".mov", ".mkv", ".wmv", ".flv", ".webm" };

            string fileExtension = Path.GetExtension(filePath).ToLower();
            return Array.Exists(videoExtensions, ext => ext == fileExtension);
        }

        public static bool IsValidFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                FileAttributes attributes = File.GetAttributes(filePath);
                return !attributes.HasFlag(FileAttributes.Directory); // Kiểm tra xem nó có phải thư mục không
            }
            return false;
        }
    }

}
