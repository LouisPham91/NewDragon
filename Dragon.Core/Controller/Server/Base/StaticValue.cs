using Dragon.Controller.GlobalControl.Property;
using Dragon.Controller.GlobalControl.Security;
using Dragon.Controller.Server.Model;
using Dragon.Controller.Server.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Dragon.Controller.Server.Base
{
    public static class StaticValue
    {
        //public static string AccessToken = string.Empty;
        public static string userHash = string.Empty;
        public static string computerHash = string.Empty;


        public static string CreateCanvasFingerprint()
        {
            using (var bmp = new Bitmap(240, 60))
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);

                string txt = "BrowserLeaks,com <canvas> 1.0";

                // Vẽ nền màu #f60
                using (var brush = new SolidBrush(Color.FromArgb(255, 246, 96, 0)))
                {
                    g.FillRectangle(brush, 125, 1, 62, 20);
                }

                // Vẽ text với nhiều lớp màu
                using (var font = new Font("Arial", 14, FontStyle.Regular))
                {
                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                    g.DrawString(txt, font, new SolidBrush(Color.FromArgb(255, 0, 102, 153)), new PointF(2, 15));
                    g.DrawString(txt, font, new SolidBrush(Color.FromArgb(179, 102, 204, 0)), new PointF(4, 17));
                }

                // Duyệt toàn bộ pixel để tạo fingerprint
                StringBuilder sb = new StringBuilder();
                for (int y = 0; y < bmp.Height; y++)
                {
                    for (int x = 0; x < bmp.Width; x++)
                    {
                        Color c = bmp.GetPixel(x, y);
                        sb.Append($"{c.R}-{c.G}-{c.B}-{c.A};");
                    }
                }

                return ComputeSHA256(sb.ToString());
            }
        }

        private static string ComputeSHA256(string input)
        {
            byte[] data = Encoding.UTF8.GetBytes(input);
            using var sha = SHA256.Create();
            byte[] hashBytes = sha.ComputeHash(data);
            var sb = new StringBuilder(hashBytes.Length * 2);
            foreach (byte b in hashBytes)
                sb.Append(b.ToString("x2"));
            return sb.ToString();
        }

        public static Computer? getComputerInstance()
        {
            if (string.IsNullOrEmpty(computerHash)) return null;
            // KHÔNG giải mã nữa, server trả plain
            return JsonSerializer.Deserialize(computerHash, JsonServer.Default.Computer);
        }

        public static User? getUsernstance()
        {
            if (string.IsNullOrEmpty(userHash)) return null;
            return JsonSerializer.Deserialize(userHash, JsonServer.Default.User);
        }

        public static async Task LoadComputerInstance(string? webfingerPrint = null)
        {
            Computer computer = new Computer();
            var user = getUsernstance();
            if (user != null) computer.UserId = user.Id;

            computer.WebFingerPrint = webfingerPrint ?? GetSettings.GetWebFingerPrint();
            computer.FingerPrint = CreateCanvasFingerprint();

            var jsonComp = JsonSerializer.Serialize(computer, JsonServer.Default.Computer);
            var response = await HttpServices.PostJsonAuthAsync(
                new EncryptedPayload { Data = await RsaKeyPair.MaHoa(jsonComp) },
                JsonServer.Default.EncryptedPayload,
                GetSettings.GetAccessToken(),
                "/api/Computer/help");

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // server trả lại chính json đã gửi (plain)
                computerHash = await response.Content.ReadAsStringAsync();
            }
        }
    }
}