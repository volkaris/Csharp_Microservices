namespace Itmo.Csharp.Microservices.Lab4.OuterServiceUtility.Entities.ApproveOrder;

public record ApproveOrderRequest(bool IsApproved, string ApprovedBy, string? FailureReason);