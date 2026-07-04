using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Data.Sqlite;
using Dragon.Database;
using Dragon.Controller.Database.Models;

namespace Dragon.Controller.DeviceControl.OTG;


public class DeviceBackupRepository
{
    public DeviceBackupRepository()
    {
        // đảm bảo bảng đã tạo
        AOTSqliteDb.EnsureCreated();
    }

    public async Task SaveAsync(DeviceBackup backup)
    {
        if (backup == null || string.IsNullOrWhiteSpace(backup.DeviceId))
            throw new ArgumentException("DeviceId required");

        using var c = AOTSqliteDb.Open();
        using var cmd = c.CreateCommand();
        cmd.CommandText = @"
            INSERT INTO DeviceBackups (DeviceId, Service, DeviceInterfaceGuidsCsv)
            VALUES ($id, $svc, $guids)
            ON CONFLICT(DeviceId) DO UPDATE SET
                Service = excluded.Service,
                DeviceInterfaceGuidsCsv = excluded.DeviceInterfaceGuidsCsv;";
        cmd.Parameters.AddWithValue("$id", backup.DeviceId);
        cmd.Parameters.AddWithValue("$svc", (object?)backup.Service ?? DBNull.Value);
        cmd.Parameters.AddWithValue("$guids", (object?)backup.DeviceInterfaceGuidsCsv ?? DBNull.Value);
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<DeviceBackup?> FindOneAsync(string deviceId)
    {
        using var c = AOTSqliteDb.Open();
        using var cmd = c.CreateCommand();
        cmd.CommandText = "SELECT DeviceId, Service, DeviceInterfaceGuidsCsv FROM DeviceBackups WHERE DeviceId = $id LIMIT 1;";
        cmd.Parameters.AddWithValue("$id", deviceId);
        using var r = await cmd.ExecuteReaderAsync();
        if (await r.ReadAsync())
        {
            return new DeviceBackup
            {
                DeviceId = r.GetString(0),
                Service = r.IsDBNull(1) ? null : r.GetString(1),
                DeviceInterfaceGuidsCsv = r.IsDBNull(2) ? null : r.GetString(2)
            };
        }
        return null;
    }

    public async Task<List<DeviceBackup>> FindAllAsync()
    {
        var list = new List<DeviceBackup>();
        using var c = AOTSqliteDb.Open();
        using var cmd = c.CreateCommand();
        cmd.CommandText = "SELECT DeviceId, Service, DeviceInterfaceGuidsCsv FROM DeviceBackups;";
        using var r = await cmd.ExecuteReaderAsync();
        while (await r.ReadAsync())
        {
            list.Add(new DeviceBackup
            {
                DeviceId = r.GetString(0),
                Service = r.IsDBNull(1) ? null : r.GetString(1),
                DeviceInterfaceGuidsCsv = r.IsDBNull(2) ? null : r.GetString(2)
            });
        }
        return list;
    }

    public async Task<bool> RemoveAsync(string deviceId)
    {
        using var c = AOTSqliteDb.Open();
        using var cmd = c.CreateCommand();
        cmd.CommandText = "DELETE FROM DeviceBackups WHERE DeviceId = $id;";
        cmd.Parameters.AddWithValue("$id", deviceId);
        var rows = await cmd.ExecuteNonQueryAsync();
        return rows > 0;
    }

    public async Task<bool> ExistsAsync(string deviceId)
    {
        using var c = AOTSqliteDb.Open();
        using var cmd = c.CreateCommand();
        cmd.CommandText = "SELECT 1 FROM DeviceBackups WHERE DeviceId = $id LIMIT 1;";
        cmd.Parameters.AddWithValue("$id", deviceId);
        var result = await cmd.ExecuteScalarAsync();
        return result != null;
    }

    public async Task<int> CountAsync()
    {
        using var c = AOTSqliteDb.Open();
        using var cmd = c.CreateCommand();
        cmd.CommandText = "SELECT COUNT(*) FROM DeviceBackups;";
        var result = await cmd.ExecuteScalarAsync();
        return Convert.ToInt32(result);
    }
}
