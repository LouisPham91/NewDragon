using AdvancedSharpAdbClient;
using Dragon.Controller.GlobalControl.Helper;
using Dragon.Controller.TaskDeviceManager.Core;
using Dragon.Controller.TaskDeviceManager.Infrastructure;
using Dragon.Controller.TaskDeviceManager.Model.File;
using Dragon.Controller.TaskDeviceManager.Model.Mouse;
using Dragon.Database.Models;

namespace Dragon.Controller.TaskDeviceManager.Runners
{
    public class FileRunner
    {
        private static string BuildCopyCommand(bool hasSu, string sourcePath, string destinationPath)
        {
            string baseCommand = $"cp {sourcePath} {destinationPath}";
            return hasSu ? $"su -c '{baseCommand}'" : baseCommand;
        }

        public static async Task<bool> IsDeviceRooted(string serial)
        {
            try
            {
                string whichSu = await CMD.ExecuteAdbAsync($"adb -s {serial} shell which su");
                if (string.IsNullOrWhiteSpace(whichSu) || !whichSu.ToLower().Contains("su"))
                    return false;

                string suId = await CMD.ExecuteAdbAsync($"adb -s {serial} shell su -c id");
                return !string.IsNullOrWhiteSpace(suId) && suId.Contains("uid=0");
            }
            catch
            {
                return false;
            }
        }

        private static string EnsureFullPath(string path)
        {
            if (Path.IsPathRooted(path)) return path;
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            return Path.GetFullPath(Path.Combine(baseDir, path));
        }

        public static async Task<NodeResult> RunCopyFiles(DLoopContext ctx, FileArgs fileArgs)
        {
            var token = ctx.Token;
            var phone = ctx.Session.Phone;
            var device = ctx.DeviceData;
            var atx = ctx.Atx;
            var logKey = ctx.LogKey;

            if (token.IsCancellationRequested) return NodeResult.Stop();
            if (phone == null) return NodeResult.Fail("phone null");

            if (fileArgs.hasSu)
            {
                bool IsSU = await IsDeviceRooted(phone.Serial);
                if (!IsSU)
                {
                    Logger.Notify(logKey, $"Device {phone.Serial} không có root", Logger.Icon.Warning);
                    return NodeResult.Fail("No root");
                }
            }

            fileArgs.SourcePath = EnsureFullPath(fileArgs.SourcePath);
            fileArgs.DestinationPath = EnsureFullPath(fileArgs.DestinationPath);

            try
            {
                if (fileArgs.ControlMode == ControlMode.ADB 
                    || fileArgs.ControlMode == ControlMode.ADBEvent
                    || fileArgs.ControlMode == ControlMode.Scrcpy
                    || fileArgs.ControlMode == ControlMode.HDI)
                {
                    string value = "";

                    if (fileArgs.CopyFileOperation == CopyFileOperation.Pull)
                    {
                        if (fileArgs.hasSu)
                        {
                            string tempPath = "/data/local/tmp/tmpfile";
                            string commandCopy = BuildCopyCommand(true, fileArgs.SourcePath, tempPath);
                            await CMD.ExecuteAdbAsync($"adb -s {device.Serial} shell {commandCopy}");
                            value = await CMD.ExecuteAdbAsync($"adb -s {device.Serial} pull \"{tempPath}\" \"{fileArgs.DestinationPath}\"");
                        }
                        else
                        {
                            value = await CMD.ExecuteAdbAsync($"adb -s {device.Serial} pull \"{fileArgs.SourcePath}\" \"{fileArgs.DestinationPath}\"");
                        }
                    }
                    else if (fileArgs.CopyFileOperation == CopyFileOperation.Push)
                    {
                        if (fileArgs.hasSu)
                        {
                            string tempPath = "/data/local/tmp/tmpfile";
                            value = await CMD.ExecuteAdbAsync($"adb -s {device.Serial} push \"{fileArgs.SourcePath}\" \"{tempPath}\"");
                            if (!value.Contains("bytes in") && !value.Contains("file pushed"))
                                return NodeResult.Fail("Push failed");
                            string commandCopy = BuildCopyCommand(true, tempPath, fileArgs.DestinationPath);
                            await CMD.ExecuteAdbAsync($"adb -s {device.Serial} shell {commandCopy}");
                        }
                        else
                        {
                            value = await CMD.ExecuteAdbAsync($"adb -s {device.Serial} push \"{fileArgs.SourcePath}\" \"{fileArgs.DestinationPath}\"");
                        }
                    }
                    else if (fileArgs.CopyFileOperation == CopyFileOperation.FileInAndroid)
                    {
                        string commandCopy = BuildCopyCommand(fileArgs.hasSu, fileArgs.SourcePath, fileArgs.DestinationPath);
                        value = await CMD.ExecuteAdbAsync($"adb -s {device.Serial} shell {commandCopy}");
                    }

                    if (fileArgs.isArmBroadCast && !string.IsNullOrEmpty(fileArgs.ArmBroadCommand))
                    {
                        var giatri = await CMD.ExecuteAdbAsync($"adb -s {device.Serial} shell {fileArgs.ArmBroadCommand}");
                        Logger.Notify(logKey, $"arm broadcast: {giatri}", Logger.Icon.Information);
                    }

                    Logger.Notify(logKey, $"Output: {value}", Logger.Icon.Information);

                    if (value.Contains("Read-only file system") || value.Contains("failed") || value.Contains("error"))
                        return NodeResult.Fail("Copy error");

                    return NodeResult.Ok();
                }
                else if (fileArgs.ControlMode == ControlMode.ATX)
                {
                    if (atx == null) return NodeResult.Fail("ATX null");

                    bool isSuccess = false;

                    if (fileArgs.CopyFileOperation == CopyFileOperation.Pull)
                        isSuccess = await atx.PullAsync(fileArgs.SourcePath, fileArgs.DestinationPath, logKey);
                    else if (fileArgs.CopyFileOperation == CopyFileOperation.Push)
                        isSuccess = await atx.PushAsync(fileArgs.SourcePath, fileArgs.DestinationPath, 493, logKey);
                    else if (fileArgs.CopyFileOperation == CopyFileOperation.FileInAndroid)
                        isSuccess = await atx.CopyAsync(fileArgs.SourcePath, fileArgs.DestinationPath, logKey);

                    if (fileArgs.isArmBroadCast && !string.IsNullOrEmpty(fileArgs.ArmBroadCommand))
                        isSuccess = await atx.ArmBroadCastAsync(fileArgs.ArmBroadCommand, logKey);

                    return isSuccess ? NodeResult.Ok() : NodeResult.Fail("ATX copy failed");
                }

                return NodeResult.Fail("Unsupported CopyFileRunType");
            }
            catch (Exception ex)
            {
                Logger.Notify(logKey, $"Exception: {ex.Message}", Logger.Icon.Error);
                return NodeResult.Fail(ex.Message);
            }
        }
    }
}