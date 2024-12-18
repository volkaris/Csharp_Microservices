using System.Text.Json.Serialization;

namespace Itmo.Csharp.Microservices.Lab4.Entities.OrdersHistory.HistoryRecords;

[JsonDerivedType(typeof(OrderCreate), nameof(OrderCreate))]
[JsonDerivedType(typeof(ItemAddedToOrder), nameof(ItemAddedToOrder))]
[JsonDerivedType(typeof(ItemRemovedFromOrder), nameof(ItemRemovedFromOrder))]
[JsonDerivedType(typeof(OrderStateChange), nameof(OrderStateChange))]
public record OrderHistoryBaseType();