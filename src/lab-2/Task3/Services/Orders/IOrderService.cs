using Itmo.Csharp.Microservices.Lab2.Task3.Entities.OrderItems;
using Itmo.Csharp.Microservices.Lab2.Task3.Entities.Orders;
using Itmo.Csharp.Microservices.Lab2.Task3.Entities.OrdersHistory;
using Itmo.Csharp.Microservices.Lab2.Task3.Entities.Queries;

namespace Itmo.Csharp.Microservices.Lab2.Task3.Services.Orders;

public interface IOrderService
{
    public Task<OrderDto> CreateOrderAsync(Order order, CancellationToken cancellationToken);

    public Task<OrderDto?> GetOrderByIdAsync(long orderId, CancellationToken cancellationToken);

    public Task<OrderItemDto> AddProductToOrderAsync(OrderDto order, long productId, int quantity, CancellationToken cancellationToken);

    public Task DeleteProductFromOrderAsync(OrderDto order, long productId, CancellationToken cancellationToken);

    public Task ChangeOrderStateToProcessingAsync(OrderDto order, CancellationToken cancellationToken);

    public Task CompleteOrderAsync(OrderDto order, CancellationToken cancellationToken);

    public Task CancelOrderAsync(OrderDto order, CancellationToken cancellationToken);

    public IAsyncEnumerable<OrderHistory> SearchProductHistoryAsync(OrderHistoryQueryForSingleProduct query, CancellationToken cancellationToken);
}