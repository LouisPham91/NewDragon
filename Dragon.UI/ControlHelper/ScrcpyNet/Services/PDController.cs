
using AdvancedSharpAdbClient;
using AdvancedSharpAdbClient.Models;
using AdvancedSharpAdbClient.Receivers;
using Dragon.Controller.Controller.DeviceControl;
using Dragon.Controller.Database.Services;
using Dragon.Controller.DeviceControl.ScrcpyNet.Services;
using Dragon.Controller.GlobalControl.Property;
using Dragon.Database.Models;
using FFmpeg.AutoGen;
using System.Diagnostics;


namespace Dragon.ControlHelper.ScrcpyNet.Services
{
    public class PDController : IPDController, IDisposable
    {
        public string DeviceID { get; }
        public DeviceData DeviceData { get; private set; }
        public IAdbClient _adbClient { get; }
        public CancellationTokenSource? cts => _cts;
        public bool IsConnecting => (_scrcpy != null) ? _scrcpy.Connected : false;
        public IScrcpyService? ScrcpyService => _scrcpyService;
        public Phone phone => PhoneRepository.FindOneByDeviceID(DeviceID) ?? throw new NotImplementedException();
        private readonly IPanelDeviceUI _view;
        private Sdl3RenderrEngine? _renderEngine;
        private ScrcpyService? _scrcpyService;
        private Scrcpy? _scrcpy;
        private CancellationTokenSource? _cts;
        private bool _disposed;

        public PDController(string deviceId, DeviceData deviceData, IPanelDeviceUI view)
        {
            _adbClient = new AdbClient();
            DeviceID = deviceId;
            DeviceData = deviceData;
            _view = view;
            RenderInitialize(deviceId, view.RenderHandle, this);

            view.SetIPDController(this);
        }


        #region ------------- PDController -  Init Khởi Tạo Các Class Liên Quan -------------
        // Khởi tạo Render và vẽ loading
        public void RenderInitialize(string deviceID, nint RenderHandle, IPDController controller)
        {
            _renderEngine = new Sdl3RenderrEngine(deviceID, RenderHandle, controller);
            _renderEngine.InitSdl();
        }
        #endregion

        #region ------------- Interface IPDController -  StartAsync,  StopAsync -------------
        public async Task StartAsync()
        {
            // 1. dọn instance cũ nếu có, thay vì return im lặng
            if (_scrcpy != null)
            {
                await StopAsync();
            }

            var allSize = GetSettings.GetALLSize(phone);
            var bitrate = GetSettings.GetBitrate();
            var fps = GetSettings.GetFps();

            _cts = new CancellationTokenSource();
            await KillPreviousScrcpyServerAsync(_cts.Token);
            _renderEngine?.SetToken(_cts.Token);

            // 2. tạo mới và gán property bình thường, không dùng ?.
            _scrcpy = new Scrcpy(DeviceID, DeviceData)
            {
                Bitrate = bitrate,
                Fps = fps,
                MaxSize = (phone.IsRunning) ? allSize.running.h : allSize.max.h
            };

            _scrcpy.SetPhySicalSize(phone.PhysicalWidth, phone.PhysicalHeight);

            _scrcpyService = new ScrcpyService(DeviceID, _scrcpy);
            //_scrcpyService.SetVideoSize(allSize.min.w, allSize.min.h);

            // 4. đăng ký event
            _scrcpy.VideoStreamDecoder.NewFrameEvent += OnFrameReceived;
            _scrcpy.OnLoadSizeEvent += OnDeviceInfo;
            _scrcpy.OnConnecting += OnConnecting;
            _scrcpy.OnScrcpyVideoSizeChanged += _scrcpy_OnScrcpyVideoSizeChanged;

            try
            {
                await _scrcpy.StartAsync(timeoutMs: 20000, cancellationToken: _cts.Token);
            }
            catch (Exception ex)
            {
                // lỗi khởi động -> dọn và ném lên UI
                await StopAsync();
                throw new Exception($"Scrcpy start failed: {ex.Message}", ex);
            }
        }


        public async Task StopAsync()
        {
            _view.OnScrcpyServiceLost();
            _cts?.Cancel();

            if (_scrcpy != null)
            {
                await _scrcpy.StopAsync();

                _scrcpy.OnFrame -= OnFrameReceived;
                _scrcpy.OnLoadSizeEvent -= OnDeviceInfo;
                _scrcpy.OnConnecting -= OnConnecting;
                _scrcpy.VideoStreamDecoder.NewFrameEvent -= OnFrameReceived;

                _scrcpy.Dispose();
                _scrcpy = null;
            }


            _view.AddOrUpdatePhoneStatus(PhoneStatus.Disconnect);

        }
        public async Task StopNoCreateAsync()
        {
            _view.OnScrcpyServiceLost();
            _cts?.Cancel();

            if (_scrcpy != null)
            {
                await _scrcpy.StopAsync();

                _scrcpy.OnFrame -= OnFrameReceived;
                _scrcpy.OnLoadSizeEvent -= OnDeviceInfo;
                _scrcpy.OnConnecting -= OnConnecting;
                _scrcpy.VideoStreamDecoder.NewFrameEvent -= OnFrameReceived;

                _scrcpy.Dispose();
                _scrcpy = null;
            }

        }

        private async Task<bool> IsScrcpyRunningAsync(AdbClient _adb, DeviceData _device, CancellationToken ct = default)
        {
            // 1. check process (dùng [s] để không match chính grep)
            var procReceiver = new ConsoleOutputReceiver();
            await _adb.ExecuteRemoteCommandAsync(
                "ps -ef | grep [s]crcpy",
                _device, procReceiver, ct);
            bool hasProcess = !string.IsNullOrWhiteSpace(procReceiver.ToString());

            // 2. check socket abstract
            var sockReceiver = new ConsoleOutputReceiver();
            await _adb.ExecuteRemoteCommandAsync(
                "grep -a scrcpy /proc/net/unix",
                _device, sockReceiver, ct);
            bool hasSocket = sockReceiver.ToString().Contains("@scrcpy_");

            if (hasSocket == false)
            {
                // 3. check socket abstract
                var sockReceiverv2 = new ConsoleOutputReceiver();
                await _adb.ExecuteRemoteCommandAsync(
                    @"cat /proc/net/unix | tr '\0' '\n' | grep scrcpy",
                    _device, sockReceiverv2, ct);

                hasSocket = sockReceiverv2.ToString().Contains("@scrcpy_");
            }
             
            return hasProcess || hasSocket;
        }

     

        public static async Task RemoveScrcpyReversesAsync(DeviceData device, IAdbClient adb, CancellationToken token = default)
        {
            // 1. lấy list reverse – KHÔNG dùng shell
            var reverses = await adb.ListReverseForwardAsync(device, token);
        }

        private async Task KillPreviousScrcpyServerAsync(CancellationToken ct = default)
        {
            var adb = new AdbClient();

            // 1. Không chạy thì thôi
            if (!await IsScrcpyRunningAsync(adb, DeviceData, ct).ConfigureAwait(false))
                return;

            try
            {
                // 2. pkill nhanh nhất
                await adb.ExecuteRemoteCommandAsync("pkill -f scrcpy", DeviceData, new ConsoleOutputReceiver(), ct);
                await AdbService.RemoveScrcpyForwardsAsync(DeviceData, adb, ct);
                await AdbService.RemoveScrcpyReversesAsync(DeviceData, adb, ct);

                await Task.Delay(200, ct);
                // 3. Verify tối đa 5 lần
                for (int i = 0; i < 5; i++)
                {
                    if (!await IsScrcpyRunningAsync(adb, DeviceData, ct).ConfigureAwait(false))
                        return; // đã chết

                    await Task.Delay(200, ct);
                }

                await AdbService.RemoveScrcpyForwardsAsync(DeviceData, adb, ct);
                await AdbService.RemoveScrcpyReversesAsync(DeviceData, adb, ct);

                // 5. Verify cuối
                await Task.Delay(200, ct);
                if (await IsScrcpyRunningAsync(adb, DeviceData, ct).ConfigureAwait(false))
                {
                    Debug.WriteLine($"[Scrcpy] WARN: server vẫn còn sau kill -9 trên {DeviceData.Serial}");
                }
            }
            catch (OperationCanceledException) { throw; }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Scrcpy] Kill check lỗi: {ex.Message}");
            }
        }
        #endregion

        #region ------------- Interface IPDController , Khai Báo Các Hàm Hỗ Trợ PanelDevice UI View, Scrcpy, ScrcpyService, SDL3Render, Other -------------  
        // IPDController (Sở Hữu)
        // Support SDL3Render - Set Video Size (Cố Định)
        public void SetRenderPanelSize(int width, int height)
        {
            _view.InvokeOnUI(() => _renderEngine?.UpdateDisplaySize(width, height));
        }

        // ScrcpyService để thông báo server đổi Size
        public async Task ResizeVideoMaxSize()
        {
            await StopAsync();
            await StartAsync();

        }
        private async void _scrcpy_OnScrcpyVideoSizeChanged()
        {
            _view.InvokeOnUI(async () =>
            {
                _view.ScrcpySizeChanged();
                await StopAsync();
                await StartAsync();
            });

        }

        // IPDController (Sở Hữu)
        // Support SrcpyService Đồng bộ
        public void SetSycnDongBo(bool _isSycn)
        {
            ScrcpyService?.SetSyncDongBoMode(_isSycn);
        }

        // IPDController (Sở Hữu)
        // Runtime => Dloop, Check Image, ORC ... 
        public Bitmap? ScreenShot()
        {
            if (_scrcpy == null) return null;
            return _scrcpy.VideoStreamDecoder.ScreenShot();
        }


        // IPDController (Sở Hữu)
        // Support SDL3Render ShowModelName
        public void ShowModelName()
        {
            _renderEngine?.ShowModelName();
        }

        #endregion



        #region ------------- SCRCPY Event => Hõ Trợ UI View, ScrcpyService, SDL3Render ------------- 

        // Scrcpy (Sở Hữu)
        // Event Scrcpy - Khai báo cho View (paneldevice) rằng đã connect thành công
        private async void OnConnecting(bool obj)
        {
            _view.AddOrUpdatePhoneStatus(PhoneStatus.Connecting);
            await _view.OnSwitchInputMode();
        }

        // Scrcpy (Sở Hữu)
        // Event Scrcpy Gửi Size (Cố Định)
        // SDL3Render Tức là đã chắc chắn scrcpy connect thành công
        // ScrcpyService SetVideo (Cố Định)
        private async void OnDeviceInfo(int w, int h)
        {
            _view.InvokeOnUI(() =>
            {
                _view.ResizePDUI(w + 4, h + 4);
                _scrcpyService?.SetVideoSize(w, h);
                _renderEngine?.SetVideoSize(w, h);
                _renderEngine?.InitializeRenderer();
            });

        }

        // Scrcpy (Sở Hữu)
        // Scrcpy Gửi Frame cho SDL3Render render
        private void OnFrameReceived(AVFrame frame)
        {
            _view.InvokeOnUI(() => _renderEngine?.RenderFrame(frame));
        }


        public void SetViewSize(int w, int h)
        {
            _view.InvokeOnUI(() =>
            {
                _view.ResizePDUI(w + 4, h + 4);
            });
        }


        // IPDController (Sở Hữu)
        // Support SrcpyService + SDL3Render - Rotate Device or Xoay Chiều Phone
        public async Task RotateAsync()
        {
            if (_view.IsCanNotRotate() || _scrcpyService == null) return;
            _ = _scrcpyService.RotateDevice();
        }

        public bool IsCanNotRotate()
        {
            return _view.IsCanNotRotate();
        }

        #endregion

        #region ------------- PDController , Hàm Huỷ Mọi Tác Vụ ------------- 
        // PDController -- Hàm Huỷ Mọi Tác Vụ, Class Liên Quan Đang Chạy 
        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            _ = StopAsync();
            ScrcpyService?.SetSyncDongBoMode(false);
            _renderEngine?.Dispose();
            _scrcpy?.Dispose();
            _scrcpyService = null;
        }
        #endregion
    }
}