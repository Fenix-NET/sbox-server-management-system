using RabbitMQ.Client;

namespace Infrastructure.Messaging.Interfaces;

public interface IRabbitMQPersistentConnection
{
    bool IsConnected { get; }
    bool TryConnect();
    IModel CreateModel();
}