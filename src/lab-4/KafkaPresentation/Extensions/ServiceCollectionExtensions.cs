using Itmo.Csharp.Microservices.Lab4.KafkaIntegration.Consumer;
using Itmo.Csharp.Microservices.Lab4.KafkaPresentation.BackgroundServices;
using Itmo.Csharp.Microservices.Lab4.KafkaPresentation.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Orders.Kafka.Contracts;

namespace Itmo.Csharp.Microservices.Lab4.KafkaPresentation.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConsumerBackgroundService(this IServiceCollection collection)
    {
        collection.AddHostedService<ConsumerBackgroundService<OrderProcessingKey, OrderProcessingValue>>();

        return collection;
    }

    public static IServiceCollection AddConsumerHandler(this IServiceCollection collection)
    {
        collection.AddScoped<IConsumerHandler<OrderProcessingKey, OrderProcessingValue>, ConsumerHandler>();

        return collection;
    }
}