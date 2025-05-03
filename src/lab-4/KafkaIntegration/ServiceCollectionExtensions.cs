using Itmo.Csharp.Microservices.Lab4.KafkaIntegration.Consumer.Options;
using Itmo.Csharp.Microservices.Lab4.KafkaIntegration.Producers.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Itmo.Csharp.Microservices.Lab4.KafkaIntegration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddKafkaProducerOptions(this IServiceCollection collection, IConfiguration configuration)
    {
        collection.AddOptions<ProducerOptions>().Bind(configuration.GetSection("Kafka:Producer"));

        return collection;
    }

    public static IServiceCollection AddKafkaConsumerOptions(this IServiceCollection collection, IConfiguration configuration)
    {
        collection.AddOptions<ConsumerHandlerOptions>().Bind(configuration.GetSection("Kafka:Consumer:Handler"));

        collection.AddOptions<ConsumerReaderOptions>().Bind(configuration.GetSection("Kafka:Consumer:Reader"));

        return collection;
    }
}