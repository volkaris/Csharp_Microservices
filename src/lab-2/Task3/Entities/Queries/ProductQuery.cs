namespace Itmo.Csharp.Microservices.Lab2.Task3.Entities.Queries;

public record ProductQuery(
    int Cursor,
    int PageSize,
    IList<long> Ids,
    string NamePattern,
    decimal MinCost,
    decimal MaxCost);