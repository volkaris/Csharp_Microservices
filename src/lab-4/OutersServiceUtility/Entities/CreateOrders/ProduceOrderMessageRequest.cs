using Itmo.Csharp.Microservices.Lab4.OuterServiceUtility.Producer.Messages;
using Orders.Kafka.Contracts;

namespace Itmo.Csharp.Microservices.Lab4.OuterServiceUtility.Entities.CreateOrders;

public record ProduceOrderMessageRequest(IAsyncEnumerable<ProducerMessage<OrderCreationKey, OrderCreationValue>> Messages);