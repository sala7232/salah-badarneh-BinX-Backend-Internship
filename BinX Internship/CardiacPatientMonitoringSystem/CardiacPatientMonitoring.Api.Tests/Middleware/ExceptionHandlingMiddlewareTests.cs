using CardiacPatientMonitoring.Api.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace CardiacPatientMonitoring.Api.Tests.Middleware;

public class ExceptionHandlingMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_ReturnsSafeProblemDetails_WhenRequestFails()
    {
        const string internalMessage =
            "Internal database details must stay private.";

        RequestDelegate next = _ =>
            throw new InvalidOperationException(internalMessage);

        var logger =
            new Mock<ILogger<ExceptionHandlingMiddleware>>();

        var middleware = new ExceptionHandlingMiddleware(
            next,
            logger.Object);

        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        await middleware.InvokeAsync(context);

        context.Response.Body.Position = 0;

        using var reader = new StreamReader(
            context.Response.Body);

        var responseBody = await reader.ReadToEndAsync();

        Assert.Equal(
            StatusCodes.Status500InternalServerError,
            context.Response.StatusCode);
        Assert.StartsWith(
            "application/problem+json",
            context.Response.ContentType);
        Assert.Contains(
            "Please try again later.",
            responseBody);
        Assert.DoesNotContain(
            internalMessage,
            responseBody);
    }
}
