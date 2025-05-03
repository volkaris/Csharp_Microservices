using Itmo.Csharp.Microservices.Lab4.KafkaIntegration.Consumer;
using Itmo.Csharp.Microservices.Lab4.KafkaIntegration.Consumer.Options;
using Itmo.Csharp.Microservices.Lab4.OuterServiceUtility.Consumer.Messages;
using Itmo.Csharp.Microservices.Lab4.Services.Orders;
using Itmo.Dev.Platform.Common.Extensions;
using Microsoft.Extensions.Options;
using Orders.Kafka.Contracts;
using System.Threading.Channels;

namespace Itmo.Csharp.Microservices.Lab4.KafkaPresentation.Handlers;

public class ConsumerHandler : IConsumerHandler<OrderProcessingKey, OrderProcessingValue>
{
    private readonly IOrderService _service;
    private readonly int _batchSize;
    private readonly int _timeout;

    public ConsumerHandler(
        IOptions<ConsumerHandlerOptions> options,
        IOrderService service)
    {
        _service = service;
        _timeout = options.Value.Timeout;
        _batchSize = options.Value.BatchSize;
    }

    public async Task HandleAsync(ChannelReader<ConsumerMessage<OrderProcessingKey, OrderProcessingValue>> reader, CancellationToken cancellationToken)
    {
        IAsyncEnumerable<IReadOnlyList<ConsumerMessage<OrderProcessingKey, OrderProcessingValue>>> chunkedMessages =
            reader
                .ReadAllAsync(cancellationToken)
                .ChunkAsync(_batchSize, TimeSpan.FromSeconds(_timeout));

        await foreach (IReadOnlyList<ConsumerMessage<OrderProcessingKey, OrderProcessingValue>> messages in chunkedMessages)
        {
            foreach (ConsumerMessage<OrderProcessingKey, OrderProcessingValue>? message in messages)
            {
                if (message.Value.EventCase == OrderProcessingValue.EventOneofCase.ApprovalReceived)
                {
                    if (!string.IsNullOrEmpty(message.Value.ApprovalReceived.FailureReason))
                    {
                        await _service.CancelOrderAsync(message.Key.OrderId, cancellationToken);

                        message.Commit();

                        break;
                    }
                }
                else if (message.Value.EventCase == OrderProcessingValue.EventOneofCase.PackingFinished)
                {
                    if (!string.IsNullOrEmpty(message.Value.PackingFinished.FailureReason))
                    {
                        await _service.CancelOrderAsync(message.Key.OrderId, cancellationToken);

                        message.Commit();

                        break;
                    }
                }
                else if (message.Value.EventCase == OrderProcessingValue.EventOneofCase.DeliveryFinished)
                {
                    if (!string.IsNullOrEmpty(message.Value.DeliveryFinished.FailureReason))
                    {
                        await _service.CancelOrderAsync(message.Key.OrderId, cancellationToken);

                        message.Commit();

                        break;
                    }
                }

                await _service.ChangeOrderStateToProcessingAsync(message.Key.OrderId, $"Changed to {message.Value.EventCase.ToString()}", cancellationToken);

                message.Commit();
            }
        }
    }
}