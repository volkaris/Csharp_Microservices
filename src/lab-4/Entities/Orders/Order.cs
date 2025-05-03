using Itmo.Csharp.Microservices.Lab4.Entities.Orders.OrderStates;

namespace Itmo.Csharp.Microservices.Lab4.Entities.Orders;

public record Order(OrderState State, DateTime CreatedAt, string CreatedBy);