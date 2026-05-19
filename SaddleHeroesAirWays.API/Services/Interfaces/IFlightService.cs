using SaddleHeroesAirWays.Library.Models;

namespace SaddleHeroesAirWays.API.Services.Interfaces
{
    public interface IFlightService
    {
        List<Flight> GetFlightsForWeek(DateTime date);

        Task<List<Flight>> GetFlightsForMonthAsync(DateTime date);
    }
}
