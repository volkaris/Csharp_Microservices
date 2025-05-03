using Itmo.Csharp.Microservices.Lab2.Task1.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Itmo.Csharp.Microservices.Lab2.Task1.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOuterServiceOptions(this IServiceCollection collection, ConfigurationManager manager)
    {
        collection.AddOptions<OuterServiceOptions>().Bind(manager.GetSection("OuterService:Options"));

        return collection;
    }
}