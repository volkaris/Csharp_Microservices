namespace Itmo.Csharp.Microservices.Lab2.Task3.Entities.OrdersHistory.HistoryRecords;

public record ItemAddedToOrder(long ProductId, long OrderId, int Quantity) : OrderHistoryBaseType;