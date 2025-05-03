using Itmo.Csharp.Microservices.Lab1.Messaging.Interfaces;
using Itmo.Csharp.Microservices.Lab1.Messaging.Models;
using Itmo.Dev.Platform.Common.Extensions;
using System.Runtime.CompilerServices;
using System.Threading.Channels;

namespace Itmo.Csharp.Microservices.Lab1.Messaging.Implementations;

public class MessageProcessor : IMessageSender, IMessageProcessor
{
    private readonly Channel<Message> _channel;
    private readonly IMessageHandler _messageHandler;
    private readonly int _batchSize;
    private readonly TimeSpan _timeout;

    public MessageProcessor(IMessageHandler messageHandler, int channelSize, int batchSize, TimeSpan timeout)
    {
        _messageHandler = messageHandler;
        _batchSize = batchSize;
        _timeout = timeout;
        _channel = Channel.CreateBounded<Message>(new BoundedChannelOptions(channelSize)
        {
            FullMode = BoundedChannelFullMode.DropOldest,
        });
    }

    public async ValueTask SendAsync(Message message, CancellationToken cancellationToken)
    {
        await _channel.Writer.WriteAsync(message, cancellationToken).ConfigureAwait(false);
    }

    public async Task ProcessAsync(CancellationToken cancellationToken)
    {
        ConfiguredCancelableAsyncEnumerable<IReadOnlyList<Message>> messagesChunked = _channel.Reader
            .ReadAllAsync(cancellationToken)
            .ChunkAsync(_batchSize, _timeout)
            .WithCancellation(cancellationToken);
        await foreach (IReadOnlyList<Message> messages in messagesChunked)
            await _messageHandler.HandleAsync(messages, cancellationToken).ConfigureAwait(false);
    }

    public void Complete()
    {
        _channel.Writer.Complete();
    }
}