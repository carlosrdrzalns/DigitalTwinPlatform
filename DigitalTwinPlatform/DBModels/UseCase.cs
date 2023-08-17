using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalTwinPlatform.DBModels
{
    public class UseCase
    {
        [Key]
        public Guid Id { get; set; }
        public Guid modelId { get; set; }
        public string caseName { get; set; }
        public string state { get; set; }
    }
}
