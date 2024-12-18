using Itmo.Csharp.Microservices.Lab2.Task3.Entities.Orders.OrderStates;

namespace Itmo.Csharp.Microservices.Lab2.Task3.Entities.Orders;

public record OrderDto(OrderState State, DateTime CreatedAt, string CreatedBy, long OrderId);