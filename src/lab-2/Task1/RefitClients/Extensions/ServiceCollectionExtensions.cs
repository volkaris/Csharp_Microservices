using Itmo.Csharp.Microservices.Lab2.Task1.Entities;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace Itmo.Csharp.Microservices.Lab2.Task1.RefitClients.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRefitClient(this IServiceCollection collection)
    {
        collection.AddRefitClient<IRefitClient>()
            .ConfigureHttpClient((serviceProvider, client) =>
            {
                OuterServiceOptions options = serviceProvider.GetRequiredService<OuterServiceOptions>();

                client.BaseAddress = new Uri($"http://{options.Host}:{options.Port}");
            });

        collection.AddScoped<IConfigurationClient, RefitConfigurationClient>();

        return collection;
    }
}