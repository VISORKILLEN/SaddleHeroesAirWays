using Microsoft.EntityFrameworkCore;
using SaddleHeroesAirWays.API.DTOs;
using SaddleHeroesAirWays.Library.Models;

namespace SaddleHeroesAirWays.API
{
    public class DbContextAPI : DbContext
    {
        public DbContextAPI(DbContextOptions<DbContextAPI> options) : base(options)
        {
        }

        private static readonly IConfiguration _config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();


        public DbSet<Airport> Airport { get; set; }
        public DbSet<Booking> Booking { get; set; }
        public DbSet<BookingDetails> BookingDetails { get; set; }
        public DbSet<Flight> Flight { get; set; }
        public DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Flight>()
                .HasOne(f => f.DepartureAirport)
                .WithMany(a => a.DepartingFlights)
                .HasForeignKey(f => f.DepartureAirportId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Flight>()
                .HasOne(f => f.ArrivalAirport)
                .WithMany(a => a.ArrivingFlights)
                .HasForeignKey(f => f.ArrivalAirportId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BookingDetails>()
                .HasOne(bd => bd.Booking)
                .WithMany(b => b.BookingDetails)
                .HasForeignKey(bd => bd.BookingReference);

            // 1. SEED AIRPORTS
            modelBuilder.Entity<Airport>().HasData(
                new Airport { Id = 1, IATACode = "ARN", Name = "Stockholm Arlanda", City = "Stockholm", Country = "Sweden" },
                new Airport { Id = 2, IATACode = "LHR", Name = "Heathrow Airport", City = "London", Country = "UK" },
                new Airport { Id = 3, IATACode = "JFK", Name = "John F. Kennedy International", City = "New York", Country = "USA" },
                new Airport { Id = 4, IATACode = "CDG", Name = "Charles de Gaulle", City = "Paris", Country = "France" },
                new Airport { Id = 5, IATACode = "DXB", Name = "Dubai International", City = "Dubai", Country = "UAE" },
                new Airport { Id = 6, IATACode = "FRA", Name = "Frankfurt Airport", City = "Frankfurt", Country = "Germany" }
            );

            // 2. SEED USERS (Saddle Heroes Theme!)
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Firstname = "Arthur", Lastname = "Morgan", Gender = "Male", Email = "arthur.morgan@vandertravel.com", Phonenumber = "555-0101", SocialSecurityNumber = "18630101-1111", IsAdmin = false },
                new User { Id = 2, Firstname = "Sadie", Lastname = "Adler", Gender = "Female", Email = "sadie.adler@bountyhunters.com", Phonenumber = "555-0102", SocialSecurityNumber = "18650202-2222", IsAdmin = true },
                new User { Id = 3, Firstname = "John", Lastname = "Marston", Gender = "Male", Email = "john.marston@ranchers.com", Phonenumber = "555-0103", SocialSecurityNumber = "18730303-3333", IsAdmin = false },
                new User { Id = 4, Firstname = "Dutch", Lastname = "van der Linde", Gender = "Male", Email = "dutch@tahitiplans.com", Phonenumber = "555-0104", SocialSecurityNumber = "18550404-4444", IsAdmin = true },
                new User { Id = 5, Firstname = "Abigail", Lastname = "Roberts", Gender = "Female", Email = "abigail.roberts@ranchers.com", Phonenumber = "555-0105", SocialSecurityNumber = "18770505-5555", IsAdmin = false },
                new User { Id = 6, Firstname = "Charles", Lastname = "Smith", Gender = "Male", Email = "charles.smith@wanderers.com", Phonenumber = "555-0106", SocialSecurityNumber = "18680606-6666", IsAdmin = false },
                new User { Id = 7, Firstname = "Hosea", Lastname = "Matthews", Gender = "Male", Email = "hosea@conartists.com", Phonenumber = "555-0107", SocialSecurityNumber = "18440707-7777", IsAdmin = false },
                new User { Id = 8, Firstname = "Micah", Lastname = "Bell", Gender = "Male", Email = "micah.bell@trouble.com", Phonenumber = "555-0108", SocialSecurityNumber = "18600808-8888", IsAdmin = false },
                new User { Id = 9, Firstname = "Lenny", Lastname = "Summers", Gender = "Male", Email = "lenny.summers@friends.com", Phonenumber = "555-0109", SocialSecurityNumber = "18790909-9999", IsAdmin = false },
                new User { Id = 10, Firstname = "Mary-Beth", Lastname = "Gaskill", Gender = "Female", Email = "marybeth@writers.com", Phonenumber = "555-0110", SocialSecurityNumber = "18781010-0000", IsAdmin = false }
            );

            // 3. SEED FLIGHTS
            modelBuilder.Entity<Flight>().HasData(
                new Flight { Id = 1, FlightNumber = "SH-101", DepartureAirportId = 1, ArrivalAirportId = 2, DepartureTime = new DateTime(2026, 6, 1, 10, 0, 0), ArrivalTime = new DateTime(2026, 6, 1, 11, 30, 0), TotalSeats = 150, Price = 150.00m, FlightStatus = FlightStatus.Arrived },
                new Flight { Id = 2, FlightNumber = "SH-102", DepartureAirportId = 2, ArrivalAirportId = 3, DepartureTime = new DateTime(2026, 6, 2, 14, 0, 0), ArrivalTime = new DateTime(2026, 6, 2, 17, 0, 0), TotalSeats = 200, Price = 600.00m, FlightStatus = FlightStatus.Delayed },
                new Flight { Id = 3, FlightNumber = "SH-103", DepartureAirportId = 3, ArrivalAirportId = 5, DepartureTime = new DateTime(2026, 6, 5, 20, 0, 0), ArrivalTime = new DateTime(2026, 6, 6, 18, 0, 0), TotalSeats = 300, Price = 800.00m, FlightStatus = FlightStatus.Cancelled },
                new Flight { Id = 4, FlightNumber = "SH-104", DepartureAirportId = 4, ArrivalAirportId = 1, DepartureTime = new DateTime(2026, 6, 10, 8, 0, 0), ArrivalTime = new DateTime(2026, 6, 10, 10, 30, 0), TotalSeats = 120, Price = 200.00m, FlightStatus = FlightStatus.Arrived },
                new Flight { Id = 5, FlightNumber = "SH-105", DepartureAirportId = 6, ArrivalAirportId = 2, DepartureTime = new DateTime(2026, 6, 15, 9, 0, 0), ArrivalTime = new DateTime(2026, 6, 15, 9, 45, 0), TotalSeats = 100, Price = 100.00m, FlightStatus = FlightStatus.Arrived },
                new Flight { Id = 6, FlightNumber = "SH-106", DepartureAirportId = 5, ArrivalAirportId = 1, DepartureTime = new DateTime(2026, 6, 20, 2, 0, 0), ArrivalTime = new DateTime(2026, 6, 20, 7, 0, 0), TotalSeats = 250, Price = 500.00m, FlightStatus = FlightStatus.Arrived },
                new Flight { Id = 7, FlightNumber = "SH-107", DepartureAirportId = 1, ArrivalAirportId = 3, DepartureTime = new DateTime(2026, 7, 1, 11, 0, 0), ArrivalTime = new DateTime(2026, 7, 1, 14, 0, 0), TotalSeats = 200, Price = 700.00m, FlightStatus = FlightStatus.Arrived },
                new Flight { Id = 8, FlightNumber = "SH-108", DepartureAirportId = 2, ArrivalAirportId = 4, DepartureTime = new DateTime(2026, 7, 5, 16, 0, 0), ArrivalTime = new DateTime(2026, 7, 5, 18, 0, 0), TotalSeats = 150, Price = 120.00m, FlightStatus = FlightStatus.Arrived },
                new Flight { Id = 9, FlightNumber = "SH-109", DepartureAirportId = 3, ArrivalAirportId = 6, DepartureTime = new DateTime(2026, 7, 10, 22, 0, 0), ArrivalTime = new DateTime(2026, 7, 11, 11, 0, 0), TotalSeats = 220, Price = 650.00m, FlightStatus = FlightStatus.Delayed },
                new Flight { Id = 10, FlightNumber = "SH-110", DepartureAirportId = 4, ArrivalAirportId = 5, DepartureTime = new DateTime(2026, 7, 15, 13, 0, 0), ArrivalTime = new DateTime(2026, 7, 15, 22, 0, 0), TotalSeats = 280, Price = 550.00m, FlightStatus = FlightStatus.Arrived }
            );

            // 4. SEED BOOKINGS
            modelBuilder.Entity<Booking>().HasData(
                new Booking { BookingReference = "BKG-1001", UserId = 1, FlightId = 1, BookingDate = new DateTime(2026, 5, 1), TotalPrice = 150.00m, BookingStatus = BookingStatus.Confirmed },
                new Booking { BookingReference = "BKG-1002", UserId = 2, FlightId = 2, BookingDate = new DateTime(2026, 5, 2), TotalPrice = 600.00m, BookingStatus = BookingStatus.Confirmed },
                new Booking { BookingReference = "BKG-1003", UserId = 3, FlightId = 3, BookingDate = new DateTime(2026, 5, 3), TotalPrice = 800.00m, BookingStatus = BookingStatus.Cancelled },
                new Booking { BookingReference = "BKG-1004", UserId = 4, FlightId = 4, BookingDate = new DateTime(2026, 5, 4), TotalPrice = 200.00m, BookingStatus = BookingStatus.Confirmed },
                new Booking { BookingReference = "BKG-1005", UserId = 5, FlightId = 5, BookingDate = new DateTime(2026, 5, 5), TotalPrice = 100.00m, BookingStatus = BookingStatus.Confirmed },
                new Booking { BookingReference = "BKG-1006", UserId = 6, FlightId = 6, BookingDate = new DateTime(2026, 5, 6), TotalPrice = 500.00m, BookingStatus = BookingStatus.Confirmed },
                new Booking { BookingReference = "BKG-1007", UserId = 7, FlightId = 7, BookingDate = new DateTime(2026, 5, 7), TotalPrice = 700.00m, BookingStatus = BookingStatus.Rebooked },
                new Booking { BookingReference = "BKG-1008", UserId = 8, FlightId = 8, BookingDate = new DateTime(2026, 5, 8), TotalPrice = 120.00m, BookingStatus = BookingStatus.Confirmed },
                new Booking { BookingReference = "BKG-1009", UserId = 9, FlightId = 9, BookingDate = new DateTime(2026, 5, 9), TotalPrice = 650.00m, BookingStatus = BookingStatus.Confirmed },
                new Booking { BookingReference = "BKG-1010", UserId = 10, FlightId = 10, BookingDate = new DateTime(2026, 5, 10), TotalPrice = 550.00m, BookingStatus = BookingStatus.Confirmed },
                new Booking { BookingReference = "BKG-1011", UserId = 1, FlightId = 2, BookingDate = new DateTime(2026, 5, 15), TotalPrice = 600.00m, BookingStatus = BookingStatus.Confirmed },
                new Booking { BookingReference = "BKG-1012", UserId = 2, FlightId = 7, BookingDate = new DateTime(2026, 5, 20), TotalPrice = 700.00m, BookingStatus = BookingStatus.Confirmed }
            );

            // 5. SEED BOOKING DETAILS
            modelBuilder.Entity<BookingDetails>().HasData(
                new BookingDetails { Id = 1, BookingReference = "BKG-1001", Seatnumber = "12A", Baggage = true, Notes = "Vegetarian meal requested" },
                new BookingDetails { Id = 2, BookingReference = "BKG-1002", Seatnumber = "1A", Baggage = true, Notes = "VIP Customer" },
                new BookingDetails { Id = 3, BookingReference = "BKG-1003", Seatnumber = "15C", Baggage = false, Notes = "" },
                new BookingDetails { Id = 4, BookingReference = "BKG-1004", Seatnumber = "8F", Baggage = true, Notes = "Needs wheelchair assistance" },
                new BookingDetails { Id = 5, BookingReference = "BKG-1005", Seatnumber = "22B", Baggage = false, Notes = "Window seat preferred" },
                new BookingDetails { Id = 6, BookingReference = "BKG-1006", Seatnumber = "3A", Baggage = true, Notes = "Extra legroom paid" },
                new BookingDetails { Id = 7, BookingReference = "BKG-1007", Seatnumber = "14D", Baggage = true, Notes = "Traveling with pet" },
                new BookingDetails { Id = 8, BookingReference = "BKG-1008", Seatnumber = "9E", Baggage = false, Notes = "" },
                new BookingDetails { Id = 9, BookingReference = "BKG-1009", Seatnumber = "2C", Baggage = true, Notes = "Allergic to peanuts" },
                new BookingDetails { Id = 10, BookingReference = "BKG-1010", Seatnumber = "11A", Baggage = true, Notes = "" },
                new BookingDetails { Id = 11, BookingReference = "BKG-1011", Seatnumber = "1B", Baggage = true, Notes = "Traveling with VIP" },
                new BookingDetails { Id = 12, BookingReference = "BKG-1012", Seatnumber = "4F", Baggage = true, Notes = "" }
            );
        }

    }
}
