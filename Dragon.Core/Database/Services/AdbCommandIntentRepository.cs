
using Dragon.Database.Models;

namespace Dragon.Database.Services
{
    public static class AdbCommandIntentRepository
    {
        public static bool Add(AdbCommandIntent item)
        {
            using var connection = AOTSqliteDb.Open();
            using var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO AdbCommandIntents (Model, PackageName, VersionName, VersionCode, Name, Command)
                VALUES (@Model, @PackageName, @VersionName, @VersionCode, @Name, @Command);
                SELECT last_insert_rowid();";
            command.Parameters.AddWithValue("@Model", item.Model ?? string.Empty);
            command.Parameters.AddWithValue("@PackageName", item.PackageName ?? string.Empty);
            command.Parameters.AddWithValue("@VersionName", item.VersionName ?? string.Empty);
            command.Parameters.AddWithValue("@VersionCode", item.VersionCode);
            command.Parameters.AddWithValue("@Name", item.Name ?? string.Empty);
            command.Parameters.AddWithValue("@Command", item.Command ?? string.Empty);
            var result = command.ExecuteScalar();
            if (result != null && int.TryParse(result.ToString(), out int newId))
            {
                item.Id = newId;
                return true;
            }
            return false;
        }

        public static bool AddIfNotExists(AdbCommandIntent item)
        {
            using var connection = AOTSqliteDb.Open();
            using var checkCommand = connection.CreateCommand();
            checkCommand.CommandText = "SELECT COUNT(1) FROM AdbCommandIntents WHERE Model = @Model AND PackageName = @PackageName AND Command = @Command LIMIT 1";
            checkCommand.Parameters.AddWithValue("@Model", item.Model ?? string.Empty);
            checkCommand.Parameters.AddWithValue("@PackageName", item.PackageName ?? string.Empty);
            checkCommand.Parameters.AddWithValue("@Command", item.Command ?? string.Empty);
            var exists = Convert.ToInt32(checkCommand.ExecuteScalar()) > 0;
            if (exists)
            {
                return false;
            }
            return Add(item);
        }

        public static AdbCommandIntent? FindOneById(int id)
        {
            using var connection = AOTSqliteDb.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, Model, PackageName, VersionName, VersionCode, Name, Command FROM AdbCommandIntents WHERE Id = @Id LIMIT 1";
            command.Parameters.AddWithValue("@Id", id);
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new AdbCommandIntent
                {
                    Id = reader.GetInt32(0),
                    Model = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                    PackageName = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    VersionName = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    VersionCode = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                    Name = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                    Command = reader.IsDBNull(6) ? string.Empty : reader.GetString(6)
                };
            }
            return null;
        }

        public static List<AdbCommandIntent> FindByModelAndPackage(string model, string packageName)
        {
            var list = new List<AdbCommandIntent>();
            using var connection = AOTSqliteDb.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, Model, PackageName, VersionName, VersionCode, Name, Command FROM AdbCommandIntents WHERE Model = @Model AND PackageName = @PackageName ORDER BY Id";
            command.Parameters.AddWithValue("@Model", model ?? string.Empty);
            command.Parameters.AddWithValue("@PackageName", packageName ?? string.Empty);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new AdbCommandIntent
                {
                    Id = reader.GetInt32(0),
                    Model = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                    PackageName = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    VersionName = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    VersionCode = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                    Name = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                    Command = reader.IsDBNull(6) ? string.Empty : reader.GetString(6)
                });
            }
            return list;
        }

        public static List<AdbCommandIntent> FindMany(Func<AdbCommandIntent, bool> predicate)
        {
            var list = new List<AdbCommandIntent>();
            if (predicate == null) return list;

            try
            {
                using var connection = AOTSqliteDb.Open();
                using var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, Model, PackageName, VersionName, VersionCode, Name, Command FROM AdbCommandIntents ORDER BY Model, PackageName, Id";
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var item = new AdbCommandIntent
                    {
                        Id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                        Model = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                        PackageName = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                        VersionName = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                        VersionCode = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                        Name = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                        Command = reader.IsDBNull(6) ? string.Empty : reader.GetString(6)
                    };
                    list.Add(item);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"FindMany error: {ex.InnerException?.Message ?? ex.Message}");
            }

            return list.Where(predicate).ToList();
        }
        public static List<AdbCommandIntent> FindManyByPackageName(string packageName)
        {
            var list = new List<AdbCommandIntent>();
            if (string.IsNullOrWhiteSpace(packageName)) return list;

            try
            {
                using var connection = AOTSqliteDb.Open();
                using var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, Model, PackageName, VersionName, VersionCode, Name, Command FROM AdbCommandIntents WHERE PackageName = @PackageName ORDER BY Id";
                command.Parameters.AddWithValue("@PackageName", packageName.Trim());
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new AdbCommandIntent
                    {
                        Id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                        Model = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                        PackageName = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                        VersionName = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                        VersionCode = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                        Name = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                        Command = reader.IsDBNull(6) ? string.Empty : reader.GetString(6)
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"FindManyByPackageName error: {ex.InnerException?.Message ?? ex.Message}");
            }
            return list;
        }

        /// <summary>
        /// Truy vấn trực tiếp theo Model (parameterized). Hiệu quả hơn khi bảng lớn.
        /// Trả về danh sách AdbCommandIntent có Model khớp (case-sensitive).
        /// Nếu cần tìm không phân biệt hoa thường, truyền model.ToLower() và dùng LOWER(Model) trong SQL.
        /// </summary>
        public static List<AdbCommandIntent> FindManyByModel(string model)
        {
            var list = new List<AdbCommandIntent>();
            if (string.IsNullOrWhiteSpace(model)) return list;

            using var connection = AOTSqliteDb.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, Model, PackageName, VersionName, VersionCode, Name, Command FROM AdbCommandIntents WHERE Model = @Model ORDER BY Id";
            command.Parameters.AddWithValue("@Model", model.Trim());
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new AdbCommandIntent
                {
                    Id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                    Model = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                    PackageName = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    VersionName = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    VersionCode = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                    Name = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                    Command = reader.IsDBNull(6) ? string.Empty : reader.GetString(6)
                });
            }
            return list;
        }

        public static List<AdbCommandIntent> LoadAll()
        {
            var list = new List<AdbCommandIntent>();
            using var connection = AOTSqliteDb.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, Model, PackageName, VersionName, VersionCode, Name, Command FROM AdbCommandIntents ORDER BY Model, PackageName, Id";
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new AdbCommandIntent
                {
                    Id = reader.GetInt32(0),
                    Model = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                    PackageName = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    VersionName = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    VersionCode = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                    Name = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                    Command = reader.IsDBNull(6) ? string.Empty : reader.GetString(6)
                });
            }
            return list;
        }

        public static bool Update(AdbCommandIntent item)
        {
            using var connection = AOTSqliteDb.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "UPDATE AdbCommandIntents SET Model = @Model, PackageName = @PackageName, VersionName = @VersionName, VersionCode = @VersionCode, Name = @Name, Command = @Command WHERE Id = @Id";
            command.Parameters.AddWithValue("@Model", item.Model ?? string.Empty);
            command.Parameters.AddWithValue("@PackageName", item.PackageName ?? string.Empty);
            command.Parameters.AddWithValue("@VersionName", item.VersionName ?? string.Empty);
            command.Parameters.AddWithValue("@VersionCode", item.VersionCode);
            command.Parameters.AddWithValue("@Name", item.Name ?? string.Empty);
            command.Parameters.AddWithValue("@Command", item.Command ?? string.Empty);
            command.Parameters.AddWithValue("@Id", item.Id);
            return command.ExecuteNonQuery() > 0;
        }

        public static bool Delete(int id)
        {
            using var connection = AOTSqliteDb.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM AdbCommandIntents WHERE Id = @Id";
            command.Parameters.AddWithValue("@Id", id);
            return command.ExecuteNonQuery() > 0;
        }

        public static int BulkInsert(IEnumerable<AdbCommandIntent> items)
        {
            int count = 0;
            using var connection = AOTSqliteDb.Open();
            using var transaction = connection.BeginTransaction();
            try
            {
                foreach (var item in items)
                {
                    using var command = connection.CreateCommand();
                    command.Transaction = transaction;
                    command.CommandText = "INSERT INTO AdbCommandIntents (Model, PackageName, VersionName, VersionCode, Name, Command) VALUES (@Model, @PackageName, @VersionName, @VersionCode, @Name, @Command)";
                    command.Parameters.AddWithValue("@Model", item.Model ?? string.Empty);
                    command.Parameters.AddWithValue("@PackageName", item.PackageName ?? string.Empty);
                    command.Parameters.AddWithValue("@VersionName", item.VersionName ?? string.Empty);
                    command.Parameters.AddWithValue("@VersionCode", item.VersionCode);
                    command.Parameters.AddWithValue("@Name", item.Name ?? string.Empty);
                    command.Parameters.AddWithValue("@Command", item.Command ?? string.Empty);
                    count += command.ExecuteNonQuery();
                }
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
            return count;
        }

        public static bool IsAny()
        {
            using var connection = AOTSqliteDb.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(1) FROM AdbCommandIntents";
            return Convert.ToInt32(command.ExecuteScalar()) > 0;
        }
        /// <summary>
        /// Kiểm tra tồn tại AdbCommandIntent theo Id.
        /// Trả về true nếu có bản ghi với Id tương ứng.
        /// </summary>
        public static bool IsAny(int id)
        {
            if (id <= 0) return false;
            try
            {
                using var connection = AOTSqliteDb.Open();
                using var command = connection.CreateCommand();
                command.CommandText = "SELECT COUNT(1) FROM AdbCommandIntents WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", id);
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"IsAny(Id) error: {ex.InnerException?.Message ?? ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Kiểm tra tồn tại AdbCommandIntent theo các trường Name, PackageName, VersionName, VersionCode,
        /// có thể loại trừ một Id cụ thể (để kiểm tra khi cập nhật).
        /// Trả về true nếu tồn tại bản ghi khác thỏa điều kiện.
        /// </summary>
        public static bool IsAny(string name, string packageName, string versionName, int versionCode, int? excludeId = null)
        {
            using var connection = AOTSqliteDb.Open();
            using var command = connection.CreateCommand();

            if (excludeId.HasValue)
            {
                command.CommandText = @"
            SELECT COUNT(1)
            FROM AdbCommandIntents
            WHERE Name = @Name
              AND PackageName = @PackageName
              AND VersionName = @VersionName
              AND VersionCode = @VersionCode
              AND Id != @ExcludeId
            LIMIT 1";
                command.Parameters.AddWithValue("@ExcludeId", excludeId.Value);
            }
            else
            {
                command.CommandText = @"
            SELECT COUNT(1)
            FROM AdbCommandIntents
            WHERE Name = @Name
              AND PackageName = @PackageName
              AND VersionName = @VersionName
              AND VersionCode = @VersionCode
            LIMIT 1";
            }

            command.Parameters.AddWithValue("@Name", name ?? string.Empty);
            command.Parameters.AddWithValue("@PackageName", packageName ?? string.Empty);
            command.Parameters.AddWithValue("@VersionName", versionName ?? string.Empty);
            command.Parameters.AddWithValue("@VersionCode", versionCode);

            return Convert.ToInt32(command.ExecuteScalar()) > 0;
        }

        /// <summary>
        /// Tiện ích: kiểm tra tồn tại dựa trên một đối tượng AdbCommandIntent (loại trừ chính nó theo Id).
        /// Sử dụng khi muốn kiểm tra trước khi cập nhật item hiện tại.
        /// </summary>
        public static bool IsAny(AdbCommandIntent item)
        {
            if (item == null) return false;
            return IsAny(item.Name, item.PackageName, item.VersionName, item.VersionCode, item.Id);
        }

    }
}
