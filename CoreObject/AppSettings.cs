using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreObject
{
    public class AppSettings
    {   
        public string Secret { get; set; }
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Salt { get; set; }
        public string EncryptionPassPhrase { get; set; }
        public int TokenDefaultValidHours { get; set; }
    }
}
