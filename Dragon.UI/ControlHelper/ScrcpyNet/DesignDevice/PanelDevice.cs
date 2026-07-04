using Dragon.ControlHelper.ScrcpyNet.Services;
using Dragon.ControlHelper.UIController;
using Dragon.Controller.Database.Services;
using Dragon.Controller.GlobalControl.Property;
using Dragon.Database.Models;
using Dragon.DesignView.FormUI;
using Dragon.DesignView.Private;


namespace Dragon.ControlHelper.ScrcpyNet.DesignDevice
{
    public partial class PanelDevice : PanelBorderDevice, IPanelDeviceUI, IDisposable
    {
        // ==== PanelDevice ====
        public string DeviceID { get; }

        public LabelGradientDevice? _usbLabel;
        private UserPhoneStatus? userPhoneStatus = null;

        private bool _disposed;
        private bool _isDragging;
        //private bool _isFlowDrag;
        public bool _IsSelected { get; private set; }


        // ==== IPanelDeviceUI ====

        public IPDController? _PDcontroller;
        public IntPtr RenderHandle => (renderPanel != null) ? renderPanel.Handle : IntPtr.Zero;
        public (int Width, int Height) RenderPanelSize => (renderPanel != null) ? (renderPanel.ClientSize.Width, renderPanel.ClientSize.Height) : (-1, -1);
        public Phone phone => PhoneRepository.FindOneByDeviceID(DeviceID) ?? throw new InvalidOperationException($"Phone {DeviceID} not found");

        // ==== InputModeManager ====
        private InputModeManager? _inputManager;




        public PanelDevice(string deviceID)
        {
            DeviceID = deviceID;
            Name = deviceID;
            InitSize();
            InitMouse();
            AddOrUpdatePhoneStatus(PhoneStatus.Loading);
            CreateRenderPanel();
        }

        #region ------------ PanelDevice => Init Setting , Setup (this panel only) ------------
        public void InitSize()
        {
            var result = GetSettings.GetALLSize(phone);
            int pad = BorderThickness * 2;
            this.Size = new Size(result.min.w + pad, result.min.h + pad);
            ChangeColorParent();
        }

        private void InitMouse()
        {
            Cursor = Cursors.Hand;

        }

        public void ChangeColorALLControls()
        {
            if (_usbLabel != null)
                _usbLabel.ChangeColor();
            ChangeColorParent();
        }
        #endregion
        #region  ------------ PanelDevice =>  Event Hỗ Trợ UI -------------
        public void InvokeOnUI(Action action)
        {
            if (!IsDisposed && IsHandleCreated)
                BeginInvoke(action);
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (renderPanel == null) return;
            _PDcontroller?.SetRenderPanelSize(renderPanel.Width, renderPanel.Height);
        }
        #endregion
        #region  ------------ PanelDevice =>  Sycn Đồng Bộ Điều Khiển -------------
        // Hỗ Trợ => ScrcpyService
        private void ToggleSelection()
        {
            _IsSelected = !_IsSelected;
            if (_IsSelected)
            {
                UseGradientBorder = true;
            }
            else
            {
                UseGradientBorder = false;
            }
            ChangeColorALLControls();
            Invalidate();

            // Gửi Cho ScrcpyService Thông Qua Interface IPDController
            _PDcontroller?.SetSycnDongBo(_IsSelected);
        }
        // Hỗ Trợ => ScrcpyService
        public void EventDongBo(bool isTrue = false)
        {
            _IsSelected = isTrue;
            if (isTrue)
            {
                UseGradientBorder = true;
            }
            else
            {
                UseGradientBorder = false;
                BorderColor = Color.FromArgb(108, 108, 108);
            }
            ChangeColorALLControls();
            Invalidate();

            // Gửi Cho ScrcpyService Thông Qua Interface IPDController
            _PDcontroller?.SetSycnDongBo(isTrue);
        }

        #endregion

        #region  -----------   IPanelDeviceUI  ---------------

        public void SetIPDController(IPDController controller)
        {
            _PDcontroller = controller ?? throw new ArgumentNullException(nameof(controller));
        }

        public void ResizeVideoMaxSize()
        {
            _PDcontroller?.ResizeVideoMaxSize();
        }

        public void ResizePDUI(int Width, int Height)
        {

            if (Parent is FlowLayoutPanel flowLayout)
            {

                var allSize = GetSettings.GetALLSize(phone);
                if (Width > Height)
                    Size = new Size(allSize.min.h + BorderThickness, allSize.min.w + BorderThickness);
                else
                    Size = new Size(allSize.min.w + BorderThickness, allSize.min.h + BorderThickness);

            }
            else if (Parent is Panel panel && panel.Name == "panel1" && panel.Parent is FormDeviceControl form)
            {

                form.Size = new Size(Width + 10 + 190, Height);
                panel.Size = new Size(Width + 5, Height);
                Size = new Size(Width, Height);

            }


        }
        public void ScrcpySizeChanged()
        {

            int newW, newH;

            if (Parent is FlowLayoutPanel flowLayout)
            {
                newW = Height;
                newH = Width;
                Size = new Size(newW, newH);
            }
            else if (Parent is Panel panel && panel.Name == "panel1" && panel.Parent is FormDeviceControl form)
            {

                newW = Height;
                newH = Width;
                form.Size = new Size(newW + 10 + 190, newH);
                panel.Size = new Size(newW, newH + 5);

                Size = new Size(newW, newH);
                Location = Point.Empty;
            }
        }

        public bool IsCanNotRotate()
        {
            return (Parent is Panel panel && panel.Name == "panelPhone" && panel.Parent is FormAutoRecord);
        }

        public void OnScrcpyServiceLost()
        {
            _inputManager?.Dispose();
            _inputManager = null;
            MouseDown -= OnPanelMouseDown;
            MouseMove -= OnPanelMouseMove;
            MouseUp -= OnPanelMouseUp;
            DpiChangedAfterParent -= PanelDevice_DpiChangedAfterParent;
        }
        public async Task OnSwitchInputMode()
        {

            if (InvokeRequired)
            {
                await Task.Run(() => Invoke(new Func<Task>(OnSwitchInputMode)));
                return;
            }
            var svc = _PDcontroller?.ScrcpyService;
            if (svc == null || renderPanel == null)
                return;

            _inputManager?.Dispose();
            _inputManager = new InputModeManager(renderPanel, svc, DeviceID);
            await _inputManager.SwitchModeAsync(phone, useRealMouse: false);
        }

        public void AddOrUpdatePhoneStatus(PhoneStatus phoneStatus)
        {
            // 1. Đảm bảo UI thread – chỉ 1 lần duy nhất
            if (InvokeRequired)
            {
                BeginInvoke(new Action<PhoneStatus>(AddOrUpdatePhoneStatus), phoneStatus);
                return;
            }

            if (phoneStatus == PhoneStatus.Loading || phoneStatus == PhoneStatus.Disconnect || phoneStatus == PhoneStatus.Resizing)
            {
                SuspendLayout();

                if (renderPanel != null) renderPanel.Visible = false;

                if (userPhoneStatus != null)
                {
                    Controls.Remove(userPhoneStatus);
                    userPhoneStatus.Dispose();
                    userPhoneStatus = null;
                }

                var result = GetSettings.GetALLSize(phone);
                userPhoneStatus = new UserPhoneStatus(DeviceID, phoneStatus)
                {
                    Dock = DockStyle.Fill,
                };

                Controls.Add(userPhoneStatus);
                userPhoneStatus.BringToFront();

                // ép handle tạo ngay
                var _ = userPhoneStatus.Handle;

                ResumeLayout(true);

                // gọi trực tiếp, không BeginInvoke nữa
                userPhoneStatus.SetRegion(BorderRadius, BorderThickness);
            }
            else if (phoneStatus == PhoneStatus.Connecting)
            {
                if (renderPanel == null) return;

                SuspendLayout();

                renderPanel.Visible = true;
                renderPanel.BringToFront();   // đảm bảo ở trên

                AndUsbLabelchangeColor();
                InitMouseRender();

                // ẩn và dispose luôn – không cần delay
                if (userPhoneStatus != null)
                {
                    var old = userPhoneStatus;
                    userPhoneStatus = null;
                    old.Visible = false;
                    Controls.Remove(old);
                    old.Dispose();
                }

                ResumeLayout(true);
            }
        }

        public void CreateRenderPanel()
        {
            var result = GetSettings.GetALLSize(phone);
            renderPanel = new DoubleBufferedPanel();
            renderPanel.CreateControl();
            renderPanel.Size = new Size(result.min.w, result.min.h);
            renderPanel.TabStop = true;
            renderPanel.AllowDrop = true;
            renderPanel.Dock = DockStyle.Fill;
            renderPanel.SetRegion(BorderRadius, BorderThickness);
            renderPanel.Visible = false;
            this.Controls.Add(renderPanel);
            renderPanel.SendToBack();
        }

        // bắt buộc event create scrcpy thành công mới ra đây không thì sẽ phải create lại
        public void AndUsbLabelchangeColor()
        {
            AddUsbLabel();
            ChangeColorALLControls();
        }

        public void InitMouseRender()
        {
            if (renderPanel == null) return;
            renderPanel.MouseDown += (s, e) => renderPanel.Focus();
            renderPanel.MouseClick += (s, e) =>
            {
                if (e.Button == MouseButtons.Left && ModifierKeys.HasFlag(Keys.Control))
                    ToggleSelection();
            };

            MouseDown += OnPanelMouseDown;
            MouseMove += OnPanelMouseMove;
            MouseUp += OnPanelMouseUp;
            DpiChangedAfterParent += PanelDevice_DpiChangedAfterParent;
        }
        public void AddUsbLabel()
        {
            if (_usbLabel != null) { Controls.Remove(_usbLabel); _usbLabel.Dispose(); }
            _usbLabel = new LabelGradientDevice
            {
                Text = phone.PhoneMode.ToString(),
                Font = new Font("Arial", 7, FontStyle.Bold),
                Size = new Size(40, 15),
                Location = new Point(2, 2),
                TextAlign = ContentAlignment.MiddleCenter
            };
            _usbLabel.ChangeColor();
            Controls.Add(_usbLabel);
            _usbLabel.BringToFront();
        }
        #endregion


        #region  ------- Mouse Panel =>  Drag to FormDeviceControl ---------------
        private void PanelDevice_DpiChangedAfterParent(object? sender, EventArgs e)
        {

        }

        private void OnPanelMouseDown(object? s, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (Parent is FlowLayoutPanel)
                {
                    _isDragging = true;
                }
                else if (Parent is Panel p && p.Name == "panel1")
                {
                    _isDragging = false;
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (Parent is FlowLayoutPanel)
                    FormManager.Instance.Switch_Flow_To_DeviceControl(this);
            }
        }

        private void OnPanelMouseMove(object? s, MouseEventArgs e)
        {
            if (!_isDragging) return;
            var pos = MousePosition;
            Cursor = CursorHelper.HandClose;

            if (Parent is FlowLayoutPanel && FormManager.Instance.formDeviceControl?.Visible != true)
            {

                FormManager.Instance.Switch_Flow_To_DeviceControl(this, pos);
            }

            FormManager.Instance.DraggingformDeviceControl(pos);
        }

        private void OnPanelMouseUp(object? s, MouseEventArgs e)
        {
            if (!_isDragging) return;
            _isDragging = false;
            Cursor = Cursors.Hand;
            //var pos = MousePosition;

            //if (Parent is Panel p && p.Name == "panel1" && !_isDragging)
            //    FormManager.Instance.Switch_DeviceControl_To_Flow_OPEN(true);

            //_isFlowDrag = false;
        }


        #endregion


        protected override void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _usbLabel?.Dispose();
            }
            _disposed = true;
            base.Dispose(disposing);
        }


    }
}