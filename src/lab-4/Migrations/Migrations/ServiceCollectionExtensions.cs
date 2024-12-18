using FluentMigrator.Runner;
using Itmo.Csharp.Microservices.Lab4.Entities.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Itmo.Csharp.Microservices.Lab4.Migrations.Migrations;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMigrations(this IServiceCollection collection)
    {
        collection.AddFluentMigratorCore()
            .ConfigureRunner(runner => runner
                .AddPostgres()
                .WithGlobalConnectionString(provider =>
                {
                    IOptions<PostgresOptions> options = provider.GetRequiredService<IOptions<PostgresOptions>>();

                    return
                        $"Host={options.Value.Host};Port={options.Value.Port};Username={options.Value.Username};Password={options.Value.Password}";
                })
                .WithMigrationsIn(typeof(IMigrationAssemblyMarker).Assembly));

        return collection;
    }
}