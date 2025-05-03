using Confluent.Kafka;

namespace Itmo.Csharp.Microservices.Lab4.OuterServiceUtility.Consumer.Messages;

public record ConsumerMessage<TKey, TValue>
{
    private readonly IConsumer<TKey, TValue> _consumer;
    private readonly ConsumeResult<TKey, TValue> _result;

    public ConsumerMessage(
        IConsumer<TKey, TValue> consumer,
        ConsumeResult<TKey, TValue> result,
        TKey key,
        TValue value)
    {
        _consumer = consumer;
        Key = key;
        Value = value;
        _result = result;
    }

    public TKey Key { get; }

    public TValue Value { get; }

    public void Commit()
    {
        _consumer.Commit(_result);
    }
}