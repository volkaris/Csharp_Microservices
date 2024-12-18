using Itmo.Csharp.Microservices.Lab4.Entities.Orders.OrderStates;
using Itmo.Csharp.Microservices.Lab4.Entities.OrdersHistory;
using Npgsql;

namespace Itmo.Csharp.Microservices.Lab4.Entities.Extensions;

public static class DataSourceBuilderExtensions
{
    public static NpgsqlDataSourceBuilder MapEnums(this NpgsqlDataSourceBuilder builder)
    {
        builder.MapEnum<OrderState>("order_state");

        builder.MapEnum<OrderHistoryItemKind>("order_history_item_kind");

        return builder;
    }
}