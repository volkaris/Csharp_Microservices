using FluentMigrator.Runner;

namespace Itmo.Csharp.Microservices.Lab3.BackgroundServices;

public class MigrationBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public MigrationBackgroundService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using AsyncServiceScope scope = _scopeFactory.CreateAsyncScope();

        IMigrationRunner runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

        runner.MigrateUp();
    }
}