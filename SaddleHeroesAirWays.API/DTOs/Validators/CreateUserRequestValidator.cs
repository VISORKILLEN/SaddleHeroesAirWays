using FluentValidation;
using SaddleHeroesAirWays.API.DTOs;

public class CreateUserRequestValidator : AbstractValidator<CreateUser>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.Firstname)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50);

        RuleFor(x => x.Lastname)
            .NotEmpty().WithMessage("Last name is required.");

        RuleFor(x => x.Gender)
            .NotEmpty().WithMessage("Gender is required");

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress().WithMessage("A valid email is required.");

        RuleFor(x => x.Phonenumber)
            .NotEmpty().WithMessage("a phonenumber is required");

        RuleFor(x => x.SocialSecurityNumber)
            .NotEmpty()
            .Length(10, 12).WithMessage("SSN must be valid.");

    }
}