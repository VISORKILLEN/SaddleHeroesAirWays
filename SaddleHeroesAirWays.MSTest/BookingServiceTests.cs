using Microsoft.EntityFrameworkCore;
using SaddleHeroesAirWays.API;
using SaddleHeroesAirWays.API.Services;
using SaddleHeroesAirWays.Library.Models;


namespace SaddleHeroesAirWays.MSTest
{
    [TestClass]
    public class BookingServiceTests
    {
        // Test for GetBookingsForWeekAsync method in BookingService
        [TestMethod]
        public async Task GetBookingsForWeek_ShouldReturnBookingsForSpecificWeek()
        {
            var options = new DbContextOptionsBuilder<DbContextAPI>()
                .UseInMemoryDatabase(databaseName: "BookingWeekTest")
                .Options;
            using var context = new DbContextAPI(options);

            context.Flight.AddRange(
                new Flight { Id = 1, DepartureTime = new DateTime(2026, 5, 11) },
                new Flight { Id = 2, DepartureTime = new DateTime(2026, 5, 13) },
                new Flight { Id = 3, DepartureTime = new DateTime(2026, 6, 1) }
            );
            context.Booking.AddRange(
                new Booking { BookingReference = "B1", FlightId = 1 },
                new Booking { BookingReference = "B2", FlightId = 2 },
                new Booking { BookingReference = "B3", FlightId = 3 }
            );
            context.SaveChanges();

            var service = new BookingService(context);
            var result = await service.GetBookingsForWeekAsync(new DateTime(2026, 5, 12));

            Assert.AreEqual(2, result.Count()); // B1 (11 maj) och B2 (13 maj) ska returneras
        }

        [TestMethod]
        public async Task GetBookingsForMonth_ShouldReturnBookingsForSpecificMonth()
        {
            var options = new DbContextOptionsBuilder<DbContextAPI>()
                .UseInMemoryDatabase(databaseName: "BookingMonthTest")
                .Options;
            using var context = new DbContextAPI(options);

            context.Flight.AddRange(
                new Flight { Id = 1, DepartureTime = new DateTime(2026, 6, 1) },
                new Flight { Id = 2, DepartureTime = new DateTime(2026, 6, 15) },
                new Flight { Id = 3, DepartureTime = new DateTime(2026, 7, 1) }
            );
            context.Booking.AddRange(
                new Booking { BookingReference = "B1", FlightId = 1 },
                new Booking { BookingReference = "B2", FlightId = 2 },
                new Booking { BookingReference = "B3", FlightId = 3 }
            );
            context.SaveChanges();

            var service = new BookingService(context);
            var result = await service.GetBookingsForMonthAsync(new DateTime(2026, 6, 10));

            Assert.AreEqual(2, result.Count()); // B1 (1 juni) och B2 (15 juni) ska returneras
        }
    }
}