using AdvancedSharpAdbClient;
using AdvancedSharpAdbClient.Receivers;
using Dragon.Controller.TaskDeviceManager.Core;
using Dragon.Controller.TaskDeviceManager.Infrastructure;
using Dragon.Controller.TaskDeviceManager.Infrastructure.Services;
using Dragon.Controller.TaskDeviceManager.Model.Input;
using Dragon.Controller.TaskDeviceManager.Model.Mouse;
using System.Text.RegularExpressions;

namespace Dragon.Controller.TaskDeviceManager.Runners
{
    public class IntentRunner
    {
 
        public static string removeADBShell(string fullCommand)
        {
            if (string.IsNullOrWhiteSpace(fullCommand))
                return fullCommand;

            var cmd = fullCommand.Trim();

            // không phải adb -> trả về luôn
            if (!cmd.StartsWith("adb", StringComparison.OrdinalIgnoreCase))
                return cmd;

            int i = 3; // sau "adb"

            // bỏ khoảng trắng
            while (i < cmd.Length && char.IsWhiteSpace(cmd[i])) i++;

            // --- có "-s DEVICE" không? ---
            if (i + 1 < cmd.Length &&
                cmd[i] == '-' && (cmd[i + 1] == 's' || cmd[i + 1] == 'S'))
            {
                i += 2;
                while (i < cmd.Length && char.IsWhiteSpace(cmd[i])) i++;

                // bỏ deviceId (đến khoảng trắng tiếp)
                while (i < cmd.Length && !char.IsWhiteSpace(cmd[i])) i++;
                while (i < cmd.Length && char.IsWhiteSpace(cmd[i])) i++;
            }

            // --- có "shell" không? ---
            if (i + 4 < cmd.Length &&
                cmd.AsSpan(i, 5).Equals("shell", StringComparison.OrdinalIgnoreCase))
            {
                i += 5;
                while (i < cmd.Length && char.IsWhiteSpace(cmd[i])) i++;
            }

            return i < cmd.Length ? cmd[i..] : string.Empty;
        }
        public static async Task<NodeResult> ExecutePressKeyAsync(DLoopContext ctx, KeyPressArgs args)
        {
            try
            {
                string command = args.IsCustom ? removeADBShell(args.Command) : KeyCodeService.GetCommand(args.Name);

                switch (args.ControlMode)
                {
                    case ControlMode.HDI:
                    case ControlMode.Scrcpy:
                    case ControlMode.ADBEvent:
                    case ControlMode.ADB:
                        var receiver = new ConsoleOutputReceiver();
                        await ctx.AdbClient.ExecuteRemoteCommandAsync(command, ctx.DeviceData, receiver, ctx.Token);

                        Logger.Notify(ctx.LogKey, $"ADB: {args.Name} => {receiver}", Logger.Icon.Information);
                        return NodeResult.Ok(receiver.ToString());

                    case ControlMode.ATX:
                        if (ctx.Atx == null)
                            return NodeResult.Fail("ATX not connected");
                        if (args.IsCustom)
                        {
                            var result = await ctx.Atx.ShellAsync(command);
                            Logger.Notify(ctx.LogKey, $"ATX: {result}", Logger.Icon.Information);
                        }
                        else
                        {
                            await ctx.Atx.KeyPress(ctx, args.Name);
                        }
                        return NodeResult.Ok();

                    case ControlMode.ACC:
                        return NodeResult.Fail("ACC chưa hỗ trợ");

                    default:
                        return NodeResult.Fail("Mode không hợp lệ");
                }
            }
            catch (Exception ex)
            {
                Logger.Notify(ctx.LogKey, $"Error: {ex.Message}", Logger.Icon.Error);
                return NodeResult.Fail(ex.Message);
            }
        }
    }
}