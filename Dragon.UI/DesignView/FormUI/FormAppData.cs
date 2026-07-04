using AdvancedSharpAdbClient;
using AdvancedSharpAdbClient.Models;
using Dragon.Controller.Database.Services;
using Dragon.Database.Models;
using Dragon.Database.Services;
using Dragon.DesignView.Public;
using Dragon.DesignView.Public.NormalMode;
using Dragon.UI.ControlHelper;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Dragon.DesignView.FormUI
{
    public partial class FormAppData : Form
    {
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private float _roundRadius = 15f;
        private Color _borderColor = Color.FromArgb(168, 168, 168);

        // Danh sách AppData hiện tại
        private List<AppData> appDataList = new List<AppData>();

        // AppData đang được chọn
        AppData? selectedAppData = null;
        DeviceData? deviceData;
        AdbClient adbClient = new AdbClient();

        Phone? selectedPhone = null;
        public FormAppData(Phone? phone = null, DeviceData? deviceData = null, bool hidePhoneGridView = true)
        {
            selectedPhone = phone;
            this.deviceData = deviceData;
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

            if (hidePhoneGridView)
            {
                aotSafeDataGridView2.Hide();
            }
            else
            {
                LoadPhone();
            }
        }

        private void FormAppData_Load(object sender, EventArgs e)
        {

            // Load dữ liệu
            LoadAllAppData(selectedPhone);

            // Setup events
            aotSafeDataGridView1.CellClick += AotSafeDataGridView1_CellClick;
            aotSafeDataGridView1.KeyUp += AotSafeDataGridView1_KeyUp;
            aotSafeDataGridView2.CellClick += AotSafeDataGridView2_CellClick;

        }
        private void ApplyTheme()
        {


            BackColor = ThemeHelper.BorderNormalFist;
            ForeColor = ThemeHelper.ForeNormalFirst;

            panelMain.BackColor = ThemeHelper.BackNormalFirst;
            panelMain.ForeColor = ThemeHelper.ForeNormalFirst;

            panel1.BackColor = ThemeHelper.BackNormalFirst;
            panelRoundn1.BackColor = ThemeHelper.BackNormalFirst;

            PictureBoxCloseForm.ApplyTheme();
            buttonAddOne_Appdata.ApplyTheme();
            labelFile.ApplyTheme();
            aotSafeDataGridView1.ApplyTheme();
            aotSafeDataGridView2.ApplyTheme();
            textBoxSearch.ApplyTheme();
            pictureBoxBrightn1.ApplyTheme();
            labelNormaln1.ApplyTheme();
            buttomDeleteAppData.ApplyTheme();
            panelRoundedWithBorder1.BackColor = ThemeHelper.BackNormalFirst;
            labelSeletedPhone.ApplyTheme();
            aotSafeDataGridView2.BorderStyle = BorderStyle.None;

        }
        private void PictureBoxCloseForm_Click(object sender, EventArgs e)
        {
            this.Close();
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


        #endregion

        #region Phone
        public void LoadPhone()
        {
            var phones = PhoneRepository.LoadAll();
            // lấy danh sách thiết bị hiện tại từ adb
            AdbClient adbClient = new AdbClient();
            var deviceDatas = adbClient.GetDevices();

            // cập nhật trạng thái online/offline cho từng phone
            if (deviceDatas != null && deviceDatas.Any())
            {
                foreach (var p in phones)
                {
                    p.IsOnline(deviceDatas.ToList());
                }
            }

            BindAppDataToGridView(phones);
        }
        private void BindAppDataToGridView(List<Phone> data)
        {
            data ??= new List<Phone>();

            // logic cũ của bạn - giữ nguyên
            foreach (var phone in data)
            {
                var raw = PhoneStatusImage.GetImage(phone.DeviceState, phone.IsRunning);
                // resize về 20x24 cho vừa cell 30x28
                if (raw != null)
                    phone.Im = new Bitmap(raw, new Size(20, 24));
            }

            var hiddenColumns = new List<string>
            {
                "Id","PhoneBoxId","Serial","DeviceState",/*"PhoneMode",*/"Model","Product","Usb",
                "Ipv4","Message","TransportId","IsUHDI","PhysicalWidth","PhysicalHeight",
                "IsRunning","AndroidVersion","API","IsUseUSB","ProcVersion","ProcCpuInfo",
                "IsAccessibleAppInstall","IsPingWifi","IsRooted","IsMagisk","IsKernelSu","PhoneHash","IsOnline"
            };

            // rename: (tên cột cũ, header mới, width)
            var rename = new List<(string, string, int)>
            {
                ("Im", "", 28),
                ("PhoneTagNumber", "Tag", 30),
                ("DeviceID", "Device ID", 0), // 0 = không ép width, sẽ fill sau
                ("PhoneMode", "Mode", 45),
            };

            // GỌI BIND VỚI CUSTOMIZE
            GridViewBinders.BindPhone(aotSafeDataGridView2, data, hiddenColumns, rename, dgv =>
            {
                // 1. Ép cột ảnh
                var imgCol = dgv.Columns["Im"] as DataGridViewImageColumn;
                if (imgCol != null)
                {
                    imgCol.Width = 30; // đúng 30px như bạn muốn
                    imgCol.ImageLayout = DataGridViewImageCellLayout.Zoom; // QUAN TRỌNG
                    imgCol.DefaultCellStyle.Padding = new Padding(3); // cho ảnh thở
                    imgCol.Resizable = DataGridViewTriState.False;
                }

                // 2. Ép chiều cao row - phải làm SAU khi có data
                dgv.RowTemplate.Height = 28;
                dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;

                // 3. Resize ảnh gốc luôn (đảm bảo 100% nhỏ)
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    row.Height = 28; // ép từng row hiện tại
                }
                // format số cho Tag
                dgv.Columns["PhoneTagNumber"]?.DefaultCellStyle.Format = "N0";
                dgv.Columns["PhoneTagNumber"]?.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // cho DeviceID chiếm hết chỗ còn lại
                dgv.Columns["DeviceID"]?.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                // dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            });
        }
        private void AotSafeDataGridView2_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var phone = aotSafeDataGridView2.Rows[e.RowIndex].DataBoundItem as Phone;
            if (phone != null)
            {
                selectedPhone = phone;
                LoadAllAppData(selectedPhone);
            }
        }
        #endregion

        #region AppData

        /// <summary>
        /// Load toàn bộ AppData theo DeviceID của Phone
        /// </summary>
        public void LoadAllAppData(Phone? phone)
        {
            if (phone == null || string.IsNullOrEmpty(phone.DeviceID))
            {
                MessageBox.Show("Không tìm thấy DeviceID của điện thoại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Lấy danh sách AppData theo DeviceID
                appDataList = AppDataRepository.GetByDeviceId(phone.DeviceID);

                if (appDataList == null || appDataList.Count == 0)
                {
                    appDataList = new List<AppData>();
                }

                // Bind vào DataGridView
                BindAppDataToGridView(appDataList);

                // Update label tổng số
                UpdateTotalCountLabel();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Bind danh sách AppData vào DataGridView
        /// </summary>
        private void BindAppDataToGridView(List<AppData> data)
        {
            if (data == null || data.Count() == 0) return;

            // Các cột không muốn hiển thị
            var hiddenColumns = new List<string>
            {
                "guId",  // Ẩn cột Id vì không cần thiết hiển thị
                "NetworkGuId"
            };

            // Sử dụng GridViewBinders để bind (giống FormActionIntent)
            GridViewBinders.BindAppData(aotSafeDataGridView1, data, hiddenColumns);

            // Auto resize columns
            aotSafeDataGridView1.AutoResizeColumns();
        }
        /// <summary>
        /// Xử lý sự kiện click vào cell
        /// </summary>
        private void AotSafeDataGridView1_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var appData = aotSafeDataGridView1.Rows[e.RowIndex].DataBoundItem as AppData;
            if (appData != null)
            {
                selectedAppData = appData;
                labelSeletedPhone.Text = $"Selected : {selectedAppData}";
                // Có thể hiển thị thông tin chi tiết ở panel bên cạnh nếu có
                DisplayAppDataDetail(appData);
            }
        }
        /// <summary>
        /// Cập nhật label hiển thị tổng số bản ghi
        /// </summary>
        private void UpdateTotalCountLabel()
        {
            // Nếu có label hiển thị tổng số (ví dụ: lblTotalCount)
            // lblTotalCount.Text = $"Tổng: {appDataList.Count} bản ghi";
        }

        /// <summary>
        /// Xử lý phím tắt
        /// </summary>
        private void AotSafeDataGridView1_KeyUp(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && aotSafeDataGridView1.CurrentRow != null)
            {
                DeleteSelectedAppData();
            }

            if (e.KeyCode == Keys.F5)
            {
                RefreshData();
            }

            if (e.KeyCode == Keys.Enter && aotSafeDataGridView1.CurrentRow != null)
            {
                EditSelectedAppData();
            }
        }

        /// <summary>
        /// Xóa AppData đang chọn
        /// </summary>
        private void DeleteSelectedAppData()
        {
            if (selectedAppData == null)
            {
                selectedAppData = aotSafeDataGridView1.CurrentRow?.DataBoundItem as AppData;
            }

            if (selectedAppData == null) return;

            var result = MessageBox.Show(
                $"Bạn có chắc muốn xóa AppData:\n" +
                $"Package: {selectedAppData.PackageName}\n" +
                $"Email: {selectedAppData.Email}\n" +
                $"Username: {selectedAppData.Username}",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {

                if (BackUpAppDataRepository.BackupAppData(selectedAppData, false))
                {
                    MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshData();
                }
                else
                {
                    MessageBox.Show("Xóa thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Sửa AppData đang chọn
        /// </summary>
        private void EditSelectedAppData()
        {
            if (selectedAppData == null)
            {
                selectedAppData = aotSafeDataGridView1.CurrentRow?.DataBoundItem as AppData;
            }

            if (selectedAppData == null) return;

            // Mở form sửa (có thể tạo FormAppDataDetail sau)
            // var editForm = new FormAppDataDetail(selectedAppData);
            // if (editForm.ShowDialog() == DialogResult.OK)
            // {
            //     RefreshData();
            // }

            MessageBox.Show($"Sửa AppData: {selectedAppData.PackageName}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Hiển thị chi tiết AppData
        /// </summary>
        private void DisplayAppDataDetail(AppData appData)
        {
            // Có thể hiển thị thông tin chi tiết ở TextBox hoặc Label
            // Ví dụ:
            // txtPackageName.Text = appData.PackageName;
            // txtEmail.Text = appData.Email;
            // txtUsername.Text = appData.Username;
            // ...
        }

        /// <summary>
        /// Refresh dữ liệu
        /// </summary>
        private void RefreshData()
        {
            LoadAllAppData(selectedPhone);
            selectedAppData = null;
        }

        /// <summary>
        /// Tìm kiếm AppData
        /// </summary>
        private void SearchAppData(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                BindAppDataToGridView(appDataList);
                return;
            }

            string kw = keyword.ToLowerInvariant();
            var filtered = appDataList.Where(x =>
                x.NetworkName.ToLowerInvariant().Contains(kw) ||
                x.PackageName.ToLowerInvariant().Contains(kw) ||
                x.DeviceModel.ToLowerInvariant().Contains(kw) ||
                x.ABI.ToLowerInvariant().Contains(kw) ||
                x.Email.ToLowerInvariant().Contains(kw) ||
                x.Username.ToLowerInvariant().Contains(kw) ||
                x.HoTen.ToLowerInvariant().Contains(kw) ||
                x.Phone.ToLowerInvariant().Contains(kw)

            ).ToList();

            BindAppDataToGridView(filtered);
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            SearchAppData(textBoxSearch.Text);
        }

        #endregion


        FormAddOneAppData? formAddOneAppData = null;

        private void buttonAddOne_Appdata_Click(object sender, EventArgs e)
        {
            if (selectedPhone == null)
            {
                MessageBox.Show("Please Select A Phone", "Select Phone", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (deviceData == null)
            {
                AdbClient adbClient = new AdbClient();
                var deviceDatas = adbClient.GetDevices();
                deviceData = selectedPhone.GetDeviceData(deviceDatas.ToList());
            }

            if (deviceData == null)
            {
                MessageBox.Show($"This Phone {selectedPhone.DeviceID} Is Offline", "Select DeviceData", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            formAddOneAppData = new FormAddOneAppData(selectedPhone, this, deviceData);
            formAddOneAppData.StartPosition = FormStartPosition.CenterScreen;
            formAddOneAppData.ShowDialog();
        }

        private void buttomDeleteAppData_Click(object sender, EventArgs e)
        {
            if (selectedAppData == null) return;
            BackUpAppDataRepository.DeleteAppDataWithBackup(selectedAppData.guId);
            LoadAllAppData(this.selectedPhone);
        }
    }
}