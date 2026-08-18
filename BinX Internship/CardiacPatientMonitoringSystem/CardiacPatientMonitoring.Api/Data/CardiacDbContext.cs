using CardiacPatientMonitoring.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CardiacPatientMonitoring.Api.Data;

public class CardiacDbContext
    : IdentityDbContext<IdentityUser>
{
    public CardiacDbContext(
        DbContextOptions<CardiacDbContext> options)
        : base(options)
    {
    }

    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<VitalSign> VitalSigns => Set<VitalSign>();
    public DbSet<Medication> Medications => Set<Medication>();
    public DbSet<Appointment> Appointments => Set<Appointment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.Property(patient => patient.MedicalRecordNumber)
                .HasMaxLength(20);

            entity.Property(patient => patient.FullName)
                .HasMaxLength(100);

            entity.Property(patient => patient.PhoneNumber)
                .HasMaxLength(30);

            entity.Property(patient => patient.Diagnosis)
                .HasMaxLength(200);

            entity.HasIndex(patient => patient.MedicalRecordNumber)
                .IsUnique();
        });

        modelBuilder.Entity<VitalSign>(entity =>
        {
            entity.Property(vitalSign => vitalSign.OxygenSaturation)
                .HasPrecision(5, 2);

            entity.HasOne(vitalSign => vitalSign.Patient)
                .WithMany(patient => patient.VitalSigns)
                .HasForeignKey(vitalSign => vitalSign.PatientId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Medication>(entity =>
        {
            entity.Property(medication => medication.Name)
                .HasMaxLength(100);

            entity.Property(medication => medication.Dosage)
                .HasMaxLength(50);

            entity.Property(medication => medication.Frequency)
                .HasMaxLength(100);

            entity.HasOne(medication => medication.Patient)
                .WithMany(patient => patient.Medications)
                .HasForeignKey(medication => medication.PatientId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.Property(appointment => appointment.Purpose)
                .HasMaxLength(200);

            entity.Property(appointment => appointment.Status)
                .HasConversion<string>()
                .HasMaxLength(20);

            entity.HasOne(appointment => appointment.Patient)
                .WithMany(patient => patient.Appointments)
                .HasForeignKey(appointment => appointment.PatientId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Patient>().HasData(
            new Patient
            {
                Id = 1,
                MedicalRecordNumber = "MRN-1001",
                FullName = "Sample Patient One",
                DateOfBirth = new DateOnly(1975, 5, 14),
                PhoneNumber = "000-000-1001",
                Diagnosis = "Hypertension"
            },
            new Patient
            {
                Id = 2,
                MedicalRecordNumber = "MRN-1002",
                FullName = "Sample Patient Two",
                DateOfBirth = new DateOnly(1982, 11, 3),
                PhoneNumber = "000-000-1002",
                Diagnosis = "Arrhythmia"
            });

        modelBuilder.Entity<VitalSign>().HasData(
            new VitalSign
            {
                Id = 1,
                PatientId = 1,
                RecordedAt = new DateTime(
                    2026, 8, 1, 8, 30, 0, DateTimeKind.Utc),
                HeartRate = 78,
                SystolicBloodPressure = 128,
                DiastolicBloodPressure = 82,
                OxygenSaturation = 98.00m
            },
            new VitalSign
            {
                Id = 2,
                PatientId = 2,
                RecordedAt = new DateTime(
                    2026, 8, 1, 9, 0, 0, DateTimeKind.Utc),
                HeartRate = 92,
                SystolicBloodPressure = 135,
                DiastolicBloodPressure = 88,
                OxygenSaturation = 97.50m
            });

        modelBuilder.Entity<Medication>().HasData(
            new Medication
            {
                Id = 1,
                PatientId = 1,
                Name = "Lisinopril",
                Dosage = "10 mg",
                Frequency = "Once daily",
                StartDate = new DateOnly(2026, 7, 1)
            },
            new Medication
            {
                Id = 2,
                PatientId = 2,
                Name = "Metoprolol",
                Dosage = "25 mg",
                Frequency = "Twice daily",
                StartDate = new DateOnly(2026, 7, 10)
            });

        modelBuilder.Entity<Appointment>().HasData(
            new Appointment
            {
                Id = 1,
                PatientId = 1,
                ScheduledAt = new DateTime(
                    2026, 9, 1, 10, 0, 0, DateTimeKind.Utc),
                Purpose = "Blood pressure follow-up",
                Status = AppointmentStatus.Scheduled
            },
            new Appointment
            {
                Id = 2,
                PatientId = 2,
                ScheduledAt = new DateTime(
                    2026, 8, 5, 11, 30, 0, DateTimeKind.Utc),
                Purpose = "Heart rhythm review",
                Status = AppointmentStatus.Completed
            });
    }
}
