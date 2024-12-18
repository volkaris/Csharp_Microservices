namespace Itmo.Csharp.Microservices.Lab4.KafkaIntegration.Consumer.Options;

public class ConsumerHandlerOptions
{
    public int BatchSize { get; set; }

    public int Timeout { get; set; }
}