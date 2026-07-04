using Dragon.Controller.DeviceControl.ScrcpyNet.InterFace;
using Dragon.Controller.TaskDeviceManager.Core;
using Dragon.Controller.TaskDeviceManager.Infrastructure;
using Dragon.Controller.TaskDeviceManager.Infrastructure.Services;
using Dragon.Controller.TaskDeviceManager.Model.HttpResponse;
using Dragon.Controller.TaskDeviceManager.Model.Input;
using Dragon.Controller.TaskDeviceManager.Model.Mouse;
using System.Collections.Concurrent;

namespace Dragon.Controller.GlobalControl.TaskDeviceManager.Runners
{
    public static class HttpRunner
    {
        private static readonly ConcurrentDictionary<string, string> _next = new();

        private static string GetNext(PhoneSession s) => _next.TryGetValue(s.Phone.DeviceID, out var v) ? v : "";
        private static void SetNext(PhoneSession s, string v) => _next[s.Phone.DeviceID] = v;

        public static async Task<string?> GetHttpInfo(DLoopContext ctx, HttpRequestConfig cfg)
        {
            var s = ctx.Session;
            if (s.Phone == null)
            {
                Logger.Notify(ctx.LogKey, "Phone null", Logger.Icon.Error);
                return null;
            }

            var next = GetNext(s);

            for (int i = 0; i < cfg.LoopTimes; i++)
            {
                var (phone, requestId, ok) = await HttpResponseService.GetAndRegexAsync(cfg, next, ctx.Token, ctx.LogKey);

                if (!ok)
                {
                    Logger.Notify(ctx.LogKey, $"Lần {i + 1}/{cfg.LoopTimes} fail", Logger.Icon.Error);
                    return null;
                }

                if (cfg.IsSaveValue && !string.IsNullOrEmpty(requestId))
                {
                    SetNext(s, requestId);
                    next = requestId;
                }

                if (!string.IsNullOrEmpty(phone))
                {
                    Logger.Notify(ctx.LogKey, $"Lấy được: {phone} (lưu: {next})", Logger.Icon.Information);
                    return phone;
                }

                await Logger.DelayAsync(cfg.LoopDelay, ctx.Token, s.Phone.DeviceID);
            }
            return null;
        }

        public static async Task<NodeResult> RunAsync(DLoopContext ctx, HttpRequestConfig cfg, int delayIme = 2000)
        {
            var s = ctx.Session;
            var textRaw = await GetHttpInfo(ctx, cfg);
            if (string.IsNullOrEmpty(textRaw))
                return NodeResult.Fail("Http empty");

            //bool isAtx = cfg.ControlInputMode == ControlInputMode.ATX && s.Atx != null;
            //var text = await LanguageInputService.ConvertText(textRaw, s.Phone.DeviceID, isAtx, delayIme, ctx.Token, ctx.LogKey);

            var text = await LanguageInputService.ConvertText(textRaw, s.Phone.DeviceID, delayIme, ctx.Token, ctx.LogKey);

            if (string.IsNullOrEmpty(text)) return NodeResult.Fail("ConvertText failed");

            return cfg.ControlMode switch
            {
                ControlMode.ADB =>
                    await InputRunner.SendText(ctx, text),

                ControlMode.ADBEvent =>
                    await InputRunner.SendText(ctx, text),

                ControlMode.Scrcpy when ctx.Input != null => // <-- ĐỔI
                    await SendVia(ctx.Input, text, cfg.TypeOption, ctx, "Scrcpy"),

                ControlMode.HDI when ctx.InputUhid != null => // <-- ĐỔI
                    await SendVia(ctx.InputUhid, text, cfg.TypeOption, ctx, "UHDI"),

                ControlMode.ATX when s.Atx != null =>
                    await SendAtx(ctx, text),

                _ => NodeResult.Fail("Unsupported ControlInputMode")
            };
        }

        // <-- SỬA HOÀN TOÀN
        private static async Task<NodeResult> SendVia(IDeviceInput input, string text, TypeOption opt, DLoopContext ctx, string name)
        {
            if (opt == TypeOption.Typing)
            {
                await input.SendTextAsync(text, ctx.Token, ctx.LogKey);
            }
            else
            {
                await input.SetClipboardAsync(text, ctx.Token, ctx.LogKey);
            }
            Logger.Notify(ctx.LogKey, $"Sent via {name}: {text}", Logger.Icon.Information);
            return NodeResult.Ok();
        }

        private static async Task<NodeResult> SendAtx(DLoopContext ctx, string text)
        {
            //var hide = await ctx.Atx!.ImeHide24(ctx);
            //if (hide.Status != ExecutionStatus.Continue) return hide;
            if (ctx == null || ctx.Atx == null) return NodeResult.Stop();

            var input = await ctx.Atx.SendTextAsync(ctx, text);
            if (input.Status != ExecutionStatus.Continue) return input;

            Logger.Notify(ctx.LogKey, $"Sent via ATX: {text}", Logger.Icon.Information);
            return NodeResult.Ok();
        }
    }
}