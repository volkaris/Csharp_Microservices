using Itmo.Csharp.Microservices.Lab1.Messaging.Interfaces;
using Itmo.Csharp.Microservices.Lab1.Messaging.Models;
using System.Text;

namespace Itmo.Csharp.Microservices.Lab1.Messaging.Implementations;

public class ConsoleHandler : IMessageHandler
{
    public ValueTask HandleAsync(IEnumerable<Message> messages, CancellationToken cancellationToken)
    {
        var stringBuilder = new StringBuilder();

        foreach (Message message in messages)
        {
            cancellationToken.ThrowIfCancellationRequested();
            stringBuilder.Append(message + " ");
        }

        Console.WriteLine(stringBuilder.ToString());

        return ValueTask.CompletedTask;
    }
}