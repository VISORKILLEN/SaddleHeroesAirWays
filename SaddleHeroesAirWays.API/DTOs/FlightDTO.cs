namespace SaddleHeroesAirWays.API.DTOs
{
    public record FlightResponse(int Id, string Flightnumber, string DepartureAirport, string ArrivalAirport, DateTime DepartureTime, DateTime ArrivalTime, int TotalSeats, decimal Price, string? FlightStatus);

}
