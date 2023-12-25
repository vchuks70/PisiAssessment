using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCore.Integration
{
    public class LogService
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static void LogInfo(string countryId, string callerFormName, string methodName, string message)
        {
            log.InfoFormat("\r\nCountry Id: {0} Executing Operation: {1} Method Name: {2}\r\nMessage: {3}\r\n", countryId, callerFormName, methodName, message);
        }

        public static void LogError(string countryId, string callerFormName, string methodName, Exception ex)
        {
            log.ErrorFormat("\r\nCountry Id: {0} Executing Operation: {1} Method Name: {2}\r\nMessage: {3}\r\n", countryId, callerFormName, methodName, ex);
        }
    }
}
