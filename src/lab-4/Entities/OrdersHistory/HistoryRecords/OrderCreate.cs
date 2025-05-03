namespace Itmo.Csharp.Microservices.Lab4.Entities.OrdersHistory.HistoryRecords;

public record OrderCreate(long OrderId, DateTimeOffset CreationDate) : OrderHistoryBaseType;