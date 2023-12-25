using CoreObject.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreObject.Models
{
    public class UserService : BaseClass    
    {
        public string Name { get; set; }    
        public decimal Amount { get; set; }
        public StatusEnum Status { get; set; }
    }
}
