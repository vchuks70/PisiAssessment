using CoreObject.DataTransferObject.Request;
using CoreObject.DataTransferObject.Response;
using CoreObject.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCore.Interface
{
    public interface IServices
    {   
        Task<GlobalResponse> Create(CreateServiceInput request);
        Task<GlobalResponse<UserService>> GetById(Guid serviceId);
        Task<GlobalResponse<UserService>> GetByName(string serviceName);
        Task<GlobalResponse<List<UserService>>> GetAll();
        Task<GlobalResponse<List<UserService>>> GetAllInactive();
        Task<GlobalResponse<List<UserService>>> GetAllActive();
        Task<GlobalResponse> DeactivateById(Guid serviceId);
        Task<GlobalResponse> DeactivateByName(string serviceName);
        Task<GlobalResponse> ActivateById(Guid serviceId);
        Task<GlobalResponse> ActivateByName(string serviceName);
    }
}
