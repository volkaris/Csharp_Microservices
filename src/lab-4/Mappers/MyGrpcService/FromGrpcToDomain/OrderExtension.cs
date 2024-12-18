using Itmo.Csharp.Microservices.Lab4.Entities.Orders;
using orderService;

namespace Itmo.Csharp.Microservices.Lab4.Mappers.MyGrpcService.FromGrpcToDomain;

public static class OrderExtension
{
    public static OrderDto ToOrderDto(this GetOrderByIdResponse response)
    {
        return new OrderDto(
            response.OrderState.ToDomainOrderState(),
            response.CreatedAt.ToDateTime(),
            response.CreatedBy,
            response.OrderId);
    }

    public static OrderDto ToOrderDto(this CreateOrderResponse response)
    {
        return new OrderDto(
            response.OrderState.ToDomainOrderState(),
            response.CreatedAt.ToDateTime(),
            response.CreatedBy,
            response.OrderId);
    }
}