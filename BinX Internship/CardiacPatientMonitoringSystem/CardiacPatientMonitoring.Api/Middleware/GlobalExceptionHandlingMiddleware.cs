namespace CardiacPatientMonitoring.Api.Middleware;

public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    public GlobalExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "Unhandled exception while processing {RequestMethod} " +
                "{RequestPath}. Trace ID: {TraceIdentifier}",
                context.Request.Method,
                context.Request.Path.Value,
                context.TraceIdentifier);

            if (context.Response.HasStarted)
            {
                throw;
            }

            context.Response.Clear();

            var problem = Results.Problem(
                statusCode:
                    StatusCodes.Status500InternalServerError,
                title: "An unexpected error occurred.",
                detail: "The server could not complete the request.",
                instance: context.Request.Path.Value,
                extensions: new Dictionary<string, object?>
                {
                    ["traceId"] = context.TraceIdentifier
                });

            await problem.ExecuteAsync(context);
        }
    }
}
