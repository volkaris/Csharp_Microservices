namespace Itmo.Csharp.Microservices.Lab4.OuterServiceUtility.Entities.ProcessOrder;

public record ProcessOrderRequest(long OrderId, TimeSpan CreatedAt);