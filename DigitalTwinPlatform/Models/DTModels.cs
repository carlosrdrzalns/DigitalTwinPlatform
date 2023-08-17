using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigitalTwinPlatform.DBModels;

namespace DigitalTwinPlatform.Models
{
    public class DTModels
    {
        public const string SessionKeyToken = "_Token";
        public const string SessionKeyUser = "_User";
        public List<DBModels.Models> models {get;set;}
        public Guid token { get; set; }
        public string User { get; set; }
    }
}
