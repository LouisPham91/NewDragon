using System;
using System.Collections.Generic;
using Dragon.Database;
using Dragon.Database.Models;
using Microsoft.Data.Sqlite;

namespace Dragon.Database.Services
{
    public static class AppDataRepository
    {
        public static void CreateIndexOperation()
        {
            using var connection = AOTSqliteDb.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "CREATE INDEX IF NOT EXISTS IX_AppData_DeviceID ON AppDatas(DeviceID); CREATE UNIQUE INDEX IF NOT EXISTS IX_AppData_Device_Network ON AppDatas(DeviceID, NetworkName);";
            command.ExecuteNonQuery();
        }

        private static AppData ReadAppData(SqliteDataReader reader)
        {
            return new AppData
            {
                guId = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                DeviceID = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                NetworkName = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                PackageName = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                DeviceModel = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                AccStatus = (AccStatus)(reader.IsDBNull(5) ? 0 : reader.GetInt32(5)),
                Email = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                PassEmail = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                Username = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                Password = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                PrivateKey = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                Phone = reader.IsDBNull(11) ? string.Empty : reader.GetString(11),
                HoTen = reader.IsDBNull(12) ? string.Empty : reader.GetString(12),
                BirtDay = reader.IsDBNull(13) ? string.Empty : reader.GetString(13),
                Gender = (Gender)(reader.IsDBNull(14) ? 0 : reader.GetInt32(14)),
                PrimaryEducation = reader.IsDBNull(15) ? string.Empty : reader.GetString(15),
                SecondaryEducation = reader.IsDBNull(16) ? string.Empty : reader.GetString(16),
                Marital = (Marital)(reader.IsDBNull(17) ? 0 : reader.GetInt32(17)),
                Token = reader.IsDBNull(18) ? string.Empty : reader.GetString(18),
                Cookies = reader.IsDBNull(19) ? string.Empty : reader.GetString(19),
                FriendCount = reader.IsDBNull(20) ? 0 : reader.GetInt32(20),
                GroupCount = reader.IsDBNull(21) ? 0 : reader.GetInt32(21),
                FollowCount = reader.IsDBNull(22) ? 0 : reader.GetInt32(22),
                Avatar = reader.IsDBNull(23) ? string.Empty : reader.GetString(23),
                DataGroup = reader.IsDBNull(24) ? string.Empty : reader.GetString(24),
                DataFriend = reader.IsDBNull(25) ? string.Empty : reader.GetString(25),
                DataFollow = reader.IsDBNull(26) ? string.Empty : reader.GetString(26),
                AppVersion = reader.IsDBNull(27) ? string.Empty : reader.GetString(27),
                ABI = reader.IsDBNull(28) ? string.Empty : reader.GetString(28),
                NetworkGuId = reader.IsDBNull(29) ? string.Empty : reader.GetString(29)
            };
        }

        public static List<AppData> GetAll()
        {
            var list = new List<AppData>();
            using var connection = AOTSqliteDb.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT guId, DeviceID, NetworkName, PackageName, DeviceModel, AccStatus, Email, PassEmail, Username, Password, PrivateKey, Phone, HoTen, BirtDay, Gender, PrimaryEducation, SecondaryEducation, Marital, Token, Cookies, FriendCount, GroupCount, FollowCount, Avatar, DataGroup, DataFriend, DataFollow, AppVersion, ABI, NetworkGuId FROM AppDatas ORDER BY DeviceID, NetworkName";
            using var reader = command.ExecuteReader();
            while (reader.Read()) list.Add(ReadAppData(reader));
            return list;
        }

        public static AppData? GetById(string guId)
        {
            using var connection = AOTSqliteDb.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT guId, DeviceID, NetworkName, PackageName, DeviceModel, AccStatus, Email, PassEmail, Username, Password, PrivateKey, Phone, HoTen, BirtDay, Gender, PrimaryEducation, SecondaryEducation, Marital, Token, Cookies, FriendCount, GroupCount, FollowCount, Avatar, DataGroup, DataFriend, DataFollow, AppVersion, ABI, NetworkGuId FROM AppDatas WHERE guId = @guId LIMIT 1";
            command.Parameters.AddWithValue("@guId", guId);
            using var reader = command.ExecuteReader();
            return reader.Read() ? ReadAppData(reader) : null;
        }

        public static List<AppData> GetByDeviceId(string deviceId)
        {
            var list = new List<AppData>();
            using var connection = AOTSqliteDb.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT guId, DeviceID, NetworkName, PackageName, DeviceModel, AccStatus, Email, PassEmail, Username, Password, PrivateKey, Phone, HoTen, BirtDay, Gender, PrimaryEducation, SecondaryEducation, Marital, Token, Cookies, FriendCount, GroupCount, FollowCount, Avatar, DataGroup, DataFriend, DataFollow, AppVersion, ABI, NetworkGuId FROM AppDatas WHERE DeviceID = @DeviceID";
            command.Parameters.AddWithValue("@DeviceID", deviceId);
            using var reader = command.ExecuteReader();
            while (reader.Read()) list.Add(ReadAppData(reader));
            return list;
        }

        public static List<AppData> GetByNetworkName(string networkName)
        {
            var list = new List<AppData>();
            using var connection = AOTSqliteDb.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT guId, DeviceID, NetworkName, PackageName, DeviceModel, AccStatus, Email, PassEmail, Username, Password, PrivateKey, Phone, HoTen, BirtDay, Gender, PrimaryEducation, SecondaryEducation, Marital, Token, Cookies, FriendCount, GroupCount, FollowCount, Avatar, DataGroup, DataFriend, DataFollow, AppVersion, ABI, NetworkGuId FROM AppDatas WHERE NetworkName = @NetworkName";
            command.Parameters.AddWithValue("@NetworkName", networkName);
            using var reader = command.ExecuteReader();
            while (reader.Read()) list.Add(ReadAppData(reader));
            return list;
        }

        public static AppData? GetByEmailAndNetworkName(string email, string networkName)
        {
            using var connection = AOTSqliteDb.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT guId, DeviceID, NetworkName, PackageName, DeviceModel, AccStatus, Email, PassEmail, Username, Password, PrivateKey, Phone, HoTen, BirtDay, Gender, PrimaryEducation, SecondaryEducation, Marital, Token, Cookies, FriendCount, GroupCount, FollowCount, Avatar, DataGroup, DataFriend, DataFollow, AppVersion, ABI, NetworkGuId FROM AppDatas WHERE Email = @Email AND NetworkName = @NetworkName LIMIT 1";
            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@NetworkName", networkName);
            using var reader = command.ExecuteReader();
            return reader.Read() ? ReadAppData(reader) : null;
        }

        public static bool Add(AppData appData)
        {
            if (string.IsNullOrWhiteSpace(appData.DeviceID) || string.IsNullOrWhiteSpace(appData.NetworkName))
                return false;
            if (string.IsNullOrWhiteSpace(appData.guId))
                appData.guId = Guid.NewGuid().ToString();

            using var connection = AOTSqliteDb.Open();
            using var command = connection.CreateCommand();
            command.CommandText = @"INSERT OR IGNORE INTO AppDatas (guId, DeviceID, NetworkName, PackageName, DeviceModel, AccStatus, Email, PassEmail, Username, Password, PrivateKey, Phone, HoTen, BirtDay, Gender, PrimaryEducation, SecondaryEducation, Marital, Token, Cookies, FriendCount, GroupCount, FollowCount, Avatar, DataGroup, DataFriend, DataFollow, AppVersion, ABI, NetworkGuId)
VALUES (@guId, @DeviceID, @NetworkName, @PackageName, @DeviceModel, @AccStatus, @Email, @PassEmail, @Username, @Password, @PrivateKey, @Phone, @HoTen, @BirtDay, @Gender, @PrimaryEducation, @SecondaryEducation, @Marital, @Token, @Cookies, @FriendCount, @GroupCount, @FollowCount, @Avatar, @DataGroup, @DataFriend, @DataFollow, @AppVersion, @ABI, @NetworkGuId)";
            AddParameters(command, appData);
            return command.ExecuteNonQuery() > 0;
        }

        public static bool AddRange(List<AppData> appDatas)
        {
            using var connection = AOTSqliteDb.Open();
            using var transaction = connection.BeginTransaction();
            try
            {
                foreach (var appData in appDatas)
                {
                    if (string.IsNullOrWhiteSpace(appData.guId))
                        appData.guId = Guid.NewGuid().ToString();
                    using var command = connection.CreateCommand();
                    command.Transaction = transaction;
                    command.CommandText = @"INSERT OR IGNORE INTO AppDatas (guId, DeviceID, NetworkName, PackageName, DeviceModel, AccStatus, Email, PassEmail, Username, Password, PrivateKey, Phone, HoTen, BirtDay, Gender, PrimaryEducation, SecondaryEducation, Marital, Token, Cookies, FriendCount, GroupCount, FollowCount, Avatar, DataGroup, DataFriend, DataFollow, AppVersion, ABI, NetworkGuId)
VALUES (@guId, @DeviceID, @NetworkName, @PackageName, @DeviceModel, @AccStatus, @Email, @PassEmail, @Username, @Password, @PrivateKey, @Phone, @HoTen, @BirtDay, @Gender, @PrimaryEducation, @SecondaryEducation, @Marital, @Token, @Cookies, @FriendCount, @GroupCount, @FollowCount, @Avatar, @DataGroup, @DataFriend, @DataFollow, @AppVersion, @ABI, @NetworkGuId)";
                    AddParameters(command, appData);
                    command.ExecuteNonQuery();
                }
                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public static bool Update(AppData appData)
        {
            using var connection = AOTSqliteDb.Open();
            using var command = connection.CreateCommand();
            command.CommandText = @"UPDATE AppDatas SET DeviceID=@DeviceID, NetworkName=@NetworkName, PackageName=@PackageName, DeviceModel=@DeviceModel, AccStatus=@AccStatus, Email=@Email, PassEmail=@PassEmail, Username=@Username, Password=@Password, PrivateKey=@PrivateKey, Phone=@Phone, HoTen=@HoTen, BirtDay=@BirtDay, Gender=@Gender, PrimaryEducation=@PrimaryEducation, SecondaryEducation=@SecondaryEducation, Marital=@Marital, Token=@Token, Cookies=@Cookies, FriendCount=@FriendCount, GroupCount=@GroupCount, FollowCount=@FollowCount, Avatar=@Avatar, DataGroup=@DataGroup, DataFriend=@DataFriend, DataFollow=@DataFollow, AppVersion=@AppVersion, ABI=@ABI, NetworkGuId=@NetworkGuId WHERE guId=@guId";
            AddParameters(command, appData);
            return command.ExecuteNonQuery() > 0;
        }

        private static void AddParameters(SqliteCommand command, AppData appData)
        {
            command.Parameters.AddWithValue("@guId", appData.guId);
            command.Parameters.AddWithValue("@DeviceID", appData.DeviceID ?? string.Empty);
            command.Parameters.AddWithValue("@NetworkName", appData.NetworkName ?? string.Empty);
            command.Parameters.AddWithValue("@PackageName", appData.PackageName ?? string.Empty);
            command.Parameters.AddWithValue("@DeviceModel", appData.DeviceModel ?? string.Empty);
            command.Parameters.AddWithValue("@AccStatus", (int)appData.AccStatus);
            command.Parameters.AddWithValue("@Email", appData.Email ?? string.Empty);
            command.Parameters.AddWithValue("@PassEmail", appData.PassEmail ?? string.Empty);
            command.Parameters.AddWithValue("@Username", appData.Username ?? string.Empty);
            command.Parameters.AddWithValue("@Password", appData.Password ?? string.Empty);
            command.Parameters.AddWithValue("@PrivateKey", appData.PrivateKey ?? string.Empty);
            command.Parameters.AddWithValue("@Phone", appData.Phone ?? string.Empty);
            command.Parameters.AddWithValue("@HoTen", appData.HoTen ?? string.Empty);
            command.Parameters.AddWithValue("@BirtDay", appData.BirtDay ?? string.Empty);
            command.Parameters.AddWithValue("@Gender", (int)appData.Gender);
            command.Parameters.AddWithValue("@PrimaryEducation", appData.PrimaryEducation ?? string.Empty);
            command.Parameters.AddWithValue("@SecondaryEducation", appData.SecondaryEducation ?? string.Empty);
            command.Parameters.AddWithValue("@Marital", (int)appData.Marital);
            command.Parameters.AddWithValue("@Token", appData.Token ?? string.Empty);
            command.Parameters.AddWithValue("@Cookies", appData.Cookies ?? string.Empty);
            command.Parameters.AddWithValue("@FriendCount", appData.FriendCount);
            command.Parameters.AddWithValue("@GroupCount", appData.GroupCount);
            command.Parameters.AddWithValue("@FollowCount", appData.FollowCount);
            command.Parameters.AddWithValue("@Avatar", appData.Avatar ?? string.Empty);
            command.Parameters.AddWithValue("@DataGroup", appData.DataGroup ?? string.Empty);
            command.Parameters.AddWithValue("@DataFriend", appData.DataFriend ?? string.Empty);
            command.Parameters.AddWithValue("@DataFollow", appData.DataFollow ?? string.Empty);
            command.Parameters.AddWithValue("@AppVersion", appData.AppVersion ?? string.Empty);
            command.Parameters.AddWithValue("@ABI", appData.ABI ?? string.Empty);
            command.Parameters.AddWithValue("@NetworkGuId", appData.NetworkGuId ?? string.Empty);
        }

        public static bool DeleteWithBackup(string guId)
        {
            var appData = GetById(guId);
            if (appData == null) return false;
            
            using var connection = AOTSqliteDb.Open();
            using var transaction = connection.BeginTransaction();
            try
            {
                // Backup
                using (var backupCmd = connection.CreateCommand())
                {
                    backupCmd.Transaction = transaction;
                    backupCmd.CommandText = @"INSERT OR REPLACE INTO AppDataBackups (guId, DeviceID, NetworkName, PackageName, Email, PassEmail, Username, Password, PrivateKey, Phone, HoTen, BirtDay, Gender, PrimaryEducation, SecondaryEducation, Marital, Token, Cookies, FriendCount, GroupCount, FollowCount, Avatar, DataGroup, DataFriend, DataFollow, AppVersion, ABI, NetworkGuId)
VALUES (@guId, @DeviceID, @NetworkName, @PackageName, @Email, @PassEmail, @Username, @Password, @PrivateKey, @Phone, @HoTen, @BirtDay, @Gender, @PrimaryEducation, @SecondaryEducation, @Marital, @Token, @Cookies, @FriendCount, @GroupCount, @FollowCount, @Avatar, @DataGroup, @DataFriend, @DataFollow, @AppVersion, @ABI, @NetworkGuId)";
                    backupCmd.Parameters.AddWithValue("@guId", appData.guId);
                    backupCmd.Parameters.AddWithValue("@DeviceID", appData.DeviceID);
                    backupCmd.Parameters.AddWithValue("@NetworkName", appData.NetworkName);
                    backupCmd.Parameters.AddWithValue("@PackageName", appData.PackageName);
                    backupCmd.Parameters.AddWithValue("@Email", appData.Email);
                    backupCmd.Parameters.AddWithValue("@PassEmail", appData.PassEmail);
                    backupCmd.Parameters.AddWithValue("@Username", appData.Username);
                    backupCmd.Parameters.AddWithValue("@Password", appData.Password);
                    backupCmd.Parameters.AddWithValue("@PrivateKey", appData.PrivateKey);
                    backupCmd.Parameters.AddWithValue("@Phone", appData.Phone);
                    backupCmd.Parameters.AddWithValue("@HoTen", appData.HoTen);
                    backupCmd.Parameters.AddWithValue("@BirtDay", appData.BirtDay);
                    backupCmd.Parameters.AddWithValue("@Gender", (int)appData.Gender);
                    backupCmd.Parameters.AddWithValue("@PrimaryEducation", appData.PrimaryEducation);
                    backupCmd.Parameters.AddWithValue("@SecondaryEducation", appData.SecondaryEducation);
                    backupCmd.Parameters.AddWithValue("@Marital", (int)appData.Marital);
                    backupCmd.Parameters.AddWithValue("@Token", appData.Token);
                    backupCmd.Parameters.AddWithValue("@Cookies", appData.Cookies);
                    backupCmd.Parameters.AddWithValue("@FriendCount", appData.FriendCount);
                    backupCmd.Parameters.AddWithValue("@GroupCount", appData.GroupCount);
                    backupCmd.Parameters.AddWithValue("@FollowCount", appData.FollowCount);
                    backupCmd.Parameters.AddWithValue("@Avatar", appData.Avatar);
                    backupCmd.Parameters.AddWithValue("@DataGroup", appData.DataGroup);
                    backupCmd.Parameters.AddWithValue("@DataFriend", appData.DataFriend);
                    backupCmd.Parameters.AddWithValue("@DataFollow", appData.DataFollow);
                    backupCmd.Parameters.AddWithValue("@AppVersion", appData.AppVersion);
                    backupCmd.Parameters.AddWithValue("@ABI", appData.ABI);
                    backupCmd.Parameters.AddWithValue("@NetworkGuId", appData.NetworkGuId);
                    backupCmd.ExecuteNonQuery();
                }

                using (var deleteCmd = connection.CreateCommand())
                {
                    deleteCmd.Transaction = transaction;
                    deleteCmd.CommandText = "DELETE FROM AppDatas WHERE guId = @guId";
                    deleteCmd.Parameters.AddWithValue("@guId", guId);
                    deleteCmd.ExecuteNonQuery();
                }

                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public static bool Delete(string guId)
        {
            using var connection = AOTSqliteDb.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM AppDatas WHERE guId = @guId";
            command.Parameters.AddWithValue("@guId", guId);
            return command.ExecuteNonQuery() > 0;
        }

        public static bool DeleteByDeviceAndNetwork(string deviceId, string networkName)
        {
            using var connection = AOTSqliteDb.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM AppDatas WHERE DeviceID = @DeviceID AND NetworkName = @NetworkName";
            command.Parameters.AddWithValue("@DeviceID", deviceId);
            command.Parameters.AddWithValue("@NetworkName", networkName);
            return command.ExecuteNonQuery() > 0;
        }

        public static bool Exists(string guId)
        {
            using var connection = AOTSqliteDb.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(1) FROM AppDatas WHERE guId = @guId";
            command.Parameters.AddWithValue("@guId", guId);
            return Convert.ToInt32(command.ExecuteScalar()) > 0;
        }

        public static bool ExistsByEmailAndNetworkName(string email, string networkName)
        {
            using var connection = AOTSqliteDb.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(1) FROM AppDatas WHERE Email = @Email AND NetworkName = @NetworkName";
            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@NetworkName", networkName);
            return Convert.ToInt32(command.ExecuteScalar()) > 0;
        }

        public static string FindPrivateKey(string deviceId, string networkName)
        {
            using var connection = AOTSqliteDb.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT PrivateKey FROM AppDatas WHERE DeviceID = @DeviceID AND NetworkName = @NetworkName LIMIT 1";
            command.Parameters.AddWithValue("@DeviceID", deviceId);
            command.Parameters.AddWithValue("@NetworkName", networkName);
            var result = command.ExecuteScalar();
            return result?.ToString() ?? string.Empty;
        }

        public static string FindColumnValue(string deviceId, string networkName, string columnName)
        {
            // Không dùng reflection để tương thích Native AOT - dùng switch
            string column = columnName.ToLowerInvariant() switch
            {
                "guid" => "guId",
                "deviceid" => "DeviceID",
                "networkname" => "NetworkName",
                "packagename" => "PackageName",
                "email" => "Email",
                "username" => "Username",
                "privatekey" => "PrivateKey",
                "phone" => "Phone",
                "hoten" => "HoTen",
                "token" => "Token",
                _ => string.Empty
            };
            if (string.IsNullOrEmpty(column)) return string.Empty;

            using var connection = AOTSqliteDb.Open();
            using var command = connection.CreateCommand();
            command.CommandText = $"SELECT {column} FROM AppDatas WHERE DeviceID = @DeviceID AND NetworkName = @NetworkName LIMIT 1";
            command.Parameters.AddWithValue("@DeviceID", deviceId);
            command.Parameters.AddWithValue("@NetworkName", networkName);
            var result = command.ExecuteScalar();
            return result?.ToString() ?? string.Empty;
        }

        public static int CountByPackage(string packageName)
        {
            using var connection = AOTSqliteDb.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(1) FROM AppDatas WHERE PackageName = @PackageName";
            command.Parameters.AddWithValue("@PackageName", packageName);
            return Convert.ToInt32(command.ExecuteScalar());
        }

        public static (List<AppData> Items, int TotalCount) GetPaged(int page, int pageSize, string? packageName = null)
        {
            var items = new List<AppData>();
            using var connection = AOTSqliteDb.Open();
            
            int totalCount;
            using (var countCmd = connection.CreateCommand())
            {
                if (string.IsNullOrEmpty(packageName))
                    countCmd.CommandText = "SELECT COUNT(1) FROM AppDatas";
                else
                {
                    countCmd.CommandText = "SELECT COUNT(1) FROM AppDatas WHERE PackageName = @PackageName";
                    countCmd.Parameters.AddWithValue("@PackageName", packageName);
                }
                totalCount = Convert.ToInt32(countCmd.ExecuteScalar());
            }

            using var command = connection.CreateCommand();
            if (string.IsNullOrEmpty(packageName))
            {
                command.CommandText = "SELECT guId, DeviceID, NetworkName, PackageName, DeviceModel, AccStatus, Email, PassEmail, Username, Password, PrivateKey, Phone, HoTen, BirtDay, Gender, PrimaryEducation, SecondaryEducation, Marital, Token, Cookies, FriendCount, GroupCount, FollowCount, Avatar, DataGroup, DataFriend, DataFollow, AppVersion, ABI, NetworkGuId FROM AppDatas ORDER BY BirtDay DESC LIMIT @Limit OFFSET @Offset";
            }
            else
            {
                command.CommandText = "SELECT guId, DeviceID, NetworkName, PackageName, DeviceModel, AccStatus, Email, PassEmail, Username, Password, PrivateKey, Phone, HoTen, BirtDay, Gender, PrimaryEducation, SecondaryEducation, Marital, Token, Cookies, FriendCount, GroupCount, FollowCount, Avatar, DataGroup, DataFriend, DataFollow, AppVersion, ABI, NetworkGuId FROM AppDatas WHERE PackageName = @PackageName ORDER BY BirtDay DESC LIMIT @Limit OFFSET @Offset";
                command.Parameters.AddWithValue("@PackageName", packageName);
            }
            command.Parameters.AddWithValue("@Limit", pageSize);
            command.Parameters.AddWithValue("@Offset", (page - 1) * pageSize);
            
            using var reader = command.ExecuteReader();
            while (reader.Read()) items.Add(ReadAppData(reader));
            
            return (items, totalCount);
        }

        public static bool UpdateValueColumnDatabase(string deviceID, string socialNetwork, string column, string value)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(deviceID) || string.IsNullOrWhiteSpace(socialNetwork) || string.IsNullOrWhiteSpace(column))
                    return false;

                // Chuẩn hóa và whitelist cột
                var key = column.Trim().ToLowerInvariant();
                (string ColumnName, Type ColumnType) columnInfo = key switch
                {
                    "guid" => ("guId", typeof(string)),
                    "deviceid" => ("DeviceID", typeof(string)),
                    "networkname" => ("NetworkName", typeof(string)),
                    "packagename" => ("PackageName", typeof(string)),
                    "devicemodel" => ("DeviceModel", typeof(string)),
                    "accstatus" => ("AccStatus", typeof(int)),
                    "email" => ("Email", typeof(string)),
                    "passemail" => ("PassEmail", typeof(string)),
                    "username" => ("Username", typeof(string)),
                    "password" => ("Password", typeof(string)),
                    "privatekey" => ("PrivateKey", typeof(string)),
                    "phone" => ("Phone", typeof(string)),
                    "hoten" => ("HoTen", typeof(string)),
                    "birtday" => ("BirtDay", typeof(string)),
                    "gender" => ("Gender", typeof(int)),
                    "primaryeducation" => ("PrimaryEducation", typeof(string)),
                    "secondaryeducation" => ("SecondaryEducation", typeof(string)),
                    "marital" => ("Marital", typeof(int)),
                    "token" => ("Token", typeof(string)),
                    "cookies" => ("Cookies", typeof(string)),
                    "friendcount" => ("FriendCount", typeof(int)),
                    "groupcount" => ("GroupCount", typeof(int)),
                    "followcount" => ("FollowCount", typeof(int)),
                    "avatar" => ("Avatar", typeof(string)),
                    "datagroup" => ("DataGroup", typeof(string)),
                    "datafriend" => ("DataFriend", typeof(string)),
                    "datafollow" => ("DataFollow", typeof(string)),
                    "appversion" => ("AppVersion", typeof(string)),
                    "abi" => ("ABI", typeof(string)),
                    "networkguid" => ("NetworkGuId", typeof(string)),
                    _ => (string.Empty, typeof(object))
                };

                if (string.IsNullOrEmpty(columnInfo.ColumnName))
                    return false;

                using var connection = AOTSqliteDb.Open();
                using var command = connection.CreateCommand();
                command.CommandText = $"UPDATE AppDatas SET {columnInfo.ColumnName} = @Value WHERE DeviceID = @DeviceID AND NetworkName = @NetworkName";
                command.Parameters.AddWithValue("@DeviceID", deviceID);
                command.Parameters.AddWithValue("@NetworkName", socialNetwork);

                object paramValue;

                if (string.IsNullOrEmpty(value))
                {
                    // Quyết định: dùng chuỗi rỗng cho cột string để tránh NULL mismatch
                    if (columnInfo.ColumnType == typeof(string))
                        paramValue = string.Empty;
                    else
                        return false; // không thể set null cho int/enum bằng chuỗi rỗng
                }
                else
                {
                    if (columnInfo.ColumnType == typeof(string))
                    {
                        paramValue = value;
                    }
                    else if (columnInfo.ColumnType == typeof(int))
                    {
                        // Xử lý enum hoặc số nguyên
                        if (columnInfo.ColumnName == "AccStatus")
                        {
                            if (int.TryParse(value, out var intVal))
                                paramValue = intVal;
                            else if (Enum.TryParse<AccStatus>(value, true, out var enumVal))
                                paramValue = (int)enumVal;
                            else
                                return false;
                        }
                        else if (columnInfo.ColumnName == "Gender")
                        {
                            if (int.TryParse(value, out var intVal))
                                paramValue = intVal;
                            else if (Enum.TryParse<Gender>(value, true, out var enumVal))
                                paramValue = (int)enumVal;
                            else
                                return false;
                        }
                        else if (columnInfo.ColumnName == "Marital")
                        {
                            if (int.TryParse(value, out var intVal))
                                paramValue = intVal;
                            else if (Enum.TryParse<Marital>(value, true, out var enumVal))
                                paramValue = (int)enumVal;
                            else
                                return false;
                        }
                        else
                        {
                            if (!int.TryParse(value, out var intVal)) return false;
                            paramValue = intVal;
                        }
                    }
                    else
                    {
                        // fallback: treat as string
                        paramValue = value;
                    }
                }

                command.Parameters.AddWithValue("@Value", paramValue ?? DBNull.Value);
                return command.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error UpdateValueColumnDatabase: {ex.Message}");
                return false;
            }
        }

        public static string GetColumnDatabaseText(string deviceID, string socialNetwork, string column)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(deviceID) || string.IsNullOrWhiteSpace(socialNetwork) || string.IsNullOrWhiteSpace(column))
                    return string.Empty;

                var key = column.Trim().ToLowerInvariant();
                var dbColumn = key switch
                {
                    "guid" => "guId",
                    "deviceid" => "DeviceID",
                    "networkname" => "NetworkName",
                    "packagename" => "PackageName",
                    "devicemodel" => "DeviceModel",
                    "accstatus" => "AccStatus",
                    "email" => "Email",
                    "passemail" => "PassEmail",
                    "username" => "Username",
                    "password" => "Password",
                    "privatekey" => "PrivateKey",
                    "phone" => "Phone",
                    "hoten" => "HoTen",
                    "birtday" => "BirtDay",
                    "gender" => "Gender",
                    "primaryeducation" => "PrimaryEducation",
                    "secondaryeducation" => "SecondaryEducation",
                    "marital" => "Marital",
                    "token" => "Token",
                    "cookies" => "Cookies",
                    "friendcount" => "FriendCount",
                    "groupcount" => "GroupCount",
                    "followcount" => "FollowCount",
                    "avatar" => "Avatar",
                    "datagroup" => "DataGroup",
                    "datafriend" => "DataFriend",
                    "datafollow" => "DataFollow",
                    "appversion" => "AppVersion",
                    "abi" => "ABI",
                    "networkguid" => "NetworkGuId",
                    _ => string.Empty
                };

                if (string.IsNullOrEmpty(dbColumn))
                    return string.Empty;

                using var connection = AOTSqliteDb.Open();
                using var command = connection.CreateCommand();
                command.CommandText = $"SELECT {dbColumn} FROM AppDatas WHERE DeviceID = @DeviceID AND NetworkName = @NetworkName LIMIT 1";
                command.Parameters.AddWithValue("@DeviceID", deviceID);
                command.Parameters.AddWithValue("@NetworkName", socialNetwork);
                var result = command.ExecuteScalar();
                if (result == null || result == DBNull.Value)
                    return string.Empty;

                // Format enum values thành tên
                if (dbColumn == "AccStatus" && int.TryParse(result.ToString(), out var acc))
                    return ((AccStatus)acc).ToString();
                if (dbColumn == "Gender" && int.TryParse(result.ToString(), out var gen))
                    return ((Gender)gen).ToString();
                if (dbColumn == "Marital" && int.TryParse(result.ToString(), out var mar))
                    return ((Marital)mar).ToString();

                return result.ToString() ?? string.Empty;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error GetColumnDatabaseText: {ex.Message}");
                return string.Empty;
            }
        }

    }
}
