namespace Itmo.Csharp.Microservices.Lab2.Task3.Entities.OrdersHistory.HistoryRecords;

public record OrderCreated(long OrderId, DateTimeOffset CreationDate) : OrderHistoryBaseType;