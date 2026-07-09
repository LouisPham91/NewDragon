using Dragon.Controller.DeviceControl.Orc;
using Dragon.Controller.DeviceControl.OTG.Model;
using System.Diagnostics;
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
                    // Nếu node yêu cầu OCR nhưng IsAppCaptureConnected = false
                    // thì capture sẽ là null -> HandleOcrAsync sẽ xử lý fallback
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
            // ===== TRƯỜNG HỢP 1: CÓ AppCapture =====
            if (capture != null)
            {
                // Thử OCR tìm nút "Clear All" / "Close All"
                var deadline = DateTime.UtcNow.AddMilliseconds(2000);
                bool found = false;

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
                    if (!found) await Task.Delay(300, ct);
                }

                if (found)
                {
                    await Task.Delay(600, ct);
                    return;
                }
            }

            // ===== TRƯỜNG HỢP 2: KHÔNG CÓ AppCapture -> dùng HID mò =====
            // Fallback 1: Click PointCloseApp nếu có
            if (!string.IsNullOrEmpty(pointCloseApp))
            {
                var (cx, cy) = PointCloseAppHelper.ToPixel(pointCloseApp, ctrl.PhysicalWidth, ctrl.PhysicalHeight);
                await ctrl.ClickAtAsync(cx, cy, ct);
            }

            // Fallback 2: Dùng phím tắt (Home rồi mở recent apps)
            // Không làm gì thêm, coi như đã xong
            await Task.Delay(300, ct);
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

                int clickX, clickY;
                if (click.IsPerCent)
                {
                    // Convert % -> pixel (giả sử màn hình 1080x1920, cần truyền physical size vào)
                    clickX = (int)(1080 * click.X / 100f);
                    clickY = (int)(1920 * click.Y / 100f);
                }
                else
                {
                    clickX = (int)click.X;
                    clickY = (int)click.Y;
                }

                await ctrl.MoveToAsync(clickX, clickY, cancellationToken: ct);
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

                int x1, y1, x2, y2;
                if (swipe.IsPerCent)
                {
                    x1 = (int)(1080 * swipe.X1 / 100f);
                    y1 = (int)(1920 * swipe.Y1 / 100f);
                    x2 = (int)(1080 * swipe.X2 / 100f);
                    y2 = (int)(1920 * swipe.Y2 / 100f);
                }
                else
                {
                    x1 = (int)swipe.X1;
                    y1 = (int)swipe.Y1;
                    x2 = (int)swipe.X2;
                    y2 = (int)swipe.Y2;
                }

                await ctrl.SwipeAsync(x1, y1, x2, y2, swipe.DurationMs, ct);
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
            if (capture != null)
            {
                var deadline = DateTime.UtcNow.AddMilliseconds(ocr.TimeoutMs);
                int swipeCount = 0;

                while (DateTime.UtcNow < deadline)
                {
                    ct.ThrowIfCancellationRequested();
                    Bitmap? bmp = null;
                    try { bmp = await capture.ScreenshotBitmapAsync(5000); } catch { }

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
                                    return;
                                }
                            }
                        }
                    }

                    // Chưa tìm thấy, swipe nếu còn lượt
                    if (ocr.MaxSwipes > 0 && swipeCount < ocr.MaxSwipes)
                    {
                        // ===== SỬA: Dùng tọa độ từ AoaOcr =====
                        int x1, y1, x2, y2;
                        if (ocr.SwipeIsPercent)
                        {
                            x1 = (int)(ctrl.PhysicalWidth * ocr.SwipeStartX / 100f);
                            y1 = (int)(ctrl.PhysicalHeight * ocr.SwipeStartY / 100f);
                            x2 = (int)(ctrl.PhysicalWidth * ocr.SwipeEndX / 100f);
                            y2 = (int)(ctrl.PhysicalHeight * ocr.SwipeEndY / 100f);
                        }
                        else
                        {
                            x1 = (int)ocr.SwipeStartX;
                            y1 = (int)ocr.SwipeStartY;
                            x2 = (int)ocr.SwipeEndX;
                            y2 = (int)ocr.SwipeEndY;
                        }

                        await ctrl.SwipeAsync(x1, y1, x2, y2, ocr.SwipeDurationMs, ct);
                        swipeCount++;
                    }

                    await Task.Delay(ocr.IntervalMs, ct);
                }
            }
            else
            {
                // Fallback HID khi không có capture
                Debug.WriteLine($"[AoaLoop] OCR requested but no AppCapture available. Keywords: {string.Join(", ", ocr.Keywords)}");

                if (ocr.OffsetX != 0 || ocr.OffsetY != 0)
                {
                    await ctrl.ClickAtAsync(ocr.OffsetX, ocr.OffsetY, ct);
                }
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