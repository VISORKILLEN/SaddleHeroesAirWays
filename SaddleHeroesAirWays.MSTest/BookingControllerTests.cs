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
            //_controller = new BookingController(_mockBookingService.Object); //denna ger mig problem så den är temporärt kommenterad
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

        //Happy path test - returning OK with correct data
        [TestMethod]
        public async Task GetBookingsByUserId_ValidUserId_ReturnsOk()
        {
            var userId = 1;
            _mockBookingService
                .Setup(s => s.GetBookingsByUserIdAsync(userId))
                .ReturnsAsync(
                [
                    new BookingResponse("BKG-001", "Arthur", "Morgan", "SH-101", "Stockholm Arlanda", "Heathrow Airport", new DateTime(2026, 6, 1), new DateTime(2026, 5, 1), 150m, "Confirmed", null)
                ]);

            var actual = await _controller.GetBookingsByUserId(userId);

            var okResult = actual.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        //Edge case test - empty list returns notfound
        [TestMethod]
        public async Task GetBookingsByUserId_UserHasNoBookings_ReturnsNotFound()
        {
            var userId = 99;
            _mockBookingService
                .Setup(s => s.GetBookingsByUserIdAsync(userId))
                .ReturnsAsync([]);

            var actual = await _controller.GetBookingsByUserId(userId);

            var notFound = actual.Result as NotFoundObjectResult;

            Assert.IsNotNull(notFound);
            Assert.AreEqual(404, notFound.StatusCode);
        }

        //Edge case test - negative userId returns badrequest
        [TestMethod]
        public async Task GetBookingsByUserId_InvalidUserId_ReturnsBadRequest()
        {
            var userId = -1;

            var actual = await _controller.GetBookingsByUserId(userId);

            var badRequest = actual.Result as BadRequestObjectResult;

            Assert.IsNotNull(badRequest);
            Assert.AreEqual(400, badRequest.StatusCode);
        }

        //Edge case - verifies that controller returns the service once
        [TestMethod]
        public async Task GetBookingsByUserId_ValidUserId_VerifyServiceCalledOnce()
        {
            var userId = 1;
            _mockBookingService
                .Setup(s => s.GetBookingsByUserIdAsync(userId))
                .ReturnsAsync(
                [
                    new BookingResponse("BKG-001", "Arthur", "Morgan", "SH-101", "Stockholm Arlanda", "Heathrow Airport", new DateTime(2026, 6, 1), new DateTime(2026, 5, 1), 150m, "Confirmed", null)
                ]);

            await _controller.GetBookingsByUserId(userId);

            _mockBookingService.Verify(s => s.GetBookingsByUserIdAsync(userId), Times.Once);
        }
    }
}