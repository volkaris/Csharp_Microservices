using Itmo.Csharp.Microservices.Lab4.KafkaIntegration.Consumer.Readers;
using Microsoft.Extensions.DependencyInjection;
using Orders.Kafka.Contracts;

namespace Itmo.Csharp.Microservices.Lab4.KafkaIntegration.Consumer.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConsumerReader(this IServiceCollection collection)
    {
        collection
            .AddScoped<IConsumerReader<OrderProcessingKey, OrderProcessingValue>,
                ConsumerReader<OrderProcessingKey, OrderProcessingValue>>();

        return collection;
    }
}