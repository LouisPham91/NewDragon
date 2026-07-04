using Dragon.Controller.DeviceControl.OTG;
using LibUsbDotNet.LibUsb;
using LibUsbDotNet.Main;
using System.Diagnostics;
using System.Drawing;

namespace Dragon.Controller.DeviceControl.OTG
{

    /// <summary>
    /// Quản lý AOA HID (Mouse + Keyboard) dùng chung 1 IUsbDevice
    /// Đã tách khỏi AoaDeviceHelper - nhận USB từ Session
    /// </summary>
    public class AoaController : IDisposable
    {
        private readonly AoaDevice _device;
        private IUsbDevice? _d;
        private bool _disposed;
        private bool _initialized;

        private const int MOUSE_ID = 2;
        private const int KEYBOARD_ID = 1;

        private Point _currentMousePos = Point.Empty;
        private const int MAX_DELTA = 127;
        private const int HID_MOUSE_REPORT_SIZE = 4;

        private static readonly byte[] MouseDesc = new byte[]
        {
            0x05, 0x01, 0x09, 0x02, 0xA1, 0x01, 0x09, 0x01, 0xA1, 0x00,
            0x05, 0x09, 0x19, 0x01, 0x29, 0x03, 0x15, 0x00, 0x25, 0x01,
            0x95, 0x03, 0x75, 0x01, 0x81, 0x02, 0x95, 0x01, 0x75, 0x05,
            0x81, 0x03, 0x05, 0x01, 0x09, 0x30, 0x09, 0x31, 0x15, 0x81,
            0x25, 0x7F, 0x75, 0x08, 0x95, 0x02, 0x81, 0x06, 0xC0, 0xC0
        };

        private static readonly byte[] KeyboardDesc = new byte[]
        {
            0x05, 0x01, 0x09, 0x06, 0xA1, 0x01, 0x05, 0x07, 0x19, 0xE0,
            0x29, 0xE7, 0x15, 0x00, 0x25, 0x01, 0x75, 0x01, 0x95, 0x08,
            0x81, 0x02, 0x95, 0x01, 0x75, 0x08, 0x81, 0x03, 0x95, 0x05,
            0x75, 0x01, 0x05, 0x08, 0x19, 0x01, 0x29, 0x05, 0x91, 0x02,
            0x95, 0x01, 0x75, 0x03, 0x91, 0x03, 0x95, 0x06, 0x75, 0x08,
            0x15, 0x00, 0x25, 0x65, 0x05, 0x07, 0x19, 0x00, 0x29, 0x65,
            0x81, 0x00, 0xC0
        };

        public bool IsMouseRegistered { get; private set; }
        public bool IsKeyboardRegistered { get; private set; }

        public AoaController(AoaDevice device)
        {
            _device = device ?? throw new ArgumentNullException(nameof(device));
        }

        // ============================================================
        // INITIALIZE - NHẬN USB TỪ SESSION
        // ============================================================

        public bool Initialize(IUsbDevice usb, bool registerMouse = true, bool registerKeyboard = true)
        {
            if (_initialized) return true;

            try
            {
                _d = usb ?? throw new ArgumentNullException(nameof(usb));
                if (!_d.IsOpen) _d.Open();

                if (registerMouse)
                {
                    RegisterHid(MOUSE_ID, MouseDesc);
                    IsMouseRegistered = true;
                    _currentMousePos = Point.Empty;
                    Debug.WriteLine("[AoaHid] ✅ Mouse registered");
                }

                if (registerKeyboard)
                {
                    RegisterHid(KEYBOARD_ID, KeyboardDesc);
                    IsKeyboardRegistered = true;
                    Debug.WriteLine("[AoaHid] ✅ Keyboard registered");
                }

                _initialized = true;
                Debug.WriteLine("[AoaHid] ✅ All registered!");
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[AoaHid] Init: {ex.Message}");
                return false;
            }
        }


        // ============================================================
        // HID REGISTER
        // ============================================================

        private void RegisterHid(int id, byte[] desc)
        {
            if (_d == null) return;

            _d.ControlTransfer(new UsbSetupPacket(0x40, 54, (short)id, (short)desc.Length, 0));
            _d.ControlTransfer(new UsbSetupPacket(0x40, 56, (short)id, 0, (short)desc.Length), desc, 0, desc.Length);
        }

        private void SendHidReport(int id, byte[] report)
        {
            if (_d == null) return;
            try
            {
                _d.ControlTransfer(new UsbSetupPacket(0x40, 57, (short)id, 0, (short)report.Length), report, 0, report.Length);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[AoaHid] Send error: {ex.Message}");
            }
        }
        private async Task SendHidReportAsync(int id, byte[] report)
        {
            if (_d == null) return;
            try
            {
                await _d.ControlTransferAsync(new UsbSetupPacket(0x40, 57, (short)id, 0, (short)report.Length), report, 0, report.Length);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[AoaHid] SendAsync error: {ex.Message}");
            }
        }
        // ============================================================
        // MOUSE
        // ============================================================

        public void MoveToOrigin(int steps = 50, int delayMs = 10)
        {
            byte[] moveData = new byte[HID_MOUSE_REPORT_SIZE];
            moveData[1] = unchecked((byte)-MAX_DELTA);
            moveData[2] = unchecked((byte)-MAX_DELTA);

            for (int i = 0; i < steps; i++)
            {
                SendHidReport(MOUSE_ID, moveData);
                if (delayMs > 0)
                    Thread.Sleep(delayMs);
            }
            _currentMousePos = Point.Empty;
        }

        public void MoveTo(int x, int y, int delayMs = 50)
        {
            int dxTotal = x - _currentMousePos.X;
            int dyTotal = y - _currentMousePos.Y;

            while (dxTotal != 0 || dyTotal != 0)
            {
                int dx = Math.Clamp(dxTotal, -MAX_DELTA, MAX_DELTA);
                int dy = Math.Clamp(dyTotal, -MAX_DELTA, MAX_DELTA);

                byte[] data = new byte[HID_MOUSE_REPORT_SIZE];
                data[1] = unchecked((byte)dx);
                data[2] = unchecked((byte)dy);
                SendHidReport(MOUSE_ID, data);

                _currentMousePos.X += dx;
                _currentMousePos.Y += dy;

                dxTotal = x - _currentMousePos.X;
                dyTotal = y - _currentMousePos.Y;

                if (delayMs > 0)
                    Thread.Sleep(delayMs);
            }
        }

        public void ClickAt(int x, int y)
        {
            MoveToOrigin();
            MoveTo(x, y);
            MouseClick();
        }

        public void RightClickAt(int x, int y)
        {
            MoveToOrigin();
            MoveTo(x, y);
            MouseRightClick();
        }

        public void Drag(int fromX, int fromY, int toX, int toY, int durationMs = 300)
        {
            MoveToOrigin();
            MoveTo(fromX, fromY);
            Thread.Sleep(100);

            SendHidReport(MOUSE_ID, new byte[] { 1, 0, 0 });
            Thread.Sleep(50);

            MoveTo(toX, toY, durationMs / 10);

            SendHidReport(MOUSE_ID, new byte[] { 0, 0, 0 });
        }

        public void MouseMove(int dx, int dy)
        {
            SendHidReport(MOUSE_ID, new byte[] { 0, (byte)dx, (byte)dy });
        }

        public void MouseClick()
        {
            SendHidReport(MOUSE_ID, new byte[] { 1, 0, 0 });
            Thread.Sleep(30);
            SendHidReport(MOUSE_ID, new byte[] { 0, 0, 0 });
        }

        public void MouseRightClick()
        {
            SendHidReport(MOUSE_ID, new byte[] { 2, 0, 0 });
            Thread.Sleep(30);
            SendHidReport(MOUSE_ID, new byte[] { 0, 0, 0 });
        }

        public void MouseMiddleClick()
        {
            SendHidReport(MOUSE_ID, new byte[] { 4, 0, 0 });
            Thread.Sleep(30);
            SendHidReport(MOUSE_ID, new byte[] { 0, 0, 0 });
        }

        public void MouseScroll(int amount)
        {
            SendHidReport(MOUSE_ID, new byte[] { 0, 0, (byte)Math.Clamp(amount, -127, 127) });
        }

        public void MouseScrollUp(int amount = 1) => MouseScroll(amount);
        public void MouseScrollDown(int amount = 1) => MouseScroll(-amount);

        // ============================================================
        // ASYNC MOUSE OPERATIONS
        // ============================================================

        public async Task MoveToOriginAsync(int steps = 50, int delayMs = 10, CancellationToken cancellationToken = default)
        {
            byte[] moveData = new byte[HID_MOUSE_REPORT_SIZE];
            moveData[1] = unchecked((byte)-MAX_DELTA);
            moveData[2] = unchecked((byte)-MAX_DELTA);

            for (int i = 0; i < steps; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await SendHidReportAsync(MOUSE_ID, moveData);
                if (delayMs > 0)
                    await Task.Delay(delayMs, cancellationToken);
            }
            _currentMousePos = Point.Empty;
        }

        public async Task MoveToAsync(int x, int y, int delayMs = 50, CancellationToken cancellationToken = default)
        {
            int dxTotal = x - _currentMousePos.X;
            int dyTotal = y - _currentMousePos.Y;

            while (dxTotal != 0 || dyTotal != 0)
            {
                cancellationToken.ThrowIfCancellationRequested();

                int dx = Math.Clamp(dxTotal, -MAX_DELTA, MAX_DELTA);
                int dy = Math.Clamp(dyTotal, -MAX_DELTA, MAX_DELTA);

                byte[] data = new byte[HID_MOUSE_REPORT_SIZE];
                data[1] = unchecked((byte)dx);
                data[2] = unchecked((byte)dy);
                await SendHidReportAsync(MOUSE_ID, data);

                _currentMousePos.X += dx;
                _currentMousePos.Y += dy;

                dxTotal = x - _currentMousePos.X;
                dyTotal = y - _currentMousePos.Y;

                if (delayMs > 0)
                    await Task.Delay(delayMs, cancellationToken);
            }
        }

        public async Task ClickAtAsync(int x, int y, CancellationToken cancellationToken = default)
        {
            await MoveToOriginAsync(cancellationToken: cancellationToken);
            await MoveToAsync(x, y, cancellationToken: cancellationToken);
            MouseClick();
        }

        public async Task RightClickAtAsync(int x, int y, CancellationToken cancellationToken = default)
        {
            await MoveToOriginAsync(cancellationToken: cancellationToken);
            await MoveToAsync(x, y, cancellationToken: cancellationToken);
            MouseRightClick();
        }

        public async Task DragAsync(int fromX, int fromY, int toX, int toY, int durationMs = 300, CancellationToken cancellationToken = default)
        {
            await MoveToOriginAsync(cancellationToken: cancellationToken);
            await MoveToAsync(fromX, fromY, cancellationToken: cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            await SendHidReportAsync(MOUSE_ID, new byte[] { 1, 0, 0 });
            await Task.Delay(50, cancellationToken);

            await MoveToAsync(toX, toY, delayMs: durationMs / 10, cancellationToken: cancellationToken);

            await SendHidReportAsync(MOUSE_ID, new byte[] { 0, 0, 0 });
            _currentMousePos = new Point(toX, toY);
        }

        public void KeyboardSendKey(byte hidCode, byte modifier = 0)
        {
            SendHidReport(KEYBOARD_ID, new byte[] { modifier, 0, hidCode, 0, 0, 0, 0, 0 });
            Thread.Sleep(10);
            SendHidReport(KEYBOARD_ID, new byte[8]);
        }

        public void KeyboardSendText(string text)
        {
            foreach (char c in text)
            {
                byte code = 0, mod = 0;

                if (SpecialCharMap.TryGetValue(c, out var special))
                {
                    code = special.code;
                    mod = special.mod;
                }
                else if (c >= 'a' && c <= 'z')
                    code = (byte)(c - 'a' + 4);
                else if (c >= 'A' && c <= 'Z')
                {
                    code = (byte)(c - 'A' + 4);
                    mod = 0x02;
                }
                else if (c >= '1' && c <= '9')
                    code = (byte)(c - '1' + 0x1E);
                else if (c == '0')
                    code = 0x27;
                else if (c == ' ')
                    code = 0x2C;
                else if (c == '\n')
                    code = 0x28;
                else if (c == '\t')
                    code = 0x2B;
                else if (c == '.')
                    code = 0x37;
                else if (c == ',')
                    code = 0x36;
                else if (c == '-')
                    code = 0x2D;
                else if (c == '/')
                    code = 0x38;
                else
                    continue;

                KeyboardSendKey(code, mod);
                Thread.Sleep(10);
            }
        }

        // ============================================================
        // COMMON KEYS
        // ============================================================
        public void KeyboardF1() => KeyboardSendKey(0x3A);
        public void KeyboardF2() => KeyboardSendKey(0x3B);
        public void KeyboardF3() => KeyboardSendKey(0x3C);
        public void KeyboardF4() => KeyboardSendKey(0x3D);
        public void KeyboardF5() => KeyboardSendKey(0x3E);
        public void KeyboardF6() => KeyboardSendKey(0x3F);
        public void KeyboardF7() => KeyboardSendKey(0x40);
        public void KeyboardF8() => KeyboardSendKey(0x41);
        public void KeyboardF9() => KeyboardSendKey(0x42);
        public void KeyboardF10() => KeyboardSendKey(0x43);
        public void KeyboardF11() => KeyboardSendKey(0x44);
        public void KeyboardF12() => KeyboardSendKey(0x45);
        public void KeyboardEnter() => KeyboardSendKey(0x28);
        public void KeyboardSpace() => KeyboardSendKey(0x2C);
        public void KeyboardBackspace() => KeyboardSendKey(0x2A);
        public void KeyboardEscape() => KeyboardSendKey(0x29);
        public void KeyboardTab() => KeyboardSendKey(0x2B);
        public void KeyboardArrowUp() => KeyboardSendKey(0x52);
        public void KeyboardArrowDown() => KeyboardSendKey(0x51);
        public void KeyboardArrowLeft() => KeyboardSendKey(0x50);
        public void KeyboardArrowRight() => KeyboardSendKey(0x4F);
        public void KeyboardHome() => KeyboardSendKey(0x4A);
        public void KeyboardEnd() => KeyboardSendKey(0x4D);
        public void KeyboardInsert() => KeyboardSendKey(0x49);
        public void KeyboardDelete() => KeyboardSendKey(0x4C);
        public void KeyboardPageUp() => KeyboardSendKey(0x4B);
        public void KeyboardPageDown() => KeyboardSendKey(0x4E);
        public void KeyboardCapsLock() => KeyboardSendKey(0x39);
        public void KeyboardNumLock() => KeyboardSendKey(0x53);
        public void KeyboardScrollLock() => KeyboardSendKey(0x47);
        public void KeyboardPause() => KeyboardSendKey(0x48);
        public void KeyboardPrintScreen() => KeyboardSendKey(0x46);

        public void KeyPressHome() => KeyboardSendKey(0x66);
        public void KeyPressBack() => KeyboardSendKey(0x9E);
        public void KeyPressAppSwitch() => KeyboardSendKey(0xBB);

        public const byte MOD_CTRL_LEFT = 0x01;
        public const byte MOD_SHIFT_LEFT = 0x02;
        public const byte MOD_ALT_LEFT = 0x04;
        public const byte MOD_GUI_LEFT = 0x08;
        public const byte MOD_CTRL_RIGHT = 0x10;
        public const byte MOD_SHIFT_RIGHT = 0x20;
        public const byte MOD_ALT_RIGHT = 0x40;
        public const byte MOD_GUI_RIGHT = 0x80;

        public void KeyboardCtrlC() => KeyboardSendKey(0x06, MOD_CTRL_LEFT);
        public void KeyboardCtrlV() => KeyboardSendKey(0x19, MOD_CTRL_LEFT);
        public void KeyboardCtrlX() => KeyboardSendKey(0x1B, MOD_CTRL_LEFT);
        public void KeyboardCtrlZ() => KeyboardSendKey(0x1D, MOD_CTRL_LEFT);
        public void KeyboardCtrlA() => KeyboardSendKey(0x04, MOD_CTRL_LEFT);
        public void KeyboardCtrlS() => KeyboardSendKey(0x16, MOD_CTRL_LEFT);
        public void KeyboardAltF4() => KeyboardSendKey(0x3D, MOD_ALT_LEFT);
        public void KeyboardAltTab() => KeyboardSendKey(0x2B, MOD_ALT_LEFT);

        public void KeyboardVolumeUp() => KeyboardSendKey(0x80);
        public void KeyboardVolumeDown() => KeyboardSendKey(0x81);
        public void KeyboardVolumeMute() => KeyboardSendKey(0x7F);
        public void KeyboardPlayPause() => KeyboardSendKey(0xCD);
        public void KeyboardStop() => KeyboardSendKey(0xB7);
        public void KeyboardNextTrack() => KeyboardSendKey(0xB5);
        public void KeyboardPrevTrack() => KeyboardSendKey(0xB6);

        private static readonly Dictionary<char, (byte code, byte mod)> SpecialCharMap = new()
        {
            ['!'] = (0x1E, 0x02),
            ['@'] = (0x1F, 0x02),
            ['#'] = (0x20, 0x02),
            ['$'] = (0x21, 0x02),
            ['%'] = (0x22, 0x02),
            ['^'] = (0x23, 0x02),
            ['&'] = (0x24, 0x02),
            ['*'] = (0x25, 0x02),
            ['('] = (0x26, 0x02),
            [')'] = (0x27, 0x02),
            ['_'] = (0x2D, 0x02),
            ['+'] = (0x2E, 0x02),
            ['{'] = (0x2F, 0x02),
            ['}'] = (0x30, 0x02),
            ['|'] = (0x31, 0x02),
            [':'] = (0x33, 0x02),
            ['"'] = (0x34, 0x02),
            ['~'] = (0x35, 0x02),
            ['<'] = (0x36, 0x02),
            ['>'] = (0x37, 0x02),
            ['?'] = (0x38, 0x02),
        };

        public async Task KeyboardSendKeyAsync(byte hidCode, byte modifier = 0)
        {
            await Task.Run(() => SendHidReport(KEYBOARD_ID, new byte[] { modifier, 0, hidCode, 0, 0, 0, 0, 0 }));
            await Task.Delay(10);
            await Task.Run(() => SendHidReport(KEYBOARD_ID, new byte[8]));
        }

        public async Task KeyboardSendTextAsync(string text, CancellationToken cancellationToken = default)
        {
            foreach (char c in text)
            {
                cancellationToken.ThrowIfCancellationRequested();

                byte code = 0, mod = 0;

                if (SpecialCharMap.TryGetValue(c, out var special))
                {
                    code = special.code;
                    mod = special.mod;
                }
                else if (c >= 'a' && c <= 'z')
                    code = (byte)(c - 'a' + 4);
                else if (c >= 'A' && c <= 'Z')
                {
                    code = (byte)(c - 'A' + 4);
                    mod = 0x02;
                }
                else if (c >= '1' && c <= '9')
                    code = (byte)(c - '1' + 0x1E);
                else if (c == '0')
                    code = 0x27;
                else if (c == ' ')
                    code = 0x2C;
                else if (c == '\n')
                    code = 0x28;
                else if (c == '\t')
                    code = 0x2B;
                else if (c == '.')
                    code = 0x37;
                else if (c == ',')
                    code = 0x36;
                else if (c == '-')
                    code = 0x2D;
                else if (c == '/')
                    code = 0x38;
                else
                    continue;

                await KeyboardSendKeyAsync(code, mod);
                await Task.Delay(10, cancellationToken);
            }
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            try { _d?.Close(); } catch { }
            try { _d?.Dispose(); } catch { }
            _d = null;
            GC.SuppressFinalize(this);
        }
    }
}
