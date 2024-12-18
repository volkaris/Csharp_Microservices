using Itmo.Csharp.Microservices.Lab2.Task3.Entities.OrderItems;
using orderService;

namespace Itmo.Csharp.Microservices.Lab4.Mappers.FromDomainToGrpc;

public static class OrderItemExtensions
{
    public static AddProductToOrderResponse ToAddProductToOrderResponse(this OrderItemDto orderItemDto)
    {
        return new AddProductToOrderResponse
        {
            OrderId = orderItemDto.OrderId,
            ProductId = orderItemDto.ProductId,
            Quantity = orderItemDto.Quantity,
            OrderItemId = orderItemDto.OrderItemId,
            Deleted = orderItemDto.Deleted,
        };
    }
}