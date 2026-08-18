using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace CardiacPatientMonitoring.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
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
                "An unhandled exception occurred while processing the request.");

            context.Response.StatusCode =
                StatusCodes.Status500InternalServerError;
            var problem = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An unexpected error occurred.",
                Detail = "Please try again later."
            };

            await context.Response.WriteAsJsonAsync(
                problem,
                JsonSerializerOptions.Web,
                "application/problem+json",
                context.RequestAborted);
        }
    }
}
