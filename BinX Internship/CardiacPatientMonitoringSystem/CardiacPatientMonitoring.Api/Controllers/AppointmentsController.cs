using CardiacPatientMonitoring.Api.Data;
using CardiacPatientMonitoring.Api.DTOs;
using CardiacPatientMonitoring.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CardiacPatientMonitoring.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/appointments")]
public class AppointmentsController : ControllerBase
{
    private readonly CardiacDbContext _context;

    public AppointmentsController(CardiacDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppointmentResponse>>> GetAll(
        [FromQuery] int? patientId,
        [FromQuery] AppointmentStatus? status)
    {
        if (patientId.HasValue && patientId.Value <= 0)
        {
            return BadRequest(new
            {
                message = "Patient ID must be greater than 0."
            });
        }

        IQueryable<Appointment> query =
            _context.Appointments.AsNoTracking();

        if (patientId.HasValue)
        {
            query = query.Where(appointment =>
                appointment.PatientId == patientId.Value);
        }

        if (status.HasValue)
        {
            query = query.Where(appointment =>
                appointment.Status == status.Value);
        }

        var appointments = await query
            .OrderBy(appointment => appointment.ScheduledAt)
            .Select(appointment => new AppointmentResponse(
                appointment.Id,
                appointment.PatientId,
                appointment.Patient.FullName,
                appointment.ScheduledAt,
                appointment.Purpose,
                appointment.Status))
            .ToListAsync();

        return Ok(appointments);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AppointmentResponse>> GetById(int id)
    {
        var appointment = await _context.Appointments
            .AsNoTracking()
            .Where(appointment => appointment.Id == id)
            .Select(appointment => new AppointmentResponse(
                appointment.Id,
                appointment.PatientId,
                appointment.Patient.FullName,
                appointment.ScheduledAt,
                appointment.Purpose,
                appointment.Status))
            .FirstOrDefaultAsync();

        if (appointment is null)
        {
            return NotFound(new
            {
                message = $"Appointment with ID {id} was not found."
            });
        }

        return Ok(appointment);
    }

    [HttpPost]
    public async Task<ActionResult<AppointmentResponse>> Create(
        CreateAppointmentRequest request)
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

        var appointment = new Appointment
        {
            PatientId = request.PatientId,
            ScheduledAt = request.ScheduledAt,
            Purpose = request.Purpose.Trim(),
            Status = request.Status
        };

        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync();

        var response = new AppointmentResponse(
            appointment.Id,
            appointment.PatientId,
            patient.FullName,
            appointment.ScheduledAt,
            appointment.Purpose,
            appointment.Status);

        return CreatedAtAction(
            nameof(GetById),
            new { id = appointment.Id },
            response);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<AppointmentResponse>> Update(
        int id,
        UpdateAppointmentRequest request)
    {
        var appointment = await _context.Appointments.FindAsync(id);

        if (appointment is null)
        {
            return NotFound(new
            {
                message = $"Appointment with ID {id} was not found."
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

        appointment.PatientId = request.PatientId;
        appointment.ScheduledAt = request.ScheduledAt;
        appointment.Purpose = request.Purpose.Trim();
        appointment.Status = request.Status;

        await _context.SaveChangesAsync();

        return Ok(new AppointmentResponse(
            appointment.Id,
            appointment.PatientId,
            patient.FullName,
            appointment.ScheduledAt,
            appointment.Purpose,
            appointment.Status));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var appointment = await _context.Appointments.FindAsync(id);

        if (appointment is null)
        {
            return NotFound(new
            {
                message = $"Appointment with ID {id} was not found."
            });
        }

        _context.Appointments.Remove(appointment);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
