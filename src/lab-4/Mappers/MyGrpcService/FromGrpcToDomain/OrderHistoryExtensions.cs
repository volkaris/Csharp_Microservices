using Itmo.Csharp.Microservices.Lab4.Entities.OrdersHistory;
using Itmo.Csharp.Microservices.Lab4.Entities.OrdersHistory.HistoryRecords;
using Itmo.Csharp.Microservices.Lab4.Entities.Queries;
using orderService;
using ItemAddedToOrder = Itmo.Csharp.Microservices.Lab4.Entities.OrdersHistory.HistoryRecords.ItemAddedToOrder;
using ItemRemovedFromOrder = Itmo.Csharp.Microservices.Lab4.Entities.OrdersHistory.HistoryRecords.ItemRemovedFromOrder;
using OrderStateChange = Itmo.Csharp.Microservices.Lab4.Entities.OrdersHistory.HistoryRecords.OrderStateChange;

namespace Itmo.Csharp.Microservices.Lab4.Mappers.MyGrpcService.FromGrpcToDomain;

public static class OrderHistoryExtensions
{
    public static OrderHistory ToOrderHistory(this SearchProductHistoryResponse response)
    {
        var res = new OrderHistory(
            response.OrderId,
            response.CreatedAt.ToDateTime(),
            response.OrderHistoryItemKind.ToDomainOrderHistoryItemKind())
        {
            ItemPayload = response.OrderHistoryTypeCase switch
            {
                SearchProductHistoryResponse.OrderHistoryTypeOneofCase.OrderCreated => new OrderCreate(
                    response.OrderCreated.OrderId,
                    response.OrderCreated.CreatedAt.ToDateTime()),

                SearchProductHistoryResponse.OrderHistoryTypeOneofCase.OrderStateChange => new OrderStateChange(
                    response.OrderStateChange.OrderId,
                    /*"amogus",*/
                    response.OrderStateChange.PreviousState.ToDomainOrderState(),
                    response.OrderStateChange.NewState.ToDomainOrderState()),

                SearchProductHistoryResponse.OrderHistoryTypeOneofCase.ItemAddedToOrder => new ItemAddedToOrder(
                    response.ItemAddedToOrder.ProductId,
                    response.ItemAddedToOrder.OrderId,
                    response.ItemAddedToOrder.Quantity),

                SearchProductHistoryResponse.OrderHistoryTypeOneofCase.ItemRemovedFromOrder => new ItemRemovedFromOrder(
                    response.ItemRemovedFromOrder.ProductId,
                    response.ItemRemovedFromOrder.OrderId),

                SearchProductHistoryResponse.OrderHistoryTypeOneofCase.None => throw new ArgumentNullException(nameof(response)),
                _ => throw new ArgumentNullException(nameof(response)),
            },
        };

        return res;
    }

    public static OrderHistoryQueryForSingleProduct ToOrderHistoryQueryForSingleProduct(this SearchProductHistoryRequest request)
    {
        return new OrderHistoryQueryForSingleProduct(
            request.Cursor,
            request.PageSize,
            request.OrderHistoryItemKind.ToDomainOrderHistoryItemKind());
    }
}