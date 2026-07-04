using LibUsbDotNet;
using Microsoft.Data.Sqlite;
using System.Runtime.InteropServices;
using static System.Net.Mime.MediaTypeNames;

namespace Dragon.Database;

public static class AOTSqliteDb
{
    private static readonly string DbPath = Path.Combine(AppContext.BaseDirectory, "database.db");
    private static readonly string ConnStr = $"Data Source={DbPath};Pooling=False";

    static AOTSqliteDb()
    {

    }

    public static SqliteConnection Open()
    {
        var c = new SqliteConnection(ConnStr);
        c.Open();
        using var cmd = c.CreateCommand();
        cmd.CommandText = "PRAGMA busy_timeout=5000; PRAGMA journal_mode=WAL; PRAGMA synchronous=NORMAL; PRAGMA foreign_keys=ON;";
        cmd.ExecuteNonQuery();
        return c;
    }

    public static void EnsureCreated()
    {
        try
        {
            var folder = Path.GetDirectoryName(DbPath);
            if (!string.IsNullOrEmpty(folder) && !Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            using var c = new SqliteConnection(ConnStr);
            c.Open();

            string[] scripts = new[]
            {
                "PRAGMA journal_mode=WAL;",
                "PRAGMA synchronous=NORMAL;",
                "PRAGMA foreign_keys=ON;",

                // 1. AppDatas
                @"CREATE TABLE IF NOT EXISTS AppDatas (
                    guId TEXT PRIMARY KEY,
                    DeviceID TEXT NOT NULL,
                    NetworkName TEXT NOT NULL,
                    PackageName TEXT,
                    DeviceModel TEXT,
                    AccStatus INTEGER NOT NULL DEFAULT 0,
                    Email TEXT,
                    PassEmail TEXT,
                    Username TEXT,
                    Password TEXT,
                    PrivateKey TEXT,
                    Phone TEXT,
                    HoTen TEXT,
                    BirtDay TEXT,
                    Gender INTEGER NOT NULL DEFAULT 0,
                    PrimaryEducation TEXT,
                    SecondaryEducation TEXT,
                    Marital INTEGER NOT NULL DEFAULT 0,
                    Token TEXT,
                    Cookies TEXT,
                    FriendCount INTEGER DEFAULT 0,
                    GroupCount INTEGER DEFAULT 0,
                    FollowCount INTEGER DEFAULT 0,
                    Avatar TEXT,
                    DataGroup TEXT,
                    DataFriend TEXT,
                    DataFollow TEXT,
                    AppVersion TEXT,
                    ABI TEXT,
                    NetworkGuId TEXT
                );",
                "CREATE UNIQUE INDEX IF NOT EXISTS IX_AppData_Device_Network ON AppDatas(DeviceID, NetworkName);",
                "CREATE INDEX IF NOT EXISTS IX_AppData_DeviceID ON AppDatas(DeviceID);",

                // 2. AppDataBackups
                @"CREATE TABLE IF NOT EXISTS AppDataBackups (
                    guId TEXT PRIMARY KEY,
                    DeviceID TEXT NOT NULL,
                    NetworkName TEXT NOT NULL,
                    PackageName TEXT,
                    Email TEXT,
                    PassEmail TEXT,
                    Username TEXT,
                    Password TEXT,
                    PrivateKey TEXT,
                    Phone TEXT,
                    HoTen TEXT,
                    BirtDay TEXT,
                    Gender INTEGER NOT NULL DEFAULT 0,
                    PrimaryEducation TEXT,
                    SecondaryEducation TEXT,
                    Marital INTEGER NOT NULL DEFAULT 0,
                    Token TEXT,
                    Cookies TEXT,
                    FriendCount INTEGER DEFAULT 0,
                    GroupCount INTEGER DEFAULT 0,
                    FollowCount INTEGER DEFAULT 0,
                    Avatar TEXT,
                    DataGroup TEXT,
                    DataFriend TEXT,
                    DataFollow TEXT,
                    AppVersion TEXT,
                    ABI TEXT,
                    NetworkGuId TEXT
                );",
                "CREATE UNIQUE INDEX IF NOT EXISTS IX_Backup_Device_Network ON AppDataBackups(DeviceID, NetworkName);",

                // 3. SocialNetworks
                @"CREATE TABLE IF NOT EXISTS SocialNetworks (
                    guId TEXT PRIMARY KEY,
                    NetworkName TEXT NOT NULL UNIQUE
                );",

                // 4. SocialNetworkBackups
                @"CREATE TABLE IF NOT EXISTS SocialNetworkBackups (
                    guId TEXT PRIMARY KEY,
                    NetworkName TEXT NOT NULL UNIQUE
                );",

                // 5. PhoneBoxes
                @"CREATE TABLE IF NOT EXISTS PhoneBoxes (
                    Id INTEGER PRIMARY KEY
                );",

                // 6. PhoneTemplates
                @"CREATE TABLE IF NOT EXISTS PhoneTemplates (
                    Id INTEGER PRIMARY KEY,
                    Name TEXT NOT NULL,
                    Model TEXT,
                    Product TEXT,
                    PhysicalWidth INTEGER DEFAULT 0,
                    PhysicalHeight INTEGER DEFAULT 0,
                    API INTEGER DEFAULT 0
                );",

                // 7. Phones
                @"CREATE TABLE IF NOT EXISTS Phones (
                    Id INTEGER PRIMARY KEY,
                    PhoneTagNumber INTEGER NOT NULL,
                    PhoneBoxId INTEGER,
                    DeviceID TEXT,
                    Serial TEXT,
                    DeviceState INTEGER NOT NULL DEFAULT 0, 
                    PhoneMode INTEGER NOT NULL,
                    Model TEXT,
                    Product TEXT,
                    Usb TEXT,
                    Ipv4 TEXT,
                    Message TEXT,
                    TransportId TEXT,
                    IsUHDI INTEGER DEFAULT 0,
                    PhysicalWidth INTEGER DEFAULT 0,
                    PhysicalHeight INTEGER DEFAULT 0,
                    IsRunning INTEGER DEFAULT 0,
                    AndroidVersion TEXT,
                    API INTEGER DEFAULT 0,
                    IsUseUSB INTEGER DEFAULT 0,
                    ProcVersion TEXT,
                    ProcCpuInfo TEXT,
                    IsAccessibleAppInstall INTEGER DEFAULT 0,
                    IsPingWifi INTEGER DEFAULT 0,
                    IsRooted INTEGER DEFAULT 0,
                    IsMagisk INTEGER DEFAULT 0,
                    IsKernelSu INTEGER DEFAULT 0,
                    PhoneHash TEXT,
                    FOREIGN KEY(PhoneBoxId) REFERENCES PhoneBoxes(Id) ON DELETE SET NULL
                );",
                "CREATE INDEX IF NOT EXISTS IX_Phones_DeviceID ON Phones(DeviceID);",
                "CREATE INDEX IF NOT EXISTS IX_Phones_Serial ON Phones(Serial);",

                // 8. KeybroadSettings
                @"CREATE TABLE IF NOT EXISTS KeybroadSettings (
                    Id INTEGER PRIMARY KEY,
                    DeviceId TEXT,
                    Langeuage INTEGER NOT NULL DEFAULT 0,
                    IMEId TEXT
                );",

                // 9. AdbCommandIntents
                @"CREATE TABLE IF NOT EXISTS AdbCommandIntents (
                    Id INTEGER PRIMARY KEY,
                    Model TEXT,
                    PackageName TEXT,
                    VersionName TEXT,
                    VersionCode INTEGER DEFAULT 0,
                    Name TEXT,
                    Command TEXT
                );",

                // 10. InstallApks 
                @"CREATE TABLE IF NOT EXISTS InstallApks (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    AppName TEXT,
                    PackageName TEXT,
                    VersionName TEXT,
                    VersionCode TEXT,
                    MinAPI INTEGER,
                    MaxAPI INTEGER,
                    ABIs TEXT,
                    Path TEXT UNIQUE
                );",

                @"CREATE TABLE IF NOT EXISTS DeviceBackups (
                    DeviceId TEXT PRIMARY KEY,
                    Service TEXT,
                    DeviceInterfaceGuidsCsv TEXT
                );",

                @"CREATE TABLE IF NOT EXISTS KeypadStrings (
                    DeviceId TEXT PRIMARY KEY,
                    Model TEXT,
                    PassCode TEXT,
                    num0 TEXT,
                    num1 TEXT,
                    num2 TEXT,
                    num3 TEXT,
                    num4 TEXT,
                    num5 TEXT,
                    num6 TEXT,
                    num7 TEXT,
                    num8 TEXT,
                    num9 TEXT,
                    OK TEXT
                );"





            };

            foreach (var sql in scripts)
            {
                using var cmd = c.CreateCommand();
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"EnsureCreated failed: {ex.Message}");
            throw;
        }
    }

    public static string GetDatabasePath() => DbPath;
}
