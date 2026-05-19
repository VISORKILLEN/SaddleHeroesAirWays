using SaddleHeroesAirWays.Library.Models;

namespace SaddleHeroesAirWays.API.Services.Interfaces
{
    public interface IBookingService
    {
        Task<List<Booking>> GetBookingsForMonthAsync(DateTime date);
    }
}
