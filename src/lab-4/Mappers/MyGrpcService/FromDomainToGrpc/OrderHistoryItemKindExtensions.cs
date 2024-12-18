using Itmo.Csharp.Microservices.Lab4.Entities.OrdersHistory;
using orderService;

namespace Itmo.Csharp.Microservices.Lab4.Mappers.MyGrpcService.FromDomainToGrpc;

public static class OrderHistoryItemKindExtensions
{
    public static GrpcOrderHistoryItemKind ToGrpcOrderHistoryItemKind(this OrderHistoryItemKind itemKind)
    {
        return itemKind switch
        {
            OrderHistoryItemKind.ItemAdded => GrpcOrderHistoryItemKind.ItemAdded,
            OrderHistoryItemKind.Unspecified => GrpcOrderHistoryItemKind.Unspecified,
            OrderHistoryItemKind.Created => GrpcOrderHistoryItemKind.Created,
            OrderHistoryItemKind.ItemRemoved => GrpcOrderHistoryItemKind.ItemRemoved,
            OrderHistoryItemKind.StateChanged => GrpcOrderHistoryItemKind.StateChanged,
            _ => throw new ArgumentOutOfRangeException(nameof(itemKind), itemKind, null),
        };
    }
}