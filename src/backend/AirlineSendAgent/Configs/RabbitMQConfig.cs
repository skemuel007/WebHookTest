namespace AirlineSendAgent.Configs
{
    public class RabbitMQConfig
    {
        public int Port { get; set; }
        public string Host { get; set; } = default!;
        public string Exchange { get; set; } = default!;
    }
}
