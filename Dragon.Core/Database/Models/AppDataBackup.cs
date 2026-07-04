using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dragon.Database.Models
{
  
    public class AppDataBackup
    {
        [Key]
        public string guId { get; set; } = Guid.NewGuid().ToString();
        public string DeviceID { get; set; } = string.Empty;
        public string NetworkName { get; set; } = string.Empty;
        public string PackageName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PassEmail { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PrivateKey { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string HoTen { get; set; } = string.Empty;
        public string BirtDay { get; set; } = string.Empty;
        public Gender Gender { get; set; }
        public string PrimaryEducation { get; set; } = string.Empty;
        public string SecondaryEducation { get; set; } = string.Empty;
        public Marital Marital { get; set; }
        public string Token { get; set; } = string.Empty;
        public string Cookies { get; set; } = string.Empty;
        public int FriendCount { get; set; }
        public int GroupCount { get; set; }
        public int FollowCount { get; set; }
        public string Avatar { get; set; } = string.Empty;
        public string DataGroup { get; set; } = string.Empty;
        public string DataFriend { get; set; } = string.Empty;
        public string DataFollow { get; set; } = string.Empty;
        public string AppVersion { get; set; } = string.Empty;
        public string ABI { get; set; } = string.Empty;

        public string NetworkGuId { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"{NetworkName}-{HoTen}-{DeviceID}";
        }

    }

}
