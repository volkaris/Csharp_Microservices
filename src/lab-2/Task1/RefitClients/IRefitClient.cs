using Itmo.Csharp.Microservices.Lab2.Task1.Entities;
using Refit;

namespace Itmo.Csharp.Microservices.Lab2.Task1.RefitClients;

public interface IRefitClient
{
    [Get("/configurations")]
    Task<Page> GetConfigurationsAsync(int pageSize, string? pageToken);
}