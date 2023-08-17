using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalTwinPlatform.DBModels
{
    public class DTModelsContext : DbContext
    {
        public DTModelsContext(DbContextOptions<DTModelsContext> options) : base(options)
        {
        }
        public DbSet<Models> Models { get; set; }
        public DbSet<UseCase> UseCase { get; set; }
    }
}
