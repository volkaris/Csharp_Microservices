namespace Itmo.Csharp.Microservices.Lab2.Task3.Entities.Orders.OrderStates;

public enum OrderState
{
    Unspecified,
    Created,
    Processing,
    Completed,
    Cancelled,
}