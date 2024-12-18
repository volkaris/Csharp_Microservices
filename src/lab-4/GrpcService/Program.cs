using Itmo.Csharp.Microservices.Lab2.Task1.Extensions;
using Itmo.Csharp.Microservices.Lab2.Task1.ManualClients.Extensions;
using Itmo.Csharp.Microservices.Lab2.Task2.ConfigurationsSources;
using Itmo.Csharp.Microservices.Lab2.Task2.Extensions;
using Itmo.Csharp.Microservices.Lab2.Task2.Providers.Extensions;
using Itmo.Csharp.Microservices.Lab2.Task2.Services.Extensions;
using Itmo.Csharp.Microservices.Lab3.BackgroundServices;
using Itmo.Csharp.Microservices.Lab3.Extensions;
using Itmo.Csharp.Microservices.Lab4.Entities;
using Itmo.Csharp.Microservices.Lab4.GrpcService.Interceptors;
using Itmo.Csharp.Microservices.Lab4.GrpcService.Services.Orders;
using Itmo.Csharp.Microservices.Lab4.GrpcService.Services.Products;
using Itmo.Csharp.Microservices.Lab4.KafkaIntegration;
using Itmo.Csharp.Microservices.Lab4.KafkaIntegration.Consumer.Extensions;
using Itmo.Csharp.Microservices.Lab4.KafkaIntegration.Producers.Extensions;
using Itmo.Csharp.Microservices.Lab4.KafkaPresentation;
using Itmo.Csharp.Microservices.Lab4.KafkaPresentation.Extensions;
using Itmo.Csharp.Microservices.Lab4.Migrations.Migrations;
using Itmo.Csharp.Microservices.Lab4.Repositories;
using Itmo.Csharp.Microservices.Lab4.Services;
using System.Text.Json.Serialization;

namespace Itmo.Csharp.Microservices.Lab4.GrpcService;

#pragma warning disable CA1506
internal class Program
{
    public static async Task Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Configuration.AddOuterServiceProvider().AddPageInfoProvider();

        builder.Services
            .AddKafkaProducerOptions(builder.Configuration)
            .AddKafkaConsumerOptions(builder.Configuration)
            .AddKafkaBackgroundServiceOptions(builder.Configuration);

        builder.Services
            .AddProducer()
            .AddConsumerReader()
            .AddConsumerHandler()
            .AddConsumerBackgroundService();

        builder.Services.AddGrpc(
            grpc =>
            {
                grpc.Interceptors.Add<ExceptionsInterceptor>();

                grpc.EnableDetailedErrors = true;
            });

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
            .AddPostgresOptions(builder.Configuration)
            .AddNpgsqlDataSource()
            .AddMigrations()
            .AddHostedService<ConfigurationBackgroundService>()
            .AddHostedService<MigrationBackgroundService>()
            .AddControllers()
            .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        WebApplication app = builder.Build();

        app.MapGrpcService<ProductsGrpcService>();
        app.MapGrpcService<OrderGrpcService>();

        builder.Configuration.AddConfigurationSource(app.Services.GetRequiredService<IConfigurationSource>());

        await app.RunAsync();
    }
}