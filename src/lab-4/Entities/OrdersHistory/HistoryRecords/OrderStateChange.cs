using Itmo.Csharp.Microservices.Lab4.Entities.Orders.OrderStates;

namespace Itmo.Csharp.Microservices.Lab4.Entities.OrdersHistory.HistoryRecords;

public record OrderStateChange(long OrderId, OrderState PreviousState, OrderState NewState, string? Info = null) : OrderHistoryBaseType;