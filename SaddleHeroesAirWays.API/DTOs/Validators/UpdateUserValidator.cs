// DTOs/Validators/UpdateUserValidator.cs
using FluentValidation;

namespace SaddleHeroesAirWays.API.DTOs.Validators
{
    public class UpdateUserValidator : AbstractValidator<UpdateUser>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Invalid email format.")
                .When(x => !string.IsNullOrWhiteSpace(x.Email));

            RuleFor(x => x.Phonenumber)
                .Matches(@"^\+?[0-9\s\-]{7,15}$").WithMessage("Invalid phone number format.")
                .When(x => !string.IsNullOrWhiteSpace(x.Phonenumber));
        }
    }
}