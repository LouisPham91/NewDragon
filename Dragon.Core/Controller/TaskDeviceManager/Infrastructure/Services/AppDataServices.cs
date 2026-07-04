using Dragon.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dragon.Controller.TaskDeviceManager.Infrastructure.Services
{
    public static class AppDataServices
    {
        public static List<string> GetColumnAppData()
        {
            var fields = new List<string>
            {
                nameof(AppData.Email),
                nameof(AppData.PassEmail),
                nameof(AppData.Username),
                nameof(AppData.Password),
                nameof(AppData.PrivateKey),
                nameof(AppData.Phone),
                nameof(AppData.HoTen),
                nameof(AppData.BirtDay),
                nameof(AppData.Gender),
                nameof(AppData.PrimaryEducation),
                nameof(AppData.SecondaryEducation),
                nameof(AppData.Token),
                nameof(AppData.Cookies),
                nameof(AppData.FriendCount),
                nameof(AppData.GroupCount),
                nameof(AppData.FollowCount),
                nameof(AppData.Avatar),
                nameof(AppData.DataGroup),
                nameof(AppData.DataFriend),
                nameof(AppData.DataFollow),

            };
            return fields;

        }
        public static void SaveToDatabase(AppData appData, string columnName, string value)
        {
            switch (columnName)
            {
                case nameof(AppData.DeviceID):
                    appData.DeviceID = value;
                    break;
                case nameof(AppData.PackageName):
                    appData.PackageName = value;
                    break;
                case nameof(AppData.Email):
                    appData.Email = value;
                    break;
                case nameof(AppData.PassEmail):
                    appData.PassEmail = value;
                    break;
                case nameof(AppData.Username):
                    appData.Username = value;
                    break;
                case nameof(AppData.Password):
                    appData.Password = value;
                    break;
                case nameof(AppData.PrivateKey):
                    appData.PrivateKey = value;
                    break;
                case nameof(AppData.Phone):
                    appData.Phone = value;
                    break;
                case nameof(AppData.HoTen):
                    appData.HoTen = value;
                    break;
                case nameof(AppData.BirtDay):
                        appData.BirtDay = value;
                    break;
                case nameof(AppData.PrimaryEducation):
                    appData.PrimaryEducation = value;
                    break;
                case nameof(AppData.SecondaryEducation):
                    appData.SecondaryEducation = value;
                    break;
                case nameof(AppData.Token):
                    appData.Token = value;
                    break;
                case nameof(AppData.Cookies):
                    appData.Cookies = value;
                    break;
                case nameof(AppData.FriendCount):
                    if (int.TryParse(value, out var friendCount))
                        appData.FriendCount = friendCount;
                    break;
                case nameof(AppData.GroupCount):
                    if (int.TryParse(value, out var groupCount))
                        appData.GroupCount = groupCount;
                    break;
                case nameof(AppData.FollowCount):
                    if (int.TryParse(value, out var followCount))
                        appData.FollowCount = followCount;
                    break;
                case nameof(AppData.Avatar):
                    appData.Avatar = value;
                    break;
                case nameof(AppData.DataGroup):
                    appData.DataGroup = value;
                    break;
                case nameof(AppData.DataFriend):
                    appData.DataFriend = value;
                    break;
                case nameof(AppData.DataFollow):
                    appData.DataFollow = value;
                    break;
                case nameof(AppData.AppVersion):
                    appData.AppVersion = value;
                    break;
                case nameof(AppData.ABI):
                    appData.ABI = value;
                    break;
            }
        }
    }
}
