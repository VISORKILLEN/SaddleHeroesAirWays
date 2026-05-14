namespace SaddleHeroesAirWays.API.DTOs
{
    public record BookingDetailsResponse(int Id, string? SeatNumber, bool Baggage, string? Notes);
    public record CreateBookingDetails(string? SeatNumber, bool Baggage, string? Notes);
}
