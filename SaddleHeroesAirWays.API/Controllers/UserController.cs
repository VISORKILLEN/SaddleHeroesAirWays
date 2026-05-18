using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SaddleHeroesAirWays.API.DTOs;
using SaddleHeroesAirWays.API.Services;
using SaddleHeroesAirWays.Library.Models;

namespace SaddleHeroesAirWays.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IValidator<CreateUser> _validator;

        public UserController(IUserService IuserService, IValidator<CreateUser> validator)
        {
            _userService = IuserService;
            _validator = validator;
        }

        [HttpPost("CreateUser")]
        public async Task<ActionResult<User>> CreateUser(CreateUser userRequest)
        {
            var validationResult = await _validator.ValidateAsync(userRequest);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToDictionary());
            }

            User newUser = await _userService.CreateUserAsync(userRequest);

            return Ok(newUser);
        }
    }
}
