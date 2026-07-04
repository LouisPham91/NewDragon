using Dragon.Controller.GlobalControl.Security;
using Dragon.Database.Models;
using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace Dragon.Controller.GlobalControl.Property
{

    [JsonSerializable(typeof(SettingsProperty))]
    [JsonSourceGenerationOptions(WriteIndented = true, PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
    public partial class SettingsJsonContext : JsonSerializerContext
    {
    }

    public class SettingsProperty
    {
        public int ThemeMode { get; set; } = 0;
        public int ThemeStyle { get; set; } = 0;
        public int MaxScreenSize { get; set; } = 960;
        public int MinScreenSize { get; set; } = 256;
        public long Bitrate { get; set; } = 2000000;
        public int Fps { get; set; } = 60;
        public string FireBaseToken { get; set; } = string.Empty;
        public string FireBaseRefreshToken { get; set; } = string.Empty;
        public string UserGmail { get; set; } = string.Empty;
        // WebCanvas
        public string WebFingerPrint { get; set; } = string.Empty;
        // WebGL
        public string FingerPrint { get; set; } = string.Empty;

        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;

        public bool IsDisplayTitle { get; set; } = true;
        public bool IsDisplayModelName { get; set; } = true;      
        public bool IsDisplaySerial { get; set; } = false;
        public bool IsDisplayIP { get; set; } = false;

    }

    public static class GetSettings
    {
        private static readonly string _filePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Dragon",
            "settings.json"
        );

        private static SettingsProperty _settings = new();

        static GetSettings() => LoadSettings();

        // ==== Encode/Decode helpers ====
        private static string Enc(string plain) => string.IsNullOrEmpty(plain) ? "" : DecodeStringV1.EncodeV1(plain);
        private static string Dec(string enc)
        {
            if (string.IsNullOrEmpty(enc)) return "";
            try { return DecodeStringV1.DecodeV1(enc); }
            catch { return enc; } // nếu file cũ chưa mã hóa
        }

        private static void LoadSettings()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);
                    File.WriteAllText(_filePath, "{}");
                }

                var json = File.ReadAllText(_filePath);
                var loaded = JsonSerializer.Deserialize(json, SettingsJsonContext.Default.SettingsProperty) ?? new SettingsProperty();

                // decode tất cả string khi load
                loaded.FireBaseToken = Dec(loaded.FireBaseToken);
                loaded.FireBaseRefreshToken = Dec(loaded.FireBaseRefreshToken);
                loaded.UserGmail = Dec(loaded.UserGmail);
                loaded.WebFingerPrint = Dec(loaded.WebFingerPrint);
                loaded.FingerPrint = Dec(loaded.FingerPrint);
                loaded.AccessToken = Dec(loaded.AccessToken);
                loaded.RefreshToken = Dec(loaded.RefreshToken);


                Interlocked.Exchange(ref _settings, loaded);
            }
            catch
            {
                Interlocked.Exchange(ref _settings, new SettingsProperty());
            }
        }

        private static void SaveSettings()
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);
                var current = Volatile.Read(ref _settings);

                // tạo bản copy để encode trước khi ghi file
                var toSave = new SettingsProperty
                {
                    ThemeMode = current.ThemeMode,
                    ThemeStyle = current.ThemeStyle,
                    MaxScreenSize = current.MaxScreenSize,
                    MinScreenSize = current.MinScreenSize,
                    Bitrate = current.Bitrate,
                    Fps = current.Fps,
                    IsDisplayModelName = current.IsDisplayModelName,
                    IsDisplayTitle = current.IsDisplayTitle,
                    IsDisplaySerial = current.IsDisplaySerial,
                    IsDisplayIP = current.IsDisplayIP,
                    FireBaseToken = Enc(current.FireBaseToken),
                    FireBaseRefreshToken = Enc(current.FireBaseRefreshToken),
                    UserGmail = Enc(current.UserGmail),
                    WebFingerPrint = Enc(current.WebFingerPrint),
                    FingerPrint = Enc(current.FingerPrint),
                    AccessToken = Enc(current.AccessToken),
                    RefreshToken = Enc(current.RefreshToken),
                };

                var json = JsonSerializer.Serialize(toSave, SettingsJsonContext.Default.SettingsProperty);
                File.WriteAllText(_filePath, json);
            }
            catch (Exception)
            {
                
            }
        }

        public static int GetThemeMode() => Volatile.Read(ref _settings).ThemeMode;
        public static int GetThemeStyle() => Volatile.Read(ref _settings).ThemeStyle;
        public static int GetMaxScreenSize() => Volatile.Read(ref _settings).MaxScreenSize;
        public static int GetMinScreenSize() => Volatile.Read(ref _settings).MinScreenSize;
        public static long GetBitrate() => Volatile.Read(ref _settings).Bitrate;
        public static int GetFps() => Volatile.Read(ref _settings).Fps;
        public static bool GetDisplayModelName() => Volatile.Read(ref _settings).IsDisplayModelName;
        public static bool GetDisplayTitle() => Volatile.Read(ref _settings).IsDisplayTitle;
        public static bool GetDisplaySerial() => Volatile.Read(ref _settings).IsDisplaySerial;
        public static bool GetDisplayIP() => Volatile.Read(ref _settings).IsDisplayIP;
        // FireBase
        public static string GetFireBaseToken() => Volatile.Read(ref _settings).FireBaseToken;
        public static string GetFireBaseRefreshToken() => Volatile.Read(ref _settings).FireBaseRefreshToken;
        public static string GetUserEmail() => Volatile.Read(ref _settings).UserGmail;
        // FingerPrint
        public static string GetWebFingerPrint() => Volatile.Read(ref _settings).WebFingerPrint;
        public static string GetFingerPrint() => Volatile.Read(ref _settings).FingerPrint;
        // Token
        public static string GetAccessToken() => Volatile.Read(ref _settings).AccessToken;
        public static string GetRefreshAccessToken() => Volatile.Read(ref _settings).RefreshToken;

        public static void SetThemeMode(int value) => UpdateAndSave(s => s.ThemeMode = value);
        public static void SetThemeStyle(int value) => UpdateAndSave(s => s.ThemeStyle = value);
        public static void SetMaxScreenSize(int value) => UpdateAndSave(s => s.MaxScreenSize = value);
        public static void SetMinScreenSize(int value) => UpdateAndSave(s => s.MinScreenSize = value);
        public static void SetBitrate(long value) => UpdateAndSave(s => s.Bitrate = value);
        public static void SetFps(int value) => UpdateAndSave(s => s.Fps = value);
        public static void SetDisplayModelName(bool value) => UpdateAndSave(s => s.IsDisplayModelName = value);
        public static void SetDisplayTitle(bool value) => UpdateAndSave(s => s.IsDisplayTitle = value);
        public static void SetDisplaySerial(bool value) => UpdateAndSave(s => s.IsDisplaySerial = value);
        public static void SetDisplayIP(bool value) => UpdateAndSave(s => s.IsDisplayIP = value);

        // FireBase
        public static void SetFireBaseToken(string value) => UpdateAndSave(s => s.FireBaseToken = value);
        public static void SetFireBaseRefreshToken(string value) => UpdateAndSave(s => s.FireBaseRefreshToken = value);
        public static void SetUserEmail(string value) => UpdateAndSave(s => s.UserGmail = value);
        // FingerPrint
        public static void SetWebFingerPrint(string value) => UpdateAndSave(s => s.WebFingerPrint = value);
        public static void SetFingerPrint(string value) => UpdateAndSave(s => s.FingerPrint = value);
        // Token
        public static void SetAccessToken(string value) => UpdateAndSave(s => s.AccessToken = value);
        public static void SetRefreshAccessToken(string value) => UpdateAndSave(s => s.RefreshToken = value);


        private static void UpdateAndSave(Action<SettingsProperty> updater)
        {
            var current = Volatile.Read(ref _settings);
            var newSettings = new SettingsProperty
            {
                ThemeMode = current.ThemeMode,
                ThemeStyle = current.ThemeStyle,
                MaxScreenSize = current.MaxScreenSize,
                MinScreenSize = current.MinScreenSize,
                Bitrate = current.Bitrate,
                Fps = current.Fps,

                IsDisplayModelName = current.IsDisplayModelName,
                IsDisplayTitle = current.IsDisplayTitle,
                IsDisplaySerial = current.IsDisplaySerial,
                IsDisplayIP = current.IsDisplayIP,

                FireBaseToken = current.FireBaseToken,
                FireBaseRefreshToken = current.FireBaseRefreshToken,
                UserGmail = current.UserGmail,
                WebFingerPrint = current.WebFingerPrint,
                FingerPrint = current.FingerPrint,

                AccessToken = current.AccessToken,
                RefreshToken = current.RefreshToken,
            };

            updater(newSettings);

            Interlocked.Exchange(ref _settings, newSettings);
            SaveSettings();
        }


        public static (Size maxSize, Size minSize) getSize(Phone phone)
        {
            int maxH = GetMaxScreenSize();
            int minH = GetMinScreenSize();
            Size maxSize = ComputeVideoSize(phone.PhysicalWidth, phone.PhysicalHeight, maxH);
            var minSize = ComputePanelSize(maxSize, minH);
            return (maxSize, minSize);
        }

        public static ((int w, int h) max, (int w, int h) min, (int w, int h) running) GetALLSize(Phone phone)
        {
            int maxH = GetMaxScreenSize();   // ví dụ 1080
            int minH = GetMinScreenSize();   // ví dụ 240
            int runningH = 960;              // thêm theo yêu cầu

            // 1. size video tối đa (chia hết 8)
            var max = ComputeVideoSizeV2(phone.PhysicalWidth, phone.PhysicalHeight, maxH);

            // 2. size panel nhỏ nhất
            var min = ComputePanelSize(max.width, max.height, minH);

            // 3. size khi đang chạy (960)
            var running = ComputePanelSize(max.width, max.height, runningH);

            return (max, min, running);
        }

        public static Size ComputeVideoSize(int deviceWidth, int deviceHeight, int requestedMaxSize)
        {
            // Round down to multiple of 8
            int w = deviceWidth & ~7;
            int h = deviceHeight & ~7;
            int maxSize = requestedMaxSize & ~7;

            if (maxSize > 0)
            {
                bool portrait = h > w;
                int major = portrait ? h : w;
                int minor = portrait ? w : h;

                if (major > maxSize)
                {
                    // Integer division → floor
                    int minorExact = minor * maxSize / major;
                    // Round to nearest multiple of 8: add 4 then floor to multiple of 8
                    minor = minorExact + 4 & ~7;
                    major = maxSize;
                }

                // Assign back in portrait/landscape order
                if (portrait)
                {
                    w = minor;
                    h = major;
                }
                else
                {
                    w = major;
                    h = minor;
                }
            }

            return new Size(w, h);
        }

        public static Size ComputePanelSize(Size scrcpySize, int bound)
        {
            int w = scrcpySize.Width;
            int h = scrcpySize.Height;

            if (bound > 0)
            {
                bool portrait = h >= w;
                int major = portrait ? h : w;
                int minor = portrait ? w : h;

                if (major > bound)
                {
                    double scale = (double)bound / major;
                    major = bound;
                    minor = (int)Math.Round(minor * scale);
                }

                if (portrait)
                {
                    h = major;
                    w = minor;
                }
                else
                {
                    w = major;
                    h = minor;
                }
            }
            return new Size(w, h);
        }
        public static (int width, int height) ComputeVideoSizeV2(int deviceWidth, int deviceHeight, int requestedMaxSize)
        {
            // Round down to multiple of 8
            int w = deviceWidth & ~7;
            int h = deviceHeight & ~7;
            int maxSize = requestedMaxSize & ~7;

            if (maxSize > 0)
            {
                bool portrait = h > w;
                int major = portrait ? h : w;
                int minor = portrait ? w : h;

                if (major > maxSize)
                {
                    int minorExact = minor * maxSize / major;
                    // round to nearest multiple of 8
                    minor = (minorExact + 4) & ~7;
                    major = maxSize;
                }

                if (portrait)
                {
                    w = minor;
                    h = major;
                }
                else
                {
                    w = major;
                    h = minor;
                }
            }

            return (w, h);
        }

        public static (int width, int height) ComputePanelSize(int srcWidth, int srcHeight, int bound)
        {
            int w = srcWidth;
            int h = srcHeight;

            if (bound > 0)
            {
                bool portrait = h >= w;
                int major = portrait ? h : w;
                int minor = portrait ? w : h;

                if (major > bound)
                {
                    double scale = (double)bound / major;
                    major = bound;
                    minor = (int)Math.Round(minor * scale);
                }

                if (portrait)
                {
                    h = major;
                    w = minor;
                }
                else
                {
                    w = major;
                    h = minor;
                }
            }
            return (w, h);
        }
    }

}
