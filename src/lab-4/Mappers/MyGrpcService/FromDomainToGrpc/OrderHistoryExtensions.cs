using Google.Protobuf.WellKnownTypes;
using Itmo.Csharp.Microservices.Lab4.Entities.OrdersHistory;
using Itmo.Csharp.Microservices.Lab4.Entities.OrdersHistory.HistoryRecords;
using orderService;
using ItemAddedToOrder = Itmo.Csharp.Microservices.Lab4.Entities.OrdersHistory.HistoryRecords.ItemAddedToOrder;
using ItemRemovedFromOrder = Itmo.Csharp.Microservices.Lab4.Entities.OrdersHistory.HistoryRecords.ItemRemovedFromOrder;
using OrderStateChange = Itmo.Csharp.Microservices.Lab4.Entities.OrdersHistory.HistoryRecords.OrderStateChange;

namespace Itmo.Csharp.Microservices.Lab4.Mappers.MyGrpcService.FromDomainToGrpc;

public static class OrderHistoryExtensions
{
    public static SearchProductHistoryResponse ToSearchProductHistoryResponse(this OrderHistory history)
    {
        var response = new SearchProductHistoryResponse
        {
            OrderId = history.OrderId,
            CreatedAt = history.CreatedAt.ToTimestamp(),
            OrderHistoryItemKind = history.Kind.ToGrpcOrderHistoryItemKind(),
        };

        switch (history.ItemPayload)
        {
            case OrderCreate orderCreated:
                response.OrderCreated = new orderService.OrderCreated
                {
                    CreatedAt = orderCreated.CreationDate.ToTimestamp(),
                    OrderId = orderCreated.OrderId,
                };
                break;

            case ItemAddedToOrder itemAddedToOrder:
                response.ItemAddedToOrder = new orderService.ItemAddedToOrder
                {
                    OrderId = itemAddedToOrder.OrderId,
                    ProductId = itemAddedToOrder.ProductId,
                    Quantity = itemAddedToOrder.Quantity,
                };
                break;

            case ItemRemovedFromOrder itemRemovedFromOrder:
                response.ItemRemovedFromOrder = new orderService.ItemRemovedFromOrder
                {
                    OrderId = itemRemovedFromOrder.OrderId,
                    ProductId = itemRemovedFromOrder.ProductId,
                };
                break;

            case OrderStateChange orderStateChange:
                response.OrderStateChange = new orderService.OrderStateChange()
                {
                    OrderId = orderStateChange.OrderId,
                    PreviousState = orderStateChange.PreviousState.ToGrpcOrderState(),
                    NewState = orderStateChange.NewState.ToGrpcOrderState(),
                };
                break;
        }

        return response;
    }
}