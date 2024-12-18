using Itmo.Csharp.Microservices.Lab4.Entities.OrderItems;
using Itmo.Csharp.Microservices.Lab4.Entities.Orders;
using Itmo.Csharp.Microservices.Lab4.Entities.Orders.OrderStates;
using Itmo.Csharp.Microservices.Lab4.Entities.OrdersHistory;
using Itmo.Csharp.Microservices.Lab4.Entities.OrdersHistory.HistoryRecords;
using Itmo.Csharp.Microservices.Lab4.Entities.Queries;
using Itmo.Csharp.Microservices.Lab4.KafkaIntegration.Producers.Services;
using Itmo.Csharp.Microservices.Lab4.OuterServiceUtility.Producer.Messages;
using Itmo.Csharp.Microservices.Lab4.Repositories.OrderItemsRepo;
using Itmo.Csharp.Microservices.Lab4.Repositories.Orders;
using Itmo.Csharp.Microservices.Lab4.Repositories.OrdersHistory;
using Orders.Kafka.Contracts;
using System.Runtime.CompilerServices;
using System.Transactions;
using IsolationLevel = System.Transactions.IsolationLevel;
using Timestamp = Google.Protobuf.WellKnownTypes.Timestamp;

#pragma warning disable SK1200
namespace Itmo.Csharp.Microservices.Lab4.Services.Orders;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderItemsRepository _orderItemsRepository;

    private readonly IOrdersHistoryRepository _historyRepository;

    private readonly IKafkaServiceProducer<OrderCreationKey, OrderCreationValue>? _producer = null;

    public OrderService(
            IOrderRepository orderRepository,
            IOrderItemsRepository orderItemsRepository,
            IOrdersHistoryRepository historyRepository,
            IKafkaServiceProducer<OrderCreationKey, OrderCreationValue>? producer)
        /*IKafkaServiceProducer<OrderCreationKey, OrderCreationValue> producer*/
    {
        _orderRepository = orderRepository;
        _orderItemsRepository = orderItemsRepository;
        _historyRepository = historyRepository;
        _producer = producer;
    }

    public async Task<OrderDto> CreateOrderAsync(Order order, CancellationToken cancellationToken)
    {
        using var transaction = new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted },
            TransactionScopeAsyncFlowOption.Enabled);

        OrderDto orderDto = await _orderRepository.CreateOrderAsync(order, cancellationToken);

        await _historyRepository.CreateOrderHistoryAsync(
            new OrderHistory(orderDto.OrderId, DateTime.Now, OrderHistoryItemKind.Created, new OrderCreate(orderDto.OrderId, orderDto.CreatedAt)),
            cancellationToken);

        await _producer?.ProduceAsync(
            new ProducerMessage<OrderCreationKey, OrderCreationValue>(
                new OrderCreationKey { OrderId = orderDto.OrderId },
                new OrderCreationValue()
                {
                    OrderCreated = new OrderCreationValue.Types.OrderCreated
                    {
                        OrderId = orderDto.OrderId,
                        CreatedAt = Timestamp.FromDateTime(orderDto.CreatedAt),
                    },
                }),
            cancellationToken)!;

        transaction.Complete();

        return orderDto;
    }

    public async Task<OrderItemDto> AddProductToOrderAsync(long orderId, long productId, int quantity, CancellationToken cancellationToken)
    {
        using var transaction = new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted },
            TransactionScopeAsyncFlowOption.Enabled);

        OrderDto orderDto = await _orderRepository.GetOrderByIdAsync(orderId, cancellationToken) ??
                            throw new ArgumentNullException(nameof(orderId), $"order with id {orderId} wasn't found");

        if (orderDto.State is not OrderState.Created)
            throw new InvalidOperationException("Cannot add product to  order that isn't in 'OrderCreate' state.");

        OrderItemDto orderItemDto = await _orderItemsRepository.CreateOrderItemAsync(
            new OrderItem(orderDto.OrderId, productId, quantity),
            cancellationToken);

        await _historyRepository.CreateOrderHistoryAsync(
            new OrderHistory(orderDto.OrderId, DateTime.Now, OrderHistoryItemKind.ItemAdded, new ItemAddedToOrder(productId, orderDto.OrderId, quantity)),
            cancellationToken);

        transaction.Complete();

        return orderItemDto;
    }

    public async Task DeleteProductFromOrderAsync(long orderId, long productId, CancellationToken cancellationToken)
    {
        using var transaction = new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted },
            TransactionScopeAsyncFlowOption.Enabled);

        OrderDto orderDto = await _orderRepository.GetOrderByIdAsync(orderId, cancellationToken) ??
                            throw new ArgumentNullException(nameof(orderId), $"order with id {orderId} wasn't found");

        if (orderDto.State is not OrderState.Created)
            throw new InvalidOperationException("Cannot add product to  order that isn't in 'OrderCreate' state.");

        await _orderItemsRepository.SoftDeleteAsync(orderDto.OrderId, productId, cancellationToken);

        await _historyRepository.CreateOrderHistoryAsync(
            new OrderHistory(orderDto.OrderId, DateTime.Now, OrderHistoryItemKind.ItemRemoved, new ItemRemovedFromOrder(productId, orderDto.OrderId)),
            cancellationToken);

        transaction.Complete();
    }

    public async Task ChangeOrderStateToProcessingAsync(long orderId, string info, CancellationToken cancellationToken)
    {
        using var transaction = new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted },
            TransactionScopeAsyncFlowOption.Enabled);

        OrderDto orderDto = await _orderRepository.GetOrderByIdAsync(orderId, cancellationToken) ??
                            throw new ArgumentNullException(nameof(orderId), $"order with id {orderId} wasn't found");

        await ChangeStatus(orderDto, info, OrderState.Processing, cancellationToken);

        await _producer?.ProduceAsync(
            new ProducerMessage<OrderCreationKey, OrderCreationValue>(
                new OrderCreationKey { OrderId = orderDto.OrderId },
                new OrderCreationValue()
                {
                    OrderProcessingStarted = new OrderCreationValue.Types.OrderProcessingStarted
                    {
                        OrderId = orderId,
                        StartedAt = Timestamp.FromDateTime(DateTime.Now),
                    },
                }),
            cancellationToken)!;

        transaction.Complete();
    }

    public async Task CompleteOrderAsync(long orderId, CancellationToken cancellationToken)
    {
        OrderDto orderDto = await _orderRepository.GetOrderByIdAsync(orderId, cancellationToken) ??
                            throw new ArgumentNullException(nameof(orderId), $"order with id {orderId} wasn't found");

        await ChangeStatus(orderDto, "changed to complete", OrderState.Completed, cancellationToken);
    }

    public async Task CancelOrderAsync(long orderId, CancellationToken cancellationToken)
    {
        OrderDto orderDto = await _orderRepository.GetOrderByIdAsync(orderId, cancellationToken) ??
                            throw new ArgumentNullException(nameof(orderId), $"order with id {orderId} wasn't found");

        if (orderDto.State != OrderState.Created)
        {
            throw new InvalidOperationException($"Only orders in status created could be cancelled. Yours status is {orderDto.State}");
        }

        await ChangeStatus(orderDto, "to cancelled", OrderState.Cancelled, cancellationToken);
    }

    public async IAsyncEnumerable<OrderHistory> SearchProductHistoryAsync(
        long orderId,
        OrderHistoryQueryForSingleProduct query,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        IAsyncEnumerable<OrderHistory> history = _historyRepository.SearchOrderHistoryAsync(
            new OrderHistoryQuery(query.Cursor, query.PageSize, new[] { orderId }, query.HistoryKind),
            cancellationToken);

        await foreach (OrderHistory orderHistory in history)
        {
            yield return orderHistory;
        }
    }

    public async Task<OrderDto?> GetOrderByIdAsync(long orderId, CancellationToken cancellationToken)
    {
        OrderDto? orderDto = await _orderRepository.GetOrderByIdAsync(orderId, cancellationToken);

        return orderDto;
    }

    private async Task ChangeStatus(OrderDto order, string info, OrderState newState, CancellationToken cancellationToken)
    {
        using var transaction = new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted },
            TransactionScopeAsyncFlowOption.Enabled);

        await _orderRepository.ChangeOrderState(order, newState, cancellationToken);

        await _historyRepository.CreateOrderHistoryAsync(
            new OrderHistory(order.OrderId, DateTime.Now, OrderHistoryItemKind.StateChanged, new OrderStateChange(order.OrderId, order.State, newState, info)),
            cancellationToken);

        transaction.Complete();
    }
}