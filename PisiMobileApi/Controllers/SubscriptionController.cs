using BusinessCore.Interface;
using BusinessCore.Service;
using CoreObject.DataTransferObject.Request;
using CoreObject.DataTransferObject.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PisiMobileApi.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class SubscriptionController : ControllerBase
    {
        private ISubscriptions _subscriptions;

        public SubscriptionController(ISubscriptions subscriptions)
        {
            _subscriptions = subscriptions;
        }

        [HttpPost("subscribe")]
        public async Task<ActionResult<UserLoginResponse>> Subscribe([FromBody] SubscribeInput request)
        {
            var result = await _subscriptions.Subscribe(request);
            return result.RequestStatus ? Ok(result) : BadRequest(result);
        }

        [HttpPost("unsubscribe")]
        public async Task<ActionResult<GlobalResponse>> UnSubscribe([FromBody] UnSubscribeInput request)
        {
            var result = await _subscriptions.UnSubscribe(request);
            return result.RequestStatus ? Ok(result) : BadRequest(result);
        }

        [HttpPost("checkStatus")]
        public async Task<ActionResult<SubscriptionCheckStatusResponse>> UnSubscribe([FromBody] SubscriptionCheckStatusInput request)
        {
            var result = await _subscriptions.CheckStatus(request);
            return result.RequestStatus ? Ok(result) : BadRequest(result);
        }
    }
}
