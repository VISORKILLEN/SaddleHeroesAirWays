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

            // GetBookingsForWeekAsync
            return await MapBookingToResponse(
                _context.Booking.Where(b => b.Flight.DepartureTime >= startOfWeek && b.Flight.DepartureTime < endOfWeek))
                .ToListAsync();
        }

        public async Task<IEnumerable<BookingResponse>> GetBookingsForMonthAsync(DateTime date)
        {
            var startOfMonth = new DateTime(date.Year, date.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1);

            return await MapBookingToResponse(
                _context.Booking.Where(b => b.Flight.DepartureTime >= startOfMonth && b.Flight.DepartureTime < endOfMonth))
                .ToListAsync();
        }

        public async Task<ServiceResult<BookingResponse?>> GetBookingByBookingReferenceAsync(string bookingReference)
        {
            var booking = await MapBookingToResponse(
                _context.Booking.AsNoTracking().Where(b => b.BookingReference == bookingReference))
                .FirstOrDefaultAsync();

            if(booking == null)
            {
                return ServiceResult<BookingResponse>.NotFound($"Bokning {bookingReference} hittades inte.");
            }

            return ServiceResult<BookingResponse>.Ok(booking);
        }

        public async Task<IEnumerable<BookingResponse>> GetBookingsByUserIdAsync(int userId)
        {
            return await MapBookingToResponse(
                _context.Booking.AsNoTracking().Where(b => b.UserId == userId))
                .ToListAsync();
        }

        // Get all bookings made
        public async Task<IEnumerable<BookingResponse>> GetAllBookingsMadeAsync()
        {
            return await MapBookingToResponse(_context.Booking)
                .ToListAsync();
        }

        public async Task<ServiceResult<BookingResponse>> CreateBookingAsync(CreateBookingRequest bookingRequest)
        {
            var flight = await _context.Flight.FindAsync(bookingRequest.FlightId);
            
            if(flight == null)
            {
                return ServiceResult<BookingResponse>.NotFound($"Flyget finns inte.");
            }

            if(await IsFlightFullAsync(bookingRequest.FlightId, flight.TotalSeats))
            {
                return ServiceResult<BookingResponse>.ValidationError($"Flyget är fullt.");
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

            if(bookingRequest.Details != null && bookingRequest.Details.Any())
            {
                var detailsList = new List<BookingDetails>();

                foreach (var d in bookingRequest.Details)
                {
                    detailsList.Add(new BookingDetails
                    {
                        BookingReference = bookingToAdd.BookingReference,
                        Seatnumber = await GenerateSeatNumberAsync(bookingRequest.FlightId),
                        Baggage = d.Baggage,
                        Notes = d.Notes

                    });
                }
                
                await _context.BookingDetails.AddRangeAsync(detailsList);
                await _context.SaveChangesAsync();
            };

            var createdBooking = await MapBookingToResponse(
                _context.Booking.Where(b => b.BookingReference == bookingToAdd.BookingReference))
                .FirstOrDefaultAsync();

            return ServiceResult<BookingResponse>.Ok(createdBooking); 
        }

        private async Task<bool> IsFlightFullAsync(int flightId, int totalSeats)
        {
            var takenSeats = await _context.BookingDetails
                .Where(bd => bd.Booking.FlightId == flightId)
                .CountAsync();

            return takenSeats >= totalSeats;
        }

        private async Task<string> GenerateSeatNumberAsync(int flightId)
        {
            var takenSeats = await _context.BookingDetails
                .Where(bd => bd.Booking.FlightId == flightId)
                .Select(bd => bd.Seatnumber)
                .ToListAsync();

            var rows = Enumerable.Range(1,30);
            var cols = new[] { "A", "B", "C", "D", "E", "F" };

            var allSeats = rows
                .SelectMany(r => cols.Select(c => $"{r}{c}"))
                .ToList();

            var availbleSeats = allSeats.Except(takenSeats).ToList();
            var random = new Random();
            return availbleSeats[random.Next(availbleSeats.Count)];
        }

        public async Task<IEnumerable<BookingResponse>> GetBookingsForDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                throw new ArgumentException("startDate must be before or equal to endDate");
            }

            var start = startDate.Date;
            var end = endDate.Date.AddDays(1);

            return await MapBookingToResponse(
                _context.Booking.Where(b => b.Flight.DepartureTime
                >= start && b.Flight.DepartureTime < end))
                .ToListAsync();
        }

        public async Task<ServiceResult<BookingResponse>> UpdateBookingAsync(string bookingReference, UpdateBooking updateBooking)
        {
            var booking = await _context.Booking
                .FirstOrDefaultAsync(b => b.BookingReference == bookingReference);
            
            if (booking is null) 
            { 
                return ServiceResult<BookingResponse>.NotFound($"Bokning {bookingReference} hittades inte.");
            }

            var newFlightId = updateBooking.FlightId ?? booking.FlightId;
            var flight = await _context.Flight.FindAsync(newFlightId);

            if (flight is null) 
            { 
                return ServiceResult<BookingResponse>.NotFound("Flyget hittades inte.");
            } 
            
            if (flight.DepartureTime <= DateTime.Now.AddHours(1))
            {
                return ServiceResult<BookingResponse>.ValidationError("Det går inte att omboka mindre än en timme innan avgång.");
            }

            booking.FlightId = newFlightId;
            booking.BookingStatus = BookingStatus.Rebooked;

            await _context.SaveChangesAsync();

            var updatedBooking = await MapBookingToResponse(
                _context.Booking.Where(b => b.BookingReference == bookingReference))
                .FirstOrDefaultAsync();

           return ServiceResult<BookingResponse>.Ok(updatedBooking);
        }

        public async Task<ServiceResult<bool>> DeleteBookingPermanentlyAsync(string bookingReference)
        {
            var booking = await _context.Booking
                .Include(b => b.BookingDetails)
                .FirstOrDefaultAsync(b => b.BookingReference == bookingReference);

            if (booking == null)
            {
                return ServiceResult<bool>.NotFound($"Bokning {bookingReference} hittades inte.");
            }

            if (booking.BookingDetails != null && booking.BookingDetails.Any())
            {
                _context.BookingDetails.RemoveRange(booking.BookingDetails);
            }

            _context.Booking.Remove(booking);
            await _context.SaveChangesAsync();

            return ServiceResult<bool>.Ok(true);
        }

        private static IQueryable<BookingResponse> MapBookingToResponse(IQueryable<Booking> query)
        {
            return query.Select(b => new BookingResponse(
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
                ));
        }
    }
}