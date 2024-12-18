using Google.Protobuf.WellKnownTypes;
using Itmo.Csharp.Microservices.Lab4.Entities.Orders;
using orderService;

namespace Itmo.Csharp.Microservices.Lab4.Mappers.MyGrpcService.FromDomainToGrpc;

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

    public static AddProductToOrderRequest ToAddProductToOrderRequest(int orderId, long productId, int quantity)
    {
        return new AddProductToOrderRequest
        {
            OrderId = orderId,
            ProductId = productId,
            Quantity = quantity,
        };
    }

    public static DeleteProductFromOrderRequest ToDeleteProductFromOrderRequest(int orderId, long productId)
    {
        return new DeleteProductFromOrderRequest
        {
            OrderId = orderId,
            ProductId = productId,
        };
    }

    public static ChangeOrderStateToProcessingRequest ToChangeOrderStateToProcessingRequest(int orderId)
    {
        return new ChangeOrderStateToProcessingRequest
        {
            OrderId = orderId,
        };
    }

    public static CompleteOrderRequest ToCompleteOrderRequest(int orderId)
    {
        return new CompleteOrderRequest
        {
            OrderId = orderId,
        };
    }

    public static CancelOrderRequest ToCancelOrderRequest(int orderId)
    {
        return new CancelOrderRequest
        {
            OrderId = orderId,
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