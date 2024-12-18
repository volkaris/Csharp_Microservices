namespace Itmo.Csharp.Microservices.Lab4.OuterServiceUtility.Entities.FinishOrderPacking;

public record FinishOrderPackingRequest(bool IsSuccessful, string? FailureReason);