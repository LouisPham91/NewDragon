using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Dragon.Controller.DeviceControl
{
    public class ADBInit
    {
        public static void LoadADB()
        {
            var portableHome = Path.Combine(AppContext.BaseDirectory, "Extension", "adb_data");
            var androidDir = Path.Combine(portableHome, ".android");
            Directory.CreateDirectory(androidDir);

            var userAndroid = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                ".android");
            Directory.CreateDirectory(userAndroid);

            var portableKey = Path.Combine(androidDir, "adbkey");
            var portablePub = portableKey + ".pub";
            var userKey = Path.Combine(userAndroid, "adbkey");
            var userPub = userKey + ".pub";

            // 1. Nếu chưa có portable -> lấy key cũ của máy làm gốc (lần đầu dev)
            if (!File.Exists(portableKey) && File.Exists(userKey))
            {
                File.Copy(userKey, portableKey, true);
                if (File.Exists(userPub)) File.Copy(userPub, portablePub, true);
                return; // xong, portable đã có
            }

            // 2. Nếu portable đã có -> ép Users dùng chung
            if (File.Exists(portableKey))
            {
                bool needCopy = false;

                // chưa có ở Users
                if (!File.Exists(userKey)) needCopy = true;
                // có rồi nhưng khác nội dung
                else if (!FilesEqual(portableKey, userKey) || !FilesEqual(portablePub, userPub))
                    needCopy = true;

                if (needCopy)
                {
                    // backup key cũ phòng hờ
                    if (File.Exists(userKey))
                        File.Copy(userKey, userKey + ".bak_" + DateTime.Now.Ticks, true);

                    File.Copy(portableKey, userKey, true);
                    if (File.Exists(portablePub))
                        File.Copy(portablePub, userPub, true);
                }
            }
            // 3. Nếu cả 2 đều chưa có -> để adb tự tạo lần đầu (sẽ tạo ở portable nhờ env)
        }

        private static bool FilesEqual(string path1, string path2)
        {
            if (!File.Exists(path1) || !File.Exists(path2)) return false;
            using var sha = SHA256.Create();
            var h1 = sha.ComputeHash(File.ReadAllBytes(path1));
            var h2 = sha.ComputeHash(File.ReadAllBytes(path2));
            return Convert.ToBase64String(h1) == Convert.ToBase64String(h2);
        }
    }
}
