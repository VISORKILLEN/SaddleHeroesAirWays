using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaddleHeroesAirWays.API.Migrations
{
    /// <inheritdoc />
    public partial class addedforeignkey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingDetails_Booking_BookingReference1",
                table: "BookingDetails");

            migrationBuilder.DropIndex(
                name: "IX_BookingDetails_BookingReference1",
                table: "BookingDetails");

            migrationBuilder.DropColumn(
                name: "BookingReference1",
                table: "BookingDetails");

            migrationBuilder.AlterColumn<string>(
                name: "BookingReference",
                table: "BookingDetails",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookingDetails_BookingReference",
                table: "BookingDetails",
                column: "BookingReference");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingDetails_Booking_BookingReference",
                table: "BookingDetails",
                column: "BookingReference",
                principalTable: "Booking",
                principalColumn: "BookingReference");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingDetails_Booking_BookingReference",
                table: "BookingDetails");

            migrationBuilder.DropIndex(
                name: "IX_BookingDetails_BookingReference",
                table: "BookingDetails");

            migrationBuilder.AlterColumn<string>(
                name: "BookingReference",
                table: "BookingDetails",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BookingReference1",
                table: "BookingDetails",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "BookingDetails",
                keyColumn: "Id",
                keyValue: 1,
                column: "BookingReference1",
                value: null);

            migrationBuilder.UpdateData(
                table: "BookingDetails",
                keyColumn: "Id",
                keyValue: 2,
                column: "BookingReference1",
                value: null);

            migrationBuilder.UpdateData(
                table: "BookingDetails",
                keyColumn: "Id",
                keyValue: 3,
                column: "BookingReference1",
                value: null);

            migrationBuilder.UpdateData(
                table: "BookingDetails",
                keyColumn: "Id",
                keyValue: 4,
                column: "BookingReference1",
                value: null);

            migrationBuilder.UpdateData(
                table: "BookingDetails",
                keyColumn: "Id",
                keyValue: 5,
                column: "BookingReference1",
                value: null);

            migrationBuilder.UpdateData(
                table: "BookingDetails",
                keyColumn: "Id",
                keyValue: 6,
                column: "BookingReference1",
                value: null);

            migrationBuilder.UpdateData(
                table: "BookingDetails",
                keyColumn: "Id",
                keyValue: 7,
                column: "BookingReference1",
                value: null);

            migrationBuilder.UpdateData(
                table: "BookingDetails",
                keyColumn: "Id",
                keyValue: 8,
                column: "BookingReference1",
                value: null);

            migrationBuilder.UpdateData(
                table: "BookingDetails",
                keyColumn: "Id",
                keyValue: 9,
                column: "BookingReference1",
                value: null);

            migrationBuilder.UpdateData(
                table: "BookingDetails",
                keyColumn: "Id",
                keyValue: 10,
                column: "BookingReference1",
                value: null);

            migrationBuilder.UpdateData(
                table: "BookingDetails",
                keyColumn: "Id",
                keyValue: 11,
                column: "BookingReference1",
                value: null);

            migrationBuilder.UpdateData(
                table: "BookingDetails",
                keyColumn: "Id",
                keyValue: 12,
                column: "BookingReference1",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_BookingDetails_BookingReference1",
                table: "BookingDetails",
                column: "BookingReference1");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingDetails_Booking_BookingReference1",
                table: "BookingDetails",
                column: "BookingReference1",
                principalTable: "Booking",
                principalColumn: "BookingReference");
        }
    }
}
