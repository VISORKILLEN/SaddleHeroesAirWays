using Microsoft.EntityFrameworkCore;
using SaddleHeroesAirWays.API.Services.Interfaces;
using SaddleHeroesAirWays.Library.Models;

namespace SaddleHeroesAirWays.API.Services
{
    public class BookingService : IBookingService
    {
        private readonly DbContextAPI _context;

        public BookingService(DbContextAPI context)
        {
            _context = context;
        }

        public async Task<List<Booking>> GetBookingsForMonthAsync(DateTime date)
        {
            var startOfMonth = new DateTime(date.Year, date.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1);

            return await _context.Booking
                .Where(b => b.BookingDate >= startOfMonth &&
                            b.BookingDate < endOfMonth)
                .ToListAsync();
        }
    }
}