using Dragon.ControlHelper.ScrcpyNet.Services;
using Dragon.ControlHelper.ScrcpyNet.SettingDevice;
using Dragon.Controller.DeviceControl.ScrcpyNet.AndroidKeyBroad;
using Dragon.Controller.DeviceControl.ScrcpyNet.Services;
using Dragon.Controller.TaskDeviceManager.Core;
using Dragon.Controller.TaskDeviceManager.Infrastructure;

namespace Dragon.ControlHelper.ScrcpyNet.EventHandle
{
    /// <summary>
    /// Xử lý toàn bộ phím cho 1 PanelDeviceView.
    /// Được InputModeManager tạo/hủy theo chế độ USB/WIFI/UHDI.
    /// </summary>
    public sealed class KeyEventHandlers : IDisposable
    {
        private readonly Control _hostControl;
        private readonly IScrcpyService _scrcpyService;
        private string DeviceID { get; }
        private bool _disposed;

        public KeyEventHandlers(Control hostControl, IScrcpyService scrcpyService, string deviceID)
        {
            _hostControl = hostControl;
            _scrcpyService = scrcpyService;
            DeviceID = deviceID;

            _hostControl.KeyDown += OnKeyDown;
            _hostControl.KeyUp += OnKeyUp;
            _hostControl.PreviewKeyDown += OnPreviewKeyDown;
        }
        private void OnPreviewKeyDown(object? sender, PreviewKeyDownEventArgs e)
        {
            // Cho phép Tab, arrows đi qua KeyDown
            if (e.KeyCode == Keys.Tab || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down ||
                e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
            {
                e.IsInputKey = true;
            }
        }
        private async void OnKeyDown(object? sender, KeyEventArgs e)
        {
            if (_disposed) return;
            var keycode = KeycodeHelper.ConvertKey(e.KeyCode);
            var meta = KeycodeHelper.ConvertModifiers(e.Modifiers);

            // Ctrl+V không gửi ngay, chờ xử lý ở KeyUp
            if (e.Control && e.KeyCode == Keys.V)
            {
                e.Handled = true;
                return;
            }

            var down = new InjectKeycodeControlMessage(AndroidKeyEventAction.AKEY_EVENT_ACTION_DOWN, keycode, 0, meta);
            await _scrcpyService.SendSyncDongBoCommand(down); // key down luôn local để tránh double event khi đồng bộ
        }

        private async void OnKeyUp(object? sender, KeyEventArgs e)
        {
            if (_disposed) return;
            var keycode = KeycodeHelper.ConvertKey(e.KeyCode);
            var meta = KeycodeHelper.ConvertModifiers(e.Modifiers);
            var up = new InjectKeycodeControlMessage(AndroidKeyEventAction.AKEY_EVENT_ACTION_UP, keycode, 0, meta);

            // Ctrl+C: copy từ device về PC -> local
            if (e.Control && e.KeyCode == Keys.C)
            {
                try
                {
                    var text = await _scrcpyService.GetClipboardAsync();
                    if (!string.IsNullOrEmpty(text))
                        Clipboard.SetText(text);
                }
                catch { }
                await _scrcpyService.SendSyncDongBoCommand(up);
                return;
            }

            // Ctrl+V: paste từ PC lên device -> local
            if (e.Control && e.KeyCode == Keys.V)
            {
                if (await SetClipboardFromPCAsync())
                {
                    var down = new InjectKeycodeControlMessage(AndroidKeyEventAction.AKEY_EVENT_ACTION_DOWN, keycode, 0, meta);
                    await _scrcpyService.SendSyncDongBoCommand(down);
                    await Task.Delay(15);
                    await _scrcpyService.SendSyncDongBoCommand(up);
                }
                return;
            }

            // Phím thường: cho phép đồng bộ nếu đang selected
            await _scrcpyService.SendSyncDongBoCommand(up);
        }


        private async Task SendControlAsync(IControlMessage msg)
        {
            if (_scrcpyService == null) return;
            await _scrcpyService.SendControlCommand(msg);
        }

        #region Clipboard

        private async Task<bool> SetClipboardFromPCAsync()
        {
            if (!Clipboard.ContainsText()) return false;
            string text = Clipboard.GetText();
            if (string.IsNullOrEmpty(text) || text.Length > ControlMessage.CLIPBOARD_TEXT_MAX_LENGTH) return false;

            var sequence = _scrcpyService.ClipboardSequence;
            var msg = new SetClipboardControlMessage(sequence, text, paste: true);
            await _scrcpyService.SendControlCommand(msg);
            long? ack = await _scrcpyService.ReceiveAckClipboardAsync();
            return ack == sequence;
        }

        #endregion

        #region SendText - dùng cho automation

        public async Task<NodeResult> SendText(DLoopContext ctx, string text)
        {
            var status = await SendTextInternal(ctx.Token, ctx.LogKey, text);
            return status == ExecutionStatus.Continue ? NodeResult.Ok() : NodeResult.Stop();
        }

        public async Task<ExecutionStatus> SendTextV2(string text, CancellationToken token, string deviceId)
        {
            return await SendTextInternal(token, deviceId, text);
        }

        private async Task<ExecutionStatus> SendTextInternal(CancellationToken token, string deviceId, string text)
        {
            if (token.IsCancellationRequested) return ExecutionStatus.Stop;
            if (string.IsNullOrEmpty(text)) return ExecutionStatus.Stop;

            text = text.Replace("\r\n", "\n").Replace('\r', '\n');

            const int baseDelay = 80;
            const int shiftDelay = 40;
            const int downUp = 15;
            const int batchSize = 30;

            int total = text.Length;
            Logger.Notify(deviceId, $"Bắt đầu gửi {total} ký tự...", Logger.Icon.Information);

            for (int i = 0; i < total; i++)
            {
                if (token.IsCancellationRequested) break;

                char c = text[i];
                var keycode = AndroidCharMapper.CharToAndroidKeycode(c);
                if (keycode == AndroidKeycode.AKEYCODE_UNKNOWN) continue;

                bool needShift = AndroidCharMapper.RequiresShift(c);
                var meta = needShift ? AndroidMetastate.AMETA_SHIFT_ON : AndroidMetastate.AMETA_NONE;

                try
                {
                    if (needShift)
                    {
                        await SendControlAsync(new InjectKeycodeControlMessage(
                            AndroidKeyEventAction.AKEY_EVENT_ACTION_DOWN,
                            AndroidKeycode.AKEYCODE_SHIFT_LEFT, 0, AndroidMetastate.AMETA_NONE));
                        await Task.Delay(downUp, token);
                    }

                    await SendControlAsync(new InjectKeycodeControlMessage(
                        AndroidKeyEventAction.AKEY_EVENT_ACTION_DOWN, keycode, 0, meta));
                    await Task.Delay(downUp, token);
                    await SendControlAsync(new InjectKeycodeControlMessage(
                        AndroidKeyEventAction.AKEY_EVENT_ACTION_UP, keycode, 0, meta));

                    if (needShift)
                    {
                        await Task.Delay(downUp, token);
                        await SendControlAsync(new InjectKeycodeControlMessage(
                            AndroidKeyEventAction.AKEY_EVENT_ACTION_UP,
                            AndroidKeycode.AKEYCODE_SHIFT_LEFT, 0, AndroidMetastate.AMETA_NONE));
                    }
                }
                catch (Exception ex)
                {
                    Logger.Notify(deviceId, $"Lỗi '{c}': {ex.Message}", Logger.Icon.Error);
                }

                await Task.Delay(baseDelay + (needShift ? shiftDelay : 0) + Random.Shared.Next(5, 15), token);

                if ((i + 1) % batchSize == 0)
                {
                    var returnStop = await Logger.DelayAsync(200, token, DeviceID);
                    if (returnStop == ExecutionStatus.Stop) return ExecutionStatus.Stop;

                }
            }

            Logger.Notify(deviceId, $"Hoàn thành gửi {total} ký tự", Logger.Icon.Information);
            return token.IsCancellationRequested ? ExecutionStatus.Stop : ExecutionStatus.Continue;
        }

        #endregion

        #region PressKey

        public async Task<ExecutionStatus> PressKey(string command, CancellationToken token, string deviceId)
        {
            if (token.IsCancellationRequested) return ExecutionStatus.Stop;

            var code = KeycodeHelper.MapStringToAndroidKeycode(command);
            if (code == AndroidKeycode.AKEYCODE_UNKNOWN)
            {
                Logger.Notify(deviceId, $"Key không hợp lệ: {command}", Logger.Icon.Error);
                return ExecutionStatus.Stop;
            }

            await SendControlAsync(new InjectKeycodeControlMessage(AndroidKeyEventAction.AKEY_EVENT_ACTION_DOWN, code, 0, 0));
            await SendControlAsync(new InjectKeycodeControlMessage(AndroidKeyEventAction.AKEY_EVENT_ACTION_UP, code, 0, 0));

            var returnStop = await Logger.DelayAsync(100, token, DeviceID);
            if (returnStop == ExecutionStatus.Stop) return ExecutionStatus.Stop;

            Logger.Notify(deviceId, $"Đã gửi: {command}", Logger.Icon.Information);
            return ExecutionStatus.Continue;
        }

        #endregion


        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            try
            {
                _hostControl.KeyDown -= OnKeyDown;
                _hostControl.KeyUp -= OnKeyUp;
                _hostControl.PreviewKeyDown -= OnPreviewKeyDown;
            }
            catch { }
        }
    }
}
