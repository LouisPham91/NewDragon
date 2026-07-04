using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dragon.Controller.GlobalControl.Setting
{
    public static class GetIcon
    {
        private static readonly string basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Extension", "Icon");
        private static readonly Dictionary<string, Icon> _cache = new(StringComparer.OrdinalIgnoreCase);

        static GetIcon()
        {
            if (!Directory.Exists(basePath))
                return;

            foreach (var file in Directory.GetFiles(basePath, "*.ico", SearchOption.TopDirectoryOnly))
            {
                string name = Path.GetFileNameWithoutExtension(file);
                if (!_cache.ContainsKey(name))
                {
                    try
                    {
                        _cache[name] = new Icon(file);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"❌ Lỗi khi load ảnh {file}: {ex.Message}");
                    }
                }
            }
        }

        public static Icon? GetIconByName(string name)
        {
            if (_cache.TryGetValue(name, out var originalIcon))
            {
                return (Icon?)originalIcon.Clone(); // Clone an toàn cho đa luồng
            }

            Console.WriteLine($"⚠️ Không tìm thấy ảnh tên: {name}");
            return null;
        }
    }


}
