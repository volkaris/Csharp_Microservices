using Itmo.Csharp.Microservices.Lab2.Task2.Services.Implementations;
using Itmo.Csharp.Microservices.Lab2.Task2.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Itmo.Csharp.Microservices.Lab2.Task2.Services.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConfigurationService(this IServiceCollection collection)
    {
        collection.AddScoped<IConfigurationService, ConfigurationService>();

        return collection;
    }
}