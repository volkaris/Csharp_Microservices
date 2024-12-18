using Itmo.Csharp.Microservices.Lab1.Messaging.Models;

namespace Itmo.Csharp.Microservices.Lab1.Messaging.Interfaces;

public interface IMessageSender
{
    ValueTask SendAsync(Message message, CancellationToken cancellationToken);
}