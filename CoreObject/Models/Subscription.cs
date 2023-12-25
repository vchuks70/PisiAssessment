using CoreObject.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreObject.Models
{
    public class Subscription : BaseClass
    {   
        public Guid UserId { get; set; }
        public Guid ServiceId { get; set; }
        public StatusEnum Status { get; set; }
    }
}
