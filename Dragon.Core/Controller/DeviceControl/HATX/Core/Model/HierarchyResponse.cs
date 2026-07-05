
using System.Text.Json.Serialization;

namespace Dragon.Controller.DeviceControl.HATX.Core.Model
{
    public class HierarchyResponse
    {
        [JsonPropertyName("jsonrpc")] 
        public string? Jsonrpc { get; set; }
        [JsonPropertyName("id")] 
        public long Id { get; set; }
        [JsonPropertyName("result")] 
        public string? Result { get; set; } 
    }
}
