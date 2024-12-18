using Itmo.Csharp.Microservices.Lab2.Task3.Entities.OrderItems;
using Itmo.Csharp.Microservices.Lab2.Task3.Entities.Orders;
using Itmo.Csharp.Microservices.Lab2.Task3.Entities.Orders.OrderStates;
using Itmo.Csharp.Microservices.Lab2.Task3.Entities.OrdersHistory;
using Itmo.Csharp.Microservices.Lab2.Task3.Entities.OrdersHistory.HistoryRecords;
using Itmo.Csharp.Microservices.Lab2.Task3.Entities.Products;
using Itmo.Csharp.Microservices.Lab2.Task3.Entities.Queries;
using Itmo.Csharp.Microservices.Lab2.Task3.Repositories.OrderItemsRepo;
using Itmo.Csharp.Microservices.Lab2.Task3.Repositories.Orders;
using Itmo.Csharp.Microservices.Lab2.Task3.Repositories.OrdersHistory;
using System.Runtime.CompilerServices;
using System.Transactions;

namespace Itmo.Csharp.Microservices.Lab2.Task3.Services.Orders;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderItemsRepository _orderItemsRepository;
    private readonly IOrdersHistoryRepository _historyRepository;

    public OrderService(
        IOrderRepository orderRepository,
        IOrderItemsRepository orderItemsRepository,
        IOrdersHistoryRepository historyRepository)
    {
        _orderRepository = orderRepository;
        _orderItemsRepository = orderItemsRepository;
        _historyRepository = historyRepository;
    }

    public async Task<OrderDto> CreateOrderAsync(Order order, CancellationToken cancellationToken)
    {
        using var transaction = new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions() { IsolationLevel = IsolationLevel.Serializable },
            TransactionScopeAsyncFlowOption.Enabled);

        OrderDto orderDto = await _orderRepository.CreateOrderAsync(order, cancellationToken);

        await _historyRepository.CreateOrderHistoryAsync(
            new OrderHistory(orderDto.OrderId, DateTime.Now, OrderHistoryItemKind.Created, new OrderCreated(orderDto.OrderId, orderDto.CreatedAt)),
            cancellationToken);

        transaction.Complete();

        return orderDto;
    }

    public async Task<OrderItemDto> AddProductToOrder(OrderDto order, ProductDto product, int quantity, CancellationToken cancellationToken)
    {
        using var transaction = new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions() { IsolationLevel = IsolationLevel.Serializable },
            TransactionScopeAsyncFlowOption.Enabled);

        if (order.State is not OrderState.Created)
            throw new InvalidOperationException("Cannot add product to  order that isn't in 'OrderCreated' state.");

        OrderItemDto orderItemDto = await _orderItemsRepository.CreateOrderItemAsync(
            new OrderItem(order.OrderId, product.Id, quantity),
            cancellationToken);

        await _historyRepository.CreateOrderHistoryAsync(
            new OrderHistory(order.OrderId, DateTime.Now, OrderHistoryItemKind.ItemAdded, new ItemAddedToOrder(product.Id, order.OrderId, quantity)),
            cancellationToken);

        transaction.Complete();

        return orderItemDto;
    }

    public async Task DeleteProductFromOrder(OrderDto order, ProductDto product, CancellationToken cancellationToken)
    {
        using var transaction = new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions() { IsolationLevel = IsolationLevel.Serializable },
            TransactionScopeAsyncFlowOption.Enabled);

        if (order.State is not OrderState.Created)
            throw new InvalidOperationException("Cannot add product to  order that isn't in 'OrderCreated' state.");

        await _orderItemsRepository.SoftDeleteAsync(order.OrderId, product.Id, cancellationToken);

        await _historyRepository.CreateOrderHistoryAsync(
            new OrderHistory(order.OrderId, DateTime.Now, OrderHistoryItemKind.ItemRemoved, new ItemRemovedFromOrder(product.Id, order.OrderId)),
            cancellationToken);

        transaction.Complete();
    }

    public async Task ChangeOrderStateToProcessing(OrderDto order, CancellationToken cancellationToken)
    {
        await ChangeStatus(order, OrderState.Processing, cancellationToken);
    }

    public async Task CompleteOrder(OrderDto order, CancellationToken cancellationToken)
    {
        await ChangeStatus(order, OrderState.Completed, cancellationToken);
    }

    public async Task CancelOrder(OrderDto order, CancellationToken cancellationToken)
    {
        await ChangeStatus(order, OrderState.Cancelled, cancellationToken);
    }

    public async IAsyncEnumerable<OrderHistory> SearchProductHistoryAsync(OrderHistoryQueryForSingleProduct query, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        IAsyncEnumerable<OrderHistory> history = _historyRepository.SearchOrderHistoryAsync(
            new OrderHistoryQuery(query.Cursor, query.PageSize, new[] { query.OrderId }, query.HistoryKind),
            cancellationToken);

        await foreach (OrderHistory orderHistory in history)
        {
            yield return orderHistory;
        }
    }

    public async Task<OrderDto?> GetOrderByIdAsync(int orderId, CancellationToken cancellationToken)
    {
        OrderDto? orderDto = await _orderRepository.GetOrderByIdAsync(orderId, cancellationToken);

        return orderDto;
    }

    private async Task ChangeStatus(OrderDto order, OrderState newState, CancellationToken cancellationToken)
    {
        using var transaction = new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions() { IsolationLevel = IsolationLevel.Serializable },
            TransactionScopeAsyncFlowOption.Enabled);

        await _orderRepository.ChangeOrderState(order, newState, cancellationToken);

        await _historyRepository.CreateOrderHistoryAsync(
            new OrderHistory(order.OrderId, DateTime.Now, OrderHistoryItemKind.StateChanged, new OrderStateChange(order.OrderId, order.State, newState)),
            cancellationToken);

        transaction.Complete();
    }
}