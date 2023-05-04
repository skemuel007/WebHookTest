using AirlineSendAgent.App;
using AirlineSendAgent.Configs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using System.Threading;

namespace AirlineSendAgent
{
    public class MessageBusWorker : BackgroundService /*, IHostedService, IDisposable*/
    {
        private readonly ILogger _logger;
        public IServiceProvider Services { get; }
        public MessageBusWorker(ILogger logger, IServiceProvider services)
        {
            _logger = logger;
            Services = services;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.Information($"Task is started");
            using (var scope = Services.CreateScope())
            {
                var scopeProcessingService = scope.ServiceProvider.GetRequiredService<IAppHost>();
                scopeProcessingService.Run(stoppingToken);
            }

            return Task.CompletedTask;
        }
    }
    /*public class MessageBusWorker : BackgroundService
    {
        // private readonly IAppHost _appHost;
        private readonly ILogger _logger;
        // private readonly RabbitMQConfig _rabbitmqConfig;
        // private readonly RabbitMQ.Client.IConnection _connection;
        // private readonly RabbitMQ.Client.IModel _channel;

        public IServiceProvider Services { get; }
        public MessageBusWorker(IAppHost appHost,
            ILogger logger,
            RabbitMQConfig rabbitMQConfig,
            IServiceProvider services) {
            // _appHost = appHost;

            // _rabbitmqConfig = rabbitMQConfig;
            _logger = logger;

            Services = services;

            *//*var factory = new ConnectionFactory()
            {
                HostName = _rabbitmqConfig.Host,
                Port = _rabbitmqConfig.Port
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(exchange: _rabbitmqConfig.Exchange, type: ExchangeType.Fanout);

            var queueName = _channel.QueueDeclare().QueueName;

            _channel.QueueBind(queue: queueName,
                exchange: _rabbitmqConfig.Exchange, routingKey: "");*//*
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            *//*stoppingToken.ThrowIfCancellationRequested();

            // _appHost.Run();
            var consumer = new EventingBasicConsumer(_channel);
            var queueName = _channel.QueueDeclare().QueueName;

            _logger.Information($"Listening on the message bus...");
            consumer.Received += async (ModuleHandle, ea) =>
            {
                _logger.Information("Events triggered");
            };

            _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);*//*
            using (var scope = Services.CreateScope())
            {
                var scopeProcessingService = scope.ServiceProvider.GetRequiredService<IAppHost>();
                await scopeProcessingService.Run(stoppingToken);
            }

            // return Task.CompletedTask;
        }

        public override void Dispose()
        {
            // _channel.Dispose();
            // _connection.Dispose();
            base.Dispose();
        }
    }*/
}
