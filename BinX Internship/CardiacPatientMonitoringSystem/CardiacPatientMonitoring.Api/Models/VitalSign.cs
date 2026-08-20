namespace CardiacPatientMonitoring.Api.Models;

public class VitalSign
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public DateTime RecordedAt { get; set; }
    public int HeartRate { get; set; }
    public int SystolicBloodPressure { get; set; }
    public int DiastolicBloodPressure { get; set; }
    public decimal OxygenSaturation { get; set; }

    public Patient Patient { get; set; } = null!;
}
