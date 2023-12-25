using BusinessCore.Interface;
using CoreObject.DataTransferObject.Request;
using CoreObject.DataTransferObject.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PisiMobileApi.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class UserAuthenticationController : ControllerBase
    {
        private IUserAuthentication _userAuthentication;

        public UserAuthenticationController(IUserAuthentication userAuthentication)
        {
            _userAuthentication = userAuthentication;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserLoginResponse>> Login([FromBody]UserLoginInput request)
        {
            var result = await _userAuthentication.Login(request);
            return result.RequestStatus ? Ok(result) : BadRequest(result);
        }

        [HttpPost("register")]
        public async Task<ActionResult<GlobalResponse>> Register([FromBody] UserRegisterationInput request)
        {
            var result = await _userAuthentication.Register(request);
            return result.RequestStatus ? Ok(result) : BadRequest(result);
        }
    }
}
