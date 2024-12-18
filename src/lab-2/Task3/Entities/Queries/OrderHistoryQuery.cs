using Itmo.Csharp.Microservices.Lab2.Task3.Entities.OrdersHistory;

namespace Itmo.Csharp.Microservices.Lab2.Task3.Entities.Queries;

public record OrderHistoryQuery(
    int Cursor,
    int PageSize,
    IList<long> OrderIds,
    OrderHistoryItemKind? HistoryKind);