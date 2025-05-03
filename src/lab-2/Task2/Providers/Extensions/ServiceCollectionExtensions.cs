using Microsoft.Extensions.DependencyInjection;

namespace Itmo.Csharp.Microservices.Lab2.Task2.Providers.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConfigurationProvider(this IServiceCollection collection)
    {
        collection.AddSingleton<CustomConfigurationProvider, CustomConfigurationProvider>();

        return collection;
    }
}