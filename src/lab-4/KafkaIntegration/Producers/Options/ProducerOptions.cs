namespace Itmo.Csharp.Microservices.Lab4.KafkaIntegration.Producers.Options;

public class ProducerOptions
{
    public string TopicName { get; set; } = string.Empty;

    public string BootstrapServers { get; set; } = string.Empty;
}