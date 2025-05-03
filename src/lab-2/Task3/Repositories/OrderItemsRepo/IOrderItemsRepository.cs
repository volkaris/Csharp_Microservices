using Itmo.Csharp.Microservices.Lab2.Task3.Entities.OrderItems;
using Itmo.Csharp.Microservices.Lab2.Task3.Entities.Queries;

namespace Itmo.Csharp.Microservices.Lab2.Task3.Repositories.OrderItemsRepo;

public interface IOrderItemsRepository
{
    public Task<OrderItemDto> CreateOrderItemAsync(OrderItem orderItem, CancellationToken cancellationToken);

    public Task SoftDeleteAsync(long orderId, long productId, CancellationToken cancellationToken);

    public IAsyncEnumerable<OrderItemDto> SearchOrderItemAsync(OrderItemQuery query, CancellationToken cancellationToken);
}