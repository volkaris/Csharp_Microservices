using Itmo.Csharp.Microservices.Lab2.Task1;
using Itmo.Csharp.Microservices.Lab2.Task1.Entities;
using Itmo.Csharp.Microservices.Lab2.Task2.Providers;
using Itmo.Csharp.Microservices.Lab2.Task2.Services.Interfaces;

namespace Itmo.Csharp.Microservices.Lab2.Task2.Services.Implementations;

public class ConfigurationService : IConfigurationService
{
    private readonly CustomConfigurationProvider _configurationProvider;

    private readonly IConfigurationClient _configurationClient;

    public ConfigurationService(CustomConfigurationProvider configurationProvider, IConfigurationClient configurationClient)
    {
        _configurationProvider = configurationProvider;
        _configurationClient = configurationClient;
    }

    public async Task Update(int pageSize, string? pageToken, CancellationToken token)
    {
        var configs = new List<Configurations>();

        await foreach (Configurations configurations in _configurationClient.GetConfigurationsAsync(pageSize, pageToken, token))
        {
            configs.Add(configurations);
        }

        _configurationProvider.UpdateConfigurations(configs);
    }

    public async Task UpdateAfter(int pageSize, string? pageToken, TimeSpan time, CancellationToken token)
    {
        using var timer = new PeriodicTimer(time);

        try
        {
            while (await timer.WaitForNextTickAsync(token))
            {
                var configs = new List<Configurations>();

                await foreach (Configurations configurations in _configurationClient.GetConfigurationsAsync(pageSize, pageToken, token))
                {
                    configs.Add(configurations);
                }

                _configurationProvider.UpdateConfigurations(configs);
            }
        }
        catch (OperationCanceledException)
        {
        }
    }
}