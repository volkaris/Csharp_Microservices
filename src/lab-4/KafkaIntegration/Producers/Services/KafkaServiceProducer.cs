using Confluent.Kafka;
using Google.Protobuf;
using Itmo.Csharp.Microservices.Lab4.KafkaIntegration.Producers.Options;
using Itmo.Csharp.Microservices.Lab4.KafkaIntegration.Serializers;
using Itmo.Csharp.Microservices.Lab4.OuterServiceUtility.Producer.Messages;
using Microsoft.Extensions.Options;

namespace Itmo.Csharp.Microservices.Lab4.KafkaIntegration.Producers.Services;

public class KafkaServiceProducer<TKey, TValue> : IKafkaServiceProducer<TKey, TValue>, IDisposable where TKey : IMessage<TKey>, new() where TValue : IMessage<TValue>, new()
{
    private readonly IProducer<TKey, TValue> _producer;
    private readonly string _topicName;

    public KafkaServiceProducer(IOptions<ProducerOptions> producerOptions)
    {
        _topicName = producerOptions.Value.TopicName;

        var config = new ProducerConfig
        {
            BootstrapServers = producerOptions.Value.BootstrapServers,
        };

        var keySerializer = new ProtobufValueSerializer<TKey>();
        var valueSerializer = new ProtobufValueSerializer<TValue>();

        _producer = new ProducerBuilder<TKey, TValue>(config)
            .SetKeySerializer(keySerializer)
            .SetValueSerializer(valueSerializer)
            .Build();
    }

    public async Task ProduceAsync(ProducerMessage<TKey, TValue> message, CancellationToken cancellationToken)
    {
        var msg = new Message<TKey, TValue>
        {
            Key = message.Key,
            Value = message.Value,
        };

        await _producer.ProduceAsync(
            _topicName,
            msg,
            cancellationToken);
    }

    public void Dispose()
    {
        _producer.Dispose();
    }
}