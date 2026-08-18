using CardiacPatientMonitoring.Api.Controllers;
using CardiacPatientMonitoring.Api.DTOs;
using CardiacPatientMonitoring.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CardiacPatientMonitoring.Api.Tests.Controllers;

public class PatientsControllerTests
{
    [Fact]
    public async Task GetById_ReturnsOk_WhenPatientExists()
    {
        var patient = CreatePatientResponse();
        var service = new Mock<IPatientService>();

        service
            .Setup(patientService =>
                patientService.GetByIdAsync(patient.Id))
            .ReturnsAsync(patient);

        var controller = new PatientsController(service.Object);

        var result = await controller.GetById(patient.Id);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(patient, okResult.Value);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenPatientDoesNotExist()
    {
        var service = new Mock<IPatientService>();

        service
            .Setup(patientService =>
                patientService.GetByIdAsync(999))
            .ReturnsAsync((PatientResponse?)null);

        var controller = new PatientsController(service.Object);

        var result = await controller.GetById(999);

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task Create_ReturnsCreated_WhenRequestIsValid()
    {
        var request = CreatePatientRequest();
        var patient = CreatePatientResponse();
        var service = new Mock<IPatientService>();

        service
            .Setup(patientService =>
                patientService.MedicalRecordNumberExistsAsync(
                    request.MedicalRecordNumber,
                    null))
            .ReturnsAsync(false);

        service
            .Setup(patientService =>
                patientService.CreateAsync(request))
            .ReturnsAsync(patient);

        var controller = new PatientsController(service.Object);

        var result = await controller.Create(request);

        var createdResult =
            Assert.IsType<CreatedAtActionResult>(result.Result);

        Assert.Equal(nameof(PatientsController.GetById),
            createdResult.ActionName);
        Assert.Equal(patient, createdResult.Value);
    }

    [Fact]
    public async Task Create_ReturnsConflict_WhenRecordNumberExists()
    {
        var request = CreatePatientRequest();
        var service = new Mock<IPatientService>();

        service
            .Setup(patientService =>
                patientService.MedicalRecordNumberExistsAsync(
                    request.MedicalRecordNumber,
                    null))
            .ReturnsAsync(true);

        var controller = new PatientsController(service.Object);

        var result = await controller.Create(request);

        Assert.IsType<ConflictObjectResult>(result.Result);

        service.Verify(
            patientService => patientService.CreateAsync(
                It.IsAny<CreatePatientRequest>()),
            Times.Never);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenPatientDoesNotExist()
    {
        var service = new Mock<IPatientService>();

        service
            .Setup(patientService =>
                patientService.DeleteAsync(999))
            .ReturnsAsync(false);

        var controller = new PatientsController(service.Object);

        var result = await controller.Delete(999);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    private static CreatePatientRequest CreatePatientRequest()
    {
        return new CreatePatientRequest
        {
            MedicalRecordNumber = "MRN-2001",
            FullName = "Test Patient",
            DateOfBirth = new DateOnly(1990, 1, 1),
            PhoneNumber = "000-000-2001",
            Diagnosis = "Hypertension"
        };
    }

    private static PatientResponse CreatePatientResponse()
    {
        return new PatientResponse(
            10,
            "MRN-2001",
            "Test Patient",
            new DateOnly(1990, 1, 1),
            "000-000-2001",
            "Hypertension");
    }
}
