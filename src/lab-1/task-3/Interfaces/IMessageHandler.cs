using Itmo.Csharp.Microservices.Lab1.Messaging.Models;

namespace Itmo.Csharp.Microservices.Lab1.Messaging.Interfaces;

public interface IMessageHandler
{
    ValueTask HandleAsync(IEnumerable<Message> messages, CancellationToken cancellationToken);
}