using Microsoft.AspNetCore.Diagnostics;

namespace RhythmRivals.WebAPI.Middlewares;

public class ExceptionHandlerMiddleware
{
    private readonly IEnumerable<IExceptionHandler> _handlers;
    private readonly RequestDelegate _next;
    public ExceptionHandlerMiddleware(IServiceProvider services, RequestDelegate next)
    {
        _handlers = services.GetServices<IExceptionHandler>();
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var handleResult = await TryHandleAsync(context, ex);

            if (!handleResult)
            {
                throw;
            }
        }
    }

    private async Task<bool> TryHandleAsync(HttpContext context, Exception ex)
    {
        foreach (var handler in _handlers)
        {
            if (await handler.TryHandleAsync(context, ex, default))
            {
                return true;
            }
        }

        return false;
    }
}
