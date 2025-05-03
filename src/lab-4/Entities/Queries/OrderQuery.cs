using Itmo.Csharp.Microservices.Lab4.Entities.Orders.OrderStates;

namespace Itmo.Csharp.Microservices.Lab4.Entities.Queries;

public record OrderQuery(
    int Cursor,
    int PageSize,
    IList<long> Ids,
    OrderState OrderState,
    string CreatedBy);