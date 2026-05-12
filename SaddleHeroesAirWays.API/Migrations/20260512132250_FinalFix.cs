using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SaddleHeroesAirWays.API.Migrations
{
    /// <inheritdoc />
    public partial class FinalFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "airport",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IATACode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_airport", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Firstname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lastname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phonenumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SocialSecurityNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "flight",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlightNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepartureAirportId = table.Column<int>(type: "int", nullable: false),
                    ArrivalAirportId = table.Column<int>(type: "int", nullable: false),
                    DepartureTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ArrivalTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalSeats = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FlightStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_flight", x => x.Id);
                    table.ForeignKey(
                        name: "FK_flight_airport_ArrivalAirportId",
                        column: x => x.ArrivalAirportId,
                        principalTable: "airport",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_flight_airport_DepartureAirportId",
                        column: x => x.DepartureAirportId,
                        principalTable: "airport",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "booking",
                columns: table => new
                {
                    BookingReference = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    FlightId = table.Column<int>(type: "int", nullable: false),
                    BookingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BookingStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_booking", x => x.BookingReference);
                    table.ForeignKey(
                        name: "FK_booking_flight_FlightId",
                        column: x => x.FlightId,
                        principalTable: "flight",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_booking_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bookingDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Seatnumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Baggage = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BookingReference1 = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bookingDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_bookingDetails_booking_BookingReference1",
                        column: x => x.BookingReference1,
                        principalTable: "booking",
                        principalColumn: "BookingReference");
                });

            migrationBuilder.InsertData(
                table: "airport",
                columns: new[] { "Id", "City", "Country", "IATACode", "Name" },
                values: new object[,]
                {
                    { 1, "Stockholm", "Sweden", "ARN", "Stockholm Arlanda" },
                    { 2, "London", "UK", "LHR", "Heathrow Airport" },
                    { 3, "New York", "USA", "JFK", "John F. Kennedy International" },
                    { 4, "Paris", "France", "CDG", "Charles de Gaulle" },
                    { 5, "Dubai", "UAE", "DXB", "Dubai International" },
                    { 6, "Frankfurt", "Germany", "FRA", "Frankfurt Airport" }
                });

            migrationBuilder.InsertData(
                table: "bookingDetails",
                columns: new[] { "Id", "Baggage", "BookingReference", "BookingReference1", "Notes", "Seatnumber" },
                values: new object[,]
                {
                    { 1, true, "BKG-1001", null, "Vegetarian meal requested", "12A" },
                    { 2, true, "BKG-1002", null, "VIP Customer", "1A" },
                    { 3, false, "BKG-1003", null, "", "15C" },
                    { 4, true, "BKG-1004", null, "Needs wheelchair assistance", "8F" },
                    { 5, false, "BKG-1005", null, "Window seat preferred", "22B" },
                    { 6, true, "BKG-1006", null, "Extra legroom paid", "3A" },
                    { 7, true, "BKG-1007", null, "Traveling with pet", "14D" },
                    { 8, false, "BKG-1008", null, "", "9E" },
                    { 9, true, "BKG-1009", null, "Allergic to peanuts", "2C" },
                    { 10, true, "BKG-1010", null, "", "11A" },
                    { 11, true, "BKG-1011", null, "Traveling with VIP", "1B" },
                    { 12, true, "BKG-1012", null, "", "4F" }
                });

            migrationBuilder.InsertData(
                table: "user",
                columns: new[] { "Id", "Email", "Firstname", "Gender", "IsAdmin", "Lastname", "Phonenumber", "SocialSecurityNumber" },
                values: new object[,]
                {
                    { 1, "arthur.morgan@vandertravel.com", "Arthur", "Male", false, "Morgan", "555-0101", "18630101-1111" },
                    { 2, "sadie.adler@bountyhunters.com", "Sadie", "Female", true, "Adler", "555-0102", "18650202-2222" },
                    { 3, "john.marston@ranchers.com", "John", "Male", false, "Marston", "555-0103", "18730303-3333" },
                    { 4, "dutch@tahitiplans.com", "Dutch", "Male", true, "van der Linde", "555-0104", "18550404-4444" },
                    { 5, "abigail.roberts@ranchers.com", "Abigail", "Female", false, "Roberts", "555-0105", "18770505-5555" },
                    { 6, "charles.smith@wanderers.com", "Charles", "Male", false, "Smith", "555-0106", "18680606-6666" },
                    { 7, "hosea@conartists.com", "Hosea", "Male", false, "Matthews", "555-0107", "18440707-7777" },
                    { 8, "micah.bell@trouble.com", "Micah", "Male", false, "Bell", "555-0108", "18600808-8888" },
                    { 9, "lenny.summers@friends.com", "Lenny", "Male", false, "Summers", "555-0109", "18790909-9999" },
                    { 10, "marybeth@writers.com", "Mary-Beth", "Female", false, "Gaskill", "555-0110", "18781010-0000" }
                });

            migrationBuilder.InsertData(
                table: "flight",
                columns: new[] { "Id", "ArrivalAirportId", "ArrivalTime", "DepartureAirportId", "DepartureTime", "FlightNumber", "FlightStatus", "Price", "TotalSeats" },
                values: new object[,]
                {
                    { 1, 2, new DateTime(2026, 6, 1, 11, 30, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2026, 6, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), "SH-101", 0, 150.00m, 150 },
                    { 2, 3, new DateTime(2026, 6, 2, 17, 0, 0, 0, DateTimeKind.Unspecified), 2, new DateTime(2026, 6, 2, 14, 0, 0, 0, DateTimeKind.Unspecified), "SH-102", 1, 600.00m, 200 },
                    { 3, 5, new DateTime(2026, 6, 6, 18, 0, 0, 0, DateTimeKind.Unspecified), 3, new DateTime(2026, 6, 5, 20, 0, 0, 0, DateTimeKind.Unspecified), "SH-103", 2, 800.00m, 300 },
                    { 4, 1, new DateTime(2026, 6, 10, 10, 30, 0, 0, DateTimeKind.Unspecified), 4, new DateTime(2026, 6, 10, 8, 0, 0, 0, DateTimeKind.Unspecified), "SH-104", 0, 200.00m, 120 },
                    { 5, 2, new DateTime(2026, 6, 15, 9, 45, 0, 0, DateTimeKind.Unspecified), 6, new DateTime(2026, 6, 15, 9, 0, 0, 0, DateTimeKind.Unspecified), "SH-105", 0, 100.00m, 100 },
                    { 6, 1, new DateTime(2026, 6, 20, 7, 0, 0, 0, DateTimeKind.Unspecified), 5, new DateTime(2026, 6, 20, 2, 0, 0, 0, DateTimeKind.Unspecified), "SH-106", 0, 500.00m, 250 },
                    { 7, 3, new DateTime(2026, 7, 1, 14, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2026, 7, 1, 11, 0, 0, 0, DateTimeKind.Unspecified), "SH-107", 0, 700.00m, 200 },
                    { 8, 4, new DateTime(2026, 7, 5, 18, 0, 0, 0, DateTimeKind.Unspecified), 2, new DateTime(2026, 7, 5, 16, 0, 0, 0, DateTimeKind.Unspecified), "SH-108", 0, 120.00m, 150 },
                    { 9, 6, new DateTime(2026, 7, 11, 11, 0, 0, 0, DateTimeKind.Unspecified), 3, new DateTime(2026, 7, 10, 22, 0, 0, 0, DateTimeKind.Unspecified), "SH-109", 1, 650.00m, 220 },
                    { 10, 5, new DateTime(2026, 7, 15, 22, 0, 0, 0, DateTimeKind.Unspecified), 4, new DateTime(2026, 7, 15, 13, 0, 0, 0, DateTimeKind.Unspecified), "SH-110", 0, 550.00m, 280 }
                });

            migrationBuilder.InsertData(
                table: "booking",
                columns: new[] { "BookingReference", "BookingDate", "BookingStatus", "FlightId", "TotalPrice", "UserId" },
                values: new object[,]
                {
                    { "BKG-1001", new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 1, 150.00m, 1 },
                    { "BKG-1002", new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 2, 600.00m, 2 },
                    { "BKG-1003", new DateTime(2026, 5, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 3, 800.00m, 3 },
                    { "BKG-1004", new DateTime(2026, 5, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 4, 200.00m, 4 },
                    { "BKG-1005", new DateTime(2026, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 5, 100.00m, 5 },
                    { "BKG-1006", new DateTime(2026, 5, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 6, 500.00m, 6 },
                    { "BKG-1007", new DateTime(2026, 5, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 7, 700.00m, 7 },
                    { "BKG-1008", new DateTime(2026, 5, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 8, 120.00m, 8 },
                    { "BKG-1009", new DateTime(2026, 5, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 9, 650.00m, 9 },
                    { "BKG-1010", new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 10, 550.00m, 10 },
                    { "BKG-1011", new DateTime(2026, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 2, 600.00m, 1 },
                    { "BKG-1012", new DateTime(2026, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 7, 700.00m, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_booking_FlightId",
                table: "booking",
                column: "FlightId");

            migrationBuilder.CreateIndex(
                name: "IX_booking_UserId",
                table: "booking",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_bookingDetails_BookingReference1",
                table: "bookingDetails",
                column: "BookingReference1");

            migrationBuilder.CreateIndex(
                name: "IX_flight_ArrivalAirportId",
                table: "flight",
                column: "ArrivalAirportId");

            migrationBuilder.CreateIndex(
                name: "IX_flight_DepartureAirportId",
                table: "flight",
                column: "DepartureAirportId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bookingDetails");

            migrationBuilder.DropTable(
                name: "booking");

            migrationBuilder.DropTable(
                name: "flight");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "airport");
        }
    }
}
