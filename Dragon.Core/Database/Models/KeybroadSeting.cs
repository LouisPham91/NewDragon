
using System.ComponentModel.DataAnnotations;

namespace Dragon.Database.Models
{
    public class KeybroadSetting 
    {
        [Key]
        public int Id { get; set; }
        public string DeviceId { get; set; } = string.Empty;
        public Langeuage Langeuage { get; set; } = Langeuage.None;
        public string IMEId { get; set; } = string.Empty;

    }
    public enum Langeuage
    {
        None,
        Vietnamese,
        English,
        ATX_Unicode,
    }
}
