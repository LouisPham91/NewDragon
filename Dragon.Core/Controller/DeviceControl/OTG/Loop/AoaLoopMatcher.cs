using Dragon.Controller.Database.Services;
using Dragon.Controller.DeviceControl.OTG.Model;
using Dragon.Database.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Dragon.Controller.DeviceControl.OTG.Loop
{
    public static class AoaLoopMatcher
    {
        /// <summary>
        /// Tìm AoaLoop khớp nhất. Nếu không có, tạo default template dựa trên model/brand.
        /// </summary>
        public static async Task<AoaLoop?> FindBestMatchAsync(Phone phone, bool isAppCaptureConnected)
        {
            // 1. Tìm exact match với 5 trường (bao gồm IsAppCaptureConnected)
            var candidates = AoaLoopRepository.FindByPhoneModel(phone.Model, isAppCaptureConnected);

            var exact = candidates.FirstOrDefault(c =>
                c.PhoneModel == phone.Model &&
                c.ProcVersion == phone.ProcVersion &&
                c.ProcCpuInfo == phone.ProcCpuInfo &&
                c.API == phone.API &&
                c.IsAppCaptureConnected == isAppCaptureConnected);

            if (exact != null)
            {
                exact.HydrateTree();
                return exact;
            }

            // 2. Tìm match Model + API + IsAppCaptureConnected
            var modelApi = candidates.FirstOrDefault(c =>
                c.PhoneModel == phone.Model && c.API == phone.API && c.IsAppCaptureConnected == isAppCaptureConnected);

            if (modelApi != null)
            {
                var cloned = await CloneForPhoneAsync(modelApi, phone);
                cloned.HydrateTree();
                return cloned;
            }

            // 3. Tìm match Model + IsAppCaptureConnected only
            var modelOnly = candidates.FirstOrDefault();
            if (modelOnly != null)
            {
                var cloned = await CloneForPhoneAsync(modelOnly, phone);
                cloned.HydrateTree();
                return cloned;
            }

            // 4. Không có gì → tạo default từ template chung với IsAppCaptureConnected tương ứng
            var defaultLoop = CreateDefaultForPhone(phone, isAppCaptureConnected);
            bool saved = AoaLoopRepository.Add(defaultLoop);

            if (saved)
            {
                defaultLoop.HydrateTree();
                return defaultLoop;
            }

            return null;
        }

        /// <summary>
        /// Tạo AoaLoop mặc định cho phone mới.
        /// Customer có thể vào Form để custom lại sau.
        /// </summary>
        public static AoaLoop CreateDefaultForPhone(Phone phone, bool isAppCaptureConnected)
        {
            var loop = new AoaLoop
            {
                PhoneModel = phone.Model,
                ProcVersion = phone.ProcVersion,
                ProcCpuInfo = phone.ProcCpuInfo,
                API = phone.API,
                Type = AoaType.Delay,
                Payload = 1000,
                IsAppCaptureConnected = isAppCaptureConnected,
                Children = new List<AoaLoop>()
            };

            if (isAppCaptureConnected)
            {
                // ===== CÓ CAPTURE: Dùng OCR như cũ =====
                CreateDefaultWithCapture(loop);
            }
            else
            {
                // ===== KHÔNG CAPTURE: Dùng HID mò =====
                CreateDefaultWithoutCapture(loop);
            }

            return loop;
        }

        // ===== DEFAULT CHO CAPTURE CONNECTED (giữ nguyên logic cũ) =====
        private static void CreateDefaultWithCapture(AoaLoop loop)
        {
            // Bước 1: Mở Settings
            loop.Children.Add(new AoaLoop
            {
                Type = AoaType.Deeplink,
                Payload = new AoaDeeplink { Url = "settings", WaitAfterMs = 2000 }
            });

            // Bước 2: Tìm và click "About phone" (dùng OCR)
            loop.Children.Add(new AoaLoop
            {
                Type = AoaType.Ocr,
                Payload = new AoaOcr
                {
                    Keywords = new[] { "About phone", "About", "Giới thiệu", "Thông tin", "Device info" },
                    TimeoutMs = 8000,
                    IntervalMs = 800,
                    MaxSwipes = 5,
                    OffsetX = 0,
                    OffsetY = 0
                }
            });

            // Bước 3: Đợi load
            loop.Children.Add(new AoaLoop { Type = AoaType.Delay, Payload = 1500 });

            // Bước 4: Tìm "Build number"
            loop.Children.Add(new AoaLoop
            {
                Type = AoaType.Ocr,
                Payload = new AoaOcr
                {
                    Keywords = new[] { "Build number", "Build", "Số hiệu", "MIUI version", "Version" },
                    TimeoutMs = 5000,
                    IntervalMs = 500,
                    MaxSwipes = 5,
                    OffsetX = 0,
                    OffsetY = 0
                }
            });

            // Bước 5: Click 7 lần vào vị trí vừa tìm thấy
            loop.Children.Add(new AoaLoop
            {
                Type = AoaType.Click,
                Payload = new AoaClick { X = 540, Y = 1200, NumClicks = 7, DelayBetweenMs = 400 }
            });

            // Bước 6: Back
            loop.Children.Add(new AoaLoop
            {
                Type = AoaType.KeyPress,
                Payload = new AoaKeyPress { Key = "back", Repeat = 1, DelayBetweenMs = 300 }
            });

            // Bước 7: Đợi
            loop.Children.Add(new AoaLoop { Type = AoaType.Delay, Payload = 800 });

            // Bước 8: Tìm "Developer options"
            loop.Children.Add(new AoaLoop
            {
                Type = AoaType.Ocr,
                Payload = new AoaOcr
                {
                    Keywords = new[] { "Developer", "Nhà phát triển", "Developer options" },
                    TimeoutMs = 5000,
                    IntervalMs = 500,
                    MaxSwipes = 5,
                    OffsetX = 0,
                    OffsetY = 0
                }
            });

            // Bước 9: Tìm và bật "USB debugging"
            loop.Children.Add(new AoaLoop
            {
                Type = AoaType.Ocr,
                Payload = new AoaOcr
                {
                    Keywords = new[] { "USB debugging", "Gỡ lỗi USB", "Debugging" },
                    TimeoutMs = 5000,
                    IntervalMs = 500,
                    MaxSwipes = 5,
                    OffsetX = 300,
                    OffsetY = 0
                }
            });

            // Bước 10: Xác nhận dialog
            loop.Children.Add(new AoaLoop
            {
                Type = AoaType.Ocr,
                Payload = new AoaOcr
                {
                    Keywords = new[] { "OK", "Allow", "Cho phép", "Yes" },
                    TimeoutMs = 3000,
                    IntervalMs = 300,
                    MaxSwipes = 0,
                    OffsetX = 0,
                    OffsetY = 0
                }
            });

            // Bước 11: Home
            loop.Children.Add(new AoaLoop
            {
                Type = AoaType.KeyPress,
                Payload = new AoaKeyPress { Key = "home", Repeat = 1 }
            });
        }

        // ===== DEFAULT CHO KHÔNG CAPTURE (HID mò theo flow của cậu) =====
        private static void CreateDefaultWithoutCapture(AoaLoop loop)
        {
            // Bước 1: Mở App Switch → Click Close All → Click đóng tất cả
            loop.Children.Add(new AoaLoop
            {
                Type = AoaType.KeyPress,
                Payload = new AoaKeyPress { Key = "appswitch", Repeat = 1, DelayBetweenMs = 300 }
            });
            loop.Children.Add(new AoaLoop { Type = AoaType.Delay, Payload = 500 });
            loop.Children.Add(new AoaLoop
            {
                Type = AoaType.Click,
                Payload = new AoaClick { X = 50, Y = 90, NumClicks = 1 } // % -> pixel sẽ được convert trong runner
            });
            loop.Children.Add(new AoaLoop { Type = AoaType.Delay, Payload = 300 });

            // Bước 2: App Switch lần nữa → Click top (50%,20%)
            loop.Children.Add(new AoaLoop
            {
                Type = AoaType.KeyPress,
                Payload = new AoaKeyPress { Key = "appswitch", Repeat = 1, DelayBetweenMs = 300 }
            });
            loop.Children.Add(new AoaLoop { Type = AoaType.Delay, Payload = 500 });
            loop.Children.Add(new AoaLoop
            {
                Type = AoaType.Click,
                Payload = new AoaClick { X = 50, Y = 20, NumClicks = 1 }
            });
            loop.Children.Add(new AoaLoop { Type = AoaType.Delay, Payload = 300 });

            // Gõ "setting"
            loop.Children.Add(new AoaLoop
            {
                Type = AoaType.SendText,
                Payload = new AoaSendText { Text = "setting", DelayPerCharMs = 50 }
            });
            loop.Children.Add(new AoaLoop { Type = AoaType.Delay, Payload = 500 });

            // Click app đầu tiên bên trái (15%,30%)
            loop.Children.Add(new AoaLoop
            {
                Type = AoaType.Click,
                Payload = new AoaClick { X = 15, Y = 30, NumClicks = 1 }
            });
            loop.Children.Add(new AoaLoop { Type = AoaType.Delay, Payload = 1000 });

            // Bước 3: Kéo xuống 4 lần (50%,40% → 50%,80%, 200ms)

            loop.Children.Add(new AoaLoop
            {
                Type = AoaType.Swipe,
                Payload = new AoaSwipe
                {
                    X1 = 50,
                    Y1 = 80,
                    X2 = 50,
                    Y2 = 40,
                    DurationMs = 200,
                    NumSwipe = 4
                }
            });
            loop.Children.Add(new AoaLoop { Type = AoaType.Delay, Payload = 300 });


            // Bước 4: Click 50%,90% (cuối trang) → Click 50%,80% → Click 50%,60% 7 lần
            loop.Children.Add(new AoaLoop
            {
                Type = AoaType.Click,
                Payload = new AoaClick { X = 50, Y = 90, NumClicks = 1 }
            });
            loop.Children.Add(new AoaLoop { Type = AoaType.Delay, Payload = 300 });

            loop.Children.Add(new AoaLoop
            {
                Type = AoaType.Click,
                Payload = new AoaClick { X = 50, Y = 80, NumClicks = 1 }
            });
            loop.Children.Add(new AoaLoop { Type = AoaType.Delay, Payload = 300 });

            loop.Children.Add(new AoaLoop
            {
                Type = AoaType.Click,
                Payload = new AoaClick { X = 50, Y = 60, NumClicks = 7, DelayBetweenMs = 400 }
            });
            loop.Children.Add(new AoaLoop { Type = AoaType.Delay, Payload = 500 });

            // Bước 5: Back 2 lần
            loop.Children.Add(new AoaLoop
            {
                Type = AoaType.KeyPress,
                Payload = new AoaKeyPress { Key = "back", Repeat = 2, DelayBetweenMs = 300 }
            });
            loop.Children.Add(new AoaLoop { Type = AoaType.Delay, Payload = 500 });

            // Bước 6: Click 50%,90% → Kéo xuống 2 lần chậm (1000ms)
            loop.Children.Add(new AoaLoop
            {
                Type = AoaType.Click,
                Payload = new AoaClick { X = 50, Y = 90, NumClicks = 1 }
            });
            loop.Children.Add(new AoaLoop { Type = AoaType.Delay, Payload = 300 });


            loop.Children.Add(new AoaLoop
            {
                Type = AoaType.Swipe,
                Payload = new AoaSwipe
                {
                    X1 = 50,
                    Y1 = 80,
                    X2 = 50,
                    Y2 = 40,
                    DurationMs = 1000,
                    NumSwipe = 2
                }
            });
            loop.Children.Add(new AoaLoop { Type = AoaType.Delay, Payload = 500 });


            // Bước 7: Click "gỡ lỗi USB" / "USB debugging" (50%,55%)
            loop.Children.Add(new AoaLoop
            {
                Type = AoaType.Click,
                Payload = new AoaClick { X = 50, Y = 55, NumClicks = 1 }
            });
            loop.Children.Add(new AoaLoop { Type = AoaType.Delay, Payload = 4000 }); // Chờ 4s

            // Bước 8: Click "đồng ý" / "allow" / "ủy quyền" (50%,55%)
            loop.Children.Add(new AoaLoop
            {
                Type = AoaType.Click,
                Payload = new AoaClick { X = 50, Y = 55, NumClicks = 1 }
            });
            loop.Children.Add(new AoaLoop { Type = AoaType.Delay, Payload = 500 });

            // Bước 9: Click 70%,60%
            loop.Children.Add(new AoaLoop
            {
                Type = AoaType.Click,
                Payload = new AoaClick { X = 70, Y = 60, NumClicks = 1 }
            });
            loop.Children.Add(new AoaLoop { Type = AoaType.Delay, Payload = 300 });

            // Bước 10: Home
            loop.Children.Add(new AoaLoop
            {
                Type = AoaType.KeyPress,
                Payload = new AoaKeyPress { Key = "home", Repeat = 1 }
            });
        }

        public static async Task<AoaLoop> CloneForPhoneAsync(AoaLoop source, Phone phone)
        {
            var json = JsonSerializer.Serialize(source, AoaLoopJsonContext.Default.AoaLoop);
            var cloned = JsonSerializer.Deserialize(json, AoaLoopJsonContext.Default.AoaLoop)!;

            cloned.Id = 0;
            cloned.PhoneModel = phone.Model;
            cloned.ProcVersion = phone.ProcVersion;
            cloned.ProcCpuInfo = phone.ProcCpuInfo;
            cloned.API = phone.API;
            cloned.IsAppCaptureConnected = source.IsAppCaptureConnected; // <-- THÊM: giữ nguyên flag từ source

            bool saved = AoaLoopRepository.Add(cloned);
            if (!saved)
            {
                return AoaLoopRepository.FindOneByUnique(phone.Model, phone.ProcVersion, phone.ProcCpuInfo, phone.API, source.IsAppCaptureConnected) ?? cloned;

            }

            return cloned;
        }
    }
}
