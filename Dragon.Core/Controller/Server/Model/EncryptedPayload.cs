using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dragon.Controller.Server.Model
{
    public class EncryptedPayload
    {
        public string Data { get; set; } = string.Empty;
    }

    public class EncryptedPayload2value
    {
        public string Data { get; set; } = string.Empty;      // user JSON đã mã hóa
        public string Data2 { get; set; } = string.Empty;// fingerprint đã mã hóa
    }

    public class RefreshRequest
    {
        public string RefreshToken { get; set; } = string.Empty;
    }

    // File: Shared/EncryptedBinaryPayload.cs
    public class EncryptedBinaryPayload
    {
        public byte[] Data { get; set; } = []; // System.Text.Json tự Base64
    }
    public class EncryptedBinaryPayloadV2
    {
        public byte[] Data { get; set; } = [];
        public string Data2 { get; set; } = "";
    }
}
