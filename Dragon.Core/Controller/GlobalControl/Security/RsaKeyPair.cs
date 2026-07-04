using Dragon.Controller.Server.Services;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Dragon.Controller.GlobalControl.Security
{
    public static class RsaKeyPair
    {
        private static string? _cachedPublicKey;
        private static readonly string CachePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Dragon", "dragon_pub.key");

        private static async Task<string> GetPublicKeyAsync()
        {
            if (!string.IsNullOrEmpty(_cachedPublicKey)) return _cachedPublicKey;

            try
            {
                if (File.Exists(CachePath))
                {
                    var hashv2string = await File.ReadAllTextAsync(CachePath);
                    _cachedPublicKey = DecodeStringV2.DecodeV2(hashv2string);
                    if (!string.IsNullOrWhiteSpace(_cachedPublicKey)) return _cachedPublicKey;
                }
            }
            catch { }

            var response = await HttpServices.GetStringAsync("/api/crypto/publickey");
            try
            {
                using var doc = JsonDocument.Parse(response);
                _cachedPublicKey = doc.RootElement.TryGetProperty("key", out var k)
                   ? k.GetString()?.Trim()
                    : response.Trim();
            }
            catch { _cachedPublicKey = response.Trim(); }

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(CachePath)!);
                if (!string.IsNullOrWhiteSpace(_cachedPublicKey))
                    await File.WriteAllTextAsync(CachePath, DecodeStringV2.EncodeV2(_cachedPublicKey));
            }
            catch { }

            return _cachedPublicKey!;
        }

        // ==== ĐÃ SỬA CHO KHỚP SERVER ====
        public static async Task<string> MaHoa(string plainText)
        {
            var pubKey = await GetPublicKeyAsync();
            using var rsa = RSA.Create();
            rsa.ImportSubjectPublicKeyInfo(Convert.FromBase64String(pubKey), out _);

            using var aes = Aes.Create();
            aes.KeySize = 256; aes.Mode = CipherMode.CBC; aes.Padding = PaddingMode.PKCS7;
            aes.GenerateKey(); aes.GenerateIV();

            var cipher = aes.CreateEncryptor()
               .TransformFinalBlock(Encoding.UTF8.GetBytes(plainText), 0, plainText.Length);

            // server chỉ mã hóa KEY, không gộp IV
            var encKey = rsa.Encrypt(aes.Key, RSAEncryptionPadding.OaepSHA256);

            using var ms = new MemoryStream();
            using var bw = new BinaryWriter(ms); // little-endian giống server
            bw.Write(encKey.Length);
            bw.Write(encKey);
            bw.Write(aes.IV.Length);
            bw.Write(aes.IV);
            bw.Write(cipher);

            return Convert.ToBase64String(ms.ToArray());
        }

        public static string GiaiMa(string data) => data;

        public static void ClearCache()
        {
            _cachedPublicKey = null;
            try { if (File.Exists(CachePath)) File.Delete(CachePath); } catch { }
        }

        public static async Task<byte[]> MaHoaBytes(byte[] raw)
        {
            var pubKey = await GetPublicKeyAsync();
            using var rsa = RSA.Create();
            rsa.ImportSubjectPublicKeyInfo(Convert.FromBase64String(pubKey), out _);

            using var aes = Aes.Create();
            aes.KeySize = 256; aes.Mode = CipherMode.CBC; aes.Padding = PaddingMode.PKCS7;
            aes.GenerateKey(); aes.GenerateIV();

            var cipher = aes.CreateEncryptor().TransformFinalBlock(raw, 0, raw.Length);
            var encKey = rsa.Encrypt(aes.Key, RSAEncryptionPadding.OaepSHA256);

            using var ms = new MemoryStream();
            using var bw = new BinaryWriter(ms);
            bw.Write(encKey.Length);
            bw.Write(encKey);
            bw.Write(aes.IV.Length);
            bw.Write(aes.IV);
            bw.Write(cipher);
            return ms.ToArray();
        }

        public static byte[] GiaiMaBytes(byte[] data) => data;
    }
}