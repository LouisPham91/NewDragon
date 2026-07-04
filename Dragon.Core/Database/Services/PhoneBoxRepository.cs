using Dragon.Database;
using Dragon.Database.Models;

namespace Dragon.Controller.Database.Services
{
    public static class PhoneBoxRepository
    {
        // Tìm ID trống nhỏ nhất, bắt đầu từ 1
        public static int GetNextAvailableId()
        {
            using var connection = AOTSqliteDb.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT Id FROM PhoneBoxes ORDER BY Id ASC";
            using var reader = command.ExecuteReader();

            int expected = 1;
            while (reader.Read())
            {
                int current = reader.GetInt32(0);
                if (current == expected)
                {
                    expected++;          // 1,2,3... vẫn liền mạch
                }
                else if (current > expected)
                {
                    break;               // gặp khoảng trống, ví dụ có 2 mà thiếu 1
                }
            }
            return expected; // nếu DB rỗng -> 1, nếu có 2,5 -> 1, lần sau -> 3
        }
        // Tạo mới và tự gán Id
        public static PhoneBox AddNew()
        {
            var box = new PhoneBox { Id = GetNextAvailableId() };
            Add(box); // dùng hàm Add cũ của bạn
            return box;
        }
        public static bool Add(PhoneBox phoneBox)
        {
            using var connection = AOTSqliteDb.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "INSERT OR IGNORE INTO PhoneBoxes (Id) VALUES (@Id)";
            command.Parameters.AddWithValue("@Id", phoneBox.Id);
            return command.ExecuteNonQuery() > 0;
        }

        public static bool AddIf(PhoneBox phoneBox)
        {
            if (FindOneByID(phoneBox.Id) != null)
                return false;
            return Add(phoneBox);
        }

        public static PhoneBox? FindOneByID(int id)
        {
            using var connection = AOTSqliteDb.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT Id FROM PhoneBoxes WHERE Id=@Id LIMIT 1";
            command.Parameters.AddWithValue("@Id", id);
            using var reader = command.ExecuteReader();
            return reader.Read() ? new PhoneBox { Id = reader.GetInt32(0) } : null;
        }

        public static List<PhoneBox> FindMany(List<int> ids)
        {
            var list = new List<PhoneBox>();
            using var connection = AOTSqliteDb.Open();
            foreach (var id in ids)
            {
                using var command = connection.CreateCommand();
                command.CommandText = "SELECT Id FROM PhoneBoxes WHERE Id=@Id";
                command.Parameters.AddWithValue("@Id", id);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new PhoneBox { Id = reader.GetInt32(0) });
                }
            }
            return list;
        }
        public static bool Delete(int id)
        {
            using var connection = AOTSqliteDb.Open();
            using var transaction = connection.BeginTransaction();
            try
            {
                // B1: gỡ tất cả phone đang trỏ tới box này
                using (var cmdUpdate = connection.CreateCommand())
                {
                    cmdUpdate.Transaction = transaction;
                    cmdUpdate.CommandText = "UPDATE Phones SET PhoneBoxId = NULL WHERE PhoneBoxId = @Id";
                    cmdUpdate.Parameters.AddWithValue("@Id", id);
                    cmdUpdate.ExecuteNonQuery();
                }

                // B2: xóa box
                using (var cmdDelete = connection.CreateCommand())
                {
                    cmdDelete.Transaction = transaction;
                    cmdDelete.CommandText = "DELETE FROM PhoneBoxes WHERE Id = @Id";
                    cmdDelete.Parameters.AddWithValue("@Id", id);
                    int rows = cmdDelete.ExecuteNonQuery();

                    transaction.Commit();
                    return rows > 0;
                }
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
        public static bool IsAny()
        {
            using var connection = AOTSqliteDb.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(1) FROM PhoneBoxes";
            return Convert.ToInt32(command.ExecuteScalar()) > 0;
        }

        public static List<PhoneBox> LoadAll()
        {
            var list = new List<PhoneBox>();
            using var connection = AOTSqliteDb.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT Id FROM PhoneBoxes ORDER BY Id";
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new PhoneBox { Id = reader.GetInt32(0) });
            }
            return list;
        }

        public static void ReorderPhoneBoxes()
        {
            using var connection = AOTSqliteDb.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                // 1. Lấy danh sách PhoneBoxes
                var boxes = new List<int>();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id FROM PhoneBoxes ORDER BY Id";
                    using var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        boxes.Add(reader.GetInt32(0));
                    }
                }

                // 2. Lấy danh sách Phones
                var phones = new List<(int Id, int? PhoneBoxId)>();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, PhoneBoxId FROM Phones";
                    using var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        int? phoneBoxId = reader.IsDBNull(1) ? null : reader.GetInt32(1);
                        phones.Add((id, phoneBoxId));
                    }
                }

                // 3. Tạo map Id cũ -> Id mới
                var idMap = boxes.Select((oldId, index) => new { oldId, newId = index + 1 })
                                 .ToDictionary(x => x.oldId, x => x.newId);

                // 4. Xóa PhoneBoxes cũ
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM PhoneBoxes";
                    cmd.ExecuteNonQuery();
                }

                // 5. Cập nhật Phones với PhoneBoxId mới
                foreach (var phone in phones)
                {
                    if (phone.PhoneBoxId.HasValue && idMap.TryGetValue(phone.PhoneBoxId.Value, out var newId))
                    {
                        using var cmd = connection.CreateCommand();
                        cmd.CommandText = "UPDATE Phones SET PhoneBoxId=@NewId WHERE Id=@Id";
                        cmd.Parameters.AddWithValue("@NewId", newId);
                        cmd.Parameters.AddWithValue("@Id", phone.Id);
                        cmd.ExecuteNonQuery();
                    }
                }

                // 6. Thêm lại PhoneBoxes với Id mới
                foreach (var newId in idMap.Values)
                {
                    using var cmd = connection.CreateCommand();
                    cmd.CommandText = "INSERT INTO PhoneBoxes (Id) VALUES (@Id)";
                    cmd.Parameters.AddWithValue("@Id", newId);
                    cmd.ExecuteNonQuery();
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

    }
}
