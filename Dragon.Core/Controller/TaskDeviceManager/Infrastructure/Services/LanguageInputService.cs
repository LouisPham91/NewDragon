using Dragon.Controller.Database.Services;
using Dragon.Controller.GlobalControl.Helper;
using Dragon.Database.Models;
using Dragon.Database.Services;
using System.Text;

namespace Dragon.Controller.TaskDeviceManager.Infrastructure.Services
{
    public class LanguageInputService
    {
        // Dictionary ánh xạ ký tự tiếng Việt sang cách gõ
        private static readonly Dictionary<char, string> VietDict = new Dictionary<char, string>
        {
            // Nguyên âm thường
            {'a',"a"}, {'à',"af"}, {'á',"as"}, {'ả',"ar"}, {'ã',"ax"}, {'ạ',"aj"},
            {'ă',"aw"}, {'ằ',"awf"}, {'ắ',"aws"}, {'ẳ',"awr"}, {'ẵ',"awx"}, {'ặ',"awj"},
            {'â',"aa"}, {'ầ',"aaf"}, {'ấ',"aas"}, {'ẩ',"aar"}, {'ẫ',"aax"}, {'ậ',"aaj"},
            {'e',"e"}, {'è',"ef"}, {'é',"es"}, {'ẻ',"er"}, {'ẽ',"ex"}, {'ẹ',"ej"},
            {'ê',"ee"}, {'ề',"eef"}, {'ế',"ees"}, {'ể',"eer"}, {'ễ',"eex"}, {'ệ',"eej"},
            {'i',"i"}, {'ì',"if"}, {'í',"is"}, {'ỉ',"ir"}, {'ĩ',"ix"}, {'ị',"ij"},
            {'o',"o"}, {'ò',"of"}, {'ó',"os"}, {'ỏ',"or"}, {'õ',"ox"}, {'ọ',"oj"},
            {'ô',"oo"}, {'ồ',"oof"}, {'ố',"oos"}, {'ổ',"oor"}, {'ỗ',"oox"}, {'ộ',"ooj"},
            {'ơ',"ow"}, {'ờ',"owf"}, {'ớ',"ows"}, {'ở',"owr"}, {'ỡ',"owx"}, {'ợ',"owj"},
            {'u',"u"}, {'ù',"uf"}, {'ú',"us"}, {'ủ',"ur"}, {'ũ',"ux"}, {'ụ',"uj"},
            {'ư',"uw"}, {'ừ',"uwf"}, {'ứ',"uws"}, {'ử',"uwr"}, {'ữ',"uwx"}, {'ự',"uwj"},
            {'y',"y"}, {'ỳ',"yf"}, {'ý',"ys"}, {'ỷ',"yr"}, {'ỹ',"yx"}, {'ỵ',"yj"},

            // Nguyên âm hoa
            {'A',"A"}, {'À',"Af"}, {'Á',"As"}, {'Ả',"Ar"}, {'Ã',"Ax"}, {'Ạ',"Aj"},
            {'Ă',"Aw"}, {'Ằ',"Awf"}, {'Ắ',"Aws"}, {'Ẳ',"Awr"}, {'Ẵ',"Awx"}, {'Ặ',"Awj"},
            {'Â',"Aa"}, {'Ầ',"Aaf"}, {'Ấ',"Aas"}, {'Ẩ',"Aar"}, {'Ẫ',"Aax"}, {'Ậ',"Aaj"},
            {'E',"E"}, {'È',"Ef"}, {'É',"Es"}, {'Ẻ',"Er"}, {'Ẽ',"Ex"}, {'Ẹ',"Ej"},
            {'Ê',"Ee"}, {'Ề',"Eef"}, {'Ế',"Ees"}, {'Ể',"Eer"}, {'Ễ',"Eex"}, {'Ệ',"Eej"},
            {'I',"I"}, {'Ì',"If"}, {'Í',"Is"}, {'Ỉ',"Ir"}, {'Ĩ',"Ix"}, {'Ị',"Ij"},
            {'O',"O"}, {'Ò',"Of"}, {'Ó',"Os"}, {'Ỏ',"Or"}, {'Õ',"Ox"}, {'Ọ',"Oj"},
            {'Ô',"Oo"}, {'Ồ',"Oof"}, {'Ố',"Oos"}, {'Ổ',"Oor"}, {'Ỗ',"Oox"}, {'Ộ',"Ooj"},
            {'Ơ',"Ow"}, {'Ờ',"Owf"}, {'Ớ',"Ows"}, {'Ở',"Owr"}, {'Ỡ',"Owx"}, {'Ợ',"Owj"},
            {'U',"U"}, {'Ù',"Uf"}, {'Ú',"Us"}, {'Ủ',"Ur"}, {'Ũ',"Ux"}, {'Ụ',"Uj"},
            {'Ư',"Uw"}, {'Ừ',"Uwf"}, {'Ứ',"Uws"}, {'Ử',"Uwr"}, {'Ữ',"Uwx"}, {'Ự',"Uwj"},
            {'Y',"Y"}, {'Ỳ',"Yf"}, {'Ý',"Ys"}, {'Ỷ',"Yr"}, {'Ỹ',"Yx"}, {'Ỵ',"Yj"},

            // Phụ âm thường
            {'d',"d"}, {'đ',"dd"},
            {'b',"b"}, {'c',"c"}, {'g',"g"}, {'h',"h"}, {'k',"k"}, {'l',"l"},
            {'m',"m"}, {'n',"n"}, {'p',"p"}, {'q',"q"}, {'r',"r"}, {'s',"s"},
            {'t',"t"}, {'v',"v"}, {'x',"x"},

            // Phụ âm hoa
            {'D',"D"}, {'Đ',"Dd"},
            {'B',"B"}, {'C',"C"}, {'G',"G"}, {'H',"H"}, {'K',"K"}, {'L',"L"},
            {'M',"M"}, {'N',"N"}, {'P',"P"}, {'Q',"Q"}, {'R',"R"}, {'S',"S"},
            {'T',"T"}, {'V',"V"}, {'X',"X"}
        };

        public static List<string> VietnameseChars = new List<string>
        {
            // chữ cái đặc biệt
            "đ", "Đ",

            // nguyên âm a
            "á","à","ả","ã","ạ",
            "ắ","ằ","ẳ","ẵ","ặ",
            "ấ","ầ","ẩ","ẫ","ậ",
            "Á","À","Ả","Ã","Ạ",
            "Ắ","Ằ","Ẳ","Ẵ","Ặ",
            "Ấ","Ầ","Ẩ","Ẫ","Ậ",

            // nguyên âm e
            "é","è","ẻ","ẽ","ẹ",
            "ế","ề","ể","ễ","ệ",
            "É","È","Ẻ","Ẽ","Ẹ",
            "Ế","Ề","Ể","Ễ","Ệ",

            // nguyên âm i
            "í","ì","ỉ","ĩ","ị",
            "Í","Ì","Ỉ","Ĩ","Ị",

            // nguyên âm o
            "ó","ò","ỏ","õ","ọ",
            "ố","ồ","ổ","ỗ","ộ",
            "ớ","ờ","ở","ỡ","ợ",
            "Ó","Ò","Ỏ","Õ","Ọ",
            "Ố","Ồ","Ổ","Ỗ","Ộ",
            "Ớ","Ờ","Ở","Ỡ","Ợ",

            // nguyên âm u
            "ú","ù","ủ","ũ","ụ",
            "ứ","ừ","ử","ữ","ự",
            "Ú","Ù","Ủ","Ũ","Ụ",
            "Ứ","Ừ","Ử","Ữ","Ự",

            // nguyên âm y
            "ý","ỳ","ỷ","ỹ","ỵ",
            "Ý","Ỳ","Ỷ","Ỹ","Ỵ"
        };

        public static string ConvertToTyping(string input)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char ch in input)
            {
                if (VietDict.ContainsKey(ch))
                    sb.Append(VietDict[ch]);
                else
                    sb.Append(ch); // giữ nguyên nếu không có trong dict
            }
            return sb.ToString();
        }

        public static bool IsVietnamese(string input)
        {
            return VietnameseChars.Any(ch => input.Contains(ch));
        }
        public static async Task<string?> ConvertText(string input, string deviceID, int timewait = 2000, CancellationToken token = default, string? logKey = null)
        {
            string key = logKey ?? deviceID;
            if (IsVietnamese(input))
            {
                if (!await SwitchLanguageMode(deviceID, Langeuage.Vietnamese, timewait, token, key)) { Logger.Notify(key, $"Không tìm thấy IME tiếng Việt cho {deviceID}", Logger.Icon.Warning); return null; }
                return ConvertToTyping(input);

            }
            else
            {
                if (!await SwitchLanguageMode(deviceID, Langeuage.English, timewait, token, key))
                {
                    Logger.Notify(key, $"Không tìm thấy IME tiếng Anh cho {deviceID}", Logger.Icon.Warning);
                    return null;
                }
                return input;
            }
        }

        public static async Task<string?> ConvertText(string input, string deviceID, bool isATX, int timewait = 2000, CancellationToken token = default, string? logKey = null)
        {
            string key = logKey ?? deviceID;
            if (IsVietnamese(input))
            {
                if (isATX)
                {
                    if (!await SwitchLanguageMode(deviceID, Langeuage.ATX_Unicode, timewait, token, key)) { Logger.Notify(key, $"Không tìm thấy IME tiếng Việt cho {deviceID}", Logger.Icon.Warning); return null; }
                    return input;
                }
                else
                {
                    if (!await SwitchLanguageMode(deviceID, Langeuage.Vietnamese, timewait, token, key)) { Logger.Notify(key, $"Không tìm thấy IME tiếng Việt cho {deviceID}", Logger.Icon.Warning); return null; }
                    return ConvertToTyping(input);
                }
            }
            else
            {
                if (isATX)
                {
                    if (!await SwitchLanguageMode(deviceID, Langeuage.ATX_Unicode, timewait, token, key)) { Logger.Notify(key, $"Không tìm thấy IME cho {deviceID}", Logger.Icon.Warning); return null; }
                    return input;
                }
                else if (!await SwitchLanguageMode(deviceID, Langeuage.English, timewait, token, key))
                {
                    Logger.Notify(key, $"Không tìm thấy IME tiếng Anh cho {deviceID}", Logger.Icon.Warning);
                    return null;
                }
                return input;
            }
        }


        public static async Task<bool> SwitchLanguageMode(string deviceID, Langeuage lang, int timewait = 2000, CancellationToken token = default, string? logKey = null)
        {
            string key = logKey ?? deviceID;
            if (token.IsCancellationRequested) return false;

            var setting = KeybroadSettingRepository.FindOne(deviceID, lang);
            if (setting == null)
            {
                await CheckNullKeyBroadSetting(deviceID, key);
                setting = KeybroadSettingRepository.FindOne(deviceID, lang);
                if (setting == null) return false;
            }

            var check = CMD.ExecuteAdb($"adb -s {deviceID} shell settings get secure default_input_method");
            if (string.IsNullOrEmpty(check) || !check.Contains("exit", StringComparison.OrdinalIgnoreCase)) return false;
            if (check.Contains(setting.IMEId, StringComparison.OrdinalIgnoreCase)) return true;

            var result = CMD.ExecuteAdb($"adb -s {deviceID} shell ime set {setting.IMEId}");
            if (string.IsNullOrEmpty(result) || !result.Contains("exit", StringComparison.OrdinalIgnoreCase)) return false;

            await Logger.DelayAsync(timewait, token, key);
            Logger.Notify(key, $"Đã chuyển sang IME {(lang == Langeuage.Vietnamese ? "tiếng Việt" : lang == Langeuage.English ? "tiếng Anh" : "ATX")} cho {deviceID}", Logger.Icon.Information);
            return true;
        }

        public static async Task CheckNullKeyBroadSetting(string deviceID, string? logKey = null)
        {
            string key = logKey ?? deviceID;
            try
            {
                var list = await CMD.ExecuteAdbAsync($"adb -s {deviceID} shell ime list -s");
                if (string.IsNullOrEmpty(list)) return;
                var imeis = list.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries).Where(x => x.Contains("/")).ToList();
                foreach (var imei in imeis)
                {
                    if (imei.Contains("voice", StringComparison.OrdinalIgnoreCase)) continue;
                    Langeuage lang = imei.Contains("labankey", StringComparison.OrdinalIgnoreCase) ? Langeuage.Vietnamese :
                                    imei.Contains("AdbKeyboard", StringComparison.OrdinalIgnoreCase) ? Langeuage.ATX_Unicode : Langeuage.English;
                    if (!KeybroadSettingRepository.IsAny(deviceID, imei))
                        KeybroadSettingRepository.Add(new KeybroadSetting { DeviceId = deviceID, IMEId = imei, Langeuage = lang });
                }
            }
            catch (Exception ex)
            {
                Logger.Notify(key, $"Lỗi khi kiểm tra IME cho {deviceID}: {ex.Message}", Logger.Icon.Error);
            }
        }
    }

}
