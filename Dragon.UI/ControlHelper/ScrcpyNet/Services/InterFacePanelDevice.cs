using AdvancedSharpAdbClient;
using AdvancedSharpAdbClient.Models;
using Dragon.Controller.DeviceControl.ScrcpyNet.Services;
using FFmpeg.AutoGen;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dragon.ControlHelper.ScrcpyNet.Services
{
    // ----------- PandelDevice -----------------
    public interface IPanelDeviceUI
    {
        // ---------- Lấy dữ liệu (giữ nguyên) ----------
        string DeviceID { get; }
        IntPtr RenderHandle { get; }
        (int Width, int Height) RenderPanelSize { get; }
        int BorderThickness { get; }


        // ---------- Thao tác UI ----------
        void InvokeOnUI(Action action);
        void ResizePDUI(int width, int height);
        void ScrcpySizeChanged();
        void ChangeColorALLControls();
        bool IsCanNotRotate();

        // ---------- Callback từ PDController ----------
        Task OnSwitchInputMode();
        void OnScrcpyServiceLost();
        void InitMouseRender();
        void SetIPDController(IPDController pDController);
        void AddOrUpdatePhoneStatus(PhoneStatus phoneStatus);
    }

    // ----------- PDController -----------------
    public interface IPDController
    {
        // ---------- Vòng đời ----------
        Task StartAsync();
        Task StopAsync();

        // ---------- Hành động ----------
        Task RotateAsync();

        // ---------- Phản hồi UI ----------
        void SetRenderPanelSize(int width, int height);
        void SetViewSize(int w, int h);
        bool IsCanNotRotate();
        // ---------- Truy cập service ----------
        IScrcpyService? ScrcpyService { get; }

        // ---------- Lấy dự liệu Từ PDController ----------
        DeviceData DeviceData { get; }
        Bitmap? ScreenShot();
        IAdbClient _adbClient { get; }
        bool IsConnecting { get; }
        CancellationTokenSource? cts { get; }
        // ---------- Gửi dữ liệu ----------
        // Gửi Cho IScrcpyService Thông Qua PDController
        void SetSycnDongBo(bool _isSycn);

        // Sdl3RenderrEngine
        Task ResizeVideoMaxSize();
        void ShowModelName();


    }
    // ------------ ScrcpyService ----------------------
    public interface IScrcpyService
    {
        long ClipboardSequence { get; }

        // Điều khiển
        Task SendSyncDongBoCommand(IControlMessage msg); // Đồng bộ
        Task SendControlCommand(IControlMessage msg); // Không đồng bộ
        Task RotateDevice();
        Task SetDisplayPower(bool on);
        Task StartApp(string packageName);
        Task<bool> WaitScrcpyAsync(string DeviceID, CancellationToken ct = default);
        // Clipboard
        Task<string?> GetClipboardAsync(CancellationToken ct = default);
        Task SetClipboardAsync(string text, bool paste, CancellationToken ct = default);
        Task<string?> ReceiveClipboardDataAsync(CancellationToken ct = default);
        Task<long?> ReceiveAckClipboardAsync(CancellationToken ct = default);
        Size _videoSize { get; }
        bool GetSyncDongBo();
        void SetSyncDongBoMode(bool sync);
      
        // UHID
        Task<(int id, byte[] data)?> ReceiveUhidOutputAsync(CancellationToken ct = default);

        Task RequetRealMouseFakeUHDI();
        // Trạng thái
        bool Connected { get; }
    }

}
