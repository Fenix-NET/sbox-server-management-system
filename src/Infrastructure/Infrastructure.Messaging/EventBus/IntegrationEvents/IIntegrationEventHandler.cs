namespace Infrastructure.Messaging.EventBus.IntegrationEvents;

public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler where TIntegrationEvent : IntegrationEvent
{
    Task Handle(TIntegrationEvent @event);
}
public interface IIntegrationEventHandler
{
}
