using Itmo.Csharp.Microservices.Lab1.Messaging.Implementations;
using Itmo.Csharp.Microservices.Lab1.Messaging.Interfaces;
using Itmo.Csharp.Microservices.Lab1.Messaging.Models;

internal class Program
{
    public static async Task Main(string[] args)
    {
        var implementation = new MessageProcessor(new ConsoleHandler(), 100, 10, TimeSpan.FromSeconds(2));

#pragma warning disable CA1859
        IMessageProcessor processor = implementation;

        IMessageSender sender = implementation;
        var cts = new CancellationTokenSource();

        Task processingTask = processor.ProcessAsync(cts.Token);

        IEnumerable<Message> messages = GenerateMessages(100);

        await Parallel.ForEachAsync(
                messages,
                cts.Token,
                async (message, token) =>
                {
                    await sender.SendAsync(message, token).ConfigureAwait(false);
                })
            .ConfigureAwait(false);

        processor.Complete();

        await processingTask.ConfigureAwait(false);
    }

    private static IEnumerable<Message> GenerateMessages(int count)
    {
        return Enumerable.Range(0, count).Select(number => new Message($"Title {number}", $"Text {number}\n"));
    }
}