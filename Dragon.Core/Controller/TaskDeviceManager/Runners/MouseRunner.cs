using Dragon.Controller.TaskDeviceManager.Core;
using Dragon.Controller.TaskDeviceManager.Infrastructure.Services;
using Dragon.Controller.TaskDeviceManager.Model.Mouse;

namespace Dragon.Controller.TaskDeviceManager.Runners
{
    public static class MouseRunner
    {
        public static async Task<NodeResult> Click(DLoopContext ctx, ClickArg arg)
        {
            if (ctx.Token.IsCancellationRequested) return NodeResult.Stop();

            switch (arg.ControlMode)
            {
                case ControlMode.ADBEvent:
                    return await ADBEventServices.Click(ctx, arg);
                case ControlMode.ADB:
                    return await ADBMouseServices.Click(ctx, arg);
                case ControlMode.ATX:
                    return ctx.Atx == null
                        ? NodeResult.Fail("ATX not available")
                        : await ctx.Atx.ClickAsync(ctx, arg);
                case ControlMode.Scrcpy:
                    return ctx.Mouse == null
                        ? NodeResult.Fail("Scrcpy mouse not available")
                        : await ctx.Mouse.ClickAsync(ctx, arg);
                case ControlMode.HDI:
                    if (ctx.Session.Phone.IsUHDI == false)
                        return NodeResult.Fail("Phone does not support UHDI");
                    return ctx.MouseUhid == null
                        ? NodeResult.Fail("UHDI mouse not available")
                        : await ctx.MouseUhid.ClickAsync(ctx, arg);
                case ControlMode.ACC:
                    return NodeResult.Fail("ACC not available");
                default:
                    return NodeResult.Fail("Unsupported ControlMode");
            }
        }

        public static async Task<NodeResult> Swipe(DLoopContext ctx, SwipeArg arg)
        {
            if (ctx.Token.IsCancellationRequested) return NodeResult.Stop();

            switch (arg.ControlMode)
            {
                case ControlMode.ADBEvent:
                    return await ADBEventServices.Swipe(ctx, arg);
                case ControlMode.ADB:
                    return await ADBMouseServices.Swipe(ctx, arg);
                case ControlMode.ATX:
                    return ctx.Atx == null
                        ? NodeResult.Fail("ATX not available")
                        : await ctx.Atx.SwipeAsync(ctx, arg);
                case ControlMode.Scrcpy:
                    return ctx.Mouse == null
                        ? NodeResult.Fail("Scrcpy mouse not available")
                        : await ctx.Mouse.SwipeAsync(ctx, arg);
                case ControlMode.HDI:
                    if (ctx.Session.Phone.IsUHDI == false)
                        return NodeResult.Fail("Phone does not support UHDI");
                    return ctx.MouseUhid == null
                        ? NodeResult.Fail("UHDI mouse not available")
                        : await ctx.MouseUhid.SwipeAsync(ctx, arg);
                default:
                    return NodeResult.Fail("Unsupported ControlMode");
            }
        }

        public static async Task<NodeResult> LongPress(DLoopContext ctx, LongPressArg arg)
        {
            if (ctx.Token.IsCancellationRequested) return NodeResult.Stop();

            switch (arg.ControlMode)
            {
                case ControlMode.ADBEvent:
                    return await ADBEventServices.LongPress(ctx, arg);
                case ControlMode.ADB:
                    return await ADBMouseServices.LongPress(ctx, arg);
                case ControlMode.ATX:
                    return ctx.Atx == null
                        ? NodeResult.Fail("ATX not available")
                        : await ctx.Atx.LongPressAsync(ctx, arg);
                case ControlMode.Scrcpy:
                    return ctx.Mouse == null
                        ? NodeResult.Fail("Scrcpy mouse not available")
                        : await ctx.Mouse.LongPressAsync(ctx, arg);
                case ControlMode.HDI:
                    if (ctx.Session.Phone.IsUHDI == false)
                        return NodeResult.Fail("Phone does not support UHDI");
                    return ctx.MouseUhid == null
                        ? NodeResult.Fail("UHDI mouse not available")
                        : await ctx.MouseUhid.LongPressAsync(ctx, arg);
                default:
                    return NodeResult.Fail("Unsupported ControlMode");
            }
        }

        public static async Task<NodeResult> DragDrop(DLoopContext ctx, DragArg arg)
        {
            if (ctx.Token.IsCancellationRequested) return NodeResult.Stop();

            switch (arg.ControlMode)
            {
                case ControlMode.ADB:
                case ControlMode.ADBEvent:
                    return await ADBEventServices.DragAndDrop(ctx, arg);
                case ControlMode.ATX:
                    return ctx.Atx == null
                        ? NodeResult.Fail("ATX not available")
                        : await ctx.Atx.DragAndDropAsync(ctx, arg);
                case ControlMode.Scrcpy:
                    return ctx.Mouse == null
                        ? NodeResult.Fail("Scrcpy mouse not available")
                        : await ctx.Mouse.DragDropAsync(ctx, arg);
                case ControlMode.HDI:
                    if (ctx.Session.Phone.IsUHDI == false)
                        return NodeResult.Fail("Phone does not support UHDI");
                    return ctx.MouseUhid == null
                        ? NodeResult.Fail("UHDI mouse not available")
                        : await ctx.MouseUhid.DragDropAsync(ctx, arg);
                default:
                    return NodeResult.Fail("Unsupported ControlMode");
            }
        }
    }
}