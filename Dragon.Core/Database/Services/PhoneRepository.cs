using Dragon.Database;
using Dragon.Database.Models;
using Microsoft.Data.Sqlite;
using AdvancedSharpAdbClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dragon.Controller.Database.Services
{
    public static class PhoneRepository
    {
        private const string COLS = "Id,PhoneTagNumber,PhoneBoxId,DeviceID,Serial,DeviceState,PhoneMode,Model,Product,Usb,Ipv4,Message,TransportId,IsUHDI,PhysicalWidth,PhysicalHeight,IsRunning,AndroidVersion,API,IsUseUSB,ProcVersion,ProcCpuInfo,IsAccessibleAppInstall,IsPingWifi,IsRooted,IsMagisk,IsKernelSu,PhoneHash";

        public static bool Add(Phone phone)
        {
            using var c = AOTSqliteDb.Open();
            using var cmd = c.CreateCommand();
            cmd.CommandText = @"INSERT INTO Phones 
            (PhoneTagNumber,PhoneBoxId,DeviceID,Serial,DeviceState,PhoneMode,Model,Product,Usb,Ipv4,Message,TransportId,IsUHDI,PhysicalWidth,PhysicalHeight,IsRunning,AndroidVersion,API,IsUseUSB,ProcVersion,ProcCpuInfo,IsAccessibleAppInstall,IsPingWifi,IsRooted,IsMagisk,IsKernelSu,PhoneHash)
            VALUES 
            (@PhoneTagNumber,@PhoneBoxId,@DeviceID,@Serial,@DeviceState,@PhoneMode,@Model,@Product,@Usb,@Ipv4,@Message,@TransportId,@IsUHDI,@PhysicalWidth,@PhysicalHeight,@IsRunning,@AndroidVersion,@API,@IsUseUSB,@ProcVersion,@ProcCpuInfo,@IsAccessibleAppInstall,@IsPingWifi,@IsRooted,@IsMagisk,@IsKernelSu,@PhoneHash);
            SELECT last_insert_rowid();";
            AddParameters(cmd, phone);
            var newId = Convert.ToInt64(cmd.ExecuteScalar());
            phone.Id = (int)newId;
            return newId > 0;
        }

        public static bool AddIf(Phone phone)
        {
            if (IsAnyByDeviceID(phone.DeviceID)) return false;
            return Add(phone);
        }

        public static Phone? FindOneByDeviceID(string deviceId)
        {
            using var c = AOTSqliteDb.Open();
            using var cmd = c.CreateCommand();
            cmd.CommandText = $"SELECT {COLS} FROM Phones WHERE DeviceID=@DeviceID LIMIT 1";
            cmd.Parameters.AddWithValue("@DeviceID", deviceId);
            using var r = cmd.ExecuteReader();
            return r.Read() ? MapPhone(r) : null;
        }

        public static Phone? FindOneByPhoneTagNumber(int tag)
        {
            using var c = AOTSqliteDb.Open();
            using var cmd = c.CreateCommand();
            cmd.CommandText = $"SELECT {COLS} FROM Phones WHERE PhoneTagNumber=@t LIMIT 1";
            cmd.Parameters.AddWithValue("@t", tag);
            using var r = cmd.ExecuteReader();
            return r.Read() ? MapPhone(r) : null;
        }

        public static Phone? FindOneByID(int id)
        {
            using var c = AOTSqliteDb.Open();
            using var cmd = c.CreateCommand();
            cmd.CommandText = $"SELECT {COLS} FROM Phones WHERE Id=@Id LIMIT 1";
            cmd.Parameters.AddWithValue("@Id", id);
            using var r = cmd.ExecuteReader();
            return r.Read() ? MapPhone(r) : null;
        }

        public static Phone? FindOneBySerial(string serial)
        {
            if (string.IsNullOrWhiteSpace(serial)) return null;
            using var c = AOTSqliteDb.Open();
            using var cmd = c.CreateCommand();
            cmd.CommandText = $"SELECT {COLS} FROM Phones WHERE DeviceID=@s OR (Ipv4||':5555')=@s LIMIT 1";
            cmd.Parameters.AddWithValue("@s", serial);
            using var r = cmd.ExecuteReader();
            return r.Read() ? MapPhone(r) : null;
        }

        public static List<Phone> LoadAll()
        {
            var list = new List<Phone>();
            using var c = AOTSqliteDb.Open();
            using var cmd = c.CreateCommand();
            cmd.CommandText = $"SELECT {COLS} FROM Phones";
            using var r = cmd.ExecuteReader();
            while (r.Read()) list.Add(MapPhone(r));
            return list;
        }

        public static List<Phone> FindManyWhereDeviceIdIsNullOrEmpty()
        {
            var list = new List<Phone>();
            using var c = AOTSqliteDb.Open();
            using var cmd = c.CreateCommand();
            cmd.CommandText = $"SELECT {COLS} FROM Phones WHERE DeviceID IS NULL OR DeviceID = '' ORDER BY Id";
            using var r = cmd.ExecuteReader();
            while (r.Read()) list.Add(MapPhone(r));
            return list;
        }

        public static List<Phone> FindManyByPhoneModes(params PhoneMode[] modes)
        {
            var list = new List<Phone>();
            if (modes == null || modes.Length == 0) return list;
            using var c = AOTSqliteDb.Open();
            var ps = modes.Select((_, i) => $"@m{i}").ToArray();
            using var cmd = c.CreateCommand();
            cmd.CommandText = $"SELECT {COLS} FROM Phones WHERE PhoneMode IN ({string.Join(",", ps)})";
            for (int i = 0; i < modes.Length; i++) cmd.Parameters.AddWithValue($"@m{i}", (int)modes[i]);
            using var r = cmd.ExecuteReader();
            while (r.Read()) list.Add(MapPhone(r));
            return list;
        }

        public static List<Phone> FindManyByPhoneMode(PhoneMode mode)
        {
            var list = new List<Phone>();
            using var c = AOTSqliteDb.Open();
            using var cmd = c.CreateCommand();
            cmd.CommandText = $"SELECT {COLS} FROM Phones WHERE PhoneMode=@m";
            cmd.Parameters.AddWithValue("@m", (int)mode);
            using var r = cmd.ExecuteReader();
            while (r.Read()) list.Add(MapPhone(r));
            return list;
        }

        public static List<Phone> FindManyByPhoneBoxId(int phoneBoxId = -1)
        {
            var list = new List<Phone>();
            using var c = AOTSqliteDb.Open();
            using var cmd = c.CreateCommand();
            if (phoneBoxId == -1)
                cmd.CommandText = $"SELECT {COLS} FROM Phones";
            else
            {
                cmd.CommandText = $"SELECT {COLS} FROM Phones WHERE PhoneBoxId=@id";
                cmd.Parameters.AddWithValue("@id", phoneBoxId);
            }
            using var r = cmd.ExecuteReader();
            while (r.Read()) list.Add(MapPhone(r));
            return list;
        }

        public static List<Phone> FindManyByPhoneTagNumber(int tag)
        {
            var list = new List<Phone>();
            using var c = AOTSqliteDb.Open();
            using var cmd = c.CreateCommand();
            cmd.CommandText = $"SELECT {COLS} FROM Phones WHERE PhoneTagNumber=@t";
            cmd.Parameters.AddWithValue("@t", tag);
            using var r = cmd.ExecuteReader();
            while (r.Read()) list.Add(MapPhone(r));
            return list;
        }

        public static bool Update(Phone p)
        {
            using var c = AOTSqliteDb.Open();
            using var cmd = c.CreateCommand();
            cmd.CommandText = @"UPDATE Phones SET 
                PhoneTagNumber=@PhoneTagNumber, PhoneBoxId=@PhoneBoxId, DeviceID=@DeviceID, Serial=@Serial, 
                DeviceState=@DeviceState, PhoneMode=@PhoneMode, Model=@Model, Product=@Product, Usb=@Usb, 
                Ipv4=@Ipv4, Message=@Message, TransportId=@TransportId, IsUHDI=@IsUHDI, 
                PhysicalWidth=@PhysicalWidth, PhysicalHeight=@PhysicalHeight, IsRunning=@IsRunning, 
                AndroidVersion=@AndroidVersion, API=@API, IsUseUSB=@IsUseUSB, ProcVersion=@ProcVersion, 
                ProcCpuInfo=@ProcCpuInfo, IsAccessibleAppInstall=@IsAccessibleAppInstall, IsPingWifi=@IsPingWifi, 
                IsRooted=@IsRooted, IsMagisk=@IsMagisk, IsKernelSu=@IsKernelSu, PhoneHash=@PhoneHash 
                WHERE Id=@Id";
            AddParameters(cmd, p);
            cmd.Parameters.AddWithValue("@Id", p.Id);
            return cmd.ExecuteNonQuery() > 0;
        }

        public static bool UpdateByDeviceID(string deviceID, Phone p)
        {
            if (string.IsNullOrWhiteSpace(deviceID) || p == null) return false;
            using var c = AOTSqliteDb.Open();
            int? id = null;
            using (var f = c.CreateCommand())
            {
                f.CommandText = "SELECT Id FROM Phones WHERE DeviceID=@d LIMIT 1";
                f.Parameters.AddWithValue("@d", deviceID);
                var o = f.ExecuteScalar();
                if (o == null) return false;
                id = Convert.ToInt32(o);
            }
            p.Id = id.Value;
            return Update(p);
        }

        public static int DeleteMany(List<Phone> listPhone)
        {
            if (listPhone == null || listPhone.Count == 0) return 0;
            int count = 0;
            using var c = AOTSqliteDb.Open();
            using var tx = c.BeginTransaction();
            try
            {
                foreach (var p in listPhone)
                {
                    using var cmd = c.CreateCommand();
                    cmd.Transaction = tx;
                    cmd.CommandText = "DELETE FROM Phones WHERE Id=@Id";
                    cmd.Parameters.AddWithValue("@Id", p.Id);
                    count += cmd.ExecuteNonQuery();
                }
                tx.Commit();
                return count;
            }
            catch { tx.Rollback(); throw; }
        }

        public static bool DeleteOne(int id)
        {
            using var c = AOTSqliteDb.Open();
            using var cmd = c.CreateCommand();
            cmd.CommandText = "DELETE FROM Phones WHERE Id=@Id";
            cmd.Parameters.AddWithValue("@Id", id);
            return cmd.ExecuteNonQuery() > 0;
        }

        public static bool DeleteOneByDeviceID(string deviceId)
        {
            using var c = AOTSqliteDb.Open();
            int? id = null;
            using (var f = c.CreateCommand())
            {
                f.CommandText = "SELECT Id FROM Phones WHERE DeviceID=@d LIMIT 1";
                f.Parameters.AddWithValue("@d", deviceId ?? "");
                var o = f.ExecuteScalar();
                if (o == null) return false;
                id = Convert.ToInt32(o);
            }
            return DeleteOne(id.Value);
        }

        public static bool IsAnyByDeviceID(string id)
        {
            using var c = AOTSqliteDb.Open();
            using var cmd = c.CreateCommand();
            cmd.CommandText = "SELECT COUNT(1) FROM Phones WHERE DeviceID=@d";
            cmd.Parameters.AddWithValue("@d", id);
            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }

        public static bool IsAny()
        {
            using var c = AOTSqliteDb.Open();
            using var cmd = c.CreateCommand();
            cmd.CommandText = "SELECT COUNT(1) FROM Phones";
            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }

        public static void ReorderAllPhoneTagNumbers()
        {
            using var c = AOTSqliteDb.Open();
            using var tx = c.BeginTransaction();
            try
            {
                var phones = new List<(int Id, int Box, int Tag)>();
                using (var cmd = c.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, PhoneBoxId, PhoneTagNumber FROM Phones WHERE PhoneBoxId IS NOT NULL ORDER BY PhoneBoxId, PhoneTagNumber";
                    using var r = cmd.ExecuteReader();
                    while (r.Read()) phones.Add((r.GetInt32(0), r.GetInt32(1), r.GetInt32(2)));
                }
                int tag = 1;
                foreach (var p in phones)
                {
                    using var u = c.CreateCommand();
                    u.CommandText = "UPDATE Phones SET PhoneTagNumber=@t WHERE Id=@id";
                    u.Parameters.AddWithValue("@t", tag++);
                    u.Parameters.AddWithValue("@id", p.Id);
                    u.ExecuteNonQuery();
                }
                tx.Commit();
            }
            catch { tx.Rollback(); throw; }
        }

        private static void AddParameters(SqliteCommand cmd, Phone p)
        {
            cmd.Parameters.AddWithValue("@PhoneTagNumber", p.PhoneTagNumber);
            cmd.Parameters.AddWithValue("@PhoneBoxId", (object?)p.PhoneBoxId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@DeviceID", p.DeviceID);
            cmd.Parameters.AddWithValue("@Serial", p.Serial);
            cmd.Parameters.AddWithValue("@DeviceState", (int)p.DeviceState);
            cmd.Parameters.AddWithValue("@PhoneMode", (int)p.PhoneMode);
            cmd.Parameters.AddWithValue("@Model", p.Model);
            cmd.Parameters.AddWithValue("@Product", p.Product);
            cmd.Parameters.AddWithValue("@Usb", p.Usb);
            cmd.Parameters.AddWithValue("@Ipv4", p.Ipv4);
            cmd.Parameters.AddWithValue("@Message", p.Message);
            cmd.Parameters.AddWithValue("@TransportId", p.TransportId);
            cmd.Parameters.AddWithValue("@IsUHDI", p.IsUHDI ? 1 : 0);
            cmd.Parameters.AddWithValue("@PhysicalWidth", p.PhysicalWidth);
            cmd.Parameters.AddWithValue("@PhysicalHeight", p.PhysicalHeight);
            cmd.Parameters.AddWithValue("@IsRunning", p.IsRunning ? 1 : 0);
            cmd.Parameters.AddWithValue("@AndroidVersion", p.AndroidVersion);
            cmd.Parameters.AddWithValue("@API", p.API);
            cmd.Parameters.AddWithValue("@IsUseUSB", p.IsUseUSB ? 1 : 0);
            cmd.Parameters.AddWithValue("@ProcVersion", p.ProcVersion);
            cmd.Parameters.AddWithValue("@ProcCpuInfo", p.ProcCpuInfo);
            cmd.Parameters.AddWithValue("@IsAccessibleAppInstall", p.IsAccessibleAppInstall ? 1 : 0);
            cmd.Parameters.AddWithValue("@IsPingWifi", p.IsPingWifi ? 1 : 0);
            cmd.Parameters.AddWithValue("@IsRooted", p.IsRooted ? 1 : 0);
            cmd.Parameters.AddWithValue("@IsMagisk", p.IsMagisk ? 1 : 0);
            cmd.Parameters.AddWithValue("@IsKernelSu", p.IsKernelSu ? 1 : 0);
            cmd.Parameters.AddWithValue("@PhoneHash", p.PhoneHash);
        }

        private static Phone MapPhone(SqliteDataReader r)
        {
            return new Phone
            {
                Id = r.GetInt32(0),
                PhoneTagNumber = r.GetInt32(1),
                PhoneBoxId = r.IsDBNull(2) ? null : r.GetInt32(2),
                DeviceID = r.GetString(3),
                Serial = r.GetString(4),
                DeviceState = (DeviceState)r.GetInt32(5),
                PhoneMode = (PhoneMode)r.GetInt32(6),
                Model = r.GetString(7),
                Product = r.GetString(8),
                Usb = r.GetString(9),
                Ipv4 = r.GetString(10),
                Message = r.GetString(11),
                TransportId = r.GetString(12),
                IsUHDI = r.GetInt32(13) == 1,
                PhysicalWidth = r.GetInt32(14),
                PhysicalHeight = r.GetInt32(15),
                IsRunning = r.GetInt32(16) == 1,
                AndroidVersion = r.GetString(17),
                API = r.GetInt32(18),
                IsUseUSB = r.GetInt32(19) == 1,
                ProcVersion = r.GetString(20),
                ProcCpuInfo = r.GetString(21),
                IsAccessibleAppInstall = r.GetInt32(22) == 1,
                IsPingWifi = r.GetInt32(23) == 1,
                IsRooted = r.GetInt32(24) == 1,
                IsMagisk = r.GetInt32(25) == 1,
                IsKernelSu = r.GetInt32(26) == 1,
                PhoneHash = r.GetString(27)
            };
        }
    }
}