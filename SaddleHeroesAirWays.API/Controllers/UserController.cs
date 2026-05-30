using FluentValidation;
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
        private readonly IValidator<CreateUser> _validator;

        private readonly IValidator<UpdateUser> _updateValidator;

        public UserController(IUserService userService, IValidator<CreateUser> validator, IValidator<UpdateUser> updateValidator)
        {
            _userService = userService;
            _validator = validator;
            _updateValidator = updateValidator;
        }

        [HttpPost("CreateUser")]
        public async Task<ActionResult<UserResponse>> CreateUser(CreateUser userRequest)
        {
            var validationResult = await _validator.ValidateAsync(userRequest);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(error => new
                {
                    field = error.PropertyName,
                    message = error.ErrorMessage
                });

                return BadRequest(errors);
            }

            var serviceResult = await _userService.CreateUserAsync(userRequest);
            return Ok(serviceResult);
        }

        [HttpGet("GetUsersInAlphabeticalOrder")]
        public async Task<IActionResult> GetAllUsersAlphabetical()
        {
            var result = await _userService.GetAllUsersAlphabeticlyAsync();
            return Ok(result);
        }

        //HttpDelete to delete one user.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid user id.");
            }

            var deleted = await _userService.DeleteUserAsync(id);

            if (!deleted)
            {
                return NotFound($"User with id {id} was not found.");
            }

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponse>> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (!user.Success)
            {
                return user.Status switch
                {
                    ServiceResultStatus.NotFound => NotFound(user.ErrorMessage),
                    _ => StatusCode(500, user.ErrorMessage)
                };
            }

            return Ok(user.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UpdateUser request)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid user id.");
            }

            var result = await _userService.UpdateUserAsync(id, request);

            return result.Status switch
            {
                ServiceResultStatus.Success => Ok(result.Data),
                ServiceResultStatus.NotFound => NotFound(result.ErrorMessage),
                _ => BadRequest(result.ErrorMessage)
            };
        }
    }
}
