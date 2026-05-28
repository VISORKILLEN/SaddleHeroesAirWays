using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using SaddleHeroesAirWays.API;
using SaddleHeroesAirWays.API.DTOs;
using SaddleHeroesAirWays.API.Services;
using SaddleHeroesAirWays.Library.Models;


namespace SaddleHeroesAirWays.MSTest
{
    [TestClass]
    public class BookingServiceTests
    {
        private DbContextAPI CreateContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<DbContextAPI>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            return new DbContextAPI(options);
        }

        //Happy path test - return the bookings of indicated week
        [TestMethod]
        public async Task GetBookingsForWeek_ShouldReturnBookingsForSpecificWeek()
        {
            using var context = CreateContext("BookingWeekTest");

            context.Airport.AddRange(
                new Airport { Id = 1, Name = "Göteborg", IATACode = "GOT" },
                new Airport { Id = 2, Name = "Stockholm", IATACode = "ARN" }
            );
            context.User.Add(new User { Id = 1, Firstname = "Test", Lastname = "User" });
            context.Flight.AddRange(
                new Flight { Id = 1, DepartureTime = new DateTime(2026, 5, 11), FlightNumber = "SH001", DepartureAirportId = 1, ArrivalAirportId = 2 },
                new Flight { Id = 2, DepartureTime = new DateTime(2026, 5, 13), FlightNumber = "SH002", DepartureAirportId = 1, ArrivalAirportId = 2 },
                new Flight { Id = 3, DepartureTime = new DateTime(2026, 6, 1), FlightNumber = "SH003", DepartureAirportId = 1, ArrivalAirportId = 2 }
            );
            context.Booking.AddRange(
                new Booking { BookingReference = "B1", FlightId = 1, UserId = 1 },
                new Booking { BookingReference = "B2", FlightId = 2, UserId = 1 },
                new Booking { BookingReference = "B3", FlightId = 3, UserId = 1 }
            );
            context.SaveChanges();

            var service = new BookingService(context);
            var result = await service.GetBookingsForWeekAsync(new DateTime(2026, 5, 12));

            Assert.AreEqual(2, result.Count()); // B1 och B2 ska returneras
        }

        //Happy path test - return the bookings of specific month
        [TestMethod]
        public async Task GetBookingsForMonth_ShouldReturnBookingsForSpecificMonth()
        {
            using var context = CreateContext("BookingMonthTest");

            context.Airport.AddRange(
                new Airport { Id = 1, Name = "Göteborg", IATACode = "GOT" },
                new Airport { Id = 2, Name = "Stockholm", IATACode = "ARN" }
            );
            context.User.Add(new User { Id = 1, Firstname = "Test", Lastname = "User" });
            context.Flight.AddRange(
                new Flight { Id = 1, DepartureTime = new DateTime(2026, 6, 1), FlightNumber = "SH001", DepartureAirportId = 1, ArrivalAirportId = 2 },
                new Flight { Id = 2, DepartureTime = new DateTime(2026, 6, 15), FlightNumber = "SH002", DepartureAirportId = 1, ArrivalAirportId = 2 },
                new Flight { Id = 3, DepartureTime = new DateTime(2026, 7, 1), FlightNumber = "SH003", DepartureAirportId = 1, ArrivalAirportId = 2 }
            );
            context.Booking.AddRange(
                new Booking { BookingReference = "B1", FlightId = 1, UserId = 1 },
                new Booking { BookingReference = "B2", FlightId = 2, UserId = 1 },
                new Booking { BookingReference = "B3", FlightId = 3, UserId = 1 }
            );
            context.SaveChanges();

            var service = new BookingService(context);
            var result = await service.GetBookingsForMonthAsync(new DateTime(2026, 6, 10));

            Assert.AreEqual(2, result.Count()); // B1 och B2 ska returneras
        }


        //Happy path test - return the bookings of indicated id
        [TestMethod]
        public async Task GetBookingsByUserId_EnterUserIdAndGetTheirBookings_ReturnBookings()
        {
            using var context = CreateContext("BookingsByIdOne");

            context.User.Add(new User { Id = 1, Firstname = "Arthur", Lastname = "Morgan", Email = "arthur@test.com" });

            context.Flight.Add(new Flight { Id = 1, FlightNumber = "SH-101", DepartureAirportId = 1, ArrivalAirportId = 2, DepartureTime = new DateTime(2026, 6, 1), ArrivalTime = new DateTime(2026, 6, 1), TotalSeats = 150, Price = 150m });

            context.Airport.AddRange(
                new Airport { Id = 1, IATACode = "ARN", Name = "Stockholm Arlanda", City = "Stockholm", Country = "Sweden" },
                new Airport { Id = 2, IATACode = "LHR", Name = "Heathrow Airport", City = "London", Country = "UK" }
            );
            context.Booking.Add(new Booking { BookingReference = "BKG-001", UserId = 1, FlightId = 1, BookingDate = new DateTime(2026, 5, 1), TotalPrice = 150m, BookingStatus = BookingStatus.Confirmed });

            context.SaveChanges();

            var service = new BookingService(context);
            var actual = await service.GetBookingsByUserIdAsync(userId: (1));

            Assert.AreEqual(1, actual.Count());
        }

        //Happy path test - returns the first booking including bookingdetails
        [TestMethod]
        public async Task GetBookingsByUserId_EnterUserIdAndGetTheirBookingsWithBookingDetails_ReturnBookingsWithBookingDetails()
        {
            using var context = CreateContext("BookingsByIdTwo");

            context.User.Add(new User { Id = 1, Firstname = "Arthur", Lastname = "Morgan", Email = "arthur@test.com" });

            context.Flight.Add(new Flight { Id = 1, FlightNumber = "SH-101", DepartureAirportId = 1, ArrivalAirportId = 2, DepartureTime = new DateTime(2026, 6, 1), ArrivalTime = new DateTime(2026, 6, 1), TotalSeats = 150, Price = 150m });

            context.Airport.AddRange(
                new Airport { Id = 1, IATACode = "ARN", Name = "Stockholm Arlanda", City = "Stockholm", Country = "Sweden" },
                new Airport { Id = 2, IATACode = "LHR", Name = "Heathrow Airport", City = "London", Country = "UK" }
            );
            context.Booking.Add(new Booking { BookingReference = "BKG-001", UserId = 1, FlightId = 1, BookingDate = new DateTime(2026, 5, 1), TotalPrice = 150m, BookingStatus = BookingStatus.Confirmed });

            context.BookingDetails.Add(new BookingDetails
            {
                Id = 1,
                BookingReference = "BKG-001",
                Seatnumber = "12A",
                Baggage = true,
                Notes = "Vegetarian meal requested"
            });

            context.SaveChanges();

            var service = new BookingService(context);
            var actual = await service.GetBookingsByUserIdAsync(userId: (1));
            var firstBooking = actual.First();

            Assert.AreEqual(1, firstBooking.Details.Count());
        }

        //Edge case test - verifies that an empty list returns if user dont have any bookings
        [TestMethod]
        public async Task GetBookingsByUserId_UserHasNoBookings_ReturnEmpty()
        {
            using var context = CreateContext("BookingsByIdThree");

            var service = new BookingService(context);
            var actual = await service.GetBookingsByUserIdAsync(userId: 1);

            Assert.AreEqual(0, actual.Count());
        }

        //Happy case test - returns correctly mapped data
        [TestMethod]
        public async Task GetBookingsByUserId_ValidUserId_ReturnCorrectlyMappedData()
        {
            using var context = CreateContext("BookingsByIdFour");

            context.User.Add(new User { Id = 1, Firstname = "Arthur", Lastname = "Morgan", Email = "arthur@test.com" });

            context.Flight.Add(new Flight { Id = 1, FlightNumber = "SH-101", DepartureAirportId = 1, ArrivalAirportId = 2, DepartureTime = new DateTime(2026, 6, 1), ArrivalTime = new DateTime(2026, 6, 1), TotalSeats = 150, Price = 150m });

            context.Airport.AddRange(
                new Airport { Id = 1, IATACode = "ARN", Name = "Stockholm Arlanda", City = "Stockholm", Country = "Sweden" },
                new Airport { Id = 2, IATACode = "LHR", Name = "Heathrow Airport", City = "London", Country = "UK" }
            );
            context.Booking.Add(new Booking { BookingReference = "BKG-001", UserId = 1, FlightId = 1, BookingDate = new DateTime(2026, 5, 1), TotalPrice = 150m, BookingStatus = BookingStatus.Confirmed });

            context.SaveChanges();

            var service = new BookingService(context);
            var actual = await service.GetBookingsByUserIdAsync(userId: (1));

            Assert.AreEqual("Arthur", actual.First().Firstname);
            Assert.AreEqual("BKG-001", actual.First().BookingReference);
            Assert.AreEqual("Stockholm Arlanda", actual.First().DepartureAirport);
            Assert.AreEqual("Confirmed", actual.First().BookingStatus);
        }

        //Happy path test - returns all bookings in the system
        [TestMethod]
        public async Task GetAllBookings_ShouldReturnAllBookings()
        {
            using var context = CreateContext("GetAllBookings");

            context.Airport.AddRange(
                new Airport { Id = 1, Name = "Göteborg", IATACode = "GOT" },
                new Airport { Id = 2, Name = "Stockholm", IATACode = "ARN" }
            );
            context.User.Add(new User { Id = 1, Firstname = "Test", Lastname = "User" });
            context.Flight.AddRange(
                new Flight { Id = 1, DepartureTime = new DateTime(2026, 6, 1), FlightNumber = "SH001", DepartureAirportId = 1, ArrivalAirportId = 2 },
                new Flight { Id = 2, DepartureTime = new DateTime(2026, 6, 15), FlightNumber = "SH002", DepartureAirportId = 1, ArrivalAirportId = 2 },
                new Flight { Id = 3, DepartureTime = new DateTime(2026, 7, 1), FlightNumber = "SH003", DepartureAirportId = 1, ArrivalAirportId = 2 }
            );
            context.Booking.AddRange(
                new Booking { BookingReference = "B1", FlightId = 1, UserId = 1 },
                new Booking { BookingReference = "B2", FlightId = 2, UserId = 1 },
                new Booking { BookingReference = "B3", FlightId = 3, UserId = 1 }
            );
            context.SaveChanges();

            var service = new BookingService(context);
            var result = await service.GetAllBookingsMadeAsync();

            Assert.AreEqual(3, result.Count());
        }

        // Happy path - returns bookings within the given date range
        [TestMethod]
        public async Task GetBookingsForDateRange_ValidRange_ReturnsOnlyBookingsWithinRange()
        {
            using var context = CreateContext("BookingDateRangeTest");

            context.Airport.AddRange(
                new Airport { Id = 1, Name = "Göteborg", IATACode = "GOT" },
                new Airport { Id = 2, Name = "Stockholm", IATACode = "ARN" }
            );
            context.User.Add(new User { Id = 1, Firstname = "Test", Lastname = "User" });
            context.Flight.AddRange(
                new Flight { Id = 1, DepartureTime = new DateTime(2026, 6, 10), FlightNumber = "SH001", DepartureAirportId = 1, ArrivalAirportId = 2 },
                new Flight { Id = 2, DepartureTime = new DateTime(2026, 7, 10), FlightNumber = "SH002", DepartureAirportId = 1, ArrivalAirportId = 2 }
            );
            context.Booking.AddRange(
                new Booking { BookingReference = "B1", FlightId = 1, UserId = 1 },
                new Booking { BookingReference = "B2", FlightId = 2, UserId = 1 }
            );
            context.SaveChanges();

            var service = new BookingService(context);
            var result = await service.GetBookingsForDateRangeAsync(
                new DateTime(2026, 6, 1),
                new DateTime(2026, 6, 30));

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("B1", result.First().BookingReference);
        }

        // Edge case - start date after end date throws ArgumentException
        [TestMethod]
        public async Task GetBookingsForDateRange_StartAfterEnd_ThrowsArgumentException()
        {
            using var context = CreateContext("BookingDateRangeInvalidTest");
            var service = new BookingService(context);

            await Assert.ThrowsExceptionAsync<ArgumentException>(() =>
                service.GetBookingsForDateRangeAsync(
                    new DateTime(2026, 6, 30),
                    new DateTime(2026, 6, 1)));
        }

        //Happy path - skapar en korrekt bokning
        [TestMethod]
        public async Task CreateBooking_ValidRequest_ReturnBookingResponse()
        {
            using var context = CreateContext("CreateBookingHappyPath");

            context.Airport.AddRange(
                new Airport { Id = 1, IATACode = "ARN", Name = "Stockholm Arlanda", City = "Stockholm", Country = "Sweden" },
                new Airport { Id = 2, IATACode = "LHR", Name = "Heathrow Airport", City = "London", Country = "UK" }
                );

            context.User.Add(new User { Id = 1, Firstname = "Arthur", Lastname = "Morgan", Email = "arthur@test.com" });

            context.Flight.Add(new Flight { Id = 1, FlightNumber = "SH-101", DepartureAirportId = 1, ArrivalAirportId = 2, DepartureTime = new DateTime(2026, 6, 1), ArrivalTime = new DateTime(2026, 6, 1), TotalSeats = 150, Price = 150m });

            context.SaveChanges();

            var service = new BookingService(context);
            var request = new CreateBookingRequest(1, 1, null);
            var actual = await service.CreateBookingAsync(request);

            Assert.IsTrue(actual.Success);
            Assert.IsNotNull(actual.Data);
            Assert.AreEqual("Arthur", actual.Data!.Firstname);
            Assert.AreEqual("Confirmed", actual.Data!.BookingStatus);
            Assert.AreEqual(150m, actual.Data!.TotalPrice);
        }

        //Edge case - Flight finns inte - retunera null
        [TestMethod]
        public async Task CreateBooking_FlightNotFound_ReturnNotFound()
        {
            using var context = CreateContext("CreateBookingNoFLight");

            var service = new BookingService(context);
            var request = new CreateBookingRequest(1, 99, null);
            var actual = await service.CreateBookingAsync(request);

            Assert.IsFalse(actual.Success);
            Assert.AreEqual(ServiceResultStatus.NotFound, actual.Status);
        }

        [TestMethod]
        public async Task CreateBooking_FlightIsFull_ReturnValidationError()
        {
            using var context = CreateContext("CreateBookingFullFlight");

            context.Airport.AddRange(
                new Airport { Id = 1, IATACode = "ARN", Name = "Stockholm Arlanda", City = "Stockholm", Country = "Sweden" },
                new Airport { Id = 2, IATACode = "LHR", Name = "Heathrow Airport", City = "London", Country = "UK" }
            );

            context.User.Add(new User { Id = 1, Firstname = "Arthur", Lastname = "Morgan", Email = "arthur@test.com" });
            context.Flight.Add(new Flight { Id = 1, FlightNumber = "SH-101", DepartureAirportId = 1, ArrivalAirportId = 2, DepartureTime = new DateTime(2026, 6, 1), ArrivalTime = new DateTime(2026, 6, 1), TotalSeats = 1, Price = 150m });
            context.Booking.Add(new Booking { BookingReference = "BKG-001", UserId = 1, FlightId = 1, BookingDate = DateTime.Now, TotalPrice = 150m, BookingStatus = BookingStatus.Confirmed });
            context.BookingDetails.Add(new BookingDetails { Id = 1, BookingReference = "BKG-001", Seatnumber = "1A", Baggage = false, Notes = "" });
            
            context.SaveChanges();

            var service = new BookingService(context);
            var request = new CreateBookingRequest(1, 1, null);
            var actual = await service.CreateBookingAsync(request);

            Assert.IsFalse(actual.Success);
            Assert.AreEqual(ServiceResultStatus.ValidationError, actual.Status);
        }

        [TestMethod]
        public async Task CreateBooking_ValidRequest_ReturnCorrectlyMappedData()
        {
            using var context = CreateContext("CreateBookingMappedData");

            context.Airport.AddRange(
                new Airport { Id = 1, IATACode = "ARN", Name = "Stockholm Arlanda", City = "Stockholm", Country = "Sweden" },
                new Airport { Id = 2, IATACode = "LHR", Name = "Heathrow Airport", City = "London", Country = "UK" }
            );

            context.User.Add(new User { Id = 1, Firstname = "Arthur", Lastname = "Morgan", Email = "arthur@test.com" });
            context.Flight.Add(new Flight { Id = 1, FlightNumber = "SH-101", DepartureAirportId = 1, ArrivalAirportId = 2, DepartureTime = new DateTime(2026, 6, 1), ArrivalTime = new DateTime(2026, 6, 1), TotalSeats = 150, Price = 150m });

            context.SaveChanges();

            var service = new BookingService(context);
            var request = new CreateBookingRequest(1, 1, null);
            var actual = await service.CreateBookingAsync(request);

            Assert.AreEqual("BKG-1001", actual.Data!.BookingReference);
            Assert.AreEqual("Stockholm Arlanda", actual.Data!.DepartureAirport);
            Assert.AreEqual("Heathrow Airport", actual.Data!.ArrivalAirport);
            Assert.AreEqual("SH-101", actual.Data!.Flightnumber);
        }

        //ombokning lyckas
        [TestMethod]
        public async Task UpdateBooking_ValidRequest_ReturnSuccess()
        {
            using var context = CreateContext("UpdateBookingHappyPath");

            context.Airport.AddRange(
                new Airport { Id = 1, IATACode = "ARN", Name = "Stockholm Arlanda", City = "Stockholm", Country = "Sweden" },
                new Airport { Id = 2, IATACode = "LHR", Name = "Heathrow Airport", City = "London", Country = "UK" }
            );

            context.User.Add(new User { Id = 1, Firstname = "Arthur", Lastname = "Morgan", Email = "arthur@test.com" });
            
            context.Flight.AddRange(
                new Flight { Id = 1, FlightNumber = "SH-101", DepartureAirportId = 1, ArrivalAirportId = 2, DepartureTime = DateTime.Now.AddDays(2), ArrivalTime = DateTime.Now.AddDays(2), TotalSeats = 150, Price = 150m },
                new Flight { Id = 2, FlightNumber = "SH-102", DepartureAirportId = 1, ArrivalAirportId = 2, DepartureTime = DateTime.Now.AddDays(3), ArrivalTime = DateTime.Now.AddDays(3), TotalSeats = 150, Price = 200m }
            );
            
            context.Booking.Add(new Booking { BookingReference = "BKG-001", UserId = 1, FlightId = 1, BookingDate = DateTime.Now, TotalPrice = 150m, BookingStatus = BookingStatus.Confirmed });

            context.SaveChanges();

            var service = new BookingService(context);
            var request = new UpdateBooking(2);
            var actual = await service.UpdateBookingAsync("BKG-001", request);

            Assert.IsTrue(actual.Success);
            Assert.AreEqual("Rebooked", actual.Data.BookingStatus);
        }

        //bokning finns inte
        [TestMethod]
        public async Task UpdateBooking_BookingNotFound_ReturnNotFound()
        {
            using var context = CreateContext("UpdateBookingNotFound");

            var service = new BookingService(context);
            var request = new UpdateBooking(1);
            var actual = await service.UpdateBookingAsync("BKG-999", request);

            Assert.IsFalse(actual.Success);
            Assert.AreEqual(ServiceResultStatus.NotFound, actual.Status);
        }

        [TestMethod]
        public async Task UpdateBooking_TooLateToRebook_ReturnValidationError()
        {
            using var context = CreateContext("UpdateBookingTooLate");

            context.Airport.AddRange(
                new Airport { Id = 1, IATACode = "ARN", Name = "Stockholm Arlanda", City = "Stockholm", Country = "Sweden" },
                new Airport { Id = 2, IATACode = "LHR", Name = "Heathrow Airport", City = "London", Country = "UK" }
            );
            context.User.Add(new User { Id = 1, Firstname = "Arthur", Lastname = "Morgan", Email = "arthur@test.com" });
            context.Flight.Add(new Flight { Id = 1, FlightNumber = "SH-101", DepartureAirportId = 1, ArrivalAirportId = 2, DepartureTime = DateTime.Now.AddMinutes(30), ArrivalTime = DateTime.Now.AddMinutes(30), TotalSeats = 150, Price = 150m });
            context.Booking.Add(new Booking { BookingReference = "BKG-001", UserId = 1, FlightId = 1, BookingDate = DateTime.Now, TotalPrice = 150m, BookingStatus = BookingStatus.Confirmed });
            context.SaveChanges();

            var service = new BookingService(context);
            var request = new UpdateBooking(1);
            var actual = await service.UpdateBookingAsync("BKG-001", request);

            Assert.IsFalse(actual.Success);
            Assert.AreEqual(ServiceResultStatus.ValidationError, actual.Status);
        }

        //Happy path - returns the booking with correct reference
        [TestMethod]
        public async Task GetBookingByBookingReference_ValidReference_ReturnSuccess()
        {
            using var context = CreateContext("GetBookingByReferenceHappyPath");

            context.Airport.AddRange(
                new Airport { Id = 1, IATACode = "ARN", Name = "Stockholm Arlanda", City = "Stockholm", Country = "Sweden" },
                new Airport { Id = 2, IATACode = "LHR", Name = "Heathrow Airport", City = "London", Country = "UK" }
            );
            context.User.Add(new User { Id = 1, Firstname = "Arthur", Lastname = "Morgan", Email = "arthur@test.com" });
            context.Flight.Add(new Flight { Id = 1, FlightNumber = "SH-101", DepartureAirportId = 1, ArrivalAirportId = 2, DepartureTime = new DateTime(2026, 6, 1), ArrivalTime = new DateTime(2026, 6, 1), TotalSeats = 150, Price = 150m });
            context.Booking.Add(new Booking { BookingReference = "BKG-001", UserId = 1, FlightId = 1, BookingDate = DateTime.Now, TotalPrice = 150m, BookingStatus = BookingStatus.Confirmed });
            context.SaveChanges();

            var service = new BookingService(context);
            var actual = await service.GetBookingByBookingReferenceAsync("BKG-001");

            Assert.IsTrue(actual.Success);
            Assert.IsNotNull(actual.Data);
            Assert.AreEqual("BKG-001", actual.Data.BookingReference);
        }

        //Edge case - booking dosent excists returns not found
        [TestMethod]
        public async Task GetBookingByBookingReference_InvalidReference_ReturnNotFound()
        {
            using var context = CreateContext("GetBookingByReferenceNotFound");

            var service = new BookingService(context);

            var actual = await service.GetBookingByBookingReferenceAsync("BKG-999");

            Assert.IsFalse(actual.Success);
            Assert.AreEqual(ServiceResultStatus.NotFound, actual.Status);
        }

        [TestMethod]
        public async Task GetBookingByBookingReference_ValidReference_ReturnCorrectMappedData()
        {
            using var context = CreateContext("GetBookingByReferenceMappedData");

            context.Airport.AddRange(
                new Airport { Id = 1, IATACode = "ARN", Name = "Stockholm Arlanda", City = "Stockholm", Country = "Sweden" },
                new Airport { Id = 2, IATACode = "LHR", Name = "Heathrow Airport", City = "London", Country = "UK" }
            );

            context.User.Add(new User { Id = 1, Firstname = "Arthur", Lastname = "Morgan", Email = "arthur@test.com" });
            context.Flight.Add(new Flight { Id = 1, FlightNumber = "SH-101", DepartureAirportId = 1, ArrivalAirportId = 2, DepartureTime = new DateTime(2026, 6, 1), ArrivalTime = new DateTime(2026, 6, 1), TotalSeats = 150, Price = 150m });
            context.Booking.Add(new Booking { BookingReference = "BKG-001", UserId = 1, FlightId = 1, BookingDate = DateTime.Now, TotalPrice = 150m, BookingStatus = BookingStatus.Confirmed });
            context.SaveChanges();

            var service = new BookingService(context);
            var actual = await service.GetBookingByBookingReferenceAsync("BKG-001");

            Assert.AreEqual("Arthur", actual.Data!.Firstname);
            Assert.AreEqual("Stockholm Arlanda", actual.Data!.DepartureAirport);
            Assert.AreEqual("SH-101", actual.Data!.Flightnumber);
            Assert.AreEqual("Confirmed", actual.Data!.BookingStatus);
        }
    }
}
