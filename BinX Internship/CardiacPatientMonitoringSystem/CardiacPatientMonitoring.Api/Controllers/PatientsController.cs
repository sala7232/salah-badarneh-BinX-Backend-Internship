using CardiacPatientMonitoring.Api.DTOs;
using CardiacPatientMonitoring.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CardiacPatientMonitoring.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/patients")]
public class PatientsController : ControllerBase
{
    private readonly IPatientService _patientService;

    public PatientsController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PatientResponse>>> GetAll(
        [FromQuery] string? search)
    {
        var patients = await _patientService.GetAllAsync(search);

        return Ok(patients);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PatientResponse>> GetById(int id)
    {
        var patient = await _patientService.GetByIdAsync(id);

        if (patient is null)
        {
            return NotFound(new
            {
                message = $"Patient with ID {id} was not found."
            });
        }

        return Ok(patient);
    }

    [HttpPost]
    public async Task<ActionResult<PatientResponse>> Create(
        CreatePatientRequest request)
    {
        if (await _patientService.MedicalRecordNumberExistsAsync(
                request.MedicalRecordNumber))
        {
            return Conflict(new
            {
                message =
                    "A patient with this medical record number already exists."
            });
        }

        var patient = await _patientService.CreateAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = patient.Id },
            patient);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<PatientResponse>> Update(
        int id,
        UpdatePatientRequest request)
    {
        if (await _patientService.GetByIdAsync(id) is null)
        {
            return NotFound(new
            {
                message = $"Patient with ID {id} was not found."
            });
        }

        if (await _patientService.MedicalRecordNumberExistsAsync(
                request.MedicalRecordNumber,
                id))
        {
            return Conflict(new
            {
                message =
                    "A patient with this medical record number already exists."
            });
        }

        var patient = await _patientService.UpdateAsync(
            id,
            request);

        if (patient is null)
        {
            return NotFound(new
            {
                message = $"Patient with ID {id} was not found."
            });
        }

        return Ok(patient);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _patientService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound(new
            {
                message = $"Patient with ID {id} was not found."
            });
        }

        return NoContent();
    }
}
