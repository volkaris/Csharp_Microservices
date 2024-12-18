using Google.Protobuf.WellKnownTypes;
using Itmo.Csharp.Microservices.Lab2.Task3.Entities.Orders;
using orderService;

namespace Itmo.Csharp.Microservices.Lab4.Mappers.FromDomainToGrpc;

public static class OrderExtension
{
    public static CreateOrderRequest ToCreateOrderRequest(this Order order)
    {
        return new CreateOrderRequest
        {
            OrderState = order.State.ToGrpcOrderState(),
            CreatedAt = Timestamp.FromDateTime(order.CreatedAt),
            CreatedBy = order.CreatedBy,
        };
    }

    public static AddProductToOrderRequest ToAddProductToOrderRequest(this OrderDto orderDto, long productId, int quantity)
    {
        return new AddProductToOrderRequest
        {
            CreatedAt = orderDto.CreatedAt.ToTimestamp(),
            CreatedBy = orderDto.CreatedBy,
            OrderId = orderDto.OrderId,
            OrderState = orderDto.State.ToGrpcOrderState(),
            ProductId = productId,
            Quantity = quantity,
        };
    }

    public static DeleteProductFromOrderRequest ToDeleteProductFromOrderRequest(this OrderDto orderDto, long productId)
    {
        return new DeleteProductFromOrderRequest
        {
            CreatedAt = orderDto.CreatedAt.ToTimestamp(),
            CreatedBy = orderDto.CreatedBy,
            OrderId = orderDto.OrderId,
            OrderState = orderDto.State.ToGrpcOrderState(),
            ProductId = productId,
        };
    }

    public static ChangeOrderStateToProcessingRequest ToChangeOrderStateToProcessingRequest(this OrderDto orderDto)
    {
        return new ChangeOrderStateToProcessingRequest
        {
            CreatedAt = orderDto.CreatedAt.ToTimestamp(),
            CreatedBy = orderDto.CreatedBy,
            OrderId = orderDto.OrderId,
            OrderState = orderDto.State.ToGrpcOrderState(),
        };
    }

    public static CompleteOrderRequest ToCompleteOrderRequest(this OrderDto orderDto)
    {
        return new CompleteOrderRequest
        {
            CreatedAt = orderDto.CreatedAt.ToTimestamp(),
            CreatedBy = orderDto.CreatedBy,
            OrderId = orderDto.OrderId,
            OrderState = orderDto.State.ToGrpcOrderState(),
        };
    }

    public static CancelOrderRequest ToCancelOrderRequest(this OrderDto orderDto)
    {
        return new CancelOrderRequest
        {
            CreatedAt = orderDto.CreatedAt.ToTimestamp(),
            CreatedBy = orderDto.CreatedBy,
            OrderId = orderDto.OrderId,
            OrderState = orderDto.State.ToGrpcOrderState(),
        };
    }

    public static CreateOrderResponse ToCreateOrderResponse(this OrderDto orderDto)
    {
        return new CreateOrderResponse
        {
            OrderState = orderDto.State.ToGrpcOrderState(),
            CreatedAt = Timestamp.FromDateTime(orderDto.CreatedAt),
            CreatedBy = orderDto.CreatedBy,
            OrderId = orderDto.OrderId,
        };
    }

    public static GetOrderByIdResponse ToGetOrderByIdResponse(this OrderDto orderDto)
    {
        return new GetOrderByIdResponse
        {
            OrderState = orderDto.State.ToGrpcOrderState(),
            CreatedAt = Timestamp.FromDateTime(orderDto.CreatedAt),
            CreatedBy = orderDto.CreatedBy,
            OrderId = orderDto.OrderId,
        };
    }
}