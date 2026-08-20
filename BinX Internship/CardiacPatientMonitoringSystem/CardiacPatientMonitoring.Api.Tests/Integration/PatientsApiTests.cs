using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using CardiacPatientMonitoring.Api.DTOs;
using CardiacPatientMonitoring.Api.Models;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CardiacPatientMonitoring.Api.Tests.Integration;

public class PatientsApiTests
    : IClassFixture<CustomWebApplicationFactory>, IDisposable
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public PatientsApiTests(
        CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient(
            new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("https://localhost")
            });
    }

    [Fact]
    public async Task GetById_ReturnsFullPatient_WhenPatientExistsAndJwtIsValid()
    {
        // Arrange
        var patient = new Patient
        {
            Id = 101,
            MedicalRecordNumber = "MRN-INTEGRATION-101",
            FullName = "Integration Test Patient",
            DateOfBirth = new DateOnly(1985, 7, 20),
            PhoneNumber = "059-000-0101",
            Diagnosis = "Stable angina"
        };

        var expected = new PatientResponse(
            patient.Id,
            patient.MedicalRecordNumber,
            patient.FullName,
            patient.DateOfBirth,
            patient.PhoneNumber,
            patient.Diagnosis);

        await _factory.ResetDatabaseAsync(patient);

        using var request = CreateAuthenticatedRequest(
            $"/api/v1/patients/{patient.Id}");

        // Act
        using var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content
            .ReadFromJsonAsync<PatientResponse>();

        Assert.NotNull(result);
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenPatientDoesNotExist()
    {
        // Arrange
        const int missingPatientId = 99999;
        await _factory.ResetDatabaseAsync();

        using var request = CreateAuthenticatedRequest(
            $"/api/v1/patients/{missingPatientId}");

        // Act
        using var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    public void Dispose()
    {
        _client.Dispose();
        GC.SuppressFinalize(this);
    }

    private static HttpRequestMessage CreateAuthenticatedRequest(
        string requestUri)
    {
        var request = new HttpRequestMessage(
            HttpMethod.Get,
            requestUri);

        request.Headers.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                TestJwtTokenFactory.CreateToken());

        return request;
    }
}
