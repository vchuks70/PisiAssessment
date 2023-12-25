using BusinessCore.Interface;
using CoreObject.DataTransferObject.Request;
using CoreObject.DataTransferObject.Response;
using CoreObject.Enum;
using CoreObject.Helpers;
using CoreObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCore.Service
{
    public class Subscriptions : ISubscriptions
    {
        private AppDbContext _appDbContext;

        public Subscriptions(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<SubscriptionCheckStatusResponse> CheckStatus(SubscriptionCheckStatusInput request)
        {
            var response = new SubscriptionCheckStatusResponse()
            {
                RequestStatus = false,
                Message = "Failed",
                SubscriptionStatus = ErrorMessage.UserUnSubscribed
            };

            #region Get User

            var user = await _appDbContext.Users.FirstOrDefaultAsync( user => user.PhoneNumber == request.Phone_number);
            if (user == null)  return response;

            #endregion  

            var subscription = await _appDbContext.Subscriptions.FirstOrDefaultAsync(s => s.ServiceId == request.Service_id &&
                                                                                s.UserId == user.Id);

            if (subscription != null)
            {
                if (subscription.Status == StatusEnum.Active)
                {
                    response.SubscriptionStatus = ErrorMessage.UserSubscribed;
                    response.Date = subscription.UpdatedDate;
                    response.Message = "Successful";
                    response.RequestStatus = true;
                }
                else
                {
                    if (subscription.Status == StatusEnum.Inactive)
                    {
                        response.SubscriptionStatus = ErrorMessage.UserUnSubscribed;
                        response.Date = subscription.UpdatedDate;
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

        public async Task<GlobalResponse> Subscribe(SubscribeInput request)
        {
            var response = new GlobalResponse()
            {
                RequestStatus = false,
                Message = "Failed",
            };

            #region Get User

            var user = await _appDbContext.Users.FirstOrDefaultAsync(user => user.PhoneNumber == request.Phone_number);
            if (user == null) return response;

            #endregion

            #region Get Service

            var service = await _appDbContext.Services.FirstOrDefaultAsync(service => service.Id == request.Service_id);
            if (service == null) return response;

            #endregion

            var date = DateTime.Now;

            var subscription = await _appDbContext.Subscriptions.FirstOrDefaultAsync(s => s.ServiceId == service.Id &&
                                                                                s.UserId == user.Id);

            if (subscription != null)
            {
                if (subscription.Status == StatusEnum.Active)
                {
                    response.Message = ErrorMessage.UserAlreadySubscribed;
                }
                else
                {
                    subscription.Status = StatusEnum.Active;
                    subscription.UpdatedDate = date;

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
            else
            {
                var newSubscription = new Subscription
                {
                    Status = StatusEnum.Active,
                    CreatedDate = date,
                    UpdatedDate = date,
                    ServiceId = service.Id,
                    UserId = user.Id,
                };

                await _appDbContext.Subscriptions.AddAsync(newSubscription);
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

        public async Task<GlobalResponse> UnSubscribe(UnSubscribeInput request)
        {
            var response = new GlobalResponse()
            {
                RequestStatus = false,
                Message = "Failed",
            };

            #region Get User

            var user = await _appDbContext.Users.FirstOrDefaultAsync(user => user.PhoneNumber == request.Phone_number);
            if (user == null) return response;

            #endregion

            #region Get Service

            var service = await _appDbContext.Services.FirstOrDefaultAsync(service => service.Id == request.Service_id);
            if (service == null) return response;

            #endregion

            var date = DateTime.Now;

            var subscription = await _appDbContext.Subscriptions.FirstOrDefaultAsync(s => s.ServiceId == service.Id &&
                                                                                s.UserId == user.Id);

            if (subscription != null)
            {
                if (subscription.Status == StatusEnum.Inactive)
                {
                    response.Message = ErrorMessage.UserUnSubscribed;
                }
                else
                {
                    subscription.Status = StatusEnum.Inactive;
                    subscription.UpdatedDate = date;

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
    }
}
