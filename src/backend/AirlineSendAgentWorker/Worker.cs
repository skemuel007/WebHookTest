using AirlineSendAgentWorker.Configs;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;
using System.Text;
using System.Threading.Channels;
using AirlineSendAgentWorker.Dtos;

namespace AirlineSendAgentWorker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly RabbitMQConfig _rabbitmqConfig;
    private string _queueName;

    public Worker(ILogger<Worker> logger,
        RabbitMQConfig rabbitMQConfig)
    {
        _logger = logger;
        _rabbitmqConfig = rabbitMQConfig;

        var factory = new ConnectionFactory() { HostName = rabbitMQConfig.Host, Port = rabbitMQConfig.Port };
        _connection = factory.CreateConnection();

        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(exchange: _rabbitmqConfig.Exchange, type: ExchangeType.Fanout);

        _queueName = _channel.QueueDeclare().QueueName;

        _channel.QueueBind(queue: _queueName,
            exchange: _rabbitmqConfig.Exchange, routingKey: "");

    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        _logger.LogInformation($"Listening for message on Event Bus...");
        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += (sender, ea) =>
        {
            _logger.LogInformation("Events triggered");
            var body = ea.Body;
            var notificationMessage = Encoding.UTF8.GetString(body.ToArray());

            var message = JsonSerializer.Deserialize<NotificationMessageDto>(notificationMessage);
        };

        _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);

        //return Task.CompletedTask;
        while(!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(0, stoppingToken);
        }
    }

    public override void Dispose()
    {
        _channel.Dispose();
        _connection.Dispose();
        base.Dispose();
    }
}