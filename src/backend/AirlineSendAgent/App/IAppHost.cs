using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineSendAgent.App
{
    public interface IAppHost
    {
        Task Run(CancellationToken stoppingToken);
    }
}
