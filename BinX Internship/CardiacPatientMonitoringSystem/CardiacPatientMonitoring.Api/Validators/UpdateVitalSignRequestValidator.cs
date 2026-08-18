using CardiacPatientMonitoring.Api.DTOs;
using FluentValidation;

namespace CardiacPatientMonitoring.Api.Validators;

public class UpdateVitalSignRequestValidator
    : AbstractValidator<UpdateVitalSignRequest>
{
    public UpdateVitalSignRequestValidator()
    {
        RuleFor(request => request.PatientId)
            .GreaterThan(0)
            .WithMessage("Patient ID must be greater than 0.");

        RuleFor(request => request.RecordedAt)
            .NotEmpty()
            .WithMessage("Recorded time is required.")
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Recorded time cannot be in the future.");

        RuleFor(request => request.HeartRate)
            .InclusiveBetween(30, 220)
            .WithMessage("Heart rate must be between 30 and 220 bpm.");

        RuleFor(request => request.SystolicBloodPressure)
            .InclusiveBetween(70, 250)
            .WithMessage(
                "Systolic blood pressure must be between 70 and 250 mmHg.");

        RuleFor(request => request.DiastolicBloodPressure)
            .InclusiveBetween(40, 150)
            .WithMessage(
                "Diastolic blood pressure must be between 40 and 150 mmHg.")
            .LessThan(request => request.SystolicBloodPressure)
            .WithMessage(
                "Diastolic blood pressure must be lower than systolic blood pressure.");

        RuleFor(request => request.OxygenSaturation)
            .InclusiveBetween(50m, 100m)
            .WithMessage(
                "Oxygen saturation must be between 50 and 100 percent.");
    }
}
