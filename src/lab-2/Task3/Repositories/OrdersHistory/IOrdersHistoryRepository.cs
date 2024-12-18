using Itmo.Csharp.Microservices.Lab2.Task3.Entities.OrdersHistory;
using Itmo.Csharp.Microservices.Lab2.Task3.Entities.Queries;

namespace Itmo.Csharp.Microservices.Lab2.Task3.Repositories.OrdersHistory;

public interface IOrdersHistoryRepository
{
    public Task CreateOrderHistoryAsync(OrderHistory orderHistory, CancellationToken cancellationToken);

    public IAsyncEnumerable<OrderHistory> SearchOrderHistoryAsync(OrderHistoryQuery query, CancellationToken cancellationToken);
}