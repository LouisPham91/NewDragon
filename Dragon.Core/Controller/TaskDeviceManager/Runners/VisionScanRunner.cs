
using Dragon.Controller.Controller.GlobalControl.Helper;
using Dragon.Controller.DeviceControl.Orc;
using Dragon.Controller.GlobalControl.Helper;
using Dragon.Controller.TaskDeviceManager.Core;
using Dragon.Controller.TaskDeviceManager.Infrastructure;
using Dragon.Controller.TaskDeviceManager.Infrastructure.Services;
using Dragon.Controller.TaskDeviceManager.Model.Mouse;
using Dragon.Controller.TaskDeviceManager.Model.Vision;
using Dragon.Database.Models;
using System.Drawing;
using System.Globalization;

namespace Dragon.Controller.GlobalControl.TaskDeviceManager.Runners
{
    public static class VisionScanRunner
    {
        private static Rectangle? ParseCropRegion(string crop, int imgW, int imgH)
        {
            if (string.IsNullOrWhiteSpace(crop)) return null;
            try
            {
                var p = crop.Split(',');
                if (p.Length != 4) return null;
                float x = float.Parse(p[0], CultureInfo.InvariantCulture);
                float y = float.Parse(p[1], CultureInfo.InvariantCulture);
                float w = float.Parse(p[2], CultureInfo.InvariantCulture);
                float h = float.Parse(p[3], CultureInfo.InvariantCulture);
                int ix = (int)(imgW * x / 100f);
                int iy = (int)(imgH * y / 100f);
                int iw = (int)(imgW * w / 100f);
                int ih = (int)(imgH * h / 100f);
                return new Rectangle(ix, iy, iw, ih);
            }
            catch { return null; }
        }

        private static Point? ParsePercentPoint(string txt, int w, int h)
        {
            if (string.IsNullOrWhiteSpace(txt) || !txt.Contains(',')) return null;
            try
            {
                var p = txt.Split(',');
                float x = float.Parse(p[0], CultureInfo.InvariantCulture);
                float y = float.Parse(p[1], CultureInfo.InvariantCulture);
                return new Point((int)(w * x / 100f), (int)(h * y / 100f));
            }
            catch { return null; }
        }

        public static async Task<(Point? point, string PointTextPercent)> FindPointWithLoop(DLoopContext ctx, VisionScanArgs? args)
        {
            var phone = ctx.Session.Phone;
            if (phone == null || args == null) return (null, "");

            if (args.ATXNode != null) args.ATXNode.VisionAction = args.VisionAction;
            foreach (var img in args.ImageOrcTexts) img.VisionAction = args.VisionAction;

            Point point = Point.Empty;
            string PointTextPercent = "";

            for (int attempt = 1; attempt <= args.RetryCount && point == Point.Empty; attempt++)
            {
                if (ctx.Token.IsCancellationRequested) return (null, "");

                // ATX
                if (args.VisionMode == VisionMode.ByAtxNode && ctx.Session.Atx != null && args.ATXNode != null)
                {
                    var list = await ctx.Session.Atx.FindNodesWithDuplicates(args.ATXNode, phone.PhysicalWidth, phone.PhysicalHeight);
                    var gp = list?.FirstOrDefault()?.Bound.CenterPos;
                    if (gp.HasValue) { point = gp.Value; PointTextPercent = args.ATXNode.SecialClickPoint; break; }
                }

                // Image / OCR
                if (point == Point.Empty && args.VisionMode != VisionMode.ByAtxNode && ctx.Session.DeviceData != null)
                {
                    Bitmap? bitmap = null;
                    bool useScrcpy = ctx.Screen != null; // <-- ĐỔI
                    try
                    {
                        if (useScrcpy)
                        {
                            using var bmp = ctx.Screen!.ScreenShotV2(); // <-- ĐỔI
                            if (bmp != null) bitmap = new Bitmap(bmp);
                        }
                        else
                        {
                            var fb = ctx.Session.AdbClient.GetFrameBuffer(ctx.Session.DeviceData);
                            using var bmp = fb.ToImage();
                            if (bmp != null) bitmap = new Bitmap(bmp);
                        }
                    }
                    catch { break; }

                    if (bitmap != null)
                    {
                        using (bitmap)
                        {
                            if (args.VisionMode == VisionMode.ByImageTemplate)
                            {
                                foreach (var img in args.ImageOrcTexts.Where(x => x.IsActive))
                                {
                                    byte[]? data = img.ImageDataSrcpy;
                                    if (data == null) continue;
                                    using var template = BitmapConverter.ByteArrayToBitmap(data);
                                    if (template == null) return (null, "");
                                    var found = BitmapFinderServices.FindTemplate(BitmapExtensions.ToImageSharp(bitmap), BitmapExtensions.ToImageSharp(template), Convert.ToSingle(img.Accuracy));
                                    if (found.HasValue)
                                    {
                                        PointTextPercent = img.SecialClickPoint; break;
                                    }
                                }
                            }
                            else if (args.VisionMode == VisionMode.ByOcrText)
                            {
                                string tessPath = Path.GetFullPath("tessdata");
                                foreach (var img in args.ImageOrcTexts.Where(x => x.IsActive && !string.IsNullOrEmpty(x.TextToFind)))
                                {
                                    var pt = OrcImageHelper.GetPointImageByText(bitmap, img.TextToFind, tessPath);
                                    if (pt.HasValue) { point = pt.Value; PointTextPercent = img.SecialClickPoint ?? ""; break; }
                                }
                            }
                        }
                    }
                }

                if (point == Point.Empty) await Task.Delay(args.RetryDelayMs, ctx.Token);
            }

            return point == Point.Empty ? (null, "") : (point, PointTextPercent);
        }

        public static async Task<NodeResult> GetPositionAndClick(DLoopContext ctx, VisionScanArgs? args)
        {
            if (ctx.Token.IsCancellationRequested) return NodeResult.Stop();
            if (args == null) return NodeResult.Fail("args null");
            var phone = ctx.Session.Phone;
            if (phone == null) return NodeResult.Fail("phone null");

            var mouseType = args.ControlMode;

            if (args.VisionAction == VisionAction.DetectOnly || args.VisionAction == VisionAction.ExtractText)
            {
                var res = await FindPointWithLoop(ctx, args);
                return res.point != null ? NodeResult.Ok() : NodeResult.Fail("Not found");
            }

            if (args.VisionAction == VisionAction.DetectAndClick)
            {
                var res = await FindPointWithLoop(ctx, args);
                if (res.point == null) return NodeResult.Fail("Not found");
                await ClickAt(ctx, res.point.Value, mouseType);
                return NodeResult.Ok();
            }

            if (args.VisionAction == VisionAction.ClickAtPoint)
            {
                var res = await FindPointWithLoop(ctx, args);
                if (res.point == null) return NodeResult.Fail("Target not found");

                string pct = res.PointTextPercent;
                var clickPt = ParsePercentPoint(pct, phone.PhysicalWidth, phone.PhysicalHeight);
                if (clickPt == null) return NodeResult.Fail("SecialClickPoint invalid");

                await ClickAt(ctx, clickPt.Value, mouseType);
                return NodeResult.Ok();
            }

            return NodeResult.Fail("VisionAction None");
        }

        private static async Task ClickAt(DLoopContext ctx, Point pt, ControlMode type)
        {
            var s = ctx.Session;
            var click = new ClickArg { x = pt.X, y = pt.Y, ControlMode = type };

            switch (type)
            {
                case ControlMode.ATX:
                    s.Atx?.ClickAsync(pt.X, pt.Y);
                    break;
                case ControlMode.ADBEvent:
                case ControlMode.ADB:
                    if (s.AdbClient != null) await ADBEventServices.Click(ctx, click);
                    break;
                case ControlMode.Scrcpy:
                    if (ctx.Mouse != null) // <-- ĐỔI
                        await ctx.Mouse.ClickAsync(ctx, click);
                    break;
                case ControlMode.HDI:
                    if (ctx.MouseUhid != null) // <-- ĐỔI
                        await ctx.MouseUhid.ClickAsync(ctx, click);
                    break;
                default:
                    s.Atx?.ClickAsync(pt.X, pt.Y);
                    break;
            }
            Logger.Notify(ctx.LogKey, $"✅ Vision click [{type}] at {pt.X},{pt.Y}", Logger.Icon.Information);
        }
    }
}