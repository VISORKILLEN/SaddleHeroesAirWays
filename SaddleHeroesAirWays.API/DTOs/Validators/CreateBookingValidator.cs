using FluentValidation;

namespace SaddleHeroesAirWays.API.DTOs.Validators
{
    public class CreateBookingValidator : AbstractValidator<CreateBookingRequest>
    {
        public CreateBookingValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("UserId måste vara ett positivt tal.");

            RuleFor(x => x.FlightId)
                .GreaterThan(0).WithMessage("FlightId måste vara ett positivt tal.");
        }
    }
}
