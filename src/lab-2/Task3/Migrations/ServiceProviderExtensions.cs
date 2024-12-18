using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace Itmo.Csharp.Microservices.Lab2.Task3.Migrations;

public static class ServiceProviderExtensions
{
    public static async Task StartMigrations(this ServiceProvider provider)
    {
        await using AsyncServiceScope scope = provider.CreateAsyncScope();
        {
            IMigrationRunner runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

            runner.MigrateUp();
        }
    }
}