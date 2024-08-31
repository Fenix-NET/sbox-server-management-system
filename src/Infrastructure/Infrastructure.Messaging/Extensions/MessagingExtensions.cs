using Infrastructure.Messaging.EventBus;
using Infrastructure.Messaging.Interfaces;
using Infrastructure.Messaging.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Infrastructure.Messaging.Extensions;

public static class MessagingExtensions
{
    public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
        {
            var factory = new ConnectionFactory()
            {
                HostName = configuration["EventBus:HostName"]
            };

            return new DefaultRabbitMQPersistentConnection(factory);
        });

        services.AddSingleton<IEventBus, RabbitMQEventBus>(sp =>
        {
            var persistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
            var serviceProvider = sp.GetRequiredService<IServiceProvider>();
            var subsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

            return new RabbitMQEventBus(persistentConnection, subsManager, serviceProvider);
        });

        services.AddSingleton<IEventBusSubscriptionsManager, EventBusSubscriptionsManager>();

        return services;
    }
}