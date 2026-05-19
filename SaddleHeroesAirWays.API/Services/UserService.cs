using SaddleHeroesAirWays.API.DTOs;
using SaddleHeroesAirWays.Library.Models;

namespace SaddleHeroesAirWays.API.Services
{
    public interface IUserService // Move to interface folder
    {
        Task<User> CreateUserAsync(CreateUser request);
    }
  public class UserService : IUserService
    { // Task<User> can maybe be changed to branch
        public async Task<User> CreateUserAsync(CreateUser request)
        {
            var newUser = new User
            {
                Firstname = request.Firstname,
                Lastname = request.Lastname,
                Gender = request.Gender,
                Email = request.Email,
                Phonenumber = request.Phonenumber,
                SocialSecurityNumber = request.SocialSecurityNumber,
                IsAdmin = false                             
            };

            await Task.CompletedTask;

            return newUser;
        }
    }
}
