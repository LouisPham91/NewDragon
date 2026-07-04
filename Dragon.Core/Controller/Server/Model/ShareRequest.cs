using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dragon.Controller.Server.Model
{
    public class ShareRequest
    {
        public Guid DloopId { get; set; }
        public List<string> Gmails { get; set; } = new();
        public SharePermission Permission { get; set; } = SharePermission.View;
        public DateTime? ExpiresAt { get; set; }
    }
}
