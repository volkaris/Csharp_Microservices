using Itmo.Csharp.Microservices.Lab2.Task3.Entities.Orders.OrderStates;
using orderService;

namespace Itmo.Csharp.Microservices.Lab4.Mappers.FromGrpcToDomain;

public static class OrderStateExtensions
{
    public static OrderState ToDomainOrderState(this GrpcOrderState orderState)
    {
        return orderState switch
        {
            GrpcOrderState.Unspecified => OrderState.Unspecified,
            GrpcOrderState.Created => OrderState.Created,
            GrpcOrderState.Processing => OrderState.Processing,
            GrpcOrderState.Completed => OrderState.Completed,
            GrpcOrderState.Cancelled => OrderState.Cancelled,
            _ => throw new ArgumentOutOfRangeException(nameof(orderState), orderState, $"Invalid order state"),
        };
    }
}