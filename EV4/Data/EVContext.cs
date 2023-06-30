using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EV4.Models;

namespace EV4.Data
{
    public class EVContext : DbContext
    {
        public EVContext (DbContextOptions<EVContext> options)
            : base(options)
        {
        }

        public DbSet<EV4.Models.Empleado> Empleado { get; set; } = default!;
    }
}
