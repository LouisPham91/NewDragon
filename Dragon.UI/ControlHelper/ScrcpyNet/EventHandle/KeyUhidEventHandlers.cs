
using Dragon.ControlHelper.ScrcpyNet.SettingDevice;
using Dragon.Controller.DeviceControl.ScrcpyNet.AndroidKeyBroad;
using Dragon.Controller.DeviceControl.ScrcpyNet.Services;
using Dragon.Controller.TaskDeviceManager.Core;
using Dragon.Controller.TaskDeviceManager.Infrastructure;
using Dragon.ControlHelper.ScrcpyNet.Services;
using System.Runtime.InteropServices;

namespace Dragon.ControlHelper.ScrcpyNet.EventHandle
{
    public class KeyUhidEventHandlers : IDisposable
    {
        public string DeviceId { get; }
        IScrcpyService scrcpyService;
        // private static readonly ILogger log = Log.ForContext<KeyUhidEventHandler>();
        private readonly Control _hostcontrol;
        private readonly ushort _uhidId = 1; // SC_HID_ID_KEYBOARD
        private readonly CancellationTokenSource _uhidCts = new CancellationTokenSource();

        // Sửa từ 0x66 thành 256 để hỗ trợ tất cả HID keycode (0x00-0xFF)
        private readonly bool[] _keys = new bool[256];

        private bool _disposed;
        //private long _clipboardSequence;
        private bool _capsLock;
        private bool _numLock;

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        private const int KEYEVENTF_EXTENDEDKEY = 0x1;
        private const int KEYEVENTF_KEYUP = 0x2;
        private const byte VK_CAPITAL = 0x14;
        private const byte VK_NUMLOCK = 0x90;

        private bool _capsLockSynced = false; // Đánh dấu đã đồng bộ lần đầu chưa
        private bool _numLockSynced = false;


        public KeyUhidEventHandlers(Control hostcontrol, IScrcpyService _scrcpyService, string deviceID)
        {
            DeviceId = deviceID;
            _hostcontrol = hostcontrol ?? throw new ArgumentNullException(nameof(hostcontrol));
            scrcpyService = _scrcpyService;
            _hostcontrol.KeyDown += KeyDown;
            _hostcontrol.KeyUp += KeyUp;

            _ = StartUhidKeyboardAsync();
        }

        private async Task StartUhidKeyboardAsync()
        {
            await CreateKeyboardAsync();
            await TrackUhidOutputAsync();
        }

        private async Task CreateKeyboardAsync()
        {
            try
            {
                ushort vendorId = 0;
                ushort productId = 0;
                string name = "VirtualKeyboard";
                byte[] reportDesc = CreateKeyboardReportDescriptor();

                var msg = new UhidCreateControlMessage(_uhidId, vendorId, productId, name, reportDesc);
                await scrcpyService.SendSyncDongBoCommand(msg);

                // Đợi keyboard được tạo
                await Task.Delay(200);

                // Gửi một input report trống để Android phản hồi LED state
                await RequestLedStateAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task RequestLedStateAsync()
        {
            try
            {
                // Gửi UHID_INPUT với data trống (chỉ modifier=0, reserved=0, keys=0)
                byte[] emptyReport = new byte[8]; // Tất cả = 0
                var msg = new UhidInputControlMessage(_uhidId, emptyReport);
                await scrcpyService.SendSyncDongBoCommand(msg);

                //log.Debug("Sent empty UHID input to request LED state");
            }
            catch (Exception)
            {
                //log.Error("Failed to request LED state: {Message}", ex.Message);
            }
        }

        private byte[] CreateKeyboardReportDescriptor()
        {
            return new byte[]
            {
                0x05, 0x01, 0x09, 0x06, 0xA1, 0x01, 0x05, 0x07, 0x19, 0xE0, 0x29, 0xE7, 0x15, 0x00, 0x25, 0x01,
                0x75, 0x01, 0x95, 0x08, 0x81, 0x02, 0x75, 0x08, 0x95, 0x01, 0x81, 0x01, 0x05, 0x08, 0x19, 0x01,
                0x29, 0x05, 0x75, 0x01, 0x95, 0x05, 0x91, 0x02, 0x75, 0x03, 0x95, 0x01, 0x91, 0x01, 0x05, 0x07,
                0x19, 0x00, 0x29, 0x65, 0x15, 0x00, 0x25, 0x65, 0x75, 0x08, 0x95, 0x06, 0x81, 0x00, 0xC0
            };
        }



        private async Task TrackUhidOutputAsync()
        {
            try
            {
                while (!_uhidCts.Token.IsCancellationRequested)
                {
                    var uhidOutput = await scrcpyService.ReceiveUhidOutputAsync(_uhidCts.Token);
                    if (uhidOutput.HasValue && uhidOutput.Value.id == _uhidId)
                    {
                        var (id, data) = uhidOutput.Value;
                        if (data.Length >= 1)
                        {
                            var (capsLock, numLock) = KeycodeUhidHelper.ConvertHidLedToMods(data[0]);

                            // Đồng bộ CapsLock
                            bool windowsCapsLock = Control.IsKeyLocked(Keys.CapsLock);
                            if (!_capsLockSynced && windowsCapsLock != capsLock)
                            {
                                keybd_event(VK_CAPITAL, 0x45, KEYEVENTF_EXTENDEDKEY, UIntPtr.Zero);
                                keybd_event(VK_CAPITAL, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, UIntPtr.Zero);
                            }
                            _capsLock = capsLock;
                            _capsLockSynced = true;

                            // Đồng bộ NumLock
                            bool windowsNumLock = Control.IsKeyLocked(Keys.NumLock);
                            if (!_numLockSynced && windowsNumLock != numLock)
                            {
                                keybd_event(VK_NUMLOCK, 0x45, KEYEVENTF_EXTENDEDKEY, UIntPtr.Zero);
                                keybd_event(VK_NUMLOCK, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, UIntPtr.Zero);
                            }
                            _numLock = numLock;
                            _numLockSynced = true;
                        }

                        // BỎ BREAK - để tiếp tục nhận LED state updates!
                        // break; // <-- XÓA DÒNG NÀY!
                    }

                    await Task.Delay(100, _uhidCts.Token);
                }
            }
            catch (TaskCanceledException)
            {
            }
            catch (Exception)
            {
            }
        }



        private void KeyDown(object? sender, KeyEventArgs e)
        {
            byte keycode;

            if (e.KeyCode == Keys.Escape)
            {
                keycode = 0x29;
            }
            else if (e.KeyCode == Keys.Enter)
            {
                bool isNumPadEnter = ((uint)e.KeyData & 0x01000000) != 0;
                keycode = isNumPadEnter ? (byte)0x58 : (byte)0x28;
            }
            else
            {
                keycode = KeycodeUhidHelper.ConvertToHidKeycode(e.KeyCode);
            }

            if (keycode == 0x00 || keycode >= _keys.Length) return;

            _keys[keycode] = true;

            if (keycode == 0x39) _capsLock = !_capsLock;
            else if (keycode == 0x53) _numLock = !_numLock;

            // Xử lý đặc biệt cho Numpad: cần Shift cho * và /
            if (e.KeyCode == Keys.Multiply || e.KeyCode == Keys.Divide)
            {
                _keys[0xE1] = true; // Bật Shift
            }
            // Xử lý cho Numpad + (cần Shift vì map với phím =)
            else if (e.KeyCode == Keys.Add)
            {
                _keys[0xE1] = true; // Bật Shift để = thành +
            }

            ApplyModifiersFromKeyDown(e.KeyCode, e.Modifiers);

            byte modifier = CalculateCurrentHidModifiers();
            _ = SendKeyEventAsync(modifier);
        }

        private async void KeyUp(object? sender, KeyEventArgs e)
        {
            byte keycode = KeycodeUhidHelper.ConvertToHidKeycode(e.KeyCode);

            if (keycode >= _keys.Length) return;

            _keys[keycode] = false;

            // Thả Shift nếu là Numpad *, /, +
            if (e.KeyCode == Keys.Multiply || e.KeyCode == Keys.Divide || e.KeyCode == Keys.Add)
            {
                _keys[0xE1] = false;
            }

            RemoveModifiersFromKeyUp(e.KeyCode);

            byte modifier = CalculateCurrentHidModifiers();
            await SendKeyEventAsync(modifier);
        }
        /// <summary>
        /// Áp dụng modifier keys vào mảng _keys khi nhấn phím
        /// </summary>
        private void ApplyModifiersFromKeyDown(Keys key, Keys modifiers)
        {
            // Set trực tiếp dựa trên key đang nhấn
            switch (key)
            {
                case Keys.LShiftKey:
                case Keys.RShiftKey:
                case Keys.ShiftKey:
                    _keys[0xE1] = true; // Left Shift
                    break;
                case Keys.LControlKey:
                case Keys.RControlKey:
                case Keys.ControlKey:
                    _keys[0xE0] = true; // Left Ctrl
                    break;
                case Keys.LMenu:
                case Keys.RMenu:
                case Keys.Menu:
                case Keys.Alt:
                    _keys[0xE2] = true; // Left Alt
                    break;
                case Keys.LWin:
                case Keys.RWin:
                    _keys[0xE3] = true; // Left GUI
                    break;
            }

            // Đảm bảo SHIFT được set nếu modifiers có Shift
            // (hữu ích khi nhấn tổ hợp như Shift+A)
            if ((modifiers & Keys.Shift) == Keys.Shift)
            {
                _keys[0xE1] = true;
            }
            if ((modifiers & Keys.Control) == Keys.Control)
            {
                _keys[0xE0] = true;
            }
            if ((modifiers & Keys.Alt) == Keys.Alt)
            {
                _keys[0xE2] = true;
            }
        }

        /// <summary>
        /// Xóa modifier keys khỏi mảng _keys khi thả phím
        /// </summary>
        private void RemoveModifiersFromKeyUp(Keys key)
        {
            switch (key)
            {
                case Keys.LShiftKey:
                case Keys.RShiftKey:
                case Keys.ShiftKey:
                    _keys[0xE1] = false;
                    break;
                case Keys.LControlKey:
                case Keys.RControlKey:
                case Keys.ControlKey:
                    _keys[0xE0] = false;
                    break;
                case Keys.LMenu:
                case Keys.RMenu:
                case Keys.Menu:
                case Keys.Alt:
                    _keys[0xE2] = false;
                    break;
                case Keys.LWin:
                case Keys.RWin:
                    _keys[0xE3] = false;
                    break;
            }
        }
        /// <summary>
        /// Tính toán HID modifier byte từ trạng thái hiện tại của _keys
        /// </summary>
        private byte CalculateCurrentHidModifiers()
        {
            byte modifier = 0;

            if (_keys[0xE0] || _keys[0xE4]) // Left Ctrl hoặc Right Ctrl
                modifier |= 0x01;

            if (_keys[0xE1] || _keys[0xE5]) // Left Shift hoặc Right Shift
                modifier |= 0x02;

            if (_keys[0xE2] || _keys[0xE6]) // Left Alt hoặc Right Alt
                modifier |= 0x04;

            if (_keys[0xE3] || _keys[0xE7]) // Left GUI hoặc Right GUI
                modifier |= 0x08;

            return modifier;
        }

        private async Task SendKeyEventAsync(byte modifier)
        {
            try
            {
                byte[] data = new byte[8];
                data[0] = modifier;
                data[1] = 0; // Reserved

                int keyCount = 0;

                // Duyệt qua tất cả các phím đang nhấn (bỏ qua modifier keys vì đã ở data[0])
                for (int i = 0; i < _keys.Length && keyCount < 6; i++)
                {
                    // Bỏ qua các modifier keys (0xE0-0xE7) vì đã được xử lý trong data[0]
                    if (i >= 0xE0 && i <= 0xE7)
                        continue;

                    if (_keys[i])
                    {
                        if (keyCount >= 6)
                        {
                            // Phantom state - quá nhiều phím
                            for (int j = 2; j < 8; j++)
                                data[j] = 0x01;
                            keyCount = 6;
                            // log.Warning("Phantom state generated: Too many keys pressed");
                            break;
                        }
                        data[2 + keyCount] = (byte)i;
                        keyCount++;
                    }
                }

                var msg = new UhidInputControlMessage(_uhidId, data);
                await scrcpyService.SendSyncDongBoCommand(msg);

                // log.Debug("Sent UHID keyboard input: Modifier={Modifier:X2}, KeyCount={KeyCount}", modifier, keyCount);
            }
            catch (Exception)
            {
                // log.Error("Failed to send UHID keyboard input: {Message}", ex.Message);
            }

        }


        // SendText automation
        public async Task<NodeResult> SendText(DLoopContext ctx, string text)
        {
            var status = await SendTextInternal(ctx.Token, ctx.LogKey, text);
            return status == ExecutionStatus.Continue ? NodeResult.Ok() : NodeResult.Stop();
        }

        public async Task<ExecutionStatus> SendText(string text, CancellationToken token, string deviceId)
        {
            return await SendTextInternal(token, deviceId, text);
        }

        private async Task<ExecutionStatus> SendTextInternal(CancellationToken token, string deviceId, string text)
        {
            if (token.IsCancellationRequested) return ExecutionStatus.Stop;
            if (string.IsNullOrEmpty(text)) return ExecutionStatus.Stop;

            text = text.Replace("\r\n", "\n").Replace('\r', '\n');

            const int baseDelay = 80, shiftExtra = 40, batchSize = 30, batchDelay = 200, downUp = 15;
            int total = text.Length;
            int count = 0;

            Logger.Notify(deviceId, $"Bắt đầu gửi {total} ký tự...", Logger.Icon.Information);

            foreach (char c in text)
            {
                if (token.IsCancellationRequested) return ExecutionStatus.Stop;

                var androidCode = AndroidCharMapper.CharToAndroidKeycode(c);
                if (androidCode == AndroidKeycode.AKEYCODE_UNKNOWN) continue;
                byte hidCode = KeycodeUhidHelper.ConvertAndroidKeycodeToHid(androidCode);
                if (hidCode == 0) continue;

                bool needShift = AndroidCharMapper.RequiresShift(c);

                try
                {
                    if (needShift)
                    {
                        _keys[0xE1] = true;
                        await SendKeyEventAsync(0x02);
                        if (await Logger.DelayAsync(downUp, token, deviceId) == ExecutionStatus.Stop) return ExecutionStatus.Stop;
                    }

                    _keys[hidCode] = true;
                    await SendKeyEventAsync((byte)(needShift ? 0x02 : 0x00));
                    if (await Logger.DelayAsync(downUp, token, deviceId) == ExecutionStatus.Stop) return ExecutionStatus.Stop;

                    _keys[hidCode] = false;
                    await SendKeyEventAsync((byte)(needShift ? 0x02 : 0x00));

                    if (needShift)
                    {
                        if (await Logger.DelayAsync(downUp, token, deviceId) == ExecutionStatus.Stop) return ExecutionStatus.Stop;
                        _keys[0xE1] = false;
                        await SendKeyEventAsync(0x00);
                    }

                    count++;
                    int delay = baseDelay + (needShift ? shiftExtra : 0) + Random.Shared.Next(5, 15);
                    if (await Logger.DelayAsync(delay, token, deviceId) == ExecutionStatus.Stop) return ExecutionStatus.Stop;

                    if (count % batchSize == 0)
                    {
                        if (await Logger.DelayAsync(batchDelay, token, deviceId) == ExecutionStatus.Stop) return ExecutionStatus.Stop;
                        Logger.Notify(deviceId, $"Tiến độ: {count}/{total} ({count * 100 / total}%)", Logger.Icon.Information);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Notify(deviceId, $"Lỗi '{c}': {ex.Message}", Logger.Icon.Error);
                }
            }

            Logger.Notify(deviceId, $"Hoàn thành! Đã gửi {total} ký tự", Logger.Icon.Information);
            return ExecutionStatus.Continue;
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            _uhidCts.Cancel();
            _uhidCts.Dispose();
            _hostcontrol.KeyDown -= KeyDown;
            _hostcontrol.KeyUp -= KeyUp;
            try
            {
                var msg = new UhidDestroyControlMessage(_uhidId);
                _ = scrcpyService.SendControlCommand(msg);
            }
            catch { }
        }
    }
}