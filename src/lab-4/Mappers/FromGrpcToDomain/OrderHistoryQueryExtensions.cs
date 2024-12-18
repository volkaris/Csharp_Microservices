using Itmo.Csharp.Microservices.Lab2.Task3.Entities.Queries;
using Itmo.Csharp.Microservices.Lab4.Mappers.FromDomainToGrpc;
using orderService;

namespace Itmo.Csharp.Microservices.Lab4.Mappers.FromGrpcToDomain;

public static class OrderHistoryQueryExtensions
{
    public static SearchProductHistoryRequest ToSearchProductHistoryRequest(this OrderHistoryQueryForSingleProduct request)
    {
        return new SearchProductHistoryRequest
        {
            Cursor = request.Cursor,
            OrderId = request.OrderId,
            PageSize = request.PageSize,
            OrderHistoryItemKind = request.HistoryKind.ToGrpcOrderHistoryItemKind(),
        };
    }
}