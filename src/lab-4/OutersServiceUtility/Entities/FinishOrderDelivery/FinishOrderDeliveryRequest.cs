namespace Itmo.Csharp.Microservices.Lab4.OuterServiceUtility.Entities.FinishOrderDelivery;

public record FinishOrderDeliveryRequest(bool IsSuccessful, string? FailureReason);