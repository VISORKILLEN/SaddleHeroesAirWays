using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SaddleHeroesAirWays.API;
using SaddleHeroesAirWays.API.Controllers;
using SaddleHeroesAirWays.API.Services;
using SaddleHeroesAirWays.API.Services.Interfaces;
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

        [TestMethod]
        public void GetWeeklyFlights_ShouldReturnOkWithFlights()
        {
            // ARRANGE
            var mockService = new Mock<IFlightService>();

            var mockFlights = new List<Flight>
            {
                new Flight { Id = 1, DepartureTime = new DateTime(2026, 5, 11) },
                new Flight { Id = 2, DepartureTime = new DateTime(2026, 5, 13) }
            };

            mockService
                .Setup(s => s.GetFlightsForWeek(It.IsAny<DateTime>()))
                .Returns(mockFlights);

            var controller = new FlightController(mockService.Object);

            // ACT
            var result = controller.GetWeeklyFlights(new DateTime(2026, 5, 12));

            // ASSERT
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult, "Resultatet borde vara ett OkObjectResult (HTTP 200)");
            Assert.AreEqual(200, okResult.StatusCode);

            var returnedFlights = okResult.Value as List<Flight>;
            Assert.IsNotNull(returnedFlights, "Datan i svaret borde vara en lista med Flight");
            Assert.AreEqual(2, returnedFlights.Count);
        }
    }
}
