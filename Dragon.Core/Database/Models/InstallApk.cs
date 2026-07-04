


using System.ComponentModel.DataAnnotations.Schema;

namespace Dragon.Database.Models
{

    public class InstallApk
    {
        public int Id { get; set; } // Khóa chính

        public string AppName { get; set; } = string.Empty;
        public string PackageName { get; set; } = string.Empty;
        public string VersionName { get; set; } = string.Empty;
        public string VersionCode { get; set; } = string.Empty;
        public int MinAPI { get; set; }
        public int MaxAPI { get; set; }
        public string ABIs { get; set; } = string.Empty; // lưu dạng "abi1,abi2"
        public string Path { get; set; } = string.Empty;

        [NotMapped]
        public List<string> ABI => string.IsNullOrEmpty(ABIs)
                ? new List<string>()
                : ABIs.Split(',', StringSplitOptions.RemoveEmptyEntries)
                      .Select(x => x.Trim())
                      .ToList();
    }

}
