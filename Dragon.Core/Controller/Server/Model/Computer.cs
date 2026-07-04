
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Management;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace Dragon.Controller.Server.Model
{
    public class Computer
    {
        [DllImport("user32.dll")]
        private static extern int GetSystemMetrics(int nIndex);
        private const int SM_CXSCREEN = 0;
        private const int SM_CYSCREEN = 1;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [StringLength(200)]
        public string MachineName { get; set; } = string.Empty;

        [StringLength(50)]
        public string Status { get; set; } = string.Empty;

        public string FingerPrint { get; set; } = string.Empty;
        public string WebFingerPrint { get; set; } = string.Empty;
        public DateTime Created_At { get; set; }
        public DateTime Expires_At { get; set; }

        // Thêm thông tin phần cứng
        public string OSVersion { get; set; } = string.Empty;
        public int ProcessorCount { get; set; }
        public bool Is64Bit { get; set; }
        public string MacAddress { get; set; } = string.Empty;
        public string IpAddress { get; set; } = string.Empty;
        public string TotalRAM { get; set; } = string.Empty;
        public string GPUName { get; set; } = string.Empty;
        public string DiskInfo { get; set; } = string.Empty;


        //Các thông tin phần mềm & môi trường
        public string UserName { get; set; } = string.Empty;
        public string DomainName { get; set; } = string.Empty;
        public string CLRVersion { get; set; } = string.Empty;
        public string ScreenResolution { get; set; } = string.Empty;

        public Guid UserId { get; set; }


        public Computer()
        {
            UserId = Guid.NewGuid();
            MachineName = Environment.MachineName;
            Status = "trailer";
            Expires_At = DateTime.UtcNow.AddDays(1);
            FingerPrint = CreateCanvasFingerprint();
            Created_At = DateTime.UtcNow;

            // Lấy thêm thông tin
            OSVersion = Environment.OSVersion.ToString();
            ProcessorCount = Environment.ProcessorCount;
            Is64Bit = Environment.Is64BitOperatingSystem;
            UserName = Environment.UserName;
            DomainName = Environment.UserDomainName;
            CLRVersion = Environment.Version.ToString();
            MacAddress = GetMacAddress();
            IpAddress = GetIpAddress();
            TotalRAM = GetTotalRAM();
            GPUName = GetGPUName();
            DiskInfo = GetDiskInfo();


            try
            {
                int w = GetSystemMetrics(SM_CXSCREEN);
                int h = GetSystemMetrics(SM_CYSCREEN);
                ScreenResolution = $"{w}x{h}";
            }
            catch
            {
                ScreenResolution = "Unknown";
            }

        }

        private string GetMacAddress()
        {
            var nic = System.Net.NetworkInformation.NetworkInterface
                .GetAllNetworkInterfaces()
                .FirstOrDefault(n => n.OperationalStatus == System.Net.NetworkInformation.OperationalStatus.Up);
            return nic?.GetPhysicalAddress().ToString() ?? string.Empty;
        }
        private string GetIpAddress()
        {
            var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
            var ip = host.AddressList.FirstOrDefault(a => a.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
            return ip?.ToString() ?? string.Empty;
        }
        private string GetTotalRAM()
        {
            try
            {
                ObjectQuery winQuery = new ObjectQuery("SELECT * FROM Win32_ComputerSystem");
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(winQuery))
                {
                    foreach (ManagementObject item in searcher.Get())
                    {
                        double ramBytes = Convert.ToDouble(item["TotalPhysicalMemory"]);
                        double ramGB = Math.Round(ramBytes / (1024 * 1024 * 1024), 2);
                        return $"{ramGB} GB";
                    }
                }
            }
            catch { }
            return "Unknown";
        }
        private string GetGPUName()
        {
            try
            {
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController"))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        return obj["Name"]?.ToString() ?? "Unknown GPU";
                    }
                }
            }
            catch { }
            return "Unknown";
        }
        private string GetDiskInfo()
        {
            try
            {
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive"))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        string model = obj["Model"]?.ToString() ?? "Unknown";
                        string mediaType = obj["MediaType"]?.ToString() ?? "Unknown";
                        decimal sizeBytes = Convert.ToInt64(obj["Size"]);
                        decimal sizeGB = Math.Round(sizeBytes / (1024 * 1024 * 1024), 2);
                        return $"{model} - {sizeGB} GB - {mediaType}";
                    }
                }
            }
            catch { }
            return "Unknown";
        }
        string CreateCanvasFingerprint()
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
        private string ComputeSHA256(string input)
        {
            byte[] data = Encoding.UTF8.GetBytes(input);
            using var sha = SHA256.Create();
            byte[] hashBytes = sha.ComputeHash(data);
            var sb = new StringBuilder(hashBytes.Length * 2);
            foreach (byte b in hashBytes)
                sb.Append(b.ToString("x2"));
            return sb.ToString();
        }
    }
}
