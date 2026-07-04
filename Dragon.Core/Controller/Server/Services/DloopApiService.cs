using Dragon.Controller.GlobalControl.Property;
using Dragon.Controller.GlobalControl.Security;
using Dragon.Controller.Server.Model;
using Dragon.Controller.Server.Services;
using System.IO.Compression;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace Dragon.Controller.Server.Services
{
    public class DloopApiService
    {
        private static byte[] Pack<T>(T obj, JsonTypeInfo<T> typeInfo)
        {
            var json = JsonSerializer.Serialize(obj, typeInfo);
            var b = Encoding.UTF8.GetBytes(json);
            using var ms = new MemoryStream();
            using var z = new BrotliStream(ms, CompressionLevel.Optimal, true);
            z.Write(b, 0, b.Length); z.Flush();
            return ms.ToArray();
        }
        private static T Unpack<T>(byte[] data, JsonTypeInfo<T> typeInfo)
        {
            using var ms = new MemoryStream(data);
            using var z = new BrotliStream(ms, CompressionMode.Decompress);
            using var o = new MemoryStream(); z.CopyTo(o);
            return JsonSerializer.Deserialize(o.ToArray(), typeInfo)!;
        }

        private async Task<EncryptedBinaryPayloadV2> BuildV2(byte[] raw)
        {
            var encData = await RsaKeyPair.MaHoaBytes(raw);
            var fp = $"{GetSettings.GetFingerPrint()},{GetSettings.GetWebFingerPrint()}";
            return new EncryptedBinaryPayloadV2
            {
                Data = encData,
                Data2 = await RsaKeyPair.MaHoa(fp)
            };
        }

        public async Task<(bool ok, Guid id, string err)> SaveAsync(ServerDloop dto)
        {
            dto.OwnerGmail = GetSettings.GetUserEmail();
            var payload = await BuildV2(Pack(dto, JsonServer.Default.ServerDloop));
            var resp = await HttpServices.PostV2Async(payload, GetSettings.GetAccessToken(), "/api/dloop/save");
            var body = await resp.Content.ReadAsStringAsync();
            if (!resp.IsSuccessStatusCode) return (false, Guid.Empty, body);
            // server trả plain Guid json
            var id = JsonSerializer.Deserialize(body, JsonServer.Default.Guid);
            return (true, id, "");
        }

        public async Task<(bool ok, string err)> DeleteAsync(Guid id)
        {
            var payload = await BuildV2(Pack(id, JsonServer.Default.Guid));
            var resp = await HttpServices.PostV2Async(payload, GetSettings.GetAccessToken(), "/api/dloop/delete");
            return (resp.IsSuccessStatusCode, await resp.Content.ReadAsStringAsync());
        }

        public async Task<(bool ok, List<ServerDloop>? data, string err)> MineAsync()
        {
            var payload = await BuildV2(Array.Empty<byte>());
            var resp = await HttpServices.PostV2Async(payload, GetSettings.GetAccessToken(), "/api/dloop/mine");
            var body = await resp.Content.ReadAsStringAsync();
            if (!resp.IsSuccessStatusCode) return (false, null, body);
            var list = JsonSerializer.Deserialize(body, JsonServer.Default.ListServerDloop);
            return (true, list, "");
        }

        public async Task<(bool ok, ServerDloop? data, string err)> GetAsync(Guid id)
        {
            var payload = await BuildV2(Pack(id, JsonServer.Default.Guid));
            var resp = await HttpServices.PostV2Async(payload, GetSettings.GetAccessToken(), "/api/dloop/get");
            var body = await resp.Content.ReadAsStringAsync();
            if (!resp.IsSuccessStatusCode) return (false, null, body);
            var d = JsonSerializer.Deserialize(body, JsonServer.Default.ServerDloop);
            return (true, d, "");
        }

        public async Task<(bool ok, string err)> ShareAsync(ShareRequest req)
        {
            var payload = await BuildV2(Pack(req, JsonServer.Default.ShareRequest));
            var resp = await HttpServices.PostV2Async(payload, GetSettings.GetAccessToken(), "/api/dloop/share");
            return (resp.IsSuccessStatusCode, await resp.Content.ReadAsStringAsync());
        }
    }
}