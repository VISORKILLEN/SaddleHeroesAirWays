using SaddleHeroesAirWays.API.DTOs;

namespace SaddleHeroesAirWays.API.Services.Interfaces
{
    public interface IBookingService
    {
        Task<IEnumerable<BookingResponse>> GetBookingsByUserIdAsync(int userId);
    }
}
