using FluentValidation;

namespace SaddleHeroesAirWays.API.DTOs.Validators
{
    public class CreateBookingValidator : AbstractValidator<CreateBookingRequest>
    {
        public CreateBookingValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("UserId must be a positive number");

            RuleFor(x => x.FlightId)
                .GreaterThan(0).WithMessage("FlightId must be a positive number");
        }
    }
}
