
using Dragon.Controller.TaskDeviceManager.Core;
using Dragon.Controller.TaskDeviceManager.Infrastructure;
using Dragon.Controller.TaskDeviceManager.Infrastructure.Services;
using Dragon.Controller.TaskDeviceManager.Model.DatabaseColumn;
using Dragon.Controller.TaskDeviceManager.Model.HttpResponse;
using Dragon.Controller.TaskDeviceManager.Model.Input;
using Dragon.Controller.TaskDeviceManager.Model.Mouse;
using Dragon.Database.Models;
using System.Collections.Concurrent;
using System.Text;

namespace Dragon.Controller.GlobalControl.TaskDeviceManager.Runners
{
    public static class InputRunner
    {
        public static ConcurrentDictionary<string, string> RegexNexValeList = new();

        private static string EncodeForAdb(string text)
        {
            var sb = new StringBuilder();
            foreach (char c in text)
            {
                if (c == ' ') sb.Append("%s");
                else if (char.IsLetterOrDigit(c)) sb.Append(c);
                else sb.Append('\\').Append(c);
            }
            return sb.ToString();
        }

        public static string GetNextValueForPhone(Phone phone) 
        {
            if (phone == null) return string.Empty;
            RegexNexValeList.TryGetValue(phone.DeviceID, out var v);
            if (string.IsNullOrEmpty(v)) return string.Empty;
            return v;
        }
        public static bool SetNextValueForPhone(Phone phone, string value) 
        { 
            if (phone == null) return false; 
            RegexNexValeList.AddOrUpdate(phone.DeviceID, value ?? "", (k, o) => value ?? ""); 
            return true; 
        }
        public static void ClearNextValueForPhone(Phone phone) 
        { 
            if (phone != null) 
                RegexNexValeList.TryRemove(phone.DeviceID, out _); 
        }
        public static void ClearAllNextValues() => RegexNexValeList.Clear();

        public static async Task<NodeResult> SendLongText(DLoopContext ctx, string text)
        {
            if (ctx.DeviceData == null) { Logger.Notify(ctx.LogKey, "deviceData null", Logger.Icon.Error); return NodeResult.Fail("deviceData null"); }
            if (ctx.Token.IsCancellationRequested) { Logger.Notify(ctx.LogKey, "Token hủy SendText", Logger.Icon.Warning); return NodeResult.Stop(); }

            var adb = ctx.AdbClient;
            int chunk = 50;
            var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var sb = new StringBuilder(); int count = 0;
            foreach (var w in words)
            {
                if (count >= chunk)
                {
                    await adb.ExecuteRemoteCommandAsync($"input text {EncodeForAdb(sb.ToString())}", ctx.DeviceData, ctx.Token);
                    await Logger.DelayAsync(50, ctx.Token, ctx.LogKey);
                    sb.Clear(); count = 0;
                }
                if (sb.Length > 0) sb.Append(" ");
                sb.Append(w); count++;
            }
            if (sb.Length > 0) await adb.ExecuteRemoteCommandAsync($"input text {EncodeForAdb(sb.ToString())}", ctx.DeviceData, ctx.Token);
            return NodeResult.Ok();
        }

        public static async Task<NodeResult> SendText(DLoopContext ctx, string text)
        {
            try
            {
                if (ctx.Token.IsCancellationRequested) { Logger.Notify(ctx.LogKey, "Token hủy", Logger.Icon.Warning); return NodeResult.Stop(); }
                if (ctx.DeviceData == null) { Logger.Notify(ctx.LogKey, "deviceData null", Logger.Icon.Error); return NodeResult.Fail("deviceData null"); }
                if (text.Length > 1000) return await SendLongText(ctx, text);
                await ctx.AdbClient.ExecuteRemoteCommandAsync($"input text {EncodeForAdb(text)}", ctx.DeviceData, ctx.Token);
                Logger.Notify(ctx.LogKey, $"Đã gửi text: {text}", Logger.Icon.Information);
                return NodeResult.Ok();
            }
            catch (Exception ex) { Logger.Notify(ctx.LogKey, $"Lỗi SendText: {ex.Message}", Logger.Icon.Error); return NodeResult.Fail(ex.Message); }
        }

        public static async Task<NodeResult> Run_InputText_Test(DLoopContext ctx, SendTextArgs input, int delayChangeIMEIID = 2000)
        {

            var text = await LanguageInputService.ConvertText(input.Text, ctx.Session.Phone.DeviceID, delayChangeIMEIID, ctx.Token, ctx.LogKey);

            if (string.IsNullOrEmpty(text)) { Logger.Notify(ctx.LogKey, "Không thể chuyển đổi text", Logger.Icon.Error); return NodeResult.Fail("ConvertText failed"); }

            var mode = input.ControlMode;
            var type = input.TypeOption;

            if (mode == ControlMode.ADB || mode == ControlMode.ADBEvent)
            {
                return await SendText(ctx, text);
            }

            if (mode == ControlMode.Scrcpy && ctx.Input != null) // <-- ĐỔI
            {
                if (type == TypeOption.Typing) await ctx.Input.SendTextAsync(text, ctx.Token, ctx.LogKey);
                else await ctx.Input.SetClipboardAsync(text, ctx.Token, ctx.LogKey);
                Logger.Notify(ctx.LogKey, $"Sent via Scrcpy: {text}", Logger.Icon.Information); return NodeResult.Ok();
            }

            if (mode == ControlMode.HDI && ctx.InputUhid != null) // <-- ĐỔI
            {
                if (type == TypeOption.Typing) await ctx.InputUhid.SendTextAsync(text, ctx.Token, ctx.LogKey);
                else await ctx.InputUhid.SetClipboardAsync(text, ctx.Token, ctx.LogKey);
                Logger.Notify(ctx.LogKey, $"Sent via UHDI: {text}", Logger.Icon.Information); return NodeResult.Ok();
            }

            if (mode == ControlMode.ATX && ctx.Atx != null)
            {
                var r = await ctx.Atx.SendTextAsync(ctx, text);
                Logger.Notify(ctx.LogKey, $"Sent via ATX: {text}", Logger.Icon.Information); return r;
            }
            Logger.Notify(ctx.LogKey, "KeyType không hợp lệ", Logger.Icon.Error); return NodeResult.Fail("Invalid ControlMode");
        }

        public static async Task<string?> GetHttpInfo(DLoopContext ctx, HttpRequestConfig httpInfo)
        {
            var phone = ctx.Session.Phone;
            string nextValue = GetNextValueForPhone(phone);
            for (int i = 0; i < httpInfo.LoopTimes; i++)
            {
                var value = await HttpResponseService.GetAndRegexAsync(httpInfo, nextValue);
                if (!value.IsRequestSucsess) { Logger.Notify(ctx.LogKey, $"Lần {i + 1} fail", Logger.Icon.Error); return null; }
                if (httpInfo.IsSaveValue && !string.IsNullOrEmpty(value.requestId)) { SetNextValueForPhone(phone, value.requestId); nextValue = value.requestId; }
                if (!string.IsNullOrEmpty(value.phoneNumber)) { Logger.Notify(ctx.LogKey, $"Lấy được: {value.phoneNumber}", Logger.Icon.Information); return value.phoneNumber; }
                await Logger.DelayAsync(httpInfo.LoopDelay, ctx.Token, ctx.LogKey);
            }
            return null;
        }

        public static async Task<NodeResult> Run_HttpReponse_Test(DLoopContext ctx, HttpRequestConfig httpInfo, int delayChangeIMEIID = 2000)
        {
            var resp = await GetHttpInfo(ctx, httpInfo);
            if (string.IsNullOrEmpty(resp))
            {
                Logger.Notify(ctx.LogKey, "Không lấy được thông tin từ Http, không thể gửi text", Logger.Icon.Error);
                return NodeResult.Fail("Http empty");
            }

            //bool isATX = httpInfo.ControlMode == ControlMode.ATX && ctx.Atx != null;
            var text = await LanguageInputService.ConvertText(resp, ctx.Session.Phone.DeviceID, delayChangeIMEIID, ctx.Token, ctx.LogKey);
            if (string.IsNullOrEmpty(text))
            {
                Logger.Notify(ctx.LogKey, "Không thể chuyển đổi text, không thể gửi text", Logger.Icon.Error);
                return NodeResult.Fail("ConvertText failed");
            }

            var mode = httpInfo.ControlMode;
            var type = httpInfo.TypeOption;

            if (mode == ControlMode.ADB || mode == ControlMode.ADBEvent)
            {
                return await SendText(ctx, text);
            }
            else if (mode == ControlMode.Scrcpy && ctx.Input != null) // <-- ĐỔI
            {
                if (type == TypeOption.Typing) await ctx.Input.SendTextAsync(text, ctx.Token, ctx.LogKey);
                else await ctx.Input.SetClipboardAsync(text, ctx.Token, ctx.LogKey);
                Logger.Notify(ctx.LogKey, $"Sent Text via Scrcpy: {text}", Logger.Icon.Information);
                return NodeResult.Ok();
            }
            else if (mode == ControlMode.HDI && ctx.InputUhid != null) // <-- ĐỔI
            {
                if (type == TypeOption.Typing) await ctx.InputUhid.SendTextAsync(text, ctx.Token, ctx.LogKey);
                else await ctx.InputUhid.SetClipboardAsync(text, ctx.Token, ctx.LogKey);
                Logger.Notify(ctx.LogKey, $"Sent Text via UHDI: {text}", Logger.Icon.Information);
                return NodeResult.Ok();
            }
            else if (mode == ControlMode.ATX && ctx.Atx != null)
            {
                var r = await ctx.Atx.SendTextAsync(ctx, text);
                Logger.Notify(ctx.LogKey, $"Sent Text via ATX: {text}", Logger.Icon.Information);
                return r;
            }
            else
            {
                Logger.Notify(ctx.LogKey, "KeyType không hợp lệ hoặc handler chưa khởi tạo.", Logger.Icon.Error);
                return NodeResult.Fail("Invalid mode");
            }
        }

        public static async Task<NodeResult> Run_InputText_GetColumnDatabase(DLoopContext ctx, GetColumnDataArgs col, int delayChangeIMEIID = 2000)
        {

            var text = await LanguageInputService.ConvertText(col.Text, ctx.Session.Phone.DeviceID, delayChangeIMEIID, ctx.Token, ctx.LogKey);
            if (string.IsNullOrEmpty(text))
            {
                Logger.Notify(ctx.LogKey, "Không thể chuyển đổi text, không thể gửi text", Logger.Icon.Error);
                return NodeResult.Fail("ConvertText failed");
            }

            var type = col.TypeOption;

            if (col.ControlMode == ControlMode.ADB || col.ControlMode == ControlMode.ADBEvent)
            {
                return await SendText(ctx, text);
            }
            else if (col.ControlMode == ControlMode.Scrcpy && ctx.Input != null) // <-- ĐỔI
            {
                if (type == TypeOption.Typing) await ctx.Input.SendTextAsync(text, ctx.Token, ctx.LogKey);
                else await ctx.Input.SetClipboardAsync(text, ctx.Token, ctx.LogKey);
                Logger.Notify(ctx.LogKey, $"Sent Text via Scrcpy: {text}", Logger.Icon.Information);
                return NodeResult.Ok();
            }
            else if (col.ControlMode == ControlMode.HDI && ctx.InputUhid != null) // <-- ĐỔI
            {
                if (type == TypeOption.Typing) await ctx.InputUhid.SendTextAsync(text, ctx.Token, ctx.LogKey);
                else await ctx.InputUhid.SetClipboardAsync(text, ctx.Token, ctx.LogKey);
                Logger.Notify(ctx.LogKey, $"Sent Text via UHDI: {text}", Logger.Icon.Information);
                return NodeResult.Ok();
            }
            else if (col.ControlMode == ControlMode.ATX && ctx.Atx != null)
            {
                //await ctx.Atx.ImeHide24(ctx);
                var r = await ctx.Atx.SendTextAsync(ctx, text);
                Logger.Notify(ctx.LogKey, $"Sent Text via ATX: {text}", Logger.Icon.Information);
                return r;
            }
            else
            {
                Logger.Notify(ctx.LogKey, "KeyType không hợp lệ hoặc handler chưa khởi tạo.", Logger.Icon.Error);
                return NodeResult.Fail("Invalid ControlMode");
            }
        }




    }
}