using Itmo.Csharp.Microservices.Lab4.Gateway.Options;
using Microsoft.Extensions.Options;
using Orders.ProcessingService.Contracts;
using orderService;
using products;

namespace Itmo.Csharp.Microservices.Lab4.Gateway.Extensions;

public static class ClientServiceCollectionExtensions
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
        collection.AddGrpcClient<OrdersService.OrdersServiceClient>(
            (serviceProvider, instance) =>
            {
                IOptions<ClientConnectionOptions> options =
                    serviceProvider.GetRequiredService<IOptions<ClientConnectionOptions>>();
                instance.Address = new Uri(options.Value.Address);
            });

        return collection;
    }

    public static IServiceCollection AddOuterOrdersServiceClient(this IServiceCollection collection)
    {
        collection.AddGrpcClient<OrderService.OrderServiceClient>(
            (serviceProvider, instance) =>
            {
                IOptions<OuterOrderServiceConnectionOptions> options =
                    serviceProvider.GetRequiredService<IOptions<OuterOrderServiceConnectionOptions>>();
                instance.Address = new Uri(options.Value.Address);
            });

        return collection;
    }
}