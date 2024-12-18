using Grpc.Core;

namespace Itmo.Csharp.Microservices.Lab4.Gateway.Middlewares;

public class GrpcExceptionsHandler : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (RpcException e)
        {
            if (e.StatusCode == StatusCode.NotFound)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            }

            await context.Response.WriteAsJsonAsync(e.Message);
        }
    }
}