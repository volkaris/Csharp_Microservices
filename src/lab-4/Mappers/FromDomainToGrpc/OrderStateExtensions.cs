using Itmo.Csharp.Microservices.Lab2.Task3.Entities.Orders.OrderStates;
using orderService;

namespace Itmo.Csharp.Microservices.Lab4.Mappers.FromDomainToGrpc;

public static class OrderStateExtensions
{
    public static GrpcOrderState ToGrpcOrderState(this OrderState orderState)
    {
        return orderState switch
        {
            OrderState.Unspecified => GrpcOrderState.Unspecified,
            OrderState.Created => GrpcOrderState.Created,
            OrderState.Processing => GrpcOrderState.Processing,
            OrderState.Completed => GrpcOrderState.Completed,
            OrderState.Cancelled => GrpcOrderState.Cancelled,
            _ => throw new ArgumentOutOfRangeException(nameof(orderState), orderState, $"Invalid order state"),
        };
    }
}