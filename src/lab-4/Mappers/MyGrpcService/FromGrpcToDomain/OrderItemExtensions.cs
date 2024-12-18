using Itmo.Csharp.Microservices.Lab4.Entities.OrderItems;
using orderService;

namespace Itmo.Csharp.Microservices.Lab4.Mappers.MyGrpcService.FromGrpcToDomain;

public static class OrderItemExtensions
{
    public static OrderItemDto ToOrderItemDto(this AddProductToOrderResponse response)
    {
        return new OrderItemDto(response.OrderId, response.ProductId, response.Quantity, response.OrderItemId, response.Deleted);
    }
}