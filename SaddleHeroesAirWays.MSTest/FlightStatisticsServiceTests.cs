using Microsoft.EntityFrameworkCore;
using SaddleHeroesAirWays.API;
using SaddleHeroesAirWays.API.Services;
using SaddleHeroesAirWays.Library.Models;

namespace SaddleHeroesAirWays.MSTest
{
    [TestClass]
    public class FlightStatisticsServiceTests
    {
        [TestMethod]
        public void GetFlightsForWeek_ShouldReturnFlightsForSpecificWeek()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DbContextAPI>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using var context = new DbContextAPI(options);

            context.Flight.AddRange(
                new Flight { Id = 1, DepartureTime = new DateTime(2026, 5, 11) },
                new Flight { Id = 2, DepartureTime = new DateTime(2026, 5, 13) },
                new Flight { Id = 3, DepartureTime = new DateTime(2026, 6, 1) }
            );

            context.SaveChanges();

            var service = new FlightService(context);

            // Act
            var result = service.GetFlightsForWeek(new DateTime(2026, 5, 12));

            // Assert
            Assert.AreEqual(2, result.Count);
        }
    }
}
