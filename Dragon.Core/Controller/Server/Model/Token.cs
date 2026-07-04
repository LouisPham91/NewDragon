using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dragon.Controller.Server.Model
{
    public class Token
    {
        public string refreshToken { get; set; } = string.Empty;
        public string accessToken { get; set; } = string.Empty;
    }
}
