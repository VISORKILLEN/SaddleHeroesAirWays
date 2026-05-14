namespace SaddleHeroesAirWays.API.DTOs
{
    public record BookingResponse(string? BookingReference, string? Firstname, string? Lastname, string? Flightnumber, string? DepartureAirport, string? ArrivalAirport, DateTime DepartureTime, DateTime BookingDate, decimal TotalPrice, string? BookingStatus, List<BookingDetailsResponse>? Details);
    public record CreateBooking(int UserId, int FlightId, List<BookingDetailsResponse>? Details);

    public record UpdateBooking(int? FlightId, List<CreateBookingDetails>? Details);
}
