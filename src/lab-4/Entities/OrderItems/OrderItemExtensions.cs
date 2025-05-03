namespace Itmo.Csharp.Microservices.Lab4.Entities.OrderItems;

public static class OrderItemExtensions
{
    public static OrderItem ToOrderItem(this OrderItemDto orderItemDto)
    {
        return new OrderItem(
            orderItemDto.OrderId,
            orderItemDto.ProductId,
            orderItemDto.Quantity,
            orderItemDto.Deleted,
            orderItemDto.OrderItemId);
    }
}