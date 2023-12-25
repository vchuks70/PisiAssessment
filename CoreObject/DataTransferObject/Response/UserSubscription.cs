using CoreObject.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreObject.DataTransferObject.Response    
{   
    public class UserSubscriptionResponse
    {   
        public string PhoneNumber { get; set; }
        public List<UserSubscriptions> Subscriptions { get; set; }  

    }
    public class UserSubscriptions
    {
        public string Service { get; set; }
        public string SubscriptionStatus { get; set; }
        public DateTime SubscriptionDate { get; set; }
    }
}
