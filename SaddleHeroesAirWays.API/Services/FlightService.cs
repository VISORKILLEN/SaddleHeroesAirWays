using Microsoft.EntityFrameworkCore;
using SaddleHeroesAirWays.Library.Models;

namespace SaddleHeroesAirWays.API.Services
{
    public class FlightService
    {
        private readonly DbContextAPI _context;

        public FlightService(DbContextAPI context)
        {
            _context = context;
        }

        public async Task<List<Flight>> GetFlightsForWeekAsync(DateTime date)
        {
            int diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;

            var startOfWeek = date.AddDays(-diff).Date;
            var endOfWeek = startOfWeek.AddDays(7);

            return await _context.Flight
                .Where(f => f.DepartureTime >= startOfWeek &&
                            f.DepartureTime < endOfWeek)
                .ToListAsync();
        }
    }
}
