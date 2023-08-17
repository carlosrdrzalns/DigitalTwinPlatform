using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace DigitalTwinPlatform.Models
{
    public class LoginViewModel
    {
        public const string SessionKeyToken = "_Token";
        public const string SessionKeyUser = "_User";
        public const string SessionKeyPassword = "_Password";
        public Guid? Token { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }
}
