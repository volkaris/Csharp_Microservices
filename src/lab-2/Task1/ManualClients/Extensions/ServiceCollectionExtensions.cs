using Microsoft.Extensions.DependencyInjection;

namespace Itmo.Csharp.Microservices.Lab2.Task1.ManualClients.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddManualClient(this IServiceCollection collection)
    {
        collection.AddScoped<IConfigurationClient, ManualConfigurationClient>();

        return collection;
    }
}