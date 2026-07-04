using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Data.Sqlite;
using Dragon.Database.Models;

namespace Dragon.Database.Services
{
    /// <summary>
    /// SQLite repository for SocialNetwork.
    /// - Namespace: Dragon.Database.Services
    /// - Class: SocialNetworkRepository
    /// - ADO.NET, parameterized SQL, no reflection — suitable for Native AOT.
    /// </summary>
    public static class SocialNetworkRepository
    {
        private const string TABLE = "SocialNetworks";

        private static SocialNetwork MapSocialNetwork(SqliteDataReader reader)
        {
            return new SocialNetwork
            {
                guId = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                NetworkName = reader.IsDBNull(1) ? string.Empty : reader.GetString(1)
            };
        }

        public static List<SocialNetwork> GetAll()
        {
            var list = new List<SocialNetwork>();
            try
            {
                using var connection = AOTSqliteDb.Open();
                using var cmd = connection.CreateCommand();
                cmd.CommandText = $"SELECT guId, NetworkName FROM {TABLE} ORDER BY NetworkName";
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                    list.Add(MapSocialNetwork(reader));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"GetAll SocialNetwork error: {ex.InnerException?.Message ?? ex.Message}");
            }
            return list;
        }

        // Backwards-compatible alias
        public static List<SocialNetwork> LoadAll() => GetAll();

        public static bool Add(SocialNetwork socialNetwork)
        {
            if (socialNetwork == null) return false;
            if (string.IsNullOrWhiteSpace(socialNetwork.NetworkName)) return false;

            try
            {
                using var connection = AOTSqliteDb.Open();

                // Check exists by NetworkName
                using (var checkCmd = connection.CreateCommand())
                {
                    checkCmd.CommandText = $"SELECT COUNT(1) FROM {TABLE} WHERE NetworkName = @NetworkName";
                    checkCmd.Parameters.AddWithValue("@NetworkName", socialNetwork.NetworkName.Trim());
                    var exists = Convert.ToInt32(checkCmd.ExecuteScalar()) > 0;
                    if (exists) return false;
                }

                using var cmd = connection.CreateCommand();
                cmd.CommandText = $"INSERT INTO {TABLE} (guId, NetworkName) VALUES (@guId, @NetworkName)";
                cmd.Parameters.AddWithValue("@guId", string.IsNullOrEmpty(socialNetwork.guId) ? Guid.NewGuid().ToString() : socialNetwork.guId);
                cmd.Parameters.AddWithValue("@NetworkName", socialNetwork.NetworkName.Trim());
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Add SocialNetwork error: {ex.InnerException?.Message ?? ex.Message}");
                return false;
            }
        }

        public static bool AddIf(SocialNetwork socialNetwork)
        {
            if (socialNetwork == null) return false;
            try
            {
                using var connection = AOTSqliteDb.Open();

                // If guId provided and exists, skip
                if (!string.IsNullOrEmpty(socialNetwork.guId))
                {
                    using var checkById = connection.CreateCommand();
                    checkById.CommandText = $"SELECT COUNT(1) FROM {TABLE} WHERE guId = @guId";
                    checkById.Parameters.AddWithValue("@guId", socialNetwork.guId);
                    if (Convert.ToInt32(checkById.ExecuteScalar()) > 0) return false;
                }

                // If NetworkName exists, skip
                using var checkCmd = connection.CreateCommand();
                checkCmd.CommandText = $"SELECT COUNT(1) FROM {TABLE} WHERE NetworkName = @NetworkName";
                checkCmd.Parameters.AddWithValue("@NetworkName", socialNetwork.NetworkName ?? string.Empty);
                if (Convert.ToInt32(checkCmd.ExecuteScalar()) > 0) return false;

                return Add(socialNetwork);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"AddIf SocialNetwork error: {ex.InnerException?.Message ?? ex.Message}");
                return false;
            }
        }

        public static bool Exit(SocialNetwork socialNetwork)
        {
            if (socialNetwork == null) return false;
            if (string.IsNullOrEmpty(socialNetwork.guId)) return false;
            return Exists(socialNetwork.guId);
        }
        public static bool ExistByName(string SocialNetworkName)
        {
            if (string.IsNullOrEmpty(SocialNetworkName)) return false;
            try
            {
                using var connection = AOTSqliteDb.Open();
                using var cmd = connection.CreateCommand();
                cmd.CommandText = $"SELECT COUNT(1) FROM {TABLE} WHERE NetworkName = @NetworkName";
                cmd.Parameters.AddWithValue("@NetworkName", SocialNetworkName);
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exists SocialNetwork error: {ex.InnerException?.Message ?? ex.Message}");
                return false;
            }
        }
        public static bool Exists(string guId)
        {
            if (string.IsNullOrEmpty(guId)) return false;
            try
            {
                using var connection = AOTSqliteDb.Open();
                using var cmd = connection.CreateCommand();
                cmd.CommandText = $"SELECT COUNT(1) FROM {TABLE} WHERE guId = @guId";
                cmd.Parameters.AddWithValue("@guId", guId);
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exists SocialNetwork error: {ex.InnerException?.Message ?? ex.Message}");
                return false;
            }
        }

        public static SocialNetwork? FindOneByID(string guId)
        {
            if (string.IsNullOrEmpty(guId)) return null;
            try
            {
                using var connection = AOTSqliteDb.Open();
                using var cmd = connection.CreateCommand();
                cmd.CommandText = $"SELECT guId, NetworkName FROM {TABLE} WHERE guId = @guId LIMIT 1";
                cmd.Parameters.AddWithValue("@guId", guId);
                using var reader = cmd.ExecuteReader();
                return reader.Read() ? MapSocialNetwork(reader) : null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"FindOneByID SocialNetwork error: {ex.InnerException?.Message ?? ex.Message}");
                return null;
            }
        }

        public static bool Update(SocialNetwork socialNetwork)
        {
            if (socialNetwork == null) return false;
            if (string.IsNullOrWhiteSpace(socialNetwork.NetworkName)) return false;
            if (string.IsNullOrEmpty(socialNetwork.guId)) return false;

            try
            {
                using var connection = AOTSqliteDb.Open();
                using var tx = connection.BeginTransaction();

                SocialNetwork? existing = null;
                using (var selectCmd = connection.CreateCommand())
                {
                    selectCmd.Transaction = tx;
                    selectCmd.CommandText = $"SELECT guId, NetworkName FROM {TABLE} WHERE guId = @guId LIMIT 1";
                    selectCmd.Parameters.AddWithValue("@guId", socialNetwork.guId);
                    using var reader = selectCmd.ExecuteReader();
                    if (reader.Read()) existing = MapSocialNetwork(reader);
                }

                if (existing == null)
                {
                    tx.Rollback();
                    return false;
                }

                var oldName = existing.NetworkName ?? string.Empty;
                var newName = socialNetwork.NetworkName.Trim();

                using (var updateCmd = connection.CreateCommand())
                {
                    updateCmd.Transaction = tx;
                    updateCmd.CommandText = $"UPDATE {TABLE} SET NetworkName = @NetworkName WHERE guId = @guId";
                    updateCmd.Parameters.AddWithValue("@NetworkName", newName);
                    updateCmd.Parameters.AddWithValue("@guId", socialNetwork.guId);
                    updateCmd.ExecuteNonQuery();
                }

                if (!string.Equals(oldName, newName, StringComparison.Ordinal))
                {
                    using var updateAppCmd = connection.CreateCommand();
                    updateAppCmd.Transaction = tx;
                    updateAppCmd.CommandText = "UPDATE AppDatas SET NetworkName = @NewNetworkName WHERE NetworkName = @OldNetworkName";
                    updateAppCmd.Parameters.AddWithValue("@NewNetworkName", newName);
                    updateAppCmd.Parameters.AddWithValue("@OldNetworkName", oldName);
                    updateAppCmd.ExecuteNonQuery();
                }

                tx.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Update SocialNetwork error: {ex.InnerException?.Message ?? ex.Message}");
                return false;
            }
        }

        public static bool Delete(string guId, bool IsDeleteAppdata = true)
        {
            if (string.IsNullOrEmpty(guId)) return false;
            try
            {
                using var connection = AOTSqliteDb.Open();
                using var tx = connection.BeginTransaction();

                // Ensure exists and get NetworkName
                string networkName;
                using (var selectCmd = connection.CreateCommand())
                {
                    selectCmd.Transaction = tx;
                    selectCmd.CommandText = $"SELECT NetworkName FROM {TABLE} WHERE guId = @guId LIMIT 1";
                    selectCmd.Parameters.AddWithValue("@guId", guId);
                    var result = selectCmd.ExecuteScalar();
                    if (result == null || result == DBNull.Value)
                    {
                        tx.Rollback();
                        return false;
                    }
                    networkName = Convert.ToString(result) ?? string.Empty;
                }

                using (var deleteCmd = connection.CreateCommand())
                {
                    deleteCmd.Transaction = tx;
                    deleteCmd.CommandText = $"DELETE FROM {TABLE} WHERE guId = @guId";
                    deleteCmd.Parameters.AddWithValue("@guId", guId);
                    deleteCmd.ExecuteNonQuery();
                }

                if (IsDeleteAppdata)
                {
                    using var deleteAppCmd = connection.CreateCommand();
                    deleteAppCmd.Transaction = tx;
                    deleteAppCmd.CommandText = "DELETE FROM AppDatas WHERE NetworkGuId = @NetworkGuId OR NetworkName = @NetworkName";
                    deleteAppCmd.Parameters.AddWithValue("@NetworkGuId", guId);
                    deleteAppCmd.Parameters.AddWithValue("@NetworkName", networkName);
                    deleteAppCmd.ExecuteNonQuery();
                }

                tx.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Delete SocialNetwork error: {ex.InnerException?.Message ?? ex.Message}");
                return false;
            }
        }

        public static bool DeleteByName(string networkName, bool IsDeleteAppdata = true)
        {
            if (string.IsNullOrWhiteSpace(networkName)) return false;
            try
            {
                using var connection = AOTSqliteDb.Open();
                using var tx = connection.BeginTransaction();

                string? guId = null;
                using (var selectCmd = connection.CreateCommand())
                {
                    selectCmd.Transaction = tx;
                    selectCmd.CommandText = $"SELECT guId FROM {TABLE} WHERE NetworkName = @NetworkName LIMIT 1";
                    selectCmd.Parameters.AddWithValue("@NetworkName", networkName.Trim());
                    var result = selectCmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        guId = Convert.ToString(result);
                }

                using (var deleteCmd = connection.CreateCommand())
                {
                    deleteCmd.Transaction = tx;
                    deleteCmd.CommandText = $"DELETE FROM {TABLE} WHERE NetworkName = @NetworkName";
                    deleteCmd.Parameters.AddWithValue("@NetworkName", networkName.Trim());
                    deleteCmd.ExecuteNonQuery();
                }

                if (IsDeleteAppdata)
                {
                    using var deleteAppCmd = connection.CreateCommand();
                    deleteAppCmd.Transaction = tx;
                    deleteAppCmd.CommandText = "DELETE FROM AppDatas WHERE NetworkName = @NetworkName OR NetworkGuId = @GuId";
                    deleteAppCmd.Parameters.AddWithValue("@NetworkName", networkName.Trim());
                    deleteAppCmd.Parameters.AddWithValue("@GuId", guId ?? string.Empty);
                    deleteAppCmd.ExecuteNonQuery();
                }

                tx.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"DeleteByName SocialNetwork error: {ex.InnerException?.Message ?? ex.Message}");
                return false;
            }
        }

        public static List<SocialNetwork> SearchSocialNetworks(string keyword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keyword)) return GetAll();

                var list = new List<SocialNetwork>();
                using var connection = AOTSqliteDb.Open();
                using var cmd = connection.CreateCommand();
                cmd.CommandText = $"SELECT guId, NetworkName FROM {TABLE} WHERE LOWER(NetworkName) LIKE @Pattern ORDER BY NetworkName";
                cmd.Parameters.AddWithValue("@Pattern", "%" + keyword.Trim().ToLowerInvariant() + "%");
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                    list.Add(MapSocialNetwork(reader));
                return list;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SearchSocialNetworks error: {ex.InnerException?.Message ?? ex.Message}");
                return new List<SocialNetwork>();
            }
        }

        public static bool IsAny()
        {
            try
            {
                using var connection = AOTSqliteDb.Open();
                using var cmd = connection.CreateCommand();
                cmd.CommandText = $"SELECT COUNT(1) FROM {TABLE}";
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"IsAny SocialNetwork error: {ex.InnerException?.Message ?? ex.Message}");
                return false;
            }
        }
    }
}
