using Confluent.Kafka;
using Google.Protobuf;
using Itmo.Csharp.Microservices.Lab4.KafkaIntegration.Consumer.Options;
using Itmo.Csharp.Microservices.Lab4.KafkaIntegration.Serializers;
using Itmo.Csharp.Microservices.Lab4.OuterServiceUtility.Consumer.Messages;
using Microsoft.Extensions.Options;
using System.Threading.Channels;

namespace Itmo.Csharp.Microservices.Lab4.KafkaIntegration.Consumer.Readers;

public class ConsumerReader<TKey, TValue> : IConsumerReader<TKey, TValue> where TValue : IMessage<TValue>, new() where TKey : IMessage<TKey>, new()
{
    private readonly string _bootstrapServers;

    private readonly string _topicName;

    private readonly string _groupId;

    public ConsumerReader(IOptions<ConsumerReaderOptions> options)
    {
        _bootstrapServers = options.Value.BootstrapServers;
        _topicName = options.Value.TopicName;
        _groupId = options.Value.GroupId;
    }

    public async Task ReadAsync(ChannelWriter<ConsumerMessage<TKey, TValue>> writer, CancellationToken cancellationToken)
    {
        var consumerConfiguration = new ConsumerConfig
        {
            BootstrapServers = _bootstrapServers,
            GroupId = _groupId,
        };

        var keySerializer = new ProtobufValueSerializer<TKey>();
        var valueSerializer = new ProtobufValueSerializer<TValue>();

        using IConsumer<TKey, TValue> consumer = new ConsumerBuilder<TKey, TValue>(consumerConfiguration)
            .SetKeyDeserializer(keySerializer)
            .SetValueDeserializer(valueSerializer)
            .Build();

        consumer.Subscribe(_topicName);

        try
        {
            while (cancellationToken.IsCancellationRequested is false)
            {
                ConsumeResult<TKey, TValue>? res = consumer.Consume(cancellationToken);

                var msg = new ConsumerMessage<TKey, TValue>(
                    consumer,
                    res,
                    res.Message.Key,
                    res.Message.Value);

                await writer.WriteAsync(msg, cancellationToken);
            }
        }
        finally
        {
            consumer.Close();
        }
    }
}