using Itmo.Csharp.Microservices.Lab4.Entities.OrdersHistory;

namespace Itmo.Csharp.Microservices.Lab4.Entities.Queries;

public record OrderHistoryQuery(
    int Cursor,
    int PageSize,
    IList<long> OrderIds,
    OrderHistoryItemKind? HistoryKind);