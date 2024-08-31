using Infrastructure.Messaging.EventBus.Common;
using Infrastructure.Messaging.EventBus.IntegrationEvents;

namespace Infrastructure.Messaging.Interfaces;

public interface IEventBusSubscriptionsManager
{
    event EventHandler<string> OnEventRemoved;
    bool IsEmpty { get; }

    void AddSubscription<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>;

    void RemoveSubscription<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>;

    bool HasSubscriptionsForEvent<T>() where T : IntegrationEvent;
    bool HasSubscriptionsForEvent(string eventName);
    Type GetEventTypeByName(string eventName);
    IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName);
    void Clear();

}