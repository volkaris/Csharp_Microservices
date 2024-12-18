namespace Itmo.Csharp.Microservices.Lab3.Middlewares;

public class ExceptionHandler : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (Exception e)
        {
            string message = $"Unexpected error occurred when trying to process.Error type {e.GetType()} . Error details : {e.Message}";

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            await context.Response.WriteAsJsonAsync(message);
        }
    }
}