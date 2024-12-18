using Itmo.Csharp.Microservices.Lab2.Task3.Entities.Orders;
using orderService;

namespace Itmo.Csharp.Microservices.Lab4.Mappers.FromGrpcToDomain;

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

    public static OrderDto ToOrderDto(this AddProductToOrderRequest request)
    {
        return new OrderDto(
            request.OrderState.ToDomainOrderState(),
            request.CreatedAt.ToDateTime(),
            request.CreatedBy,
            request.OrderId);
    }

    public static OrderDto ToOrderDto(this DeleteProductFromOrderRequest request)
    {
        return new OrderDto(
            request.OrderState.ToDomainOrderState(),
            request.CreatedAt.ToDateTime(),
            request.CreatedBy,
            request.OrderId);
    }

    public static OrderDto ToOrderDto(this ChangeOrderStateToProcessingRequest request)
    {
        return new OrderDto(
            request.OrderState.ToDomainOrderState(),
            request.CreatedAt.ToDateTime(),
            request.CreatedBy,
            request.OrderId);
    }

    public static OrderDto ToOrderDto(this CompleteOrderRequest request)
    {
        return new OrderDto(
            request.OrderState.ToDomainOrderState(),
            request.CreatedAt.ToDateTime(),
            request.CreatedBy,
            request.OrderId);
    }

    public static OrderDto ToOrderDto(this CancelOrderRequest request)
    {
        return new OrderDto(
            request.OrderState.ToDomainOrderState(),
            request.CreatedAt.ToDateTime(),
            request.CreatedBy,
            request.OrderId);
    }
}