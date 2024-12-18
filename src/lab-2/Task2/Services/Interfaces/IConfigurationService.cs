namespace Itmo.Csharp.Microservices.Lab2.Task2.Services.Interfaces;

public interface IConfigurationService
{
    public Task Update(int pageSize, string? pageToken, CancellationToken token);

    public Task UpdateAfter(int pageSize, string? pageToken, TimeSpan time, CancellationToken token);
}