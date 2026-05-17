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

        public List<Flight> GetFlightsForWeek(DateTime date)
        {
            var startOfWeek = date.AddDays(-(int)date.DayOfWeek + 1);
            var endOfWeek = startOfWeek.AddDays(7);

            return _flights
                .Where(f => f.DepartureTime >= startOfWeek &&
                f.DepartureTime < endOfWeek)
                .ToList();
        }

    }
}
