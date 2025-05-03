using Itmo.Csharp.Microservices.Lab4.OuterServiceUtility.Producer.Messages;

namespace Itmo.Csharp.Microservices.Lab4.KafkaIntegration.Producers.Services;

public interface IKafkaServiceProducer<TKey, TValue>
{
    Task ProduceAsync(ProducerMessage<TKey, TValue> message, CancellationToken cancellationToken);
}