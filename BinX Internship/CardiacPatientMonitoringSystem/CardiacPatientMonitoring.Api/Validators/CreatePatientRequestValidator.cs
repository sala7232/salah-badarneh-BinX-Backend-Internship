using CardiacPatientMonitoring.Api.DTOs;
using FluentValidation;

namespace CardiacPatientMonitoring.Api.Validators;

public class CreatePatientRequestValidator
    : AbstractValidator<CreatePatientRequest>
{
    public CreatePatientRequestValidator()
    {
        RuleFor(request => request.MedicalRecordNumber)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Medical record number is required.")
            .MaximumLength(20)
            .WithMessage(
                "Medical record number must not exceed 20 characters.")
            .Matches("^[A-Za-z0-9-]+$")
            .WithMessage(
                "Medical record number may contain letters, numbers, and hyphens only.");

        RuleFor(request => request.FullName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Full name is required.")
            .MaximumLength(100)
            .WithMessage("Full name must not exceed 100 characters.");

        RuleFor(request => request.DateOfBirth)
            .NotEqual(default(DateOnly))
            .WithMessage("Date of birth is required.")
            .LessThan(DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("Date of birth must be in the past.");

        RuleFor(request => request.PhoneNumber)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Phone number is required.")
            .MaximumLength(30)
            .WithMessage("Phone number must not exceed 30 characters.");

        RuleFor(request => request.Diagnosis)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Diagnosis is required.")
            .MaximumLength(200)
            .WithMessage("Diagnosis must not exceed 200 characters.");
    }
}
