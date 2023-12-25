using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;   

namespace CoreObject.DataTransferObject.Response
{
    public class SubscriptionCheckStatusResponse: GlobalResponse  
    {
        public string SubscriptionStatus { get; set; }
        public DateTime? Date { get; set; }
    }
}
