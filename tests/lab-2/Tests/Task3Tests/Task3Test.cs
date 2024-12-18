using Itmo.Csharp.Microservices.Lab2.Task1;
using Itmo.Csharp.Microservices.Lab2.Task1.Entities;
using Itmo.Csharp.Microservices.Lab2.Task1.RefitClients.Extensions;
using Itmo.Csharp.Microservices.Lab2.Task2.ConfigurationsSources;
using Itmo.Csharp.Microservices.Lab2.Task2.Providers;
using Itmo.Csharp.Microservices.Lab2.Task2.Providers.Extensions;
using Itmo.Csharp.Microservices.Lab2.Task2.Services.Extensions;
using Itmo.Csharp.Microservices.Lab2.Task2.Services.Implementations;
using Itmo.Csharp.Microservices.Lab2.Task3;
using Itmo.Csharp.Microservices.Lab2.Task3.Entities.OrderItems;
using Itmo.Csharp.Microservices.Lab2.Task3.Entities.Orders;
using Itmo.Csharp.Microservices.Lab2.Task3.Entities.Orders.OrderStates;
using Itmo.Csharp.Microservices.Lab2.Task3.Entities.OrdersHistory;
using Itmo.Csharp.Microservices.Lab2.Task3.Entities.Products;
using Itmo.Csharp.Microservices.Lab2.Task3.Entities.Queries;
using Itmo.Csharp.Microservices.Lab2.Task3.Migrations;
using Itmo.Csharp.Microservices.Lab2.Task3.Repositories;
using Itmo.Csharp.Microservices.Lab2.Task3.Services;
using Itmo.Csharp.Microservices.Lab2.Task3.Services.Orders;
using Itmo.Csharp.Microservices.Lab2.Task3.Services.Products;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Itmo.Csharp.Microservices.Lab2.Tests.Task3Tests;

#pragma warning disable CA1506
public class Task3Test
{
    private readonly ITestOutputHelper _testOutputHelper;

    public Task3Test(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task TestFunctionality()
    {
        IServiceCollection collection = new ServiceCollection();

        using var configManager = new ConfigurationManager();

        IConfigurationBuilder configurationBuilder = configManager;

        IConfigurationRoot configurationRoot = configManager;

        IConfiguration config = configurationBuilder
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("file.json", false, true)
            .Build();

        var outerServiceOptions = new OuterServiceOptions();

        config.GetSection("OuterService:Options").Bind(outerServiceOptions);

        collection.AddSingleton(outerServiceOptions);

        collection.AddHttpClient();
        /*collection.AddManualClient();*/
        collection.AddRefitClient();

        collection.AddConfigurationProvider();
        collection.AddConfigurationSource();
        collection.AddConfigurationService();

        collection.AddRepositories();
        collection.AddServices();

        collection.AddNpgsqlDataSource();
        collection.AddPostgresOptions(configurationRoot);

        collection.AddMigrations();

        await using ServiceProvider serviceProvider = collection.BuildServiceProvider();

        CustomConfigurationProvider provider = serviceProvider.GetRequiredService<CustomConfigurationProvider>();

        configurationBuilder.Add(serviceProvider.GetRequiredService<IConfigurationSource>());

        var configurationService = new ConfigurationService(provider, serviceProvider.GetRequiredService<IConfigurationClient>());

        using var cancellationTokenSource = new CancellationTokenSource();

        cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(5));

        await configurationService.UpdateAfter(4, null, TimeSpan.FromSeconds(1), cancellationTokenSource.Token);

        await serviceProvider.StartMigrations();

        IProductService productService = serviceProvider.GetRequiredService<IProductService>();
        IOrderService ordersService = serviceProvider.GetRequiredService<IOrderService>();

        ProductDto product1 = await productService.CreateProductAsync(new Product("1", 100), CancellationToken.None);
        ProductDto product2 = await productService.CreateProductAsync(new Product("2", 200), CancellationToken.None);
        ProductDto product3 = await productService.CreateProductAsync(new Product("3", 300), CancellationToken.None);

        OrderDto order = await ordersService.CreateOrderAsync(new Order(OrderState.Created, DateTime.Now, "volkaris"), CancellationToken.None);

        OrderItemDto orderItem1 = await ordersService.AddProductToOrder(order, product1, 10, CancellationToken.None);
        OrderItemDto orderItem2 = await ordersService.AddProductToOrder(order, product2, 20, CancellationToken.None);
        OrderItemDto orderItem3 = await ordersService.AddProductToOrder(order, product3, 30, CancellationToken.None);

        await ordersService.DeleteProductFromOrder(order, product1, CancellationToken.None);

        await ordersService.ChangeOrderStateToProcessing(order, CancellationToken.None);
        await ordersService.CompleteOrder(order, CancellationToken.None);

        await foreach (OrderHistory historyRecord in ordersService.SearchProductHistoryAsync(
                           new OrderHistoryQueryForSingleProduct(0, 100, order.OrderId, null),
                           CancellationToken.None))
        {
            _testOutputHelper.WriteLine(historyRecord.ToString());
        }
    }
}