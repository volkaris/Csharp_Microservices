using Itmo.Csharp.Microservices.Lab4.OuterServiceUtility.Consumer.Messages;
using System.Threading.Channels;

namespace Itmo.Csharp.Microservices.Lab4.KafkaIntegration.Consumer.Readers;

public interface IConsumerReader<TKey, TValue>
{
    public Task ReadAsync(ChannelWriter<ConsumerMessage<TKey, TValue>> writer, CancellationToken cancellationToken);
}