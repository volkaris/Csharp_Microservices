using Itmo.Csharp.Microservices.Lab4.KafkaPresentation.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Itmo.Csharp.Microservices.Lab4.KafkaPresentation;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddKafkaBackgroundServiceOptions(this IServiceCollection collection, IConfiguration configuration)
    {
        collection.AddOptions<BackgroundServiceOptions>().Bind(configuration.GetSection("Kafka:BackgroundService"));

        return collection;
    }
}