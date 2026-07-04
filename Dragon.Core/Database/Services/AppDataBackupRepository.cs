using System;
using System.Collections.Generic;
using Dragon.Database;
using Dragon.Database.Models;
using Microsoft.Data.Sqlite;

namespace Dragon.Database.Services
{
    public static class AppDataBackupRepository
    {
        public static List<AppDataBackup> GetAll()
        {
            var list = new List<AppDataBackup>();
            using var connection = AOTSqliteDb.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT guId, DeviceID, NetworkName, PackageName, Email, PassEmail, Username, Password, PrivateKey, Phone, HoTen, BirtDay, Gender, PrimaryEducation, SecondaryEducation, Marital, Token, Cookies, FriendCount, GroupCount, FollowCount, Avatar, DataGroup, DataFriend, DataFollow, AppVersion, ABI, NetworkGuId FROM AppDataBackups";
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new AppDataBackup
                {
                    guId = reader.GetString(0),
                    DeviceID = reader.GetString(1),
                    NetworkName = reader.GetString(2),
                    PackageName = reader.GetString(3),
                    Email = reader.GetString(4),
                    PassEmail = reader.GetString(5),
                    Username = reader.GetString(6),
                    Password = reader.GetString(7),
                    PrivateKey = reader.GetString(8),
                    Phone = reader.GetString(9),
                    HoTen = reader.GetString(10),
                    BirtDay = reader.GetString(11),
                    Gender = (Gender)reader.GetInt32(12),
                    PrimaryEducation = reader.GetString(13),
                    SecondaryEducation = reader.GetString(14),
                    Marital = (Marital)reader.GetInt32(15),
                    Token = reader.GetString(16),
                    Cookies = reader.GetString(17),
                    FriendCount = reader.GetInt32(18),
                    GroupCount = reader.GetInt32(19),
                    FollowCount = reader.GetInt32(20),
                    Avatar = reader.GetString(21),
                    DataGroup = reader.GetString(22),
                    DataFriend = reader.GetString(23),
                    DataFollow = reader.GetString(24),
                    AppVersion = reader.GetString(25),
                    ABI = reader.GetString(26),
                    NetworkGuId = reader.GetString(27)
                });
            }
            return list;
        }

        public static bool Add(AppDataBackup item)
        {
            using var connection = AOTSqliteDb.Open();
            using var command = connection.CreateCommand();
            command.CommandText = @"INSERT OR REPLACE INTO AppDataBackups (guId, DeviceID, NetworkName, PackageName, Email, PassEmail, Username, Password, PrivateKey, Phone, HoTen, BirtDay, Gender, PrimaryEducation, SecondaryEducation, Marital, Token, Cookies, FriendCount, GroupCount, FollowCount, Avatar, DataGroup, DataFriend, DataFollow, AppVersion, ABI, NetworkGuId)
VALUES (@guId, @DeviceID, @NetworkName, @PackageName, @Email, @PassEmail, @Username, @Password, @PrivateKey, @Phone, @HoTen, @BirtDay, @Gender, @PrimaryEducation, @SecondaryEducation, @Marital, @Token, @Cookies, @FriendCount, @GroupCount, @FollowCount, @Avatar, @DataGroup, @DataFriend, @DataFollow, @AppVersion, @ABI, @NetworkGuId)";
            command.Parameters.AddWithValue("@guId", item.guId);
            command.Parameters.AddWithValue("@DeviceID", item.DeviceID);
            command.Parameters.AddWithValue("@NetworkName", item.NetworkName);
            command.Parameters.AddWithValue("@PackageName", item.PackageName);
            command.Parameters.AddWithValue("@Email", item.Email);
            command.Parameters.AddWithValue("@PassEmail", item.PassEmail);
            command.Parameters.AddWithValue("@Username", item.Username);
            command.Parameters.AddWithValue("@Password", item.Password);
            command.Parameters.AddWithValue("@PrivateKey", item.PrivateKey);
            command.Parameters.AddWithValue("@Phone", item.Phone);
            command.Parameters.AddWithValue("@HoTen", item.HoTen);
            command.Parameters.AddWithValue("@BirtDay", item.BirtDay);
            command.Parameters.AddWithValue("@Gender", (int)item.Gender);
            command.Parameters.AddWithValue("@PrimaryEducation", item.PrimaryEducation);
            command.Parameters.AddWithValue("@SecondaryEducation", item.SecondaryEducation);
            command.Parameters.AddWithValue("@Marital", (int)item.Marital);
            command.Parameters.AddWithValue("@Token", item.Token);
            command.Parameters.AddWithValue("@Cookies", item.Cookies);
            command.Parameters.AddWithValue("@FriendCount", item.FriendCount);
            command.Parameters.AddWithValue("@GroupCount", item.GroupCount);
            command.Parameters.AddWithValue("@FollowCount", item.FollowCount);
            command.Parameters.AddWithValue("@Avatar", item.Avatar);
            command.Parameters.AddWithValue("@DataGroup", item.DataGroup);
            command.Parameters.AddWithValue("@DataFriend", item.DataFriend);
            command.Parameters.AddWithValue("@DataFollow", item.DataFollow);
            command.Parameters.AddWithValue("@AppVersion", item.AppVersion);
            command.Parameters.AddWithValue("@ABI", item.ABI);
            command.Parameters.AddWithValue("@NetworkGuId", item.NetworkGuId);
            return command.ExecuteNonQuery() > 0;
        }
    }
}
