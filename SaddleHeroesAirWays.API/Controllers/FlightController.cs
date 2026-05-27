using Microsoft.AspNetCore.Mvc;
using SaddleHeroesAirWays.API.Services.Interfaces;

namespace SaddleHeroesAirWays.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightController(IFlightService flightService) : ControllerBase
    {
        private readonly IFlightService _flightService = flightService;

        [HttpGet("search")]
        public async Task<IActionResult> SearchAvailableFlights(
            [FromQuery] DateTime date,
            [FromQuery] string? city = null)
        {
            var flights = await _flightService.SearchAvailableFlightsAsync(date, city);

            if (!flights.Any())
            {
                return NotFound("Inga lediga flyg hittades.");
            }

            return Ok(flights);
        }
    }
}
