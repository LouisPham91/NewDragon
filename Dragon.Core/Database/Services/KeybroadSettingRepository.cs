using Dragon.Database;
using Dragon.Database.Models;
using Microsoft.Data.Sqlite;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;

namespace Dragon.Controller.Database.Services
{
    public static class KeybroadSettingRepository
    {
        public static bool Add(KeybroadSetting setting)
        {
            using var connection = AOTSqliteDb.Open();
            using var command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO KeybroadSettings (DeviceId, Langeuage, IMEId) 
                                    VALUES (@DeviceId, @Langeuage, @IMEId)";
            command.Parameters.AddWithValue("@DeviceId", setting.DeviceId);
            command.Parameters.AddWithValue("@Langeuage", (int)setting.Langeuage);
            command.Parameters.AddWithValue("@IMEId", setting.IMEId);
            return command.ExecuteNonQuery() > 0;
        }

        public static bool AddIf(KeybroadSetting setting)
        {
            if (FindOneByID(setting.Id) != null)
                return false;
            return Add(setting);
        }
        public static bool Update(KeybroadSetting setting)
        {
            using var connection = AOTSqliteDb.Open();
            using var command = connection.CreateCommand();
            command.CommandText = @"UPDATE KeybroadSettings 
                                    SET DeviceId=@DeviceId, Langeuage=@Langeuage, IMEId=@IMEId 
                                    WHERE Id=@Id";
            command.Parameters.AddWithValue("@DeviceId", setting.DeviceId);
            command.Parameters.AddWithValue("@Langeuage", (int)setting.Langeuage);
            command.Parameters.AddWithValue("@IMEId", setting.IMEId);
            command.Parameters.AddWithValue("@Id", setting.Id);
            return command.ExecuteNonQuery() > 0;
        }

        // Hàm cũ: FindOne(deviceId, lang)
        public static KeybroadSetting? FindOne(string deviceId, Langeuage lang)
        {
            using var connection = AOTSqliteDb.Open();
            using var command = connection.CreateCommand();
            command.CommandText = @"SELECT Id, DeviceId, Langeuage, IMEId 
                                    FROM KeybroadSettings 
                                    WHERE DeviceId=@DeviceId AND Langeuage=@Lang LIMIT 1";
            command.Parameters.AddWithValue("@DeviceId", deviceId);
            command.Parameters.AddWithValue("@Lang", (int)lang);
            using var reader = command.ExecuteReader();
            return reader.Read() ? MapKeybroadSetting(reader) : null;
        }

        // Hàm cũ: IsAny(deviceId, imei)
        public static bool IsAny(string deviceId, string imei)
        {
            using var connection = AOTSqliteDb.Open();
            using var command = connection.CreateCommand();
            command.CommandText = @"SELECT COUNT(1) 
                                    FROM KeybroadSettings 
                                    WHERE DeviceId=@DeviceId AND IMEId=@IMEId";
            command.Parameters.AddWithValue("@DeviceId", deviceId);
            command.Parameters.AddWithValue("@IMEId", imei);
            return Convert.ToInt32(command.ExecuteScalar()) > 0;
        }

        // Hàm cũ: AddIfNotExists(deviceId, imei, lang)
        public static bool AddIfNotExists(string deviceId, string imei, Langeuage lang)
        {
            if (IsAny(deviceId, imei))
                return true;
            return Add(new KeybroadSetting { DeviceId = deviceId, IMEId = imei, Langeuage = lang });
        }

        public static KeybroadSetting? FindOneByID(int id)
        {
            using var connection = AOTSqliteDb.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, DeviceId, Langeuage, IMEId FROM KeybroadSettings WHERE Id=@Id LIMIT 1";
            command.Parameters.AddWithValue("@Id", id);
            using var reader = command.ExecuteReader();
            return reader.Read() ? MapKeybroadSetting(reader) : null;
        }
        public static void DeleteByID(int id)
        {
            using var connection = AOTSqliteDb.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM KeybroadSettings WHERE Id=@Id";
            command.Parameters.AddWithValue("@Id", id);
            command.ExecuteNonQuery();
        }
        public static List<KeybroadSetting> FindManyByIMEId(string imeId)
        {
            var list = new List<KeybroadSetting>();
            using var connection = AOTSqliteDb.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, DeviceId, Langeuage, IMEId FROM KeybroadSettings WHERE IMEId=@IMEId";
            command.Parameters.AddWithValue("@IMEId", imeId);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                list.Add(MapKeybroadSetting(reader));
            }
            return list;
        }
        public static List<KeybroadSetting> FindManyByDeviceID(string deviceId)
        {
            var list = new List<KeybroadSetting>();
            using var connection = AOTSqliteDb.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, DeviceId, Langeuage, IMEId FROM KeybroadSettings WHERE DeviceId=@DeviceId";
            command.Parameters.AddWithValue("@DeviceId", deviceId);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                list.Add(MapKeybroadSetting(reader));
            }
            return list;
        }

        public static bool IsAny()
        {
            using var connection = AOTSqliteDb.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(1) FROM KeybroadSettings";
            return Convert.ToInt32(command.ExecuteScalar()) > 0;
        }

        public static List<KeybroadSetting> LoadAll()
        {
            var list = new List<KeybroadSetting>();
            using var connection = AOTSqliteDb.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, DeviceId, Langeuage, IMEId FROM KeybroadSettings";
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                list.Add(MapKeybroadSetting(reader));
            }
            return list;
        }

        private static KeybroadSetting MapKeybroadSetting(SqliteDataReader reader)
        {
            return new KeybroadSetting
            {
                Id = reader.GetInt32(0),
                DeviceId = reader.GetString(1),
                Langeuage = (Langeuage)reader.GetInt32(2),
                IMEId = reader.GetString(3)
            };
        }
    }
}
