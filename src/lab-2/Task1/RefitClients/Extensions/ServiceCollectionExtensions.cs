using Itmo.Csharp.Microservices.Lab2.Task1.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Refit;

namespace Itmo.Csharp.Microservices.Lab2.Task1.RefitClients.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRefitClient(this IServiceCollection collection)
    {
        collection.AddRefitClient<IRefitClient>()
            .ConfigureHttpClient((serviceProvider, client) =>
            {
                IOptions<OuterServiceOptions> options = serviceProvider.GetRequiredService<IOptions<OuterServiceOptions>>();

                client.BaseAddress = new Uri($"http://{options.Value.Host}:{options.Value.Port}");
            });

        collection.AddScoped<IConfigurationClient, RefitConfigurationClient>();

        return collection;
    }
}