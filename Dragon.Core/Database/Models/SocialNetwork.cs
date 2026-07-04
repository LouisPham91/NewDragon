
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dragon.Database.Models
{
  
    public class SocialNetwork
    {
        [Key]
        public string guId { get; set; } = Guid.NewGuid().ToString();
        public string NetworkName { get; set; } = string.Empty;
        public override string ToString()
        {
            return NetworkName.ToString();
        }
    }
}
