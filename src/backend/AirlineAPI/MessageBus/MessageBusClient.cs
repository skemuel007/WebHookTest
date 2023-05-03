using AirlineAPI.Configs;
using AirlineAPI.Dtos;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace AirlineAPI.MessageBus
{
    public class MessageBusClient : IMessageBusClient
    {
        // using connection factory
        private readonly RabbitMQConfig _rabbitmqConfig;
        private readonly ILogger<MessageBusClient> _logger;
        public MessageBusClient(IOptionsMonitor<RabbitMQConfig> optionsMonitor,
            ILogger<MessageBusClient> logger) {
            _rabbitmqConfig = optionsMonitor.CurrentValue;
            _logger = logger;
        }

        public void SendMessage(NotificationMessageDto notificationMessageDto)
        {
            var factory = new ConnectionFactory() { HostName = _rabbitmqConfig.Host, Port = _rabbitmqConfig.Port };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: _rabbitmqConfig.Exchange, type: ExchangeType.Fanout);

                var message = JsonSerializer.Serialize(notificationMessageDto);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: _rabbitmqConfig.Exchange,
                    routingKey: "", basicProperties: null, body: body);

                _logger.LogInformation($"--> Message Published on Message Bus");
            }
        }
    }
}
