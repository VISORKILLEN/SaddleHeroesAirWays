namespace SaddleHeroesAirWays.API.DTOs
{
    public record BookingResponse(string? BookingReference, string? Firstname, string? Lastname, string? Flightnumber, string? DepartureAirport, string? ArrivalAirport, DateTime DepartureTime, DateTime BookingDate, decimal TotalPrice, string? BookingStatus, IEnumerable<BookingDetailsResponse>? Details);
    public record CreateBookingRequest(int UserId, int FlightId, IEnumerable<CreateBookingDetails>? Details);

    public record UpdateBooking(int? FlightId);
}
