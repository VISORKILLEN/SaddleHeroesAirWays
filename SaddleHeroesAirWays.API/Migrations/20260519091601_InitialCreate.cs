using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaddleHeroesAirWays.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_booking_flight_FlightId",
                table: "booking");

            migrationBuilder.DropForeignKey(
                name: "FK_booking_user_UserId",
                table: "booking");

            migrationBuilder.DropForeignKey(
                name: "FK_bookingDetails_booking_BookingReference1",
                table: "bookingDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_flight_airport_ArrivalAirportId",
                table: "flight");

            migrationBuilder.DropForeignKey(
                name: "FK_flight_airport_DepartureAirportId",
                table: "flight");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user",
                table: "user");

            migrationBuilder.DropPrimaryKey(
                name: "PK_flight",
                table: "flight");

            migrationBuilder.DropPrimaryKey(
                name: "PK_bookingDetails",
                table: "bookingDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_booking",
                table: "booking");

            migrationBuilder.DropPrimaryKey(
                name: "PK_airport",
                table: "airport");

            migrationBuilder.RenameTable(
                name: "user",
                newName: "User");

            migrationBuilder.RenameTable(
                name: "flight",
                newName: "Flight");

            migrationBuilder.RenameTable(
                name: "bookingDetails",
                newName: "BookingDetails");

            migrationBuilder.RenameTable(
                name: "booking",
                newName: "Booking");

            migrationBuilder.RenameTable(
                name: "airport",
                newName: "Airport");

            migrationBuilder.RenameIndex(
                name: "IX_flight_DepartureAirportId",
                table: "Flight",
                newName: "IX_Flight_DepartureAirportId");

            migrationBuilder.RenameIndex(
                name: "IX_flight_ArrivalAirportId",
                table: "Flight",
                newName: "IX_Flight_ArrivalAirportId");

            migrationBuilder.RenameIndex(
                name: "IX_bookingDetails_BookingReference1",
                table: "BookingDetails",
                newName: "IX_BookingDetails_BookingReference1");

            migrationBuilder.RenameIndex(
                name: "IX_booking_UserId",
                table: "Booking",
                newName: "IX_Booking_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_booking_FlightId",
                table: "Booking",
                newName: "IX_Booking_FlightId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Flight",
                table: "Flight",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookingDetails",
                table: "BookingDetails",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Booking",
                table: "Booking",
                column: "BookingReference");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Airport",
                table: "Airport",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Flight_FlightId",
                table: "Booking",
                column: "FlightId",
                principalTable: "Flight",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_User_UserId",
                table: "Booking",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookingDetails_Booking_BookingReference1",
                table: "BookingDetails",
                column: "BookingReference1",
                principalTable: "Booking",
                principalColumn: "BookingReference");

            migrationBuilder.AddForeignKey(
                name: "FK_Flight_Airport_ArrivalAirportId",
                table: "Flight",
                column: "ArrivalAirportId",
                principalTable: "Airport",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Flight_Airport_DepartureAirportId",
                table: "Flight",
                column: "DepartureAirportId",
                principalTable: "Airport",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Flight_FlightId",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Booking_User_UserId",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_BookingDetails_Booking_BookingReference1",
                table: "BookingDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Flight_Airport_ArrivalAirportId",
                table: "Flight");

            migrationBuilder.DropForeignKey(
                name: "FK_Flight_Airport_DepartureAirportId",
                table: "Flight");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Flight",
                table: "Flight");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookingDetails",
                table: "BookingDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Booking",
                table: "Booking");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Airport",
                table: "Airport");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "user");

            migrationBuilder.RenameTable(
                name: "Flight",
                newName: "flight");

            migrationBuilder.RenameTable(
                name: "BookingDetails",
                newName: "bookingDetails");

            migrationBuilder.RenameTable(
                name: "Booking",
                newName: "booking");

            migrationBuilder.RenameTable(
                name: "Airport",
                newName: "airport");

            migrationBuilder.RenameIndex(
                name: "IX_Flight_DepartureAirportId",
                table: "flight",
                newName: "IX_flight_DepartureAirportId");

            migrationBuilder.RenameIndex(
                name: "IX_Flight_ArrivalAirportId",
                table: "flight",
                newName: "IX_flight_ArrivalAirportId");

            migrationBuilder.RenameIndex(
                name: "IX_BookingDetails_BookingReference1",
                table: "bookingDetails",
                newName: "IX_bookingDetails_BookingReference1");

            migrationBuilder.RenameIndex(
                name: "IX_Booking_UserId",
                table: "booking",
                newName: "IX_booking_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Booking_FlightId",
                table: "booking",
                newName: "IX_booking_FlightId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user",
                table: "user",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_flight",
                table: "flight",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_bookingDetails",
                table: "bookingDetails",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_booking",
                table: "booking",
                column: "BookingReference");

            migrationBuilder.AddPrimaryKey(
                name: "PK_airport",
                table: "airport",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_booking_flight_FlightId",
                table: "booking",
                column: "FlightId",
                principalTable: "flight",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_booking_user_UserId",
                table: "booking",
                column: "UserId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_bookingDetails_booking_BookingReference1",
                table: "bookingDetails",
                column: "BookingReference1",
                principalTable: "booking",
                principalColumn: "BookingReference");

            migrationBuilder.AddForeignKey(
                name: "FK_flight_airport_ArrivalAirportId",
                table: "flight",
                column: "ArrivalAirportId",
                principalTable: "airport",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_flight_airport_DepartureAirportId",
                table: "flight",
                column: "DepartureAirportId",
                principalTable: "airport",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
