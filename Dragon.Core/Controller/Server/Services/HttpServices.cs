using Dragon.Controller.Server.Model;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace Dragon.Controller.Server.Services
{
    public static class HttpServices
    {
        public const string _host = "http://localhost:5213";
        //public const string _host = "http://dragonphonefarm.ooguy.com";

        private static readonly HttpClient _httpClient = new HttpClient();

        // --- THÊM MỚI: lấy string thuần (dùng cho public key) ---
        public static async Task<string> GetStringAsync(string endpoint, string baseUrl = _host)
        {
            var resp = await _httpClient.GetAsync($"{baseUrl}{endpoint}");
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadAsStringAsync();
        }


        public static async Task<HttpResponseMessage> PostJsonAsync<T>(
        T data, 
        JsonTypeInfo<T> typeInfo, 
        string endpoint = "/api/User/login/", 
        string baseUrl = _host)
        {
            string fullUrl = $"{baseUrl}{endpoint}";
            var json = JsonSerializer.Serialize(data, typeInfo);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync(fullUrl, content);
        }

        public static async Task<HttpResponseMessage> PostJsonAuthAsync(
        EncryptedPayload payload,
        JsonTypeInfo<EncryptedPayload> typeInfo,
        string token,
        string endpoint = "/api/Computer/byhash/",
        string baseUrl = _host)
        {
            string fullUrl = $"{baseUrl}{endpoint}";
            var json = JsonSerializer.Serialize(payload, typeInfo);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Đổi sang POST
            var request = new HttpRequestMessage(HttpMethod.Post, fullUrl)
            {
                Content = content
            };

            // Thêm Authorization header
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            return await _httpClient.SendAsync(request);
        }

        public static async Task<HttpResponseMessage> PostJsonAuth2ValueAsync(
        EncryptedPayload2value payload,
        JsonTypeInfo<EncryptedPayload2value> typeInfo,
        string token,
        string endpoint = "/api/Computer/transfer/",
        string baseUrl = _host)
        {
            string fullUrl = $"{baseUrl}{endpoint}";
            var json = JsonSerializer.Serialize(payload, typeInfo);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Đổi sang POST
            var request = new HttpRequestMessage(HttpMethod.Post, fullUrl)
            {
                Content = content
            };

            // Thêm Authorization header
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            return await _httpClient.SendAsync(request);
        }

        public static async Task<HttpResponseMessage> PostAuthAsync(
        string token,
        string endpoint = "/api/User/getUser",
        string baseUrl = _host)
        {
            string fullUrl = $"{baseUrl}{endpoint}";

            // Vì server không yêu cầu body cụ thể, ta có thể gửi body rỗng
            var content = new StringContent("{}", Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, fullUrl)
            {
                Content = content
            };

            // Thêm Authorization header
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            return await _httpClient.SendAsync(request);
        }

        public static async Task<T?> ReadJsonAsync<T>(HttpResponseMessage response, JsonTypeInfo<T> typeInfo)
        {
            if (!response.IsSuccessStatusCode) return default;
            var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync(stream, typeInfo);
        }

        public static async Task<HttpResponseMessage> GetJsonAuthAsync(
        EncryptedPayload payload,
        JsonTypeInfo<EncryptedPayload> typeInfo,
        string token,
        string endpoint = "/api/Computer/byhash/",
        string baseUrl = _host)
        {
            string fullUrl = $"{baseUrl}{endpoint}";
            var json = JsonSerializer.Serialize(payload, typeInfo);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Get, fullUrl)
            {
                Content = content
            };

            // Thêm Authorization header
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            return await _httpClient.SendAsync(request);
        }

        


        public static async Task<HttpResponseMessage> GetAuthAsync(
        string token,
        string endpoint = "/api/Computer/Any",
        string baseUrl = _host)
        {
            string fullUrl = $"{baseUrl}{endpoint}";

            var request = new HttpRequestMessage(HttpMethod.Get, fullUrl);

            // Thêm Authorization header
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            return await _httpClient.SendAsync(request);
        }

        public static async Task<HttpResponseMessage> PostJsonAuthAsync<T>(
        T payload,
        JsonTypeInfo<T> typeInfo,
        string token,
        string endpoint,
        string baseUrl = _host)
        {
            var json = JsonSerializer.Serialize(payload, typeInfo);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var req = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}{endpoint}") { Content = content };
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await _httpClient.SendAsync(req);
        }

        public static async Task<HttpResponseMessage> DeleteAuthAsync(
        string token,
        string endpoint,
        string baseUrl = _host)
        {
            var req = new HttpRequestMessage(HttpMethod.Delete, $"{baseUrl}{endpoint}");
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await _httpClient.SendAsync(req);
        }

        public static async Task<HttpResponseMessage> PostV2Async(
        EncryptedBinaryPayloadV2 payload,
        string token,
        string endpoint = "/api/dloop/save",
        string baseUrl = _host)
        {
            var json = JsonSerializer.Serialize(payload, JsonServer.Default.EncryptedBinaryPayloadV2);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var req = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}{endpoint}")
            {
                Content = content
            };
            req.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            return await _httpClient.SendAsync(req);
        }

    }
}
