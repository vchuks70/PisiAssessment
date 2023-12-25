using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreObject.Models
{
    public class User : BaseClass   
    {
        [Required]
        public string PhoneNumber { get; set; }
        [Required]  
        public string HashPassword { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
