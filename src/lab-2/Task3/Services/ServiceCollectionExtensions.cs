using Itmo.Csharp.Microservices.Lab2.Task3.Services.Orders;
using Itmo.Csharp.Microservices.Lab2.Task3.Services.Products;
using Microsoft.Extensions.DependencyInjection;

namespace Itmo.Csharp.Microservices.Lab2.Task3.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection collection)
    {
        collection.AddScoped<IOrderService, OrderService>();

        collection.AddScoped<IProductService, ProductService>();

        return collection;
    }
}