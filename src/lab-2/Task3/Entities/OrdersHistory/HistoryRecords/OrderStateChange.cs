using Itmo.Csharp.Microservices.Lab2.Task3.Entities.Orders.OrderStates;

namespace Itmo.Csharp.Microservices.Lab2.Task3.Entities.OrdersHistory.HistoryRecords;

public record OrderStateChange(long OrderId, OrderState PreviousState, OrderState NewState) : OrderHistoryBaseType;