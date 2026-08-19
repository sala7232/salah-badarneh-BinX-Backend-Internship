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

    }
}
