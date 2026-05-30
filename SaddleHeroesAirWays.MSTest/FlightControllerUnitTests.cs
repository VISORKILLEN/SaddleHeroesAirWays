using Microsoft.AspNetCore.Mvc;
using Moq;
using SaddleHeroesAirWays.API.Controllers;
using SaddleHeroesAirWays.API.DTOs;
using SaddleHeroesAirWays.API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaddleHeroesAirWays.MSTest
{
    [TestClass]
    public class FlightControllerUnitTests
    {
        private readonly Mock<IFlightService> _mockFlightService;
        private readonly FlightController _controller;

        public FlightControllerUnitTests()
        {
            _mockFlightService = new Mock<IFlightService>();
            _controller = new FlightController(_mockFlightService.Object);
        }

        public enum FlightStatus
        {
            Scheduled,
            Boarding,
            Departed,
            Arrived,
            Cancelled
        }

        // Happy path test, verifies that the controller returns OK with available flights
        [TestMethod]
        public async Task SearchAvailableFlights_NoCity_ReturnOK()
        {
            _mockFlightService
                .Setup(s => s.SearchAvailableFlightsAsync(null))
                .ReturnsAsync(new List<FlightResponse>
                {
                    new FlightResponse(1, "SH001", "Stockholm Arlanda", "Heathrow Airport", new DateTime(2026, 6, 1), new DateTime(2026, 6, 1), 149, 150m, FlightStatus.Arrived)
                });

            var result = await _controller.SearchAvailableFlights(null);

            var ok = result as OkObjectResult;
            Assert.IsNotNull(ok);
            Assert.AreEqual(200, ok.StatusCode);
        }

        //Happy path, city filter return 200 with filtirerd flights
        [TestMethod]
        public async Task SearchAvailableFlights_WithCity_ReturnOK()
        {
            _mockFlightService
                .Setup(s => s.SearchAvailableFlightsAsync("Stockholm"))
                .ReturnsAsync(new List<FlightResponse>
                {
                    new FlightResponse(1, "SH001", "Stockholm Arlanda", "Heathrow Airport", new DateTime(2026, 6, 1), new DateTime(2026, 6, 1), 149, 150m, FlightStatus.Arrived)
                });
            var result = await _controller.SearchAvailableFlights("Stockholm");

            var ok = result as OkObjectResult;
            Assert.IsNotNull(ok);
            Assert.AreEqual(200, ok.StatusCode);
        }

        // Edge case - no flights found returns 404
        [TestMethod]
        public async Task SearchAvailableFlights_NoFlightsFound_ReturnsNotFound()
        {
            _mockFlightService
                .Setup(s => s.SearchAvailableFlightsAsync("Tokyo"))
                .ReturnsAsync(new List<FlightResponse>());

            var result = await _controller.SearchAvailableFlights("Tokyo");

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }
    }
}
