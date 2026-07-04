using System;
using System.Collections.Generic;
using Dragon.Database;
using Dragon.Database.Models;
using Microsoft.Data.Sqlite;

namespace Dragon.Database.Services
{
    public static class SocialNetworkBackupRepository
    {
        public static List<SocialNetworkBackup> GetAll()
        {
            var list = new List<SocialNetworkBackup>();
            using var connection = AOTSqliteDb.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT guId, NetworkName FROM SocialNetworkBackups ORDER BY NetworkName";
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new SocialNetworkBackup
                {
                    guId = reader.GetString(0),
                    NetworkName = reader.GetString(1)
                });
            }
            return list;
        }

        public static bool Add(SocialNetworkBackup item)
        {
            if (string.IsNullOrWhiteSpace(item.guId))
                item.guId = Guid.NewGuid().ToString();
            using var connection = AOTSqliteDb.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "INSERT OR REPLACE INTO SocialNetworkBackups (guId, NetworkName) VALUES (@guId, @NetworkName)";
            command.Parameters.AddWithValue("@guId", item.guId);
            command.Parameters.AddWithValue("@NetworkName", item.NetworkName ?? string.Empty);
            return command.ExecuteNonQuery() > 0;
        }

        public static bool Delete(string guId)
        {
            using var connection = AOTSqliteDb.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM SocialNetworkBackups WHERE guId = @guId";
            command.Parameters.AddWithValue("@guId", guId);
            return command.ExecuteNonQuery() > 0;
        }
    }
}
