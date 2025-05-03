namespace Itmo.Csharp.Microservices.Lab2.Task3.Entities.OrderItems;

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