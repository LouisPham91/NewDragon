using Dragon.Controller.DeviceControl.HATX.Core.Model;
using System.Text.Json.Serialization;

namespace Dragon.Controller.DeviceControl.HATX.Core
{
    [JsonSerializable(typeof(AtxInfo))]
    [JsonSerializable(typeof(HierarchyResponse))]
    [JsonSerializable(typeof(TouchCmd))]
    internal partial class AtxJsonContext : JsonSerializerContext { }

}
