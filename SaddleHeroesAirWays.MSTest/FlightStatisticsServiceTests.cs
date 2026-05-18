using SaddleHeroesAirWays.Library.Models;
using SaddleHeroesAirWays.API.Services;

namespace SaddleHeroesAirWays.MSTest
{
    [TestClass]
    public class FlightStatisticsServiceTests
    {
        [TestMethod]
        public void GetFlightsForWeek_ShouldReturnFlightsForSpecificWeek()
        {
            // Arrange
            var flights = new List<Flight>
                {
                    new Flight { Id = 1, DepartureTime = new DateTime(2026, 5, 11) },
                    new Flight { Id = 2, DepartureTime = new DateTime(2026, 5, 13) },
                    new Flight { Id = 3, DepartureTime = new DateTime(2026, 6, 1) }
                };

            var service = new FlightService(flights);

            // Act
            var result = service.GetFlightsForWeek(new DateTime(2026, 5, 12));

            // Assert
            Assert.AreEqual(2, result.Count);
        }
    }
}
