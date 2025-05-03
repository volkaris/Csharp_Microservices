using Itmo.Csharp.Microservices.Lab4.Entities.OrdersHistory;
using Itmo.Csharp.Microservices.Lab4.Entities.Queries;

namespace Itmo.Csharp.Microservices.Lab4.Repositories.OrdersHistory;

public interface IOrdersHistoryRepository
{
    public Task CreateOrderHistoryAsync(OrderHistory orderHistory, CancellationToken cancellationToken);

    public IAsyncEnumerable<OrderHistory> SearchOrderHistoryAsync(OrderHistoryQuery query, CancellationToken cancellationToken);
}