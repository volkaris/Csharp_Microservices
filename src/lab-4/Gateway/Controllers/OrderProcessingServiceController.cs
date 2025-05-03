using Itmo.Csharp.Microservices.Lab4.Mappers.OuterGrpcService.FromDomainToGrpc;
using Microsoft.AspNetCore.Mvc;
using Orders.ProcessingService.Contracts;
using myRealization = Itmo.Csharp.Microservices.Lab4.OuterServiceUtility.Entities;

namespace Itmo.Csharp.Microservices.Lab4.Gateway.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderProcessingServiceController : ControllerBase
{
    private readonly OrderService.OrderServiceClient _outerServiceClient;

    public OrderProcessingServiceController(
        OrderService.OrderServiceClient outerServiceClient)
    {
        _outerServiceClient = outerServiceClient;
    }

    [HttpPatch("{orderId}/approve")]
    public async Task<ActionResult<myRealization.ApproveOrder.ApproveOrderResponse>> ApproveOrderAsync(
        [FromBody] myRealization.ApproveOrder.ApproveOrderRequest request,
        long orderId,
        CancellationToken cancellationToken)
    {
        await _outerServiceClient.ApproveOrderAsync(
            request.ToApproveOrderRequest(orderId),
            cancellationToken: cancellationToken);

        return Ok(new myRealization.ApproveOrder.ApproveOrderResponse());
    }

    [HttpPatch("{orderId}/start/packing")]
    public async Task<ActionResult<myRealization.StartOrderPacking.StartOrderPackingResponse>> StartOrderPackingAsync(
        [FromBody] myRealization.StartOrderPacking.StartOrderPackingRequest request,
        int orderId,
        CancellationToken cancellationToken)
    {
        await _outerServiceClient.StartOrderPackingAsync(
            request.ToStartOrderPackingRequest(orderId),
            cancellationToken: cancellationToken);

        return Ok(new myRealization.StartOrderPacking.StartOrderPackingResponse());
    }

    [HttpPatch("{orderId}/start/delivery")]
    public async Task<ActionResult<myRealization.StartOrderDelivery.StartOrderDeliveryResponse>> StartOrderDeliveryAsync(
        [FromBody] myRealization.StartOrderDelivery.StartOrderDeliveryRequest request,
        int orderId,
        CancellationToken cancellationToken)
    {
        await _outerServiceClient.StartOrderDeliveryAsync(
            request.ToStartOrderDeliveryRequest(orderId),
            cancellationToken: cancellationToken);

        return Ok(new myRealization.StartOrderDelivery.StartOrderDeliveryResponse());
    }

    [HttpPatch("{orderId}/finish/packing")]
    public async Task<ActionResult<myRealization.FinishOrderPacking.FinishOrderPackingResponse>> FinishOrderPackingAsync(
        [FromBody] myRealization.FinishOrderPacking.FinishOrderPackingRequest request,
        int orderId,
        CancellationToken cancellationToken)
    {
        await _outerServiceClient.FinishOrderPackingAsync(
            request.ToFinishOrderPackingRequest(orderId),
            cancellationToken: cancellationToken);

        return Ok(new myRealization.FinishOrderPacking.FinishOrderPackingResponse());
    }

    [HttpPatch("{orderId}/finish/delivery")]
    public async Task<ActionResult<myRealization.FinishOrderDelivery.FinishOrderDeliveryResponse>> FinishOrderDeliveryAsync(
        [FromBody] myRealization.FinishOrderDelivery.FinishOrderDeliveryRequest request,
        int orderId,
        CancellationToken cancellationToken)
    {
        await _outerServiceClient.FinishOrderDeliveryAsync(
            request.ToFinishOrderDeliveryRequest(orderId),
            cancellationToken: cancellationToken);

        return Ok(new myRealization.FinishOrderDelivery.FinishOrderDeliveryResponse());
    }
}