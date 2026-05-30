using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SaddleHeroesAirWays.API.Controllers;
using SaddleHeroesAirWays.API.DTOs;
using SaddleHeroesAirWays.API.Services;
using SaddleHeroesAirWays.API.Services.Interfaces;

namespace SaddleHeroesAirWays.MSTest
{
    [TestClass]
    public class BookingControllerUnitTests
    {
        private Mock<IBookingService> _mockBookingService = null!;
        private Mock<IValidator<CreateBookingRequest>> _mockValidator = null!;
        private BookingController _controller = null!;

        [TestInitialize]
        public void Setup()
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

        // Edge case - GetWeeklyBookings - no bookings for that week returns empty list
        [TestMethod]
        public async Task GetWeeklyBookings_NoBookingsForWeek_ReturnsOkWithEmptyList()
        {
            var date = new DateTime(2026, 1, 1);
            _mockBookingService
                .Setup(s => s.GetBookingsForWeekAsync(date))
                .ReturnsAsync(new List<BookingResponse>());

            var result = await _controller.GetWeeklyBookings(date);

            var ok = result.Result as OkObjectResult;
            Assert.IsNotNull(ok);
            Assert.AreEqual(0, (ok.Value as IEnumerable<BookingResponse>).Count());
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

        // Edge case - GetMonthlyBookings - no bookings for that month returns empty list
        [TestMethod]
        public async Task GetMonthlyBookings_NoBookingsForMonth_ReturnsOkWithEmptyList()
        {
            var date = new DateTime(2026, 1, 1);
            _mockBookingService
                .Setup(s => s.GetBookingsForMonthAsync(date))
                .ReturnsAsync(new List<BookingResponse>());

            var result = await _controller.GetMonthlyBookings(date);

            var ok = result.Result as OkObjectResult;
            Assert.IsNotNull(ok);
            Assert.AreEqual(0, (ok.Value as IEnumerable<BookingResponse>).Count());
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

            _mockBookingService.Verify(
                s => s.GetBookingsByUserIdAsync(It.IsAny<int>()), Times.Never);
        }

        //Edge case - Verifies that service is called once
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
                    new BookingResponse("REF001", "Anna", "Svensson", "SK001", "Stockholm Arlanda", "London Heathrow",
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

        //Happy path - 200 ok
        [TestMethod]
        public async Task UpdateBooking_ValidRequest_ReturnsOk()
        {
            var bookingReference = "BKG-001";
            var updateBooking = new UpdateBooking(2);
            var bookingResponse = new BookingResponse("BKG-001", "Arthur", "Morgan", "SH-101", "Stockholm Arlanda", "Heathrow Airport", DateTime.Now.AddDays(2), DateTime.Now, 150m, "Rebooked", null);

            _mockBookingService
                .Setup(s => s.UpdateBookingAsync(bookingReference, updateBooking))
                .ReturnsAsync(ServiceResult<BookingResponse>.Ok(bookingResponse));

            var result = await _controller.UpdateBooking(bookingReference, updateBooking);

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [TestMethod]
        public async Task UpdateBooking_BookingNotFound_ReturnsNotFound()
        {
            var bookingReference = "BKG-999";
            var updateBooking = new UpdateBooking(2);

            _mockBookingService
                .Setup(s => s.UpdateBookingAsync(bookingReference, updateBooking))
                .ReturnsAsync(ServiceResult<BookingResponse>.NotFound("Bokning BKG-999 hittades inte."));

            var result = await _controller.UpdateBooking(bookingReference, updateBooking);

            var notFoundResult = result.Result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);

            _mockBookingService.Verify(
                s => s.UpdateBookingAsync(bookingReference, updateBooking), Times.Once);
        }

        [TestMethod]
        public async Task UpdateBooking_TooLateToRebook_ReturnsBadRequest()
        {
            var bookingReference = "BKG-001";
            var updateBooking = new UpdateBooking(2);

            _mockBookingService
                .Setup(s => s.UpdateBookingAsync(bookingReference, updateBooking))
                .ReturnsAsync(ServiceResult<BookingResponse>.ValidationError("Det går inte att omboka mindre än en timme innan avgång."));

            var actual = await _controller.UpdateBooking(bookingReference, updateBooking);

            var badRequestResult = actual.Result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
        }

        //Happy path - 201 created
        [TestMethod]
        public async Task CreateBooking_ValidRequest_ReturnsCreated()
        {
            var request = new CreateBookingRequest(1, 1, null);
            var bookingResponse = new BookingResponse("BKG-001", "Arthur", "Morgan", "SH-101", "Stockholm Arlanda", "Heathrow Airport", new DateTime(2026, 6, 1), DateTime.Now, 150m, "Confirmed", null);

            _mockValidator
                .Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _mockBookingService.Setup(s => s.CreateBookingAsync(request))
                .ReturnsAsync(ServiceResult<BookingResponse>.Ok(bookingResponse));

            var actual = await _controller.CreateBooking(request);

            var createdResult = actual.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);
        }

        //Edge case - 404 flight dosent excists
        [TestMethod]
        public async Task CreateBooking_FlightNotFound_ReturnsNotFound()
        {
            var request = new CreateBookingRequest(1, 99, null);

            _mockValidator
                .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _mockBookingService
                .Setup(s => s.CreateBookingAsync(request))
                .ReturnsAsync(ServiceResult<BookingResponse>.NotFound("Flyget finns inte."));

            var actual = await _controller.CreateBooking(request);

            var notFoundResult = actual.Result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        //Edge case - 400 Flight is full
        [TestMethod]
        public async Task CreateBooking_FlightIsFull_ReturnsBadRequest()
        {
            var request = new CreateBookingRequest(1, 1, null);

            _mockValidator
                .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _mockBookingService
                .Setup(s => s.CreateBookingAsync(request))
                .ReturnsAsync(ServiceResult<BookingResponse>.ValidationError("Flyget är fullt."));

            var actual = await _controller.CreateBooking(request);

            var badRequestResult = actual.Result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
        }

        //Edge case - 400 invalid validation
        [TestMethod]
        public async Task CreateBooking_InvalidRequest_ReturnsBadRequest()
        {
            var request = new CreateBookingRequest(0, 0, null);

            var validationFailures = new List<FluentValidation.Results.ValidationFailure>
            {
                new("UserId", "UserId måste vara ett positivt tal."),
                new("FlightId", "FlightId måste vara ett positivt tal.")
            };

            _mockValidator
                .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult(validationFailures));

            var actual = await _controller.CreateBooking(request);

            var badRequestResult = actual.Result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);

            _mockBookingService.Verify(s => s.CreateBookingAsync(It.IsAny<CreateBookingRequest>()),Times.Never);
        }

        //happy path - 200 OK
        [TestMethod]
        public async Task GetBookingByBookingReference_ValidReference_ReturnsOk()
        {
            var bookingReference = "BKG-001";
            var bookingResponse = new BookingResponse("BKG-001", "Arthur", "Morgan", "SH-101", "Stockholm Arlanda", "Heathrow Airport", new DateTime(2026, 6, 1), DateTime.Now, 150m, "Confirmed", null);

            _mockBookingService
                .Setup(s => s.GetBookingByBookingReferenceAsync(bookingReference))
                .ReturnsAsync(ServiceResult<BookingResponse>.Ok(bookingResponse));

            var actual = await _controller.GetBookingByBookingReference(bookingReference);

            var okResult = actual.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        //Edge case - 404 booking dosent excists
        [TestMethod]
        public async Task GetBookingByBookingReference_InvalidReference_ReturnsNotFound()
        {
            var bookingReference = "BKG-999";

            _mockBookingService
                .Setup(s => s.GetBookingByBookingReferenceAsync(bookingReference))
                .ReturnsAsync(ServiceResult<BookingResponse>.NotFound("Bokning BKG-999 hittades inte."));

            var actual = await _controller.GetBookingByBookingReference(bookingReference);

            var notFoundResult = actual.Result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        //Verify that service is called once
        [TestMethod]
        public async Task GetBookingByBookingReference_ValidReference_VerifyServiceCalledOnce()
        {
            var bookingReference = "BKG-001";
            var bookingResponse = new BookingResponse("BKG-001", "Arthur", "Morgan", "SH-101", "Stockholm Arlanda", "Heathrow Airport", new DateTime(2026, 6, 1), DateTime.Now, 150m, "Confirmed", null);

            _mockBookingService
                .Setup(s => s.GetBookingByBookingReferenceAsync(bookingReference))
                .ReturnsAsync(ServiceResult<BookingResponse>.Ok(bookingResponse));

            await _controller.GetBookingByBookingReference(bookingReference);

            _mockBookingService.Verify(s => s.GetBookingByBookingReferenceAsync(bookingReference), Times.Once);
        }

    }
}