using Itmo.Csharp.Microservices.Lab2.Task3.Entities.OrdersHistory;
using Itmo.Csharp.Microservices.Lab2.Task3.Entities.OrdersHistory.HistoryRecords;
using Itmo.Csharp.Microservices.Lab2.Task3.Entities.Queries;
using Npgsql;
using NpgsqlTypes;
using System.Data;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace Itmo.Csharp.Microservices.Lab2.Task3.Repositories.OrdersHistory;

public class OrdersHistoryRepository : IOrdersHistoryRepository
{
    private readonly NpgsqlDataSource _source;

    public OrdersHistoryRepository(NpgsqlDataSource source)
    {
        _source = source;
    }

    public async Task CreateOrderHistoryAsync(OrderHistory orderHistory, CancellationToken cancellationToken)
    {
        const string sql = """
                           INSERT INTO ORDER_HISTORY(ORDER_ID, ORDER_HISTORY_ITEM_CREATED_AT, ORDER_HISTORY_ITEM_KIND, ORDER_HISTORY_ITEM_PAYLOAD)
                           VALUES (:orderId, :orderHistoryItemCreatedAt, :orderHistoryItemKind, :orderHistoryItemPayload);
                           """;

        await using NpgsqlConnection connection = await _source.OpenConnectionAsync(cancellationToken);

        await using var command = new NpgsqlCommand(sql, connection);

        command.Parameters.Add(new NpgsqlParameter("orderId", orderHistory.OrderId));
        command.Parameters.Add(new NpgsqlParameter("orderHistoryItemCreatedAt", orderHistory.CreatedAt));
        command.Parameters.Add(new NpgsqlParameter<OrderHistoryItemKind?>("orderHistoryItemKind", orderHistory.Kind));

        string json = JsonSerializer.Serialize(orderHistory.ItemPayload);

        command.Parameters.AddWithValue("orderHistoryItemPayload", NpgsqlDbType.Jsonb, json);

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async IAsyncEnumerable<OrderHistory> SearchOrderHistoryAsync(
        OrderHistoryQuery query,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        const string sql = """
                           SELECT ORDER_ID, ORDER_HISTORY_ITEM_CREATED_AT,ORDER_HISTORY_ITEM_KIND,ORDER_HISTORY_ITEM_PAYLOAD,ORDER_HISTORY_ITEM_ID
                           FROM ORDER_HISTORY
                           WHERE
                               (ORDER_HISTORY_ITEM_ID > :Cursor)
                               AND (cardinality(:Ids) = 0 OR ORDER_ID=ANY(:Ids))
                               AND (:HistoryKind IS NULL OR ORDER_HISTORY_ITEM_KIND=:HistoryKind)
                           LIMIT :PageSize;
                           """;

        await using NpgsqlConnection connection = await _source.OpenConnectionAsync(cancellationToken);

        await using var command = new NpgsqlCommand(sql, connection);

        command.Parameters.Add(new NpgsqlParameter("Cursor", query.Cursor));
        command.Parameters.Add(new NpgsqlParameter("Ids", query.OrderIds));
        command.Parameters.Add(new NpgsqlParameter<OrderHistoryItemKind?>("HistoryKind", query.HistoryKind));
        command.Parameters.Add(new NpgsqlParameter("PageSize", query.PageSize));

        await using NpgsqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            string payloadAsJson = await reader.GetFieldValueAsync<string>("order_history_item_payload", cancellationToken);

            OrderHistoryBaseType payloadAsBaseType = JsonSerializer.Deserialize<OrderHistoryBaseType>(payloadAsJson) ?? throw new NullReferenceException();

            yield return new OrderHistory(
                    reader.GetInt32("order_id"),
                    reader.GetDateTime("order_history_item_created_at"),
                    await reader.GetFieldValueAsync<OrderHistoryItemKind>("order_history_item_kind", cancellationToken),
                    payloadAsBaseType);
        }
    }
}