
using AdvancedSharpAdbClient;
using Dragon.ControlHelper;
using Dragon.ControlHelper.ScrcpyNet.DesignDevice;
using Dragon.ControlHelper.UIController;
using Dragon.Controller.Database.Services;
using Dragon.Controller.GlobalControl.Helper;
using Dragon.Database.Models;
using Dragon.DesignView.ControlUI.Private;
using Dragon.DesignView.Public;
using Dragon.DesignView.Public.ColorMode;
using Dragon.DesignView.Public.NormalMode;
using System.Diagnostics;
using System.Runtime.InteropServices;


namespace Dragon.DesignView.FormUI
{
    public partial class FormDeviceControl : Form, IThemeable
    {
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        public string DeviceID { get; }
        public volatile bool IsEnable = false;
        protected override bool ShowWithoutActivation => true;
        AdbClient adbClient = new AdbClient();
        public Phone phone => PhoneRepository.FindOneByDeviceID(DeviceID) ?? throw new InvalidOperationException("Phone chưa được gọi");
        MenuContext? menu;
        AutoSizeContextMenu menuInstallApp = new AutoSizeContextMenu();
        ToolStripDropDownMenu newDropDownMenu = new ToolStripDropDownMenu();
        FormExportFile formAndroidExplorer = new FormExportFile();
        FormExportApk formExportApk = new FormExportApk();
        private VirtualFlowHelper virtualFlowHelper => FormManager.Instance.virtualFlowHelper ?? throw new InvalidOperationException("LoadvirtualFlowHelper() chưa được gọi");

        public FormDeviceControl(string deviceID)
        {
            DeviceID = deviceID;
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            UpdateStyles();

            this.AutoScaleMode = AutoScaleMode.Dpi;
            InitializeComponent();
            ThemeHelper.ThemeChanged += OnGlobalThemeChanged;

            this.BackColor = Color.Lime;
            this.TransparencyKey = Color.Lime;
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = false;
            EnableFormDrag(labelPhoneTagNumber);
            EnableFormDrag(labelStatusMode);
            EnableFormDrag(labelModelName);
            EnableFormDrag(tableLayoutPanel1);

            tableLayoutPanel1.ColumnStyles[0] = new ColumnStyle(SizeType.AutoSize);
            tableLayoutPanel1.ColumnStyles[1] = new ColumnStyle(SizeType.Percent, 100);



            labelADB.Text += "\u25B8";
            labelInstallApk.Text += "\u25B8";
            labelSwithToMode.Text += "\u25B8";
            labelPhoneTagNumber.MouseHover += LabelMoveForm_MouseHover;
            labelModelName.MouseHover += LabelMoveForm_MouseHover;
            labelStatusMode.MouseHover += LabelMoveForm_MouseHover;

            labelPhoneTagNumber.MouseLeave += LabelStatusMode_MouseLeave;
            labelModelName.MouseLeave += LabelStatusMode_MouseLeave;
            labelStatusMode.MouseLeave += LabelStatusMode_MouseLeave;
            this.SizeChanged += FormDeviceMain_SizeChanged;

            this.Capture = false; // thêm vào toàn bộ form
            panelAuto.Capture = false;   // panel chứa màn hình điện thoại
            panel1.Capture = false;
            panelMenuRight.Capture = false;



        }
        public void Loading()
        {
            menu = new MenuContext(phone, adbClient);

            labelADB.MouseHover += (s, e) =>
            {
                Point screenPanelRount1 = panelRound1.PointToScreen(new Point(panelRound1.Width, 0));
                var newPoint = new Point(screenPanelRount1.X + 3, screenPanelRount1.Y);
                menu.MenuADBShow(newPoint);
                SetForegroundWindow(this.Handle);
            };
            labelSwithToMode.MouseHover += (s, e) =>
            {
                var locationLabelSwitch = labelSwithToMode.PointToScreen(new Point(labelSwithToMode.Width, 0));
                var newPoint = new Point(locationLabelSwitch.X + 27, locationLabelSwitch.Y);
                menu.MenuSwitchModeShow(newPoint);
                SetForegroundWindow(this.Handle);
            };

            ApplyTheme();
        }
        private void OnGlobalThemeChanged(object? s, EventArgs e)
        {
            if (IsDisposed || !IsHandleCreated) return;
            BeginInvoke(new Action(ApplyTheme)); // tránh cross-thread
        }
        private void FormDeviceMain_SizeChanged(object? sender, EventArgs e)
        {
            Debug.WriteLine($"Size: {this.Width},{this.Height}, Panel1: {panel1.Width},{panel1.Height}, panelMenuRight: {panelMenuRight.Width},{panelMenuRight.Height}");
        }

        private void LabelStatusMode_MouseLeave(object? sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        private void LabelMoveForm_MouseHover(object? sender, EventArgs e)
        {
            this.Cursor = CursorHelper.HandClose;
        }


        public void ApplyTheme()
        {


            foreach (var ctrl in DesignHelper.GetAllControls(this))
            {
                if (ctrl is PanelDevice panelDevice)
                {
                    panelDevice.ChangeColorALLControls();
                }
                //Debug.WriteLine("Control Name: " + ctrl.Name);
                if (ctrl is IThemeable themeable)
                    themeable.ApplyTheme();

            }

            panelRound1.BackColor = ThemeHelper.BackColofulFirst;
            panelRound2.BackColor = ThemeHelper.BackNormalFirst;

            tableLayoutPanel4.BackColor = ThemeHelper.BackNormalFirst;
            flowPanel1.BackColor = ThemeHelper.BackNormalFirst;
            tableLayoutPanel2.BackColor = ThemeHelper.BackNormalFirst;
            panelRoundCustom1.BackColor = ThemeHelper.BackNormalFirst;
            panelRoundCustom1.DG_BorderColor = ThemeHelper.BackNormalFirst;

            tableLayoutPanel1.BackColor = ThemeHelper.BackColofulFirst;
            labelPhoneTagNumber.BackColor = ThemeHelper.BackColofulFirst;
            labelModelName.BackColor = ThemeHelper.BackColofulFirst;
            labelPhoneTagNumber.ForeColor = ThemeHelper.ForceColoful3rd;
            labelModelName.ForeColor = ThemeHelper.ForceColoful3rd;

            PictureBoxCloseForm.BackColor = ThemeHelper.BackColofulFirst;
            PictureBoxCloseForm.DG_ImageColor = Color.FromArgb(245, 245, 245);

            menuInstallApp.BackColor = ThemeHelper.BackNormalFirst;
            menuInstallApp.ForeColor = ThemeHelper.ForeNormalFirst;
            menuInstallApp.ApplyTheme();
            newDropDownMenu.BackColor = ThemeHelper.BackNormalFirst;
            newDropDownMenu.Renderer = menuInstallApp.Renderer;
            menu?.ApplyTheme();


        }


        private void EnableFormDrag(Control ctrl)
        {
            if (ctrl == null) return;

            ctrl.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    ReleaseCapture();
                    SendMessage(this.Handle, 0xA1, 2, 0);
                    Console.WriteLine($"Dragging from {ctrl.Name}");
                }
            };
        }



        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }

        public (PanelDevice? panelDevice, List<PanelDevice>? seletedPanelDevices) GetDevicePanel()
        {
            var devicepanel = panel1.Controls.OfType<PanelDevice>().FirstOrDefault();
            if (devicepanel == null) return (null, null);
            var seletedPanelDevice = virtualFlowHelper.GetListPanelDeviceFormSeletect();
            return (devicepanel, seletedPanelDevice);
        }

        private void labelVolumnUp_Click(object sender, EventArgs e)
        {
            var listphone = virtualFlowHelper.GetListPhoneFormSeletect();
            if (phone != null)
                listphone.Add(phone);
            if (listphone != null && listphone.Count() > 0)
            {
                foreach (var phone in listphone)
                {
                    Task.Run(async () =>
                    {
                        await AdbHelper.VOLUME_UP(phone.Serial);
                    });
                }
            }
        }
        private void labelVolumnDown_Click(object sender, EventArgs e)
        {

            var listphone = virtualFlowHelper.GetListPhoneFormSeletect();
            if (phone != null)
                listphone.Add(phone);
            if (listphone != null && listphone.Count() > 0)
            {
                foreach (var phone in listphone)
                {
                    Task.Run(async () =>
                    {
                        await AdbHelper.VOLUME_DOWN(phone.Serial);
                    });

                }
            }
        }

        private void labelTurnOnScreen_Click(object sender, EventArgs e)
        {

            var listphone = virtualFlowHelper.GetListPhoneFormSeletect();
            if (phone != null)
                listphone.Add(phone);
            if (listphone != null && listphone.Count() > 0)
            {
                foreach (var phone in listphone)
                {
                    Task.Run(async () =>
                    {
                        await AdbHelper.Power(phone.Serial);
                    });

                }
            }
        }

        private void labelRestart_Click(object sender, EventArgs e)
        {

            var listphone = virtualFlowHelper.GetListPhoneFormSeletect();
            if (phone != null)
                listphone.Add(phone);
            if (listphone != null && listphone.Count() > 0)
            {
                foreach (var phone in listphone)
                {
                    Task.Run(async () =>
                    {
                        await AdbHelper.Reboot(phone.Serial);
                    });

                }
            }
        }



        private DateTime _lastApkScan = DateTime.MinValue;

        private async void labelInstallApk_MouseClick (object sender, EventArgs e)
        {
            var sw = Stopwatch.StartNew();

            // tránh hover liên tục quét ổ đĩa
            if ((DateTime.Now - _lastApkScan).TotalSeconds < 3)
            {
                MenuInstallApp(); // chỉ vẽ lại
                return;
            }
            _lastApkScan = DateTime.Now;

            await FormManager.Instance.labelnstallApk_Mouseclick();
            MenuInstallApp();
            sw.Stop();

            Debug.WriteLine($"Thời gian lấy install apks {sw.ElapsedMilliseconds} ms");
        }

        public void MenuInstallApp()
        {
            menuInstallApp.Items.Clear();
            menuInstallApp.Items.Add(new ToolStripMenuItem("Apks In Folder")
            {
                Enabled = false,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            });

            var dbApks = InstallApkRepository.LoadAll()
                            .OrderBy(x => x.AppName).ToList();

            int count = 1;
            foreach (var apk in dbApks)
            {
                var item = new ToolStripMenuItem($"{count}. {apk.AppName}");
                var drop = new ToolStripDropDownMenu
                {
                    ShowImageMargin = false,
                    ShowCheckMargin = false,
                    BackColor = ThemeHelper.BackNormalFirst,
                    Renderer = menuInstallApp.Renderer
                };
                item.DropDown = drop;

                // info panel
                var infoPanel = new TableLayoutI_InfoPanelC { DG_IsNormalBack = true, Margin = new Padding(0) };
                infoPanel.AddLabel("ApkName", apk.AppName);
                infoPanel.AddLabel("PackageName", apk.PackageName);
                infoPanel.AddLabel("VersionName", apk.VersionName);
                infoPanel.AddLabel("VersionCode", apk.VersionCode);
                infoPanel.AddLabel("MinAPI", apk.MinAPI.ToString());
                infoPanel.AddLabel("MaxAPI", apk.MaxAPI.ToString());
                infoPanel.AddLabel("ABI", string.Join(",", apk.ABI ?? new List<string>()));
                infoPanel.AddLabel("Path", Path.GetFileName(apk.Path));

                drop.Items.Add(new ToolStripMenuItem(" Apk Information") { Enabled = false, Font = new Font("Segoe UI", 9, FontStyle.Bold) });
                drop.AddInfoPanel(infoPanel);

                // click = cài
                var apkPath = apk.Path; // capture
                item.Click += (s, e) =>
                {
                    var phones = virtualFlowHelper.GetListPhoneFormSeletect();
                    if (phone != null && !phones.Contains(phone)) phones.Add(phone);
                    if (phones.Any())
                    {
                        var f = new FormInstallAPK { StartPosition = FormStartPosition.CenterScreen };
                        f.Show(this);
                        f.createPanel(phones, apkPath);
                    }
                };

                menuInstallApp.Items.Add(item);
                count++;
            }

            var pt = labelInstallApk.PointToScreen(new Point(labelSwithToMode.Width + 30, 0));
            menuInstallApp.Show(pt);
            SetForegroundWindow(Handle);
        }



        private void labelInstallApkFolder_Click(object sender, EventArgs e)
        {
            var dir = Directory.GetCurrentDirectory();
            var folder = $"{dir}\\Extension\\Apk";
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            System.Diagnostics.Process.Start("explorer.exe", folder);
        }

        private async void labelRotateRight_Click(object sender, EventArgs e)
        {
            var devicepanels = panel1.Controls.OfType<PanelDevice>();
            if (!devicepanels.Any()) return;
            var paneldevice = devicepanels.First();
            if (paneldevice != null && paneldevice._PDcontroller != null)
            {
                await paneldevice._PDcontroller.RotateAsync();
            }



        }

        private void labelShutDown_Click(object sender, EventArgs e)
        {
            // trước khi shut down đảm bảo tất cả phải đc đóng lại ngay cả scrcpy và panneldevice này nữa
            // sau khi đóng toàn bộ thì mới đc shutdown code dưới đây sẽ là lượt chạy cuối, 
            // vậy đầu tiên chúng ta phải tìm toàn bộ panneldevice đang đc chọn và đóng hết scrcpy của nó
            // sau đó mới shutdown nhớ devicemanager cũng phải xóa bỏ nó, switch và PictureGradientBorderDevice cũng phải xóa khỏi flowlayoutpanel
            // làm thôi nhỉ 

            var listphone = virtualFlowHelper.GetListPhoneFormSeletect();
            if (phone != null)
                listphone.Add(phone);

            if (listphone != null && listphone.Count() > 0)
            {
                foreach (var phone in listphone)
                {
                    Task.Run(async () =>
                    {
                        await AdbHelper.ShutDown(phone.Serial);
                    });

                }
            }

            this.Close();
        }

        private void PictureBoxCloseForm_Click(object sender, EventArgs e)
        {
            var panneldevice = panel1.Controls.OfType<PanelDevice>().FirstOrDefault();
            if (panneldevice != null)
            {
                FormManager.Instance.Switch_DeviceControl_To_Flow_OPEN(true);
            }
        }

        private void labelImportFile_Click(object sender, EventArgs e)
        {
            if (virtualFlowHelper == null) return;
            var listphone = virtualFlowHelper.GetListPhoneFormSeletect();
            if (phone != null)
                listphone.Add(phone);

            var dir = Directory.GetCurrentDirectory();
            var folder = Path.Combine(dir, "Extension", "File");

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = folder;
                openFileDialog.Filter = "All files (*.*)|*.*";
                //openFileDialog.Title = "Chọn một hoặc nhiều file";
                openFileDialog.Title = "Choose one or many files";
                openFileDialog.Multiselect = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string[] selectedFiles = openFileDialog.FileNames;
                    if (selectedFiles != null && selectedFiles.Count() > 0)
                    {
                        foreach (var item in listphone)
                        {
                            Task.Run(async () =>
                            {
                                foreach (var filePath in selectedFiles)
                                {
                                    await CMD.ExecuteAdbAsync($"adb -s {item.Serial} push \"{filePath}\" sdcard/FileImport");
                                }
                            });
                        }
                    }
                }
            }
        }

        private void labelImportFileFolder_Click(object sender, EventArgs e)
        {
            var listphone = virtualFlowHelper.GetListPhoneFormSeletect();
            if (phone != null)
                listphone.Add(phone);
            var dir = Directory.GetCurrentDirectory();
            var explorerDir = Path.Combine(dir, "Extension", "Apk", "Explorer.apk");

            var deviceDatas = AdbClient.Instance.GetDevices();

            foreach (var item in listphone)
            {
                Task.Run(async () =>
                {
                    var deviceData = deviceDatas.FirstOrDefault(c => c.Serial == item.Serial);
                    if (deviceData == null) return;

                    //
                    var stringCheckExit = await CMD.ExecuteAdbAsync($"adb -s {item.Serial} shell pm list package | findstr com.speedsoftware.explorer");
                    if (!string.IsNullOrEmpty(stringCheckExit) && stringCheckExit.Contains("com.speedsoftware.explorer") == false)
                    {
                        await CMD.ExecuteAdbAsync($"adb -s {item.Serial} install \"{explorerDir}\"");
                    }
                    await CMD.ExecuteAdbAsync($"adb -s {item.Serial} shell mkdir -p sdcard/FileImport");
                    await CMD.ExecuteAdbAsync($"adb -s {item.Serial} shell am start -a android.intent.action.VIEW -d file:///sdcard/FileImport -n com.speedsoftware.explorer/.Explorer");

                });
            }
        }



        private void labelExportFile_Click(object sender, EventArgs e)
        {
            if (formAndroidExplorer != null)
            {
                formAndroidExplorer.Close();
            }
            formAndroidExplorer = new FormExportFile();
            formAndroidExplorer.StartPosition = FormStartPosition.CenterScreen;
            formAndroidExplorer.GetFileFolder(phone);
            formAndroidExplorer.Show();
        }

        private void labelScreenShotFolder_Click(object sender, EventArgs e)
        {
            if (phone == null) return;
            var dir = Directory.GetCurrentDirectory();
            var folder = $"{dir}\\Extension\\ScreenShot\\{phone.PhoneTagNumber}";
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            System.Diagnostics.Process.Start("explorer.exe", folder);
        }

        private void labelScreenShot_Click(object sender, EventArgs e)
        {
            if (phone == null) return;
            var panelDevice = DeviceManager.Instance.FindOneDevicePanel(phone.DeviceID);
            if (panelDevice == null) return;
            var random = new Random().Next(10000, 99999);
            var saveName = $"{random}";
            var bitmap = panelDevice._PDcontroller?.ScreenShot();
            if (bitmap != null)
            {
                var dir = Directory.GetCurrentDirectory();
                var dirpathSave = Path.Combine(dir, "Extension", "ScreenShot", phone.PhoneTagNumber.ToString());

                if (!Directory.Exists(dirpathSave))
                {
                    Directory.CreateDirectory(dirpathSave);
                }

                var fileSave = Path.Combine(dirpathSave, $"{saveName}.png");
                bitmap.Save(fileSave);

                MessageBox.Show($"Screenshot saved as '{saveName}.png'", "Screenshot Captured", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }

        private void labelExportApk_Click(object sender, EventArgs e)
        {
            if (formExportApk.InvokeRequired)
            {
                formExportApk.Invoke(new Action(() =>
                {
                    if (formExportApk != null)
                    {
                        formExportApk.Close();
                    }
                    formExportApk = new FormExportApk();
                    formExportApk.StartPosition = FormStartPosition.CenterScreen;
                    formExportApk.GetApkPackage(phone);
                    formExportApk.Show();
                }));

            }
            else
            {
                if (formExportApk != null)
                {
                    formExportApk.Close();
                }
                formExportApk = new FormExportApk();
                formExportApk.StartPosition = FormStartPosition.CenterScreen;
                formExportApk.GetApkPackage(phone);
                formExportApk.Show();
            }
        }

        private void labelActionRecord_Click(object sender, EventArgs e)
        {
            FormManager.Instance.Switch_DeviceControl_To_AutoRecord();
        }

        private void labelHome_Click(object sender, EventArgs e)
        {
            if (phone == null) return;
            var deviceDatas = adbClient.GetDevices();
            if (deviceDatas == null || !deviceDatas.Any()) return;
            var device = deviceDatas.FirstOrDefault(c => c.Serial == phone.Serial);
            if (device == null) return;
            adbClient.ExecuteRemoteCommand("input keyevent 3", device);

        }

        private void labelAppSwitch_Click(object sender, EventArgs e)
        {
            if (phone == null) return;
            var deviceDatas = adbClient.GetDevices();
            if (deviceDatas == null || !deviceDatas.Any()) return;
            var device = deviceDatas.FirstOrDefault(c => c.Serial == phone.Serial);
            if (device == null) return;
            adbClient.ExecuteRemoteCommand("input keyevent 187", device);
        }

        private void labelBack_Click(object sender, EventArgs e)
        {
            if (phone == null) return;
            var deviceDatas = adbClient.GetDevices();
            if (deviceDatas == null || !deviceDatas.Any()) return;
            var device = deviceDatas.FirstOrDefault(c => c.Serial == phone.Serial);
            if (device == null) return;
            adbClient.ExecuteRemoteCommand("input keyevent 4", device);

        }


    }
}