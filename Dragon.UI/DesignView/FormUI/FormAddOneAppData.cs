using AdvancedSharpAdbClient;
using AdvancedSharpAdbClient.Models;
using AdvancedSharpAdbClient.Receivers;
using Dragon.Database.Models;
using Dragon.Database.Services;
using Dragon.DesignView.Public;
using System.ComponentModel;
using System.Data;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace Dragon.DesignView.FormUI
{
    public partial class FormAddOneAppData : Form
    {
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private float _roundRadius = 15f;
        private Color _borderColor = Color.FromArgb(168, 168, 168);
        

        FormAppData? FormAppData = null;
        Phone? Phone = null;
        DeviceData? deviceData = null;
        public FormAddOneAppData(Phone phone, FormAppData? formAppData, DeviceData? deviceData)
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

                
            ApplyTheme(); // 🔥 Gọi ngay sau khi subscribe
            FormAppData = formAppData;
            Phone = phone;
            this.deviceData = deviceData;

            labelNum.Text = "Num : " + Phone.PhoneTagNumber.ToString();
            labelDeviceID.Text = "DeviceID : " + Phone.DeviceID;
            labelModel.Text = "Model : " + Phone.Model;
        }

        public void ApplyTheme()
        {     
            BackColor = ThemeHelper.BorderNormalFist;
            ForeColor = ThemeHelper.ForeNormalFirst;

            panelMain.BackColor = ThemeHelper.BackNormalFirst;
            panelMain.ForeColor = ThemeHelper.ForeNormalFirst;

            panel1.BackColor = ThemeHelper.BackNormalFirst;
            panelRoundn1.BackColor = ThemeHelper.BackNormalFirst;

            PictureBoxCloseForm.ApplyTheme();
            buttonAddOne_AppData.ApplyTheme();
            labelFile.ApplyTheme();

            comboxPackageName.DG_BorderSize = 0;
            comboxSocialNetwork.DG_BorderSize = 0;

            comboxPackageName.ApplyTheme();
            comboxSocialNetwork.ApplyTheme();
            tableLayoutPanelBottomBordern1.ApplyTheme();
            comboxSocialNetwork.ForeColor = ThemeHelper.ForeNormalFirst;

            if (ThemeHelper.CurrentMode == ThemeMode.Light)
            {
                comboxSocialNetwork.BackColor = Color.FromArgb(230, 230, 230);     
                comboxPackageName.BackColor = Color.FromArgb(230, 230, 230);
            }
            else
            {
                comboxSocialNetwork.BackColor = Color.FromArgb(55, 55, 55);
                comboxPackageName.BackColor = Color.FromArgb(55, 55, 55);
            }
        
        }


        private void FormAndroidExplorer_Load(object sender, EventArgs e)
        {
            comboxSocialNetwork.DropDownStyle = ComboBoxStyle.DropDown;
            comboxSocialNetwork.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboxSocialNetwork.AutoCompleteSource = AutoCompleteSource.ListItems;

            comboxPackageName.DropDownStyle = ComboBoxStyle.DropDown;
            comboxPackageName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboxPackageName.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboxPackageName.TextUpdate += comboxPackageName_TextUpdate;

            LoadAppData();
            LoadSocialNetwork();
       
        }


        #region Support Begin No Edit
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
        #endregion

        public AppData? GetAppData()
        {
            if (Phone == null)
            {
                MessageBox.Show("Phone is Null");
                return null;
            }
            string package = comboxPackageName.Text;
            string network = comboxSocialNetwork.Text;
            if (string.IsNullOrEmpty(package) || string.IsNullOrEmpty(network))
            {
                MessageBox.Show("Please Seleted package or network");
                return null;
            }

            if (comboxSocialNetwork.SelectedItem is not SocialNetwork network1)
            {
                MessageBox.Show("Please select a valid social network.");
                return null;
            }

            var appData = new AppData
            {
                DeviceID = Phone.DeviceID,
                DeviceModel = Phone.Model,
                PackageName = package,
                NetworkName = network1.NetworkName,
                NetworkGuId = network1.guId
            };

            return appData;
        }
        private void buttonAddOne_AppData_Click(object sender, EventArgs e)
        {
            if (Phone == null) return;
            if (FormAppData == null) return;

            var appdata = GetAppData();
            if (appdata == null) return;

            AppDataRepository.Add(appdata);

            FormAppData.LoadAllAppData(Phone);
        }

        // giữ toàn bộ danh sách gốc
        private List<string> allPackages = new List<string>();

        private void comboxPackageName_TextUpdate(object? sender, EventArgs e)
        {
            string keyword = comboxPackageName.Text.ToLower();

            // lọc theo Contains
            var filtered = allPackages
                .Where(p => p.ToLower().Contains(keyword))
                .ToList();

            // cập nhật lại Items
            comboxPackageName.BeginUpdate();
            comboxPackageName.Items.Clear();
            comboxPackageName.Items.AddRange(filtered.ToArray());
            comboxPackageName.EndUpdate();

            // giữ dropdown mở
            comboxPackageName.DroppedDown = true;

            // đặt lại caret
            comboxPackageName.SelectionStart = keyword.Length;
            comboxPackageName.SelectionLength = 0;
        }

        public void LoadAppData()
        {
            if (deviceData == null) return;
            this.BeginInvoke(new Action(async () =>
            {
                var output = new ConsoleOutputReceiver();

                // Tạo CancellationToken với timeout 10 giây
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

                try
                {
                    var adbClient = new AdbClient();
                    await adbClient.ExecuteRemoteCommandAsync("pm list package", deviceData, output, cts.Token);
                    allPackages.Clear();
                    allPackages = output.ToString()
                        .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(line => line.Replace("package:", "").Trim())
                        .ToList();

                    comboxPackageName.Items.Clear();
                    comboxPackageName.Items.AddRange(allPackages.ToArray());

                    // xử lý packages ở đây
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Lệnh adb bị hủy do quá thời gian 10 giây.");
                }
            }));
        }

        public void LoadSocialNetwork()
        {
            var networks = SocialNetworkRepository.GetAll();
            if (networks == null || networks.Count() == 0) return;

            comboxSocialNetwork.Items.Clear();
            comboxSocialNetwork.Items.AddRange(networks.ToArray());

        }
    }
}

