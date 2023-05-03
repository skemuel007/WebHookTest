using AirlineAPI.Dtos;

namespace AirlineAPI.MessageBus
{
    public interface IMessageBusClient
    {
        void SendMessage(NotificationMessageDto notificationMessageDto);
    }
}
