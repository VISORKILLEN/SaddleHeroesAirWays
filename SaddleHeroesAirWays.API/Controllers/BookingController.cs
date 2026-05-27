using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SaddleHeroesAirWays.API.DTOs;
using SaddleHeroesAirWays.API.Services.Interfaces;

namespace SaddleHeroesAirWays.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController(IBookingService bookingService, IValidator<CreateBookingRequest> createBookingValidator) : ControllerBase
    {
        private readonly IBookingService _bookingService = bookingService;
        private readonly IValidator<CreateBookingRequest> _createBookingValidator = createBookingValidator;

        // Get bookings for a specific week based on the provided date
        [HttpGet("weekly")]
        public async Task<ActionResult<List<BookingResponse>>> GetWeeklyBookings(DateTime date)
        {
            var bookings = await _bookingService.GetBookingsForWeekAsync(date);
            return Ok(bookings);
        }

        [HttpGet("monthly")]
        public async Task<ActionResult<List<BookingResponse>>> GetMonthlyBookings(DateTime date)
        {
            var bookings = await _bookingService.GetBookingsForMonthAsync(date);
            return Ok(bookings);
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<BookingResponse>>> GetAllBookings()
        {
            var bookings = await _bookingService.GetAllBookingsMadeAsync();
            return Ok(bookings);
        }

        [HttpGet("{id}", Name = "GetBookingsById")]
        public async Task<ActionResult<IEnumerable<BookingResponse>>> GetBookingsByUserId(int id)
        {
            if(id <= 0)
            {
                return BadRequest("Ogiltigt användar-Id.");
            }

            var booking = await _bookingService.GetBookingsByUserIdAsync(id);

            if (!booking.Any())
            {
                return NotFound($"Användare med id {id} har inga bokningar.");
            }

            return Ok(booking);
        }

        [HttpPost]
        public async Task<ActionResult<BookingResponse>> CreateBooking(CreateBookingRequest bookingRequest)
        {
            var validationResult = await _createBookingValidator.ValidateAsync(bookingRequest);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => new
                {
                    field = e.PropertyName,
                    message = e.ErrorMessage
                });

                return BadRequest(errors);
            }

            var result = await _bookingService.CreateBookingAsync(bookingRequest);
            if (result == null)
            {
                return BadRequest("Flyget hittades inte.");
            }

            return Created(string.Empty,result);
        }

        // Get bookings for a specific date range based on the provided start and end dates
        [HttpGet("by-date-range")]
        public async Task<IActionResult> GetBookingsByDateRange(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            if (startDate > endDate)
                return BadRequest("startDate must be before or equal to endDate.");

            var bookings = await _bookingService.GetBookingsForDateRangeAsync(startDate, endDate);
            return Ok(bookings);
        }
    }
}