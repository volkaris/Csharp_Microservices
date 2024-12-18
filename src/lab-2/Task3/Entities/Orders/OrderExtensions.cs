namespace Itmo.Csharp.Microservices.Lab2.Task3.Entities.Orders;

public static class OrderExtensions
{
    public static Order ToOrder(this OrderDto orderDto)
    {
        return new Order(
            orderDto.State,
            orderDto.CreatedAt,
            orderDto.CreatedBy,
            orderDto.OrderId);
    }
}