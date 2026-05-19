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

            context.Booking.AddRange(
                new Booking { BookingReference = "B1", BookingDate = new DateTime(2026, 5, 11) },
                new Booking { BookingReference = "B2", BookingDate = new DateTime(2026, 5, 13) },
                new Booking { BookingReference = "B3", BookingDate = new DateTime(2026, 6, 1) }
            );

            context.SaveChanges();

            var service = new BookingService(context);

            var result = await service.GetBookingsForWeekAsync(new DateTime(2026, 5, 12));

            Assert.AreEqual(2, result.Count);
        }

        // This test checks if the GetBookingsForMonthAsync method correctly retrieves bookings for a specific month.
        [TestMethod]
        public async Task GetBookingsForMonth_ShouldReturnBookingsForSpecificMonth()
        {
            var options = new DbContextOptionsBuilder<DbContextAPI>()
                .UseInMemoryDatabase(databaseName: "BookingMonthTest")
                .Options;

            using var context = new DbContextAPI(options);

            context.Booking.AddRange(
                new Booking { BookingReference = "B1", BookingDate = new DateTime(2026, 6, 1) },
                new Booking { BookingReference = "B2", BookingDate = new DateTime(2026, 6, 15) },
                new Booking { BookingReference = "B3", BookingDate = new DateTime(2026, 7, 1) }
            );

            context.SaveChanges();

            var service = new BookingService(context);

            var result = await service.GetBookingsForMonthAsync(new DateTime(2026, 6, 10));

            Assert.AreEqual(2, result.Count);
        }
    }
}