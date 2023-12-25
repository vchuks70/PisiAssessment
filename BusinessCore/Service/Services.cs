using BusinessCore.Interface;
using CoreObject.DataTransferObject.Request;
using CoreObject.DataTransferObject.Response;
using CoreObject.Enum;
using CoreObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCore.Service
{
    public class Services : IServices
    {
        private AppDbContext _appDbContext;

        public Services(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<GlobalResponse> ActivateById(Guid serviceId)
        {
            var response = new GlobalResponse()
            {
                RequestStatus = false,
                Message = "Failed"
            };

            var service  = await _appDbContext.Services.FirstOrDefaultAsync(s => s.Id == serviceId);

            if (service != null)
            {
                if (service.Status == StatusEnum.Active)
                {
                    response.Message = "Service already active";
                }
                else
                {
                    service.Status = StatusEnum.Active;
                    int result = await _appDbContext.SaveChangesAsync();
                    if (result > 0)
                    {
                        response.Message = "Successful";
                        response.RequestStatus = true;
                    }
                    else
                    {
                        response.Message = "Internal Error, try again";
                    }

                }
                               

            }
            return response;
        }

        public async Task<GlobalResponse> ActivateByName(string serviceName)
        {

            var response = new GlobalResponse()
            {
                RequestStatus = false,
                Message = "Failed"
            };

            var service = await _appDbContext.Services.FirstOrDefaultAsync(s => s.Name == serviceName);

            if (service != null)
            {
                if (service.Status == StatusEnum.Active)
                {
                    response.Message = "Service already active";
                }
                else
                {
                    service.Status = StatusEnum.Active;
                    int result = await _appDbContext.SaveChangesAsync();
                    if (result > 0)
                    {
                        response.Message = "Successful";
                        response.RequestStatus = true;
                    }
                    else
                    {
                        response.Message = "Internal Error, try again";
                    }

                }


            }
            return response;
        }

        public async Task<GlobalResponse> Create(CreateServiceInput request)
        {

            var response = new GlobalResponse()
            {
                RequestStatus = false,
                Message = "Failed"
            };

            var service = await _appDbContext.Services.FirstOrDefaultAsync(s => s.Name == request.Name);

            if (service != null)
            {
                response.Message = "Service already exist";
            }
            else
            {
                var date = DateTime.Now;
                var newService = new UserService
                {
                    Amount = request.Amount,
                    Name = request.Name,
                    CreatedDate = date,
                    UpdatedDate = date,
                    Status = StatusEnum.Active
                };

                await _appDbContext.Services.AddAsync(newService);
                int result = await _appDbContext.SaveChangesAsync();
                if (result > 0)
                {
                    response.Message = "Successful";
                    response.RequestStatus = true;
                }
                else
                {
                    response.Message = "Internal Error, try again";
                }
            }

            return response;
        }

        public async Task<GlobalResponse> DeactivateById(Guid serviceId)
        {
            var response = new GlobalResponse()
            {
                RequestStatus = false,
                Message = "Failed"
            };

            var service = await _appDbContext.Services.FirstOrDefaultAsync(s => s.Id == serviceId);

            if (service != null)
            {
                if (service.Status == StatusEnum.Inactive)
                {
                    response.Message = "Service already Inactive";
                }
                else
                {
                    service.Status = StatusEnum.Inactive;
                    int result = await _appDbContext.SaveChangesAsync();
                    if (result > 0)
                    {
                        response.Message = "Successful";
                        response.RequestStatus = true;
                    }
                    else
                    {
                        response.Message = "Internal Error, try again";
                    }

                }


            }
            return response;
        }

        public async Task<GlobalResponse> DeactivateByName(string serviceName)
        {
            var response = new GlobalResponse()
            {
                RequestStatus = false,
                Message = "Failed"
            };

            var service = await _appDbContext.Services.FirstOrDefaultAsync(s => s.Name == serviceName);

            if (service != null)
            {
                if (service.Status == StatusEnum.Inactive)
                {
                    response.Message = "Service already Inactive";
                }
                else
                {
                    service.Status = StatusEnum.Inactive;
                    int result = await _appDbContext.SaveChangesAsync();
                    if (result > 0)
                    {
                        response.Message = "Successful";
                        response.RequestStatus = true;
                    }
                    else
                    {
                        response.Message = "Internal Error, try again";
                    }

                }


            }
            return response;
        }

        public async Task<GlobalResponse<List<UserService>>> GetAll()
        {
            var services = await _appDbContext.Services.ToListAsync();
            return new GlobalResponse<List<UserService>>
            {
                RequestStatus = true,
                Message = services.Any() ? "Successful" : "No Content",
                Data = services
            };
        }

        public async Task<GlobalResponse<List<UserService>>> GetAllActive()
        {
            var services = await _appDbContext.Services.Where(service => service.Status == StatusEnum.Active).ToListAsync();
            return new GlobalResponse<List<UserService>>
            {
                RequestStatus = true,
                Message = services.Any() ? "Successful" : "No Content",
                Data = services
            };
        }

        public async Task<GlobalResponse<List<UserService>>> GetAllInactive()
        {
            var services = await _appDbContext.Services.Where(service => service.Status == StatusEnum.Inactive).ToListAsync();
            return new GlobalResponse<List<UserService>>
            {
                RequestStatus = true,
                Message = services.Any() ? "Successful" : "No Content",
                Data = services
            };
        }

        public async Task<GlobalResponse<UserService>> GetById(Guid serviceId)
        {
            var service = await _appDbContext.Services.FirstOrDefaultAsync(service => service.Id == serviceId);
            return new GlobalResponse<UserService>
            {
                RequestStatus = true,
                Message = service != null ? "Successful" : "No Content",
                Data = service
            };
        }

        public async Task<GlobalResponse<UserService>> GetByName(string serviceName)
        {
            var service = await _appDbContext.Services.FirstOrDefaultAsync(service => service.Name == serviceName);
            return new GlobalResponse<UserService>
            {
                RequestStatus = true,
                Message = service != null ? "Successful" : "No Content",
                Data = service
            };
        }
    }
}
