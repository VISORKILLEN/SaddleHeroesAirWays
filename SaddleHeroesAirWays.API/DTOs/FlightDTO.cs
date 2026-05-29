namespace SaddleHeroesAirWays.API.DTOs
{
    public record FlightResponse(string Flightnumber, string DepartureAirport, string ArrivalAirport, DateTime DepartureTime, DateTime ArrivalTime, int TotalSeats, decimal Price, string? FlightStatus);

    public record UpdateFlight(DateTime DepartureTime, DateTime ArrivalTime, decimal Price, string? FlightStatus);
}
