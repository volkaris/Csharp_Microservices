using Itmo.Csharp.Microservices.Lab4.Entities.Orders;
using Itmo.Csharp.Microservices.Lab4.Entities.Orders.OrderStates;
using Itmo.Csharp.Microservices.Lab4.Entities.Queries;

namespace Itmo.Csharp.Microservices.Lab4.Repositories.Orders;

public interface IOrderRepository
{
    public Task<OrderDto> CreateOrderAsync(Order order, CancellationToken cancellationToken);

    public Task<OrderDto?> GetOrderByIdAsync(long orderId, CancellationToken cancellationToken);

    public Task ChangeOrderState(OrderDto order, OrderState newOrderState, CancellationToken cancellationToken);

    public IAsyncEnumerable<OrderDto> SearchOrderAsync(OrderQuery query, CancellationToken cancellationToken);
}