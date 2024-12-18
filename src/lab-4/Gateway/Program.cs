#pragma warning disable IDE0005
using Itmo.Csharp.Microservices.Lab4.Gateway.Extensions;
using Itmo.Csharp.Microservices.Lab4.Gateway.Middlewares;
using System.Text.Json.Serialization;

namespace Itmo.Csharp.Microservices.Lab4.Gateway;

internal class Program
{
    public static async Task Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services
            .AddClientConnectionOptions(builder.Configuration)
            .AddOuterOrderServiceConnectionOptions(builder.Configuration);

        builder.Services
            .AddProductsServiceClient()
            .AddOrdersServiceClient()
            .AddOuterOrdersServiceClient();

        builder.Services.AddEndpointsApiExplorer()
            .AddSwaggerGen(swaggerOptions =>
            {
                swaggerOptions.EnableAnnotations();

                swaggerOptions.UseAllOfForInheritance();

                swaggerOptions.UseOneOfForPolymorphism();
            })
            .AddControllers()
            .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        builder.Services
            .AddScoped<GrpcExceptionsHandler>()
            .AddProductsServiceClient()
            .AddOrdersServiceClient()
            .AddOuterOrdersServiceClient()
            .AddGrpc();

        WebApplication app = builder.Build();

        app.UseMiddleware<GrpcExceptionsHandler>();

        app.MapControllers();

        app
            .UseRouting()
            .UseSwagger()
            .UseSwaggerUI();

        await app.RunAsync();
    }
}