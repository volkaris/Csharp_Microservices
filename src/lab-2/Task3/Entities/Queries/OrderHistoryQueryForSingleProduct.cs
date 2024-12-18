using Itmo.Csharp.Microservices.Lab2.Task3.Entities.OrdersHistory;

namespace Itmo.Csharp.Microservices.Lab2.Task3.Entities.Queries;

public record OrderHistoryQueryForSingleProduct(
    int Cursor,
    int PageSize,
    long OrderId,
    OrderHistoryItemKind? HistoryKind);