using Itmo.Csharp.Microservices.Lab2.Task2.Providers;
using Microsoft.Extensions.Configuration;

namespace Itmo.Csharp.Microservices.Lab2.Task2.ConfigurationsSources;

public class ConfigurationSource : IConfigurationSource
{
    private readonly CustomConfigurationProvider _provider;

    public ConfigurationSource(CustomConfigurationProvider provider)
    {
        _provider = provider;
    }

    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        return _provider;
    }
}