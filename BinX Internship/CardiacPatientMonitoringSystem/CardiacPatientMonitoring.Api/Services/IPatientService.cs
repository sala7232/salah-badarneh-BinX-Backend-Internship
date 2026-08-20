using CardiacPatientMonitoring.Api.DTOs;

namespace CardiacPatientMonitoring.Api.Services;

public interface IPatientService
{
    Task<IReadOnlyList<PatientResponse>> GetAllAsync(
        string? search);

    Task<PatientResponse?> GetByIdAsync(int id);

    Task<bool> MedicalRecordNumberExistsAsync(
        string medicalRecordNumber,
        int? excludedPatientId = null);

    Task<PatientResponse> CreateAsync(
        CreatePatientRequest request);

    Task<PatientResponse?> UpdateAsync(
        int id,
        UpdatePatientRequest request);

    Task<bool> DeleteAsync(int id);
}
