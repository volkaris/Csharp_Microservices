using Grpc.Core;
using Itmo.Csharp.Microservices.Lab2.Task3.Entities.OrderItems;
using Itmo.Csharp.Microservices.Lab2.Task3.Entities.Orders;
using Itmo.Csharp.Microservices.Lab2.Task3.Entities.OrdersHistory;
using Itmo.Csharp.Microservices.Lab2.Task3.Entities.Queries;
using Itmo.Csharp.Microservices.Lab4.Mappers.FromDomainToGrpc;
using Itmo.Csharp.Microservices.Lab4.Mappers.FromGrpcToDomain;
using Microsoft.AspNetCore.Mvc;
using orderService;
using Swashbuckle.AspNetCore.Annotations;
using System.Runtime.CompilerServices;

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

    [HttpPost("addProductToOrder")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns when product added to order successfully")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Returns if order or product wasn't found")]
    public async Task<ActionResult<OrderItemDto>> AddProductToOrder(
        OrderDto order,
        long productId,
        int quantity,
        CancellationToken cancellationToken)
    {
        AddProductToOrderResponse res = await _client.AddProductToOrderAsync(order.ToAddProductToOrderRequest(productId, quantity), cancellationToken: cancellationToken);

        return Ok(res.ToOrderItemDto());
    }

    [HttpDelete("deleteProductFromOrder")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Returns if product successfully soft deleted from order")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Returns if order or product wasn't found")]
    public async Task DeleteProductFromOrder(
        OrderDto order,
        long productId,
        CancellationToken cancellationToken)
    {
        await _client.DeleteProductFromOrderAsync(order.ToDeleteProductFromOrderRequest(productId), cancellationToken: cancellationToken);
    }

    [HttpPatch("toProcessing")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns when order state changed to processing successfully")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Returns if order wasn't found")]
    public async Task ChangeOrderStateToProcessing(
        OrderDto order,
        CancellationToken cancellationToken)
    {
        await _client.ChangeOrderStateToProcessingAsync(order.ToChangeOrderStateToProcessingRequest(), cancellationToken: cancellationToken);
    }

    [HttpPatch("toCompleted")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns when order state changed to completed successfully")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Returns if order wasn't found")]
    public async Task CompleteOrder(
        OrderDto order,
        CancellationToken cancellationToken)
    {
        await _client.CompleteOrderAsync(order.ToCompleteOrderRequest(), cancellationToken: cancellationToken);
    }

    [HttpPatch("toCancelled")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns when order state changed to cancelled successfully")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Returns if order wasn't found")]
    public async Task CancelOrder(
        OrderDto order,
        CancellationToken cancellationToken)
    {
        await _client.CancelOrderAsync(order.ToCancelOrderRequest(), cancellationToken: cancellationToken);
    }

    [HttpGet("searchProductHistory")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns when searching is successful")]
    public async IAsyncEnumerable<OrderHistory> SearchProductHistoryAsync(
        [FromQuery] OrderHistoryQueryForSingleProduct query,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        AsyncServerStreamingCall<SearchProductHistoryResponse> res =
            _client.SearchProductHistoryAsync(query.ToSearchProductHistoryRequest(), cancellationToken: cancellationToken);

        await foreach (SearchProductHistoryResponse response in res.ResponseStream.ReadAllAsync(cancellationToken))
        {
            yield return response.ToOrderHistory();
        }
    }

    [HttpGet]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns if order was found")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Returns if order wasn't found")]
    public async Task<ActionResult<OrderDto>> GetOrderByIdAsync(long orderId, CancellationToken cancellationToken)
    {
        GetOrderByIdResponse res = await _client.GetOrderByIdAsync(new GetOrderByIdRequest { OrderId = orderId }, cancellationToken: cancellationToken);

        return Ok(res.ToOrderDto());
    }
}