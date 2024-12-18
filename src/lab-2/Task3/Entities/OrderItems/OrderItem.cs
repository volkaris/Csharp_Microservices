namespace Itmo.Csharp.Microservices.Lab2.Task3.Entities.OrderItems;

public record OrderItem(long OrderId, long ProductId, int Quantity, bool Deleted = false, long? OrderItemId = null);