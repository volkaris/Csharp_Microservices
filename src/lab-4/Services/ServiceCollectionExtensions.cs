using Itmo.Csharp.Microservices.Lab4.Services.Orders;
using Itmo.Csharp.Microservices.Lab4.Services.Products;
using Microsoft.Extensions.DependencyInjection;

namespace Itmo.Csharp.Microservices.Lab4.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection collection)
    {
        collection.AddScoped<IOrderService, OrderService>();

        collection.AddScoped<IProductService, ProductService>();

        return collection;
    }
}