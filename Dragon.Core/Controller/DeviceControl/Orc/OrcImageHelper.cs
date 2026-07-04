using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Text;
using TesseractOCR;
using TesseractOCR.Enums;
using TesseractOCR.Pix;

namespace Dragon.Controller.DeviceControl.Orc
{
    public static class OrcImageHelper
    {
        private static readonly ConcurrentDictionary<string, Engine> _engines = new();

        public static void WarmUpOcr(string testDataPath = @"tessdata")
        {
            try
            {
                _engines.GetOrAdd("vie", _ => new Engine(Path.GetFullPath(testDataPath), "vie", EngineMode.Default));
                _engines.GetOrAdd("eng", _ => new Engine(Path.GetFullPath(testDataPath), "eng", EngineMode.Default));
                Debug.WriteLine("OCR WarmUp: vie + eng loaded");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"WarmUp failed: {ex.Message}");
            }
        }


        public static string GetImageText(string imagePath, string languge = "eng", string testDataPath = "tessdata")
        {
            try
            {
                using var engine = new Engine(Path.GetFullPath(testDataPath), languge, EngineMode.Default);
                using var img = TesseractOCR.Pix.Image.LoadFromFile(imagePath);
                using var page = engine.Process(img, PageSegMode.Auto);
                return page.Text ?? string.Empty;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Lỗi OCR: {ex.Message}");
                return string.Empty;
            }
        }

        public static string GetImageTextFromBitmap(Bitmap bitmap, string testDataPath = @"tessdata")
        {
            return GetImageTextFromBitmap(bitmap, TaskDeviceManager.Model.DatabaseColumn.Language.English, testDataPath);
        }

        public static string GetImageTextFromBitmap(Bitmap bitmap, TaskDeviceManager.Model.DatabaseColumn.Language language, string testDataPath = @"tessdata")
        {
            try
            {
                string lang = language == TaskDeviceManager.Model.DatabaseColumn.Language.TiengViet ? "vie" : "eng";
                var engine = _engines.GetOrAdd(lang, l => new Engine(Path.GetFullPath(testDataPath), l, EngineMode.Default));
                using var ms = new MemoryStream();
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                ms.Position = 0;
                using var img = TesseractOCR.Pix.Image.LoadFromMemory(ms);
                using var page = engine.Process(img, PageSegMode.Auto);
                return page.Text ?? string.Empty;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Lỗi OCR: {ex.Message}");
                return string.Empty;
            }
        }


        public static Point? GetPointImageByText(Bitmap bitmap, string textToFind, string tessDataPath = @"tessdata")
        {
            if (bitmap == null || string.IsNullOrWhiteSpace(textToFind))
                return null;

            try
            {
                // LUÔN dùng vie vì màn hình tiếng Việt có dấu
                var engine = _engines.GetOrAdd("vie", _ =>
                {
                    string path = Path.GetFullPath(tessDataPath);
                    return new Engine(path, "vie", EngineMode.LstmOnly);
                });

                // Convert Bitmap -> Pix
                using var ms = new MemoryStream();
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                using var pix = TesseractOCR.Pix.Image.LoadFromMemory(ms.ToArray());

                using var page = engine.Process(pix, PageSegMode.Auto);

                // Lấy TSV
                string tsv = page.TsvText;
                if (string.IsNullOrEmpty(tsv))
                    return null;

                // Parse TSV thành dictionary theo dòng
                var linesDict = new Dictionary<string, List<WordInfo>>();

                foreach (var row in tsv.Split('\n'))
                {
                    var cols = row.Split('\t');
                    if (cols.Length < 12) continue;

                    // level 5 = word
                    if (!int.TryParse(cols[0], out int level) || level != 5)
                        continue;

                    string text = cols[11]?.Trim() ?? "";
                    if (string.IsNullOrEmpty(text)) continue;

                    if (!int.TryParse(cols[2], out int block)) continue;
                    if (!int.TryParse(cols[3], out int par)) continue;
                    if (!int.TryParse(cols[4], out int line)) continue;
                    if (!int.TryParse(cols[5], out int wordNum)) continue;
                    if (!int.TryParse(cols[6], out int left)) continue;
                    if (!int.TryParse(cols[7], out int top)) continue;
                    if (!int.TryParse(cols[8], out int width)) continue;
                    if (!int.TryParse(cols[9], out int height)) continue;
                    if (!float.TryParse(cols[10], out float conf)) conf = 0;

                    string key = $"{block}-{par}-{line}";

                    if (!linesDict.ContainsKey(key))
                        linesDict[key] = new List<WordInfo>();

                    linesDict[key].Add(new WordInfo
                    {
                        Block = block,
                        Par = par,
                        Line = line,
                        WordNum = wordNum,
                        Left = left,
                        Top = top,
                        Right = left + width,
                        Bottom = top + height,
                        Text = text,
                        Confidence = conf
                    });
                }

                // Duyệt từng dòng
                foreach (var kvp in linesDict)
                {
                    var words = kvp.Value.OrderBy(w => w.WordNum).ToList();
                    string lineText = string.Join(" ", words.Select(w => w.Text));

                    Debug.WriteLine($"[LINE {kvp.Key}] '{lineText}'");

                    // So sánh trực tiếp có dấu
                    if (lineText.IndexOf(textToFind, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        // Tìm vị trí chính xác của cụm trong dòng
                        var targetWords = textToFind.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                        for (int i = 0; i <= words.Count - targetWords.Length; i++)
                        {
                            var slice = words.Skip(i).Take(targetWords.Length).ToList();
                            string sliceText = string.Join(" ", slice.Select(s => s.Text));

                            if (sliceText.Equals(textToFind, StringComparison.OrdinalIgnoreCase))
                            {
                                int left = slice.Min(s => s.Left);
                                int top = slice.Min(s => s.Top);
                                int right = slice.Max(s => s.Right);
                                int bottom = slice.Max(s => s.Bottom);

                                int cx = (left + right) / 2;
                                int cy = (top + bottom) / 2;

                                double avgConf = slice.Average(s => s.Confidence);

                                Debug.WriteLine(
                                    $"[FOUND] '{sliceText}' | Conf: {avgConf:F1}% | at ({cx},{cy})");

                                return new Point(cx, cy);
                            }
                        }

                        // Fallback: trả về tâm cả dòng nếu không khớp từng từ
                        int l = words.Min(w => w.Left);
                        int t = words.Min(w => w.Top);
                        int r = words.Max(w => w.Right);
                        int b = words.Max(w => w.Bottom);
                        return new Point((l + r) / 2, (t + b) / 2);
                    }
                }

                Debug.WriteLine($"[NOT FOUND] '{textToFind}'");
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"OCR Error: {ex.Message}");
                return null;
            }
        }

        // Class phụ
        private class WordInfo
        {
            public int Block { get; set; }
            public int Par { get; set; }
            public int Line { get; set; }
            public int WordNum { get; set; }
            public int Left { get; set; }
            public int Top { get; set; }
            public int Right { get; set; }
            public int Bottom { get; set; }
            public string Text { get; set; } = "";
            public float Confidence { get; set; }
        }

    }
}
