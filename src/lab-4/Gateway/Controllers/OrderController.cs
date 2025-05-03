using Grpc.Core;
using Itmo.Csharp.Microservices.Lab4.Entities.OrderItems;
using Itmo.Csharp.Microservices.Lab4.Entities.Orders;
using Itmo.Csharp.Microservices.Lab4.Entities.OrdersHistory;
using Itmo.Csharp.Microservices.Lab4.Entities.Queries;
using Itmo.Csharp.Microservices.Lab4.Mappers.MyGrpcService.FromDomainToGrpc;
using Itmo.Csharp.Microservices.Lab4.Mappers.MyGrpcService.FromGrpcToDomain;
using Microsoft.AspNetCore.Mvc;
using orderService;
using Swashbuckle.AspNetCore.Annotations;
using System.Runtime.CompilerServices;
using OrderExtension = Itmo.Csharp.Microservices.Lab4.Mappers.MyGrpcService.FromDomainToGrpc.OrderExtension;

namespace Itmo.Csharp.Microservices.Lab4.Gateway.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly OrdersService.OrdersServiceClient _client;

    public OrderController(OrdersService.OrdersServiceClient client)
    {
        _client = client;
    }

    [HttpPost]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns when order was successfully created")]
    public async Task<ActionResult<OrderDto>> CreateOrderAsync(
        [FromBody] Order order,
        CancellationToken cancellationToken)
    {
        CreateOrderResponse res = await _client.CreateOrderAsync(order.ToCreateOrderRequest(), cancellationToken: cancellationToken);

        return Ok(res.ToOrderDto());
    }

    [HttpPost("{orderId}/product/{productId}/{quantity}")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns when product added to order successfully")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Returns if order or product wasn't found")]
    public async Task<ActionResult<OrderItemDto>> AddProductToOrder(
        long productId,
        int orderId,
        int quantity,
        CancellationToken cancellationToken)
    {
        AddProductToOrderResponse res =
            await _client.AddProductToOrderAsync(OrderExtension.ToAddProductToOrderRequest(orderId, productId, quantity), cancellationToken: cancellationToken);

        return Ok(res.ToOrderItemDto());
    }

    [HttpDelete("{orderId}/product/{productId}")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Returns if product successfully soft deleted from order")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Returns if order or product wasn't found")]
    public async Task DeleteProductFromOrder(
        int orderId,
        long productId,
        CancellationToken cancellationToken)
    {
        await _client.DeleteProductFromOrderAsync(OrderExtension.ToDeleteProductFromOrderRequest(orderId, productId), cancellationToken: cancellationToken);
    }

    [HttpPatch("{orderId}/toProcessing")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns when order state changed to processing successfully")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Returns if order wasn't found")]
    public async Task ChangeOrderStateToProcessing(
        int orderId,
        CancellationToken cancellationToken)
    {
        await _client.ChangeOrderStateToProcessingAsync(OrderExtension.ToChangeOrderStateToProcessingRequest(orderId), cancellationToken: cancellationToken);
    }

    [HttpPatch("{orderId}/toCancelled")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns when order state changed to cancelled successfully")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Returns if order wasn't found")]
    public async Task CancelOrder(
        int orderId,
        CancellationToken cancellationToken)
    {
        await _client.CancelOrderAsync(OrderExtension.ToCancelOrderRequest(orderId), cancellationToken: cancellationToken);
    }

    [HttpGet("{orderId}/product/history")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns when searching is successful")]
    public async IAsyncEnumerable<OrderHistory> SearchProductHistoryAsync(
        long orderId,
        [FromQuery] OrderHistoryQueryForSingleProduct query,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        AsyncServerStreamingCall<SearchProductHistoryResponse> res =
            _client.SearchProductHistoryAsync(query.ToSearchProductHistoryRequest(orderId), cancellationToken: cancellationToken);

        await foreach (SearchProductHistoryResponse response in res.ResponseStream.ReadAllAsync(cancellationToken))
        {
            yield return response.ToOrderHistory();
        }
    }

    [HttpGet("{orderId}")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns if order was found")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Returns if order wasn't found")]
    public async Task<ActionResult<OrderDto>> GetOrderByIdAsync(long orderId, CancellationToken cancellationToken)
    {
        GetOrderByIdResponse res = await _client.GetOrderByIdAsync(new GetOrderByIdRequest { OrderId = orderId }, cancellationToken: cancellationToken);

        return Ok(res.ToOrderDto());
    }
}