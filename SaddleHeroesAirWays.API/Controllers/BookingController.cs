using Microsoft.AspNetCore.Mvc;
using SaddleHeroesAirWays.API.DTOs;
using SaddleHeroesAirWays.API.Services.Interfaces;

namespace SaddleHeroesAirWays.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet("{id}", Name = "GetBookingsById")]
        public async Task<ActionResult<IEnumerable<BookingResponse>>> GetBookingsByUserId(int id)
        {
            var booking = await _bookingService.GetBookingsByUserIdAsync(id);

            if (!booking.Any())
            {
                return NotFound($"Användare med id {id} har inga bokningar.");
            }

            return Ok(booking);
        }
    }
}
