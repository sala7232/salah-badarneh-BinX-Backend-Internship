namespace CardiacPatientMonitoring.Api.DTOs;

public record PatientResponse(
    int Id,
    string MedicalRecordNumber,
    string FullName,
    DateOnly DateOfBirth,
    string PhoneNumber,
    string Diagnosis);
