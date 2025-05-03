using Orders.ProcessingService.Contracts;
using myRealization = Itmo.Csharp.Microservices.Lab4.OuterServiceUtility.Entities;

namespace Itmo.Csharp.Microservices.Lab4.Mappers.OuterGrpcService.FromDomainToGrpc;

public static class ApproveOrderExtensions
{
    public static ApproveOrderRequest ToApproveOrderRequest(this myRealization.ApproveOrder.ApproveOrderRequest request, long orderId)
    {
        return new ApproveOrderRequest
        {
            ApprovedBy = request.ApprovedBy,
            FailureReason = request.FailureReason,
            IsApproved = request.IsApproved,
            OrderId = orderId,
        };
    }
}