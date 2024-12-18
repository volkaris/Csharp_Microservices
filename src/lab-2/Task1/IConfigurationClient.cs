using Itmo.Csharp.Microservices.Lab2.Task1.Entities;

namespace Itmo.Csharp.Microservices.Lab2.Task1;

public interface IConfigurationClient
{
    IAsyncEnumerable<Configurations> GetConfigurationsAsync(int pageSize, string? pageToken, CancellationToken cancellationToken);
}