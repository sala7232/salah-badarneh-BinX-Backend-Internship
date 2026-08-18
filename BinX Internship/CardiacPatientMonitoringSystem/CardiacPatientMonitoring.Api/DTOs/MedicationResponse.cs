namespace CardiacPatientMonitoring.Api.DTOs;

public record MedicationResponse(
    int Id,
    int PatientId,
    string PatientName,
    string Name,
    string Dosage,
    string Frequency,
    DateOnly StartDate,
    DateOnly? EndDate);
