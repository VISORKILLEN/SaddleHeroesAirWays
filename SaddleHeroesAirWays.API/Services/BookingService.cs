using Microsoft.EntityFrameworkCore;
using SaddleHeroesAirWays.API.DTOs;
using SaddleHeroesAirWays.API.Services.Interfaces;

namespace SaddleHeroesAirWays.API.Services
{
    public class BookingService : IBookingService
    {
        private readonly DbContextAPI _context;
        public BookingService(DbContextAPI context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BookingResponse>> GetBookingsByUserIdAsync(int userId)
        {
            return await _context.Booking
                .Where(b => b.UserId == userId)
                .Select(b => new BookingResponse(
                    b.BookingReference,
                    b.User.Firstname,
                    b.User.Lastname,
                    b.Flight.FlightNumber,
                    b.Flight.DepartureAirport.Name,
                    b.Flight.ArrivalAirport.Name,
                    b.Flight.DepartureTime,
                    b.BookingDate,
                    b.TotalPrice,
                    b.BookingStatus.ToString(),
                    null
                    ))
                .ToListAsync();
        }
    }
}
