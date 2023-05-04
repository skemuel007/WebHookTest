﻿using AirlineSendAgentWorker.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineSendAgentWorker.Data
{
    public class SendAgentDbContext : DbContext
    {
        public SendAgentDbContext(DbContextOptions<SendAgentDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<WebhookSubscription> WebhookSubscription { get; set; }
    }
}
