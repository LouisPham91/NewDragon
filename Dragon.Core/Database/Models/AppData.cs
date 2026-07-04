
using System;
using System.ComponentModel.DataAnnotations;

namespace Dragon.Database.Models
{

    public class AppData
    {
        [Key]
        public string guId { get; set; } = Guid.NewGuid().ToString(); 
        public string DeviceID { get; set; } = string.Empty;
        public string NetworkName { get; set; } = string.Empty;
        public string PackageName { get; set; } = string.Empty;
        public string DeviceModel { get; set; } = string.Empty;
        public AccStatus AccStatus { get; set; }
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
            var parts = new List<string>();

            if (!string.IsNullOrEmpty(NetworkName))
                parts.Add(NetworkName);

            if (!string.IsNullOrEmpty(HoTen))
                parts.Add(HoTen);          

            if (!string.IsNullOrEmpty(DeviceID))
                parts.Add(DeviceID);

            return string.Join("-", parts);
        }


    }

    public enum AccStatus
    {
        None = 0,        // Không có trạng thái, mặc định
        Live,            // Tài khoản hoạt động bình thường
        Died,            // Tài khoản đã chết, không đăng nhập được
        Pending,         // Tài khoản đang trong trạng thái chờ xác minh hoặc chờ kết quả kiểm tra
        CheckPoint,      // Bị chặn đăng nhập, yêu cầu xác minh danh tính
        Locked,          // Bị khóa tạm thời do vi phạm hoặc bảo mật
        Disabled,        // Bị vô hiệu hóa hoàn toàn, không thể khôi phục
        Suspended,       // Bị treo trong một khoảng thời gian nhất định
        Restricted,      // Bị hạn chế chức năng (ví dụ không đăng bài, không nhắn tin)
        Verified,        // Đã xác minh, trạng thái “sống” nhưng đặc biệt
        Unknown          // Không xác định, chưa kiểm tra được
    }



    public enum Gender
    {
        Male,
        Female,
        Other
    }

    public enum Marital
    {
        Single,
        Married,
        Divorced,
        Widowed,
        Other
    }
}
