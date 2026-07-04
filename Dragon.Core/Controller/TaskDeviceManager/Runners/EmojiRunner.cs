
using Dragon.Controller.TaskDeviceManager.Core;
using Dragon.Controller.TaskDeviceManager.Infrastructure;
using Dragon.Controller.TaskDeviceManager.Infrastructure.Services;
using Dragon.Controller.TaskDeviceManager.Model.Emoji;
using Dragon.Controller.TaskDeviceManager.Model.Mouse;
using System.Drawing;
using Dragon.Controller.GlobalControl.TaskDeviceManager.Runners;

namespace Dragon.Controller.TaskDeviceManager.Runners
{
    public class EmojiRunner
    {
        public static async Task<NodeResult> RunAsync(DLoopContext ctx, EmojiArgs emoijReaction)
        {
            var s = ctx.Session;
            var token = ctx.Token;
            var logKey = ctx.LogKey;
            try
            {
                if (token.IsCancellationRequested) return NodeResult.Stop();
                if (s.Phone == null || emoijReaction?.VisionScanArgs == null || emoijReaction?.Reactions == null)
                {
                    Logger.Notify(logKey, "❌ Config null", Logger.Icon.Error);
                    return NodeResult.Fail("Config null");
                }

                Logger.Notify(logKey, "🔍 B1: Tìm button Like...", Logger.Icon.Information);
                var likePoint = await VisionScanRunner.FindPointWithLoop(ctx, emoijReaction.VisionScanArgs);
                if (likePoint.point == null || likePoint.point == Point.Empty)
                {
                    Logger.Notify(logKey, "❌ Không tìm thấy button Like", Logger.Icon.Warning);
                    return NodeResult.Fail("Like not found");
                }

                float percentX = likePoint.point.Value.X * 100f / s.Phone.PhysicalWidth;
                float percentY = likePoint.point.Value.Y * 100f / s.Phone.PhysicalHeight;
                var reactionPercent = emoijReaction.Reactions.GetReactionPoint(percentX, percentY);
                int reactionX = (int)(s.Phone.PhysicalWidth * reactionPercent.X / 100f);
                int reactionY = (int)(s.Phone.PhysicalHeight * reactionPercent.Y / 100f);

                var type = emoijReaction.ControlMode;
                var longPress = new LongPressArg { ControlMode = type, x = likePoint.point.Value.X, y = likePoint.point.Value.Y, Duration = 1000 };
                var click = new ClickArg { ControlMode = type, x = reactionX, y = reactionY };

                bool ok = false;
                switch (type)
                {
                    case ControlMode.ADB:
                    case ControlMode.ADBEvent:
                        if (s.DeviceData != null) { await ADBEventServices.LongPress(ctx, longPress); ok = true; }
                        break;
                    case ControlMode.ATX:
                        if (s.Atx != null) { await s.Atx.LongPressAsync(ctx, longPress); ok = true; }
                        break;
                    case ControlMode.Scrcpy:
                        if (ctx.Mouse != null)
                        {
                            var r = ctx.Mouse.renderSize;
                            var lp = new LongPressArg
                            {
                                ControlMode = type,
                                x = (int)(longPress.x * r.Width / s.Phone.PhysicalWidth),
                                y = (int)(longPress.y * r.Height / s.Phone.PhysicalHeight),
                                Duration = 1000
                            };
                            await ctx.Mouse.LongPressAsync(ctx, lp);
                            ok = true;
                        }
                        break;
                    case ControlMode.HDI:
                        if (ctx.MouseUhid != null) { await ctx.MouseUhid.LongPressAsync(ctx, longPress); ok = true; }
                        break;
                }
                if (!ok) return NodeResult.Fail("LongPress failed");
                await Logger.DelayAsync(600, token, logKey);

                switch (type)
                {
                    case ControlMode.ADB:
                    case ControlMode.ADBEvent:
                        if (s.DeviceData != null) await ADBEventServices.Click(ctx, click); break;
                    case ControlMode.ATX:
                        if (s.Atx != null) await s.Atx.ClickAsync(ctx, click); break;
                    case ControlMode.Scrcpy:
                        if (ctx.Mouse != null)
                        {
                            var r = ctx.Mouse.renderSize;
                            var c = new ClickArg
                            {
                                ControlMode = type,
                                x = (int)(click.x * r.Width / s.Phone.PhysicalWidth),
                                y = (int)(click.y * r.Height / s.Phone.PhysicalHeight)
                            };
                            await ctx.Mouse.ClickAsync(ctx, c);
                        }
                        break;
                    case ControlMode.HDI:
                        if (ctx.MouseUhid != null) await ctx.MouseUhid.ClickAsync(ctx, click); break;
                }

                Logger.Notify(logKey, "✅ Done Reaction!", Logger.Icon.Asterisk);
                return NodeResult.Ok();
            }
            catch (Exception ex)
            {
                Logger.Notify(logKey, $"❌ Error: {ex.Message}", Logger.Icon.Error);
                return NodeResult.Fail(ex.Message);
            }
        }

    }
}