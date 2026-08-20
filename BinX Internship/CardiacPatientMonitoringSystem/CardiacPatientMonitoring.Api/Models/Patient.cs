namespace CardiacPatientMonitoring.Api.Models;

public class Patient
{
    public int Id { get; set; }
    public string MedicalRecordNumber { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string Diagnosis { get; set; } = string.Empty;

    public ICollection<VitalSign> VitalSigns { get; set; }
        = new List<VitalSign>();

    public ICollection<Medication> Medications { get; set; }
        = new List<Medication>();

    public ICollection<Appointment> Appointments { get; set; }
        = new List<Appointment>();
}
