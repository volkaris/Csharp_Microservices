namespace Itmo.Csharp.Microservices.Lab1.Reactive;

public interface IRequestClient
{
    Task<ResponseModel> SendAsync(RequestModel request, CancellationToken cancellationToken);
}