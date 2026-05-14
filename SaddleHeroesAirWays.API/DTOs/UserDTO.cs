namespace SaddleHeroesAirWays.API.DTOs
{
    public record UserResponse(int Id, string? Firstname, string? Lastname, string? Email, string? Phonenumber);
    public record CreateUser(string? Gender, string? Firstname, string? Lastname, string? Email, string? Phonenumber, string SocialSecurityNumber);
    public record UpdateUser(string? FIrstname, string? Lastname, string? Email, string? Phonenumber);
}
