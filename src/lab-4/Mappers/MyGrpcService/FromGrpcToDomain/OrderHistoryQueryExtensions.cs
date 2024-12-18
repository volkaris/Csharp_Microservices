using Itmo.Csharp.Microservices.Lab4.Entities.Queries;
using Itmo.Csharp.Microservices.Lab4.Mappers.MyGrpcService.FromDomainToGrpc;
using orderService;

namespace Itmo.Csharp.Microservices.Lab4.Mappers.MyGrpcService.FromGrpcToDomain;

public static class OrderHistoryQueryExtensions
{
    public static SearchProductHistoryRequest ToSearchProductHistoryRequest(this OrderHistoryQueryForSingleProduct request, long orderId)
    {
        return new SearchProductHistoryRequest
        {
            Cursor = request.Cursor,
            OrderId = orderId,
            PageSize = request.PageSize,
            OrderHistoryItemKind = request.HistoryKind.ToGrpcOrderHistoryItemKind(),
        };
    }
}