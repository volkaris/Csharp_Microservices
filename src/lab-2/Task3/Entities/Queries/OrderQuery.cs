using Itmo.Csharp.Microservices.Lab2.Task3.Entities.Orders.OrderStates;

namespace Itmo.Csharp.Microservices.Lab2.Task3.Entities.Queries;

public record OrderQuery(
    int Cursor,
    int PageSize,
    IList<long> Ids,
    OrderState OrderState,
    string CreatedBy);