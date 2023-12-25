using BusinessCore.Interface;
using CoreObject.DataTransferObject.Request;
using CoreObject.DataTransferObject.Response;
using CoreObject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PisiMobileApi.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ServiceController : ControllerBase
    {
        private IServices _services;

        public ServiceController(IServices services)
        {
            _services = services;
        }

        [HttpPost("create")]
        public async Task<ActionResult<GlobalResponse>> Create([FromBody] CreateServiceInput request)
        {
            var result = await _services.Create(request);
            return result.RequestStatus ? Ok(result) : BadRequest(result);
        }


        [HttpGet("get-by-id")]
        public async Task<ActionResult<GlobalResponse<UserService>>> GetById([FromQuery] Guid serviceId)
        {
            var result = await _services.GetById(serviceId);
            return result.RequestStatus ? Ok(result) : BadRequest(result);
        }

        [HttpGet("get-by-name")]
        public async Task<ActionResult<GlobalResponse<UserService>>> GetByName([FromQuery] string serviceName)
        {
            var result = await _services.GetByName(serviceName);
            return result.RequestStatus ? Ok(result) : BadRequest(result);
        }

        [HttpGet("get-all")]
        public async Task<ActionResult<GlobalResponse<UserService>>> GetAll()
        {
            var result = await _services.GetAll();
            return result.RequestStatus ? Ok(result) : BadRequest(result);
        }

        [HttpGet("get-all-inactive")]
        public async Task<ActionResult<GlobalResponse<UserService>>> GetAllInactive()
        {
            var result = await _services.GetAllInactive();
            return result.RequestStatus ? Ok(result) : BadRequest(result);
        }


        [HttpGet("get-all-active")]
        public async Task<ActionResult<GlobalResponse<UserService>>> GetAllActive()
        {
            var result = await _services.GetAllActive();
            return result.RequestStatus ? Ok(result) : BadRequest(result);
        }

        [HttpPut("deactivate-by-id")]
        public async Task<ActionResult<GlobalResponse<UserService>>> DeactivateById([FromQuery] Guid serviceId)
        {
            var result = await _services.DeactivateById(serviceId);
            return result.RequestStatus ? Ok(result) : BadRequest(result);
        }

        [HttpPut("deactivate-by-name")]
        public async Task<ActionResult<GlobalResponse<UserService>>> DeactivateByName([FromQuery] string serviceName)
        {
            var result = await _services.DeactivateByName(serviceName);
            return result.RequestStatus ? Ok(result) : BadRequest(result);
        }

        [HttpPut("activate-by-id")]
        public async Task<ActionResult<GlobalResponse<UserService>>> ActivateById([FromQuery] Guid serviceId)
        {
            var result = await _services.ActivateById(serviceId);
            return result.RequestStatus ? Ok(result) : BadRequest(result);
        }

        [HttpPut("activate-by-name")]
        public async Task<ActionResult<GlobalResponse<UserService>>> ActivateByName([FromQuery] string serviceName)
        {
            var result = await _services.ActivateByName(serviceName);
            return result.RequestStatus ? Ok(result) : BadRequest(result);
        }

    }
}
