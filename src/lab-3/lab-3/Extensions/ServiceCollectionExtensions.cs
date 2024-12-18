using Itmo.Csharp.Microservices.Lab3.Middlewares;
using Itmo.Csharp.Microservices.Lab3.Options;

namespace Itmo.Csharp.Microservices.Lab3.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPageInfoOptions(this IServiceCollection collection, ConfigurationManager manager)
    {
        collection.AddOptions<PageInfoOptions>().Bind(manager.GetSection("PageInfo"));

        return collection;
    }

    public static IServiceCollection AddExceptionHandler(this IServiceCollection collection)
    {
        collection.AddScoped<ExceptionHandler>();

        return collection;
    }
}