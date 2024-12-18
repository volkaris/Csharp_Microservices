using Grpc.Core;
using Itmo.Csharp.Microservices.Lab4.Entities.OrderItems;
using Itmo.Csharp.Microservices.Lab4.Entities.Orders;
using Itmo.Csharp.Microservices.Lab4.Entities.OrdersHistory;
using Itmo.Csharp.Microservices.Lab4.Mappers.MyGrpcService.FromDomainToGrpc;
using Itmo.Csharp.Microservices.Lab4.Mappers.MyGrpcService.FromGrpcToDomain;
using Itmo.Csharp.Microservices.Lab4.Services.Orders;
using orderService;

namespace Itmo.Csharp.Microservices.Lab4.GrpcService.Services.Orders;

public class OrderGrpcService : OrdersService.OrdersServiceBase
{
    private readonly IOrderService _service;

    public OrderGrpcService(IOrderService service)
    {
        _service = service;
    }

    public override async Task<CreateOrderResponse> CreateOrder(CreateOrderRequest request, ServerCallContext context)
    {
        var order = new Order(request.OrderState.ToDomainOrderState(), request.CreatedAt.ToDateTime(), request.CreatedBy);

        OrderDto res = await _service.CreateOrderAsync(order, context.CancellationToken);

        return res.ToCreateOrderResponse();
    }

    public override async Task<GetOrderByIdResponse> GetOrderById(GetOrderByIdRequest request, ServerCallContext context)
    {
        OrderDto? res = await _service.GetOrderByIdAsync(request.OrderId, context.CancellationToken);

        if (res is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "order not found"));
        }

        return res.ToGetOrderByIdResponse();
    }

    public override async Task<AddProductToOrderResponse> AddProductToOrder(AddProductToOrderRequest request, ServerCallContext context)
    {
        OrderItemDto res = await _service.AddProductToOrderAsync(
            request.OrderId,
            request.ProductId,
            request.Quantity,
            context.CancellationToken);

        return res.ToAddProductToOrderResponse();
    }

    public override async Task<DeleteProductFromOrderResponse> DeleteProductFromOrder(DeleteProductFromOrderRequest request, ServerCallContext context)
    {
        await _service.DeleteProductFromOrderAsync(request.ProductId, request.ProductId, context.CancellationToken);

        return new DeleteProductFromOrderResponse();
    }

    public override async Task<ChangeOrderStateToProcessingResponse> ChangeOrderStateToProcessing(ChangeOrderStateToProcessingRequest request, ServerCallContext context)
    {
        await _service.ChangeOrderStateToProcessingAsync(request.OrderId, "changed to processing", context.CancellationToken);

        return new ChangeOrderStateToProcessingResponse();
    }

    public override async Task<CompleteOrderResponse> CompleteOrder(CompleteOrderRequest request, ServerCallContext context)
    {
        await _service.CompleteOrderAsync(request.OrderId, context.CancellationToken);

        return new CompleteOrderResponse();
    }

    public override async Task<CancelOrderResponse> CancelOrder(CancelOrderRequest request, ServerCallContext context)
    {
        await _service.CancelOrderAsync(request.OrderId, context.CancellationToken);

        return new CancelOrderResponse();
    }

    public override async Task SearchProductHistoryAsync(
        SearchProductHistoryRequest request,
        IServerStreamWriter<SearchProductHistoryResponse> responseStream,
        ServerCallContext context)
    {
        await foreach (OrderHistory res in _service.SearchProductHistoryAsync(request.OrderId, request.ToOrderHistoryQueryForSingleProduct(), context.CancellationToken))
        {
            await responseStream.WriteAsync(res.ToSearchProductHistoryResponse(), context.CancellationToken);
        }
    }
}