using Itmo.Csharp.Microservices.Lab4.Entities.Extensions;
using Itmo.Csharp.Microservices.Lab4.Entities.Options;
using Itmo.Csharp.Microservices.Lab4.Repositories.OrderItemsRepo;
using Itmo.Csharp.Microservices.Lab4.Repositories.Orders;
using Itmo.Csharp.Microservices.Lab4.Repositories.OrdersHistory;
using Itmo.Csharp.Microservices.Lab4.Repositories.Products;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Itmo.Csharp.Microservices.Lab4.Repositories;

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

    public static IServiceCollection AddNpgsqlDataSource(
        this IServiceCollection collection)
    {
        collection.AddSingleton<NpgsqlDataSource>(serviceProvider =>
        {
            IOptions<PostgresOptions> options = serviceProvider.GetRequiredService<IOptions<PostgresOptions>>();

            string connectionString =
                $"Host={options.Value.Host};Port={options.Value.Port};Username={options.Value.Username};Password={options.Value.Password}";

            var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);

            dataSourceBuilder.MapEnums();

            NpgsqlDataSource dataSource = dataSourceBuilder.Build();

            return dataSource;
        });

        return collection;
    }
}