namespace Infrastructure.Messaging.EventBus.IntegrationEvents;

public interface IIntegrationEventHandler<in TIntegrationEvent> where TIntegrationEvent : IntegrationEvent
{
    Task Handle(TIntegrationEvent @event);
}