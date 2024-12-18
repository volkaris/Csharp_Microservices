using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Itmo.Csharp.Microservices.Lab4.GrpcService.Interceptors;

public class ExceptionsInterceptor : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (RpcException e)
        {
            string message = $"Exception happened while processing request, type = {e.GetType().Name}, message = {e.Message}";

            throw new RpcException(new Status(e.StatusCode, message));
        }
    }

    public override async Task ServerStreamingServerHandler<TRequest, TResponse>(
        TRequest request,
        IServerStreamWriter<TResponse> responseStream,
        ServerCallContext context,
        ServerStreamingServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            await continuation(request, responseStream, context);
        }
        catch (Exception e)
        {
            string message = $"Exception happened while processing request, type = {e.GetType().Name}, message = {e.Message}";

            throw new RpcException(new Status(StatusCode.Internal, message));
        }
    }
}