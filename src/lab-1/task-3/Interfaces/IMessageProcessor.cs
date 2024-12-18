namespace Itmo.Csharp.Microservices.Lab1.Messaging.Interfaces;

public interface IMessageProcessor
{
    Task ProcessAsync(CancellationToken cancellationToken);

    void Complete();
}