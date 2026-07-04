using Dragon.ControlHelper.UIController;
using Dragon.Controller.Database.Services;
using Dragon.Controller.DeviceControl.ScrcpyNet.Services;
using Dragon.ControlHelper.ScrcpyNet.Services;
using System.Drawing;
using System.Windows.Forms;

namespace Dragon.ControlHelper.ScrcpyNet.EventHandle
{
    /// <summary>
    /// Xử lý chuột UHID chế độ capture (chuột thật, di chuyển tương đối).
    /// Được InputModeManager tạo khi chọn chế độ chuột thật.
    /// </summary>
    public sealed class MouseUhidEventHandles : IDisposable
    {
        public string DeviceID { get; }
        private readonly Control _hostControl;
        private readonly IScrcpyService _scrcpyService;
        private readonly ushort _uhidId = 2;               // SC_HID_ID_MOUSE
        private bool _disposed;
        private bool _isCaptured;
        private Point _lastMousePosition;
        private Rectangle _captureBounds;
        private const int SC_HID_MOUSE_INPUT_SIZE = 4;
        private const int MAX_DELTA = 127;
        private int PhysicalWidth = 0;
        private int PhysicalHeight = 0;

        public MouseUhidEventHandles(Control hostControl, IScrcpyService scrcpyService, string deviceID)
        {
            DeviceID = deviceID;
            _hostControl = hostControl ?? throw new ArgumentNullException(nameof(hostControl));
            _scrcpyService = scrcpyService ?? throw new ArgumentNullException(nameof(scrcpyService));
            var phone = PhoneRepository.FindOneByDeviceID(deviceID);
            if (phone == null) return;
            PhysicalWidth = phone.PhysicalWidth;
            PhysicalHeight = phone.PhysicalHeight;
            _hostControl.MouseDown += OnMouseDown;
            _hostControl.MouseUp += OnMouseUp;
            _hostControl.MouseMove += OnMouseMove;
            _hostControl.MouseWheel += OnMouseWheel;
            _hostControl.KeyDown += OnKeyDown;
            _hostControl.LostFocus += OnLostFocus;

            _ = StartUhidMouseAsync();
        }

        private async Task StartUhidMouseAsync()
        {
            await CreateMouseAsync();
        }

        private async Task CreateMouseAsync()
        {
            try
            {
                byte[] reportDesc = CreateMouseReportDescriptor();
                var msg = new UhidCreateControlMessage(_uhidId, 0, 0, "VirtualMouse", reportDesc);
                await _scrcpyService.SendSyncDongBoCommand(msg);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"MouseUhidEventHandlers: Failed to create mouse: {ex.Message}");
            }
        }

        private static byte[] CreateMouseReportDescriptor() => new byte[]
        {
            0x05, 0x01, 0x09, 0x02, 0xA1, 0x01, 0x09, 0x01, 0xA1, 0x00,
            0x05, 0x09, 0x19, 0x01, 0x29, 0x05, 0x15, 0x00, 0x25, 0x01,
            0x95, 0x05, 0x75, 0x01, 0x81, 0x02, 0x95, 0x01, 0x75, 0x03,
            0x81, 0x01, 0x05, 0x01, 0x09, 0x30, 0x09, 0x31, 0x09, 0x38,
            0x15, 0x81, 0x25, 0x7F, 0x75, 0x08, 0x95, 0x03, 0x81, 0x06,
            0xC0, 0xC0
        };

        // ----- Bắt / thả capture -----
        private void EnableMouseCapture()
        {
            _isCaptured = true;
            //_lastMousePosition = new Point(_hostControl.Width / 2, _hostControl.Height / 2);
            _lastMousePosition = new Point(PhysicalWidth / 2, PhysicalHeight / 2);

            Point center = _lastMousePosition;
            Cursor.Position = _hostControl.PointToScreen(center);

            _captureBounds = _hostControl.RectangleToScreen(_hostControl.ClientRectangle);
            Cursor.Clip = _captureBounds;
            Cursor.Hide();
        }

        private void DisableMouseCapture()
        {
            _isCaptured = false;
            Cursor.Clip = Rectangle.Empty;
            Cursor.Show();
        }

        // ----- Event handlers -----
        private void OnMouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && !_isCaptured)
                EnableMouseCapture();

            SendMouseClick(e.Button, true);
        }

        private void OnMouseUp(object? sender, MouseEventArgs e)
        {
            SendMouseClick(e.Button, false);
        }

        private void OnMouseMove(object? sender, MouseEventArgs e)
        {
            if (_isCaptured)
            {
                int dx = e.X - _lastMousePosition.X;
                int dy = e.Y - _lastMousePosition.Y;
                _lastMousePosition = e.Location;

                // Warp con trỏ về giữa panel để không bị kẹt biên
                //Point center = new Point(_hostControl.Width / 2, _hostControl.Height / 2);
                Point center = new Point(PhysicalWidth / 2, PhysicalHeight / 2);
                Cursor.Position = _hostControl.PointToScreen(center);
                _lastMousePosition = center;

                SendMouseMotion(dx, dy, e.Button);
            }
            else
            {
                _lastMousePosition = e.Location;
            }
        }

        private void OnMouseWheel(object? sender, MouseEventArgs e)
        {
            int wheel = e.Delta > 0 ? 1 : e.Delta < 0 ? -1 : 0;
            SendMouseScroll(wheel);
        }

        private void OnKeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape && _isCaptured)
                DisableMouseCapture();
        }

        private void OnLostFocus(object? sender, EventArgs e)
        {
            if (_isCaptured)
                DisableMouseCapture();
        }

        // ----- Gửi các loại input -----
        private byte GetButtonsState(MouseButtons buttons)
        {
            byte state = 0;
            if (buttons.HasFlag(MouseButtons.Left)) state |= 1 << 0;
            if (buttons.HasFlag(MouseButtons.Right)) state |= 1 << 1;
            if (buttons.HasFlag(MouseButtons.Middle)) state |= 1 << 2;
            return state;
        }

        private void SendMouseClick(MouseButtons button, bool down)
        {
            byte buttonsState = down ? GetButtonsState(button) : (byte)0;
            byte[] data = new byte[SC_HID_MOUSE_INPUT_SIZE];
            data[0] = buttonsState;
            _ = _scrcpyService.SendSyncDongBoCommand(new UhidInputControlMessage(_uhidId, data));
        }

        private void SendMouseMotion(int dx, int dy, MouseButtons buttons)
        {
            byte buttonsState = GetButtonsState(buttons);
            byte[] data = new byte[SC_HID_MOUSE_INPUT_SIZE];
            data[0] = buttonsState;
            data[1] = (byte)Math.Clamp(dx, -MAX_DELTA, MAX_DELTA);
            data[2] = (byte)Math.Clamp(dy, -MAX_DELTA, MAX_DELTA);
            _ = _scrcpyService.SendSyncDongBoCommand(new UhidInputControlMessage(_uhidId, data));
        }

        private void SendMouseScroll(int wheel)
        {
            byte[] data = new byte[SC_HID_MOUSE_INPUT_SIZE];
            data[3] = (byte)Math.Clamp(wheel, -MAX_DELTA, MAX_DELTA);
            _ = _scrcpyService.SendSyncDongBoCommand(new UhidInputControlMessage(_uhidId, data));
        }



        // ----- Dọn dẹp -----
        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            DisableMouseCapture();

            _hostControl.MouseDown -= OnMouseDown;
            _hostControl.MouseUp -= OnMouseUp;
            _hostControl.MouseMove -= OnMouseMove;
            _hostControl.MouseWheel -= OnMouseWheel;
            _hostControl.KeyDown -= OnKeyDown;
            _hostControl.LostFocus -= OnLostFocus;

            try
            {
                var msg = new UhidDestroyControlMessage(_uhidId);
                _ = _scrcpyService.SendSyncDongBoCommand(msg);
            }
            catch { }
        }
    }
}