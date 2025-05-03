using Itmo.Csharp.Microservices.Lab2.Task3.Entities.OrderItems;
using Itmo.Csharp.Microservices.Lab2.Task3.Entities.Orders;
using Itmo.Csharp.Microservices.Lab2.Task3.Entities.OrdersHistory;
using Itmo.Csharp.Microservices.Lab2.Task3.Entities.Products;
using Itmo.Csharp.Microservices.Lab2.Task3.Entities.Queries;
using Itmo.Csharp.Microservices.Lab2.Task3.Services.Orders;
using Itmo.Csharp.Microservices.Lab2.Task3.Services.Products;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Itmo.Csharp.Microservices.Lab3.Controllers.Orders;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IProductService _productService;

    public OrderController(IOrderService orderService, IProductService productService)
    {
        _orderService = orderService;
        _productService = productService;
    }

    [SwaggerResponse(StatusCodes.Status200OK, "Returns when order was successfully created")]
    [HttpPost]
    public async Task<ActionResult<OrderDto>> CreateOrderAsync(
        [FromBody] Order order,
        CancellationToken cancellationToken)
    {
        OrderDto orderDto = await _orderService.CreateOrderAsync(order, cancellationToken);

        return Ok(orderDto);
    }

    [SwaggerResponse(StatusCodes.Status200OK, "Returns when product added to order successfully")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Returns if order or product wasn't found")]
    [HttpPost("addProductToOrder")]
    public async Task<ActionResult<OrderItemDto>> AddProductToOrder(
        [FromQuery] int orderId,
        [FromQuery] int productId,
        [FromQuery] int quantity,
        CancellationToken cancellationToken)
    {
        OrderDto? orderDto = await _orderService.GetOrderByIdAsync(orderId, cancellationToken);

        if (orderDto is null)
        {
            return NotFound($"Order with id {orderId} wasn't found");
        }

        ProductDto? productDto = await _productService.GetProductByIdAsync(productId, cancellationToken);

        if (productDto is null)
        {
            return NotFound($"Product with id {productId} wasn't found");
        }

        OrderItemDto orderItemDto = await _orderService.AddProductToOrderAsync(orderDto, productDto.Id, quantity, cancellationToken);

        return Ok(orderItemDto);
    }

    [SwaggerResponse(StatusCodes.Status204NoContent, "Returns if product successfully soft deleted from order")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Returns if order or product wasn't found")]
    [HttpDelete("deleteProductFromOrder")]
    public async Task<ActionResult> DeleteProductFromOrder(
        [FromQuery] int orderId,
        [FromQuery] int productId,
        CancellationToken cancellationToken)
    {
        OrderDto? orderDto = await _orderService.GetOrderByIdAsync(orderId, cancellationToken);

        if (orderDto is null)
        {
            return NotFound($"Order with id {orderId} wasn't found");
        }

        ProductDto? productDto = await _productService.GetProductByIdAsync(productId, cancellationToken);

        if (productDto is null)
        {
            return NotFound($"Product with id {productId} wasn't found");
        }

        await _orderService.DeleteProductFromOrderAsync(orderDto, productDto.Id, cancellationToken);

        return NoContent();
    }

    [SwaggerResponse(StatusCodes.Status200OK, "Returns when order state changed to processing successfully")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Returns if order wasn't found")]
    [HttpPatch("toProcessing")]
    public async Task<ActionResult> ChangeOrderStateToProcessing(
        [FromQuery] int orderId,
        CancellationToken cancellationToken)
    {
        OrderDto? orderDto = await _orderService.GetOrderByIdAsync(orderId, cancellationToken);

        if (orderDto is null)
        {
            return NotFound($"Order with id {orderId} wasn't found");
        }

        await _orderService.ChangeOrderStateToProcessingAsync(orderDto, cancellationToken);

        return Ok();
    }

    [SwaggerResponse(StatusCodes.Status200OK, "Returns when order state changed to completed successfully")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Returns if order wasn't found")]
    [HttpPatch("toCompleted")]
    public async Task<ActionResult> CompleteOrder(
        [FromQuery] int orderId,
        CancellationToken cancellationToken)
    {
        OrderDto? orderDto = await _orderService.GetOrderByIdAsync(orderId, cancellationToken);

        if (orderDto is null)
        {
            return NotFound($"Order with id {orderId} wasn't found");
        }

        await _orderService.CompleteOrderAsync(orderDto, cancellationToken);

        return Ok();
    }

    [SwaggerResponse(StatusCodes.Status200OK, "Returns when order state changed to cancelled successfully")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Returns if order wasn't found")]
    [HttpPatch("toCancelled")]
    public async Task<ActionResult> CancelOrder(
        [FromQuery] int orderId,
        CancellationToken cancellationToken)
    {
        OrderDto? orderDto = await _orderService.GetOrderByIdAsync(orderId, cancellationToken);

        if (orderDto is null)
        {
            return NotFound($"Order with id {orderId} wasn't found");
        }

        await _orderService.CancelOrderAsync(orderDto, cancellationToken);

        return Ok();
    }

    [HttpGet("searchProductHistory")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns when searching is successful")]
    public ActionResult<IAsyncEnumerable<OrderHistory>> SearchProductHistoryAsync(
        [FromQuery] OrderHistoryQueryForSingleProduct query,
        CancellationToken cancellationToken)
    {
        return Ok(_orderService.SearchProductHistoryAsync(query, cancellationToken));
    }

    [SwaggerResponse(StatusCodes.Status200OK, "Returns if order was found")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Returns if order wasn't found")]
    [HttpGet]
    public async Task<ActionResult<OrderDto>> GetByIdAsync(
        [FromQuery] int orderId,
        CancellationToken cancellationToken)
    {
        OrderDto? orderDto = await _orderService.GetOrderByIdAsync(orderId, cancellationToken);

        if (orderDto is null)
        {
            return NotFound($"Order with id {orderId} wasn't found");
        }

        return Ok(orderDto);
    }
}