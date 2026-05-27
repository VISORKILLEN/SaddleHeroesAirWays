using FluentValidation;
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
        private readonly Mock<IValidator<CreateBookingRequest>> _mockValidator;
        private readonly BookingController _controller;

        public BookingControllerUnitTests()
        {
            _mockBookingService = new Mock<IBookingService>();
            _mockValidator = new Mock<IValidator<CreateBookingRequest>>();
            _controller = new BookingController(_mockBookingService.Object, _mockValidator.Object);
        }

        // Happy path test - verifies that the controller returns OK for weekly bookings
        [TestMethod]
        public async Task GetWeeklyBookings_ReturnsOk()
        {
            var date = new DateTime(2026, 5, 12);
            _mockBookingService
                .Setup(s => s.GetBookingsForWeekAsync(date))
                .ReturnsAsync(new List<BookingResponse>());

            var result = await _controller.GetWeeklyBookings(date);

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        // Happy path test - verifies that the controller returns OK for monthly bookings
        [TestMethod]
        public async Task GetMonthlyBookings_ReturnsOk()
        {
            var date = new DateTime(2026, 6, 10);
            _mockBookingService
                .Setup(s => s.GetBookingsForMonthAsync(date))
                .ReturnsAsync(new List<BookingResponse>());

            var result = await _controller.GetMonthlyBookings(date);

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

        // Happy path test - verifies that the controller returns OK for all bookings
        [TestMethod]
        public async Task GetAllBookings_ReturnsSuccess()
        {
            // Arrange
            var mockService = new Mock<IBookingService>();
            mockService.Setup(s => s.GetAllBookingsMadeAsync())
                .ReturnsAsync(new List<BookingResponse>
                {
                new BookingResponse("BKG-1001", "Arthur", "Morgan", "SH-101", "Stockholm Arlanda", "Heathrow Airport", new DateTime(2026, 6, 1), new DateTime(2026, 5, 1), 150.00m, "Confirmed", null),
                new BookingResponse("BKG-1002", "Sadie", "Adler", "SH-102", "Heathrow Airport", "John F. Kennedy International", new DateTime(2026, 6, 2), new DateTime(2026, 5, 2), 600.00m, "Confirmed", null)
                });

            var controller = new BookingController(mockService.Object, _mockValidator.Object);

            // Act
            var result = await controller.GetAllBookings();

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        // Happy path - valid date range returns OK
        [TestMethod]
        public async Task GetBookingsByDateRange_ValidDates_ReturnsOk()
        {
            var start = new DateTime(2025, 6, 1);
            var end = new DateTime(2025, 6, 30);

            _mockBookingService
                .Setup(s => s.GetBookingsForDateRangeAsync(start, end))
                .ReturnsAsync(new List<BookingResponse>
                {
            new BookingResponse("REF001", "Anna", "Svensson", "SK001",
                "Stockholm Arlanda", "London Heathrow",
                new DateTime(2025, 6, 5), DateTime.Now, 1000, "Confirmed", null)
                });

            var result = await _controller.GetBookingsByDateRange(start, end);

            var ok = result as OkObjectResult;
            Assert.IsNotNull(ok);
            Assert.AreEqual(200, ok.StatusCode);
        }

        // Edge case - start after end returns BadRequest without calling service
        [TestMethod]
        public async Task GetBookingsByDateRange_StartAfterEnd_ReturnsBadRequest()
        {
            var result = await _controller.GetBookingsByDateRange(
                new DateTime(2025, 6, 30),
                new DateTime(2025, 6, 1));

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            _mockBookingService.Verify(s => s.GetBookingsForDateRangeAsync(
                It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never);
        }

    }
}