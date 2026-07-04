
using AdvancedSharpAdbClient.Models;
using Dragon.ControlHelper.UIController;
using Dragon.Controller.Database.Services;
using Dragon.Controller.GlobalControl.Property;
using Dragon.Database.Models;
using Dragon.DesignView.FormUI;
using Dragon.DesignView.Public;
using Dragon.DesignView.Public.ColorMode;
using Dragon.DesignView.Public.NormalMode;
using System.Data;
using System.Diagnostics;

namespace Dragon.DesignView.ControlUI.Private
{
    public partial class UserCommon : UserControl
    {
        ContextMenuStrip LabelPhoneTagMenuStrip = new ContextMenuStrip();
        TableLayoutI_InfoPanelC infoPanel = new TableLayoutI_InfoPanelC();

        FormSettings? formSettings;
        private List<int> allPhoneTagLabels = new List<int>();
        private List<int> allPhoneBoxLabels = new List<int>();
        private int currentBoxPage = 1;
        private int currentPhonePage = 1;
        private const int pageSize = 50;

        public PhoneMode phoneModeSeleted = PhoneMode.ALL;
        bool IsUseBox = false;
        int BoxNumberSelected = 0;

        private Point dragStartPoint;
        private Rectangle selectionRect;
        private bool isDragging = false;
        private Rectangle _lastScreenRect = Rectangle.Empty;

        bool IsNoneBox = false;
        List<PhoneBox> listPhoneBoxs = new List<PhoneBox>();
        List<Phone> listPhoneSeleteds = new List<Phone>();

        // 🔥 Theme handler

        public UserCommon()
        {
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            this.UpdateStyles();

            InitializeComponent();

            ApplyTheme();

            SliderMaxScreenSize.Value = GetSettings.GetMaxScreenSize();
            SliderSmallScreenSize.Value = GetSettings.GetMinScreenSize();
            SliderBitrate.Value = (int)GetSettings.GetBitrate();
            SilderFPS.Value = GetSettings.GetFps();

            LoadPhoneBox();

            this.AutoScaleMode = AutoScaleMode.Dpi;

            IsUseBox = toggleControlBox.DG_IsToggled;
            labelKeepALL.DG_IsClicked = true;
            labelKeepALL.DrawLineBaseOnClick();

            var mode = GetSettings.GetThemeMode();
            if ((ThemeMode)mode == ThemeMode.Light)
            {
                buttomDarkMode.Text = "Dark Mode";
            }
            else
            {
                buttomDarkMode.Text = "Light Mode";
            }

            SliderMaxScreenSize.DragCompleted += SliderMaxScreenSize_DragCompleted;
            SliderMaxScreenSize.KeyUp += SliderMaxScreenSize_KeyUp;
            SliderSmallScreenSize.DragCompleted += SliderSmallScreenSize_DragCompleted;
            SliderSmallScreenSize.KeyUp += SliderSmallScreenSize_KeyUp;
            SliderBitrate.DragCompleted += SliderBitrate_DragCompleted;
            SilderFPS.DragCompleted += SilderFPS_DragCompleted;

            if (toggleControlScreen.DG_IsToggled)
            {
                SliderMaxScreenSize.Enabled = true;
                SliderSmallScreenSize.Enabled = true;
                SliderBitrate.Enabled = true;
                SilderFPS.Enabled = true;
            }
            else
            {
                SliderMaxScreenSize.Enabled = false;
                SliderSmallScreenSize.Enabled = false;
                SliderBitrate.Enabled = false;
                SilderFPS.Enabled = false;
            }

            buttomDarkMode.isChangeText = true;
        }

        public void ApplyTheme()
        {
            if (IsDisposed) return;

            BackColor = ThemeHelper.BackNormalFirst;
            // Cập nhật màu cho ContextMenuStrip
            if (ThemeHelper.CurrentMode == ThemeMode.Dark)
            {
                LabelPhoneTagMenuStrip.BackColor = ThemeHelper.BackColofulFirst;
                LabelPhoneTagMenuStrip.ForeColor = ThemeHelper.ForceColofulFist;
                LabelPhoneTagMenuStrip.Renderer = new ContextMenu_Renderer() { DG_IsNoImage = true };
                numbericBoxPage.DG_BoxColor = ThemeHelper.BackNormalFirst;
                numbericPhonePage.DG_BoxColor = ThemeHelper.BackNormalFirst;
            }
            else
            {
                LabelPhoneTagMenuStrip.BackColor = ThemeHelper.BackNormalFirst;
                LabelPhoneTagMenuStrip.ForeColor = ThemeHelper.ForeNormalFirst;
                LabelPhoneTagMenuStrip.Renderer = new ContextMenu_Renderer() { DG_IsNoImage = true };
                numbericBoxPage.DG_BoxColor = ThemeHelper.BackNormalFirst;
                numbericPhonePage.DG_BoxColor = ThemeHelper.BackNormalFirst;
            }
      
        }

        public void LoadPhoneBox()
        {
            var check = PhoneBoxRepository.LoadAll();
            if (check.Count() == 0)
            {
                PhoneBoxRepository.AddNew();
            }
        }

        #region FlatButton Click
        private void ButtomSetting_Click(object? sender, EventArgs e)
        {
            if (formSettings != null)
            {
                formSettings.Close();
            }
            formSettings = new FormSettings();
            formSettings.StartPosition = FormStartPosition.CenterScreen;
            formSettings.Show();
        }

        private void buttomMore_Click(object sender, EventArgs e)
        {
            Point screenPoint = buttonMore.PointToScreen(new Point(buttonMore.Width, 0));

            FormDeviceMode popupForm = new FormDeviceMode
            {
                FormBorderStyle = FormBorderStyle.None,
                StartPosition = FormStartPosition.Manual,
                ShowInTaskbar = false,
                Location = screenPoint,
            };
            popupForm.Deactivate += (s, args) => popupForm.Close();
            popupForm.Show();
        }

        private void buttonDevice_Click(object sender, EventArgs e)
        {
            FormAppData formAppData = new FormAppData(null, null, false)
            {
                StartPosition = FormStartPosition.CenterScreen
            };
            formAppData.Show();
        }

        private void buttomDarkMode_Click(object sender, EventArgs e)
        {
            // tránh ApplyTheme() của chính nút chạy lúc đổi Text
            buttomDarkMode.isChangeText = false;

            // 1. Đảo mode dựa trên mode HIỆN TẠI (không dựa vào Text)
            var newMode = ThemeHelper.CurrentMode == ThemeMode.Dark
                ? ThemeMode.Light
                : ThemeMode.Dark;

            // 2. Lưu theme
            ThemeHelper.SetTheme(newMode, ThemeHelper.CurrentStyle);
            GetSettings.SetThemeMode((int)newMode);
            GetSettings.SetThemeStyle((int)ThemeHelper.CurrentStyle);
            GetSettings.GetThemeMode();


            // 4. Cập nhật text NÚT = hành động kế tiếp
            //    nếu đang ở Light -> nút phải ghi "Dark Mode"
            //    nếu đang ở Dark  -> nút phải ghi "Light Mode"
            buttomDarkMode.Text = newMode == ThemeMode.Light ? "Dark Mode" : "Light Mode";

            buttomDarkMode.isChangeText = true;
        }

        private bool isLoading = false;
        private async void buttomLoad_Click(object sender, EventArgs e)
        {
            if (isLoading)
            {
                Debug.WriteLine("LoadingDevice is already running, skip duplicate click.");
                return;
            }

            try
            {
                isLoading = true;
                await LoadingDevice();
                Debug.WriteLine("ButtomLoad Complete Loading");
                await DeviceManager.Instance.ShowDevicePanel();
            }
            finally
            {
                isLoading = false;
            }
        }

        private void buttonADB_Click(object sender, EventArgs e)
        {
            MouseSelect mouseSelect = new MouseSelect();
            mouseSelect.Show();
        }
        #endregion

        #region Event Slider, ToolControlScreen
        private void SliderMaxScreenSize_KeyUp(object? sender, KeyEventArgs e)
        {
            var value = SliderMaxScreenSize.Value;
            var masize = GetSettings.GetMaxScreenSize();
            if (value != masize)
            {
                GetSettings.SetMaxScreenSize(value);
                DeviceManager.Instance.ChangeAllMaxSizePanelDevice();
            }
        }

        private void SliderMaxScreenSize_DragCompleted(object? sender, EventArgs e)
        {
            var value = SliderMaxScreenSize.Value;
            var masize = GetSettings.GetMaxScreenSize();
            if (value != masize)
            {
                GetSettings.SetMaxScreenSize(value);
                DeviceManager.Instance.ChangeAllMaxSizePanelDevice();
            }
        }

        private void SliderSmallScreenSize_DragCompleted(object? sender, EventArgs e)
        {
            var value = SliderSmallScreenSize.Value;
            var minsize = GetSettings.GetMinScreenSize();
            if (value != minsize)
            {
                GetSettings.SetMinScreenSize(value);
                DeviceManager.Instance.ChangeAllMinSizePanelDevice();
            }
        }

        private void SliderSmallScreenSize_KeyUp(object? sender, KeyEventArgs e)
        {
            var value = SliderSmallScreenSize.Value;
            var minsize = GetSettings.GetMinScreenSize();
            if (value != minsize)
            {
                GetSettings.SetMinScreenSize(value);
                DeviceManager.Instance.ChangeAllMinSizePanelDevice();
            }
        }

        private void SliderBitrate_DragCompleted(object? sender, EventArgs e)
        {
            var value = SliderBitrate.Value;
            var bitrate = GetSettings.GetBitrate();
            if (value != bitrate)
            {
                GetSettings.SetBitrate(value);
                DeviceManager.Instance.ChangeAllMaxSizePanelDevice();
            }
        }

        private void SilderFPS_DragCompleted(object? sender, EventArgs e)
        {
            var value = SilderFPS.Value;
            var fps = GetSettings.GetFps();
            if (value != fps)
            {
                GetSettings.SetFps(value);
                DeviceManager.Instance.ChangeAllMaxSizePanelDevice();
            }
        }

        private void toggleControlScreen_Click(object sender, EventArgs e)
        {
            var toggleButton = sender as ToggleWithTextControl;
            if (toggleButton != null)
            {
                toggleButton.Invoke(new Action(() =>
                {
                    if (toggleButton.DG_IsToggled == true)
                    {
                        SliderMaxScreenSize.Enabled = false;
                        SliderSmallScreenSize.Enabled = false;
                        SliderBitrate.Enabled = false;
                        SilderFPS.Enabled = false;
                    }
                    else
                    {
                        SliderMaxScreenSize.Enabled = true;
                        SliderSmallScreenSize.Enabled = true;
                        SliderBitrate.Enabled = true;
                        SilderFPS.Enabled = true;
                    }
                }));
            }
        }
        #endregion

        #region Support
        public async Task LoadingDevice()
        {
            await DeviceManager.Instance.LoadDeviceList();
            LoadPhoneLabel();
            LoadBoxLabel();
        }
        #endregion

        #region Phone Tags

        // lấy tất cả control nằm trong rect (rect tính theo tọa độ của container)


        private void Label_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if ((ModifierKeys & Keys.Control) == Keys.Control)
                {
                    if (sender is Label label)
                    {
                        label.BackColor = Color.FromArgb(14, 83, 154);
                        if (int.TryParse(label.Text, out int phoneNumber))
                        {
                            var phone = PhoneRepository.FindOneByPhoneTagNumber(phoneNumber);
                            if (phone == null) return;
                            if (!listPhoneSeleteds.Any(p => p.DeviceID == phone.DeviceID))
                            {
                                listPhoneSeleteds.Add(phone);
                            }
                        }
                    }
                }
                else
                {
                    isDragging = true;
                    dragStartPoint = tableLayoutPhoneTag.PointToClient(Cursor.Position);
                    selectionRect = new Rectangle(dragStartPoint, Size.Empty);
                }
            }
        }
   
        private void Label_MouseMove(object? sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point currentPoint = tableLayoutPhoneTag.PointToClient(Cursor.Position);
                int x = Math.Min(dragStartPoint.X, currentPoint.X);
                int y = Math.Min(dragStartPoint.Y, currentPoint.Y);
                int width = Math.Abs(dragStartPoint.X - currentPoint.X);
                int height = Math.Abs(dragStartPoint.Y - currentPoint.Y);

                selectionRect = new Rectangle(x, y, width, height);
                _lastScreenRect = tableLayoutPhoneTag.RectangleToScreen(selectionRect);
                ControlPaint.DrawReversibleFrame(_lastScreenRect, Color.Blue, FrameStyle.Dashed);
            }
        }

        private void Label_MouseUp(object? sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                _lastScreenRect = Rectangle.Empty;
                isDragging = false;
                SelectLabelsInRectangle();
                tableLayoutPhoneTag.Refresh();
            }
        }

        private void SelectLabelsInRectangle()
        {
            listPhoneSeleteds.Clear();

            foreach (LabelPhones label in tableLayoutPhoneTag.Controls)
            {
                Rectangle labelRect = tableLayoutPhoneTag.RectangleToClient(
                    label.RectangleToScreen(label.ClientRectangle)
                );

                if (selectionRect.IntersectsWith(labelRect))
                {
                    label.BackColor = Color.FromArgb(10, 109, 176);
                    label.ForeColor = ThemeHelper.ForeNormalFirst;

                    if (int.TryParse(label.Text, out int phoneNumber))
                    {
                        var listSelects = PhoneRepository.FindManyByPhoneTagNumber(phoneNumber);
                        if (listSelects == null || listSelects.Count == 0) continue;
                        foreach (var phone in listSelects)
                        {
                            if (!listPhoneSeleteds.Any(p => p.DeviceID == phone.DeviceID))
                            {
                                listPhoneSeleteds.Add(phone);
                            }
                        }
                    }
                }
                else
                {
                    label.ApplyTheme();
                }
            }
        }

        private void ProcessLabelSelection(LabelPhones label)
        {
            label.BackColor = Color.FromArgb(14, 83, 154);

            if (!int.TryParse(label.Text, out int phoneNumber))
                return;

            var phones = PhoneRepository.FindManyByPhoneTagNumber(phoneNumber);
            foreach (var phone in phones)
            {
                if (!listPhoneSeleteds.Any(p => p.DeviceID == phone.DeviceID))
                    listPhoneSeleteds.Add(phone);
            }
        }

        private void TableLayoutPhoneTag_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;
            var clickedControl = tableLayoutPhoneTag.GetChildAtPoint(e.Location);
            if (clickedControl == null)
            {
                ClearAllLabelThemes();
            }
        }

        private void ClearAllLabelThemes()
        {
            foreach (Control ctrl in tableLayoutPhoneTag.Controls)
            {
                if (ctrl is LabelPhones lbl)
                {
                    lbl.ApplyTheme();
                }
            }
        }

        public void ShowListPhone()
        {
            foreach (LabelPhones label in tableLayoutPhoneTag.Controls)
            {
                tableLayoutPhoneTag.Controls.Remove(label);
                label.Dispose();
            }
            tableLayoutPhoneTag.Controls.Clear();

            var listphone = getListLabel();
            if (listphone == null || listphone.Count() == 0) return;

            int totalPages = (int)Math.Ceiling(listphone.Count / (double)pageSize);
            numbericPhonePage.DG_Maximum = totalPages;
            numbericPhonePage.Value = totalPages;
            ShowPhonePage(totalPages);
        }

        public void LoadPhoneLabel()
        {
            foreach (LabelPhones label in tableLayoutPhoneTag.Controls)
            {
                tableLayoutPhoneTag.Controls.Remove(label);
                label.Dispose();
            }
            tableLayoutPhoneTag.Controls.Clear();
            allPhoneTagLabels.Clear();

            var listphone = PhoneRepository.LoadAll();

            if (listphone?.Count() > 0)
            {
                foreach (var phone in listphone)
                {
                    allPhoneTagLabels.Add(phone.PhoneTagNumber);
                }

                int totalPages = (int)Math.Ceiling(allPhoneTagLabels.Count / (double)pageSize);
                if (totalPages > 0)
                {
                    numbericPhonePage.DG_Maximum = totalPages;
                    numbericPhonePage.Value = totalPages;
                    ShowPhonePage(totalPages);
                }
                else
                {
                    numbericPhonePage.DG_Maximum = 1;
                    numbericPhonePage.Value = 1;
                }
            }
        }

        private void ShowPhonePage(int pageNumber)
        {
            ShowPhonePageHelper(pageNumber);
        }

        public void ShowPhonePageHelper(int pageNumber)
        {
            var listphone = getListLabel();
            if (listphone == null || listphone.Count() == 0) return;

            labelPhoneTags.Text = $"Phone Tags ({listphone.Count})";

            int totalItems = listphone.Count;
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            if (pageNumber < 1 || pageNumber > totalPages)
                return;

            int start = (pageNumber - 1) * pageSize;
            int end = Math.Min(start + pageSize, totalItems);

            var list = new List<Label>();
            for (int i = start; i < end; i++)
            {
                list.Add(listphone[i]);
            }

            tableLayoutPhoneTag.Controls.AddRange(list.ToArray());

            currentPhonePage = pageNumber;
            IsNoneBox = false;
        }

        public void SeletecAllLabelPhone()
        {
            listPhoneSeleteds.Clear();
            foreach (LabelPhones label in tableLayoutPhoneTag.Controls.OfType<LabelPhones>())
            {
                ProcessLabelSelection(label);
            }
        }

        public List<LabelPhones>? getListLabel()
        {
            listPhoneSeleteds.Clear();

            var listphoneUse = new List<Phone>();
            if (IsNoneBox)
            {
                listphoneUse = PhoneRepository.FindManyByPhoneBoxId(-1); // -1 để lấy những điện thoại không có box
            }
            else if (phoneModeSeleted == PhoneMode.USB)
            {
                listphoneUse = PhoneRepository.FindManyByPhoneMode(PhoneMode.USB);
            }
            else if (phoneModeSeleted == PhoneMode.WIFI)
            {
                listphoneUse = PhoneRepository.FindManyByPhoneMode(PhoneMode.WIFI);
            }
            else if (phoneModeSeleted == PhoneMode.UATX)
            {
                listphoneUse = PhoneRepository.FindManyByPhoneMode(PhoneMode.UATX);
            }
            else if (phoneModeSeleted == PhoneMode.WATX)
            {
                listphoneUse = PhoneRepository.FindManyByPhoneMode(PhoneMode.WATX);
            }
            else if (phoneModeSeleted == PhoneMode.UHDI)
            {
                listphoneUse = PhoneRepository.FindManyByPhoneMode(PhoneMode.UHDI);
            }
            else if (phoneModeSeleted == PhoneMode.WHDI)
            {
                listphoneUse = PhoneRepository.FindManyByPhoneMode(PhoneMode.WHDI);
            }
            else if (phoneModeSeleted == PhoneMode.ACC)
            {
                listphoneUse = PhoneRepository.FindManyByPhoneMode(PhoneMode.ACC);
            }
            else
            {
                listphoneUse = PhoneRepository.LoadAll();
            }

            if (listphoneUse == null || listphoneUse.Count() == 0) return null;
            if (allPhoneTagLabels == null || allPhoneTagLabels.Count() == 0) return null;

            if (!IsUseBox)
            {
                var listPhoneTagNumbers = listphoneUse.Select(c => c.PhoneTagNumber).ToList();
                if (listPhoneTagNumbers != null && listPhoneTagNumbers.Count() > 0)
                {
                    var ListLabel = allPhoneTagLabels.Where(c => listPhoneTagNumbers.Contains(Convert.ToInt32(c))).ToList();
                    if (ListLabel != null && ListLabel.Count() > 0)
                    {
                        var listLabels = new List<LabelPhones>();
                        foreach (var phoneNumtag in ListLabel)
                        {
                            var phone = listphoneUse.FirstOrDefault(p => p.PhoneTagNumber == phoneNumtag);
                            bool isConnected = phone != null && phone.DeviceState == DeviceState.Online;
                            listLabels.Add(getListLabel(phoneNumtag, isConnected));
                        }
                        return listLabels;
                    }
                }
            }
            else
            {
                var listPhoneWithBoxID = listphoneUse.Where(p => p.PhoneBoxId != null && p.PhoneBoxId == BoxNumberSelected).Select(c => c.PhoneTagNumber).ToList();
                if (listPhoneWithBoxID != null && listPhoneWithBoxID.Count() > 0)
                {
                    var ListLabel = allPhoneTagLabels.Where(c => listPhoneWithBoxID.Contains(Convert.ToInt32(c))).ToList();
                    if (ListLabel != null && ListLabel.Count() > 0)
                    {
                        var listLabels = new List<LabelPhones>();
                        foreach (var phoneNumtag in ListLabel)
                        {
                            var phone = listphoneUse.FirstOrDefault(p => p.PhoneTagNumber == phoneNumtag);
                            bool isConnected = phone != null && phone.DeviceState == DeviceState.Online;
                            listLabels.Add(getListLabel(phoneNumtag, isConnected));
                        }
                        return listLabels;
                    }
                }
            }

            return null;
        }

        public LabelPhones getListLabel(int num, bool Online = false)
        {
            LabelPhones label = new LabelPhones();
            label.OnSelectAllPhones += SeletecAllLabelPhone;
            label.Margin = new Padding(1);
            label.Dock = DockStyle.Fill;
            label.Text = num.ToString();
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.ImageAlign = ContentAlignment.MiddleCenter;
            label.MouseClick += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    isDragging = false;
                    LabelPhoneTagClick(label);
                }
            };
            label.MouseDown += Label_MouseDown;
            label.MouseMove += Label_MouseMove;
            label.MouseUp += Label_MouseUp;
            Debug.WriteLine($"Label Phone Tag : {label.Text} - IsOnline: {Online}");
            label.IsConnect(Online);

            return label;
        }

        public void LabelPhoneTagClick(Label label)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<Label>(LabelPhoneTagClick), label);
                return;
            }

            if (label == null || label.IsDisposed) return;
            var IsAnyPhone = PhoneRepository.IsAny();
            if (IsAnyPhone == false) return;

            Point screenPoint = label.PointToScreen(new Point(label.Width, label.Height));

            LabelPhoneTagMenuStrip.Items.Clear();
            infoPanel?.Dispose();

            var phoneModeState = new string[] { "USB", "WIFI", "UATX", "WATX", "UHDI", "WHDI", "ACC" };

            var phone = PhoneRepository.FindOneByPhoneTagNumber(Convert.ToInt32(label.Text));
            if (phone == null) return;
            if (listPhoneSeleteds == null || listPhoneSeleteds.Count() == 0) return;

            infoPanel = new TableLayoutI_InfoPanelC();
            infoPanel.AddLabel("Phone Tag", phone.PhoneTagNumber.ToString());
            infoPanel.AddLabel("Model", phone.Model);
            infoPanel.AddLabel("Product", phone.Product);
            infoPanel.AddLabel("DeviceID", phone.DeviceID);
            var phonemode = infoPanel.AddLabel("Phone Mode", phone.PhoneMode.ToString());
            var boxNumber = infoPanel.AddLabel("Box Number", (phone.PhoneBoxId.HasValue) ? phone.PhoneBoxId.Value.ToString() : "");
            infoPanel.AddLabel("UHDI", phone.IsUHDI.ToString());
            infoPanel.AddLabel("Ipv4", phone.Ipv4);
            infoPanel.AddLabel("Physical Size", $"{phone.PhysicalWidth}x{phone.PhysicalHeight}");
            infoPanel.AddLabel("Install App Accessible", phone.IsAccessibleAppInstall.ToString());
            infoPanel.AddLabel("Ping Wifi", phone.IsPingWifi.ToString());
            infoPanel.AddLabel("★ IsRunning", phone.IsRunning.ToString());
            infoPanel.AddLabel("★ State", phone.DeviceState.ToString());

            var toolStripProVersion = new MyToolStripMenuItem();
            toolStripProVersion.Text = $"🔢 Phone Proc Version : 👉";
            var rendererProVersion = new ContextMenu_Renderer { DG_IsNoImage = true };
            toolStripProVersion.DropDown.Renderer = rendererProVersion;
            if (toolStripProVersion.DropDown is ToolStripDropDownMenu ddMenuProcVersion)
                ddMenuProcVersion.ShowImageMargin = false;
            if (!string.IsNullOrEmpty(phone.ProcVersion))
            {
                var toolSctipProcVersion = new MyLabeledToolStripOnlyTextBox(phone.ProcVersion, 400, 50);
                toolStripProVersion.DropDownItems.Add(toolSctipProcVersion);
                toolStripProVersion.Padding = new Padding(0);
                toolStripProVersion.Margin = new Padding(0);
            }
            toolStripProVersion.Padding = new Padding(0);
            toolStripProVersion.Margin = new Padding(0);

            var toolStripProCpuInfo = new MyToolStripMenuItem();
            toolStripProCpuInfo.Text = $"ℹ️ Phone Proc Cpuinfo : 👉";
            if (toolStripProCpuInfo.DropDown is ToolStripDropDownMenu ddMenuProCpuInfo)
                ddMenuProCpuInfo.ShowImageMargin = false;

            if (!string.IsNullOrEmpty(phone.ProcCpuInfo))
            {
                var toolSctipProcCpuinfo = new MyLabeledToolStripOnlyTextBox(phone.ProcCpuInfo, 400, 300);
                toolStripProCpuInfo.DropDownItems.Add(toolSctipProcCpuinfo);
            }

            var toolStripOffLine = new MyToolStripMenuItem();
            toolStripOffLine.Text = $"📵 Phone Offline or Authorizing : 👉";
            var renderer = new ContextMenu_Renderer { DG_IsNoImage = true };
            toolStripOffLine.DropDown.Renderer = renderer;
            if (toolStripOffLine.DropDown is ToolStripDropDownMenu ddMenu)
                ddMenu.ShowImageMargin = false;

            var offlineList = listPhoneSeleteds.Where(c => c.DeviceState != DeviceState.Online).ToList();
            if (offlineList != null && offlineList.Count() > 0)
            {
                foreach (var item in offlineList)
                {
                    var newtoolstrip = new MyToolStripMenuItem();
                    newtoolstrip.Text = $" Phone {item.PhoneTagNumber} : " + phone.DeviceState.ToString();
                    newtoolstrip.Image = null;
                    newtoolstrip.DropDown.ImageScalingSize = Size.Empty;
                    newtoolstrip.DisplayStyle = ToolStripItemDisplayStyle.Text;
                    toolStripOffLine.DropDownItems.Add(newtoolstrip);
                }
            }

            var toolStripShowUp = new MyToolStripMenuItem();
            toolStripShowUp.Text = $"📱 Show ({listPhoneSeleteds.Count}) Phones Screen";
            toolStripShowUp.Click += async (s, e) =>
            {
                await DeviceManager.Instance.ShowDevicePanel(listPhoneSeleteds);
            };

            var toolStripHideUp = new MyToolStripMenuItem();
            toolStripHideUp.Text = $"🚫 Hide ({listPhoneSeleteds.Count}) Phones Screen";
            toolStripHideUp.Click += (s, e) =>
            {
                FormManager.Instance.RemoveAllcontrolPhoneSelected(listPhoneSeleteds);
            };

            MyToolStripMenuItem ToolStripChangPhone = new MyToolStripMenuItem();
            ToolStripChangPhone.Text = $"📱 Change Phone Mode ({listPhoneSeleteds.Count}) Phones ";
            var rendererChangPhone = new ContextMenu_Renderer { DG_IsNoImage = true };
            ToolStripChangPhone.DropDown.Renderer = rendererChangPhone;
            if (ToolStripChangPhone.DropDown is ToolStripDropDownMenu ddMenuChangPhone)
                ddMenuChangPhone.ShowImageMargin = false;

            foreach (var phoneState in phoneModeState)
            {
                MyToolStripMenuItem ToolStripChanger = new MyToolStripMenuItem();
                ToolStripChanger.Text = phoneState;
                ToolStripChanger.Click += async (s, e) =>
                {
                    if (Enum.TryParse<PhoneMode>(phoneState, out var selectedMode))
                    {
                        await DeviceManager.Instance.ChangeDeviceMode(selectedMode, listPhoneSeleteds);
                    }
                };
                ToolStripChangPhone.DropDownItems.Add(ToolStripChanger);
            }

            MyToolStripMenuItem ToolStripchangeBox = new MyToolStripMenuItem();
            ToolStripchangeBox.Text = $"📦 Change BoxNum ({listPhoneSeleteds.Count}) Phones ";
            var rendererChangBox = new ContextMenu_Renderer { DG_IsNoImage = true };
            ToolStripchangeBox.DropDown.Renderer = rendererChangBox;
            if (ToolStripchangeBox.DropDown is ToolStripDropDownMenu ddchangBox)
                ddchangBox.ShowImageMargin = false;

            MyLabeledToolStripOnlyCombox ToolStripOnlyComboxchangeBoxNum = 
            new MyLabeledToolStripOnlyCombox("Change BoxNum", 70, 24, DockStyle.Right, 20);
            ToolStripchangeBox.DropDownItems.Add(ToolStripOnlyComboxchangeBoxNum);

            var isSameBoxID = listPhoneSeleteds.Any(c => c.PhoneBoxId == null || c.PhoneBoxId != phone.PhoneBoxId);
            var listBoxs = PhoneBoxRepository.LoadAll();
            ToolStripOnlyComboxchangeBoxNum.ComboBox.Items.AddRange(listBoxs.ToArray());
            if (isSameBoxID == false && listBoxs != null && listBoxs.Count() > 0)
            {
                var indexboxid = listBoxs.FindIndex(c => c.Id == phone.PhoneBoxId);
                ToolStripOnlyComboxchangeBoxNum.ComboBox.SelectedIndex = indexboxid;
            }

            ToolStripOnlyComboxchangeBoxNum.ComboBox.SelectedValueChanged += (s, e) =>
            {
                var cb = s as ComboBoxCustomNC;
                if (cb != null)
                {
                    var selectPhoneBox = cb.SelectedItem as PhoneBox;
                    if (selectPhoneBox != null)
                    {
                        boxNumber.Text = selectPhoneBox.Id.ToString();
                        foreach (var phoneItem in listPhoneSeleteds)
                        {
                            phoneItem.PhoneBoxId = selectPhoneBox.Id;
                            PhoneRepository.Update(phoneItem);
                        }
                        ShowPhonePage((int)numbericPhonePage.Value);
                    }
                }
            };

            var toolStripReconect = new MyToolStripMenuItem();
            toolStripReconect.Text = $"⏳ Check And Reconnect ({listPhoneSeleteds.Count}) Phones";
            toolStripReconect.Click += (s, e) => { };

            var toolStripSocialAccount = new MyToolStripMenuItem();
            toolStripSocialAccount.Text = $"🌐 Social Account ({listPhoneSeleteds.Count}) Phones";
            toolStripSocialAccount.Click += (s, e) => { };

            var toolStripReorderAllPhoneTagNumbers = new MyToolStripMenuItem();
            toolStripReorderAllPhoneTagNumbers.Text = $"🔄 Reoder All Phones TagNumbers";
            toolStripReorderAllPhoneTagNumbers.Click += (s, e) =>
            {
                PhoneRepository.ReorderAllPhoneTagNumbers();
            };

            var toolStripIntentAction = new MyToolStripMenuItem();
            toolStripIntentAction.Text = $"▶ Adb Acion DeepLink For Model {phone.Model}";
            toolStripIntentAction.Click += (s, e) =>
            {
                var fomactionIntent = FormManager.Instance.formActionIntent;
                if (FormManager.Instance.formActionIntent == null || FormManager.Instance.formActionIntent.IsDisposed)
                {
                    fomactionIntent = FormManager.Instance.CreateNewFormActionIntent(phone);
                    fomactionIntent.StartPosition = FormStartPosition.Manual;
                }
                if (fomactionIntent == null) return;

                var screen = Screen.PrimaryScreen!.WorkingArea;

                if (fomactionIntent.Width > screen.Width)
                    fomactionIntent.Width = screen.Width;

                if (fomactionIntent.Height > screen.Height)
                    fomactionIntent.Height = screen.Height;

                int newX = screen.Right - fomactionIntent.Width;
                int newY = screen.Top + (screen.Height - fomactionIntent.Height) / 2;

                if (newX < screen.Left) newX = screen.Left;
                if (newY < screen.Top) newY = screen.Top;

                fomactionIntent.Location = new Point(newX, newY);

                if (!fomactionIntent.Visible)
                {
                    fomactionIntent.Show();
                }
                else
                {
                    fomactionIntent.BringToFront();
                }
            };

            var toolStripRotateTest = new MyToolStripMenuItem();
            toolStripRotateTest.Text = $"🔄 Rotate Phone";
            toolStripRotateTest.Click += async (s, e) =>
            {
                var fist = listPhoneSeleteds.First();
                var panelDevice = DeviceManager.Instance.FindOneDevicePanel(fist.DeviceID);
                if (panelDevice != null && panelDevice._PDcontroller != null)
                {
                    await panelDevice._PDcontroller.RotateAsync();
                }
            };

            LabelPhoneTagMenuStrip.Items.Add(new ToolStripControlHost(infoPanel));
            LabelPhoneTagMenuStrip.Items.Add(toolStripReorderAllPhoneTagNumbers);
            LabelPhoneTagMenuStrip.Items.Add(toolStripProVersion);
            LabelPhoneTagMenuStrip.Items.Add(toolStripProCpuInfo);
            LabelPhoneTagMenuStrip.Items.Add(toolStripShowUp);
            LabelPhoneTagMenuStrip.Items.Add(toolStripHideUp);
            LabelPhoneTagMenuStrip.Items.Add(toolStripSocialAccount);
            LabelPhoneTagMenuStrip.Items.Add(toolStripReconect);

            if (offlineList != null && offlineList.Count() > 0)
            {
                LabelPhoneTagMenuStrip.Items.Add(toolStripOffLine);
            }
            LabelPhoneTagMenuStrip.Items.Add(ToolStripChangPhone);
            LabelPhoneTagMenuStrip.Items.Add(ToolStripchangeBox);
            LabelPhoneTagMenuStrip.Items.Add(toolStripIntentAction);
            LabelPhoneTagMenuStrip.Items.Add(toolStripRotateTest);

            LabelPhoneTagMenuStrip.ShowImageMargin = false;
            LabelPhoneTagMenuStrip.Renderer = new ContextMenu_Renderer()
            {
                DG_IsNoImage = true,
            };

            LabelPhoneTagMenuStrip.Show(screenPoint);
        }

        private void labelKeepALL_Click(object sender, EventArgs e)
        {
            phoneModeSeleted = PhoneMode.ALL;
            ShowListPhone();
        }

        private void labelKeepUSB_Click(object sender, EventArgs e)
        {
            phoneModeSeleted = PhoneMode.USB;
            ShowListPhone();
        }

        private void labelKeepWIFI_Click(object sender, EventArgs e)
        {
            phoneModeSeleted = PhoneMode.WIFI;
            ShowListPhone();
        }

        private void labelKeepUATXClick(object sender, EventArgs e)
        {
            phoneModeSeleted = PhoneMode.UATX;
            ShowListPhone();
        }

        private void labelKeepWATX_Click(object sender, EventArgs e)
        {
            phoneModeSeleted = PhoneMode.WATX;
            ShowListPhone();
        }

        private void labelKeepUHDI_Click(object sender, EventArgs e)
        {
            phoneModeSeleted = PhoneMode.UHDI;
            ShowListPhone();
        }

        private void labelKeepWHDI_Click(object sender, EventArgs e)
        {
            phoneModeSeleted = PhoneMode.WHDI;
            ShowListPhone();
        }

        private void labelKeepACC_Click(object sender, EventArgs e)
        {
            phoneModeSeleted = PhoneMode.ACC;
            ShowListPhone();
        }

        private void labelNoneBox_Click(object sender, EventArgs e)
        {
            IsNoneBox = true;
            labelKeepALL.DrawLineBaseOnClick();
            ShowListPhone();
        }

        private void numbericPhonePage_MouseClick(object sender, MouseEventArgs e)
        {
            ShowPhonePage((int)numbericPhonePage.Value);
        }

        private void numbericPhonePage_KeyUp(object sender, KeyEventArgs e)
        {
            if (numbericPhonePage.Value <= numbericPhonePage.DG_Maximum)
            {
                ShowPhonePage((int)numbericPhonePage.Value);
            }
        }
        #endregion

        #region Phone Box
        private void toggleControlBox_MouseUp(object sender, MouseEventArgs e)
        {
            var toggleButton = sender as ToggleWithTextControl;
            if (toggleButton == null) return;
            IsUseBox = toggleButton.DG_IsToggled;
            if (!IsUseBox)
            {
                phoneModeSeleted = PhoneMode.ALL;
                ShowListPhone();
            }
            else
            {
                foreach (LabelPhones label in tableLayoutPhoneTag.Controls)
                {
                    tableLayoutPhoneTag.Controls.Remove(label);
                    label.Dispose();
                }
                tableLayoutPhoneTag.Controls.Clear();
            }
        }

        public labelBox getBoxLabel(int boxNum)
        {
            labelBox label = new labelBox();
            label.Margin = new Padding(1);
            label.Dock = DockStyle.Fill;
            label.Text = boxNum.ToString();
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.ImageAlign = ContentAlignment.MiddleCenter;
            label.MouseClick += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (IsUseBox)
                    {
                        listPhoneSeleteds.Clear();
                        phoneModeSeleted = PhoneMode.ALL;
                        var boxID = int.Parse(label.Text);
                        BoxNumberSelected = boxID;
                        ShowListPhone();
                    }
                }
                else if (e.Button == MouseButtons.Right)
                {
                    if (IsUseBox)
                    {
                        listPhoneSeleteds.Clear();
                        LablPhonBoxClick(label);
                    }
                }
            };
            return label;
        }

        public void LoadBoxLabel()
        {
            foreach (labelBox boxLabel in tableLayoutPhoneBox.Controls)
            {
                tableLayoutPhoneBox.Controls.Remove(boxLabel);
                boxLabel.Dispose();
            }
            allPhoneBoxLabels.Clear();
            listPhoneBoxs.Clear();

            listPhoneBoxs = PhoneBoxRepository.LoadAll() ?? new List<PhoneBox>();
            if (listPhoneBoxs?.Count() > 0)
            {
                listPhoneBoxs = listPhoneBoxs.OrderBy(c => c.Id).ToList();
                foreach (var box in listPhoneBoxs)
                {
                    allPhoneBoxLabels.Add(box.Id);
                }
                labelPhoneBox.Text = $"Phone Boxs ({allPhoneBoxLabels.Count})";

                int totalItems = allPhoneBoxLabels.Count;
                int totalBoxpage = (int)Math.Ceiling(totalItems / (double)pageSize);
                if (totalBoxpage > 0)
                {
                    numbericBoxPage.DG_Maximum = totalBoxpage;
                    numbericBoxPage.Value = totalBoxpage;
                    ShowBoxPage(totalBoxpage);
                }
                else
                {
                    numbericBoxPage.DG_Maximum = 1;
                    numbericBoxPage.Value = 1;
                }
            }
        }

        public void LablPhonBoxClick(labelBox label)
        {
            int LabelNum = 0;
            try { LabelNum = int.Parse(label.Text); }
            catch (Exception) { return; }

            if (label == null || label.IsDisposed || int.Parse(label.Text) <= 0) return;
            if (listPhoneBoxs == null || listPhoneBoxs.Count == 0) return;
            Point screenPoint = label.PointToScreen(new Point(label.Width, label.Height));

            var ContextStrip = new MyContextMenu();
            ContextStrip.ShowImageMargin = true;
            ContextStrip.DG_Padding = 40;

            var toolStripBoxNum = new MyToolStripMenuItem("<svg xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 448 512\"><!--! Font Awesome Pro 7.0.0 by @fontawesome - https://fontawesome.com License - https://fontawesome.com/license (Commercial License) Copyright 2025 Fonticons, Inc. --><path fill=\"currentColor\" d=\"M416 416c0 17.7-14.3 32-32 32L64 448c-17.7 0-32-14.3-32-32l0-256 384 0 0 256zM39.3 128L86.9 61.4c6-8.4 15.7-13.4 26-13.4l95.1 0 0 80-168.7 0zM240 128l0-80 95.1 0c10.3 0 20 5 26 13.4L408.7 128 240 128zM11.9 111.3C4.2 122.2 0 135.2 0 148.5L0 416c0 35.3 28.7 64 64 64l320 0c35.3 0 64-28.7 64-64l0-267.5c0-13.3-4.2-26.3-11.9-37.2L387.1 42.8C375.1 26 355.7 16 335.1 16L112.9 16C92.3 16 72.9 26 60.9 42.8L11.9 111.3z\"/></svg>");
            toolStripBoxNum.Text = $"Box {label.Text}";

            int total = PhoneRepository.LoadAll().Count(c => c.PhoneBoxId == LabelNum);
            var toolStripTotalPhone = new MyToolStripMenuItem("<svg xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 448 512\"><!--! Font Awesome Pro 7.0.0 by @fontawesome - https://fontawesome.com License - https://fontawesome.com/license (Commercial License) Copyright 2025 Fonticons, Inc. --><path fill=\"currentColor\" d=\"M48 64c-8.8 0-16 7.2-16 16l0 96c0 8.8 7.2 16 16 16l96 0c8.8 0 16-7.2 16-16l0-96c0-8.8-7.2-16-16-16L48 64zM0 80C0 53.5 21.5 32 48 32l96 0c26.5 0 48 21.5 48 48l0 96c0 26.5-21.5 48-48 48l-96 0c-26.5 0-48-21.5-48-48L0 80zM48 320c-8.8 0-16 7.2-16 16l0 96c0 8.8 7.2 16 16 16l96 0c8.8 0 16-7.2 16-16l0-96c0-8.8-7.2-16-16-16l-96 0zM0 336c0-26.5 21.5-48 48-48l96 0c26.5 0 48 21.5 48 48l0 96c0 26.5-21.5 48-48 48l-96 0c-26.5 0-48-21.5-48-48l0-96zM400 64l-96 0c-8.8 0-16 7.2-16 16l0 96c0 8.8 7.2 16 16 16l96 0c8.8 0 16-7.2 16-16l0-96c0-8.8-7.2-16-16-16zM304 32l96 0c26.5 0 48 21.5 48 48l0 96c0 26.5-21.5 48-48 48l-96 0c-26.5 0-48-21.5-48-48l0-96c0-26.5 21.5-48 48-48zm0 288c-8.8 0-16 7.2-16 16l0 96c0 8.8 7.2 16 16 16l96 0c8.8 0 16-7.2 16-16l0-96c0-8.8-7.2-16-16-16l-96 0zm-48 16c0-26.5 21.5-48 48-48l96 0c26.5 0 48 21.5 48 48l0 96c0 26.5-21.5 48-48 48l-96 0c-26.5 0-48-21.5-48-48l0-96z\"/></svg>");
            toolStripTotalPhone.Text = $"Total Phones ({total})";

            var toolStripShowPhoneTag = new MyToolStripMenuItem("<svg xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 576 512\"><!--! Font Awesome Pro 7.0.0 by @fontawesome - https://fontawesome.com License - https://fontawesome.com/license (Commercial License) Copyright 2025 Fonticons, Inc. --><path fill=\"currentColor\" d=\"M64.3 96c0-17.7 14.3-32 32-32l136.8 0c6.4 0 12.5 2.5 17 7L404.5 225.4c12.5 12.5 12.5 32.8 0 45.3L271 404.1c-12.5 12.5-32.8 12.5-45.3 0L71.4 249.8c-4.5-4.5-7-10.6-7-17L64.3 96zm-32 0l0 136.8c0 14.9 5.9 29.1 16.4 39.6L203.1 426.7c25 25 65.5 25 90.5 0L427.1 293.3c25-25 25-65.5 0-90.5L272.7 48.4C262.2 37.9 248 32 233.1 32L96.3 32c-35.3 0-64 28.7-64 64zM381.1 52.6c-6.3 6.2-6.4 16.3-.2 22.6L534.8 231.1c9.2 9.4 9.2 24.4 0 33.7l-162 163.9c-6.2 6.3-6.2 16.4 .1 22.6s16.4 6.2 22.6-.1l162-163.9c21.5-21.8 21.6-56.9 0-78.7L403.7 52.8c-6.2-6.3-16.3-6.4-22.6-.2zM144.3 168a24 24 0 1 0 0-48 24 24 0 1 0 0 48z\"/></svg>");
            toolStripShowPhoneTag.Text = "Show Phone Tags";
            toolStripShowPhoneTag.Click += (s, e) =>
            {
                if (IsUseBox)
                {
                    phoneModeSeleted = PhoneMode.ALL;
                    BoxNumberSelected = LabelNum;
                    labelKeepALL.DrawLineBaseOnClick();
                    ShowListPhone();
                }
            };

            var toolStripadd = new MyToolStripMenuItem("<svg xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 448 512\"><!--! Font Awesome Pro 7.0.0 by @fontawesome - https://fontawesome.com License - https://fontawesome.com/license (Commercial License) Copyright 2025 Fonticons, Inc. --><path fill=\"currentColor\" d=\"M240 48c0-8.8-7.2-16-16-16s-16 7.2-16 16l0 192-192 0c-8.8 0-16 7.2-16 16s7.2 16 16 16l192 0 0 192c0 8.8 7.2 16 16 16s16-7.2 16-16l0-192 192 0c8.8 0 16-7.2 16-16s-7.2-16-16-16l-192 0 0-192z\"/></svg>");
            toolStripadd.Text = "Add New Box";
            toolStripadd.Click += (s, e) =>
            {
                if (IsUseBox)
                {
                    listPhoneBoxs.Add(PhoneBoxRepository.AddNew());
                    LoadBoxLabel();
                }
            };

            var toolStripEdit = new MyToolStripMenuItem("<svg xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 512 512\"><!--! Font Awesome Pro 7.0.0 by @fontawesome - https://fontawesome.com License - https://fontawesome.com/license (Commercial License) Copyright 2025 Fonticons, Inc. --><path fill=\"currentColor\" d=\"M454.6 45.3l12.1 12.1c12.5 12.5 12.5 32.8 0 45.3L440 129.4 382.6 72 409.4 45.3c12.5-12.5 32.8-12.5 45.3 0zM189 265.6l171-170.9 57.4 57.4-171 171c-4.2 4.2-9.6 7.2-15.4 8.6l-65.6 15.1 15.1-65.6c1.3-5.8 4.3-11.2 8.6-15.4zM386.7 22.6L166.4 243c-8.5 8.5-14.4 19.2-17.1 30.9l-20.9 90.6c-1.2 5.4 .4 11 4.3 14.9s9.5 5.5 14.9 4.3l90.6-20.9c11.7-2.7 22.4-8.6 30.9-17.1L489.4 125.3c25-25 25-65.5 0-90.5L477.3 22.6c-25-25-65.5-25-90.5 0zM80 64C35.8 64 0 99.8 0 144L0 432c0 44.2 35.8 80 80 80l288 0c44.2 0 80-35.8 80-80l0-128c0-8.8-7.2-16-16-16s-16 7.2-16 16l0 128c0 26.5-21.5 48-48 48L80 480c-26.5 0-48-21.5-48-48l0-288c0-26.5 21.5-48 48-48l128 0c8.8 0 16-7.2 16-16s-7.2-16-16-16L80 64z\"/></svg>");
            toolStripEdit.Text = "Renumber Box IDs";
            toolStripEdit.Click += (s, e) =>
            {
                if (IsUseBox)
                {
                    PhoneBoxRepository.ReorderPhoneBoxes();
                    DeviceManager.Instance.ReNameBoxId_PanelDeviceUpdatePhone();
                    LoadPhoneLabel();
                    LoadBoxLabel();
                }
            };

            var toolStripRemove = new MyToolStripMenuItem("<svg xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 448 512\"><!--! Font Awesome Pro 7.0.0 by @fontawesome - https://fontawesome.com License - https://fontawesome.com/license (Commercial License) Copyright 2025 Fonticons, Inc. --><path fill=\"currentColor\" d=\"M160.5 27.4c2-6.8 8.3-11.4 15.3-11.4l96.4 0c7.1 0 13.3 4.6 15.3 11.4l11 36.6-149 0 11-36.6zM116.1 64L16 64C7.2 64 0 71.2 0 80S7.2 96 16 96l416 0c8.8 0 16-7.2 16-16s-7.2-16-16-16l-100.1 0-13.7-45.8C312.1-2.1 293.4-16 272.2-16l-96.4 0c-21.2 0-39.9 13.9-46 34.2L116.1 64zM32 144l0 304c0 35.3 28.7 64 64 64l256 0c35.3 0 64-28.7 64-64l0-304-32 0 0 304c0 17.7-14.3 32-32 32L96 480c-17.7 0-32-14.3-32-32l0-304-32 0zm112 64c0-8.8-7.2-16-16-16s-16 7.2-16 16l0 192c0 8.8 7.2 16 16 16s16-7.2 16-16l0-192zm96 0c0-8.8-7.2-16-16-16s-16 7.2-16 16l0 192c0 8.8 7.2 16 16 16s16-7.2 16-16l0-192zm96 0c0-8.8-7.2-16-16-16s-16 7.2-16 16l0 192c0 8.8 7.2 16 16 16s16-7.2 16-16l0-192z\"/></svg>");
            toolStripRemove.Text = "Remove this Box";
            toolStripRemove.Click += (s, e) =>
            {
                if (IsUseBox)
                {
                    var boxId = LabelNum;
                    var box = listPhoneBoxs.FirstOrDefault(c => c.Id == boxId);
                    if (box != null)
                    {
                        PhoneBoxRepository.Delete(boxId);
                        listPhoneBoxs.Remove(box);
                        LoadBoxLabel();
                    }
                }
            };

            ContextStrip.Items.Add(toolStripBoxNum);
            ContextStrip.Items.Add(toolStripTotalPhone);
            ContextStrip.Items.Add(toolStripShowPhoneTag);
            ContextStrip.Items.Add(toolStripadd);
            ContextStrip.Items.Add(toolStripEdit);
            ContextStrip.Items.Add(toolStripRemove);
            ContextStrip.Renderer = new ContextMenu_Renderer();

            ContextStrip.Show(screenPoint);
        }

        public void ShowBoxPage(int pageNumber)
        {
            foreach (labelBox boxLabel in tableLayoutPhoneBox.Controls)
            {
                var index = tableLayoutPhoneBox.Controls.IndexOf(boxLabel);
                tableLayoutPhoneBox.Controls.RemoveAt(index);
                boxLabel.Dispose();
            }
            tableLayoutPhoneBox.Controls.Clear();

            var listBoxs = listPhoneBoxs;
            if (listBoxs == null || listBoxs.Count() == 0) return;
            var listLabelBox = allPhoneBoxLabels.FindAll(c => listBoxs.Select(p => p.Id).ToList().Contains(c));
            if (listLabelBox == null || listLabelBox.Count() == 0) return;

            labelPhoneBox.Text = $"Phone Boxs ({listLabelBox.Count})";

            int totalItems = listLabelBox.Count;
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            if (pageNumber < 1 || pageNumber > totalPages)
                return;

            int start = (pageNumber - 1) * pageSize;
            int end = Math.Min(start + pageSize, totalItems);
            var list = new List<labelBox>();
            for (int i = start; i < end; i++)
            {
                var phoneNum = listLabelBox[i];
                var boxLabel = getBoxLabel(phoneNum);
                list.Add(boxLabel);
            }

            tableLayoutPhoneBox.Controls.AddRange(list.ToArray());
            tableLayoutPhoneBox.Refresh();
            currentBoxPage = pageNumber;
        }

        private void numbericBoxPage_Click(object sender, EventArgs e)
        {
            ShowBoxPage((int)numbericBoxPage.Value);
        }

        private void numbericBoxPage_KeyUp(object sender, KeyEventArgs e)
        {
            if (numbericBoxPage.Value <= numbericBoxPage.DG_Maximum)
            {
                ShowBoxPage((int)numbericBoxPage.Value);
            }
        }
        #endregion

        private void buttonMobileRotate_Click(object sender, EventArgs e)
        {

        }

        private void buttomSocialNetwork_Click(object sender, EventArgs e)
        {
            FormSocicalNetwork formSocicalNetwork = new FormSocicalNetwork();
            formSocicalNetwork.StartPosition = FormStartPosition.CenterScreen;
            formSocicalNetwork.ShowDialog();
        }
    }
}