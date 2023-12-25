using CoreObject.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http.Extensions;
using System.Text;
using CoreObject;
using Microsoft.Extensions.Options;
using CoreObject.DataTransferObject.Response;
using BusinessCore.Service;
using CoreObject.DataTransferObject.Request;
using CoreObject.Helpers;
using Microsoft.EntityFrameworkCore;

namespace BusinessCore.Integration
{
    public class LogHandler
    {
        private readonly RequestDelegate _next;
        public LogHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IOptions<AppSettings> generalSettings, AppDbContext appDbContext)
        {
            var appSettings = generalSettings.Value;

            var tempRequestId = string.Empty;

            bool validateToken = false;

            try
            {

                var routeData = context.Request.Scheme + "://" + context.Request.Host + context.Request.Path;

                if (routeData.Contains("/Subscriptions"))
                {
                    validateToken = true;
                }

                //try and get the operation name and version
                string operationName = string.Empty;
                string operationVersion = string.Empty;

                var operationDetails = routeData.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                operationName = operationDetails[operationDetails.Length - 1];//Get the last item in Url
                operationVersion = operationDetails[operationDetails.Length - 3];



                var incomingRequest = CreateRequestResponseMessage(context);
                var item = JsonConvert.SerializeObject(incomingRequest);
                var entry = JsonConvert.DeserializeObject<RequestResponseEntry>(item);


                if (entry == null)
                {
                    context.Response.StatusCode = 400;
                    context.Response.ContentType = "application/xml";
                    await context.Response.WriteAsJsonAsync(
                    new GlobalResponse
                    {
                        Message = "Invalid JSON message"
                    });
                    return;
                }

                if (context != null)
                {
                    context.Request.EnableBuffering();
                    var stream2 = context.Request.Body;
                    long? length = context.Request.ContentLength;
                    if (length != null && length > 0)
                    {
                        //  Use this method to read , And use asynchronous 
                        StreamReader streamReader = new StreamReader(stream2, Encoding.UTF8);
                        entry.RequestContentBody = streamReader.ReadToEndAsync().Result;
                    }
                    context.Request.Body.Position = 0;
                }
                if (validateToken)
                {
                    var token = string.Empty;
                    var phoneNumber = string.Empty;
                    var contentBody = JsonConvert.DeserializeObject<GlobalInput>(entry.RequestContentBody);
                    if (contentBody != null)
                    {
                        token = contentBody.Token_id;
                        phoneNumber = contentBody.Phone_number;
                    }

                    if (token == null)
                    {
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/xml";
                        await context.Response.WriteAsJsonAsync(
                        new GlobalResponse
                        {
                            Message = ErrorMessage.WrongToken
                        });
                        return;
                    }

                    #region Get UserToken and user
                    var user = await appDbContext.Users.FirstOrDefaultAsync(user => user.PhoneNumber == phoneNumber);

                    if (user == null)
                    {
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/xml";
                        await context.Response.WriteAsJsonAsync(
                        new GlobalResponse
                        {
                            Message = ErrorMessage.WrongPhonenumber
                        });
                        return;
                    }

                    var exisingToken = await appDbContext.UserTokens.FirstOrDefaultAsync(t => t.Token == token && t.UserId == user.Id);

                    if (exisingToken == null)
                    {
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/xml";
                        await context.Response.WriteAsJsonAsync(
                        new GlobalResponse
                        {
                            Message = ErrorMessage.WrongToken
                        });
                        return;
                    }

                    if (!exisingToken.IsValid)
                    {
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/xml";
                        await context.Response.WriteAsJsonAsync(
                        new GlobalResponse
                        {
                            Message = ErrorMessage.ExpiredToken
                        });
                        return;
                    }
                    #endregion

                }

                entry.OperationName = operationName;
                entry.OperationVersion = operationVersion;

                entry.CreatedDate = DateTime.Now;
                entry.UpdatedDate = DateTime.Now;
                Stream originalBody = context.Response.Body;

                string responseBody = string.Empty;
                try
                {
                    using (var memStream = new MemoryStream())
                    {
                        context.Response.Body = memStream;

                        await _next(context);

                        memStream.Position = 0;
                        responseBody = new StreamReader(memStream).ReadToEnd();

                        memStream.Position = 0;
                        await memStream.CopyToAsync(originalBody);
                    }

                    entry.ResponseContentBody = responseBody;
                    entry.ResponseContentType = context.Response.ContentType;
                    entry.ResponseHeaders = SerializeHeaders(context.Response.Headers);
                    entry.ResponseTimestamp = DateTime.Now;
                    entry.ResponseStatusCode = context.Response.StatusCode;

                }
                finally
                {
                    context.Response.Body = originalBody;

                    await appDbContext.RequestResponseEntries.AddAsync(entry);
                    await appDbContext.SaveChangesAsync();

                }
            }
            catch (Exception)
            {

                context.Response.StatusCode = 400;
                context.Response.ContentType = "application/xml";
                await context.Response.WriteAsJsonAsync(
                new GlobalResponse
                {
                    Message = "Internal processing error"
                });
                return;
            }

        }


        private IncomingRequest CreateRequestResponseMessage(HttpContext context)
        {
            try
            {

                string queryString = string.Empty;
                if (context.Request.QueryString.ToString() != null)
                {
                    queryString = context.Request.QueryString.ToString();
                }
                return new IncomingRequest
                {
                    AppId = GetHeaderValue(context.Request, "AppId"),
                    AppKey = GetHeaderValue(context.Request, "AppKey"),
                    RequestContentType = context?.Request?.ContentType?.ToString() ?? string.Empty,
                    RequestIpAddress = GetIp(context),
                    RequestMethod = context.Request.Method,
                    RequestHeaders = SerializeHeaders(context.Request.Headers),
                    RequestTimestamp = DateTime.Now,
                    RequestUri = context.Request.GetDisplayUrl(), //;.Url.ToString()
                    QueryString = queryString
                };
            }
            catch (Exception ex)
            {
                LogService.LogError("00", "LogHandler", "CreateRequestResponseMessage", ex);
                return null;
            }
        }

        private string ReadXMLFromStream(HttpContext context)
        {
            string request = "";
            context.Request.EnableBuffering();
            var stream = context.Request.Body;
            long? length = context.Request.ContentLength;
            if (length != null && length > 0)
            {
                //  Use this method to read , And use asynchronous 
                StreamReader streamReader = new StreamReader(stream, Encoding.UTF8);
                request = streamReader.ReadToEndAsync().Result;
            }
            context.Request.Body.Position = 0;

            return request;

        }
        private string GetHeaderValue(HttpRequest request, string headerKey)
        {
            try
            {
                request.Headers.TryGetValue(headerKey, out var result);
                if (string.IsNullOrEmpty(result))
                {
                    result = request.Query[headerKey];
                }
                return result;
            }
            catch (Exception)
            {
                return null;
            }

        }

        //private string GetIp(HttpContext context)
        //{
        //    // string ip = context.Connection.RemoteIpAddress.MapToIPv4().ToString();
        //    string ip = context.Connection.ip.MapToIPv4().ToString();
        //    return ip;
        //}

        public string GetIp(HttpContext context)
        {
            string ip = string.Empty;
            if (!string.IsNullOrEmpty(context.Request.Headers["X-Forwarded-For"]))
            {
                ip = context.Request.Headers["X-Forwarded-For"];
            }
            else
            {
                ip = context.Request.HttpContext.Features.Get<IHttpConnectionFeature>().RemoteIpAddress.ToString();
            }
            return ip;
        }

        private string SerializeHeaders(IHeaderDictionary headers)
        {
            var dict = new Dictionary<string, string>();
            foreach (var item in headers.ToList())
            {
                if (item.Value.Any())
                {
                    var header = item.Value.Aggregate(String.Empty, (current, value) => current + (value + " "));
                    header = header.TrimEnd(" ".ToCharArray());
                    dict.Add(item.Key, header);
                }
            }
            return JsonConvert.SerializeObject(dict, Newtonsoft.Json.Formatting.Indented);
        }
    }
    public static class GlobalCutomMiddleware
    {
        public static IApplicationBuilder UseGlobalCustomMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<LogHandler>();
        }
    }

}



