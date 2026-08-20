using CardiacPatientMonitoring.Api.Data;
using CardiacPatientMonitoring.Api.DTOs;
using CardiacPatientMonitoring.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CardiacPatientMonitoring.Api.Services;

public class PatientService : IPatientService
{
    private readonly CardiacDbContext _context;

    public PatientService(CardiacDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<PatientResponse>> GetAllAsync(
        string? search)
    {
        IQueryable<Patient> query =
            _context.Patients.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchValue = search.Trim();

            query = query.Where(patient =>
                patient.FullName.Contains(searchValue) ||
                patient.MedicalRecordNumber.Contains(searchValue));
        }

        return await query
            .OrderBy(patient => patient.Id)
            .Select(patient => new PatientResponse(
                patient.Id,
                patient.MedicalRecordNumber,
                patient.FullName,
                patient.DateOfBirth,
                patient.PhoneNumber,
                patient.Diagnosis))
            .ToListAsync();
    }

    public async Task<PatientResponse?> GetByIdAsync(int id)
    {
        return await _context.Patients
            .AsNoTracking()
            .Where(patient => patient.Id == id)
            .Select(patient => new PatientResponse(
                patient.Id,
                patient.MedicalRecordNumber,
                patient.FullName,
                patient.DateOfBirth,
                patient.PhoneNumber,
                patient.Diagnosis))
            .FirstOrDefaultAsync();
    }

    public async Task<bool> MedicalRecordNumberExistsAsync(
        string medicalRecordNumber,
        int? excludedPatientId = null)
    {
        var normalizedNumber = NormalizeMedicalRecordNumber(
            medicalRecordNumber);

        return await _context.Patients.AnyAsync(patient =>
            patient.MedicalRecordNumber == normalizedNumber &&
            (!excludedPatientId.HasValue ||
             patient.Id != excludedPatientId.Value));
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

        _context.Patients.Add(patient);
        await _context.SaveChangesAsync();

        return Map(patient);
    }

    public async Task<PatientResponse?> UpdateAsync(
        int id,
        UpdatePatientRequest request)
    {
        var patient = await _context.Patients.FindAsync(id);

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

        await _context.SaveChangesAsync();

        return Map(patient);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var patient = await _context.Patients.FindAsync(id);

        if (patient is null)
        {
            return false;
        }

        _context.Patients.Remove(patient);
        await _context.SaveChangesAsync();

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
