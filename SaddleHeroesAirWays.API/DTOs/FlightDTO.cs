namespace SaddleHeroesAirWays.API.DTOs
{
    public record FlightResponse(string Flightnumber, string DepartureAirport, string ArrivalAirport, DateTime DepartureTime, DateTime ArrivalTime, int TotalSeats, decimal Price, string? FlightStatus);

    //behövs en createFlight?

    public record UpdateFlight(DateTime DepartureTime, DateTime ArrivalTime, decimal Price, string? FlightStatus);
}
