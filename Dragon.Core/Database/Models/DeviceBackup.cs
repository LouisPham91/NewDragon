using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Dragon.Controller.Database.Models
{
    public class DeviceBackup
    {
        [Key]
        public string DeviceId { get; set; } = "";
        public string? Service { get; set; }

        // --- Cột thật lưu trong SQLite ---
        // Đặt tên rõ ràng để EF tạo cột
        public string? DeviceInterfaceGuidsCsv { get; set; }

        // --- Property bạn dùng trong code, không map DB ---
        [NotMapped]
        public string[]? DeviceInterfaceGuids
        {
            get => string.IsNullOrWhiteSpace(DeviceInterfaceGuidsCsv)
                   ? null
                    : DeviceInterfaceGuidsCsv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            set => DeviceInterfaceGuidsCsv = (value == null || value.Length == 0)
                   ? null
                    : string.Join(',', value);
        }

        // tiện ích nếu bạn cần Guid[]
        [NotMapped]
        public Guid[]? DeviceInterfaceGuidsAsGuid
        {
            get => DeviceInterfaceGuids?.Select(g => Guid.Parse(g)).ToArray();
            set => DeviceInterfaceGuids = value?.Select(g => g.ToString()).ToArray();
        }
    }
}
