using AdvancedSharpAdbClient;
using AdvancedSharpAdbClient.Models;
using Dragon.Controller.Controller.DeviceControl;
using Dragon.Controller.Database.Services;
using Dragon.Controller.DeviceControl;
using Dragon.Controller.DeviceControl.HATX;
using Dragon.Controller.DeviceControl.OTG;
using Dragon.Controller.TaskDeviceManager.Core;
using Dragon.Controller.TaskDeviceManager.Model.Mouse;
using Dragon.Controller.TaskDeviceManager.Model.Vision;
using Dragon.Database.Models;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace Dragon.Controller.TaskDeviceManager.Infrastructure
{
    public class DragonPhoneManager
    {
        private static readonly Lazy<DragonPhoneManager> _inst = new(() => new());
        public static DragonPhoneManager Instance => _inst.Value;

        private readonly PhoneFarmHost _host = new();
        private readonly ConcurrentDictionary<string, PhoneSession> _sessions = new();

        // 1. Tạo session DỰA VÀO root.ControlMode
        private async Task<PhoneSession?> BuildSessionAsync(Phone phone, DLoop root)
        {
            var adb = new AdbClient();
            DeviceData? dd;

            // ưu tiên wifi nếu phone đang ở wifi mode
            if (phone.IsWifimode())
                dd = adb.GetDevices().FirstOrDefault(d => d.Serial.Contains(phone.Ipv4));
            else
                dd = adb.GetDevices().FirstOrDefault(d => d.Serial == phone.DeviceID);

            if (dd == null)
            {
                Logger.Notify(phone.DeviceID, "Không tìm thấy DeviceData", Logger.Icon.Error);
                return null;
            }

            var session = new PhoneSession(phone, adb, dd);

            // khởi tạo đúng 1 loại input theo root
            switch (root.ControlMode)
            {
                case ControlMode.ATX:
                    Logger.Notify(phone.DeviceID, "Khởi tạo ATX...", Logger.Icon.Information);
                    session.Atx = await AtxDevice.CreateAsync(dd.Serial, false, session.Cts.Token);
                    break;

                case ControlMode.Scrcpy:
                    // giả sử bạn có factory scrcpy
                    var scrcpy = await ScrcpyServiceFactory.CreateAsync(dd.Serial);
                    session.Input = scrcpy.Input;
                    session.Screen = scrcpy.Screen;
                    break;

                case ControlMode.HDI:
                    session.InputUhid = await UhidServiceFactory.CreateAsync(dd.Serial);
                    break;

                case ControlMode.OTG:
                    session.Aoa = await AoaDeviceManager.Instance.StartByDeviceIdAsync(phone.DeviceID);
                    if (session.Aoa == null)
                    {
                        Logger.Notify(phone.DeviceID, "Không khởi tạo được AOA", Logger.Icon.Error);
                        return null;
                    }
                    break;

                case ControlMode.ACC:
                    // chưa hỗ trợ
                    Logger.Notify(phone.DeviceID, "ACC chưa hỗ trợ", Logger.Icon.Error);
                    return null;

                    // ADB và ADBEvent không cần khởi tạo thêm
            }

            // nếu là wifi thì chuẩn bị ScreenShotApp cho Vision
            if (phone.IsWifimode())
                ScreenShotApp.Instance.Add(phone);

            return session;
        }

        public async Task StartAsync(string deviceId, DLoop dLoop)
        {
            Stop(deviceId);
            await Task.Delay(100);

            var phone = PhoneRepository.FindOneByDeviceID(deviceId);
            if (phone == null) { Logger.Notify(deviceId, "Không tìm thấy Phone", Logger.Icon.Error); return; }
            if (phone.IsRunning) { Logger.Notify(deviceId, "Phone đang chạy task khác", Logger.Icon.Error); return; }

            // --- DÙNG THẲNG MODE TỪ DLOOP GỐC ---
            if (dLoop.ControlMode == default) dLoop.ControlMode = ControlMode.ADB;
            if (dLoop.VisionMode == default) dLoop.VisionMode = VisionMode.ByAtxNode;

            // đẩy xuống toàn bộ cây
            dLoop.Hydrate();

            var session = await BuildSessionAsync(phone, dLoop);
            if (session == null) return;

            phone.IsRunning = true;
            PhoneRepository.Update(phone);
            _sessions[deviceId] = session;

            _ = Task.Run(async () =>
            {
                try
                {
                    var result = await _host.ExecuteAsync(dLoop, session);
                    Logger.Notify(deviceId, $"Task kết thúc: {result.Status}", Logger.Icon.Information);
                }
                finally
                {
                    Stop(deviceId);
                }
            });
        }

        public void Stop(string deviceId)
        {
            if (_sessions.TryRemove(deviceId, out var session))
            {
                try { session.Cts.Cancel(); } catch { }
                try { ScreenShotApp.Instance.Remove(session.Phone); } catch { }
                try { session.Aoa?.Dispose(); } catch { }

                var phone = session.Phone;
                phone.IsRunning = false;

                var dev = AdbClient.Instance.GetDevices().FirstOrDefault(d => d.Serial == deviceId || d.Serial == phone.Ipv4);
                if (dev != null) phone.DeviceState = dev.State;
                phone.IsOnline(AdbClient.Instance.GetDevices());

                PhoneRepository.Update(phone);
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
    }
}