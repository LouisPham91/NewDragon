using AdvancedSharpAdbClient;
using AdvancedSharpAdbClient.Models;
using Dragon.Controller.DeviceControl;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace Dragon.UI.DesignView.FormUI
{
    public partial class FormTestScreenShotAndroid : Form
    {
        // Controls
        private TextBox txtIP = new();
        private TextBox txtPort = new();
        private Button btnConnect = new();
        private Button btnScreenshot = new();
        private PictureBox pictureBox = new();
        private Label lblStatus = new();
        private TextBox txtDeeplink = new();
        private Button btnDeeplink = new();

        private bool isConnected = false;
        private string adbPath = Path.Combine(AppContext.BaseDirectory, "Extension", "ScrcpyNet", "adb.exe");
        public FormTestScreenShotAndroid()
        {
            if (AdbServer.Instance.GetStatus().IsRunning == false)
                AdbServer.Instance.StartServer(adbPath, restartServerIfNewer: false);
            InitializeComponent();
            SetupUI();

            AdbClient adbClient = new AdbClient();
            var devicedata = adbClient.GetDevices().FirstOrDefault();
            if (devicedata != null)
            {
                var size = GetPhysicalSize(devicedata, adbClient);
                Debug.WriteLine("Physical size: " + size.w + "x" + size.h);

            }

        }

        private void SetupUI()
        {
            // Cấu hình form
            this.Text = "Chụp màn hình Android - Dragons";
            this.ClientSize = new Size(600, 1100); // Đủ cao để chứa PictureBox 960px
            this.MinimumSize = new Size(550, 800);

            // Panel chứa thông tin kết nối
            Panel panelTop = new Panel
            {
                Location = new Point(10, 10),
                Size = new Size(560, 90),
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(panelTop);

            // IP
            Label lblIP = new Label { Text = "IP:", Location = new Point(10, 15), AutoSize = true };
            panelTop.Controls.Add(lblIP);
            txtIP = new TextBox { Text = "192.168.1.7", Location = new Point(50, 12), Width = 150 };
            panelTop.Controls.Add(txtIP);

            // Port
            Label lblPort = new Label { Text = "Port:", Location = new Point(220, 15), AutoSize = true };
            panelTop.Controls.Add(lblPort);
            txtPort = new TextBox { Text = "8888", Location = new Point(260, 12), Width = 60 };
            panelTop.Controls.Add(txtPort);

            // Nút Kết nối
            btnConnect = new Button
            {
                Text = "Kết nối",
                Location = new Point(340, 10),
                Width = 90,
                Height = 28
            };
            btnConnect.Click += BtnConnect_Click;
            panelTop.Controls.Add(btnConnect);

            // Trạng thái
            lblStatus = new Label
            {
                Text = "Chưa kết nối",
                Location = new Point(10, 55),
                AutoSize = true,
                ForeColor = Color.Red
            };
            panelTop.Controls.Add(lblStatus);

            // Nút Chụp màn hình
            btnScreenshot = new Button
            {
                Text = "Chụp màn hình",
                Location = new Point(10, 110),
                Width = 130,
                Height = 35,
                Enabled = false // Chỉ bật khi đã kết nối
            };
            btnScreenshot.Click += BtnScreenshot_Click;
            this.Controls.Add(btnScreenshot);

            // PictureBox – chiều cao tối thiểu 960, giữ tỉ lệ
            pictureBox = new PictureBox
            {
                Location = new Point(10, 155),
                Size = new Size(560, 960),
                BackColor = Color.Black,
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.Fixed3D
            };
            this.Controls.Add(pictureBox);

            // --- Deeplink test ---
            Label lblLink = new Label { Text = "Deeplink:", Location = new Point(150, 118), AutoSize = true };
            this.Controls.Add(lblLink);

            txtDeeplink = new TextBox
            {
                Text = "https://",
                Location = new Point(220, 115),
                Width = 250
            };
            this.Controls.Add(txtDeeplink);

            btnDeeplink = new Button
            {
                Text = "Mở Link",
                Location = new Point(480, 113),
                Width = 90,
                Height = 28,
                Enabled = false
            };
            btnDeeplink.Click += BtnDeeplink_Click;
            this.Controls.Add(btnDeeplink);
        }

        private async void BtnConnect_Click(object? sender, EventArgs e)
        {
            string ip = txtIP.Text.Trim();
            int port = int.Parse(txtPort.Text);

            if (isConnected)
            {
                ScreenShotApp.Instance.Remove(ip);
                isConnected = false;
                lblStatus.Text = "Đã ngắt"; lblStatus.ForeColor = Color.Red;
                btnConnect.Text = "Kết nối";
                btnScreenshot.Enabled = btnDeeplink.Enabled = false;
                return;
            }

            try
            {
                btnConnect.Enabled = false;
                lblStatus.Text = "Đang kết nối..."; lblStatus.ForeColor = Color.Blue;

                ScreenShotApp.Instance.Add(ip, port); // <-- thêm vào pool
                                                      // thử chụp 1 phát để test
                var test = await ScreenShotApp.Instance.ScreenshotAsync(ip);

                isConnected = true;
                lblStatus.Text = "Đã kết nối"; lblStatus.ForeColor = Color.Green;
                btnConnect.Text = "Ngắt kết nối";
                btnScreenshot.Enabled = btnDeeplink.Enabled = true;
            }
            catch (Exception ex)
            {
                ScreenShotApp.Instance.Remove(ip);
                lblStatus.Text = "Kết nối thất bại"; lblStatus.ForeColor = Color.Red;
                MessageBox.Show(ex.Message);
            }
            finally { btnConnect.Enabled = true; }
        }

        private static readonly Regex PhysicalRegex = new(@"Physical size: (\d+)x(\d+)", RegexOptions.Compiled);
        private static readonly Regex OverrideRegex = new(@"Override size: (\d+)x(\d+)", RegexOptions.Compiled);
        static (int w, int h) GetPhysicalSize(DeviceData device, IAdbClient adb)
        {
            // 1. ưu tiên mCurrentDisplayRect – Samsung/Xiaomi/Oppo đều có
            var out1 = Shell(device, adb, "wm size");
            // ví dụ: mCurrentDisplayRect=Rect(0, 0 - 1080, 2400)
            var m = OverrideRegex.Match(out1);
            if (m.Success) return (int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value));

            m = PhysicalRegex.Match(out1);
            if (m.Success) return (int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value));
            return (0, 0);
        }
        static string Shell(DeviceData device, IAdbClient adb, string command)
        {
            var sb = new StringBuilder();
            try
            {
                // thêm 2>&1 để gộp stderr vào stdout, và dùng predicate để đọc từng dòng
                adb.ExecuteRemoteCommand(command + " 2>&1", device, line =>
                {
                    sb.AppendLine(line);
                    return true; // tiếp tục đọc
                });
                return sb.ToString().Trim();
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine($"[{device.Serial}] shell timeout: {command}");
                return string.Empty;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[{device.Serial}] shell error: {ex.Message}");
                return string.Empty;
            }
        }

        private async void BtnScreenshot_Click(object? sender, EventArgs e)
        {
            var sw = Stopwatch.StartNew();
            string ip = txtIP.Text.Trim();

            btnScreenshot.Enabled = false;
            lblStatus.Text = "Đang chụp..."; lblStatus.ForeColor = Color.Blue;

            try
            {
                byte[] imageData = await ScreenShotApp.Instance.ScreenshotAsync(ip); // <-- đổi
                using var ms = new MemoryStream(imageData);
                var img = Image.FromStream(ms);
                pictureBox.Image?.Dispose();
                pictureBox.Image = img;
                pictureBox.Size = new Size(img.Width, img.Height);

                lblStatus.Text = "OK"; lblStatus.ForeColor = Color.Green;
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Lỗi: {ex.Message}"; lblStatus.ForeColor = Color.Red;
            }
            finally { btnScreenshot.Enabled = isConnected; sw.Stop(); Debug.WriteLine(sw.ElapsedMilliseconds); }
        }

        private async void BtnDeeplink_Click(object? sender, EventArgs e)
        {
            string ip = txtIP.Text.Trim();
            string url = txtDeeplink.Text.Trim();

            btnDeeplink.Enabled = false;
            try
            {
                await ScreenShotApp.Instance.DeeplinkAsync(ip, url); // <-- đổi
                lblStatus.Text = "Đã gửi deeplink"; lblStatus.ForeColor = Color.Green;
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Gửi thất bại"; lblStatus.ForeColor = Color.Red;
                MessageBox.Show(ex.Message);
            }
            finally { btnDeeplink.Enabled = isConnected; }
        }
    }
}