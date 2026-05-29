using SaddleHeroesAirWays.API.DTOs;
using SaddleHeroesAirWays.Library.Models;

namespace SaddleHeroesAirWays.API.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(CreateUser request);

        Task<IEnumerable<UserResponse>> GetAllUsersAlphabeticlyAsync();
        Task<bool> DeleteUserAsync(int id);

        Task<ServiceResult<UserResponse>> GetUserByIdAsync(int id);
    }
}
