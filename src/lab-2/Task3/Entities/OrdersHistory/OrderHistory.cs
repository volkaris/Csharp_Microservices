using Itmo.Csharp.Microservices.Lab2.Task3.Entities.OrdersHistory.HistoryRecords;

namespace Itmo.Csharp.Microservices.Lab2.Task3.Entities.OrdersHistory;

public record OrderHistory(long OrderId, DateTime CreatedAt, OrderHistoryItemKind? Kind, OrderHistoryBaseType? ItemPayload);