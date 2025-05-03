using Orders.ProcessingService.Contracts;
using myRealization = Itmo.Csharp.Microservices.Lab4.OuterServiceUtility.Entities;

namespace Itmo.Csharp.Microservices.Lab4.Mappers.OuterGrpcService.FromDomainToGrpc;

public static class FinishOrderPackingExtensions
{
    public static FinishOrderPackingRequest ToFinishOrderPackingRequest(this myRealization.FinishOrderPacking.FinishOrderPackingRequest request, long orderId)
    {
        return new FinishOrderPackingRequest
        {
            OrderId = orderId,
            FailureReason = request.FailureReason,
            IsSuccessful = request.IsSuccessful,
        };
    }
}