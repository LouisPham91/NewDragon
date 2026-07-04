using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Dragon.Controller.TaskDeviceManager.Core
{
    public static class DLoopCompressor
    {
        public static byte[] Pack(DLoop dloop)
        {
            var json = JsonSerializer.Serialize(dloop, DLoopJsonContext.Default.DLoop);
            var bytes = Encoding.UTF8.GetBytes(json);
            using var ms = new MemoryStream();
            using (var bz = new BrotliStream(ms, CompressionLevel.Optimal, true))
                bz.Write(bytes, 0, bytes.Length);
            return ms.ToArray();
        }

        public static DLoop Unpack(byte[] data)
        {
            using var ms = new MemoryStream(data);
            using var bz = new BrotliStream(ms, CompressionMode.Decompress);
            using var outMs = new MemoryStream();
            bz.CopyTo(outMs);
            var json = Encoding.UTF8.GetString(outMs.ToArray());
            return JsonSerializer.Deserialize(json, DLoopJsonContext.Default.DLoop)!;
        }
    }
}
