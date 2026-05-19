using Microsoft.EntityFrameworkCore;
using SaddleHeroesAirWays.API.DTOs;
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

        public async Task<IEnumerable<BookingResponse>> GetBookingsForWeekAsync(DateTime date)
        {
            int diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
            var startOfWeek = date.AddDays(-diff).Date;
            var endOfWeek = startOfWeek.AddDays(7);

            return await _context.Booking
                .Include(b => b.Flight).ThenInclude(f => f.DepartureAirport)
                .Include(b => b.Flight).ThenInclude(f => f.ArrivalAirport)
                .Include(b => b.User)
                .Where(b => b.Flight.DepartureTime >= startOfWeek &&
                            b.Flight.DepartureTime < endOfWeek)
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

        public async Task<IEnumerable<BookingResponse>> GetBookingsForMonthAsync(DateTime date)
        {
            var startOfMonth = new DateTime(date.Year, date.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1);

            return await _context.Booking
                .Include(b => b.Flight).ThenInclude(f => f.DepartureAirport)
                .Include(b => b.Flight).ThenInclude(f => f.ArrivalAirport)
                .Include(b => b.User)
                .Where(b => b.Flight.DepartureTime >= startOfMonth &&
                            b.Flight.DepartureTime < endOfMonth)
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