using CardiacPatientMonitoring.Api.DTOs;
using CardiacPatientMonitoring.Api.Models;
using CardiacPatientMonitoring.Api.Repositories;

namespace CardiacPatientMonitoring.Api.Services;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepository;

    public PatientService(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }

    public async Task<IReadOnlyList<PatientResponse>> GetAllAsync(
        string? search)
    {
        var searchValue = string.IsNullOrWhiteSpace(search)
            ? null
            : search.Trim();

        var patients = await _patientRepository.GetAllAsync(
            searchValue);

        return patients.Select(Map).ToList();
    }

    public async Task<PatientResponse?> GetByIdAsync(int id)
    {
        var patient = await _patientRepository.GetByIdAsync(id);

        return patient is null ? null : Map(patient);
    }

    public async Task<bool> MedicalRecordNumberExistsAsync(
        string medicalRecordNumber,
        int? excludedPatientId = null)
    {
        var normalizedNumber = NormalizeMedicalRecordNumber(
            medicalRecordNumber);

        return await _patientRepository
            .MedicalRecordNumberExistsAsync(
                normalizedNumber,
                excludedPatientId);
    }

    public async Task<PatientResponse> CreateAsync(
        CreatePatientRequest request)
    {
        var patient = new Patient
        {
            MedicalRecordNumber = NormalizeMedicalRecordNumber(
                request.MedicalRecordNumber),
            FullName = request.FullName.Trim(),
            DateOfBirth = request.DateOfBirth,
            PhoneNumber = request.PhoneNumber.Trim(),
            Diagnosis = request.Diagnosis.Trim()
        };

        _patientRepository.Add(patient);
        await _patientRepository.SaveChangesAsync();

        return Map(patient);
    }

    public async Task<PatientResponse?> UpdateAsync(
        int id,
        UpdatePatientRequest request)
    {
        var patient = await _patientRepository
            .GetTrackedByIdAsync(id);

        if (patient is null)
        {
            return null;
        }

        patient.MedicalRecordNumber =
            NormalizeMedicalRecordNumber(
                request.MedicalRecordNumber);
        patient.FullName = request.FullName.Trim();
        patient.DateOfBirth = request.DateOfBirth;
        patient.PhoneNumber = request.PhoneNumber.Trim();
        patient.Diagnosis = request.Diagnosis.Trim();

        await _patientRepository.SaveChangesAsync();

        return Map(patient);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var patient = await _patientRepository
            .GetTrackedByIdAsync(id);

        if (patient is null)
        {
            return false;
        }

        _patientRepository.Remove(patient);
        await _patientRepository.SaveChangesAsync();

        return true;
    }

    private static string NormalizeMedicalRecordNumber(
        string medicalRecordNumber)
    {
        return medicalRecordNumber.Trim().ToUpperInvariant();
    }

    private static PatientResponse Map(Patient patient)
    {
        return new PatientResponse(
            patient.Id,
            patient.MedicalRecordNumber,
            patient.FullName,
            patient.DateOfBirth,
            patient.PhoneNumber,
            patient.Diagnosis);
    }
}
