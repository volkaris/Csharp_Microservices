namespace Itmo.Csharp.Microservices.Lab2.Task3.Entities.Queries;

public record OrderItemQuery(
    int Cursor,
    int PageSize,
    IList<long> OrderIds,
    IList<long> ProductIds,
    bool IsDeleted = false);