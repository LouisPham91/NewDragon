
using System.Text.Json.Serialization;

namespace Dragon.Controller.DeviceControl.HATX.Core.Model
{
    public class AppCurrentInfo
    {
        [JsonPropertyName("package")]
        public string? Package { get; set; } = null;

        [JsonPropertyName("activity")]
        public string? Activity { get; set; } = null;

        [JsonPropertyName("pid")]
        public int Pid { get; set; } = -1;
    }
}
