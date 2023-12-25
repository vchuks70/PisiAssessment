using CoreObject.DataTransferObject.Request;
using CoreObject.DataTransferObject.Response;
using CoreObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCore.Interface
{
    public interface IUserAuthentication
    {
        Task<GlobalResponse> Register(UserRegisterationInput request);
        Task<UserLoginResponse> Login(UserLoginInput request);
        Task<IEnumerable<UserSubscriptionResponse>> GetAllUserSubscriptions();

    }
}
