using SaddleHeroesAirWays.API.DTOs;
using SaddleHeroesAirWays.Library.Models;

namespace SaddleHeroesAirWays.API.Services.Interfaces
{
    public interface IFlightService
    {
        // IFlightService.cs
        Task<IEnumerable<FlightResponse>> SearchAvailableFlightsAsync(string? city = null);

    }
}
