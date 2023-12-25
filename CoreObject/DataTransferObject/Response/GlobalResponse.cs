using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreObject.DataTransferObject.Response
{
    public class GlobalResponse
    {
        public bool RequestStatus { get; set; } 
        public string Message { get; set; }
    }

    public class GlobalResponse <T>
    {   
        public bool RequestStatus { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
    