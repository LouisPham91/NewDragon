using AdvancedSharpAdbClient.Models;
using Dragon.ControlHelper.ScrcpyNet.DesignDevice;
using Dragon.ControlHelper.ScrcpyNet.Services;
using Dragon.Database.Models;
using Dragon.DesignView.Public.NormalMode;
using System.Diagnostics;
using System.Drawing.Drawing2D;

namespace Dragon.ControlHelper.UIController
{
    public class VirtualFlowHelper : IDisposable
    {
        // SỬA 1: đổi kiểu từ PanelDevice sang DoubleBufferedFlowLayoutPanel
        public DoubleBufferedFlowLayoutPanel flowLayoutPanel { get; }
        private readonly DoubleBufferedFlowLayoutPanel _panel;

        private System.Windows.Forms.Timer wheelTimer;
        public int DebounceDelay { get; set; } = 100;

        private bool isSelecting = false;
        private Point selectionStart;
        private Rectangle selectionRect = Rectangle.Empty;

        public Color SelectionBorderColor { get; set; } = Color.FromArgb(0, 173, 232);
        public Color SelectionFillColor { get; set; } = Color.FromArgb(50, Color.FromArgb(0, 173, 232));
        public float SelectionBorderWidth { get; set; } = 2f;
        public float[] SelectionDashPattern { get; set; } = new float[] { 4, 4 };

        public VirtualFlowHelper(DoubleBufferedFlowLayoutPanel panel)
        {
            _panel = panel ?? throw new ArgumentNullException(nameof(panel));
            flowLayoutPanel = _panel;

            _panel.MouseDown += Panel_MouseDown;
            _panel.MouseMove += Panel_MouseMove;
            _panel.MouseUp += Panel_MouseUp;
            _panel.Paint += Panel_Paint;

            wheelTimer = new System.Windows.Forms.Timer();
            wheelTimer.Interval = 50;
            wheelTimer.Tick += WheelTimer_Tick;
            _panel.MouseWheel += DoubleBufferedFlowLayoutPanel_MouseWheel;
            _panel.Scroll += Panel_Scroll;
        }

        public void StartDevice(List<(Phone Phone, DeviceData Data)> listPhoneWorks, List<PanelDevice> allDevicePanels, int maxSize, int bitrate, int fps, int minSize)
        {
            if (_panel == null || _panel.IsDisposed) return;

            foreach (var (phone, data) in listPhoneWorks)
            {
                Task.Run(async () =>
                {
                    bool exists = DeviceManager.Instance.DeviceDict.ContainsKey(phone.DeviceID);
                    if (!exists)
                    {
                        Debug.WriteLine($"Start Phone {phone.PhoneTagNumber}, Mode: {phone.PhoneMode}, DeviceID: {phone.DeviceID}, Serial: {phone.Serial}");
                        // chạy ở thread hiện tại, không ép lên UI
                        await DeviceManager.Instance.StartPanelDevice(data, phone, maxSize, bitrate, fps, minSize).ConfigureAwait(false);
                    }
                    else
                    {
                        var panelDevice = allDevicePanels.FirstOrDefault(c => c.DeviceID == phone.DeviceID);
                        _panel.Controls.Add(panelDevice);
                    }
                });
            }
        }

        private void Panel_Scroll(object? sender, ScrollEventArgs e)
        {
            wheelTimer.Stop();
            wheelTimer.Start();
        }

        private void WheelTimer_Tick(object? sender, EventArgs e)
        {
            wheelTimer.Stop();
            // SỬA 2: bỏ gọi trùng 2 lần
            HandleScrollFinished();
        }

        private void DoubleBufferedFlowLayoutPanel_MouseWheel(object? sender, MouseEventArgs e)
        {
            wheelTimer.Stop();
            wheelTimer.Start();
        }

        public void HandleScrollFinished()
        {
            var visibleArea = new Rectangle(
                -_panel.AutoScrollPosition.X,
                -_panel.AutoScrollPosition.Y,
                _panel.ClientSize.Width + 400,
                _panel.ClientSize.Height + 400);

            var visibleControls = _panel.Controls.OfType<PanelDevice>()
                .Where(c => visibleArea.IntersectsWith(c.Bounds)).ToList();
        }

        public void RunInvoke(Action action)
        {
            if (_panel.IsDisposed) return;
            if (_panel.InvokeRequired)
                _panel.BeginInvoke(action);
            else
                action();
        }

        public void AddOne(PanelDevice panelDevice) => _panel.Controls.Add(panelDevice);
        public bool CheckExitingDevicePanel(PanelDevice panelDevice) => _panel.Controls.Contains(panelDevice);
        public void RemoveOnePanelDevice(PanelDevice panelDevice) => _panel.Controls.Remove(panelDevice);

        public void RemoveAllPhoneControl(List<Phone> phones)
        {
            foreach (var phone in phones)
            {
                var panelDevice = FindOneDevicePanelByPhone(phone);
                if (panelDevice != null) _panel.Controls.Remove(panelDevice);
            }
        }

        public void RemoveAllDevicePanel()
        {
            if (_panel == null) return;
            var all = _panel.Controls.OfType<PanelDevice>().ToArray();
            foreach (var item in all) _panel.Controls.Remove(item);

        }

        public void RemovePanelDevicesSelected(List<PanelDevice> panelDevices, bool IsRemoveInDictionary)
        {
            foreach (var devicePanel in panelDevices)
            {
                if (_panel.Controls.Contains(devicePanel))
                    _panel.Controls.Remove(devicePanel);

                if (IsRemoveInDictionary && devicePanel.phone != null && !string.IsNullOrEmpty(devicePanel.phone.DeviceID))
                    DeviceManager.Instance.DeleteDevicePanel(devicePanel.phone.DeviceID);
            }
        }


        public List<Phone> GetListPhoneFormSeletect()
        {
            var phones = new List<Phone>();
            if (_panel == null) return phones;
            // SỬA 4: PanelDevice dùng _IsSelected, không phải IsSelected
            phones = _panel.Controls.OfType<PanelDevice>()
                .Where(c => c._IsSelected && c.phone != null)
                .Select(c => c.phone!).ToList();
            return phones;
        }

        public List<PanelDevice> GetListPanelDeviceFormSeletect()
        {
            if (_panel == null) return new List<PanelDevice>();
            return _panel.Controls.OfType<PanelDevice>()
                .Where(c => c._IsSelected).ToList();
        }

        public PanelDevice? FindOneDevicePanelByPhone(Phone phone)
        {
            return _panel.Controls.OfType<PanelDevice>()
                .FirstOrDefault(c => c.phone != null && c.phone.Id == phone.Id);
        }

        public PanelDevice? FindOneDevicePanelByDeviceID(string deviceID)
        {
            return _panel.Controls.OfType<PanelDevice>()
                .FirstOrDefault(c => c.phone != null && c.phone.DeviceID == deviceID);
        }

        #region Region Selection
        private void Panel_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isSelecting = true;
                selectionStart = e.Location;
                selectionRect = new Rectangle(e.Location, Size.Empty);
            }
        }

        private void Panel_MouseMove(object? sender, MouseEventArgs e)
        {
            if (isSelecting)
            {
                selectionRect = GetNormalizedRectangle(selectionStart, e.Location);
                _panel.Invalidate();
            }
        }

        public void Panel_MouseUp(object? sender, MouseEventArgs e)
        {
            if (!isSelecting) return;
            isSelecting = false;
            selectionRect = GetNormalizedRectangle(selectionStart, e.Location);

            var allDevicePanels = _panel.Controls.OfType<PanelDevice>().ToArray();
            var selected = allDevicePanels.Where(c => selectionRect.IntersectsWith(c.Bounds)).ToArray();
            var notSelected = allDevicePanels.Except(selected).ToArray();

            _panel.SuspendLayout();

            if (selected.Length == 0)
            {
                PDSyncDongBo.Instance.IsDongBo = false;
                var form = FormManager.Instance.formDeviceControl;
                var panel = form?.panel1.Controls.OfType<PanelDevice>().FirstOrDefault();
                panel?.EventDongBo(false);
                if (form != null) form.labelStatusMode.Text = "Individual Device Mode";
            }
            else
            {
                PDSyncDongBo.Instance.IsDongBo = true;
                foreach (var dev in selected) dev.EventDongBo(true);
                var form = FormManager.Instance.formDeviceControl;
                var panel = form?.panel1.Controls.OfType<PanelDevice>().FirstOrDefault();
                if (panel != null)
                {
                    panel.EventDongBo(true);
                    if (form != null) form.labelStatusMode.Text = "Multi Device Mode";
                }
            }

            foreach (var dev in notSelected) dev.EventDongBo(false);

            _panel.ResumeLayout();
            _panel.Invalidate();
        }

        private void Panel_Paint(object? sender, PaintEventArgs e)
        {
            if (isSelecting && selectionRect != Rectangle.Empty)
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using var fillBrush = new SolidBrush(SelectionFillColor);
                g.FillRectangle(fillBrush, selectionRect);
                using var borderPen = new Pen(SelectionBorderColor, SelectionBorderWidth);
                borderPen.DashPattern = SelectionDashPattern;
                g.DrawRectangle(borderPen, selectionRect);
            }
        }

        private Rectangle GetNormalizedRectangle(Point p1, Point p2)
        {
            return new Rectangle(
                Math.Min(p1.X, p2.X),
                Math.Min(p1.Y, p2.Y),
                Math.Abs(p1.X - p2.X),
                Math.Abs(p1.Y - p2.Y));
        }
        #endregion

        public void Dispose()
        {
            wheelTimer?.Stop();
            wheelTimer?.Dispose();
        }
    }
}
