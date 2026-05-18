using Microsoft.AspNetCore.Mvc;
using SaddleHeroesAirWays.API.DTOs;
using SaddleHeroesAirWays.API.Services;
using SaddleHeroesAirWays.API.Services.Interfaces;
using SaddleHeroesAirWays.Library.Models;

namespace SaddleHeroesAirWays.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("/CreateUser")]
        public async Task<ActionResult<IEnumerable<CreateUser>>> GetAllPersons(int id, string FirstName, string LastName, string Gender, string Email, string PhoneNumber, string SocialSecurityNumber, bool IsAdmin)
        {
            User NewUser = new User { Id = id, Firstname = FirstName, Lastname = LastName, Gender = Gender, Email = Email, Phonenumber = PhoneNumber, SocialSecurityNumber = SocialSecurityNumber, IsAdmin = IsAdmin};
            return Ok(NewUser);
        }
    }
}
