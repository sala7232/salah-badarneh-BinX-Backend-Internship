using CardiacPatientMonitoring.Api.Models;

namespace CardiacPatientMonitoring.Api.DTOs;

public record AppointmentResponse(
    int Id,
    int PatientId,
    string PatientName,
    DateTime ScheduledAt,
    string Purpose,
    AppointmentStatus Status);
