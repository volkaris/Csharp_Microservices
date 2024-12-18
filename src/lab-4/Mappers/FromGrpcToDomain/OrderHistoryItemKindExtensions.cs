using Itmo.Csharp.Microservices.Lab2.Task3.Entities.OrdersHistory;
using orderService;

namespace Itmo.Csharp.Microservices.Lab4.Mappers.FromGrpcToDomain;

public static class OrderHistoryItemKindExtensions
{
    public static OrderHistoryItemKind ToDomainOrderHistoryItemKind(this GrpcOrderHistoryItemKind grpcOrderHistoryItemKind)
    {
        return grpcOrderHistoryItemKind switch
        {
            GrpcOrderHistoryItemKind.Unspecified => OrderHistoryItemKind.Unspecified,
            GrpcOrderHistoryItemKind.Created => OrderHistoryItemKind.Created,
            GrpcOrderHistoryItemKind.StateChanged => OrderHistoryItemKind.StateChanged,
            GrpcOrderHistoryItemKind.ItemRemoved => OrderHistoryItemKind.ItemRemoved,
            GrpcOrderHistoryItemKind.ItemAdded => OrderHistoryItemKind.ItemAdded,
            _ => throw new ArgumentOutOfRangeException(nameof(grpcOrderHistoryItemKind), grpcOrderHistoryItemKind, null),
        };
    }
}