using Dragon.Controller.Database.Models;
using Dragon.Controller.DeviceControl.OTG;
using Dragon.Database;
using Microsoft.Data.Sqlite;

namespace Dragon.Controller.Database;

public class KeypadStringRepository
{
    public static async Task SaveAsync(KeypadString kp)
    {
        using var c = AOTSqliteDb.Open();
        using var cmd = c.CreateCommand();
        cmd.CommandText = @"
        INSERT INTO KeypadStrings (DeviceId, Model, PassCode, num0, num1, num2, num3, num4, num5, num6, num7, num8, num9, OK)
        VALUES ($id,$model,$pass,$n0,$n1,$n2,$n3,$n4,$n5,$n6,$n7,$n8,$n9,$ok)
        ON CONFLICT(DeviceId) DO UPDATE SET
            Model=excluded.Model,
            PassCode=excluded.PassCode,
            num0=excluded.num0,
            num1=excluded.num1,
            num2=excluded.num2,
            num3=excluded.num3,
            num4=excluded.num4,
            num5=excluded.num5,
            num6=excluded.num6,
            num7=excluded.num7,
            num8=excluded.num8,
            num9=excluded.num9,
            OK=excluded.OK;
        ";
        cmd.Parameters.AddWithValue("$id", kp.DeviceId ?? "");
        cmd.Parameters.AddWithValue("$model", kp.Model ?? "");
        cmd.Parameters.AddWithValue("$pass", kp.PassCoce ?? "123456");
        cmd.Parameters.AddWithValue("$n0", kp.num0);
        cmd.Parameters.AddWithValue("$n1", kp.num1);
        cmd.Parameters.AddWithValue("$n2", kp.num2);
        cmd.Parameters.AddWithValue("$n3", kp.num3);
        cmd.Parameters.AddWithValue("$n4", kp.num4);
        cmd.Parameters.AddWithValue("$n5", kp.num5);
        cmd.Parameters.AddWithValue("$n6", kp.num6);
        cmd.Parameters.AddWithValue("$n7", kp.num7);
        cmd.Parameters.AddWithValue("$n8", kp.num8);
        cmd.Parameters.AddWithValue("$n9", kp.num9);
        cmd.Parameters.AddWithValue("$ok", kp.OK);
        await cmd.ExecuteNonQueryAsync();
    }


    public static async Task SaveAllSelectsAsync(KeypadString template, List<KeypadString> listKeypads)
    {
        if (template == null) throw new ArgumentNullException(nameof(template));
        using var c = AOTSqliteDb.Open();
        using var tx = c.BeginTransaction();
        using var cmd = c.CreateCommand();
        cmd.Transaction = tx;
        cmd.CommandText = @"
        UPDATE KeypadStrings SET
            Model=$model,
            PassCode=$pass,
            num0=$n0, num1=$n1, num2=$n2, num3=$n3, num4=$n4,
            num5=$n5, num6=$n6, num7=$n7, num8=$n8, num9=$n9,
            OK=$ok
        WHERE DeviceId=$id;
        ";
        var pId = cmd.Parameters.Add("$id", SqliteType.Text);
        var pModel = cmd.Parameters.Add("$model", SqliteType.Text);
        var pPass = cmd.Parameters.Add("$pass", SqliteType.Text);
        var p0 = cmd.Parameters.Add("$n0", SqliteType.Text);
        var p1 = cmd.Parameters.Add("$n1", SqliteType.Text);
        var p2 = cmd.Parameters.Add("$n2", SqliteType.Text);
        var p3 = cmd.Parameters.Add("$n3", SqliteType.Text);
        var p4 = cmd.Parameters.Add("$n4", SqliteType.Text);
        var p5 = cmd.Parameters.Add("$n5", SqliteType.Text);
        var p6 = cmd.Parameters.Add("$n6", SqliteType.Text);
        var p7 = cmd.Parameters.Add("$n7", SqliteType.Text);
        var p8 = cmd.Parameters.Add("$n8", SqliteType.Text);
        var p9 = cmd.Parameters.Add("$n9", SqliteType.Text);
        var pOk = cmd.Parameters.Add("$ok", SqliteType.Text);

        // Hàm local để gán param
        void SetParams(string deviceId)
        {
            pId.Value = deviceId;
            pModel.Value = template.Model ?? "";
            pPass.Value = template.PassCoce ?? "123456";
            p0.Value = template.num0;
            p1.Value = template.num1;
            p2.Value = template.num2;
            p3.Value = template.num3;
            p4.Value = template.num4;
            p5.Value = template.num5;
            p6.Value = template.num6;
            p7.Value = template.num7;
            p8.Value = template.num8;
            p9.Value = template.num9;
            pOk.Value = template.OK;
        }

        // 1. Update chính template
        SetParams(template.DeviceId);
        await cmd.ExecuteNonQueryAsync();

        // 2. Update tất cả trong list
        if (listKeypads != null)
        {
            foreach (var kp in listKeypads)
            {
                if (string.IsNullOrEmpty(kp?.DeviceId) || kp.DeviceId == template.DeviceId) continue;
                SetParams(kp.DeviceId);
                await cmd.ExecuteNonQueryAsync();
            }
        }

        await tx.CommitAsync();
    }

    public static async Task<KeypadString?> FindOneAsync(string deviceId)
    {
        using var c = AOTSqliteDb.Open();
        using var cmd = c.CreateCommand();
        cmd.CommandText = "SELECT DeviceId, Model, PassCode, num0,num1,num2,num3,num4,num5,num6,num7,num8,num9,OK FROM KeypadStrings WHERE DeviceId=$id LIMIT 1;";
        cmd.Parameters.AddWithValue("$id", deviceId);
        using var r = await cmd.ExecuteReaderAsync();
        if (await r.ReadAsync())
        {
            return new KeypadString
            {
                DeviceId = r.GetString(0),
                Model = r.IsDBNull(1) ? "" : r.GetString(1),
                PassCoce = r.IsDBNull(2) ? "123789" : r.GetString(2),
                num0 = r.GetString(3),
                num1 = r.GetString(4),
                num2 = r.GetString(5),
                num3 = r.GetString(6),
                num4 = r.GetString(7),
                num5 = r.GetString(8),
                num6 = r.GetString(9),
                num7 = r.GetString(10),
                num8 = r.GetString(11),
                num9 = r.GetString(12),
                OK = r.GetString(13)
            };
        }
        return null;
    }

    public static async Task<List<KeypadString>> GetAllAsync()
    {
        var list = new List<KeypadString>();
        using var c = AOTSqliteDb.Open();
        using var cmd = c.CreateCommand();
        cmd.CommandText = "SELECT DeviceId, Model, PassCode, num0,num1,num2,num3,num4,num5,num6,num7,num8,num9,OK FROM KeypadStrings;";
        using var r = await cmd.ExecuteReaderAsync();
        while (await r.ReadAsync())
        {
            list.Add(new KeypadString
            {
                DeviceId = r.GetString(0),
                Model = r.IsDBNull(1) ? "" : r.GetString(1),
                PassCoce = r.IsDBNull(2) ? "123456" : r.GetString(2),
                num0 = r.GetString(3),
                num1 = r.GetString(4),
                num2 = r.GetString(5),
                num3 = r.GetString(6),
                num4 = r.GetString(7),
                num5 = r.GetString(8),
                num6 = r.GetString(9),
                num7 = r.GetString(10),
                num8 = r.GetString(11),
                num9 = r.GetString(12),
                OK = r.GetString(13)
            });
        }
        return list;
    }

    public static async Task RemoveAsync(string deviceId)
    {
        using var c = AOTSqliteDb.Open();
        using var cmd = c.CreateCommand();
        cmd.CommandText = "DELETE FROM KeypadStrings WHERE DeviceId=$id;";
        cmd.Parameters.AddWithValue("$id", deviceId);
        await cmd.ExecuteNonQueryAsync();
    }
}