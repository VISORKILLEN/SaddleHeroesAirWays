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
                    b.BookingDetails.Select(bd => new BookingDetailsResponse(
                        bd.Id,
                        bd.Seatnumber,
                        bd.Baggage,
                        bd.Notes
                        ))
                ))
                .ToListAsync();
        }

        public async Task<IEnumerable<BookingResponse>> GetBookingsForMonthAsync(DateTime date)
        {
            var startOfMonth = new DateTime(date.Year, date.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1);

            return await _context.Booking
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
                    b.BookingDetails.Select(bd => new BookingDetailsResponse(
                        bd.Id,
                        bd.Seatnumber,
                        bd.Baggage,
                        bd.Notes
                        ))
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
                    b.BookingDetails.Select(bd => new BookingDetailsResponse(
                        bd.Id, 
                        bd.Seatnumber,
                        bd.Baggage,
                        bd.Notes
                        ))
                ))
                .ToListAsync();
        }

        public async Task<BookingResponse> CreateBookingAsync(CreateBookingRequest bookingRequest)
        {
            var flight = await _context.Flight.FindAsync(bookingRequest.FlightId);
            
            if(flight == null)
            {
                return null;
            }

            var count = await _context.Booking.CountAsync();
            
            var bookingToAdd = new Booking
            {
                UserId = bookingRequest.UserId,
                FlightId = bookingRequest.FlightId,
                BookingDate = DateTime.Now,
                BookingStatus = BookingStatus.Confirmed,
                TotalPrice = flight.Price,
                BookingReference = $"BKG-{1000 + count + 1}"
            };

            await _context.AddAsync(bookingToAdd);
            await _context.SaveChangesAsync();

            var createdBooking = await _context.Booking
                .Where(b => b.BookingReference == bookingToAdd.BookingReference)
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
                    b.BookingDetails.Select(bd => new BookingDetailsResponse(
                        bd.Id,
                        bd.Seatnumber,
                        bd.Baggage,
                        bd.Notes
                        ))
                ))
                .FirstOrDefaultAsync();

            return createdBooking;
        }

    }
}