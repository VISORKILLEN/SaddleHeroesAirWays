using Microsoft.AspNetCore.Mvc;
using SaddleHeroesAirWays.API.Services.Interfaces;
using SaddleHeroesAirWays.API.Services;

namespace SaddleHeroesAirWays.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightController(IFlightService flightService) : ControllerBase
    {
        private readonly IFlightService _flightService = flightService;

        [HttpGet("weekly")]
        public IActionResult GetWeeklyFlights(DateTime date)
        {
            var flights = _flightService.GetFlightsForWeek(date);

            return Ok(flights);
        }
    }
}
