namespace Itmo.Csharp.Microservices.Lab4.Entities.OrderItems;

public record OrderItemDto(long OrderId, long ProductId, int Quantity, long OrderItemId, bool Deleted = false);