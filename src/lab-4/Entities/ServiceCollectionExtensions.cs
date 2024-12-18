using Itmo.Csharp.Microservices.Lab4.Entities.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Itmo.Csharp.Microservices.Lab4.Entities;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPostgresOptions(
        this IServiceCollection collection,
        IConfigurationRoot configurationRoot)
    {
        collection.AddOptions<PostgresOptions>().Bind(configurationRoot.GetSection("Persistence:Postgres"));

        return collection;
    }
}