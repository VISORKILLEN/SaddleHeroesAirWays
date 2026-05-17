using SaddleHeroesAirWays.Library.Models;

namespace SaddleHeroesAirWays.Library.Services
{
    public class FlightServices
    {
        private readonly List<Flight> _flights;

        public FlightServices(List<Flight> flights)
        {
            _flights = flights;
        }

        // This method retrieves all flights that are scheduled to depart within the week of the provided date.
        public List<Flight> GetFlightsForWeek(DateTime date)
        {
            int diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;

            var startOfWeek = date.AddDays(-1 * diff).Date;
            var endOfWeek = startOfWeek.AddDays(7);

            return _flights
                .Where(f => f.DepartureTime >= startOfWeek &&
                                  f.DepartureTime < endOfWeek)
                .ToList();
        }

    }
}
