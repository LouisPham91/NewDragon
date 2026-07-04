using System.Text.Json.Serialization;

namespace Dragon.ControlHelper.WebSite;

[JsonSourceGenerationOptions(WriteIndented = true, PropertyNameCaseInsensitive = true)]
[JsonSerializable(typeof(HostObjectInfo))]
internal partial class JsonSourceGenerationContext : JsonSerializerContext
{

}
