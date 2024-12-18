namespace Itmo.Csharp.Microservices.Lab1.Reactive;

public interface ILibraryOperationService
{
    void BeginOperation(Guid requestId, RequestModel model, CancellationToken cancellationToken);
}