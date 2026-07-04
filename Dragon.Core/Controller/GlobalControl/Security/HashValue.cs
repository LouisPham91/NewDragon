using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Dragon.Controller.GlobalControl.Security
{
    public class HashValue
    {
        public static string ComputeFileSHA256(string filePath)
        {
            using FileStream fs = File.OpenRead(filePath);
            using var sha = SHA256.Create();
            byte[] hashBytes = sha.ComputeHash(fs);
            var sb = new StringBuilder(hashBytes.Length * 2);
            foreach (byte b in hashBytes)
                sb.Append(b.ToString("x2"));
            return sb.ToString();
        }
        public static string ComputeStringSHA256(string input)
        {
            byte[] data = Encoding.UTF8.GetBytes(input);
            using var sha = SHA256.Create();
            byte[] hashBytes = sha.ComputeHash(data);
            // Format thành chuỗi hex
            var sb = new StringBuilder(hashBytes.Length * 2);
            foreach (byte b in hashBytes)
                sb.Append(b.ToString("x2"));
            return sb.ToString();
        }
    }
}
