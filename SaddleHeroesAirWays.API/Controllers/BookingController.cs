using Microsoft.AspNetCore.Mvc;
using SaddleHeroesAirWays.API.Services.Interfaces;

namespace SaddleHeroesAirWays.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController(IBookingService bookingService) : ControllerBase
    {
        private readonly IBookingService _bookingService = bookingService;

        [HttpGet("monthly")]
        public async Task<IActionResult> GetMonthlyBookings(DateTime date)
        {
            var bookings = await _bookingService.GetBookingsForMonthAsync(date);

            return Ok(bookings);
        }
    }
}