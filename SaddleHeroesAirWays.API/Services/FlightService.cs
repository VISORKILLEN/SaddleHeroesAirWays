using Microsoft.EntityFrameworkCore;
using SaddleHeroesAirWays.API.DTOs;
using SaddleHeroesAirWays.API.Services.Interfaces;
using SaddleHeroesAirWays.Library.Models;

namespace SaddleHeroesAirWays.API.Services
{
    public class FlightService : IFlightService
    {
        private readonly DbContextAPI _context;

        public FlightService(DbContextAPI context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FlightResponse>> SearchAvailableFlightsAsync(string? city = null)
        {
            var query = _context.Flight
                .Where(f => f.Bookings.Count < f.TotalSeats);

            if (!string.IsNullOrWhiteSpace(city))
                query = query.Where(f => f.DepartureAirport.City == city ||
                                         f.ArrivalAirport.City == city);

            return await query
                .Select(f => new FlightResponse(
                    f.FlightNumber,
                    f.DepartureAirport.Name,
                    f.ArrivalAirport.Name,
                    f.DepartureTime,
                    f.ArrivalTime,
                    f.TotalSeats - f.Bookings.Count,
                    f.Price,
                    f.FlightStatus.ToString()
                ))
                .ToListAsync();
        }
    }
}