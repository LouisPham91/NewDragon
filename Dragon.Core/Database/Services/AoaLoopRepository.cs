using Dragon.Controller.DeviceControl.OTG.Loop;
using Dragon.Controller.DeviceControl.OTG.Model;
using Dragon.Database;
using Microsoft.Data.Sqlite;
using System.Text.Json;

namespace Dragon.Controller.Database.Services
{
    public static class AoaLoopRepository
    {
        private const string TABLE = "AoaLoops";
        private const string COLS = "Id, PhoneModel, ProcVersion, ProcCpuInfo, API, PhysicalWidth, PhysicalHeight, PointCloseApp, Type, ArgsJson, ChildrenJson";
        // ==================== MAPPER ====================
        private static AoaLoop Map(SqliteDataReader r)
        {
            var loop = new AoaLoop
            {
                Id = r.GetInt32(0),
                PhoneModel = r.IsDBNull(1) ? "" : r.GetString(1),
                ProcVersion = r.IsDBNull(2) ? "" : r.GetString(2),
                ProcCpuInfo = r.IsDBNull(3) ? "" : r.GetString(3),
                API = r.IsDBNull(4) ? 0 : r.GetInt32(4),
                PhysicalWidth = r.IsDBNull(5) ? 0 : r.GetInt32(5),   // MỚI
                PhysicalHeight = r.IsDBNull(6) ? 0 : r.GetInt32(6),  // MỚI
                PointCloseApp = r.IsDBNull(7) ? "" : r.GetString(7),
                Type = (AoaType)(r.IsDBNull(8) ? 0 : r.GetInt32(8)),
                ArgsJson = r.IsDBNull(9) ? "{}" : r.GetString(9),
            };

            // Deserialize Children
            string childrenJson = r.IsDBNull(10) ? "[]" : r.GetString(10);
            if (!string.IsNullOrEmpty(childrenJson) && childrenJson != "[]")
            {
                try
                {
                    loop.Children = JsonSerializer.Deserialize(
                        childrenJson,
                        AoaLoopJsonContext.Default.ListAoaLoop) ?? new List<AoaLoop>();
                }
                catch
                {
                    loop.Children = new List<AoaLoop>();
                }
            }

            return loop;
        }

        // ==================== CHECK UNIQUE ====================
        /// <summary>
        /// Kiểm tra xem đã tồn tại bản ghi với 4 trường unique chưa
        /// </summary>
        public static bool Exists(string phoneModel, string procVersion, string procCpuInfo, int api, int? excludeId = null)
        {
            using var c = AOTSqliteDb.Open();
            using var cmd = c.CreateCommand();

            if (excludeId.HasValue)
            {
                cmd.CommandText = $@"
                    SELECT COUNT(1) FROM {TABLE} 
                    WHERE PhoneModel = @m 
                      AND ProcVersion = @pv 
                      AND ProcCpuInfo = @pc 
                      AND API = @api 
                      AND Id != @exId 
                    LIMIT 1";
                cmd.Parameters.AddWithValue("@exId", excludeId.Value);
            }
            else
            {
                cmd.CommandText = $@"
                    SELECT COUNT(1) FROM {TABLE} 
                    WHERE PhoneModel = @m 
                      AND ProcVersion = @pv 
                      AND ProcCpuInfo = @pc 
                      AND API = @api 
                    LIMIT 1";
            }

            cmd.Parameters.AddWithValue("@m", phoneModel ?? "");
            cmd.Parameters.AddWithValue("@pv", procVersion ?? "");
            cmd.Parameters.AddWithValue("@pc", procCpuInfo ?? "");
            cmd.Parameters.AddWithValue("@api", api);

            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }

        /// <summary>
        /// Kiểm tra trùng với chính item (dùng khi Update)
        /// </summary>
        public static bool Exists(AoaLoop item)
        {
            if (item == null) return false;
            return Exists(item.PhoneModel, item.ProcVersion, item.ProcCpuInfo, item.API, item.Id);
        }

        // ==================== ADD ====================
        /// <summary>
        /// Thêm mới. Trả về false nếu trùng 4 trường unique.
        /// </summary>
        public static bool Add(AoaLoop item)
        {
            if (item == null) return false;

            // Check trùng
            if (Exists(item.PhoneModel, item.ProcVersion, item.ProcCpuInfo, item.API))
                return false;

            using var c = AOTSqliteDb.Open();
            using var cmd = c.CreateCommand();
            cmd.CommandText = $@"
            INSERT INTO {TABLE} 
            (PhoneModel, ProcVersion, ProcCpuInfo, API, PhysicalWidth, PhysicalHeight, PointCloseApp, Type, ArgsJson, ChildrenJson)
            VALUES 
            (@m, @pv, @pc, @api, @pw, @ph, @pca, @type, @args, @children);
            SELECT last_insert_rowid();";

            AddParameters(cmd, item);

            var result = cmd.ExecuteScalar();
            if (result != null && long.TryParse(result.ToString(), out long newId))
            {
                item.Id = (int)newId;
                return true;
            }
            return false;
        }

        // ==================== UPDATE ====================
        /// <summary>
        /// Cập nhật. Trả về false nếu không tìm thấy Id hoặc trùng unique với bản ghi khác.
        /// </summary>
        public static bool Update(AoaLoop item)
        {
            if (item == null || item.Id <= 0) return false;

            // Check trùng với bản ghi KHÁC (exclude chính nó)
            if (Exists(item.PhoneModel, item.ProcVersion, item.ProcCpuInfo, item.API, item.Id))
                return false;

            using var c = AOTSqliteDb.Open();
            using var cmd = c.CreateCommand();
            cmd.CommandText = $@"
            UPDATE {TABLE} SET 
                PhoneModel = @m,
                ProcVersion = @pv,
                ProcCpuInfo = @pc,
                API = @api,
                PhysicalWidth = @pw,
                PhysicalHeight = @ph,
                PointCloseApp = @pca,
                Type = @type,
                ArgsJson = @args,
                ChildrenJson = @children
            WHERE Id = @id";

            AddParameters(cmd, item);
            cmd.Parameters.AddWithValue("@id", item.Id);

            return cmd.ExecuteNonQuery() > 0;
        }

        // ==================== ADD OR UPDATE ====================
        /// <summary>
        /// Thêm nếu chưa có, ngược lại Update. Tiện cho import/sync.
        /// </summary>
        public static bool Save(AoaLoop item)
        {
            if (item == null) return false;

            // Tìm theo unique key
            var existing = FindOneByUnique(item.PhoneModel, item.ProcVersion, item.ProcCpuInfo, item.API);
            if (existing != null)
            {
                item.Id = existing.Id;
                return Update(item);
            }
            return Add(item);
        }

        // ==================== DELETE ====================
        public static bool Delete(int id)
        {
            using var c = AOTSqliteDb.Open();
            using var cmd = c.CreateCommand();
            cmd.CommandText = $"DELETE FROM {TABLE} WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            return cmd.ExecuteNonQuery() > 0;
        }

        public static bool DeleteByUnique(string phoneModel, string procVersion, string procCpuInfo, int api)
        {
            var item = FindOneByUnique(phoneModel, procVersion, procCpuInfo, api);
            if (item == null) return false;
            return Delete(item.Id);
        }

        // ==================== FIND ====================
        public static AoaLoop? FindOneById(int id)
        {
            using var c = AOTSqliteDb.Open();
            using var cmd = c.CreateCommand();
            cmd.CommandText = $"SELECT {COLS} FROM {TABLE} WHERE Id = @id LIMIT 1";
            cmd.Parameters.AddWithValue("@id", id);
            using var r = cmd.ExecuteReader();
            return r.Read() ? Map(r) : null;
        }

        public static AoaLoop? FindOneByUnique(string phoneModel, string procVersion, string procCpuInfo, int api)
        {
            using var c = AOTSqliteDb.Open();
            using var cmd = c.CreateCommand();
            cmd.CommandText = $@"
                SELECT {COLS} FROM {TABLE} 
                WHERE PhoneModel = @m 
                  AND ProcVersion = @pv 
                  AND ProcCpuInfo = @pc 
                  AND API = @api 
                LIMIT 1";
            cmd.Parameters.AddWithValue("@m", phoneModel ?? "");
            cmd.Parameters.AddWithValue("@pv", procVersion ?? "");
            cmd.Parameters.AddWithValue("@pc", procCpuInfo ?? "");
            cmd.Parameters.AddWithValue("@api", api);
            using var r = cmd.ExecuteReader();
            return r.Read() ? Map(r) : null;
        }

        public static List<AoaLoop> FindByPhoneModel(string phoneModel)
        {
            var list = new List<AoaLoop>();
            using var c = AOTSqliteDb.Open();
            using var cmd = c.CreateCommand();
            cmd.CommandText = $"SELECT {COLS} FROM {TABLE} WHERE PhoneModel = @m ORDER BY Id";
            cmd.Parameters.AddWithValue("@m", phoneModel ?? "");
            using var r = cmd.ExecuteReader();
            while (r.Read()) list.Add(Map(r));
            return list;
        }

        public static List<AoaLoop> FindByAPI(int api)
        {
            var list = new List<AoaLoop>();
            using var c = AOTSqliteDb.Open();
            using var cmd = c.CreateCommand();
            cmd.CommandText = $"SELECT {COLS} FROM {TABLE} WHERE API = @api ORDER BY Id";
            cmd.Parameters.AddWithValue("@api", api);
            using var r = cmd.ExecuteReader();
            while (r.Read()) list.Add(Map(r));
            return list;
        }

        public static List<AoaLoop> LoadAll()
        {
            var list = new List<AoaLoop>();
            using var c = AOTSqliteDb.Open();
            using var cmd = c.CreateCommand();
            cmd.CommandText = $"SELECT {COLS} FROM {TABLE} ORDER BY Id";
            using var r = cmd.ExecuteReader();
            while (r.Read()) list.Add(Map(r));
            return list;
        }

        // ==================== CHECK EXIST ====================
        public static bool IsAny()
        {
            using var c = AOTSqliteDb.Open();
            using var cmd = c.CreateCommand();
            cmd.CommandText = $"SELECT COUNT(1) FROM {TABLE}";
            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }

        public static bool IsAnyById(int id)
        {
            using var c = AOTSqliteDb.Open();
            using var cmd = c.CreateCommand();
            cmd.CommandText = $"SELECT COUNT(1) FROM {TABLE} WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }

        // ==================== HELPER ====================
        private static void AddParameters(SqliteCommand cmd, AoaLoop item)
        {
            cmd.Parameters.AddWithValue("@m", item.PhoneModel ?? "");
            cmd.Parameters.AddWithValue("@pv", item.ProcVersion ?? "");
            cmd.Parameters.AddWithValue("@pc", item.ProcCpuInfo ?? "");
            cmd.Parameters.AddWithValue("@api", item.API);
            cmd.Parameters.AddWithValue("@pw", item.PhysicalWidth);   // MỚI
            cmd.Parameters.AddWithValue("@ph", item.PhysicalHeight);  // MỚI
            cmd.Parameters.AddWithValue("@pca", item.PointCloseApp ?? "");
            cmd.Parameters.AddWithValue("@type", (int)item.Type);

            // Serialize Payload -> ArgsJson nếu Payload khác null
            if (item.Payload != null)
            {
                item.ArgsJson = JsonSerializer.Serialize(
                    item.Payload,
                    item.Payload.GetType(),
                    AoaLoopJsonContext.Default);
            }
            cmd.Parameters.AddWithValue("@args", item.ArgsJson ?? "{}");

            // Serialize Children -> JSON
            string childrenJson = "[]";
            if (item.Children?.Count > 0)
            {
                childrenJson = JsonSerializer.Serialize(
                    item.Children,
                    AoaLoopJsonContext.Default.ListAoaLoop);
            }
            cmd.Parameters.AddWithValue("@children", childrenJson);
        }
    }
}