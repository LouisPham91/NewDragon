using Dragon.Controller.DeviceControl.OTG.Model;
using Dragon.Controller.DeviceControl.Orc;
using System.Drawing;

namespace Dragon.Controller.DeviceControl.OTG.Loop
{
    public static class AoaLoopRunner
    {
        public static async Task RunAsync(
            AoaLoop loop,
            AoaController ctrl,
            AppCapture? capture = null,
            CancellationToken ct = default)
        {
            // Hydrate trước khi chạy
            loop.HydrateTree();

            await ExecuteNodeAsync(loop, ctrl, capture, ct);

            if (loop.Children?.Count > 0)
            {
                foreach (var child in loop.Children)
                {
                    if (ct.IsCancellationRequested) break;
                    await RunAsync(child, ctrl, capture, ct);
                }
            }
        }

        private static async Task ExecuteNodeAsync(
            AoaLoop node,
            AoaController ctrl,
            AppCapture? capture,
            CancellationToken ct)
        {
            switch (node.Payload)
            {
                case AoaClick click:
                    await HandleClickAsync(click, ctrl, ct);
                    break;
                case AoaSwipe swipe:
                    await HandleSwipeAsync(swipe, ctrl, ct);
                    break;
                case AoaDeeplink deeplink:
                    await HandleDeeplinkAsync(deeplink, capture, ct);
                    break;
                case AoaKeyPress keyPress:
                    await HandleKeyPressAsync(keyPress, ctrl, ct);
                    break;
                case AoaOcr ocr:
                    await HandleOcrAsync(ocr, ctrl, capture, ct);
                    break;
                case AoaSendText sendText:
                    await HandleSendTextAsync(sendText, ctrl, ct);
                    break;
                case int delayMs:
                    await HandleDelayAsync(delayMs, ct);
                    break;
                case string closePoint when node.Type == AoaType.CloseAllApps:
                    await HandleCloseAllAppsAsync(closePoint, ctrl, capture, ct);
                    break;
            }
        }
        /// <summary>
        /// Đóng tất cả app: 
        /// 1. Thử OCR tìm "Clear All" 
        /// 2. Nếu không thấy → click PointCloseApp (dạng "x,y")
        /// 3. Nếu vẫn không có → bỏ qua (có thể không có app nào)
        /// </summary>
        private static async Task HandleCloseAllAppsAsync(
            string pointCloseApp,
            AoaController ctrl,
            AppCapture? capture,
            CancellationToken ct)
        {
            if (capture == null) return;

            // Thử OCR tìm nút "Clear All" / "Close All"
            bool found = false;
            var deadline = DateTime.UtcNow.AddMilliseconds(2000);

            while (DateTime.UtcNow < deadline && !found)
            {
                ct.ThrowIfCancellationRequested();

                Bitmap? bmp = null;
                try { bmp = await capture.ScreenshotBitmapAsync(3000); } catch { }

                if (bmp != null)
                {
                    using (bmp)
                    {
                        var keywords = new[] { "Clear all", "Close all", "Đóng tất cả", "Xóa tất cả", "CLEAR ALL" };
                        foreach (var kw in keywords)
                        {
                            var pt = OrcImageHelper.GetPointImageByText(bmp, kw);
                            if (pt.HasValue)
                            {
                                await ctrl.ClickAtAsync(pt.Value.X, pt.Value.Y, ct);
                                found = true;
                                break;
                            }
                        }
                    }
                }

                if (!found)
                    await Task.Delay(300, ct);
            }

            // Fallback: Click PointCloseApp nếu OCR không tìm thấy
            if (!found && !string.IsNullOrEmpty(pointCloseApp))
            {
                var parts = pointCloseApp.Split(',');
                if (parts.Length == 2 &&
                    int.TryParse(parts[0].Trim(), out int cx) &&
                    int.TryParse(parts[1].Trim(), out int cy))
                {
                    await ctrl.ClickAtAsync(cx, cy, ct);
                }
            }

            // Đợi animation đóng app
            await Task.Delay(600, ct);
        }
        // ==================== DELAY ====================
        public static async Task HandleDelayAsync(int delayMs, CancellationToken ct)
        {
            if (delayMs > 0)
                await Task.Delay(delayMs, ct);
        }

        // ==================== CLICK ====================
        public static async Task HandleClickAsync(AoaClick click, AoaController ctrl, CancellationToken ct)
        {
            await ctrl.MoveToOriginAsync(cancellationToken: ct);

            for (int i = 0; i < click.NumClicks; i++)
            {
                ct.ThrowIfCancellationRequested();

                await ctrl.MoveToAsync(click.X, click.Y, cancellationToken: ct);
                await Task.Delay(50, ct);
                ctrl.MouseClick();

                if (i < click.NumClicks - 1)
                    await Task.Delay(click.DelayBetweenMs, ct);
            }
        }

        // ==================== SWIPE ====================
        public static async Task HandleSwipeAsync(AoaSwipe swipe, AoaController ctrl, CancellationToken ct)
        {
            for (int i = 0; i < swipe.NumSwipe; i++)
            {
                ct.ThrowIfCancellationRequested();

                await ctrl.SwipeAsync(
                    swipe.X1, swipe.Y1,
                    swipe.X2, swipe.Y2,
                    swipe.DurationMs,
                    ct);

                await Task.Delay(200, ct);
            }
        }

        // ==================== DEEPLINK ====================
        public static async Task HandleDeeplinkAsync(AoaDeeplink deeplink, AppCapture? capture, CancellationToken ct)
        {
            if (capture == null)
                throw new InvalidOperationException("AppCapture is required for Deeplink action");

            await capture.DeeplinkAsync(deeplink.Url);

            if (deeplink.WaitAfterMs > 0)
                await Task.Delay(deeplink.WaitAfterMs, ct);
        }

        // ==================== KEY PRESS ====================
        public static async Task HandleKeyPressAsync(AoaKeyPress keyPress, AoaController ctrl, CancellationToken ct)
        {
            for (int i = 0; i < keyPress.Repeat; i++)
            {
                ct.ThrowIfCancellationRequested();

                PressKey(ctrl, keyPress.Key);

                if (i < keyPress.Repeat - 1)
                    await Task.Delay(keyPress.DelayBetweenMs, ct);
            }
        }



        // ==================== OCR ====================
        public static async Task HandleOcrAsync(
            AoaOcr ocr,
            AoaController ctrl,
            AppCapture? capture,
            CancellationToken ct)
        {
            if (capture == null)
                throw new InvalidOperationException("AppCapture is required for OCR action");

            var deadline = DateTime.UtcNow.AddMilliseconds(ocr.TimeoutMs);
            int swipeCount = 0;

            while (DateTime.UtcNow < deadline)
            {
                ct.ThrowIfCancellationRequested();

                Bitmap? bmp = null;
                try
                {
                    bmp = await capture.ScreenshotBitmapAsync(5000);
                }
                catch
                {
                    // Retry sau interval
                }

                if (bmp != null)
                {
                    using (bmp)
                    {
                        foreach (var keyword in ocr.Keywords)
                        {
                            var point = OrcImageHelper.GetPointImageByText(bmp, keyword);
                            if (point.HasValue)
                            {
                                int clickX = point.Value.X + ocr.OffsetX;
                                int clickY = point.Value.Y + ocr.OffsetY;

                                await ctrl.ClickAtAsync(clickX, clickY, ct);
                                return; // Tìm thấy → click → thoát
                            }
                        }
                    }
                }

                // Chưa tìm thấy, swipe nếu còn lượt
                if (ocr.MaxSwipes > 0 && swipeCount < ocr.MaxSwipes)
                {
                    // Vuốt lên để scroll (tọa độ mặc định, có thể config thêm sau)
                    await ctrl.SwipeAsync(540, 1500, 540, 500, 300, ct);
                    swipeCount++;
                }

                await Task.Delay(ocr.IntervalMs, ct);
            }
        }

        // ==================== SEND TEXT ====================
        public static async Task HandleSendTextAsync(AoaSendText sendText, AoaController ctrl, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            await ctrl.KeyboardSendTextAsync(sendText.Text, ct);

            if (sendText.DelayPerCharMs > 0)
                await Task.Delay(sendText.Text.Length * sendText.DelayPerCharMs, ct);
        }



        private static void PressKey(AoaController ctrl, string key)
        {
            switch (key.ToLowerInvariant())
            {
                // Navigation
                case "home": ctrl.KeyPressHome(); break;
                case "back": ctrl.KeyPressBack(); break;
                case "appswitch": ctrl.KeyPressAppSwitch(); break;

                // Basic
                case "enter": ctrl.KeyboardEnter(); break;
                case "space": ctrl.KeyboardSpace(); break;
                case "tab": ctrl.KeyboardTab(); break;
                case "escape": ctrl.KeyboardEscape(); break;
                case "delete": ctrl.KeyboardDelete(); break;
                case "insert": ctrl.KeyboardInsert(); break;

                // Volume
                case "volumeup": ctrl.KeyboardVolumeUp(); break;
                case "volumedown": ctrl.KeyboardVolumeDown(); break;
                case "volumemute": ctrl.KeyboardVolumeMute(); break;

                // Arrows
                case "arrowup": ctrl.KeyboardArrowUp(); break;
                case "arrowdown": ctrl.KeyboardArrowDown(); break;
                case "arrowleft": ctrl.KeyboardArrowLeft(); break;
                case "arrowright": ctrl.KeyboardArrowRight(); break;

                // Ctrl combinations
                case "ctrl_c": ctrl.KeyboardCtrlC(); break;
                case "ctrl_v": ctrl.KeyboardCtrlV(); break;
                case "ctrl_x": ctrl.KeyboardCtrlX(); break;
                case "ctrl_z": ctrl.KeyboardCtrlZ(); break;
                case "ctrl_a": ctrl.KeyboardCtrlA(); break;
                case "ctrl_s": ctrl.KeyboardCtrlS(); break;

                // Alt combinations
                case "alt_f4": ctrl.KeyboardAltF4(); break;
                case "alt_tab": ctrl.KeyboardAltTab(); break;

                // Function keys
                case "f1": ctrl.KeyboardF1(); break;
                case "f2": ctrl.KeyboardF2(); break;
                case "f3": ctrl.KeyboardF3(); break;
                case "f4": ctrl.KeyboardF4(); break;
                case "f5": ctrl.KeyboardF5(); break;
                case "f6": ctrl.KeyboardF6(); break;
                case "f7": ctrl.KeyboardF7(); break;
                case "f8": ctrl.KeyboardF8(); break;
                case "f9": ctrl.KeyboardF9(); break;
                case "f10": ctrl.KeyboardF10(); break;
                case "f11": ctrl.KeyboardF11(); break;
                case "f12": ctrl.KeyboardF12(); break;

                // Lock keys
                case "capslock": ctrl.KeyboardCapsLock(); break;
                case "numlock": ctrl.KeyboardNumLock(); break;

                // Print / Pause
                case "printscreen": ctrl.KeyboardPrintScreen(); break;
                case "pause": ctrl.KeyboardPause(); break;

                // Media
                case "playpause": ctrl.KeyboardPlayPause(); break;
                case "stop": ctrl.KeyboardStop(); break;
                case "nexttrack": ctrl.KeyboardNextTrack(); break;
                case "prevtrack": ctrl.KeyboardPrevTrack(); break;

                // Page navigation
                case "pageup": ctrl.KeyboardPageUp(); break;
                case "pagedown": ctrl.KeyboardPageDown(); break;

                // End / Home (cursor)
                case "end": ctrl.KeyboardEnd(); break;

                default:
                    if (byte.TryParse(key, out byte code))
                        ctrl.KeyboardSendKey(code);
                    break;
            }
        }

        // Đặt trong class AoaLoopRunner hoặc AoaKeyPress
        public static List<string> AoaKeypressList { get; } = new()
        {
            "home",
            "back",
            "appswitch",
            "enter",
            "space",
            "tab",
            "escape",
            "volumeup",
            "volumedown",
            "volumemute",
            "arrowup",
            "arrowdown",
            "arrowleft",
            "arrowright",
            "ctrl_c",
            "ctrl_v",
            "ctrl_a",
            "ctrl_x",
            "ctrl_z",
            "ctrl_s",
            "alt_f4",
            "alt_tab",
            "f1",
            "f2",
            "f3",
            "f4",
            "f5",
            "f6",
            "f7",
            "f8",
            "f9",
            "f10",
            "f11",
            "f12",
            "delete",
            "insert",
            "pageup",
            "pagedown",
            "capslock",
            "numlock",
            "printscreen",
            "pause",
            "playpause",
            "stop",
            "nexttrack",
            "prevtrack",
        };
    }
}