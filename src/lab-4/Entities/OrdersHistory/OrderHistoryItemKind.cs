namespace Itmo.Csharp.Microservices.Lab4.Entities.OrdersHistory;

public enum OrderHistoryItemKind
{
    Unspecified,
    Created,
    ItemAdded,
    ItemRemoved,
    StateChanged,
}