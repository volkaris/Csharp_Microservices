namespace Itmo.Csharp.Microservices.Lab1.Reactive;

public interface ILibraryOperationHandler
{
    void HandleOperationResult(Guid requestId, byte[] data);

    void HandleOperationError(Guid requestId, Exception exception);
}