using Microsoft.EntityFrameworkCore;
using SaddleHeroesAirWays.API;
using SaddleHeroesAirWays.API.Services;
using SaddleHeroesAirWays.Library.Models;

namespace SaddleHeroesAirWays.MSTest
{
    [TestClass]
    public class BookingServiceTests
    {
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