using System;
using System.Collections.Generic;

namespace Dragon.Controller.DeviceControl.OTG
{
    public static class DeviceInfoLookup
    {
        /// <summary>
        /// Trả về (Brand, Model, Width, Height) từ VID/PID nếu có trong database
        /// Nếu không tìm thấy, trả về default 1080x1920
        /// </summary>
        public static (string brand, string model, int width, int height) GetDeviceInfo(ushort vid, ushort pid)
        {
            // Ưu tiên tìm exact match VID+PID
            var key = new VidPidKey { Vid = vid, Pid = pid };
            if (VidPidDatabase.TryGetValue(key, out var info))
                return info;

            // Fallback: tìm theo VID (hãng)
            if (VidBrandFallback.TryGetValue(vid, out var fallback))
            {
                var (brand, w, h) = fallback; // Deconstruct ra 3 giá trị
                return (brand, "Unknown", w, h); // Thêm model = "Unknown"
            }

            // Default fallback
            return ("Unknown", "Unknown", 1080, 1920);
        }

        public static (int width, int height) GetPhysicalSize(ushort vid, ushort pid)
        {
            var (_, _, w, h) = GetDeviceInfo(vid, pid);
            return (w, h);
        }

        public static string GetBrand(ushort vid)
        {
            if (VidBrandFallback.TryGetValue(vid, out var fallback))
                return fallback.brand;
            return "Unknown";
        }

        // ===== STRUCT LÀM KEY (thay vì ValueTuple) =====
        private struct VidPidKey : IEquatable<VidPidKey>
        {
            public ushort Vid;
            public ushort Pid;

            public bool Equals(VidPidKey other) => Vid == other.Vid && Pid == other.Pid;
            public override bool Equals(object? obj) => obj is VidPidKey other && Equals(other);
            public override int GetHashCode() => HashCode.Combine(Vid, Pid);
        }

        // ===== STRUCT LÀM VALUE (thay vì ValueTuple named) =====
        private struct DeviceInfoValue
        {
            public string Brand;
            public string Model;
            public int Width;
            public int Height;

            public void Deconstruct(out string brand, out string model, out int width, out int height)
            {
                brand = Brand;
                model = Model;
                width = Width;
                height = Height;
            }

            public static implicit operator DeviceInfoValue((string brand, string model, int width, int height) tuple)
                => new DeviceInfoValue { Brand = tuple.brand, Model = tuple.model, Width = tuple.width, Height = tuple.height };

            public static implicit operator (string brand, string model, int width, int height)(DeviceInfoValue value)
                => (value.Brand, value.Model, value.Width, value.Height);
        }

        // ===== DATABASE VID+PID -> DeviceInfo =====
        private static readonly Dictionary<VidPidKey, DeviceInfoValue> VidPidDatabase = new()
        {
            // Samsung
            [new VidPidKey { Vid = 0x04E8, Pid = 0x6860 }] = ("Samsung", "Galaxy S23", 1080, 2340),
            [new VidPidKey { Vid = 0x04E8, Pid = 0x6861 }] = ("Samsung", "Galaxy S22", 1080, 2340),
            [new VidPidKey { Vid = 0x04E8, Pid = 0x6862 }] = ("Samsung", "Galaxy S21", 1080, 2400),
            [new VidPidKey { Vid = 0x04E8, Pid = 0x6863 }] = ("Samsung", "Galaxy A54", 1080, 2340),
            [new VidPidKey { Vid = 0x04E8, Pid = 0x6864 }] = ("Samsung", "Galaxy A34", 1080, 2340),
            [new VidPidKey { Vid = 0x04E8, Pid = 0x6865 }] = ("Samsung", "Galaxy Tab A8", 1200, 1920),

            // Xiaomi
            [new VidPidKey { Vid = 0x2717, Pid = 0x0001 }] = ("Xiaomi", "Redmi Note 12", 1080, 2400),
            [new VidPidKey { Vid = 0x2717, Pid = 0x0002 }] = ("Xiaomi", "Redmi Note 11", 1080, 2400),
            [new VidPidKey { Vid = 0x2717, Pid = 0x0003 }] = ("Xiaomi", "Redmi 12C", 720, 1650),
            [new VidPidKey { Vid = 0x2717, Pid = 0x0004 }] = ("Xiaomi", "Poco X5", 1080, 2400),

            // Google Pixel
            [new VidPidKey { Vid = 0x18D1, Pid = 0x4E11 }] = ("Google", "Pixel 7", 1080, 2400),
            [new VidPidKey { Vid = 0x18D1, Pid = 0x4E12 }] = ("Google", "Pixel 6", 1080, 2400),
            [new VidPidKey { Vid = 0x18D1, Pid = 0x4E13 }] = ("Google", "Pixel 5", 1080, 2340),

            // OnePlus
            [new VidPidKey { Vid = 0x2A70, Pid = 0x0001 }] = ("OnePlus", "OnePlus 11", 1440, 3216),
            [new VidPidKey { Vid = 0x2A70, Pid = 0x0002 }] = ("OnePlus", "OnePlus 10", 1080, 2412),
            [new VidPidKey { Vid = 0x2A70, Pid = 0x0003 }] = ("OnePlus", "OnePlus Nord", 1080, 2400),

            // Huawei
            [new VidPidKey { Vid = 0x12D1, Pid = 0x0001 }] = ("Huawei", "P40", 1080, 2340),
            [new VidPidKey { Vid = 0x12D1, Pid = 0x0002 }] = ("Huawei", "P30", 1080, 2340),

            // Oppo
            [new VidPidKey { Vid = 0x22D9, Pid = 0x0001 }] = ("Oppo", "Reno 10", 1080, 2412),
            [new VidPidKey { Vid = 0x22D9, Pid = 0x0002 }] = ("Oppo", "A78", 720, 1612),

            // Vivo
            [new VidPidKey { Vid = 0x2D95, Pid = 0x0001 }] = ("Vivo", "V27", 1080, 2400),
            [new VidPidKey { Vid = 0x2D95, Pid = 0x0002 }] = ("Vivo", "Y35", 720, 1600),

            // Realme
            [new VidPidKey { Vid = 0x3310, Pid = 0x0001 }] = ("Realme", "Realme 11", 1080, 2400),
            [new VidPidKey { Vid = 0x3310, Pid = 0x0002 }] = ("Realme", "Realme C55", 720, 1600),

            // Motorola
            [new VidPidKey { Vid = 0x22B8, Pid = 0x0001 }] = ("Motorola", "Moto G84", 1080, 2400),
            [new VidPidKey { Vid = 0x22B8, Pid = 0x0002 }] = ("Motorola", "Moto E14", 720, 1600),

            // Transsion
            [new VidPidKey { Vid = 0x2996, Pid = 0x0001 }] = ("Transsion", "Tecno Camon 20", 1080, 2460),
            [new VidPidKey { Vid = 0x2996, Pid = 0x0002 }] = ("Transsion", "Infinix Note 30", 1080, 2460),
        };

        // ===== FALLBACK THEO VID =====
        private struct BrandFallbackValue
        {
            public string brand;
            public int width;
            public int height;

            public void Deconstruct(out string brand, out int width, out int height)
            {
                brand = this.brand;
                width = this.width;
                height = this.height;
            }

            public static implicit operator BrandFallbackValue((string brand, int width, int height) tuple)
                => new BrandFallbackValue { brand = tuple.brand, width = tuple.width, height = tuple.height };

            public static implicit operator (string brand, int width, int height)(BrandFallbackValue value)
                => (value.brand, value.width, value.height);
        }

        private static readonly Dictionary<ushort, BrandFallbackValue> VidBrandFallback = new()
        {
            [0x04E8] = ("Samsung", 1080, 2340),
            [0x18D1] = ("Google", 1080, 2400),
            [0x0BB4] = ("HTC", 1080, 1920),
            [0x0FCE] = ("Sony", 1080, 2520),
            [0x1004] = ("LG", 1080, 2340),
            [0x12D1] = ("Huawei", 1080, 2340),
            [0x2717] = ("Xiaomi", 1080, 2400),
            [0x2718] = ("Xiaomi", 1080, 2400),
            [0x2A70] = ("OnePlus", 1080, 2412),
            [0x05C6] = ("Qualcomm", 1080, 1920),
            [0x22B8] = ("Motorola", 1080, 2400),
            [0x22D9] = ("Oppo", 1080, 2412),
            [0x2D95] = ("Vivo", 1080, 2400),
            [0x2970] = ("Vivo", 1080, 2400),
            [0x0B05] = ("Asus", 1080, 2400),
            [0x17EF] = ("Lenovo", 1080, 1920),
            [0x19D2] = ("ZTE", 1080, 2400),
            [0x1EBF] = ("Nothing", 1080, 2412),
            [0x1BBB] = ("TCL/Alcatel", 720, 1600),
            [0x2836] = ("TCL", 720, 1600),
            [0x2A45] = ("Meizu", 1080, 1920),
            [0x1949] = ("Amazon", 1200, 1920),
            [0x0955] = ("Nvidia", 1920, 1080),
            [0x3310] = ("Realme", 1080, 2400),
            [0x2996] = ("Transsion", 1080, 2460),
            [0x0421] = ("Nokia HMD", 720, 1600),
            [0x109B] = ("Hisense", 1080, 2400),
            [0x2207] = ("Rockchip", 1280, 720),
            [0x1D91] = ("Allwinner", 1280, 720),
            [0x2B4C] = ("Black Shark", 1080, 2400),
            [0x0E8D] = ("MediaTek", 720, 1600),
            [0x1782] = ("Unisoc", 720, 1600),
        };
    }
}