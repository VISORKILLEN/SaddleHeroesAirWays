using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ServiceResult<UserResponse?>> CreateUserAsync(CreateUser request)
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

            var createdUser = await MapUserToResponse(
               _context.User.Where(u => u.Id == newUser.Id))
               .FirstOrDefaultAsync();

            return ServiceResult<UserResponse>.Ok(createdUser);
        }

        public async Task<IEnumerable<UserResponse>> GetAllUsersAlphabeticlyAsync()
        {
            var users = await _context.User
                .OrderBy(u => u.Lastname)
                .Select(u => new UserResponse(u.Id, u.Firstname, u.Lastname, u.Email, u.Phonenumber))
                .ToListAsync();

            return users;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.User.FindAsync(id);

            if(user == null)
            {
                return false;
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ServiceResult<UserResponse>> GetUserByIdAsync(int id)
        {
            var user = await _context.User
                .AsNoTracking()
                .Where(u => u.Id == id)
                .Select(u => new UserResponse(
                    u.Id,
                    u.Firstname,
                    u.Lastname,
                    u.Email,
                    u.Phonenumber
                    ))
                .FirstOrDefaultAsync();

            if(user == null)
            {
                return ServiceResult<UserResponse>.NotFound($"User {id} could not be found");
            }

            return ServiceResult<UserResponse>.Ok(user);
        }

        private static IQueryable<UserResponse> MapUserToResponse(IQueryable<User> query)
        {
            return query.Select(u => new UserResponse(
                u.Id,
                u.Firstname,
                u.Lastname,
                u.Email,
                u.Phonenumber
            ));
        }

        public async Task<ServiceResult<User?>> UpdateUserAsync(int id, UpdateUser request)
        {
            var user = await _context.User.FindAsync(id);

            if (user == null)
                return ServiceResult<User?>.NotFound($"User with id {id} was not found.");

            user.Firstname = request.Firstname ?? user.Firstname;
            user.Lastname = request.Lastname ?? user.Lastname;
            user.Email = request.Email ?? user.Email;
            user.Phonenumber = request.Phonenumber ?? user.Phonenumber;

            await _context.SaveChangesAsync();
            return ServiceResult<User?>.Ok(user);
        }
    }
}
