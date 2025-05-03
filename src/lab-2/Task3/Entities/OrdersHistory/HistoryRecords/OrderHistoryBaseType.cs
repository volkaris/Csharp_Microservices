using System.Text.Json.Serialization;

namespace Itmo.Csharp.Microservices.Lab2.Task3.Entities.OrdersHistory.HistoryRecords;

[JsonDerivedType(typeof(OrderCreated), nameof(OrderCreated))]
[JsonDerivedType(typeof(ItemAddedToOrder), nameof(ItemAddedToOrder))]
[JsonDerivedType(typeof(ItemRemovedFromOrder), nameof(ItemRemovedFromOrder))]
[JsonDerivedType(typeof(OrderStateChange), nameof(OrderStateChange))]
public record OrderHistoryBaseType();