using Itmo.Csharp.Microservices.Lab2.Task3.Entities.Orders;
using Itmo.Csharp.Microservices.Lab2.Task3.Entities.Orders.OrderStates;
using Itmo.Csharp.Microservices.Lab2.Task3.Entities.Queries;

namespace Itmo.Csharp.Microservices.Lab2.Task3.Repositories.Orders;

public interface IOrderRepository
{
    public Task<OrderDto> CreateOrderAsync(Order order, CancellationToken cancellationToken);

    public Task<OrderDto?> GetOrderByIdAsync(int orderId, CancellationToken cancellationToken);

    public Task ChangeOrderState(OrderDto order, OrderState newOrderState, CancellationToken cancellationToken);

    public IAsyncEnumerable<OrderDto> SearchOrderAsync(OrderQuery query, CancellationToken cancellationToken);
}