namespace Itmo.Csharp.Microservices.Lab4.Entities.OrdersHistory.HistoryRecords;

public record ItemRemovedFromOrder(long ProductId, long OrderId) : OrderHistoryBaseType;