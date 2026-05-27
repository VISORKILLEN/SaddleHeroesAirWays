using Microsoft.AspNetCore.Components.Routing;
using Microsoft.EntityFrameworkCore;
using SaddleHeroesAirWays.API.DTOs;
using SaddleHeroesAirWays.API.Services.Interfaces;
using SaddleHeroesAirWays.Library.Models;

namespace SaddleHeroesAirWays.API.Services
{
  public class UserService : IUserService
    {
        private readonly DbContextAPI _context;

        public UserService(DbContextAPI context)
        {
            _context = context;
        }
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

            await _context.User.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return newUser;
        }

        public async Task<IEnumerable<UserResponse>> GetAllUsersAlphabeticlyAsync()
        {
            var users = await _context.User
                .OrderBy(u => u.Lastname)
                .Select(u => new UserResponse(u.Id, u.Firstname, u.Lastname, u.Email, u.Phonenumber))
                .ToListAsync();

            return users;
        }
    }
}
