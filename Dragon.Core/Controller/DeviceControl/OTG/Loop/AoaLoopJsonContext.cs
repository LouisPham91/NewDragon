using System.Text.Json.Serialization;
using Dragon.Controller.DeviceControl.OTG.Model;

namespace Dragon.Controller.DeviceControl.OTG.Loop
{
    [JsonSerializable(typeof(AoaLoop))]
    [JsonSerializable(typeof(List<AoaLoop>))]  // <-- THÊM
    [JsonSerializable(typeof(AoaClick))]
    [JsonSerializable(typeof(AoaSwipe))]
    [JsonSerializable(typeof(AoaDeeplink))]
    [JsonSerializable(typeof(AoaKeyPress))]
    [JsonSerializable(typeof(AoaOcr))]
    [JsonSerializable(typeof(AoaSendText))]
    [JsonSerializable(typeof(int))]
    [JsonSourceGenerationOptions(
        PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
        WriteIndented = false)]
    public partial class AoaLoopJsonContext : JsonSerializerContext
    {
    }
}