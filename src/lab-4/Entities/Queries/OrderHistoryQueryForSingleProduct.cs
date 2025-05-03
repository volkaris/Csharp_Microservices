using Itmo.Csharp.Microservices.Lab4.Entities.OrdersHistory;

namespace Itmo.Csharp.Microservices.Lab4.Entities.Queries;

public record OrderHistoryQueryForSingleProduct(
    int Cursor,
    int PageSize,
    OrderHistoryItemKind HistoryKind);