using CardiacPatientMonitoring.Api.Data;
using CardiacPatientMonitoring.Api.DTOs;
using CardiacPatientMonitoring.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CardiacPatientMonitoring.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/medications")]
public class MedicationsController : ControllerBase
{
    private readonly CardiacDbContext _context;

    public MedicationsController(CardiacDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MedicationResponse>>> GetAll(
        [FromQuery] int? patientId,
        [FromQuery] string? search)
    {
        if (patientId.HasValue && patientId.Value <= 0)
        {
            return BadRequest(new
            {
                message = "Patient ID must be greater than 0."
            });
        }

        IQueryable<Medication> query =
            _context.Medications.AsNoTracking();

        if (patientId.HasValue)
        {
            query = query.Where(medication =>
                medication.PatientId == patientId.Value);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchValue = search.Trim();

            query = query.Where(medication =>
                medication.Name.Contains(searchValue));
        }

        var medications = await query
            .OrderBy(medication => medication.Name)
            .Select(medication => new MedicationResponse(
                medication.Id,
                medication.PatientId,
                medication.Patient.FullName,
                medication.Name,
                medication.Dosage,
                medication.Frequency,
                medication.StartDate,
                medication.EndDate))
            .ToListAsync();

        return Ok(medications);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MedicationResponse>> GetById(int id)
    {
        var medication = await _context.Medications
            .AsNoTracking()
            .Where(medication => medication.Id == id)
            .Select(medication => new MedicationResponse(
                medication.Id,
                medication.PatientId,
                medication.Patient.FullName,
                medication.Name,
                medication.Dosage,
                medication.Frequency,
                medication.StartDate,
                medication.EndDate))
            .FirstOrDefaultAsync();

        if (medication is null)
        {
            return NotFound(new
            {
                message = $"Medication with ID {id} was not found."
            });
        }

        return Ok(medication);
    }

    [HttpPost]
    public async Task<ActionResult<MedicationResponse>> Create(
        CreateMedicationRequest request)
    {
        var patient = await _context.Patients
            .AsNoTracking()
            .FirstOrDefaultAsync(patient =>
                patient.Id == request.PatientId);

        if (patient is null)
        {
            return BadRequest(new
            {
                message =
                    $"Patient with ID {request.PatientId} does not exist."
            });
        }

        var medication = new Medication
        {
            PatientId = request.PatientId,
            Name = request.Name.Trim(),
            Dosage = request.Dosage.Trim(),
            Frequency = request.Frequency.Trim(),
            StartDate = request.StartDate,
            EndDate = request.EndDate
        };

        _context.Medications.Add(medication);
        await _context.SaveChangesAsync();

        var response = new MedicationResponse(
            medication.Id,
            medication.PatientId,
            patient.FullName,
            medication.Name,
            medication.Dosage,
            medication.Frequency,
            medication.StartDate,
            medication.EndDate);

        return CreatedAtAction(
            nameof(GetById),
            new { id = medication.Id },
            response);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<MedicationResponse>> Update(
        int id,
        UpdateMedicationRequest request)
    {
        var medication = await _context.Medications.FindAsync(id);

        if (medication is null)
        {
            return NotFound(new
            {
                message = $"Medication with ID {id} was not found."
            });
        }

        var patient = await _context.Patients
            .AsNoTracking()
            .FirstOrDefaultAsync(patient =>
                patient.Id == request.PatientId);

        if (patient is null)
        {
            return BadRequest(new
            {
                message =
                    $"Patient with ID {request.PatientId} does not exist."
            });
        }

        medication.PatientId = request.PatientId;
        medication.Name = request.Name.Trim();
        medication.Dosage = request.Dosage.Trim();
        medication.Frequency = request.Frequency.Trim();
        medication.StartDate = request.StartDate;
        medication.EndDate = request.EndDate;

        await _context.SaveChangesAsync();

        return Ok(new MedicationResponse(
            medication.Id,
            medication.PatientId,
            patient.FullName,
            medication.Name,
            medication.Dosage,
            medication.Frequency,
            medication.StartDate,
            medication.EndDate));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var medication = await _context.Medications.FindAsync(id);

        if (medication is null)
        {
            return NotFound(new
            {
                message = $"Medication with ID {id} was not found."
            });
        }

        _context.Medications.Remove(medication);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
