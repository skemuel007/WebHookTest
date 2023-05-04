using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AirlineAPI.Models;

namespace AirlineAPI.Data
{
    public class AirlineDBContext : DbContext
    {
        public AirlineDBContext (DbContextOptions<AirlineDBContext> options)
            : base(options)
        {
        }

        public DbSet<WebHookSubscription> WebHookSubscription { get; set; } = default!;
        public DbSet<FlightDetail> FlightDetail { get; set; } = default!;
    }
}
