using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalTwinPlatform.Models
{
    public class Viewer
    {
        public const string SessionKeyToken = "_Token";
        public const string SessionKeyUser = "_User";
        public string urn {get; set;}
        public string viewableId { get; set; }
        public Guid token {get; set;}
        public string User {get; set;}
        public Guid ModelId {get; set;}
        public string modelName { get; set; }
    }
}
