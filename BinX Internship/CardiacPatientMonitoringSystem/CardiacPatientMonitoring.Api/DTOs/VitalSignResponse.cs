namespace CardiacPatientMonitoring.Api.DTOs;

public record VitalSignResponse(
    int Id,
    int PatientId,
    string PatientName,
    DateTime RecordedAt,
    int HeartRate,
    int SystolicBloodPressure,
    int DiastolicBloodPressure,
    decimal OxygenSaturation);
