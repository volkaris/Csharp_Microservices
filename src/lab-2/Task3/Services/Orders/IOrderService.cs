using Itmo.Csharp.Microservices.Lab2.Task3.Entities.OrderItems;
using Itmo.Csharp.Microservices.Lab2.Task3.Entities.Orders;
using Itmo.Csharp.Microservices.Lab2.Task3.Entities.OrdersHistory;
using Itmo.Csharp.Microservices.Lab2.Task3.Entities.Products;
using Itmo.Csharp.Microservices.Lab2.Task3.Entities.Queries;

namespace Itmo.Csharp.Microservices.Lab2.Task3.Services.Orders;

public interface IOrderService
{
    public Task<OrderDto> CreateOrderAsync(Order order, CancellationToken cancellationToken);

    public Task<OrderItemDto> AddProductToOrder(OrderDto order, ProductDto product, int quantity, CancellationToken cancellationToken);

    public Task DeleteProductsFromOrder(OrderDto order, ProductDto product, CancellationToken cancellationToken);

    public Task ChangeOrderStateToProcessing(OrderDto order, CancellationToken cancellationToken);

    public Task CompleteOrder(OrderDto order, CancellationToken cancellationToken);

    public Task CancelOrder(OrderDto order, CancellationToken cancellationToken);

    public IAsyncEnumerable<OrderHistory> SearchProductHistoryAsync(OrderHistoryQueryForSingleProduct query, CancellationToken cancellationToken);
}