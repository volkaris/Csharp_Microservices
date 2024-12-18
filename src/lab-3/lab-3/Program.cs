using Itmo.Csharp.Microservices.Lab2.Task1.Extensions;
using Itmo.Csharp.Microservices.Lab2.Task1.ManualClients.Extensions;
using Itmo.Csharp.Microservices.Lab2.Task2.ConfigurationsSources;
using Itmo.Csharp.Microservices.Lab2.Task2.Extensions;
using Itmo.Csharp.Microservices.Lab2.Task2.Providers.Extensions;
using Itmo.Csharp.Microservices.Lab2.Task2.Services.Extensions;
using Itmo.Csharp.Microservices.Lab2.Task3;
using Itmo.Csharp.Microservices.Lab2.Task3.Migrations;
using Itmo.Csharp.Microservices.Lab2.Task3.Repositories;
using Itmo.Csharp.Microservices.Lab2.Task3.Services;
using Itmo.Csharp.Microservices.Lab3.BackgroundServices;
using Itmo.Csharp.Microservices.Lab3.Extensions;
using Itmo.Csharp.Microservices.Lab3.Middlewares;
using System.Text.Json.Serialization;

namespace Itmo.Csharp.Microservices.Lab3;

#pragma warning disable CA1506

internal class Program
{
    public static async Task Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Configuration.AddOuterServiceProvider().AddPageInfoProvider();

        builder.Services
            .AddPageInfoOptions(builder.Configuration)
            .AddOuterServiceOptions(builder.Configuration)
            .AddHttpClient()
            .AddManualClient()
            /*.AddRefitClient()*/
            .AddConfigurationProvider()
            .AddConfigurationSource()
            .AddConfigurationService()
            .AddRepositories()
            .AddServices()
            .AddNpgsqlDataSource()
            .AddPostgresOptions(builder.Configuration)
            .AddMigrations()
            .AddHostedService<ConfigurationBackgroundService>()
            .AddHostedService<MigrationBackgroundService>()
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(swaggerOptions => swaggerOptions.EnableAnnotations())
            .AddExceptionHandler()
            .AddControllers()
            .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        WebApplication app = builder.Build();

        builder.Configuration.AddConfigurationSource(app.Services.GetRequiredService<IConfigurationSource>());

        app.MapControllers();

        app
            .UseMiddleware<ExceptionHandler>()
            .UseRouting()
            .UseSwagger()
            .UseSwaggerUI();

        await app.RunAsync();
    }
}