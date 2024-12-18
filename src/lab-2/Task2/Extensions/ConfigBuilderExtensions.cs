using Microsoft.Extensions.Configuration;

namespace Itmo.Csharp.Microservices.Lab2.Task2.Extensions;

public static class ConfigBuilderExtensions
{
    public static IConfigurationBuilder AddConfigurationSource(this IConfigurationBuilder configBuilder, IConfigurationSource source)
    {
        configBuilder.Add(source);

        return configBuilder;
    }
}