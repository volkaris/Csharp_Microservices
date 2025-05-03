using Itmo.Csharp.Microservices.Lab4.Gateway.Options;

namespace Itmo.Csharp.Microservices.Lab4.Gateway.Extensions;

public static class OptionsServiceCollectionExtensions
{
    public static IServiceCollection AddClientConnectionOptions(this IServiceCollection collection, IConfiguration configuration)
    {
        collection.AddOptions<ClientConnectionOptions>().Bind(configuration.GetSection("ClientConnectionOptions"));

        return collection;
    }

    public static IServiceCollection AddOuterOrderServiceConnectionOptions(this IServiceCollection collection, IConfiguration configuration)
    {
        collection.AddOptions<OuterOrderServiceConnectionOptions>().Bind(configuration.GetSection("OuterOrderServiceConnectionOptions"));

        return collection;
    }
}