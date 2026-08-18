namespace CardiacPatientMonitoring.Api.DTOs;

public class UpdatePatientRequest
{
    public string MedicalRecordNumber { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string Diagnosis { get; set; } = string.Empty;
}
