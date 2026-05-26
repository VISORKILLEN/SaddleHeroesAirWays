using SaddleHeroesAirWays.API.DTOs;
using SaddleHeroesAirWays.Library.Models;

namespace SaddleHeroesAirWays.API.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(CreateUser request);

        Task<IEnumerable<User>> GetAllUsersAlphabeticlyAsync(UserResponse response);
    }
}
