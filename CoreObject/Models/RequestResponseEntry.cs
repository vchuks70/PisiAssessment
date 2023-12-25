using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreObject.Models
{
    public class RequestResponseEntry : BaseClass
    {
        public string? RequestHeaders { get; set; }
        public string? RequestIpAddress { get; set; }
        public string? RequestContentType { get; set; }
        public string? RequestUri { get; set; }
        public string? RequestMethod { get; set; }
        public string? OperationName { get; set; }
        public string? OperationVersion { get; set; }
        public DateTime? RequestTimestamp { get; set; }
        public string? ResponseContentType { get; set; }
        public int? ResponseStatusCode { get; set; }
        public string? ResponseHeaders { get; set; }
        public DateTime? ResponseTimestamp { get; set; }
        public string? QueryString { get; set; }
        public string? RequestContentBody { get; set; }
        public string? ResponseContentBody { get; set; }
    }
}
