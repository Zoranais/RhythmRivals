using RhythmRivals.WebAPI.ExceptionHandlers.Abstract;

namespace RhythmRivals.WebAPI.ExceptionHandlers;

public class InternalExceptionHandler : BaseExceptionHandler
{
    private readonly ILogger<RequestExceptionHandler> _logger;

    public InternalExceptionHandler(ILogger<RequestExceptionHandler> logger)
    {
        _logger = logger;
    }

    public override async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Exception occured: {Message}", exception.Message);
        await HandleException(httpContext, exception);

        return true;
    }
}
