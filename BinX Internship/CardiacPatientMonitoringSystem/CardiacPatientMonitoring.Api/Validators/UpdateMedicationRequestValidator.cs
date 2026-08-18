using CardiacPatientMonitoring.Api.DTOs;
using FluentValidation;

namespace CardiacPatientMonitoring.Api.Validators;

public class UpdateMedicationRequestValidator
    : AbstractValidator<UpdateMedicationRequest>
{
    public UpdateMedicationRequestValidator()
    {
        RuleFor(request => request.PatientId)
            .GreaterThan(0)
            .WithMessage("Patient ID must be greater than 0.");

        RuleFor(request => request.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Medication name is required.")
            .MaximumLength(100)
            .WithMessage(
                "Medication name must not exceed 100 characters.");

        RuleFor(request => request.Dosage)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Dosage is required.")
            .MaximumLength(50)
            .WithMessage("Dosage must not exceed 50 characters.");

        RuleFor(request => request.Frequency)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Frequency is required.")
            .MaximumLength(100)
            .WithMessage("Frequency must not exceed 100 characters.");

        RuleFor(request => request.StartDate)
            .NotEqual(default(DateOnly))
            .WithMessage("Start date is required.");

        RuleFor(request => request.EndDate)
            .Must((request, endDate) =>
                !endDate.HasValue ||
                endDate.Value >= request.StartDate)
            .WithMessage("End date cannot be before start date.");
    }
}
