using Dragon.Controller.GlobalControl.Helper;
using Dragon.Database.Models;
using Dragon.DesignView.Public;
using Dragon.DesignView.Public.NormalMode;
using System.ComponentModel;
using System.Data;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;


namespace Dragon.DesignView.FormUI
{
    public partial class FormInstallAPK : Form
    {
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private float _roundRadius = 15f;
        private Color _borderColor = Color.FromArgb(168, 168, 168);
        public FormInstallAPK()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);
            UpdateStyles();
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            EnableFormDrag(panel1);
            EnableFormDrag(panelRoundn1);

            
            ApplyTheme();
        }
        public void ApplyTheme()
        {
            

            BackColor = ThemeHelper.BorderNormalFist;
            ForeColor = ThemeHelper.ForeNormalFirst;

            panelMain.BackColor = ThemeHelper.BackNormalFirst;
            panelMain.ForeColor = ThemeHelper.ForeNormalFirst;

            panel1.BackColor = ThemeHelper.BackNormalFirst;
            flowLayoutPanel1.BackColor = ThemeHelper.BackNormalFirst;
            panelRoundn1.BackColor = ThemeHelper.BackNormalFirst;

            PictureBoxCloseForm.ApplyTheme();
            buttonStartInstall.ApplyTheme();
            labelFileApk.ApplyTheme();
            labelTotalPhone.ApplyTheme();
        }


        private void EnableFormDrag(Control ctrl)
        {
            if (ctrl == null) return;

            ctrl.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    ReleaseCapture();
                    SendMessage(this.Handle, 0xA1, 2, 0); // WM_NCLBUTTONDOWN
                    Console.WriteLine($"Dragging from {ctrl.Name}");
                }
            };
        }
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            SetRoundedRegion();
            SetPanelMainRoundedRegion();
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            panelMain.Size = new Size(ClientSize.Width - 2, ClientSize.Height - 2);
            SetRoundedRegion();
            SetPanelMainRoundedRegion();
        }
        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_BorderColor
        {
            get => _borderColor;
            set
            {
                _borderColor = value;
                this.BackColor = value;
            }
        }

        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public float DG_RoundRadius
        {
            get => _roundRadius;
            set
            {
                _roundRadius = value;
                SetRoundedRegion();
                SetPanelMainRoundedRegion();
            }
        }
        private void SetRoundedRegion()
        {
            using (var path = GetRoundedRectanglePath(ClientRectangle, _roundRadius))
            {
                var region = new Region(path);
                var old = Region;

                Region = region;
                old?.Dispose();
            }
        }
        private void SetPanelMainRoundedRegion()
        {
            using (var path = GetRoundedRectanglePath(panelMain.ClientRectangle, _roundRadius))
            {
                var region = new Region(path);
                var old = panelMain.Region;

                panelMain.Region = region;
                old?.Dispose();
            }
        }

        public static GraphicsPath GetRoundedRectanglePath(RectangleF rect, float radius)
        {
            float d = radius * 2f;
            var path = new GraphicsPath();
            if (radius <= 0) { path.AddRectangle(rect); return path; }

            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddLine(rect.X + radius, rect.Y, rect.Right - radius, rect.Y);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddLine(rect.Right, rect.Y + radius, rect.Right, rect.Bottom - radius);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddLine(rect.Right - radius, rect.Bottom, rect.X + radius, rect.Bottom);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.AddLine(rect.X, rect.Bottom - radius, rect.X, rect.Y + radius);
            path.CloseFigure();
            return path;
        }
        public Size originalSize;
        public Point originalLocation;
        public bool isMaximized = false;
        public void UpdateOriginalState()
        {
            originalSize = this.Size;
            originalLocation = this.Location;
        }
        protected override void OnLocationChanged(EventArgs e)
        {
            base.OnLocationChanged(e);
            if (!isMaximized && this.WindowState == FormWindowState.Normal)
            {
                UpdateOriginalState();
            }

        }
        private void PictureBoxCloseForm_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        List<Phone> phones = new List<Phone>();
        public string path = string.Empty;
        public void createPanel(List<Phone> phones, string path)
        {
            if (phones != null && phones.Count() > 0)
            {
                this.phones = phones;
                this.path = path;
                this.labelTotalPhone.Text = phones.Count().ToString();
                labelFileApk.Text = path;
                labelNormaln1.Text = $"Installing APK to {phones.Count()} devices";
                var phoness = phones.OrderBy(c => c.PhoneTagNumber);
                foreach (var item in phoness)
                {
                    CreateOneDevice(item.PhoneTagNumber);
                }
            }
        }
        public void UpdateStatusLabel(string PhoneNumber, string Status)
        {
            var label = LabelPhoneInstalls.SingleOrDefault(c => c.Name == PhoneNumber);
            if (label != null)
            {
                label.Text = Status;
            }
        }
        List<LabelNormalN> LabelPhoneInstalls = new List<LabelNormalN>();
        public void CreateOneDevice(int phoneNum = 1)
        {
            var pic = new PictureBoxBrightN();
            pic.DG_IsGrayImage = true;
            pic.DG_ImageColor = ThemeHelper.BorderNormal3rd;
            pic.DG_ImageSize = new Size(70, 70);
            pic.DG_LightenPercent = 20;
            pic.Dock = DockStyle.Top;
            pic.DG_IsBackTransparent = false;
            pic.DG_IsBrightBack = false;
            pic.Location = new Point(0, 0);
            pic.Padding = new Padding(3);
            pic.Size = new Size(77, 77);
            pic.DG_SVGString = "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><!DOCTYPE svg PUBLIC \"-//W3C//DTD SVG 1.1//EN\" \"http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd\"><svg fill=\"#000000\" version=\"1.1\" id=\"Capa_1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" width=\"800px\" height=\"800px\" viewBox=\"0 0 59.28 59.28\" xml:space=\"preserve\"><g><g><path d=\"M38.975,0h-18.67c-4.016,0-7.283,4.414-7.283,9.837v39.604c0,5.425,3.267,9.839,7.283,9.839h18.67c4.016,0,7.283-4.414,7.283-9.839V9.837C46.256,4.414,42.991,0,38.975,0z M38.975,54.07h-18.67c-1.651,0-3.046-2.118-3.046-4.629V9.837c0-2.508,1.395-4.628,3.046-4.628h18.67c1.65,0,3.047,2.12,3.047,4.628v39.604C42.022,51.952,40.625,54.07,38.975,54.07z\"/><g><circle cx=\"29.64\" cy=\"49.206\" r=\"2.401\"/></g></g></g></svg>";
            pic.DG_UseMode = UseMode.ThemeMode;


            var labelNum = new LabelNormalN();
            labelNum.AutoSize = true;
            labelNum.BackColor = ThemeHelper.BackNormalFirst;
            labelNum.DG_BackColor = ThemeHelper.BackNormalFirst;
            labelNum.DG_LightenPercent = 80;
            labelNum.DG_SVGString = "";
            labelNum.Dock = DockStyle.Fill;
            labelNum.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelNum.ForeColor = ThemeHelper.BorderNormal3rd;
            labelNum.DG_IsBrightBack = false;
            labelNum.DG_IsColorMode = false;
            labelNum.Location = new Point(3, 0);
            labelNum.Size = new Size(71, 26);
            labelNum.Text = phoneNum.ToString();
            if (phoneNum < 10)
            {
                labelNum.Text = "0" + phoneNum.ToString();
            }
            labelNum.DG_IsGrayImage = true;

            labelNum.TextAlign = ContentAlignment.MiddleCenter;

            var labelStatus = new LabelNormalN();
            labelStatus.AutoSize = true;
            labelStatus.BackColor = ThemeHelper.BackNormalFirst;
            labelStatus.DG_BackColor = ThemeHelper.BackNormalFirst;
            labelStatus.DG_LightenPercent = 80;
            labelStatus.DG_SVGString = "";
            labelStatus.Dock = DockStyle.Fill;
            labelStatus.DG_IsBrightBack = false;
            labelStatus.DG_IsColorMode = false;
            labelStatus.Location = new Point(3, 31);
            labelStatus.Size = new Size(71, 32);
            labelStatus.Text = "";
            labelStatus.Name = phoneNum.ToString();
            labelStatus.TextAlign = ContentAlignment.TopCenter;
            LabelPhoneInstalls.Add(labelStatus);

            var tabelpanel = new TableLayoutPanelBottomBorderN();
            tabelpanel.DG_BorderColor = Color.Empty;
            tabelpanel.DG_BorderThickness = 1;
            tabelpanel.ColumnCount = 1;
            tabelpanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tabelpanel.Controls.Add(labelNum, 0, 0);
            tabelpanel.Controls.Add(labelStatus, 0, 1);
            tabelpanel.Dock = DockStyle.Fill;
            tabelpanel.DG_IsCellSingleBorder = false;
            tabelpanel.Margin = new Padding(0);
            tabelpanel.RowCount = 2;
            tabelpanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tabelpanel.Size = new Size(77, 63);


            var panelNormal = new PanelNormalN();
            panelNormal.BackColor = ThemeHelper.BackNormalFirst;
            panelNormal.Controls.Add(tabelpanel);
            panelNormal.Controls.Add(pic);
            panelNormal.Size = new Size(77, 140);


            flowLayoutPanel1.Controls.Add(panelNormal);
        }

        private async void buttonStartInstall_Click(object sender, EventArgs e)
        {
            if (buttonStartInstall.Text != "Start Installation") return;

            if (phones == null || phones.Count == 0) return;

            buttonStartInstall.Text = "Installing APK...";

            var installTasks = phones.Select(phone => Task.Run(async () =>
            {
                try
                {
                    this.BeginInvoke(() => UpdateStatusLabel(phone.PhoneTagNumber.ToString(), "Installing"));

                    var result = await CMD.ExecuteAdbAsync($"adb -s {phone.Serial} install {path}");
                    if (result != null && result != string.Empty && result.ToLower().Contains("success"))
                    {
                        this.BeginInvoke(() => UpdateStatusLabel(phone.PhoneTagNumber.ToString(), "Completed!"));
                    }

                }
                catch (Exception)
                {

                }
            }));


            await Task.WhenAll(installTasks);

            this.Invoke((Action)(() =>
            {
                buttonStartInstall.Text = "Start Installation";
            }));
        }


    }
}
