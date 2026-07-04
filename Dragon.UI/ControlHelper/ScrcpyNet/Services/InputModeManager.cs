using Dragon.ControlHelper.ScrcpyNet.EventHandle;
using Dragon.Database.Models;
using Dragon.ControlHelper.ScrcpyNet.Services;

public class InputModeManager : IDisposable
{
    private readonly Control _hostControl;
    private readonly IScrcpyService _scrcpyService;
    private readonly string _deviceID;
    private bool _disposed;

    // Các handler
    private DragDropEventHandlers? _drag;
    private KeyEventHandlers? _key;
    private MouseEventHandlers? _mouse;
    private KeyUhidEventHandlers? _keyUhid;
    private MouseUhidEventHandles? _mouseUhid;
    private MouseFakeUhidHandlers? _mouseFake;

    public InputModeManager(Control hostControl, IScrcpyService scrcpyService, string deviceID)
    {
        _hostControl = hostControl;
        _scrcpyService = scrcpyService;
        _deviceID = deviceID;

        // Drag & drop không phụ thuộc chế độ, luôn hoạt động
        _drag = new DragDropEventHandlers(deviceID, hostControl);
    }

    public async Task SwitchModeAsync(Phone phone, bool useRealMouse)
    {
        // Dọn các handler cũ (trừ drag & drop)
        _key?.Dispose(); _key = null;
        _mouse?.Dispose(); _mouse = null;
        _keyUhid?.Dispose(); _keyUhid = null;
        _mouseUhid?.Dispose(); _mouseUhid = null;
        _mouseFake?.Dispose(); _mouseFake = null;

        if (phone.PhoneMode is PhoneMode.USB or PhoneMode.WIFI)
        {
            _key = new KeyEventHandlers(_hostControl, _scrcpyService , _deviceID);
            _mouse = new MouseEventHandlers(_hostControl, _scrcpyService, _deviceID);
            return;
        }

        if (phone.PhoneMode is PhoneMode.UATX or PhoneMode.UHDI or PhoneMode.WATX or PhoneMode.WHDI)
        {
            if (!phone.IsUHDI) return;
            if (!_scrcpyService.Connected) return;

            _keyUhid = new KeyUhidEventHandlers(_hostControl, _scrcpyService, _deviceID);

            if (useRealMouse)
            {
                _mouseUhid = new MouseUhidEventHandles(_hostControl, _scrcpyService, _deviceID);
            }
            else
            {
                _mouseFake = new MouseFakeUhidHandlers(_hostControl, _scrcpyService, _deviceID);
                await _mouseFake.MoveToOriginAsync(5);
            }
        }
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        _drag?.Dispose();
        _key?.Dispose();
        _mouse?.Dispose();
        _keyUhid?.Dispose();
        _mouseUhid?.Dispose();
        _mouseFake?.Dispose();
    }
}