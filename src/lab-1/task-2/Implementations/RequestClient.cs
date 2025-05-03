using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace Itmo.Csharp.Microservices.Lab1.Reactive.Implementations;

public class RequestClient : IRequestClient, ILibraryOperationHandler
{
    private readonly ILibraryOperationService _service;
    private readonly ConcurrentDictionary<Guid, TaskCompletionSource<ResponseModel>> _requests = new();

    public RequestClient(ILibraryOperationService service)
    {
        _service = service;
    }

    public async Task<ResponseModel> SendAsync(RequestModel request, CancellationToken cancellationToken)
    {
        var guid = Guid.NewGuid();
        var task = new TaskCompletionSource<ResponseModel>(TaskCreationOptions.RunContinuationsAsynchronously);
        _requests.TryAdd(guid, task);

        await using ConfiguredAsyncDisposable reg = cancellationToken.Register(() =>
            {
                if (_requests.TryRemove(guid, out TaskCompletionSource<ResponseModel>? res))
                    res.TrySetCanceled(cancellationToken);
            })
            .ConfigureAwait(false);
        try
        {
            _service.BeginOperation(guid, request, cancellationToken);
        }
        catch (Exception e)
        {
            if (_requests.TryRemove(guid, out TaskCompletionSource<ResponseModel>? res))
            {
                res.SetException(e);
                throw;
            }
        }

        return await task.Task.ConfigureAwait(false);
    }

    public void HandleOperationResult(Guid requestId, byte[] data)
    {
        if (!_requests.TryRemove(requestId, out TaskCompletionSource<ResponseModel>? request))
            return;
        request.SetResult(new ResponseModel(data));
    }

    public void HandleOperationError(Guid requestId, Exception exception)
    {
        if (!_requests.TryRemove(requestId, out TaskCompletionSource<ResponseModel>? request))
            return;
        request.SetException(exception);
    }
}