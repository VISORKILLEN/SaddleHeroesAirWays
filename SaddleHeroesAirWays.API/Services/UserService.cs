using SaddleHeroesAirWays.API.DTOs;
using SaddleHeroesAirWays.API.Services.Interfaces;
using SaddleHeroesAirWays.Library.Models;

namespace SaddleHeroesAirWays.API.Services
{
  public class UserService : IUserService
    {
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
