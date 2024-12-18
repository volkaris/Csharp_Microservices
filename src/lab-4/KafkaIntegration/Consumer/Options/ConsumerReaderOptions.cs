namespace Itmo.Csharp.Microservices.Lab4.KafkaIntegration.Consumer.Options;

public class ConsumerReaderOptions
{
    public string BootstrapServers { get; set; } = string.Empty;

    public string TopicName { get; set; } = string.Empty;

    public string GroupId { get; set; } = string.Empty;
}