using Itmo.Csharp.Microservices.Lab4.Entities.OrderItems;
using Itmo.Csharp.Microservices.Lab4.Entities.Queries;
using Npgsql;
using System.Data;
using System.Runtime.CompilerServices;

namespace Itmo.Csharp.Microservices.Lab4.Repositories.OrderItemsRepo;

public class OrderItemsRepository : IOrderItemsRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public OrderItemsRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public async Task<OrderItemDto> CreateOrderItemAsync(OrderItem orderItem, CancellationToken cancellationToken)
    {
        const string sql = """
                            INSERT INTO ORDER_ITEMS(ORDER_ID, PRODUCT_ID, ORDER_ITEM_QUANTITY, ORDER_ITEM_DELETED)
                            VALUES (:OrderId,:ProductId,:Quantity,:Deleted)
                            RETURNING ORDER_ITEM_ID ;
                           """;

        await using NpgsqlConnection connection = await _dataSource.OpenConnectionAsync(cancellationToken);

        await using var command = new NpgsqlCommand(sql, connection);

        command.Parameters.Add(new NpgsqlParameter("OrderId", orderItem.OrderId));
        command.Parameters.Add(new NpgsqlParameter("ProductId", orderItem.ProductId));
        command.Parameters.Add(new NpgsqlParameter("Quantity", orderItem.Quantity));
        command.Parameters.Add(new NpgsqlParameter("Deleted", orderItem.Deleted));

        object? result = await command.ExecuteScalarAsync(cancellationToken);
        long id = Convert.ToInt64(result);

        return new OrderItemDto(orderItem.OrderId, orderItem.ProductId, orderItem.Quantity, id, orderItem.Deleted);
    }

    public async Task SoftDeleteAsync(long orderId, long productId, CancellationToken cancellationToken)
    {
        const string sql = """
                            UPDATE ORDER_ITEMS
                            SET ORDER_ITEM_DELETED = TRUE
                            WHERE ORDER_ID=:orderId AND PRODUCT_ID=:productId ;
                           """;

        await using NpgsqlConnection connection = await _dataSource.OpenConnectionAsync(cancellationToken);

        await using var command = new NpgsqlCommand(sql, connection);

        command.Parameters.Add(new NpgsqlParameter("orderId", orderId));
        command.Parameters.Add(new NpgsqlParameter("productId", productId));

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async IAsyncEnumerable<OrderItemDto> SearchOrderItemAsync(
        OrderItemQuery query,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        const string sql = """
                           SELECT ORDER_ITEM_ID,ORDER_ID,PRODUCT_ID,ORDER_ITEM_QUANTITY,ORDER_ITEM_DELETED
                           FROM ORDER_ITEMS
                           WHERE
                               (ORDER_ITEM_ID> :Cursor)
                               AND (cardinality(:OrderIds) =0 OR ORDER_ID=ANY(:OrderIds))
                               AND (cardinality(:ProductIds) =0 OR PRODUCT_ID=ANY(:ProductIds))
                               AND (:IsDeleted IS NULL OR ORDER_ITEM_DELETED=:IsDeleted)
                           LIMIT :PageSize ;
                           """;

        await using NpgsqlConnection connection = await _dataSource.OpenConnectionAsync(cancellationToken);

        await using var command = new NpgsqlCommand(sql, connection);

        command.Parameters.Add(new NpgsqlParameter("Cursor", query.Cursor));
        command.Parameters.Add(new NpgsqlParameter("OrderIds", query.OrderIds));
        command.Parameters.Add(new NpgsqlParameter("ProductIds", query.ProductIds));
        command.Parameters.Add(new NpgsqlParameter("IsDeleted", query.IsDeleted));
        command.Parameters.Add(new NpgsqlParameter("PageSize", query.PageSize));

        await using NpgsqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            yield return new OrderItemDto(
                reader.GetInt32("order_id"),
                reader.GetInt32("product_id"),
                reader.GetInt32("order_item_quantity"),
                reader.GetInt64("order_item_id"),
                reader.GetBoolean("order_item_deleted"));
        }
    }
}