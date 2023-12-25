using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreObject.Helpers
{
    public static class ErrorMessage
    {
        public const string UserSubscribed = "User is subscribed";
        public const string UserUnSubscribed = "User is not subscribed";
        public const string UserErrorSubscription = "User subscription status not found";
        public const string UserAlreadySubscribed = "User is already subscribed";
        public const string ExpiredToken = "Token ID expired";
        public const string WrongToken = "Wrong Token ID";
        public const string WrongPhonenumber = "Wrong Phone number ID";
    }
}
