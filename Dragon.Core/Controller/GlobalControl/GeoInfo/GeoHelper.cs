using System.Text.Json;
using System.Text.Json.Serialization;

namespace Dragon.Controller.GlobalControl.GeoInfo
{

    public class GeoInfo
    {
        public string status { get; set; } = string.Empty;
        public string country { get; set; } = string.Empty;
        public string countryCode { get; set; } = string.Empty;
        public string region { get; set; } = string.Empty;
        public string regionName { get; set; } = string.Empty;
        public string city { get; set; } = string.Empty;
        public string zip { get; set; } = string.Empty;
        public double lat { get; set; }
        public double lon { get; set; }
        public string timezone { get; set; } = string.Empty;
        public string isp { get; set; } = string.Empty;
        public string org { get; set; } = string.Empty;
        [JsonPropertyName("as")]
        public string As { get; set; } = string.Empty; // đổi tên property trong C# để tránh trùng từ khóa

        public string query { get; set; } = string.Empty;
    }


    [JsonSerializable(typeof(GeoInfo))]
    public partial class JsonGeo : JsonSerializerContext { }

    public class GeoHelper
    {
        public static GeoInfo? GeoInfo = null;
        private static readonly HttpClient client = new HttpClient();

        public static GeoInfo? GetGeoInfo()
        {
            using var client = new HttpClient();

            string ip = client.GetStringAsync("https://api.ipify.org").GetAwaiter().GetResult();
            string json = client.GetStringAsync($"http://ip-api.com/json/{ip}").GetAwaiter().GetResult();


            var value = JsonSerializer.Deserialize(json, JsonGeo.Default.GeoInfo);
            return value;
        }

    }

}
