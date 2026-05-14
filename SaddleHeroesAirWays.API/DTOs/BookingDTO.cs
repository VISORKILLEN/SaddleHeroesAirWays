namespace SaddleHeroesAirWays.API.DTOs
{
    public record BookingResponse(string? BookingReference, string? Firstname, string? Lastname, string? Flightnumber, string? DepartureAirport, string? ArrivalAirport, DateTime DepartureTime, DateTime BookingDate, decimal TotalPrice, string? BookingStatus);
    public record CreateBooking(int UserId, int FlightId);
}
