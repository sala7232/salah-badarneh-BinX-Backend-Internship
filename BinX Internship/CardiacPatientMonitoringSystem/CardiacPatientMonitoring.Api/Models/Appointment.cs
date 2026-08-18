namespace CardiacPatientMonitoring.Api.Models;

public class Appointment
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public DateTime ScheduledAt { get; set; }
    public string Purpose { get; set; } = string.Empty;
    public AppointmentStatus Status { get; set; }

    public Patient Patient { get; set; } = null!;
}
