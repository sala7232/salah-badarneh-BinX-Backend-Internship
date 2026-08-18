namespace CardiacPatientMonitoring.Api.Models;

public class Medication
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Dosage { get; set; } = string.Empty;
    public string Frequency { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }

    public Patient Patient { get; set; } = null!;
}
