using Itmo.Csharp.Microservices.Lab4.Gateway.Extensions;
using Itmo.Csharp.Microservices.Lab4.Gateway.Middlewares;
using Itmo.Csharp.Microservices.Lab4.Gateway.Options;
using System.Text.Json.Serialization;

namespace Itmo.Csharp.Microservices.Lab4.Gateway;

#pragma warning disable CA1506

internal class Program
{
    public static async Task Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.AddOptions<ClientConnectionOptions>().Bind(builder.Configuration.GetSection("ClientConnectionOptions"));

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