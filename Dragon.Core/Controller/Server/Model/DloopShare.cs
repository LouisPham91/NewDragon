using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dragon.Controller.Server.Model
{
    public enum SharePermission
    {
        View = 0,
        Edit = 1
    }

    public class DloopShare
    {
        public Guid DloopId { get; set; }
        public string SharedGmail { get; set; } = string.Empty;
        public SharePermission Permission { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }
}
