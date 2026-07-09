using System.Text.Json;
using System.Text.Json.Serialization;
using Dragon.Controller.DeviceControl.OTG.Model;

namespace Dragon.Controller.DeviceControl.OTG.Loop
{
    public class AoaLoop
    {
        public int Id { get; set; }
        public string PhoneModel { get; set; } = string.Empty;
        public string ProcVersion { get; set; } = string.Empty;
        public string ProcCpuInfo { get; set; } = string.Empty;
        public int API { get; set; }
        public string PointCloseApp { get; set; } = "50,90"; // percent
        public int PhysicalWidth { get; set; } = 1080;
        public int PhysicalHeight { get; set; } = 1920;
        public bool IsAppCaptureConnected { get; set; } = false;

        // === THÊM MỚI ===
        public AoaType Type { get; set; }

        public string ArgsJson { get; set; } = "{}";

        [JsonIgnore]
        public object? Payload { get; set; }

        public List<AoaLoop> Children { get; set; } = new();

        // Giữ lại hàm cũ nếu cần tương thích DLoop
        public void SetArgs<T>(T obj) where T : notnull
        {
            Payload = obj;
            ArgsJson = JsonSerializer.Serialize(obj, typeof(T), AoaLoopJsonContext.Default);
        }

        public T GetArgs<T>() where T : notnull
        {
            if (Payload is T t) return t;

            var val = JsonSerializer.Deserialize(ArgsJson, typeof(T), AoaLoopJsonContext.Default);
            if (val is null)
                throw new InvalidOperationException($"Không deserialize được {typeof(T).Name}");

            Payload = val;
            return (T)val;
        }

        // === HÀM MỚI: Hydrate dựa vào Type enum ===
        public void HydratePayload()
        {
            if (Payload != null) return;
            if (string.IsNullOrEmpty(ArgsJson) || ArgsJson == "{}") return;

            Payload = Type switch
            {
                AoaType.Click => JsonSerializer.Deserialize(ArgsJson, AoaLoopJsonContext.Default.AoaClick),
                AoaType.Swipe => JsonSerializer.Deserialize(ArgsJson, AoaLoopJsonContext.Default.AoaSwipe),
                AoaType.Drag => JsonSerializer.Deserialize(ArgsJson, AoaLoopJsonContext.Default.AoaSwipe),
                AoaType.Deeplink => JsonSerializer.Deserialize(ArgsJson, AoaLoopJsonContext.Default.AoaDeeplink),
                AoaType.KeyPress => JsonSerializer.Deserialize(ArgsJson, AoaLoopJsonContext.Default.AoaKeyPress),
                AoaType.Ocr => JsonSerializer.Deserialize(ArgsJson, AoaLoopJsonContext.Default.AoaOcr),
                AoaType.SendText => JsonSerializer.Deserialize(ArgsJson, AoaLoopJsonContext.Default.AoaSendText),
                AoaType.Delay => JsonSerializer.Deserialize(ArgsJson, AoaLoopJsonContext.Default.Int32),
                AoaType.CloseAllApps => PointCloseApp, // Dùng thẳng PointCloseApp string
                _ => null
            };
        }

        // Hydrate toàn bộ cây
        public void HydrateTree()
        {
            HydratePayload();
            if (Children != null)
            {
                foreach (var child in Children)
                    child.HydrateTree();
            }
        }

    }
}