using CardiacPatientMonitoring.Api.Models;

namespace CardiacPatientMonitoring.Api.DTOs;

public class UpdateAppointmentRequest
{
    public int PatientId { get; set; }
    public DateTime ScheduledAt { get; set; }
    public string Purpose { get; set; } = string.Empty;
    public AppointmentStatus Status { get; set; }
}
