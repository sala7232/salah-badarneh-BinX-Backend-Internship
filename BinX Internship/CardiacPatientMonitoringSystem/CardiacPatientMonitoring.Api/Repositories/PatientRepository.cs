using CardiacPatientMonitoring.Api.Data;
using CardiacPatientMonitoring.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CardiacPatientMonitoring.Api.Repositories;

public class PatientRepository : IPatientRepository
{
    private readonly CardiacDbContext _context;

    public PatientRepository(CardiacDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Patient>> GetAllAsync(
        string? search)
    {
        IQueryable<Patient> query =
            _context.Patients.AsNoTracking();

        if (search is not null)
        {
            query = query.Where(patient =>
                patient.FullName.Contains(search) ||
                patient.MedicalRecordNumber.Contains(search));
        }

        return await query
            .OrderBy(patient => patient.Id)
            .ToListAsync();
    }

    public async Task<Patient?> GetByIdAsync(int id)
    {
        return await _context.Patients
            .AsNoTracking()
            .FirstOrDefaultAsync(patient => patient.Id == id);
    }

    public async Task<Patient?> GetTrackedByIdAsync(int id)
    {
        return await _context.Patients.FindAsync(id);
    }

    public async Task<bool> MedicalRecordNumberExistsAsync(
        string normalizedMedicalRecordNumber,
        int? excludedPatientId = null)
    {
        return await _context.Patients.AnyAsync(patient =>
            patient.MedicalRecordNumber ==
                normalizedMedicalRecordNumber &&
            (!excludedPatientId.HasValue ||
             patient.Id != excludedPatientId.Value));
    }

    public void Add(Patient patient)
    {
        _context.Patients.Add(patient);
    }

    public void Remove(Patient patient)
    {
        _context.Patients.Remove(patient);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
