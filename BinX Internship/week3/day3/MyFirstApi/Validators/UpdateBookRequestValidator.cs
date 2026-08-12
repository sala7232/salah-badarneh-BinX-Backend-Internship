using FluentValidation;
using MyFirstApi.DTOs;

namespace MyFirstApi.Validators;

public class UpdateBookRequestValidator
    : AbstractValidator<UpdateBookRequest>
{
    public UpdateBookRequestValidator()
    {
        RuleFor(request => request.Title)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Book title is required.")
            .MaximumLength(250)
            .WithMessage(
                "Book title must not exceed 250 characters.");

        var currentYear = DateTime.UtcNow.Year;

        RuleFor(request => request.PublishedYear)
            .InclusiveBetween(
                (short)1000,
                (short)currentYear)
            .WithMessage(
                $"Published year must be between 1000 and {currentYear}.");

        RuleFor(request => request.AuthorId)
            .GreaterThan(0)
            .WithMessage(
                "Author ID must be greater than 0.");
    }
}