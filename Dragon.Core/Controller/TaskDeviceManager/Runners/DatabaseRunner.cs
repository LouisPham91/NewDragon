using Dragon.Controller.DeviceControl.Orc;
using Dragon.Controller.GlobalControl.Helper;
using Dragon.Controller.GlobalControl.TaskDeviceManager.Runners;
using Dragon.Controller.TaskDeviceManager.Core;
using Dragon.Controller.TaskDeviceManager.Infrastructure;
using Dragon.Controller.TaskDeviceManager.Model.DatabaseColumn;
using Dragon.Controller.TaskDeviceManager.Model.Mouse;
using Dragon.Database.Models;
using Dragon.Database.Services;
using OtpNet;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;


namespace Dragon.Controller.TaskDeviceManager.Runners
{
    public class DatabaseRunner
    {
        private static readonly ConcurrentDictionary<string, Regex> _regexCache = new();

        private static Regex GetCachedRegex(string pattern) =>
            _regexCache.GetOrAdd(pattern, p => new Regex(p, RegexOptions.CultureInvariant));

        public static async Task<NodeResult> RunSetDatabaseColumn(
            DLoopContext ctx,
            SetColumnDataArgs setColumnData,
            Action<Bitmap>? onPreview = null)
        {
            _regexCache.Clear(); // <-- clear cache mỗi lần chạy node để tránh memory leak nếu pattern nhiều và đa dạng
            if (ctx.Token.IsCancellationRequested) return NodeResult.Stop();

            var phone = ctx.Session.Phone;
            var adbClient = ctx.AdbClient;
            var token = ctx.Token;
            var logKey = ctx.LogKey;

            if (adbClient == null) return NodeResult.Fail("adbClient null");
            if (phone == null) return NodeResult.Fail("phone null");
            if (token.IsCancellationRequested) return NodeResult.Stop();

            string region = setColumnData.ImageRectangleRegion ?? "";
            string pattern = setColumnData.ImageRegexPattern ?? "";
            var language = setColumnData.Language;
            string tessDataPath = Path.GetFullPath(@"tessdata");
            await Logger.DelayAsync(50, token, logKey);
            try
            {
                string ocrText = "";

                // --- lấy ảnh theo mode ---
                Bitmap? source = null;
                if (setColumnData.ControlMode == ControlMode.ADB || setColumnData.ControlMode == ControlMode.ADBEvent)
                {
                    if (string.IsNullOrWhiteSpace(region))
                        return NodeResult.Fail("Region empty");
                    var fb = adbClient.GetFrameBuffer(ctx.DeviceData);
                    source = fb.ToImage();
                }
                else if (setColumnData.ControlMode is ControlMode.Scrcpy or ControlMode.HDI)
                {
                    if (ctx.Screen == null) return NodeResult.Fail("Screen null");
                    source = ctx.Screen.ScreenShotV2();
                }
                else if (setColumnData.ControlMode == ControlMode.ATX && ctx.Atx != null)
                {
                    var text = await ctx.Atx.FindNodeSaveTextsAsync(setColumnData) ?? "";
                    if (string.IsNullOrEmpty(text)) return NodeResult.Fail("ATX empty");
                    AppDataRepository.UpdateValueColumnDatabase(phone.DeviceID, setColumnData.SocialNetworkName, setColumnData.ColumnName, text);
                    Logger.Notify(logKey, $"✅ ATX đọc dữ liệu: {text}", Logger.Icon.None);
                    return NodeResult.Ok();
                }
                else if (setColumnData.ControlMode is ControlMode.ACC or ControlMode.OTG)
                {
                    return NodeResult.Fail($"ScreenShot Chưa hỗ trợ Acc và OTG");
                }

                if (source == null) return NodeResult.Fail("Bitmap null");

                using (source)
                using (var cropped = BitmapCropper.CropByString(source, region))
                {
                    if (cropped == null) return NodeResult.Fail("Crop null");

                    onPreview?.Invoke((Bitmap)cropped.Clone()); // UI tự dispose clone

                    ocrText = OrcImageHelper.GetImageTextFromBitmap(cropped, language, tessDataPath) ?? "";
                }

                // --- xử lý regex ---
                string finalResult = ocrText;
                if (!string.IsNullOrWhiteSpace(pattern))
                {
                    var regex = GetCachedRegex(pattern); // <-- cache, không new mỗi lần
                    var m = regex.Match(ocrText);
                    if (!m.Success || m.Groups.Count < 2)
                    {
                        Logger.Notify(logKey, $"❌ Regex không khớp. Pattern: {pattern}", Logger.Icon.Warning);
                        return NodeResult.Fail("Regex no match");
                    }
                    finalResult = m.Groups[1].Value;
                }

                AppDataRepository.UpdateValueColumnDatabase(phone.DeviceID, setColumnData.SocialNetworkName, setColumnData.ColumnName, finalResult);
                Logger.Notify(logKey, $"✅ OCR {setColumnData.ControlMode}: {finalResult}", Logger.Icon.None);
                return NodeResult.Ok();
            }
            catch (Exception ex)
            {
                Logger.Notify(logKey, $"❌ Lỗi: {ex.Message}", Logger.Icon.Error);
                return NodeResult.Fail(ex.Message);
            }
        }


        public static async Task<NodeResult> RunGetDatabaseColumn(DLoopContext ctx, GetColumnDataArgs? getColumnDatabase)
        {
            var phone = ctx.Session.Phone;
            var token = ctx.Token;
            var logKey = ctx.LogKey;

            if (getColumnDatabase == null)
            {
                Logger.Notify(logKey, "❌ getColumnDatabase rỗng. Dừng.", Logger.Icon.Error);
                return NodeResult.Fail("getColumnDatabase null");
            }
            if (phone == null)
            {
                Logger.Notify(logKey, "❌ phone rỗng. Dừng.", Logger.Icon.Error);
                return NodeResult.Fail("phone null");
            }
            if (token.IsCancellationRequested)
            {
                Logger.Notify(logKey, $"⚠️ Tác vụ đã bị huỷ trước khi bắt đầu. Device: {phone.DeviceID}", Logger.Icon.Warning);
                return NodeResult.Stop();
            }

            try
            {

                var writeAction = getColumnDatabase.WriteAction;
                if (writeAction == WriteAction.None)
                {
                    Logger.Notify(logKey, $"⚠️ WriteAction = None. Device: {phone.DeviceID}. Dừng.", Logger.Icon.Warning);
                    return NodeResult.Fail("WriteAction None");
                }

                var columnName = getColumnDatabase.ColumnName ?? string.Empty;
                var socialNetworkName = getColumnDatabase.SocialNetworkName ?? string.Empty;
                string textToWrite;

                if (writeAction == WriteAction.Column)
                {
                    textToWrite = AppDataRepository.GetColumnDatabaseText(phone.DeviceID, socialNetworkName, columnName) ?? string.Empty;
                    if (string.IsNullOrEmpty(textToWrite))
                    {
                        Logger.Notify(logKey, $"❌ Không tìm thấy giá trị cột. Device: {phone.DeviceID}; Network: {socialNetworkName}; Column: {columnName}.", Logger.Icon.Error);
                        return NodeResult.Fail("Column not found");
                    }
                    Logger.Notify(logKey, $"ℹ️ Lấy giá trị cột thành công. Device: {phone.DeviceID}; Column: {columnName}; Length: {textToWrite.Length}.", Logger.Icon.Information);
                }
                else // OTP
                {
                    if (string.IsNullOrEmpty(columnName))
                    {
                        Logger.Notify(logKey, $"❌ Column name required for OTP. Device: {phone.DeviceID}", Logger.Icon.Error);
                        return NodeResult.Fail("OTP column missing");
                    }
                    var secret = AppDataRepository.GetColumnDatabaseText(phone.DeviceID, socialNetworkName, columnName);
                    if (string.IsNullOrEmpty(secret))
                    {
                        Logger.Notify(logKey, $"❌ OTP secret not found. Device: {phone.DeviceID}; Column: {columnName}.", Logger.Icon.Error);
                        return NodeResult.Fail("OTP secret not found");
                    }
                    try
                    {
                        var bytes = Base32Encoding.ToBytes(secret);
                        var totp = new Totp(bytes);
                        textToWrite = totp.ComputeTotp();
                    }
                    catch (Exception ex)
                    {
                        Logger.Notify(logKey, $"❌ Lỗi khi sinh OTP: {ex.Message}. Device: {phone.DeviceID}.", Logger.Icon.Error);
                        Debug.WriteLine(ex);
                        return NodeResult.Fail(ex.Message);
                    }
                }

                getColumnDatabase.Text = textToWrite;
                Logger.Notify(logKey, $"ℹ️ Chuẩn bị gửi text. Device: {phone.DeviceID}; Mode: {getColumnDatabase.ControlMode}; Length: {textToWrite.Length}.", Logger.Icon.Information);

                NodeResult sendResult;
                try
                {
                    sendResult = await InputRunner.Run_InputText_GetColumnDatabase(ctx, getColumnDatabase, delayChangeIMEIID: 1000);
                }
                catch (OperationCanceledException)
                {
                    Logger.Notify(logKey, $"⚠️ Gửi text bị huỷ. Device: {phone.DeviceID}.", Logger.Icon.Warning);
                    return NodeResult.Stop();
                }
                catch (Exception ex)
                {
                    Logger.Notify(logKey, $"❌ Lỗi khi gọi hàm gửi text: {ex.Message}. Device: {phone.DeviceID}.", Logger.Icon.Error);
                    Debug.WriteLine(ex);
                    return NodeResult.Fail(ex.Message);
                }

                if (sendResult.Status == ExecutionStatus.Continue)
                {
                    Logger.Notify(logKey, $"✅ Gửi text thành công. Device: {phone.DeviceID}; Mode: {getColumnDatabase.ControlMode}.", Logger.Icon.None);
                    return NodeResult.Ok();
                }
                else
                {
                    Logger.Notify(logKey, $"❌ Gửi text thất bại. Device: {phone.DeviceID}; Mode: {getColumnDatabase.ControlMode}.", Logger.Icon.Error);
                    return sendResult;
                }
            }
            catch (OperationCanceledException)
            {
                Logger.Notify(logKey, $"⚠️ Tác vụ bị huỷ. Device: {phone?.DeviceID ?? "Unknown"}.", Logger.Icon.Warning);
                return NodeResult.Stop();
            }
            catch (Exception ex)
            {
                Logger.Notify(logKey, $"❌ Lỗi không mong muốn: {ex.Message}. Device: {phone?.DeviceID ?? "Unknown"}.", Logger.Icon.Error);
                Debug.WriteLine(ex);
                return NodeResult.Fail(ex.Message);
            }
        }


    }
}
