using Itmo.Csharp.Microservices.Lab2.Task2.Services.Interfaces;
using Itmo.Csharp.Microservices.Lab3.Options;
using Microsoft.Extensions.Options;

namespace Itmo.Csharp.Microservices.Lab3.BackgroundServices;

public class ConfigurationBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IOptions<PageInfoOptions> _options;

    public ConfigurationBackgroundService(IServiceScopeFactory scopeFactory, IOptions<PageInfoOptions> options)
    {
        _scopeFactory = scopeFactory;
        _options = options;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        await InitializeProvider(cancellationToken);

        await base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using AsyncServiceScope scope = _scopeFactory.CreateAsyncScope();

        IConfigurationService service = scope.ServiceProvider.GetRequiredService<IConfigurationService>();

        while (!stoppingToken.IsCancellationRequested)
        {
            await service.UpdateAfter(_options.Value.PageSize, _options.Value.PageToken, TimeSpan.FromSeconds(_options.Value.UpdateTime), stoppingToken);
        }
    }

    private async Task InitializeProvider(CancellationToken token)
    {
        await using AsyncServiceScope scope = _scopeFactory.CreateAsyncScope();

        IConfigurationService service = scope.ServiceProvider.GetRequiredService<IConfigurationService>();

        await service.Update(_options.Value.PageSize, _options.Value.PageToken, token);
    }
}