using Microsoft.AspNetCore.Mvc;
using Moq;
using SaddleHeroesAirWays.API.Controllers;
using SaddleHeroesAirWays.API.DTOs;
using SaddleHeroesAirWays.API.Services.Interfaces;

namespace SaddleHeroesAirWays.MSTest
{
    [TestClass]
    public class BookingControllerUnitTests
    {
        private readonly Mock<IBookingService> _mockBookingService;
        private readonly BookingController _controller;

        public BookingControllerUnitTests()
        {
            // Skapa mock och controller istället för HttpClient
            _mockBookingService = new Mock<IBookingService>();
            _controller = new BookingController(_mockBookingService.Object);
        }

        [TestMethod]
        public async Task GetWeeklyBookings_ReturnsOk()
        {
            // Arrange – bestäm vad mocken ska returnera
            var date = new DateTime(2026, 5, 12);
            _mockBookingService
                .Setup(s => s.GetBookingsForWeekAsync(date))
                .ReturnsAsync(new List<BookingResponse>());

            // Act – anropa controllern direkt
            var result = await _controller.GetWeeklyBookings(date);

            // Assert – kontrollera att svaret är 200 OK
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [TestMethod]
        public async Task GetMonthlyBookings_ReturnsOk()
        {
            // Arrange – bestäm vad mocken ska returnera
            var date = new DateTime(2026, 6, 10);
            _mockBookingService
                .Setup(s => s.GetBookingsForMonthAsync(date))
                .ReturnsAsync(new List<BookingResponse>());

            // Act – anropa controllern direkt
            var result = await _controller.GetMonthlyBookings(date);

            // Assert – kontrollera att svaret är 200 OK
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }
    }
}