using AirlineSendAgent.Client;
using AirlineSendAgent.Configs;
using AirlineSendAgent.Data;
using AirlineSendAgent.Dtos;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using System.Text;
using System.Text.Json;

namespace AirlineSendAgent.App
{
    public class AppHost : IAppHost
    {
        private readonly ILogger _logger;
        private readonly RabbitMQConfig _rabbitmqConfig;
        private readonly SendAgentDbContext _dbContext;
        private readonly IWebhookClient _webhookClient;

        public AppHost(ILogger logger,
            RabbitMQConfig rabbitMQConfig,
            SendAgentDbContext dbContext,
            IWebhookClient webhookClient)
        {
            _logger = logger;
            _rabbitmqConfig = rabbitMQConfig;
            _dbContext = dbContext;
            _webhookClient = webhookClient;
        }
        public async Task Run(CancellationToken stoppingToken)
        {
            _logger.Information("App is running");
            var factory = new ConnectionFactory()
            {
                HostName = _rabbitmqConfig.Host,
                Port = _rabbitmqConfig.Port
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: _rabbitmqConfig.Exchange, type: ExchangeType.Fanout);

                var queueName = channel.QueueDeclare().QueueName;

                channel.QueueBind(queue: queueName,
                    exchange: _rabbitmqConfig.Exchange, routingKey: "");

                var consumer = new EventingBasicConsumer(channel);

                _logger.Information($"Listening for message on Event Bus...");
                consumer.Received += async (ModuleHandle, ea) =>
                {
                    _logger.Information("Events triggered");
                    var body = ea.Body;
                    var notificationMessage = Encoding.UTF8.GetString(body.ToArray());

                    var message = JsonSerializer.Deserialize<NotificationMessageDto>(notificationMessage);

                    var webHookToSend = new FlightDetailChangePayloadDto()
                    {
                        WebhookType = message.WebhookType,
                        WebhookUri = string.Empty,
                        Secret = string.Empty,
                        Publisher = string.Empty,
                        OldPrice = message.OldPrice,
                        NewPrice = message.NewPrice,
                        FlightCode = message.FlightCode,
                    };

                    foreach (var webhookSubscription in _dbContext.WebhookSubscription.Where(s => s.WebHookType.Equals(message.WebhookType)))
                    {
                        webHookToSend.WebhookUri = webhookSubscription.WebHookUri;
                        webHookToSend.Secret = webhookSubscription.Secret;
                        webHookToSend.Publisher = webhookSubscription.WebHookPublisher;

                        _logger.Information($"Send webhook uri data {webHookToSend}");
                        await _webhookClient.SendWebhookNotification(webHookToSend);
                        _logger.Information($"Finished send request data");
                    }
                };

                channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
                //return Task.CompletedTask;
                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(0, stoppingToken);
                }
            }
        }
    }
}
