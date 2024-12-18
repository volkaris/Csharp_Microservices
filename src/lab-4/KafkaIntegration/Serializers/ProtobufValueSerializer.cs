using Confluent.Kafka;
using Google.Protobuf;

namespace Itmo.Csharp.Microservices.Lab4.KafkaIntegration.Serializers;

public class ProtobufValueSerializer<T> : ISerializer<T>, IDeserializer<T> where T : IMessage<T>, new()
{
    private readonly MessageParser<T> _parser = new(() => new T());

    public byte[] Serialize(T data, SerializationContext context)
    {
        return data.ToByteArray();
    }

    public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        if (isNull)
        {
            throw new ArgumentNullException(nameof(data), "Message can't be null");
        }

        return _parser.ParseFrom(data);
    }
}