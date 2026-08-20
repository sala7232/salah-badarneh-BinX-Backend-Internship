namespace CardiacPatientMonitoring.Api.DTOs;

public class CreateVitalSignRequest
{
    public int PatientId { get; set; }
    public DateTime RecordedAt { get; set; }
    public int HeartRate { get; set; }
    public int SystolicBloodPressure { get; set; }
    public int DiastolicBloodPressure { get; set; }
    public decimal OxygenSaturation { get; set; }
}
