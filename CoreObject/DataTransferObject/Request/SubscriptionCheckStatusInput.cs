using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreObject.DataTransferObject.Request
{
    public class SubscriptionCheckStatusInput
    {
        public Guid Service_id { get; set; }
        public string Token_id { get; set; }
        public string Phone_number { get; set; }
    }
}
