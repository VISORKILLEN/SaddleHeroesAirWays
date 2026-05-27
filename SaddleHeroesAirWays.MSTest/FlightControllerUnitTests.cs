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

        // Happy path test, verifies that the controller returns OK with available flights
        public async Task SearchAvailableFlights_NoCity_ReturnOK()
        {
            _mockFlightService
                .Setup(s => s.SearchAvailableFlightsAsync(null))
                .ReturnsAsync(new List<FlightResponse>
                {
                    new FlightResponse("SH001", "Stockholm Arlanda", "Heathrow Airport", new DateTime(2026, 6, 1), new DateTime(2026, 6, 1), 149, 150m, "Arrived")
                });

            var result = await _controller.SearchAvailableFlights(null);

            var ok = result as OkObjectResult;
            Assert.IsNotNull(ok);
            Assert.AreEqual(200, ok.StatusCode);
        }
    }
}
