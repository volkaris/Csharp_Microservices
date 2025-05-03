namespace Itmo.Csharp.Microservices.Lab2.Task3.Entities.OrdersHistory.HistoryRecords;

public record ItemRemovedFromOrder(long ProductId, long OrderId) : OrderHistoryBaseType;