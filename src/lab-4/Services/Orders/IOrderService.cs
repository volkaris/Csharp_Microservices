using Itmo.Csharp.Microservices.Lab4.Entities.OrderItems;
using Itmo.Csharp.Microservices.Lab4.Entities.Orders;
using Itmo.Csharp.Microservices.Lab4.Entities.OrdersHistory;
using Itmo.Csharp.Microservices.Lab4.Entities.Queries;

namespace Itmo.Csharp.Microservices.Lab4.Services.Orders;

public interface IOrderService
{
    public Task<OrderDto> CreateOrderAsync(Order order, CancellationToken cancellationToken);

    public Task<OrderDto?> GetOrderByIdAsync(long orderId, CancellationToken cancellationToken);

    public Task<OrderItemDto> AddProductToOrderAsync(long orderId, long productId, int quantity, CancellationToken cancellationToken);

    public Task DeleteProductFromOrderAsync(long orderId, long productId, CancellationToken cancellationToken);

    public Task ChangeOrderStateToProcessingAsync(long orderId, string info, CancellationToken cancellationToken);

    public Task CompleteOrderAsync(long orderId, CancellationToken cancellationToken);

    public Task CancelOrderAsync(long orderId, CancellationToken cancellationToken);

    public IAsyncEnumerable<OrderHistory> SearchProductHistoryAsync(long orderId, OrderHistoryQueryForSingleProduct query, CancellationToken cancellationToken);
}