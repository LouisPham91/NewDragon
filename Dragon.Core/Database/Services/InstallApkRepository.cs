using Dragon.Database;
using Dragon.Database.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace Dragon.Controller.Database.Services
{
    public static class InstallApkRepository
    {
        public static bool Add(InstallApk apk)
        {
            using var connection = AOTSqliteDb.Open();

            // kiểm tra trùng Path
            using (var checkCmd = connection.CreateCommand())
            {
                checkCmd.CommandText = "SELECT COUNT(*) FROM InstallApks WHERE Path = @Path";
                checkCmd.Parameters.AddWithValue("@Path", apk.Path);

                var count = Convert.ToInt32(checkCmd.ExecuteScalar());
                if (count > 0)
                {
                    // đã tồn tại, không thêm nữa
                    return false;
                }
            }

            using var command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO InstallApks 
        (AppName, PackageName, VersionName, VersionCode, MinAPI, MaxAPI, ABIs, Path) 
        VALUES (@AppName, @PackageName, @VersionName, @VersionCode, @MinAPI, @MaxAPI, @ABIs, @Path)";
            command.Parameters.AddWithValue("@AppName", apk.AppName);
            command.Parameters.AddWithValue("@PackageName", apk.PackageName);
            command.Parameters.AddWithValue("@VersionName", apk.VersionName);
            command.Parameters.AddWithValue("@VersionCode", apk.VersionCode);
            command.Parameters.AddWithValue("@MinAPI", apk.MinAPI);
            command.Parameters.AddWithValue("@MaxAPI", apk.MaxAPI);
            command.Parameters.AddWithValue("@ABIs", apk.ABIs);
            command.Parameters.AddWithValue("@Path", apk.Path);

            return command.ExecuteNonQuery() > 0;
        }


        public static bool Update(InstallApk apk)
        {
            using var connection = AOTSqliteDb.Open();
            using var command = connection.CreateCommand();
            command.CommandText = @"UPDATE InstallApks 
                SET AppName=@AppName, PackageName=@PackageName, VersionName=@VersionName, 
                    VersionCode=@VersionCode, MinAPI=@MinAPI, MaxAPI=@MaxAPI, ABIs=@ABIs 
                WHERE Path=@Path";
            command.Parameters.AddWithValue("@AppName", apk.AppName);
            command.Parameters.AddWithValue("@PackageName", apk.PackageName);
            command.Parameters.AddWithValue("@VersionName", apk.VersionName);
            command.Parameters.AddWithValue("@VersionCode", apk.VersionCode);
            command.Parameters.AddWithValue("@MinAPI", apk.MinAPI);
            command.Parameters.AddWithValue("@MaxAPI", apk.MaxAPI);
            command.Parameters.AddWithValue("@ABIs", apk.ABIs);
            command.Parameters.AddWithValue("@Path", apk.Path);
            return command.ExecuteNonQuery() > 0;
        }

        public static InstallApk? FindOneByPath(string path)
        {
            using var connection = AOTSqliteDb.Open();
            using var command = connection.CreateCommand();
            command.CommandText = @"SELECT Id, AppName, PackageName, VersionName, VersionCode, 
                                           MinAPI, MaxAPI, ABIs, Path 
                                    FROM InstallApks WHERE Path=@Path LIMIT 1";
            command.Parameters.AddWithValue("@Path", path);
            using var reader = command.ExecuteReader();
            return reader.Read() ? MapInstallApk(reader) : null;
        }

        public static void DeleteByPath(string path)
        {
            using var connection = AOTSqliteDb.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM InstallApks WHERE Path=@Path";
            command.Parameters.AddWithValue("@Path", path);
            command.ExecuteNonQuery();
        }

        public static List<InstallApk> LoadAll()
        {
            var list = new List<InstallApk>();
            using var connection = AOTSqliteDb.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, AppName, PackageName, VersionName, VersionCode, MinAPI, MaxAPI, ABIs, Path FROM InstallApks";
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                list.Add(MapInstallApk(reader));
            }
            return list;
        }

        private static InstallApk MapInstallApk(SqliteDataReader reader)
        {
            var apk = new InstallApk
            {
                Id = reader.GetInt32(0),
                AppName = reader.GetString(1),
                PackageName = reader.GetString(2),
                VersionName = reader.GetString(3),
                VersionCode = reader.GetString(4),
                MinAPI = reader.GetInt32(5),
                MaxAPI = reader.GetInt32(6),
                ABIs = reader.GetString(7),
                Path = reader.GetString(8)
            };

            return apk;
        }
    }
}
