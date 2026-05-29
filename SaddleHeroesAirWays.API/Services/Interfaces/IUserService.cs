using SaddleHeroesAirWays.API.DTOs;
using SaddleHeroesAirWays.Library.Models;

namespace SaddleHeroesAirWays.API.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<CreateUser>> CreateUserAsync(CreateUser request);

        Task<IEnumerable<UserResponse>> GetAllUsersAlphabeticlyAsync();
        Task<bool> DeleteUserAsync(int id);
    }
}
