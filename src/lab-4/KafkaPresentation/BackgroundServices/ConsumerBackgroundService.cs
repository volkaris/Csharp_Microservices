using Itmo.Csharp.Microservices.Lab4.KafkaIntegration.Consumer;
using Itmo.Csharp.Microservices.Lab4.KafkaIntegration.Consumer.Readers;
using Itmo.Csharp.Microservices.Lab4.KafkaPresentation.Options;
using Itmo.Csharp.Microservices.Lab4.OuterServiceUtility.Consumer.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Channels;

namespace Itmo.Csharp.Microservices.Lab4.KafkaPresentation.BackgroundServices;

public class ConsumerBackgroundService<TKey, TValue> : BackgroundService
{
    private readonly int _channelCapacity;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ConsumerBackgroundService<TKey, TValue>> _logger;

    public ConsumerBackgroundService(
        ILogger<ConsumerBackgroundService<TKey, TValue>> logger,
        IServiceScopeFactory scopeFactory,
        IOptions<BackgroundServiceOptions> options)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        _channelCapacity = options.Value.ChannelCapacity;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (stoppingToken.IsCancellationRequested is false)
        {
            try
            {
                await ExecuteSingleAsync(stoppingToken);
            }
            catch (Exception e) when (e is not OperationCanceledException or TaskCanceledException)
            {
                _logger.LogError(e, "Error while consuming message");
            }
        }
    }

    private async Task ExecuteSingleAsync(CancellationToken cancellationToken)
    {
        await Task.Yield();

        await using AsyncServiceScope res = _scopeFactory.CreateAsyncScope();

        IConsumerReader<TKey, TValue> reader = res.ServiceProvider.GetRequiredService<IConsumerReader<TKey, TValue>>();
        IConsumerHandler<TKey, TValue> handler = res.ServiceProvider.GetRequiredService<IConsumerHandler<TKey, TValue>>();

        var channel = Channel.CreateBounded<ConsumerMessage<TKey, TValue>>(_channelCapacity);

        Parallel.Invoke(
            () => reader.ReadAsync(channel.Writer, cancellationToken),
            () => handler.HandleAsync(channel.Reader, cancellationToken));
    }
}