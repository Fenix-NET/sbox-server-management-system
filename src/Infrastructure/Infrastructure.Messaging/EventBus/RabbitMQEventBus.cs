using System.Text;
using Infrastructure.Messaging.EventBus.IntegrationEvents;
using Infrastructure.Messaging.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Infrastructure.Messaging.EventBus;

public class RabbitMQEventBus : IEventBus, IDisposable
{
    private readonly IRabbitMQPersistentConnection _persistentConnection;
    private readonly IEventBusSubscriptionsManager _subsManager;
    private readonly IServiceProvider _serviceProvider;
    private IModel _consumerChannel;
    private readonly string _exchangeName = "event_bus";
    private string _queueName;

    public RabbitMQEventBus(IRabbitMQPersistentConnection persistentConnection,
                            IEventBusSubscriptionsManager subsManager,
                            IServiceProvider serviceProvider,
                            string queueName = null)
    {
        _persistentConnection = persistentConnection ?? throw new ArgumentNullException(nameof(persistentConnection));
        _subsManager = subsManager;
        _serviceProvider = serviceProvider;
        _queueName = queueName;

        _consumerChannel = CreateConsumerChannel();
        _subsManager.OnEventRemoved += SubsManager_OnEventRemoved;
    }

    /// <summary>
    /// Публикует событие в RabbitMQ.
    /// </summary>
    /// <param name="event"></param>
    public void Publish(IntegrationEvent @event)
    {
        if (!_persistentConnection.IsConnected)
        {
            _persistentConnection.TryConnect();
        }

        using (var channel = _persistentConnection.CreateModel())
        {
            channel.ExchangeDeclare(exchange: _exchangeName, type: "direct");

            var message = JsonConvert.SerializeObject(@event);
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: _exchangeName,
                                 routingKey: @event.GetType().Name,
                                 basicProperties: null,
                                 body: body);
        }
    }

    /// <summary>
    /// Подписывает обработчик на событие и создает привязку очереди к этому событию.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TH"></typeparam>
    public void Subscribe<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        var eventName = typeof(T).Name;

        if (!_subsManager.HasSubscriptionsForEvent(eventName))
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            using (var channel = _persistentConnection.CreateModel())
            {
                channel.QueueDeclare(queue: _queueName,
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                channel.QueueBind(queue: _queueName,
                                  exchange: _exchangeName,
                                  routingKey: eventName);
            }
        }

        _subsManager.AddSubscription<T, TH>();
        StartBasicConsume();
    }

    /// <summary>
    /// Отписывает обработчик от события и удаляет привязку.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TH"></typeparam>
    public void Unsubscribe<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        _subsManager.RemoveSubscription<T, TH>();
    }

    /// <summary>
    /// Начинает прослушивание сообщений из очереди.
    /// </summary>
    private void StartBasicConsume()
    {
        if (_consumerChannel != null)
        {
            var consumer = new EventingBasicConsumer(_consumerChannel);
            consumer.Received += async (model, ea) =>
            {
                var eventName = ea.RoutingKey;
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());

                await ProcessEvent(eventName, message);
            };

            _consumerChannel.BasicConsume(queue: _queueName,
                                          autoAck: false,
                                          consumer: consumer);
        }
    }

    /// <summary>
    /// Обрабатывает полученное событие, вызывает соответствующий обработчик.
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="message"></param>
    private async Task ProcessEvent(string eventName, string message)
    {
        if (_subsManager.HasSubscriptionsForEvent(eventName))
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var subscriptions = _subsManager.GetHandlersForEvent(eventName);
                foreach (var subscription in subscriptions)
                {
                    var handler = scope.ServiceProvider.GetRequiredService(subscription.HandlerType);
                    var eventType = _subsManager.GetEventTypeByName(eventName);
                    var integrationEvent = JsonConvert.DeserializeObject(message, eventType);
                    var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

                    await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
                }
            }
        }
    }

    private IModel CreateConsumerChannel()
    {
        var channel = _persistentConnection.CreateModel();

        channel.QueueDeclare(queue: _queueName,
                             durable: true,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        return channel;
    }

    private void SubsManager_OnEventRemoved(object sender, string eventName)
    {
        if (!_persistentConnection.IsConnected)
        {
            _persistentConnection.TryConnect();
        }

        using (var channel = _persistentConnection.CreateModel())
        {
            channel.QueueUnbind(queue: _queueName,
                                exchange: _exchangeName,
                                routingKey: eventName);

            if (_subsManager.IsEmpty)
            {
                _queueName = string.Empty;
                _consumerChannel.Close();
            }
        }
    }

    public void Dispose()
    {
        _consumerChannel?.Dispose();
        _subsManager.Clear();
    }
}