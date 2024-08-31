using Infrastructure.Messaging.Interfaces;
using RabbitMQ.Client;

namespace Infrastructure.Messaging.Services;

public class DefaultRabbitMQPersistentConnection : IRabbitMQPersistentConnection
{
    private readonly IConnectionFactory _connectionFactory;
    private IConnection _connection;
    private bool _disposed;

    private readonly object sync_root = new object();

    public DefaultRabbitMQPersistentConnection(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
    }

    /// <summary>
    /// Проверяет, активно ли текущее соединение.
    /// </summary>
    public bool IsConnected => _connection != null && _connection.IsOpen && !_disposed;

    /// <summary>
    /// Пытается установить соединение с RabbitMQ.
    /// </summary>
    /// <returns></returns>
    public bool TryConnect()
    {
        lock (sync_root)
        {
            _connection = _connectionFactory.CreateConnection();

            if (IsConnected)
            {
                // Лог успешного соединения
                return true;
            }
            else
            {
                // Лог неудачного соединения
                return false;
            }
        }
    }

    /// <summary>
    /// Создает и возвращает канал для общения с RabbitMQ.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public IModel CreateModel()
    {
        if (!IsConnected)
        {
            throw new InvalidOperationException("Нет соединения с RabbitMQ");
        }

        return _connection.CreateModel();
    }

    public void Dispose()
    {
        if (_disposed) return;

        _disposed = true;
        _connection.Dispose();
    }
}