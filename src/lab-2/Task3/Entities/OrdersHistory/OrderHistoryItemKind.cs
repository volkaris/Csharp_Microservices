namespace Itmo.Csharp.Microservices.Lab2.Task3.Entities.OrdersHistory;

public enum OrderHistoryItemKind
{
    Unspecified,
    Created,
    ItemAdded,
    ItemRemoved,
    StateChanged,
}