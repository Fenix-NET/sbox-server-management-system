using Infrastructure.Messaging.EventBus.IntegrationEvents;

namespace Infrastructure.Messaging.EventBus;

public interface IEventBus
{
    /// <summary>
    /// Публикуем событие
    /// </summary>
    /// <param name="event"></param>
    void Publish(IntegrationEvent @event);
    /// <summary>
    /// Подписывает обработчик на конкретное событие.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TH"></typeparam>
    void Subscribe<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>;
    /// <summary>
    /// Отписывает обработчик от конкретного события.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TH"></typeparam>
    void Unsubscribe<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>;
}