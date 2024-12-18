using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Itmo.Csharp.Microservices.Lab2.Task2.ConfigurationsSources;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConfigurationSource(this IServiceCollection collection)
    {
        collection.AddSingleton<IConfigurationSource, ConfigurationSource>();

        return collection;
    }
}