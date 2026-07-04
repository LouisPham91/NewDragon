using AdvancedSharpAdbClient;
using Dragon.Controller.Database.Services;
using Dragon.Controller.GlobalControl.Helper;
using System.Diagnostics;

namespace Dragon.ControlHelper.ScrcpyNet.EventHandle
{
    public class DragDropEventHandlers : IDisposable
    {
        public string DeviceID { get; }
        private readonly Control panel;
        private bool _disposed = false;

        public DragDropEventHandlers(string deviceID, Control panel)
        {
            this.panel = panel;
            DeviceID = deviceID;
            panel.DragEnter += DragEnter;
            panel.DragDrop += DragDrop;
        }

        private void DragEnter(object? sender, DragEventArgs e)
        {
            if (e.Data?.GetDataPresent(DataFormats.FileDrop) == true)
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        public string? GetSerial()
        {
            var phone = PhoneRepository.FindOneByDeviceID(DeviceID);
            var devicedatas = AdbClient.Instance.GetDevices();
            var device = devicedatas.FirstOrDefault(c => c.Serial == phone?.Serial);
            if (device != null) return device.Serial;

            return null;
        }
        private void DragDrop(object? sender, DragEventArgs e)
        {
            var serial = GetSerial();
            if (string.IsNullOrEmpty(serial)) return;

            if (e.Data?.GetDataPresent(DataFormats.FileDrop) == true)
            {

                string[]? files = e.Data.GetData(DataFormats.FileDrop) as string[];
                if (files == null || files.Length == 0)
                {
                    return; // No files were dragged and dropped
                }
                
                Task.Run(() =>
                {

                    foreach (string file in files)
                    {
                        if (FileHelper.IsApkFile(file))
                        {
                            var ketqua = CMD.ExecuteAdb($"adb -s {serial} install \"{file}\"", 20000);
                            Debug.WriteLine($"Install APk : {file} , Kết Quả : {ketqua}");
                        }
                        else if (FileHelper.IsImageFile(file))
                        {
                            var kqFolder = CMD.ExecuteAdb($"adb -s {serial} shell mkdir -p /storage/emulated/0/Pictures", 20000);
                            Debug.WriteLine($"Tao Folder Pictures : {file} , Kết Quả : {kqFolder}");
                            var kqFile = CMD.ExecuteAdb($"adb -s {serial} push \"{file}\" /storage/emulated/0/Pictures", 20000);
                            Debug.WriteLine($"Push File vào Máy : {file} , Kết Quả : {kqFile}");
                        }
                        else if (FileHelper.IsMediaFile(file))
                        {
                            CMD.ExecuteAdb($"adb -s {serial} shell mkdir -p /storage/emulated/0/Videos", 20000);
                            CMD.ExecuteAdb($"adb -s {serial} push \"{file}\" /storage/emulated/0/Videos", 20000);
                        }
                        else if (FileHelper.IsValidFile(file))
                        {
                            CMD.ExecuteAdb($"adb -s {serial} push \"{file}\" /storage/emulated/0/Download", 20000);
                        }
                        else
                        {
                            Debug.WriteLine($"File không hợp lệ hoặc không được hỗ trợ: {file}");
                        }

                    }

                });

            }

        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            // gỡ event để tránh leak
            panel.DragEnter -= DragEnter;
            panel.DragDrop -= DragDrop;
        }
    }
}