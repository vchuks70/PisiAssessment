using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreObject.DataTransferObject.Request
{
    public class UserRegisterationInput
    {
        [Required]
        public string PhoneNumber { get; set; }
        [Required]  
        public string Password { get; set; }
    }
}
