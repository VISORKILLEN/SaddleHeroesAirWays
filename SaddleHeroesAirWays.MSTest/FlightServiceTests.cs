using Microsoft.EntityFrameworkCore;
using SaddleHeroesAirWays.API;
using SaddleHeroesAirWays.API.Services;
using SaddleHeroesAirWays.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaddleHeroesAirWays.MSTest
{
    [TestClass]

    internal class FlightServiceTests
    {
        // Helper method to create a new in-memory database context for each test
        private DbContextAPI CreateContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<DbContextAPI>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            return new DbContextAPI(options);
        }

        // Test to verify that searching for available flights without specifying a city returns all flights with available seats
        [TestMethod]
        public async Task SearchAvailableFlights_NoCity_ReturnsAllAvailableFlights()
        {
            using var context = CreateContext("SearchFlightsNoCityTest");

            context.Airport.AddRange(
                new Airport { Id = 1, Name = "Stockholm Arlanda", City = "Stockholm", IATACode = "ARN" },
                new Airport { Id = 2, Name = "Heathrow Airport", City = "London", IATACode = "LHR" }
            );
            context.Flight.AddRange(
                new Flight { Id = 1, FlightNumber = "SH001", DepartureAirportId = 1, ArrivalAirportId = 2, DepartureTime = new DateTime(2026, 6, 1), ArrivalTime = new DateTime(2026, 6, 1), TotalSeats = 150, Price = 150m, FlightStatus = FlightStatus.Arrived },
                new Flight { Id = 2, FlightNumber = "SH002", DepartureAirportId = 2, ArrivalAirportId = 1, DepartureTime = new DateTime(2026, 6, 2), ArrivalTime = new DateTime(2026, 6, 2), TotalSeats = 1, Price = 200m, FlightStatus = FlightStatus.Arrived }
            );
            context.User.Add(new User { Id = 1, Firstname = "Test", Lastname = "User" });
            context.Booking.Add(new Booking { BookingReference = "B1", FlightId = 2, UserId = 1 }); // SH002 är fullbokad
            context.SaveChanges();

            var service = new FlightService(context);
            var result = await service.SearchAvailableFlightsAsync();

            Assert.AreEqual(1, result.Count()); // bara SH001 har lediga platser
            Assert.AreEqual("SH001", result.First().Flightnumber);
        }


    }
}
