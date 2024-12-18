using Itmo.Csharp.Microservices.Lab2.Task3.Entities.Extensions;
using Itmo.Csharp.Microservices.Lab2.Task3.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Itmo.Csharp.Microservices.Lab2.Task3;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPostgresOptions(
        this IServiceCollection collection,
        IConfigurationRoot configurationRoot)
    {
        collection.AddOptions<PostgresOptions>().Bind(configurationRoot.GetSection("Persistence:Postgres"));

        return collection;
    }

    public static IServiceCollection AddNpgsqlDataSource(
        this IServiceCollection collection)
    {
        collection.AddSingleton<NpgsqlDataSource>(serviceProvider =>
        {
            IOptions<PostgresOptions> options = serviceProvider.GetRequiredService<IOptions<PostgresOptions>>();

            string connectionString =
                $"Host={options.Value.Host};Port={options.Value.Port};Username={options.Value.Username};Password={options.Value.Password}";

            var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);

            dataSourceBuilder.MapEnums();

            NpgsqlDataSource dataSource = dataSourceBuilder.Build();

            return dataSource;
        });

        return collection;
    }
}