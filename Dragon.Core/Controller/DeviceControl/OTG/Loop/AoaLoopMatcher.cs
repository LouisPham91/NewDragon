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
        public static async Task<AoaLoop?> FindBestMatchAsync(Phone phone)
        {
            // 1. Tìm exact match
            var candidates = AoaLoopRepository.FindByPhoneModel(phone.Model);

            var exact = candidates.FirstOrDefault(c =>
                c.PhoneModel == phone.Model &&
                c.ProcVersion == phone.ProcVersion &&
                c.ProcCpuInfo == phone.ProcCpuInfo &&
                c.API == phone.API);

            if (exact != null)
            {
                exact.HydrateTree();
                return exact;
            }

            // 2. Tìm match Model + API
            var modelApi = candidates.FirstOrDefault(c =>
                c.PhoneModel == phone.Model && c.API == phone.API);

            if (modelApi != null)
            {
                var cloned = await CloneForPhoneAsync(modelApi, phone);
                cloned.HydrateTree();
                return cloned;
            }

            // 3. Tìm match Model only
            var modelOnly = candidates.FirstOrDefault();
            if (modelOnly != null)
            {
                var cloned = await CloneForPhoneAsync(modelOnly, phone);
                cloned.HydrateTree();
                return cloned;
            }

            // 4. Không có gì → tạo default từ template chung
            var defaultLoop = CreateDefaultForPhone(phone);
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
        public static AoaLoop CreateDefaultForPhone(Phone phone)
        {
            var loop = new AoaLoop
            {
                PhoneModel = phone.Model,
                ProcVersion = phone.ProcVersion,
                ProcCpuInfo = phone.ProcCpuInfo,
                API = phone.API,
                Type = AoaType.Delay,
                Payload = 1000,
                Children = new List<AoaLoop>()
            };

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
            loop.Children.Add(new AoaLoop
            {
                Type = AoaType.Delay,
                Payload = 1500
            });

            // Bước 4: Tìm "Build number" (dùng OCR, không click)
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
            // Vì OCR đã click vào vị trí tìm thấy, ta cần biết tọa độ.
            // Với default, ta dùng tọa độ cố định ở giữa dưới (fallback)
            // KHÔNG: Customer sẽ phải custom bước này cho đúng phone của họ
            loop.Children.Add(new AoaLoop
            {
                Type = AoaType.Click,
                Payload = new AoaClick
                {
                    X = 540,   // default center X
                    Y = 1200,  // default lower area
                    NumClicks = 7,
                    DelayBetweenMs = 400
                }
            });

            // Bước 6: Back
            loop.Children.Add(new AoaLoop
            {
                Type = AoaType.KeyPress,
                Payload = new AoaKeyPress { Key = "back", Repeat = 1, DelayBetweenMs = 300 }
            });

            // Bước 7: Đợi
            loop.Children.Add(new AoaLoop
            {
                Type = AoaType.Delay,
                Payload = 800
            });

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

            // Bước 9: Tìm và bật "USB debugging" toggle
            // KHÔNG: Customer phải custom vị trí toggle cho từng phone
            loop.Children.Add(new AoaLoop
            {
                Type = AoaType.Ocr,
                Payload = new AoaOcr
                {
                    Keywords = new[] { "USB debugging", "Gỡ lỗi USB", "Debugging" },
                    TimeoutMs = 5000,
                    IntervalMs = 500,
                    MaxSwipes = 5,
                    OffsetX = 300,  // Click lệch phải để bật toggle
                    OffsetY = 0
                }
            });

            // Bước 10: Xác nhận dialog "Allow USB debugging?"
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

            return loop;
        }

        public static async Task<AoaLoop> CloneForPhoneAsync(AoaLoop source, Phone phone)
        {
            // Serialize source → deserialize thành clone (deep copy)
            var json = JsonSerializer.Serialize(source, AoaLoopJsonContext.Default.AoaLoop);
            var cloned = JsonSerializer.Deserialize(json, AoaLoopJsonContext.Default.AoaLoop)!;

            cloned.Id = 0; // Sẽ được gán Id mới khi Add
            cloned.PhoneModel = phone.Model;
            cloned.ProcVersion = phone.ProcVersion;
            cloned.ProcCpuInfo = phone.ProcCpuInfo;
            cloned.API = phone.API;

            bool saved = AoaLoopRepository.Add(cloned);
            if (!saved)
            {
                // Race condition → lấy bản đã tồn tại
                return AoaLoopRepository.FindOneByUnique(
                    phone.Model, phone.ProcVersion, phone.ProcCpuInfo, phone.API) ?? cloned;
            }

            return cloned;
        }
    }
}
