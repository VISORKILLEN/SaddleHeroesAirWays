using Microsoft.AspNetCore.Mvc;
using SaddleHeroesAirWays.API.Services.Interfaces;

namespace SaddleHeroesAirWays.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController(IBookingService bookingService) : ControllerBase
    {
        private readonly IBookingService _bookingService = bookingService;

        // Get bookings for a specific week based on the provided date
        [HttpGet("weekly")]
        public async Task<IActionResult> GetWeeklyBookings(DateTime date)
        {
            var bookings = await _bookingService.GetBookingsForWeekAsync(date);

            return Ok(bookings);
        }
        // Get bookings for a specific month based on the provided date
        [HttpGet("monthly")]
        public async Task<IActionResult> GetMonthlyBookings(DateTime date)
        {
            var bookings = await _bookingService.GetBookingsForMonthAsync(date);

            return Ok(bookings);
        }
    }
}