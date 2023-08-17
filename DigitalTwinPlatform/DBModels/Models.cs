using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalTwinPlatform.DBModels
{
    public class Models
    {
        [Key]
        public Guid Id { get; set; }
        public string username { get; set; }
        public string modelName { get; set; }
        public string urn { get; set; }
        public string viewableId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
