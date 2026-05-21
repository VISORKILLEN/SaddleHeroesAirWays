using Microsoft.AspNetCore.Mvc.Testing;

namespace SaddleHeroesAirWays.MSTest
{
    [TestClass]
    public class BookingControllerTests
    {
        private readonly HttpClient _client;

        public BookingControllerTests()
        {
            var factory = new WebApplicationFactory<Program>();
            _client = factory.CreateClient();
        }

        [TestMethod]
        public async Task GetWeeklyBookings_ReturnsSuccess()
        {
            // Act
            var response = await _client.GetAsync("/api/booking/weekly?date=2026-05-12");

            // Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task GetMonthlyBookings_ReturnsSuccess()
        {
            // Act
            var response = await _client.GetAsync("/api/booking/monthly?date=2026-06-10");

            // Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task GetBookingsByUserIdAsync__ReturnsSuccess
    }
}
