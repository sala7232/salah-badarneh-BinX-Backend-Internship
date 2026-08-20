using CardiacPatientMonitoring.Api.Models;

namespace CardiacPatientMonitoring.Api.Repositories;

public interface IPatientRepository
{
    Task<IReadOnlyList<Patient>> GetAllAsync(string? search);

    Task<Patient?> GetByIdAsync(int id);

    Task<Patient?> GetTrackedByIdAsync(int id);

    Task<bool> MedicalRecordNumberExistsAsync(
        string normalizedMedicalRecordNumber,
        int? excludedPatientId = null);

    void Add(Patient patient);

    void Remove(Patient patient);

    Task SaveChangesAsync();
}
