using Microsoft.AspNetCore.Mvc;
using SaddleHeroesAirWays.API.Services.Interfaces;

namespace SaddleHeroesAirWays.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightController(IFlightService flightService) : ControllerBase
    {
        private readonly IFlightService _flightService = flightService;

        [HttpGet("weekly")]
        public async Task<IActionResult> GetWeeklyFlights(DateTime date)
        {
            var flights = await _flightService.GetFlightsForWeekAsync(date);

            return Ok(flights);
        }
    }
}
