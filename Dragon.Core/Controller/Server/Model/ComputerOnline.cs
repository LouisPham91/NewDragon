using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dragon.Controller.Server.Model
{
    public class ComputerOnline
    {
        public Guid Id { get; set; }
        public string FingerPrint { get; set; } = string.Empty;
        public string MachineName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime Expires_At { get; set; }
        public bool IsOnline { get; set; }
        public DateTime? LastSeen { get; set; }   // cho phép null
    }

}
