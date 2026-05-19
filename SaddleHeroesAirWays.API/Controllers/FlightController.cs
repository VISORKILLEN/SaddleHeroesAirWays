using Microsoft.AspNetCore.Mvc;
using SaddleHeroesAirWays.API.Services.Interfaces;

namespace SaddleHeroesAirWays.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightController(IFlightService flightService) : ControllerBase
    {
        private readonly IFlightService _flightService = flightService;

        // Get flights for a specific week based on the provided date
        [HttpGet("weekly")]
        public IActionResult GetWeeklyFlights(DateTime date)
        {
            var flights = _flightService.GetFlightsForWeek(date);

            return Ok(flights);
        }

        // Get flights for a specific month based on the provided date
        [HttpGet("monthly")]
        public async Task<IActionResult> GetMonthlyFlights(DateTime date)
        {
            var flights = await _flightService.GetFlightsForMonthAsync(date);

            return Ok(flights);
        }
    }
}
