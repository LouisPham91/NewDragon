

using Dragon.Database.Models;
using Dragon.Database.Services;
using Dragon.DesignView.Public;
using Dragon.DesignView.Public.NormalMode;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace Dragon.DesignView.FormUI
{
    public partial class FormSocicalNetwork : Form
    {
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private float _roundRadius = 15f;
        private Color _borderColor = Color.FromArgb(168, 168, 168);
        
        List<SocialNetwork> SocialNetworkList = new();
        SocialNetwork? SelectNetwork;

        [Category("Appearance")]
        [DefaultValue(typeof(Color), "Empty")] // đổi nếu bạn khởi tạo màu khác
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_BorderColor
        {
            get => _borderColor;
            set
            {
                if (_borderColor == value) return;
                _borderColor = value;
                BackColor = value;
                if (IsHandleCreated) Invalidate();
            }
        }

        [Category("Appearance")]
        [DefaultValue(0f)] // đổi nếu _roundRadius mặc định khác
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public float DG_RoundRadius
        {
            get => _roundRadius;
            set
            {
                if (_roundRadius == value) return;
                _roundRadius = Math.Max(0f, value);
                SetRoundedRegion();
                SetPanelMainRoundedRegion();
                if (IsHandleCreated) Invalidate();
            }
        }

        public FormSocicalNetwork()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);
            UpdateStyles();
            InitializeComponent();
            ThemeHelper.ThemeChanged += OnGlobalThemeChanged;

            this.StartPosition = FormStartPosition.CenterScreen;
            EnableFormDrag(panel1);
            EnableFormDrag(panelRoundn1);       

            
            ApplyTheme(); 
            LoadAllAppData();
        }
        private void OnGlobalThemeChanged(object? s, EventArgs e)
        {
            if (IsDisposed || !IsHandleCreated) return;
            BeginInvoke(new Action(ApplyTheme)); 
        }
        public void ApplyTheme()
        {
            SuspendLayout();

            foreach (var ctrl in DesignHelper.GetAllControls(this))
            {
                //Debug.WriteLine("Control Name: " + ctrl.Name);
                if (ctrl is IThemeable themeable)
                    themeable.ApplyTheme();
            }
            BackColor = ThemeHelper.BorderNormalFist;
            ForeColor = ThemeHelper.ForeNormalFirst;

            if (ThemeHelper.CurrentMode == ThemeMode.Light)
            {
                textBoxSearch.BackColor = Color.White;
                textBoxSearch.ForeColor = Color.Black;
               
            }
            else
            {
                txt_NetworkName.BackColor = Color.FromArgb(50, 50, 50);
                textBoxSearch.BackColor = Color.FromArgb(31, 31, 31);
                textBoxSearch.ForeColor = Color.White;
            }

            aotSafeDataGridView1.ApplyTheme();
            aotSafeDataGridView1.BorderStyle = BorderStyle.None;

            ResumeLayout();
        }

        private void FormAndroidExplorer_Load(object sender, EventArgs e)
        {
            aotSafeDataGridView1.CellClick += AotSafeDataGridView1_CellClick;
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


        private void buttonAddNetwork_Click(object sender, EventArgs e)
        {
            var name = txt_NetworkName.Text;
            SocialNetworkRepository.Add(new SocialNetwork { NetworkName = name });
            LoadAllAppData();

        }
        private void buttonUpdateNetwork_Click(object sender, EventArgs e)
        {
            var NewName = txt_NetworkName.Text;
            if (SelectNetwork == null)
            {
                MessageBox.Show("Please selecte Network for update");
                return;
            }

            if (SocialNetworkRepository.Exists(SelectNetwork.guId))
            {
                if (!SocialNetworkRepository.ExistByName(NewName))
                {
                    SelectNetwork.NetworkName = NewName;
                    SocialNetworkRepository.Update(SelectNetwork);
                    MessageBox.Show("Update Successful!");
                }
            }

        }
        private void buttonDeleteNetwork_Click(object sender, EventArgs e)
        {
            if (SelectNetwork == null)
            {
                MessageBox.Show("Please selecte Network for delete");
                return;
            }

            if (SocialNetworkRepository.Exit(SelectNetwork))
            {
                var value = MessageBox.Show("Do you want to delete this SocialNetwork and all app data", "Delete SocialNetwork", MessageBoxButtons.YesNo, MessageBoxIcon.Warning); ;
                if (value == DialogResult.Yes)
                {
                    var check = BackUpAppDataRepository.DeleteSocialNetworkWithBackup(SelectNetwork.guId, true);
                    if (check)
                    {
                        MessageBox.Show("Delete Successful!");
                    }

                }

            }
        }
        private void AotSafeDataGridView1_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var SocialNetwork = aotSafeDataGridView1.Rows[e.RowIndex].DataBoundItem as SocialNetwork;
            if (SocialNetwork != null)
            {
                SelectNetwork = SocialNetwork;
                labelNetworkSeleted.Text = SocialNetwork.NetworkName;
            }
        }

        /// <summary>
        /// Load toàn bộ AppData theo DeviceID của Phone
        /// </summary>
        private void LoadAllAppData()
        {

            try
            {
                // Lấy danh sách AppData theo DeviceID
                SocialNetworkList = SocialNetworkRepository.GetAll();

                if (SocialNetworkList == null || SocialNetworkList.Count == 0)
                {
                    SocialNetworkList = new List<SocialNetwork>();
                }

                // Bind vào DataGridView
                BindAppDataToGridView(SocialNetworkList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Bind danh sách AppData vào DataGridView
        /// </summary>
        private void BindAppDataToGridView(List<SocialNetwork> data)
        {
            // Các cột không muốn hiển thị
            var hiddenColumns = new List<string>
            {
                "guId"  // Ẩn cột Id vì không cần thiết hiển thị
            };

            if (data != null && data.Count() > 0)
            {
                // Sử dụng GridViewBinders để bind (giống FormActionIntent)
                GridViewBinders.BindSocialNetwork(aotSafeDataGridView1, data, hiddenColumns);
                // Auto resize columns
                aotSafeDataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            }
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            string text = textBoxSearch.Text;
            var list = SocialNetworkRepository.SearchSocialNetworks(text);
            BindAppDataToGridView(list);
        }

    }
}
