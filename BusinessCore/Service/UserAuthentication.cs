using BusinessCore.Integration;
using BusinessCore.Interface;
using CoreObject;
using CoreObject.DataTransferObject.Request;
using CoreObject.DataTransferObject.Response;
using CoreObject.Helpers;
using CoreObject.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCore.Service
{
    public class UserAuthentication : IUserAuthentication
    {
        private readonly AppSettings _appSettings;
        private AppDbContext _appDbContext;
        private IPasswordManagement _passwordManagement;

        public UserAuthentication(IOptions<AppSettings> appSettings, AppDbContext appDbContext, IPasswordManagement passwordManagement)
        {
            _appSettings = appSettings.Value;
            _appDbContext = appDbContext;   
            _passwordManagement = passwordManagement;
        }

        public async Task<IEnumerable<UserSubscriptionResponse>> GetAllUserSubscriptions()
        {
            //var ListOfUsers = await _appDbContext.Users.ToListAsync();

            return (from user in _appDbContext.Users
                      join sub in _appDbContext.Subscriptions on user.Id equals sub.UserId
                      join service in _appDbContext.Services on sub.ServiceId equals service.Id
                      select new
                      {
                          PhoneNumber = user.PhoneNumber,
                          Service = service.Name,
                          SubscriptionStatus = sub.Status,
                          SubscriptionDate = sub.UpdatedDate
                      }).GroupBy(x => x.PhoneNumber).Select(x => new UserSubscriptionResponse
                      {
                          PhoneNumber = x.Key,
                          Subscriptions = x.Select(x => new UserSubscriptions
                          {
                              Service = x.Service,
                              SubscriptionStatus = x.SubscriptionStatus == CoreObject.Enum.StatusEnum.Active ? ErrorMessage.UserSubscribed
                                                      : x.SubscriptionStatus == CoreObject.Enum.StatusEnum.Inactive ? ErrorMessage.UserUnSubscribed : ErrorMessage.UserErrorSubscription,
                              SubscriptionDate = x.SubscriptionDate
                          }).ToList(),
                      }).ToList();

            //var ss  = rr.GroupBy(x => x.PhoneNumber).Select( x => new UserSubscriptionResponse
            //{
            //    PhoneNumber = x.Key,
            //    Subscriptions = x.Select( x =>  new UserSubscriptions
            //    {
            //        Service = x.Service,
            //        SubscriptionStatus = x.SubscriptionStatus == CoreObject.Enum.StatusEnum.Active ? ErrorMessage.UserSubscribed
            //                                : x.SubscriptionStatus == CoreObject.Enum.StatusEnum.Inactive ? ErrorMessage.UserUnSubscribed : ErrorMessage.UserErrorSubscription,
            //        SubscriptionDate = x.SubscriptionDate
            //    } ).ToList(),
            //}).ToList();

            //return ListOfUsers;
        }

        public async Task<UserLoginResponse> Login(UserLoginInput request)
        {
            var response = new UserLoginResponse()
            {
                RequestStatus = false,
                Message = "Failed"
            };


            var passwordBtye = await _passwordManagement.EncryptAsync(request.Password, _appSettings.EncryptionPassPhrase);
            var encryptedPassword = Encoding.Default.GetString(passwordBtye);

            #region Get User

            var user = await _appDbContext.Users.FirstOrDefaultAsync(user => user.Password == encryptedPassword);

            if (user == null)
            {

               response.Message = "Invalid user details";
               return response;
            }
            #endregion


            #region Get Service

            var service = await _appDbContext.Services.FirstOrDefaultAsync(s => s.Id == request.Service_id);

            if (service == null)
            {

                response.Message = "Invalid user details";
                return response;
            }
            #endregion



            #region userToken

            var exisingToken  = await _appDbContext.UserTokens.FirstOrDefaultAsync(token => token.UserId == user.Id);

            #endregion

            DateTime date = DateTime.Now;
            var token = AuthTokenManagement.WriteJwt(user, _appSettings);

            if (exisingToken == null)
            {
                var userToken = new UserToken
                {
                    Token = token,
                    CreatedDate = date,
                    UpdatedDate = date,
                    UserId = user.Id,
                    ValidHours = _appSettings.TokenDefaultValidHours
                };

                await _appDbContext.UserTokens.AddAsync(userToken);
            }
            else
            {
                exisingToken.UpdatedDate = date;
                exisingToken.Token = token;
            }

            int result = await _appDbContext.SaveChangesAsync();
            if (result > 0)
            {
                response.Token = token;
                response.Message = "Successful";
                response.RequestStatus = true;
            }
            else
            {
                response.Message = "Internal Error, try again";
            }

            return response;
        }

        public async Task<GlobalResponse> Register(UserRegisterationInput request)
        {
            var response = new GlobalResponse()
            {
                RequestStatus = false,
                Message = "Failed"
            };


            var passwordBtye = await _passwordManagement.EncryptAsync(request.Password, _appSettings.EncryptionPassPhrase);
            var encryptedPassword = Encoding.Default.GetString(passwordBtye);

            #region Get User

            var user = await _appDbContext.Users.FirstOrDefaultAsync(user => user.Password == encryptedPassword ||

            #endregion

                                                                         user.PhoneNumber == request.PhoneNumber);
            if (user != null)
            {
                if (user.PhoneNumber == request.PhoneNumber)
                {
                    response.Message = "Phone Number already exist";
                }
                else
                {
                    response.Message = "Password already exist";
                }

                return response;
            }

            byte[] salt = Encoding.ASCII.GetBytes(_appSettings.Salt);
            DateTime date = DateTime.Now;
            var newUser = new User
            {
                PhoneNumber = request.PhoneNumber,
                Password = encryptedPassword,
                HashPassword = _passwordManagement.HashPasword(request.Password, salt),
                CreatedDate = date,
                UpdatedDate = date
            };

            await _appDbContext.Users.AddAsync(newUser);
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

            return response;

        }
    }
}
