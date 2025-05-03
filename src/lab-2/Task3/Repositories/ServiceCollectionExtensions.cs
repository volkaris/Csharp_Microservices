using Itmo.Csharp.Microservices.Lab2.Task3.Repositories.OrderItemsRepo;
using Itmo.Csharp.Microservices.Lab2.Task3.Repositories.Orders;
using Itmo.Csharp.Microservices.Lab2.Task3.Repositories.OrdersHistory;
using Itmo.Csharp.Microservices.Lab2.Task3.Repositories.Products;
using Microsoft.Extensions.DependencyInjection;

namespace Itmo.Csharp.Microservices.Lab2.Task3.Repositories;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection collection)
    {
        collection.AddScoped<IOrderItemsRepository, OrderItemsRepository>();

        collection.AddScoped<IOrderRepository, OrderRepository>();

        collection.AddScoped<IOrdersHistoryRepository, OrdersHistoryRepository>();

        collection.AddScoped<IProductsRepository, ProductsRepository>();

        return collection;
    }
}