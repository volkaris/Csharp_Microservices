using Orders.ProcessingService.Contracts;
using myRealization = Itmo.Csharp.Microservices.Lab4.OuterServiceUtility.Entities;

namespace Itmo.Csharp.Microservices.Lab4.Mappers.OuterGrpcService.FromDomainToGrpc;

public static class StartOrderDeliveryExtensions
{
    public static StartOrderDeliveryRequest ToStartOrderDeliveryRequest(this myRealization.StartOrderDelivery.StartOrderDeliveryRequest request, long orderId)
    {
        return new StartOrderDeliveryRequest
        {
            OrderId = orderId,
            DeliveredBy = request.DeliveredBy,
        };
    }
}