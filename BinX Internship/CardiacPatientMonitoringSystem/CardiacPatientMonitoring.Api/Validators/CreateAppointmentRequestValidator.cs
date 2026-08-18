using CardiacPatientMonitoring.Api.DTOs;
using FluentValidation;

namespace CardiacPatientMonitoring.Api.Validators;

public class CreateAppointmentRequestValidator
    : AbstractValidator<CreateAppointmentRequest>
{
    public CreateAppointmentRequestValidator()
    {
        RuleFor(request => request.PatientId)
            .GreaterThan(0)
            .WithMessage("Patient ID must be greater than 0.");

        RuleFor(request => request.ScheduledAt)
            .NotEmpty()
            .WithMessage("Scheduled time is required.");

        RuleFor(request => request.Purpose)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Appointment purpose is required.")
            .MaximumLength(200)
            .WithMessage(
                "Appointment purpose must not exceed 200 characters.");

        RuleFor(request => request.Status)
            .IsInEnum()
            .WithMessage("Appointment status is invalid.");
    }
}
