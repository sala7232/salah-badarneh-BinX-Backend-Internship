using CardiacPatientMonitoring.Api.DTOs;
using CardiacPatientMonitoring.Api.Models;
using CardiacPatientMonitoring.Api.Repositories;
using CardiacPatientMonitoring.Api.Services;
using Moq;

namespace CardiacPatientMonitoring.Api.Tests.Services;

public class PatientServiceTests
{
    [Fact]
    public async Task GetByIdAsync_ReturnsMappedPatient_WhenRepositoryReturnsPatient()
    {
        // Arrange
        var patient = new Patient
        {
            Id = 7,
            MedicalRecordNumber = "MRN-2007",
            FullName = "Test Patient",
            DateOfBirth = new DateOnly(1988, 4, 12),
            PhoneNumber = "059-000-2007",
            Diagnosis = "Hypertension"
        };

        var expected = new PatientResponse(
            patient.Id,
            patient.MedicalRecordNumber,
            patient.FullName,
            patient.DateOfBirth,
            patient.PhoneNumber,
            patient.Diagnosis);

        var repository = new Mock<IPatientRepository>();
        repository
            .Setup(patientRepository =>
                patientRepository.GetByIdAsync(patient.Id))
            .ReturnsAsync(patient);

        var service = new PatientService(repository.Object);

        // Act
        var result = await service.GetByIdAsync(patient.Id);

        // Assert
        Assert.Equal(expected, result);
        repository.Verify(
            patientRepository =>
                patientRepository.GetByIdAsync(patient.Id),
            Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_PropagatesException_WhenRepositoryThrows()
    {
        // Arrange
        const int patientId = 7;
        var expectedException = new InvalidOperationException(
            "Database is unavailable.");

        var repository = new Mock<IPatientRepository>();
        repository
            .Setup(patientRepository =>
                patientRepository.GetByIdAsync(patientId))
            .ThrowsAsync(expectedException);

        var service = new PatientService(repository.Object);

        // Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.GetByIdAsync(patientId));

        // Assert
        Assert.Same(expectedException, exception);
    }

    [Fact]
    public async Task MedicalRecordNumberExistsAsync_NormalizesNumber_BeforeRepositoryCheck()
    {
        // Arrange
        const string normalizedMedicalRecordNumber = "MRN-100";

        var repository = new Mock<IPatientRepository>();
        repository
            .Setup(patientRepository =>
                patientRepository.MedicalRecordNumberExistsAsync(
                    normalizedMedicalRecordNumber,
                    null))
            .ReturnsAsync(true);

        var service = new PatientService(repository.Object);

        // Act
        var result = await service.MedicalRecordNumberExistsAsync(
            "  mrn-100  ");

        // Assert
        Assert.True(result);
        repository.Verify(
            patientRepository =>
                patientRepository.MedicalRecordNumberExistsAsync(
                    normalizedMedicalRecordNumber,
                    null),
            Times.Once);
    }
}
