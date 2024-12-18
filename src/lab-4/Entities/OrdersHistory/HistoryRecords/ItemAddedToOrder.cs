namespace Itmo.Csharp.Microservices.Lab4.Entities.OrdersHistory.HistoryRecords;

public record ItemAddedToOrder(long ProductId, long OrderId, int Quantity) : OrderHistoryBaseType;