using AdvancedSharpAdbClient;
using Dragon.Controller.Database.Services;
using Dragon.Controller.DeviceControl.HATX;
using Dragon.Controller.TaskDeviceManager.Core;
using Dragon.Controller.TaskDeviceManager.Model.App;
using Dragon.Controller.TaskDeviceManager.Model.DatabaseColumn;
using Dragon.Controller.TaskDeviceManager.Model.Emoji;
using Dragon.Controller.TaskDeviceManager.Model.File;
using Dragon.Controller.TaskDeviceManager.Model.HttpResponse;
using Dragon.Controller.TaskDeviceManager.Model.Input;
using Dragon.Controller.TaskDeviceManager.Model.Mouse;
using Dragon.Controller.TaskDeviceManager.Model.Vision;
using Dragon.Database.Models;
using System.Collections.Concurrent;

namespace Dragon.Controller.TaskDeviceManager.Infrastructure
{
    public class DragonPhoneManager
    {
        private static readonly Lazy<DragonPhoneManager> _inst = new(() => new());
        public static DragonPhoneManager Instance => _inst.Value;

        private readonly PhoneFarmHost _host = new();
        private readonly ConcurrentDictionary<string, PhoneSession> _sessions = new();


        // Tạo session từ DeviceManager (panel) + DB
        private PhoneSession? BuildSession(string deviceId)
        {
            var phone = PhoneRepository.FindOneByDeviceID(deviceId);
            if (phone == null)
            {
                Logger.Notify(deviceId, "Không tìm thấy Phone trong DB", Logger.Icon.Error);
                return null;
            }

            if (phone.IsRunning)
            {
                Logger.Notify(deviceId, "Phone đang được sử dụng bởi một task khác", Logger.Icon.Error);
                return null;
            }

            var adb = new AdbClient();
            var dd = adb.GetDevices().FirstOrDefault(d => d.Serial == deviceId || d.Serial == phone.Ipv4);
            if (dd == null)
            {
                Logger.Notify(deviceId, "Không tìm thấy DeviceData", Logger.Icon.Error);
                return null;
            }

            //var atx = panel._atxController; // nếu có
            return new PhoneSession(phone, adb, dd, null!);
        }

        // 1. KHI CHẠY → update IsRunning = true
        public async Task StartAsync(string deviceId, DLoop task)
        {
            Stop(deviceId); // đảm bảo không chạy trùng
            await Task.Delay(100);

            var session = BuildSession(deviceId);
            if (session == null) return;

            var require = AnalyzeRequirements(task, session.Phone);
            if (!require.IsCanRunDloop())
            {
                Logger.Notify(deviceId, "Task yêu cầu các công cụ hỗ trợ mà thiết bị không có", Logger.Icon.Error);
                return;
            }

            if (session.DeviceData == null)
            {
                Logger.Notify(deviceId, "Không lấy được DeviceData", Logger.Icon.Error);
                return;
            }

            if (require.NeedATX)
            {
                Logger.Notify(deviceId, "Đang khởi Tạo ATX.. Xin vui lòng chờ chút", Logger.Icon.Information);
                session.Atx = await AtxDevice.CreateAsync(session.DeviceData.Serial, false, session.Cts.Token);
                Logger.Notify(deviceId, "Đã khởi tạo ATX theo yêu cầu task", Logger.Icon.Information);
            }

            // THAY ĐỔI 1: kiểm tra Scrcpy bằng interface
            if (require.NeedScrcpy && (session.Input == null || session.Screen == null))
            {
                Logger.Notify(deviceId, "Task yêu cầu Scrcpy nhưng không lấy được Input/Screen", Logger.Icon.Error);
                return;
            }

            // THAY ĐỔI 2: kiểm tra UHID bằng interface (sửa luôn lỗi logic || cũ)
            if (require.NeedUHID && session.InputUhid == null)
            {
                Logger.Notify(deviceId, "Task yêu cầu UHDI nhưng không lấy được InputUhid", Logger.Icon.Error);
                return;
            }

            // update DB
            session.Phone.IsRunning = true;
            PhoneRepository.Update(session.Phone);

            _sessions[deviceId] = session;

            _ = Task.Run(async () =>
            {
                try
                {
                    var result = await _host.ExecuteAsync(task, session);
                    Logger.Notify(deviceId, $"Task kết thúc: {result.Status}", Logger.Icon.Information);
                }
                finally
                {
                    Stop(deviceId);
                }
            });
        }

        // 2. KHI STOP → xóa session + IsRunning = false
        public void Stop(string deviceId)
        {
            if (_sessions.TryRemove(deviceId, out var session))
            {
                try { session.Cts.Cancel(); } catch { }

                // update DB
                var phone = session.Phone;
                if (phone != null)
                {
                    phone.IsRunning = false;

                    var deviceList = AdbClient.Instance.GetDevices().ToList();
                    var device = deviceList.SingleOrDefault(d => d.Serial == deviceId || d.Serial == phone.Ipv4);
                    if (device != null) phone.DeviceState = device.State;
                    phone.IsOnline(deviceList);
                    PhoneRepository.Update(phone);
                }

                Logger.Notify(deviceId, "Đã dừng và reset IsRunning", Logger.Icon.Warning);
            }
        }

        public async Task RestartAsync(string deviceId, DLoop task)
        {
            Stop(deviceId);
            await Task.Delay(500);
            await StartAsync(deviceId, task);
        }

        public bool IsRunning(string deviceId) => _sessions.ContainsKey(deviceId);


        private DLoopRequirements AnalyzeRequirements(DLoop root, Phone phone)
        {
            var req = new DLoopRequirements();
            void Walk(DLoop node)
            {
                if (!node.Enabled) return;

                switch (node.Type)
                {
                    case NodeType.AppArgs:
                        var appArgs = node.GetArgs<AppArgs>();
                        req.GetRequirement_AppArgs(appArgs.ControlMode);
                        break;

                    case NodeType.GetColumnDataArgs:
                        var columnArgs = node.GetArgs<GetColumnDataArgs>();
                        req.GetRequirement_GetColumnDataArgs(columnArgs.ControlMode);
                        break;
                    case NodeType.SetColumnDataArgs:
                        var setColumnData = node.GetArgs<SetColumnDataArgs>();
                        req.GetRequirement_SetColumnDataArgs(setColumnData.ControlMode);
                        break;

                    case NodeType.EmojiArgs:
                        var emojiArgs = node.GetArgs<EmojiArgs>();
                        req.GetRequirement_Mouse(emojiArgs.ControlMode);
                        break;

                    case NodeType.FileArgs:
                        var fileArgs = node.GetArgs<FileArgs>();
                        req.GetRequirement_FileArgs(fileArgs.ControlMode);
                        break;

                    case NodeType.HttpRequestConfig:
                        var httpArgs = node.GetArgs<HttpRequestConfig>();
                        req.GetRequirement_SendTextArgs(httpArgs.ControlMode);
                        break;
                    case NodeType.ImeActionArgs:
                        var imeArgs = node.GetArgs<ImeActionArgs>();
                        req.GetRequirement_SendTextArgs(imeArgs.ControlMode);
                        break;

                    case NodeType.SendTextArgs:
                        var sendTextArgs = node.GetArgs<SendTextArgs>();
                        req.GetRequirement_SendTextArgs(sendTextArgs.ControlMode);
                        break;

                    case NodeType.KeyPressArgs:
                        var keyArgs = node.GetArgs<KeyPressArgs>();
                        req.GetRequirement_KeyPress(keyArgs.ControlMode);
                        break;

                    case NodeType.Click:
                        var clickArgs = node.GetArgs<ClickArg>();
                        req.GetRequirement_Mouse(clickArgs.ControlMode);
                        break;
                    case NodeType.Swipe:
                        var swipeArgs = node.GetArgs<SwipeArg>();
                        req.GetRequirement_Mouse(swipeArgs.ControlMode);
                        break;
                    case NodeType.LongPress:
                        var longPressArgs = node.GetArgs<LongPressArg>();
                        req.GetRequirement_Mouse(longPressArgs.ControlMode);
                        break;
                    case NodeType.DragDrop:
                        var dragDropArgs = node.GetArgs<DragArg>();
                        req.GetRequirement_Mouse(dragDropArgs.ControlMode);
                        break;

                    case NodeType.VisionScanArgs:
                        var vision = node.GetArgs<VisionScanArgs>();
                        req.GetRequirement_Mouse(vision.ControlMode);
                        break;
                }

                foreach (var child in node.Children) Walk(child);
            }

            Walk(root);
            return req;
        }
    }
}
