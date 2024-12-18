using Itmo.Csharp.Microservices.Lab4.Entities.OrdersHistory.HistoryRecords;

namespace Itmo.Csharp.Microservices.Lab4.Entities.OrdersHistory;

public record OrderHistory
{
    public OrderHistory(long orderId, DateTime createdAt, OrderHistoryItemKind kind, OrderHistoryBaseType? itemPayload)
    {
        OrderId = orderId;
        CreatedAt = createdAt;
        Kind = kind;
        ItemPayload = itemPayload;
    }

    public OrderHistory(long orderId, DateTime createdAt, OrderHistoryItemKind kind)
    {
        OrderId = orderId;
        CreatedAt = createdAt;
        Kind = kind;
    }

    public long OrderId { get; init; }

    public DateTime CreatedAt { get; init; }

    public OrderHistoryItemKind Kind { get; init; }

    public OrderHistoryBaseType? ItemPayload { get; set; }
}