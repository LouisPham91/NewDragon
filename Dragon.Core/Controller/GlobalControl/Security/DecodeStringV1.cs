using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dragon.Controller.GlobalControl.Security
{
    public class DecodeStringV1
    {

        // ==== Cặp 1: RC4-like + Unicode-escape ====
        private static readonly byte[] Key1 = Encoding.UTF8.GetBytes("mY@w3s0m3_S3cr3t_K3y!");

        public static string EncodeV1(string plain)
        {
            var data = Encoding.UTF8.GetBytes(plain);
            var cipher = Rc4Transform(data, Key1);
            var sb = new StringBuilder(cipher.Length * 6);
            // mỗi byte → "\u00XX"
            foreach (byte b in cipher)
                sb.AppendFormat("/u00{0:X2}", b);
            return sb.ToString();
        }

        public static string DecodeV1(string encoded)
        {
            // bắt block 6 ký tự: "\u00" + 2 hex
            if (encoded.Length % 6 != 0)
                throw new FormatException("Độ dài không chia hết cho 6.");

            int count = encoded.Length / 6;
            var data = new byte[count];
            for (int i = 0; i < count; i++)
            {
                string hex = encoded.Substring(i * 6 + 4, 2); // skip "\u00"
                data[i] = Convert.ToByte(hex, 16);
            }

            var plain = Rc4Transform(data, Key1);
            return Encoding.UTF8.GetString(plain);
        }

        private static byte[] Rc4Transform(byte[] data, byte[] key)
        {
            byte[] S = Enumerable.Range(0, 256).Select(i => (byte)i).ToArray(),
                   T = new byte[256];
            for (int i = 0; i < 256; i++) T[i] = key[i % key.Length];
            int j = 0;
            for (int i = 0; i < 256; i++)
            {
                j = j + S[i] + T[i] & 0xFF;
                (S[i], S[j]) = (S[j], S[i]);
            }
            var outp = new byte[data.Length];
            int i1 = 0, j1 = 0;
            for (int k = 0; k < data.Length; k++)
            {
                i1 = i1 + 1 & 0xFF;
                j1 = j1 + S[i1] & 0xFF;
                (S[i1], S[j1]) = (S[j1], S[i1]);
                int t = S[i1] + S[j1] & 0xFF;
                outp[k] = (byte)(data[k] ^ S[t]);
            }
            return outp;
        }
    }
}
