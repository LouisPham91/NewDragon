using System;
using System.Collections.Generic;
using System.Diagnostics;
using Dragon.Database;
using Dragon.Database.Models;
using Microsoft.Data.Sqlite;

namespace Dragon.Database.Services
{
    public static class BackUpAppDataRepository
    {
        #region Backup Methods

        public static bool BackupAppData(AppData appData, bool overwrite = false)
        {
            try
            {
                using var connection = AOTSqliteDb.Open();

                // Check existing
                string? existingGuId = null;
                using (var checkCmd = connection.CreateCommand())
                {
                    checkCmd.CommandText = "SELECT guId FROM AppDataBackups WHERE DeviceID = @DeviceID AND NetworkName = @NetworkName LIMIT 1";
                    checkCmd.Parameters.AddWithValue("@DeviceID", appData.DeviceID);
                    checkCmd.Parameters.AddWithValue("@NetworkName", appData.NetworkName);
                    var result = checkCmd.ExecuteScalar();
                    existingGuId = result?.ToString();
                }

                if (!string.IsNullOrEmpty(existingGuId))
                {
                    if (!overwrite) return true;

                    using var updateCmd = connection.CreateCommand();
                    updateCmd.CommandText = @"UPDATE AppDataBackups SET PackageName=@PackageName, Email=@Email, PassEmail=@PassEmail, Username=@Username, Password=@Password, PrivateKey=@PrivateKey, Phone=@Phone, HoTen=@HoTen, BirtDay=@BirtDay, Gender=@Gender, PrimaryEducation=@PrimaryEducation, SecondaryEducation=@SecondaryEducation, Marital=@Marital, Token=@Token, Cookies=@Cookies, FriendCount=@FriendCount, GroupCount=@GroupCount, FollowCount=@FollowCount, Avatar=@Avatar, DataGroup=@DataGroup, DataFriend=@DataFriend, DataFollow=@DataFollow, AppVersion=@AppVersion, ABI=@ABI, NetworkGuId=@NetworkGuId WHERE guId=@guId";
                    AddBackupParameters(updateCmd, appData);
                    updateCmd.Parameters.AddWithValue("@guId", existingGuId);
                    updateCmd.ExecuteNonQuery();
                }
                else
                {
                    using var insertCmd = connection.CreateCommand();
                    insertCmd.CommandText = @"INSERT INTO AppDataBackups (guId, DeviceID, NetworkName, PackageName, Email, PassEmail, Username, Password, PrivateKey, Phone, HoTen, BirtDay, Gender, PrimaryEducation, SecondaryEducation, Marital, Token, Cookies, FriendCount, GroupCount, FollowCount, Avatar, DataGroup, DataFriend, DataFollow, AppVersion, ABI, NetworkGuId) VALUES (@guId, @DeviceID, @NetworkName, @PackageName, @Email, @PassEmail, @Username, @Password, @PrivateKey, @Phone, @HoTen, @BirtDay, @Gender, @PrimaryEducation, @SecondaryEducation, @Marital, @Token, @Cookies, @FriendCount, @GroupCount, @FollowCount, @Avatar, @DataGroup, @DataFriend, @DataFollow, @AppVersion, @ABI, @NetworkGuId)";
                    insertCmd.Parameters.AddWithValue("@guId", Guid.NewGuid().ToString());
                    AddBackupParameters(insertCmd, appData);
                    insertCmd.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error backing up AppData: {ex.Message}");
                return false;
            }
        }

        public static bool BackupAppDataRange(IEnumerable<AppData> appDatas, bool overwrite = false)
        {
            try
            {
                using var connection = AOTSqliteDb.Open();
                using var transaction = connection.BeginTransaction();

                foreach (var appData in appDatas)
                {
                    string? existingGuId = null;
                    using (var checkCmd = connection.CreateCommand())
                    {
                        checkCmd.Transaction = transaction;
                        checkCmd.CommandText = "SELECT guId FROM AppDataBackups WHERE DeviceID = @DeviceID AND NetworkName = @NetworkName LIMIT 1";
                        checkCmd.Parameters.AddWithValue("@DeviceID", appData.DeviceID);
                        checkCmd.Parameters.AddWithValue("@NetworkName", appData.NetworkName);
                        var result = checkCmd.ExecuteScalar();
                        existingGuId = result?.ToString();
                    }

                    if (!string.IsNullOrEmpty(existingGuId))
                    {
                        if (overwrite)
                        {
                            using var updateCmd = connection.CreateCommand();
                            updateCmd.Transaction = transaction;
                            updateCmd.CommandText = @"UPDATE AppDataBackups SET PackageName=@PackageName, Email=@Email, PassEmail=@PassEmail, Username=@Username, Password=@Password, PrivateKey=@PrivateKey, Phone=@Phone, HoTen=@HoTen, BirtDay=@BirtDay, Gender=@Gender, PrimaryEducation=@PrimaryEducation, SecondaryEducation=@SecondaryEducation, Marital=@Marital, Token=@Token, Cookies=@Cookies, FriendCount=@FriendCount, GroupCount=@GroupCount, FollowCount=@FollowCount, Avatar=@Avatar, DataGroup=@DataGroup, DataFriend=@DataFriend, DataFollow=@DataFollow, AppVersion=@AppVersion, ABI=@ABI, NetworkGuId=@NetworkGuId WHERE guId=@guId";
                            AddBackupParameters(updateCmd, appData);
                            updateCmd.Parameters.AddWithValue("@guId", existingGuId);
                            updateCmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        using var insertCmd = connection.CreateCommand();
                        insertCmd.Transaction = transaction;
                        insertCmd.CommandText = @"INSERT INTO AppDataBackups (guId, DeviceID, NetworkName, PackageName, Email, PassEmail, Username, Password, PrivateKey, Phone, HoTen, BirtDay, Gender, PrimaryEducation, SecondaryEducation, Marital, Token, Cookies, FriendCount, GroupCount, FollowCount, Avatar, DataGroup, DataFriend, DataFollow, AppVersion, ABI, NetworkGuId) VALUES (@guId, @DeviceID, @NetworkName, @PackageName, @Email, @PassEmail, @Username, @Password, @PrivateKey, @Phone, @HoTen, @BirtDay, @Gender, @PrimaryEducation, @SecondaryEducation, @Marital, @Token, @Cookies, @FriendCount, @GroupCount, @FollowCount, @Avatar, @DataGroup, @DataFriend, @DataFollow, @AppVersion, @ABI, @NetworkGuId)";
                        insertCmd.Parameters.AddWithValue("@guId", Guid.NewGuid().ToString());
                        AddBackupParameters(insertCmd, appData);
                        insertCmd.ExecuteNonQuery();
                    }
                }

                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error backing up AppData range: {ex.Message}");
                return false;
            }
        }

        public static bool BackupSocialNetwork(SocialNetwork socialNetwork, bool overwrite = false)
        {
            try
            {
                using var connection = AOTSqliteDb.Open();

                string? existingGuId = null;
                using (var checkCmd = connection.CreateCommand())
                {
                    checkCmd.CommandText = "SELECT guId FROM SocialNetworkBackups WHERE NetworkName = @NetworkName LIMIT 1";
                    checkCmd.Parameters.AddWithValue("@NetworkName", socialNetwork.NetworkName);
                    var result = checkCmd.ExecuteScalar();
                    existingGuId = result?.ToString();
                }

                if (!string.IsNullOrEmpty(existingGuId))
                {
                    if (!overwrite) return true;
                    // No fields to update except NetworkName which is same
                    return true;
                }
                else
                {
                    using var insertCmd = connection.CreateCommand();
                    insertCmd.CommandText = "INSERT INTO SocialNetworkBackups (guId, NetworkName) VALUES (@guId, @NetworkName)";
                    insertCmd.Parameters.AddWithValue("@guId", Guid.NewGuid().ToString());
                    insertCmd.Parameters.AddWithValue("@NetworkName", socialNetwork.NetworkName);
                    insertCmd.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error backing up SocialNetwork: {ex.Message}");
                return false;
            }
        }

        #endregion

        #region Restore Methods

        public static bool RestoreAppData(string backupGuId, bool overwrite = false)
        {
            try
            {
                using var connection = AOTSqliteDb.Open();

                AppDataBackup? backup = null;
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT guId, DeviceID, NetworkName, PackageName, Email, PassEmail, Username, Password, PrivateKey, Phone, HoTen, BirtDay, Gender, PrimaryEducation, SecondaryEducation, Marital, Token, Cookies, FriendCount, GroupCount, FollowCount, Avatar, DataGroup, DataFriend, DataFollow, AppVersion, ABI, NetworkGuId FROM AppDataBackups WHERE guId = @guId LIMIT 1";
                    cmd.Parameters.AddWithValue("@guId", backupGuId);
                    using var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        backup = ReadBackup(reader);
                    }
                }
                if (backup == null) return false;

                // Check existing
                string? existingGuId = null;
                using (var checkCmd = connection.CreateCommand())
                {
                    checkCmd.CommandText = "SELECT guId FROM AppDatas WHERE DeviceID = @DeviceID AND NetworkName = @NetworkName LIMIT 1";
                    checkCmd.Parameters.AddWithValue("@DeviceID", backup.DeviceID);
                    checkCmd.Parameters.AddWithValue("@NetworkName", backup.NetworkName);
                    var result = checkCmd.ExecuteScalar();
                    existingGuId = result?.ToString();
                }

                if (!string.IsNullOrEmpty(existingGuId))
                {
                    if (!overwrite) return false;
                    using var updateCmd = connection.CreateCommand();
                    updateCmd.CommandText = @"UPDATE AppDatas SET PackageName=@PackageName, Email=@Email, PassEmail=@PassEmail, Username=@Username, Password=@Password, PrivateKey=@PrivateKey, Phone=@Phone, HoTen=@HoTen, BirtDay=@BirtDay, Gender=@Gender, PrimaryEducation=@PrimaryEducation, SecondaryEducation=@SecondaryEducation, Marital=@Marital, Token=@Token, Cookies=@Cookies, FriendCount=@FriendCount, GroupCount=@GroupCount, FollowCount=@FollowCount, Avatar=@Avatar, DataGroup=@DataGroup, DataFriend=@DataFriend, DataFollow=@DataFollow, AppVersion=@AppVersion, ABI=@ABI, NetworkGuId=@NetworkGuId WHERE guId=@guId";
                    AddRestoreParameters(updateCmd, backup);
                    updateCmd.Parameters.AddWithValue("@guId", existingGuId);
                    updateCmd.ExecuteNonQuery();
                }
                else
                {
                    using var insertCmd = connection.CreateCommand();
                    insertCmd.CommandText = @"INSERT INTO AppDatas (guId, DeviceID, NetworkName, PackageName, DeviceModel, AccStatus, Email, PassEmail, Username, Password, PrivateKey, Phone, HoTen, BirtDay, Gender, PrimaryEducation, SecondaryEducation, Marital, Token, Cookies, FriendCount, GroupCount, FollowCount, Avatar, DataGroup, DataFriend, DataFollow, AppVersion, ABI, NetworkGuId) VALUES (@guId, @DeviceID, @NetworkName, @PackageName, '', 0, @Email, @PassEmail, @Username, @Password, @PrivateKey, @Phone, @HoTen, @BirtDay, @Gender, @PrimaryEducation, @SecondaryEducation, @Marital, @Token, @Cookies, @FriendCount, @GroupCount, @FollowCount, @Avatar, @DataGroup, @DataFriend, @DataFollow, @AppVersion, @ABI, @NetworkGuId)";
                    insertCmd.Parameters.AddWithValue("@guId", Guid.NewGuid().ToString());
                    AddRestoreParameters(insertCmd, backup);
                    insertCmd.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error restoring AppData: {ex.Message}");
                return false;
            }
        }

        public static bool RestoreAppDataByDeviceAndNetwork(string deviceId, string networkName, bool overwrite = false)
        {
            try
            {
                using var connection = AOTSqliteDb.Open();
                string? backupGuId = null;
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT guId FROM AppDataBackups WHERE DeviceID = @DeviceID AND NetworkName = @NetworkName LIMIT 1";
                    cmd.Parameters.AddWithValue("@DeviceID", deviceId);
                    cmd.Parameters.AddWithValue("@NetworkName", networkName);
                    var result = cmd.ExecuteScalar();
                    backupGuId = result?.ToString();
                }
                if (backupGuId == null) return false;
                return RestoreAppData(backupGuId, overwrite);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error restoring AppData by device: {ex.Message}");
                return false;
            }
        }

        public static int RestoreAllAppData(bool overwrite = false)
        {
            int count = 0;
            try
            {
                using var connection = AOTSqliteDb.Open();
                var backupIds = new List<string>();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT guId FROM AppDataBackups";
                    using var reader = cmd.ExecuteReader();
                    while (reader.Read()) backupIds.Add(reader.GetString(0));
                }
                foreach (var id in backupIds)
                {
                    if (RestoreAppData(id, overwrite)) count++;
                }
                return count;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error restoring all AppData: {ex.Message}");
                return 0;
            }
        }

        public static bool RestoreSocialNetwork(string backupGuId, bool overwrite = false)
        {
            try
            {
                using var connection = AOTSqliteDb.Open();
                string? networkName = null;
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT NetworkName FROM SocialNetworkBackups WHERE guId = @guId LIMIT 1";
                    cmd.Parameters.AddWithValue("@guId", backupGuId);
                    var result = cmd.ExecuteScalar();
                    networkName = result?.ToString();
                }
                if (networkName == null) return false;

                using var checkCmd = connection.CreateCommand();
                checkCmd.CommandText = "SELECT COUNT(1) FROM SocialNetworks WHERE NetworkName = @NetworkName";
                checkCmd.Parameters.AddWithValue("@NetworkName", networkName);
                var exists = Convert.ToInt32(checkCmd.ExecuteScalar()) > 0;
                if (exists && !overwrite) return false;
                if (exists && overwrite) return true;

                using var insertCmd = connection.CreateCommand();
                insertCmd.CommandText = "INSERT INTO SocialNetworks (guId, NetworkName) VALUES (@guId, @NetworkName)";
                insertCmd.Parameters.AddWithValue("@guId", Guid.NewGuid().ToString());
                insertCmd.Parameters.AddWithValue("@NetworkName", networkName);
                insertCmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error restoring SocialNetwork: {ex.Message}");
                return false;
            }
        }

        #endregion

        #region Delete with Auto-Backup

        public static bool DeleteAppDataWithBackup(string guId, bool overwriteBackup = false)
        {
            try
            {
                using var connection = AOTSqliteDb.Open();
                AppData? appData = null;
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT guId, DeviceID, NetworkName, PackageName, DeviceModel, AccStatus, Email, PassEmail, Username, Password, PrivateKey, Phone, HoTen, BirtDay, Gender, PrimaryEducation, SecondaryEducation, Marital, Token, Cookies, FriendCount, GroupCount, FollowCount, Avatar, DataGroup, DataFriend, DataFollow, AppVersion, ABI, NetworkGuId FROM AppDatas WHERE guId = @guId LIMIT 1";
                    cmd.Parameters.AddWithValue("@guId", guId);
                    using var reader = cmd.ExecuteReader();
                    if (reader.Read()) appData = AppDataRepository_GetAppData(reader);
                }
                if (appData == null) return false;

                BackupAppData(appData, overwriteBackup);

                using var deleteCmd = connection.CreateCommand();
                deleteCmd.CommandText = "DELETE FROM AppDatas WHERE guId = @guId";
                deleteCmd.Parameters.AddWithValue("@guId", guId);
                deleteCmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error delete with backup: {ex.Message}");
                return false;
            }
        }

        public static bool DeleteAppDataWithBackup(string deviceId, string networkName, bool overwriteBackup = false)
        {
            try
            {
                using var connection = AOTSqliteDb.Open();
                string? guId = null;
                AppData? appData = null;
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT guId, DeviceID, NetworkName, PackageName, DeviceModel, AccStatus, Email, PassEmail, Username, Password, PrivateKey, Phone, HoTen, BirtDay, Gender, PrimaryEducation, SecondaryEducation, Marital, Token, Cookies, FriendCount, GroupCount, FollowCount, Avatar, DataGroup, DataFriend, DataFollow, AppVersion, ABI, NetworkGuId FROM AppDatas WHERE DeviceID = @DeviceID AND NetworkName = @NetworkName LIMIT 1";
                    cmd.Parameters.AddWithValue("@DeviceID", deviceId);
                    cmd.Parameters.AddWithValue("@NetworkName", networkName);
                    using var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        appData = AppDataRepository_GetAppData(reader);
                        guId = appData.guId;
                    }
                }
                if (appData == null || guId == null) return false;
                BackupAppData(appData, overwriteBackup);
                using var deleteCmd = connection.CreateCommand();
                deleteCmd.CommandText = "DELETE FROM AppDatas WHERE guId = @guId";
                deleteCmd.Parameters.AddWithValue("@guId", guId);
                deleteCmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error delete with backup: {ex.Message}");
                return false;
            }
        }

        #endregion



        public static bool RestoreAppDataRange(IEnumerable<string> backupGuIds, bool overwrite = false)
        {
            try
            {
                int success = 0;
                foreach (var id in backupGuIds)
                {
                    if (RestoreAppData(id, overwrite)) success++;
                }
                return success > 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error restoring AppData range: {ex.Message}");
                return false;
            }
        }

        public static bool RestoreSocialNetworkByName(string networkName, bool overwrite = false)
        {
            try
            {
                using var connection = AOTSqliteDb.Open();
                string? backupGuId = null;
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT guId FROM SocialNetworkBackups WHERE NetworkName = @NetworkName LIMIT 1";
                    cmd.Parameters.AddWithValue("@NetworkName", networkName);
                    var result = cmd.ExecuteScalar();
                    backupGuId = result?.ToString();
                }
                if (backupGuId == null) return false;
                return RestoreSocialNetwork(backupGuId, overwrite);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error restoring SocialNetwork by name: {ex.Message}");
                return false;
            }
        }

        public static int RestoreAllSocialNetworks(bool overwrite = false)
        {
            int count = 0;
            try
            {
                using var connection = AOTSqliteDb.Open();
                var ids = new List<string>();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT guId FROM SocialNetworkBackups";
                    using var reader = cmd.ExecuteReader();
                    while (reader.Read()) ids.Add(reader.GetString(0));
                }
                foreach (var id in ids)
                {
                    if (RestoreSocialNetwork(id, overwrite)) count++;
                }
                return count;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error restoring all SocialNetworks: {ex.Message}");
                return 0;
            }
        }

        public static bool DeleteSocialNetworkWithBackup(string guId, bool deleteRelatedAppData = true, bool overwriteBackup = false)
        {
            try
            {
                using var connection = AOTSqliteDb.Open();
                using var transaction = connection.BeginTransaction();

                SocialNetwork? socialNetwork = null;
                using (var cmd = connection.CreateCommand())
                {
                    cmd.Transaction = transaction;
                    cmd.CommandText = "SELECT guId, NetworkName FROM SocialNetworks WHERE guId = @guId LIMIT 1";
                    cmd.Parameters.AddWithValue("@guId", guId);
                    using var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        socialNetwork = new SocialNetwork { guId = reader.GetString(0), NetworkName = reader.GetString(1) };
                    }
                }
                if (socialNetwork == null) return false;

                BackupSocialNetwork(socialNetwork, overwriteBackup);

                if (deleteRelatedAppData)
                {
                    var relatedAppDatas = new List<AppData>();
                    using (var cmd = connection.CreateCommand())
                    {
                        cmd.Transaction = transaction;
                        cmd.CommandText = "SELECT guId, DeviceID, NetworkName, PackageName, DeviceModel, AccStatus, Email, PassEmail, Username, Password, PrivateKey, Phone, HoTen, BirtDay, Gender, PrimaryEducation, SecondaryEducation, Marital, Token, Cookies, FriendCount, GroupCount, FollowCount, Avatar, DataGroup, DataFriend, DataFollow, AppVersion, ABI, NetworkGuId FROM AppDatas WHERE NetworkGuId = @guId";
                        cmd.Parameters.AddWithValue("@guId", guId);
                        using var reader = cmd.ExecuteReader();
                        while (reader.Read()) relatedAppDatas.Add(AppDataRepository_GetAppData(reader));
                    }
                    if (relatedAppDatas.Count > 0)
                    {
                        BackupAppDataRange(relatedAppDatas, overwriteBackup);
                        using var delCmd = connection.CreateCommand();
                        delCmd.Transaction = transaction;
                        delCmd.CommandText = "DELETE FROM AppDatas WHERE NetworkGuId = @guId";
                        delCmd.Parameters.AddWithValue("@guId", guId);
                        delCmd.ExecuteNonQuery();
                    }
                }

                using (var delNetCmd = connection.CreateCommand())
                {
                    delNetCmd.Transaction = transaction;
                    delNetCmd.CommandText = "DELETE FROM SocialNetworks WHERE guId = @guId";
                    delNetCmd.Parameters.AddWithValue("@guId", guId);
                    delNetCmd.ExecuteNonQuery();
                }

                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error deleting SocialNetwork with backup: {ex.Message}");
                return false;
            }
        }

        public static bool DeleteSocialNetworkByNameWithBackup(string networkName, bool deleteRelatedAppData = true, bool overwriteBackup = false)
        {
            try
            {
                using var connection = AOTSqliteDb.Open();
                string? guId = null;
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT guId FROM SocialNetworks WHERE NetworkName = @NetworkName LIMIT 1";
                    cmd.Parameters.AddWithValue("@NetworkName", networkName);
                    var result = cmd.ExecuteScalar();
                    guId = result?.ToString();
                }
                if (guId == null) return false;
                return DeleteSocialNetworkWithBackup(guId, deleteRelatedAppData, overwriteBackup);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error deleting SocialNetwork by name with backup: {ex.Message}");
                return false;
            }
        }

        public static List<SocialNetworkBackup> GetAllSocialNetworkBackups()
        {
            var list = new List<SocialNetworkBackup>();
            using var connection = AOTSqliteDb.Open();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT guId, NetworkName FROM SocialNetworkBackups ORDER BY NetworkName";
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new SocialNetworkBackup { guId = reader.GetString(0), NetworkName = reader.GetString(1) });
            }
            return list;
        }

        public static bool DeleteSocialNetworkBackup(string backupGuId)
        {
            try
            {
                using var connection = AOTSqliteDb.Open();
                using var cmd = connection.CreateCommand();
                cmd.CommandText = "DELETE FROM SocialNetworkBackups WHERE guId = @guId";
                cmd.Parameters.AddWithValue("@guId", backupGuId);
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error deleting SocialNetworkBackup: {ex.Message}");
                return false;
            }
        }

        #region Query

        public static List<AppDataBackup> GetAllAppDataBackups()
        {
            var list = new List<AppDataBackup>();
            using var connection = AOTSqliteDb.Open();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT guId, DeviceID, NetworkName, PackageName, Email, PassEmail, Username, Password, PrivateKey, Phone, HoTen, BirtDay, Gender, PrimaryEducation, SecondaryEducation, Marital, Token, Cookies, FriendCount, GroupCount, FollowCount, Avatar, DataGroup, DataFriend, DataFollow, AppVersion, ABI, NetworkGuId FROM AppDataBackups";
            using var reader = cmd.ExecuteReader();
            while (reader.Read()) list.Add(ReadBackup(reader));
            return list;
        }

        public static AppDataBackup? GetAppDataBackup(string deviceId, string networkName)
        {
            using var connection = AOTSqliteDb.Open();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT guId, DeviceID, NetworkName, PackageName, Email, PassEmail, Username, Password, PrivateKey, Phone, HoTen, BirtDay, Gender, PrimaryEducation, SecondaryEducation, Marital, Token, Cookies, FriendCount, GroupCount, FollowCount, Avatar, DataGroup, DataFriend, DataFollow, AppVersion, ABI, NetworkGuId FROM AppDataBackups WHERE DeviceID = @DeviceID AND NetworkName = @NetworkName LIMIT 1";
            cmd.Parameters.AddWithValue("@DeviceID", deviceId);
            cmd.Parameters.AddWithValue("@NetworkName", networkName);
            using var reader = cmd.ExecuteReader();
            return reader.Read() ? ReadBackup(reader) : null;
        }

        public static bool DeleteAppDataBackup(string backupGuId)
        {
            using var connection = AOTSqliteDb.Open();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM AppDataBackups WHERE guId = @guId";
            cmd.Parameters.AddWithValue("@guId", backupGuId);
            return cmd.ExecuteNonQuery() > 0;
        }

        #endregion

        #region Helpers

        private static void AddBackupParameters(SqliteCommand cmd, AppData appData)
        {
            cmd.Parameters.AddWithValue("@DeviceID", appData.DeviceID);
            cmd.Parameters.AddWithValue("@NetworkName", appData.NetworkName);
            cmd.Parameters.AddWithValue("@PackageName", appData.PackageName ?? string.Empty);
            cmd.Parameters.AddWithValue("@Email", appData.Email ?? string.Empty);
            cmd.Parameters.AddWithValue("@PassEmail", appData.PassEmail ?? string.Empty);
            cmd.Parameters.AddWithValue("@Username", appData.Username ?? string.Empty);
            cmd.Parameters.AddWithValue("@Password", appData.Password ?? string.Empty);
            cmd.Parameters.AddWithValue("@PrivateKey", appData.PrivateKey ?? string.Empty);
            cmd.Parameters.AddWithValue("@Phone", appData.Phone ?? string.Empty);
            cmd.Parameters.AddWithValue("@HoTen", appData.HoTen ?? string.Empty);
            cmd.Parameters.AddWithValue("@BirtDay", appData.BirtDay ?? string.Empty);
            cmd.Parameters.AddWithValue("@Gender", (int)appData.Gender);
            cmd.Parameters.AddWithValue("@PrimaryEducation", appData.PrimaryEducation ?? string.Empty);
            cmd.Parameters.AddWithValue("@SecondaryEducation", appData.SecondaryEducation ?? string.Empty);
            cmd.Parameters.AddWithValue("@Marital", (int)appData.Marital);
            cmd.Parameters.AddWithValue("@Token", appData.Token ?? string.Empty);
            cmd.Parameters.AddWithValue("@Cookies", appData.Cookies ?? string.Empty);
            cmd.Parameters.AddWithValue("@FriendCount", appData.FriendCount);
            cmd.Parameters.AddWithValue("@GroupCount", appData.GroupCount);
            cmd.Parameters.AddWithValue("@FollowCount", appData.FollowCount);
            cmd.Parameters.AddWithValue("@Avatar", appData.Avatar ?? string.Empty);
            cmd.Parameters.AddWithValue("@DataGroup", appData.DataGroup ?? string.Empty);
            cmd.Parameters.AddWithValue("@DataFriend", appData.DataFriend ?? string.Empty);
            cmd.Parameters.AddWithValue("@DataFollow", appData.DataFollow ?? string.Empty);
            cmd.Parameters.AddWithValue("@AppVersion", appData.AppVersion ?? string.Empty);
            cmd.Parameters.AddWithValue("@ABI", appData.ABI ?? string.Empty);
            cmd.Parameters.AddWithValue("@NetworkGuId", appData.NetworkGuId ?? string.Empty);
        }

        private static void AddRestoreParameters(SqliteCommand cmd, AppDataBackup backup)
        {
            cmd.Parameters.AddWithValue("@DeviceID", backup.DeviceID);
            cmd.Parameters.AddWithValue("@NetworkName", backup.NetworkName);
            cmd.Parameters.AddWithValue("@PackageName", backup.PackageName);
            cmd.Parameters.AddWithValue("@Email", backup.Email);
            cmd.Parameters.AddWithValue("@PassEmail", backup.PassEmail);
            cmd.Parameters.AddWithValue("@Username", backup.Username);
            cmd.Parameters.AddWithValue("@Password", backup.Password);
            cmd.Parameters.AddWithValue("@PrivateKey", backup.PrivateKey);
            cmd.Parameters.AddWithValue("@Phone", backup.Phone);
            cmd.Parameters.AddWithValue("@HoTen", backup.HoTen);
            cmd.Parameters.AddWithValue("@BirtDay", backup.BirtDay);
            cmd.Parameters.AddWithValue("@Gender", (int)backup.Gender);
            cmd.Parameters.AddWithValue("@PrimaryEducation", backup.PrimaryEducation);
            cmd.Parameters.AddWithValue("@SecondaryEducation", backup.SecondaryEducation);
            cmd.Parameters.AddWithValue("@Marital", (int)backup.Marital);
            cmd.Parameters.AddWithValue("@Token", backup.Token);
            cmd.Parameters.AddWithValue("@Cookies", backup.Cookies);
            cmd.Parameters.AddWithValue("@FriendCount", backup.FriendCount);
            cmd.Parameters.AddWithValue("@GroupCount", backup.GroupCount);
            cmd.Parameters.AddWithValue("@FollowCount", backup.FollowCount);
            cmd.Parameters.AddWithValue("@Avatar", backup.Avatar);
            cmd.Parameters.AddWithValue("@DataGroup", backup.DataGroup);
            cmd.Parameters.AddWithValue("@DataFriend", backup.DataFriend);
            cmd.Parameters.AddWithValue("@DataFollow", backup.DataFollow);
            cmd.Parameters.AddWithValue("@AppVersion", backup.AppVersion);
            cmd.Parameters.AddWithValue("@ABI", backup.ABI);
            cmd.Parameters.AddWithValue("@NetworkGuId", backup.NetworkGuId);
        }

        private static AppDataBackup ReadBackup(SqliteDataReader reader)
        {
            return new AppDataBackup
            {
                guId = reader.GetString(0),
                DeviceID = reader.GetString(1),
                NetworkName = reader.GetString(2),
                PackageName = reader.GetString(3),
                Email = reader.GetString(4),
                PassEmail = reader.GetString(5),
                Username = reader.GetString(6),
                Password = reader.GetString(7),
                PrivateKey = reader.GetString(8),
                Phone = reader.GetString(9),
                HoTen = reader.GetString(10),
                BirtDay = reader.GetString(11),
                Gender = (Gender)reader.GetInt32(12),
                PrimaryEducation = reader.GetString(13),
                SecondaryEducation = reader.GetString(14),
                Marital = (Marital)reader.GetInt32(15),
                Token = reader.GetString(16),
                Cookies = reader.GetString(17),
                FriendCount = reader.GetInt32(18),
                GroupCount = reader.GetInt32(19),
                FollowCount = reader.GetInt32(20),
                Avatar = reader.GetString(21),
                DataGroup = reader.GetString(22),
                DataFriend = reader.GetString(23),
                DataFollow = reader.GetString(24),
                AppVersion = reader.GetString(25),
                ABI = reader.GetString(26),
                NetworkGuId = reader.GetString(27)
            };
        }

        private static AppData AppDataRepository_GetAppData(SqliteDataReader reader)
        {
            return new AppData
            {
                guId = reader.GetString(0),
                DeviceID = reader.GetString(1),
                NetworkName = reader.GetString(2),
                PackageName = reader.GetString(3),
                DeviceModel = reader.GetString(4),
                AccStatus = (AccStatus)reader.GetInt32(5),
                Email = reader.GetString(6),
                PassEmail = reader.GetString(7),
                Username = reader.GetString(8),
                Password = reader.GetString(9),
                PrivateKey = reader.GetString(10),
                Phone = reader.GetString(11),
                HoTen = reader.GetString(12),
                BirtDay = reader.GetString(13),
                Gender = (Gender)reader.GetInt32(14),
                PrimaryEducation = reader.GetString(15),
                SecondaryEducation = reader.GetString(16),
                Marital = (Marital)reader.GetInt32(17),
                Token = reader.GetString(18),
                Cookies = reader.GetString(19),
                FriendCount = reader.GetInt32(20),
                GroupCount = reader.GetInt32(21),
                FollowCount = reader.GetInt32(22),
                Avatar = reader.GetString(23),
                DataGroup = reader.GetString(24),
                DataFriend = reader.GetString(25),
                DataFollow = reader.GetString(26),
                AppVersion = reader.GetString(27),
                ABI = reader.GetString(28),
                NetworkGuId = reader.GetString(29)
            };
        }

        #endregion
    }
}
