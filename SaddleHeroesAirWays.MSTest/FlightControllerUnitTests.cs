using Moq;
using SaddleHeroesAirWays.API.Controllers;
using SaddleHeroesAirWays.API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaddleHeroesAirWays.MSTest
{
    [TestClass]
    internal class FlightControllerUnitTests
    {
        private readonly Mock<IFlightService> _mockFlightService;
        private readonly FlightController _controller;

        public FlightControllerUnitTests()
        {
            _mockFlightService = new Mock<IFlightService>();
            _controller = new FlightController(_mockFlightService.Object);
        }
    }
}
