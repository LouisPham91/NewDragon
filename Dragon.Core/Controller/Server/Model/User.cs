using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dragon.Controller.Server.Model
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [StringLength(10)]
        public string Uid { get; set; } = string.Empty; // Mã UID cho payment

        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(200)]
        public string Email { get; set; } = string.Empty;

        [StringLength(50)]
        public string Phone { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLogin { get; set; }

        // Refresh token
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime? RefreshTokenExpiryTime { get; set; }

        // Thông tin bổ sung
        [StringLength(20)]
        public string Gender { get; set; } = string.Empty;

        public DateTime? Birthday { get; set; }

        [StringLength(100)]
        public string CountryCode { get; set; } = string.Empty; // VN, US, JP...

        [StringLength(200)]
        public string City { get; set; } = string.Empty;

        [StringLength(300)]
        public string Address { get; set; } = string.Empty;

    }

}
