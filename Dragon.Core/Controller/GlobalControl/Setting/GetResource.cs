using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dragon.Controller.GlobalControl.Setting
{
    public static class GetResource
    {

        private static readonly string basePath = Path.Combine(AppContext.BaseDirectory, "Extension", "Image");

        private static readonly Dictionary<string, Image> _cache = new(StringComparer.OrdinalIgnoreCase);

        static GetResource()
        {
            if (!Directory.Exists(basePath))
                return;

            foreach (var file in Directory.GetFiles(basePath, "*.png", SearchOption.TopDirectoryOnly))
            {
                string name = Path.GetFileNameWithoutExtension(file);
                if (!_cache.ContainsKey(name))
                {
                    try
                    {
                        _cache[name] = Image.FromFile(file);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"❌ Lỗi khi load ảnh {file}: {ex.Message}");
                    }
                }
            }
        }

        public static Image? GetImageByName(string name)
        {
            if (_cache.TryGetValue(name, out var originalImage))
            {
                return (Image?)originalImage.Clone(); // Clone an toàn đa luồng
            }

           // Console.WriteLine($"⚠️ Không tìm thấy ảnh tên: {name}");
            return null;
        }
        static Random random = new Random();
        public static Image? GetRandomImage(string startName)
        {
            var matchingKeys = _cache.Keys.Where(k => k.StartsWith(startName, StringComparison.OrdinalIgnoreCase)).ToList();

            if (matchingKeys.Count == 0)
            {
                Console.WriteLine($"⚠️ Không tìm thấy ảnh nào bắt đầu với: {startName}");
                return null;
            }
            var randomKey = matchingKeys[random.Next(0, matchingKeys.Count())];
            var originalImage = _cache[randomKey];

            return (Image?)originalImage.Clone(); // Clone để tránh lỗi đa luồng
        }


    }
}
