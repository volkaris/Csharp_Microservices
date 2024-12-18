namespace Itmo.Csharp.Microservices.Lab4.Entities.Queries;

public record OrderItemQuery(
    int Cursor,
    int PageSize,
    IList<long> OrderIds,
    IList<long> ProductIds,
    bool IsDeleted = false);