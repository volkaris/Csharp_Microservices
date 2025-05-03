using Orders.ProcessingService.Contracts;
using myRealization = Itmo.Csharp.Microservices.Lab4.OuterServiceUtility.Entities;

namespace Itmo.Csharp.Microservices.Lab4.Mappers.OuterGrpcService.FromDomainToGrpc;

public static class FinishOrderDeliveryExtensions
{
    public static FinishOrderDeliveryRequest ToFinishOrderDeliveryRequest(this myRealization.FinishOrderDelivery.FinishOrderDeliveryRequest request, long orderId)
    {
        return new FinishOrderDeliveryRequest
        {
            OrderId = orderId,
            FailureReason = request.FailureReason,
            IsSuccessful = request.IsSuccessful,
        };
    }
}