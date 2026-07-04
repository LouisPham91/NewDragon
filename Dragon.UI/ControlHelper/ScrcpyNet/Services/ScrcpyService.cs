using DirectN;
using Dragon.ControlHelper.UIController;
using Dragon.Controller.Database.Services;
using Dragon.Controller.DeviceControl.ScrcpyNet.Services;
using Dragon.Controller.TaskDeviceManager.Infrastructure;
using Dragon.Database.Models;
using System.Diagnostics;


namespace Dragon.ControlHelper.ScrcpyNet.Services
{
    public class ScrcpyService : IScrcpyService
    {
        public string DeviceID { get; }
        public Phone phone => GetPhone();
        private Scrcpy _scrcpy { get; set; }

        public bool _isSyncDongBo;
        public bool GetSyncDongBo() => _isSyncDongBo;

        public Size _videoSize { get; set; }

        private long _clipboardSequence = 0;
        public long ClipboardSequence => _clipboardSequence++;
        public bool Connected => _scrcpy?.Connected ?? false;

        public ScrcpyService(string deviceID, Scrcpy scrcpy)
        {
            DeviceID = deviceID;
            _scrcpy = scrcpy;
        }
        public void SetVideoSize(int width, int height)
        {
            _videoSize = new Size(width, height);
        }
        public Phone GetPhone()
        {
            Debug.WriteLine("GetPhone ScrcpyService");
            return PhoneRepository.FindOneByDeviceID(DeviceID) ?? throw new InvalidOperationException($"Phone {DeviceID} not found");
        }
        // ---------- Gửi lệnh ----------
        public async Task SendControlCommand(IControlMessage msg)
        {
            if (_scrcpy != null && _scrcpy.Connected)
                await _scrcpy.SendControlCommand(msg);
        }
        public async Task SendSyncDongBoCommand(IControlMessage msg)
        {
            if (_isSyncDongBo)
            {
                PDSyncDongBo.Instance.SendEventTodevice(msg);
            }
            else
            {
                await _scrcpy.SendControlCommand(msg);
            }

        }

        #region ---------- Đồng bộ Điều Khiển ---------- 
        public void SetSyncDongBoMode(bool sync)
        {
            if (_isSyncDongBo == sync) return;
            _isSyncDongBo = sync;

            if (sync)
                PDSyncDongBo.DeviceEvent += HandleEvent;
            else
                PDSyncDongBo.DeviceEvent -= HandleEvent;
        }

        public async void HandleEvent(IControlMessage msg)
        {
            if (!PDSyncDongBo.Instance.IsDongBo || !_isSyncDongBo)
            {
                return;
            }

            try
            {

                if (msg.Type == 9)
                {
                    await SetClipBroadAsync(msg);
                }
                else
                {
                    await _scrcpy.SendControlCommand(msg);
                }
            }
            catch (Exception)
            {

            }
        }
        #endregion

        #region ---------- Tiện ích ---------- 
    
        public async Task RotateDevice()
        {
            var msg = new RotateDeviceControlMessage();
            if (_isSyncDongBo)
            {           
                PDSyncDongBo.Instance.SendEventTodevice(msg);
            }
            else
            {
                await _scrcpy.SendControlCommand(msg);
            }
        }

        public async Task SetDisplayPower(bool on)
        {
            if (_scrcpy != null)
                await _scrcpy.SendControlCommand(new SetDisplayPowerControlMessage(on));
        }

        public async Task StartApp(string packageName)
        {
            if (_scrcpy != null)
                await _scrcpy.SendControlCommand(new StartAppControlMessage(packageName));
        }



        public async Task<bool> WaitScrcpyAsync(string deviceID, CancellationToken ct = default)
        {
            // Đơn giản là đợi cho đến khi Connected, có thể thêm timeout
            while (!Connected && !ct.IsCancellationRequested)
                await Task.Delay(100, ct);
            return Connected;
        }

        // ---------- Clipboard ----------
        public async Task<string?> GetClipboardAsync(CancellationToken ct = default)
            => _scrcpy != null ? await _scrcpy.ReceiveClipboardDataAsync(ct) : null;

        public async Task SetClipboardAsync(string text, bool paste, CancellationToken ct = default)
        {
            if (_scrcpy == null) return;
            _clipboardSequence++;
            var msg = new SetClipboardControlMessage(_clipboardSequence, text, paste);
            await _scrcpy.SendControlCommand(msg);
        }
        public async Task SetClipBroadAsync(IControlMessage msg)
        {
            await SendControlCommand(msg);
            long? receivedSequence = await ReceiveAckClipboardAsync();
            if (receivedSequence.HasValue && receivedSequence.Value == _clipboardSequence)
            {
                Logger.Notify($"[phone {phone.PhoneTagNumber}]", "Clipboard SetText Sussecs", Logger.Icon.Information); ;
            }
            else
            {
                Logger.Notify($"[phone {phone.PhoneTagNumber}]", "Clipboard SetText False", Logger.Icon.Information);
            }
        }
        public async Task<string?> ReceiveClipboardDataAsync(CancellationToken ct = default)
            => _scrcpy != null ? await _scrcpy.ReceiveClipboardDataAsync(ct) : null;

        public async Task<long?> ReceiveAckClipboardAsync(CancellationToken ct = default)
            => _scrcpy != null ? await _scrcpy.ReceiveAckClipboardAsync(ct) : null;

        // ---------- UHID ----------
        public async Task<(int id, byte[] data)?> ReceiveUhidOutputAsync(CancellationToken ct = default)
            => _scrcpy != null ? await _scrcpy.ReceiveUhidOutputAsync(ct) : null;

        public async Task RequetRealMouseFakeUHDI()
        {

        }
        public async Task ResetVideoAync()
        {
            var msg = new ResetVideoControlMessage();
            await SendControlCommand(msg);
        }
        #region ---------- Camera & Display ----------

        public async Task SetCameraTorchAsync(bool on)
        {
            if (_scrcpy != null && _scrcpy.Connected)
                await _scrcpy.SendControlCommand(new CameraSetTorchControlMessage(on));
        }

        public async Task CameraZoomInAsync()
        {
            if (_scrcpy != null && _scrcpy.Connected)
                await _scrcpy.SendControlCommand(new CameraZoomInControlMessage());
        }

        public async Task CameraZoomOutAsync()
        {
            if (_scrcpy != null && _scrcpy.Connected)
                await _scrcpy.SendControlCommand(new CameraZoomOutControlMessage());
        }



        #endregion
        #endregion
    }
}
