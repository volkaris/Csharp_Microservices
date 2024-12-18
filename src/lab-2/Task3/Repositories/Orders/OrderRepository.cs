using Itmo.Csharp.Microservices.Lab2.Task3.Entities.Orders;
using Itmo.Csharp.Microservices.Lab2.Task3.Entities.Orders.OrderStates;
using Itmo.Csharp.Microservices.Lab2.Task3.Entities.Queries;
using Npgsql;
using System.Data;
using System.Runtime.CompilerServices;

namespace Itmo.Csharp.Microservices.Lab2.Task3.Repositories.Orders;

public class OrderRepository : IOrderRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public OrderRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public async Task<OrderDto> CreateOrderAsync(Order order, CancellationToken cancellationToken)
    {
        const string sql = """
                           INSERT INTO ORDERS(ORDER_STATE, ORDER_CREATED_AT, ORDER_CREATED_BY) VALUES (:OrderState,:OrderCreatedAt,:OrderCreatedBy)
                           RETURNING ORDER_ID ;
                           """;

        await using NpgsqlConnection connection = await _dataSource.OpenConnectionAsync(cancellationToken);

        await using var command = new NpgsqlCommand(sql, connection);

        command.Parameters.Add(new NpgsqlParameter("OrderState", order.State));
        command.Parameters.Add(new NpgsqlParameter("OrderCreatedAt", order.CreatedAt));
        command.Parameters.Add(new NpgsqlParameter("OrderCreatedBy", order.CreatedBy));

        object? result = await command.ExecuteScalarAsync(cancellationToken);
        long orderId = Convert.ToInt64(result);

        return new OrderDto(order.State, order.CreatedAt, order.CreatedBy, orderId);
    }

    public async Task ChangeOrderState(OrderDto order, OrderState newOrderState, CancellationToken cancellationToken)
    {
        const string sql = """
                            UPDATE ORDERS
                            SET ORDER_STATE=:OrderState
                            WHERE ORDER_ID=:orderId ;
                           """;

        await using NpgsqlConnection connection = await _dataSource.OpenConnectionAsync(cancellationToken);

        await using var command = new NpgsqlCommand(sql, connection);

        command.Parameters.Add(new NpgsqlParameter<OrderState>("OrderState", newOrderState));
        command.Parameters.Add(new NpgsqlParameter("orderId", order.OrderId));

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async IAsyncEnumerable<OrderDto> SearchOrderAsync(
        OrderQuery query,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        const string sql = """
                           SELECT ORDER_ID, ORDER_STATE,ORDER_CREATED_AT,ORDER_CREATED_BY
                           FROM ORDERS
                           WHERE
                               (ORDER_ID > :Cursor)
                               AND (cardinality(:Ids) =0 OR ORDER_ID=any(:Ids))
                               AND (:Author IS NULL OR ORDER_CREATED_BY LIKE :Author)
                               AND (:State IS NULL OR ORDER_STATE=:State)
                           LIMIT :PageSize ;
                           """;

        await using NpgsqlConnection connection = await _dataSource.OpenConnectionAsync(cancellationToken);

        await using var command = new NpgsqlCommand(sql, connection);

        command.Parameters.Add(new NpgsqlParameter("Cursor", query.Cursor));
        command.Parameters.Add(new NpgsqlParameter("Ids", query.Ids));
        command.Parameters.Add(new NpgsqlParameter("Author", query.CreatedBy));
        command.Parameters.Add(new NpgsqlParameter<OrderState>("OrderState", query.OrderState));
        command.Parameters.Add(new NpgsqlParameter("PageSize", query.PageSize));

        await using NpgsqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            yield return new OrderDto(
                await reader.GetFieldValueAsync<OrderState>("order_state", cancellationToken),
                reader.GetDateTime("order_created_at"),
                reader.GetString("order_created_by"),
                reader.GetInt64("order_id"));
        }
    }

    public async Task<OrderDto?> GetOrderByIdAsync(long orderId, CancellationToken cancellationToken)
    {
        const string sql = """
                           SELECT ORDER_ID, ORDER_STATE,ORDER_CREATED_AT,ORDER_CREATED_BY
                           FROM ORDERS
                           WHERE ORDER_ID=:orderId;
                           """;

        await using NpgsqlConnection connection = await _dataSource.OpenConnectionAsync(cancellationToken);

        await using var command = new NpgsqlCommand(sql, connection);

        command.Parameters.Add(new NpgsqlParameter("orderId", orderId));

        await using NpgsqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
             return new OrderDto(
                await reader.GetFieldValueAsync<OrderState>("order_state", cancellationToken),
                reader.GetDateTime("order_created_at"),
                reader.GetString("order_created_by"),
                reader.GetInt64("order_id"));
        }

        return null;
    }
}