namespace Itmo.Csharp.Microservices.Lab4.Entities.Orders.OrderStates;

public enum OrderState
{
    Unspecified,
    Created,
    Processing,
    Completed,
    Cancelled,
}