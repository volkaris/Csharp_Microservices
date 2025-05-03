using Orders.ProcessingService.Contracts;
using myRealization = Itmo.Csharp.Microservices.Lab4.OuterServiceUtility.Entities;

namespace Itmo.Csharp.Microservices.Lab4.Mappers.OuterGrpcService.FromDomainToGrpc;

public static class StartOrderPackingExtensions
{
    public static StartOrderPackingRequest ToStartOrderPackingRequest(this myRealization.StartOrderPacking.StartOrderPackingRequest request, long orderId)
    {
        return new StartOrderPackingRequest
        {
            OrderId = orderId,
            PackingBy = request.PackingBy,
        };
    }
}