using System;
using System.Collections.Generic;
using System.Text;

namespace Dragon.Controller.DeviceControl.AIHelper
{
    public static class ControllerExporter
    {
        public static void Run()
        {
            string[] targets = { "Controller", "Database" };

            foreach (var name in targets)
            {
                string? source = FindSource(name);
                if (source == null) continue;

                string outDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, name);
                Directory.CreateDirectory(outDir);

                CreateMasterFile(outDir, source, name);
            }
        }

        private static string? FindSource(string folder)
        {
            var dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            for (int i = 0; i < 6; i++)
            {
                var cand = Path.Combine(dir.FullName, "Dragon.Core", folder);
                if (Directory.Exists(cand)) return cand;
                dir = dir.Parent; if (dir == null) break;
            }
            return null;
        }

        private static void CreateMasterFile(string outputRoot, string sourceRoot, string prefix)
        {
            const int MAX_WORDS = 15000;
            var files = Directory.GetFiles(sourceRoot, "*.cs", SearchOption.AllDirectories).OrderBy(f => f);

            int part = 1, cur = 0;
            var sb = new StringBuilder();
            void Start() { sb.AppendLine($"// ===== {prefix.ToUpper()} PART {part} - {DateTime.Now} ====="); sb.AppendLine(); }
            Start();

            foreach (var f in files)
            {
                string rel = Path.GetRelativePath(sourceRoot, f);
                string block = $"// ===== FILE: {rel} =====\n{File.ReadAllText(f)}\n// ===== END: {rel} =====\n\n";
                int w = block.Split(new[] { ' ', '\r', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length;

                if (cur + w > MAX_WORDS && cur > 0)
                {
                    File.WriteAllText(Path.Combine(outputRoot, $"{prefix}{part}.txt"), sb.ToString(), new UTF8Encoding(true));
                    part++; cur = 0; sb.Clear(); Start();
                }
                sb.Append(block); cur += w;
            }
            if (sb.Length > 0)
                File.WriteAllText(Path.Combine(outputRoot, $"{prefix}{part}.txt"), sb.ToString(), new UTF8Encoding(true));
        }
    }
}
