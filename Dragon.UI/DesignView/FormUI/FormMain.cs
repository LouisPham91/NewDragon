
using AdvancedSharpAdbClient;
using Dragon.ControlHelper;
using Dragon.ControlHelper.ScrcpyNet.DesignDevice;
using Dragon.ControlHelper.ScrcpyNet.Services;
using Dragon.ControlHelper.SDL2Helper;
using Dragon.ControlHelper.UIController;
using Dragon.Controller.GlobalControl.Helper;
using Dragon.Controller.Database.Services;
using Dragon.Controller.DeviceControl.Orc;
using Dragon.Controller.GlobalControl.Property;
using Dragon.Database.Services;
using Dragon.DesignView.ControlUI.Private;
using Dragon.DesignView.Public;
using Dragon.DesignView.Public.NormalMode;
using Dragon.UI.ControlHelper;
using System.Diagnostics;

namespace Dragon.DesignView.FormUI
{

    public partial class FormMain : FormOriginal
    {
        // thêm field
        PictureBoxBrightN ChevronRight = new PictureBoxBrightN();
        UserCommon userCommon = new UserCommon();
        UserOTP userOTP = new UserOTP();
        UserVersion userVersion;

        public FormMain()
        {
            PhoneStatusImage.LoadImage();
            var (ok, log) = BCDEditHelper.EnableTestSigning();
            Debug.WriteLine(log); 

            InitializeComponent();
            ThemeHelper.ThemeChanged += OnGlobalThemeChanged;
            userVersion = new UserVersion(this);
            OrcImageHelper.WarmUpOcr(@"tessdata");
            //schemaTool.EnsureTablesExist("Dragon.Database.Models");
            //schemaTool.SynchronizeProFull("Dragon.Database.Models");
            AppDataRepository.CreateIndexOperation();
            FormManager.Instance.dragonArt = new Bitmap(Path.Combine(AppContext.BaseDirectory, "Extension", "Gif", "dragonArt.jpg"));

            pictureLogo.Image = DesignHelper.GetRandomLogo();
            StartPosition = FormStartPosition.CenterScreen;

            FormManager.Instance.LoadvirtualFlowHelper(flowLayoutMain);

            ChevronRight.DG_IsBackTransparent = true;
            FontManagerSDL3.LoadBase();
            FontManagerSDL3.Warmup();

        }
        private void OnGlobalThemeChanged(object? s, EventArgs e)
        {
            if (IsDisposed) return;
            BeginInvoke(new Action(ApplyTheme)); // tránh cross-thread
        }
        public void ApplyTheme()
        {
            SuspendLayout();

            foreach (var ctrl in DesignHelper.GetAllControls(this))
            {

                //Debug.WriteLine("Control Name: " + ctrl.Name);
                if (ctrl is IThemeable themeable)
                    themeable.ApplyTheme();
                if (ctrl is PanelDevice panelDevice)
                {
                    panelDevice.ChangeColorALLControls();
                }

                if (ctrl is UserCommon userCommon)
                {
                    userCommon.ApplyTheme();
                }
                if (ctrl is UserVersion userVersion)
                {
                    userVersion.ApplyTheme();
                }
                if (ctrl is UserOTP userOTP)
                {
                    userOTP.ApplyTheme();
                }
            }
            this.BackColor = ThemeHelper.BorderNormalFist;
            ResumeLayout();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ThemeHelper.ThemeChanged -= OnGlobalThemeChanged;
                components?.Dispose();
            }
            base.Dispose(disposing);
        }
        private string scrcpyNetPath = Path.Combine(AppContext.BaseDirectory, "Extension", "ScrcpyNet");
        private string adbPath = Path.Combine(AppContext.BaseDirectory, "Extension", "ScrcpyNet", "adb.exe");
        private void FirstForm_Load(object sender, EventArgs e)
        {
            SdlManager.Init();

            FFmpeg.AutoGen.ffmpeg.RootPath = scrcpyNetPath;

            CMD.ExecuteAdb("start-server");
            //Task.Run(() =>
            //{
            //    AdbServer.Instance.StartServer(adbPath, restartServerIfNewer: true);
            //});

            var listNoneDeviceIDs = PhoneRepository.FindManyWhereDeviceIdIsNullOrEmpty();
            if (listNoneDeviceIDs != null && listNoneDeviceIDs.Count() > 0)
            {
                PhoneRepository.DeleteMany(listNoneDeviceIDs);
            }

            pictureLogo.Image = DesignHelper.GetRandomLogo();
            panelUserControl.Controls.Add(userCommon);
            userCommon.Dock = DockStyle.Fill;
            userCommon.Visible = true;

            panelUserControl.Controls.Add(userOTP);
            userOTP.Dock = DockStyle.Fill;
            userOTP.Visible = false;

            panelUserControl.Controls.Add(userVersion);
            userVersion.Dock = DockStyle.Fill;
            userVersion.Visible = false;

            labelLoginVip.MouseClick += LabelLoginVip_MouseClick;
            labelKeepVerison.MouseClick += LabelVerion_MouseClick;
            labelKeepOTG.MouseClick += LabelOTP_MouseClick;
            labelKeepCommon.MouseClick += LabelCommon_MouseClick;
            labelKeepCommon.DrawLineBaseOnClick();

            var thememode = (ThemeMode)GetSettings.GetThemeMode();
            var style = (ThemeStyle)GetSettings.GetThemeStyle();

            ThemeHelper.SetTheme(thememode, style);

            FormManager.Instance.LoadFormMain(this);
            Task.Run(async () =>
            {
                await FormManager.Instance.labelnstallApk_Mouseclick();
            });

        }

        #region LabelKeep Click 

        private void LabelLoginVip_MouseClick(object? sender, MouseEventArgs e)
        {

        }
        public void UpdateExitUserLogin(string messager, Color color)
        {
            labelLoginVip.Invoke(() =>
            {

                labelLoginVip.Text = $"{messager}";
                labelLoginVip.ForeColor = color;
            });
        }
        private void LabelVerion_MouseClick(object? sender, MouseEventArgs e)
        {
            HideAllUserControl();
            if (userVersion != null)
            {
                userVersion.Visible = true;
            }
        }

        private void LabelOTP_MouseClick(object? sender, MouseEventArgs e)
        {
            HideAllUserControl();
            if (userOTP != null)
            {
                userOTP.Visible = true;
            }
        }

        private void LabelCommon_MouseClick(object? sender, MouseEventArgs e)
        {
            HideAllUserControl();
            if (userCommon != null)
            {
                userCommon.Visible = true;
            }
        }
        public void HideAllUserControl()
        {
            foreach (Control item in panelUserControl.Controls)
            {
                if (item is UserControl)
                {
                    item.Visible = false;
                }
            }
        }
        #endregion


        #region -------------- HOÀN THÀNH KO CẦN ĐỘNG ĐẾN -------------------
        private void PictureBox_Chevrons_left_Click(object sender, EventArgs e)
        {
            this.panelNormaln1.Padding = new Padding(3);
            this.panelLeftMainBack.Visible = false;
            this.panelNormaln1.Size = new Size(20, this.panelNormaln1.Size.Height);
            if (ChevronRight != null)
            {
                ChevronRight.Image = SvgRenderer.RenderSvgFromString("<svg xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 448 512\"><!--! Font Awesome Pro 7.0.0 by @fontawesome - https://fontawesome.com License - https://fontawesome.com/license (Commercial License) Copyright 2025 Fonticons, Inc. --><path style=\"fill:var(--fa-secondary-color,currentColor);opacity:var(--fa-secondary-opacity,.4)\" d=\"\"/><path style=\"fill:var(--fa-primary-color,currentColor);opacity:var(--fa-primary-opacity,1)\" d=\"M441.5 239c9.4 9.4 9.4 24.6 0 33.9l-200 200c-9.4 9.4-24.6 9.4-33.9 0s-9.4-24.6 0-33.9l183-183-183-183c-9.4-9.4-9.4-24.6 0-33.9s24.6-9.4 33.9 0l200 200zM49.5 39l200 200c9.4 9.4 9.4 24.6 0 33.9L49.5 473c-9.4 9.4-24.6 9.4-33.9 0s-9.4-24.6 0-33.9l183-183-183-183c-9.4-9.4-9.4-24.6 0-33.9s24.6-9.4 33.9 0z\"/></svg>", 20, 20, ThemeHelper.ForeNormalFirst);
                ChevronRight.SizeMode = PictureBoxSizeMode.Zoom;
                ChevronRight.Size = new Size(17, 17);
                ChevronRight.Location = new Point(1, 57);
                if (ChevronRight.Parent is PanelNormalN normalN)
                {
                    ChevronRight.Visible = true;
                }
                else
                {
                    this.panelNormaln1.Controls.Add(ChevronRight);
                    ChevronRight.Visible = true;

                    ChevronRight.MouseClick += (s, ev) =>
                    {
                        this.panelLeftMainBack.Visible = true;
                        this.panelNormaln1.Padding = new Padding(10);
                        this.panelNormaln1.Size = new Size(396, this.panelNormaln1.Size.Height);
                        ChevronRight.Visible = false;
                    };
                }


            }
        }

        private void PictureBox_Sun_Bright_Click(object sender, EventArgs e)
        {

            if (ThemeHelper.CurrentMode == ThemeMode.Dark)
            {
                ThemeHelper.SetTheme(ThemeMode.Light, ThemeHelper.CurrentStyle);
                GetSettings.SetThemeMode((int)ThemeMode.Light);
                GetSettings.SetThemeStyle((int)ThemeHelper.CurrentStyle);
            }
            else
            {
                ThemeHelper.SetTheme(ThemeMode.Dark, ThemeHelper.CurrentStyle);
                GetSettings.SetThemeMode((int)ThemeMode.Dark);
                GetSettings.SetThemeStyle((int)ThemeHelper.CurrentStyle);
            }
        }

        public void StartFormDevice(FormDeviceControl formDeviceControl)
        {
            formDeviceControl.Show();
        }
        #endregion
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            var (ok, log) = BCDEditHelper.DisableTestSigning();
            Debug.WriteLine(log); 
        }
    }
}
