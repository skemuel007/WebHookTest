using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TravelAgentAPI.Models;

namespace TravelAgentAPI.Data
{
    public class TravelAgentDbContext : DbContext
    {
        public TravelAgentDbContext (DbContextOptions<TravelAgentDbContext> options)
            : base(options)
        {
        }

        public DbSet<WebhookSecret> SubscriptionSecret { get; set; } = default!;
    }
}
