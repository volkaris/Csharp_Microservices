using Itmo.Csharp.Microservices.Lab4.KafkaIntegration.Producers.Services;
using Microsoft.Extensions.DependencyInjection;
using Orders.Kafka.Contracts;

namespace Itmo.Csharp.Microservices.Lab4.KafkaIntegration.Producers.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddProducer(this IServiceCollection collection)
    {
        collection.AddScoped<IKafkaServiceProducer<OrderCreationKey, OrderCreationValue>, KafkaServiceProducer<OrderCreationKey, OrderCreationValue>>();

        return collection;
    }
}