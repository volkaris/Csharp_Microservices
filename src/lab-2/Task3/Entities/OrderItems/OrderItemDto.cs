namespace Itmo.Csharp.Microservices.Lab2.Task3.Entities.OrderItems;

public record OrderItemDto(long OrderId, long ProductId, int Quantity, long OrderItemId, bool Deleted = false);