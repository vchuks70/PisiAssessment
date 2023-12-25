using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreObject.DataTransferObject.Request
{
    public class UserLoginInput
    {
        [Required]
        public Guid Service_id { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
