using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using CardiacPatientMonitoring.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace CardiacPatientMonitoring.Api.Tests.Integration;

public class GlobalExceptionHandlingTests
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public GlobalExceptionHandlingTests(
        CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task UnhandledException_ReturnsSafeProblemDetails()
    {
        // Arrange
        const int patientId = 500;
        const string sensitiveExceptionMessage =
            "Sensitive database connection details.";

        var patientService = new Mock<IPatientService>();
        patientService
            .Setup(service => service.GetByIdAsync(patientId))
            .ThrowsAsync(new InvalidOperationException(
                sensitiveExceptionMessage));

        using var testFactory = _factory.WithWebHostBuilder(
            builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.RemoveAll<IPatientService>();
                    services.AddScoped<IPatientService>(
                        _ => patientService.Object);
                });
            });

        using var client = testFactory.CreateClient(
            new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("https://localhost")
            });

        using var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"/api/v1/patients/{patientId}");

        request.Headers.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                TestJwtTokenFactory.CreateToken());

        // Act
        using var response = await client.SendAsync(request);
        var responseBody = await response.Content
            .ReadAsStringAsync();

        // Assert
        Assert.Equal(
            HttpStatusCode.InternalServerError,
            response.StatusCode);
        Assert.Equal(
            "application/problem+json",
            response.Content.Headers.ContentType?.MediaType);

        using var document = JsonDocument.Parse(responseBody);
        var problem = document.RootElement;

        Assert.Equal(
            "An unexpected error occurred.",
            problem.GetProperty("title").GetString());
        Assert.Equal(
            StatusCodes.Status500InternalServerError,
            problem.GetProperty("status").GetInt32());
        Assert.Equal(
            "The server could not complete the request.",
            problem.GetProperty("detail").GetString());
        Assert.Equal(
            $"/api/v1/patients/{patientId}",
            problem.GetProperty("instance").GetString());
        Assert.False(string.IsNullOrWhiteSpace(
            problem.GetProperty("traceId").GetString()));

        Assert.DoesNotContain(
            sensitiveExceptionMessage,
            responseBody);
        Assert.DoesNotContain(
            nameof(InvalidOperationException),
            responseBody);
        Assert.False(responseBody.Contains(
            "stackTrace",
            StringComparison.OrdinalIgnoreCase));
    }
}
