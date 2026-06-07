using Microsoft.AspNetCore.Mvc;
using SaddleHeroesAirWays.API.DTOs;
using SaddleHeroesAirWays.API.Services;
using SaddleHeroesAirWays.API.Services.Interfaces;

namespace SaddleHeroesAirWays.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightController(IFlightService flightService) : ControllerBase
    {
        private readonly IFlightService _flightService = flightService;

        // GET: api/flight/search?city=Stockholm
        [HttpGet("search")]
        public async Task<IActionResult> SearchAvailableFlights([FromQuery] string? city = null)
        {
            var flights = await _flightService.SearchAvailableFlightsAsync(city);

            if (!flights.Any())
            {
                return NotFound("No available flights were found.");
            }

            return Ok(flights);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FlightResponse>> GetFlightById(int id)
        {
            var flight = await _flightService.GetFlightByIdAsync(id);

            if (!flight.Success)
            {
                return flight.Status switch
                {
                    ServiceResultStatus.NotFound => NotFound(flight.ErrorMessage),
                    _ => StatusCode(500, flight.ErrorMessage)
                };
            }

            return Ok(flight.Data);
        }
    }
}
