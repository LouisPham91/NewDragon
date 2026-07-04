
using Dragon.Controller.Database.Services;
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
    public partial class FormKeybroadSetting : Form
    {
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private float _roundRadius = 15f;
        private Color _borderColor = Color.FromArgb(168, 168, 168);
        Phone phone;
        KeybroadSetting? SelectesKeybroadSetting = null;

        public FormKeybroadSetting(Phone phone)
        {
            this.phone = phone;
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
            panelRoundn1.BackColor = ThemeHelper.BackNormalFirst;

            PictureBoxCloseForm.ApplyTheme();
            buttonDeleteRow.ApplyTheme();
            buttonUpdateRow.ApplyTheme();
            labelFile.ApplyTheme();

            if (ThemeHelper.CurrentMode == ThemeMode.Light)
            {
                textBoxSearch.BackColor = Color.White;
                textBoxSearch.ForeColor = Color.Black;
            }
            else
            {
                textBoxSearch.BackColor = Color.FromArgb(31, 31, 31);
                textBoxSearch.ForeColor = Color.White;
            }

            if (aotSafeDataGridView1 != null)
            {
                // Style giống hệt FormActionIntent
                aotSafeDataGridView1.EnableHeadersVisualStyles = false;


                // Màu nền và chọn giống FormActionIntent
                aotSafeDataGridView1.DefaultCellStyle.BackColor = ThemeHelper.BackNormalFirst;
                aotSafeDataGridView1.DefaultCellStyle.SelectionBackColor = ThemeHelper.BorderNormalFist;
                aotSafeDataGridView1.DefaultCellStyle.ForeColor = ThemeHelper.ForeNormal2nd;
                aotSafeDataGridView1.DefaultCellStyle.SelectionForeColor = ThemeHelper.ForeNormal2nd;
                aotSafeDataGridView1.BackgroundColor = ThemeHelper.BackNormalFirst;

                // Header style
                aotSafeDataGridView1.ColumnHeadersDefaultCellStyle.BackColor = aotSafeDataGridView1.DefaultCellStyle.BackColor;
                aotSafeDataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = aotSafeDataGridView1.DefaultCellStyle.ForeColor;
                aotSafeDataGridView1.ColumnHeadersDefaultCellStyle.SelectionBackColor = aotSafeDataGridView1.DefaultCellStyle.SelectionBackColor;
                aotSafeDataGridView1.ColumnHeadersDefaultCellStyle.SelectionForeColor = aotSafeDataGridView1.DefaultCellStyle.SelectionForeColor;

                // Row headers
                aotSafeDataGridView1.RowHeadersDefaultCellStyle.BackColor = aotSafeDataGridView1.DefaultCellStyle.BackColor;
                aotSafeDataGridView1.RowHeadersDefaultCellStyle.ForeColor = aotSafeDataGridView1.DefaultCellStyle.ForeColor;
                aotSafeDataGridView1.RowHeadersDefaultCellStyle.SelectionBackColor = aotSafeDataGridView1.DefaultCellStyle.SelectionBackColor;
                aotSafeDataGridView1.RowHeadersDefaultCellStyle.SelectionForeColor = aotSafeDataGridView1.DefaultCellStyle.SelectionForeColor;

                // Grid color
                aotSafeDataGridView1.GridColor = Color.Gray;

                // Border style
                aotSafeDataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                aotSafeDataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

                // Ẩn row headers
                aotSafeDataGridView1.RowHeadersVisible = false;

                // Không tự động tạo cột
                aotSafeDataGridView1.AutoGenerateColumns = false;
            }
        }

        private async void FormAndroidExplorer_Load(object sender, EventArgs e)
        {
            LoadKeybroadSetting(phone);
            await LoadingAllIme(phone);
            aotSafeDataGridView1.CellClick += AotSafeDataGridView1_CellClick;
        }
        public async Task LoadingAllIme(Phone phone)
        {
            try
            {
                var listimage = await CMD.ExecuteAdbAsync($"adb -s {phone.DeviceID} shell ime list -s");
                if (string.IsNullOrEmpty(listimage))
                {
                    return;
                }
                // Tách chuỗi theo ký tự xuống dòng
                List<string> ImeiList = listimage
                    .Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList();

                // Nếu muốn loại bỏ "exit" ở đầu danh sách:
                var newImeiList = ImeiList.Where(x => x.Contains("/")).ToList();
                foreach (var imeiId in newImeiList)
                {
                    if (!imeiId.Contains("voice", StringComparison.OrdinalIgnoreCase))
                    {
                        if (imeiId.Contains("labankey", StringComparison.OrdinalIgnoreCase))
                        {
                            var KeybroadSetting = new KeybroadSetting
                            {
                                DeviceId = phone.DeviceID,
                                IMEId = imeiId,
                                Langeuage = Langeuage.Vietnamese
                            };


                            if (!KeybroadSettingRepository.IsAny(KeybroadSetting.DeviceId, KeybroadSetting.IMEId))
                            {
                                KeybroadSettingRepository.Add(KeybroadSetting);
                            }
                        }
                        else if (imeiId.Contains("AdbKeyboard", StringComparison.OrdinalIgnoreCase))
                        {
                            var KeybroadSetting = new KeybroadSetting
                            {
                                DeviceId = phone.DeviceID,
                                IMEId = imeiId,
                                Langeuage = Langeuage.ATX_Unicode
                            };

                            if (!KeybroadSettingRepository.IsAny(KeybroadSetting.DeviceId, KeybroadSetting.IMEId))
                            {
                                KeybroadSettingRepository.Add(KeybroadSetting);
                            }
                        }
                        else
                        {
                            var KeybroadSetting = new KeybroadSetting
                            {
                                DeviceId = phone.DeviceID,
                                IMEId = imeiId,
                                Langeuage = Langeuage.English
                            };

                            if (!KeybroadSettingRepository.IsAny(KeybroadSetting.DeviceId, KeybroadSetting.IMEId))
                            {
                                KeybroadSettingRepository.Add(KeybroadSetting);
                            }
                        }

                    }
                }

                // Bind vào DataGridView
                LoadKeybroadSetting(phone);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadKeybroadSetting(Phone phone)
        {
            try
            {
                // Lấy danh sách AppData theo DeviceID
                var keybroads = KeybroadSettingRepository.FindManyByDeviceID(phone.DeviceID);

                if (keybroads == null || keybroads.Count == 0)
                {
                    keybroads = new List<KeybroadSetting>();
                }

                // Bind vào DataGridView
                BindAppDataToGridView(keybroads);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BindAppDataToGridView(List<KeybroadSetting> data)
        {
            // Các cột không muốn hiển thị
            var hiddenColumns = new List<string>
            {
                "Id"  // Ẩn cột Id vì không cần thiết hiển thị
            };

            // Sử dụng GridViewBinders để bind (giống FormActionIntent)
            GridViewBinders.BindKeybroad(aotSafeDataGridView1, data, hiddenColumns);


            // Auto resize columns
            aotSafeDataGridView1.AutoResizeColumns();
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


        private void buttonDeleteRow_Click(object sender, EventArgs e)
        {
            if (SelectesKeybroadSetting == null)
            {
                MessageBox.Show("Please select a row to delete.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                // MessageBox.Show("Vui lòng chọn một dòng để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            KeybroadSettingRepository.DeleteByID(SelectesKeybroadSetting.Id);
            LoadKeybroadSetting(phone);
        }

        private void buttonUpdateRow_Click(object sender, EventArgs e)
        {
            if (SelectesKeybroadSetting == null)
            {
                MessageBox.Show("Please select a row to delete.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                // MessageBox.Show("Vui lòng chọn một dòng để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            KeybroadSettingRepository.Update(SelectesKeybroadSetting);
            LoadKeybroadSetting(phone);
        }


        private void AotSafeDataGridView1_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var KeybroadSetting = aotSafeDataGridView1.Rows[e.RowIndex].DataBoundItem as KeybroadSetting;
            if (KeybroadSetting != null)
            {
                SelectesKeybroadSetting = KeybroadSetting;
            }
        }
    }
}
