namespace Itmo.Csharp.Microservices.Lab4.OuterServiceUtility.Producer.Messages;

public record ProducerMessage<TKey, TValue>(TKey Key, TValue Value);