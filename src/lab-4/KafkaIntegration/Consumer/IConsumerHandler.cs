using Itmo.Csharp.Microservices.Lab4.OuterServiceUtility.Consumer.Messages;
using System.Threading.Channels;

namespace Itmo.Csharp.Microservices.Lab4.KafkaIntegration.Consumer;

public interface IConsumerHandler<TKey, TValue>
{
    Task HandleAsync(ChannelReader<ConsumerMessage<TKey, TValue>> reader, CancellationToken cancellationToken);
}