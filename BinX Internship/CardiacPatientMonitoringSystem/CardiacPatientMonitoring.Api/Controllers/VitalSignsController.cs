using CardiacPatientMonitoring.Api.Data;
using CardiacPatientMonitoring.Api.DTOs;
using CardiacPatientMonitoring.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CardiacPatientMonitoring.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/vital-signs")]
public class VitalSignsController : ControllerBase
{
    private readonly CardiacDbContext _context;

    public VitalSignsController(CardiacDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<VitalSignResponse>>> GetAll(
        [FromQuery] int? patientId)
    {
        if (patientId.HasValue && patientId.Value <= 0)
        {
            return BadRequest(new
            {
                message = "Patient ID must be greater than 0."
            });
        }

        IQueryable<VitalSign> query =
            _context.VitalSigns.AsNoTracking();

        if (patientId.HasValue)
        {
            query = query.Where(vitalSign =>
                vitalSign.PatientId == patientId.Value);
        }

        var vitalSigns = await query
            .OrderByDescending(vitalSign => vitalSign.RecordedAt)
            .Select(vitalSign => new VitalSignResponse(
                vitalSign.Id,
                vitalSign.PatientId,
                vitalSign.Patient.FullName,
                vitalSign.RecordedAt,
                vitalSign.HeartRate,
                vitalSign.SystolicBloodPressure,
                vitalSign.DiastolicBloodPressure,
                vitalSign.OxygenSaturation))
            .ToListAsync();

        return Ok(vitalSigns);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<VitalSignResponse>> GetById(int id)
    {
        var vitalSign = await _context.VitalSigns
            .AsNoTracking()
            .Where(vitalSign => vitalSign.Id == id)
            .Select(vitalSign => new VitalSignResponse(
                vitalSign.Id,
                vitalSign.PatientId,
                vitalSign.Patient.FullName,
                vitalSign.RecordedAt,
                vitalSign.HeartRate,
                vitalSign.SystolicBloodPressure,
                vitalSign.DiastolicBloodPressure,
                vitalSign.OxygenSaturation))
            .FirstOrDefaultAsync();

        if (vitalSign is null)
        {
            return NotFound(new
            {
                message = $"Vital sign with ID {id} was not found."
            });
        }

        return Ok(vitalSign);
    }

    [HttpPost]
    public async Task<ActionResult<VitalSignResponse>> Create(
        CreateVitalSignRequest request)
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

        var vitalSign = new VitalSign
        {
            PatientId = request.PatientId,
            RecordedAt = request.RecordedAt,
            HeartRate = request.HeartRate,
            SystolicBloodPressure =
                request.SystolicBloodPressure,
            DiastolicBloodPressure =
                request.DiastolicBloodPressure,
            OxygenSaturation = request.OxygenSaturation
        };

        _context.VitalSigns.Add(vitalSign);
        await _context.SaveChangesAsync();

        var response = new VitalSignResponse(
            vitalSign.Id,
            vitalSign.PatientId,
            patient.FullName,
            vitalSign.RecordedAt,
            vitalSign.HeartRate,
            vitalSign.SystolicBloodPressure,
            vitalSign.DiastolicBloodPressure,
            vitalSign.OxygenSaturation);

        return CreatedAtAction(
            nameof(GetById),
            new { id = vitalSign.Id },
            response);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<VitalSignResponse>> Update(
        int id,
        UpdateVitalSignRequest request)
    {
        var vitalSign = await _context.VitalSigns.FindAsync(id);

        if (vitalSign is null)
        {
            return NotFound(new
            {
                message = $"Vital sign with ID {id} was not found."
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

        vitalSign.PatientId = request.PatientId;
        vitalSign.RecordedAt = request.RecordedAt;
        vitalSign.HeartRate = request.HeartRate;
        vitalSign.SystolicBloodPressure =
            request.SystolicBloodPressure;
        vitalSign.DiastolicBloodPressure =
            request.DiastolicBloodPressure;
        vitalSign.OxygenSaturation = request.OxygenSaturation;

        await _context.SaveChangesAsync();

        return Ok(new VitalSignResponse(
            vitalSign.Id,
            vitalSign.PatientId,
            patient.FullName,
            vitalSign.RecordedAt,
            vitalSign.HeartRate,
            vitalSign.SystolicBloodPressure,
            vitalSign.DiastolicBloodPressure,
            vitalSign.OxygenSaturation));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var vitalSign = await _context.VitalSigns.FindAsync(id);

        if (vitalSign is null)
        {
            return NotFound(new
            {
                message = $"Vital sign with ID {id} was not found."
            });
        }

        _context.VitalSigns.Remove(vitalSign);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
