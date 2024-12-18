using Itmo.Csharp.Microservices.Lab4.Gateway.Options;
using Microsoft.Extensions.Options;
using products;

namespace Itmo.Csharp.Microservices.Lab4.Gateway.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddProductsServiceClient(this IServiceCollection collection)
    {
        collection.AddGrpcClient<ProductsService.ProductsServiceClient>(
            (serviceProvider, instance) =>
            {
                IOptions<ClientConnectionOptions> options =
                    serviceProvider.GetRequiredService<IOptions<ClientConnectionOptions>>();
                instance.Address = new Uri(options.Value.Address);
            });

        return collection;
    }

    public static IServiceCollection AddOrdersServiceClient(this IServiceCollection collection)
    {
        collection.AddGrpcClient<ProductsService.ProductsServiceClient>(
            (serviceProvider, instance) =>
            {
                IOptions<ClientConnectionOptions> options =
                    serviceProvider.GetRequiredService<IOptions<ClientConnectionOptions>>();
                instance.Address = new Uri(options.Value.Address);
            });

        return collection;
    }
}