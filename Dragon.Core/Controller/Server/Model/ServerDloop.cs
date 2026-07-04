using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dragon.Controller.Server.Model
{
    public class ServerDloop
    {
        public Guid Id { get; set; }
        public string OwnerGmail { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public bool IsSingleLoop { get; set; }
        public string PhoneModel { get; set; } = string.Empty;
        public byte[] DloopData { get; set; } = [];
        public DateTime UpdatedAt { get; set; }
    }
}
