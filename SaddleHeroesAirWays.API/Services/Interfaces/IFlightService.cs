using SaddleHeroesAirWays.API.DTOs;
using SaddleHeroesAirWays.Library.Models;

namespace SaddleHeroesAirWays.API.Services.Interfaces
{
    public interface IFlightService
    {
        Task<IEnumerable<FlightResponse>> SearchAvailableFlightsAsync(string? city = null);
        Task<ServiceResult<FlightResponse>> GetFlightByIdAsync(int id);
    }
}
