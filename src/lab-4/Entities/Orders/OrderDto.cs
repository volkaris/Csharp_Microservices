using Itmo.Csharp.Microservices.Lab4.Entities.Orders.OrderStates;

namespace Itmo.Csharp.Microservices.Lab4.Entities.Orders;

public record OrderDto(OrderState State, DateTime CreatedAt, string CreatedBy, long OrderId);