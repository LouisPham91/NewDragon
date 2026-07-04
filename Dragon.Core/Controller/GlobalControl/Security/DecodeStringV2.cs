
using System.Security.Cryptography;
using System.Text;

namespace Dragon.Controller.GlobalControl.Security
{
    public class DecodeStringV2
    {
        // ==== Cặp 2: S-Box + XOR + Rotate + Unicode-escape ====
        private static readonly byte[] Key2 = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes("An0th3r_Sup3r_S3cr3t!"));

        private static readonly byte[] SBox;
        private static readonly byte[] InvSBox;

        static DecodeStringV2()
        {
            SBox = Enumerable.Range(0, 256).Select(i => (byte)i).ToArray();
            InvSBox = new byte[256];
            byte j2 = 0;
            for (int i = 0; i < 256; i++)
            {
                j2 = (byte)(j2 + SBox[i] + Key2[i % Key2.Length] & 0xFF);
                (SBox[i], SBox[j2]) = (SBox[j2], SBox[i]);
            }
            for (int i = 0; i < 256; i++)
                InvSBox[SBox[i]] = (byte)i;
        }

        public static string EncodeV2(string plain)
        {
            var data = Encoding.UTF8.GetBytes(plain);
            var sb = new StringBuilder(data.Length * 6);
            for (int i = 0; i < data.Length; i++)
            {
                byte b = data[i];
                b = (byte)(b + i & 0xFF);
                b = SBox[b];
                b = (byte)(b ^ Key2[i % Key2.Length]);
                b = (byte)(b << 3 | b >> 5);
                sb.AppendFormat("/u00{0:X2}", b);
            }
            return sb.ToString();
        }

        public static string DecodeV2(string encoded)
        {
            if (encoded.Length % 6 != 0)
                throw new FormatException("Độ dài không chia hết cho 6.");

            int len = encoded.Length / 6;
            var bytes = new byte[len];
            for (int i = 0; i < len; i++)
            {
                string hex = encoded.Substring(i * 6 + 4, 2);
                bytes[i] = Convert.ToByte(hex, 16);
            }

            var outp = new byte[len];
            for (int i = 0; i < len; i++)
            {
                byte b = bytes[i];
                b = (byte)(b >> 3 | b << 5);
                b = (byte)(b ^ Key2[i % Key2.Length]);
                b = InvSBox[b];
                b = (byte)(b - i & 0xFF);
                outp[i] = b;
            }

            return Encoding.UTF8.GetString(outp);
        }
    }
}
