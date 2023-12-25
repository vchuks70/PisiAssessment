using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreObject.Models
{
    public class UserToken : BaseClass
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
        public int ValidHours { get; set; }
        public bool IsValid => UpdatedDate.AddHours(ValidHours) > DateTime.Now;
    }
}
