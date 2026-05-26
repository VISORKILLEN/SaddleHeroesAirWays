using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SaddleHeroesAirWays.API.Controllers;
using SaddleHeroesAirWays.API.DTOs;
using SaddleHeroesAirWays.API.Services.Interfaces;
using SaddleHeroesAirWays.Library.Models;

namespace SaddleHeroesAirWays.MSTest
{
    [TestClass]
    public class UserControllerTests
    {
        private Mock<IUserService>? _userServiceMock;
        private Mock<IValidator<CreateUser>> _validatorMock = null!;

        private UserController? _userController;

        [TestInitialize]
        public void setup()
        {
            _validatorMock = new Mock<IValidator<CreateUser>>();
            _userServiceMock = new Mock<IUserService>();
            _userController = new UserController(_userServiceMock.Object, _validatorMock.Object);
        }

        [TestMethod]
        public async Task CreateUser_CreateANewUser_ReturnTrue()
        {
            var createUserRequest = new CreateUser(
                 Gender: "Male",
                 Firstname: "John",
                 Lastname: "Doe",
                 Email: "john.doe@example.com",
                 Phonenumber: "123-456-7890",
                 SocialSecurityNumber: "19900101-1234"
            );

            var createdUser = new User
            {
                Firstname = "John",
                Lastname = "Doe",
                Email = "john.doe@example.com",
                SocialSecurityNumber = "19900101-1234"
            };

            _validatorMock
                .Setup(v => v.ValidateAsync(createUserRequest, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _userServiceMock
                .Setup(s => s.CreateUserAsync(createUserRequest))
                .ReturnsAsync(createdUser);

            var result = await _userController.CreateUser(createUserRequest);

            var okResult = result.Result as OkObjectResult;

            Assert.IsNotNull(okResult, "Förväntad att controllern kommer returna OK() result :)");
            Assert.AreEqual(200, okResult.StatusCode);

            var returnedUser = okResult.Value as User;
            Assert.IsNotNull(returnedUser);
            Assert.AreEqual("19900101-1234", returnedUser.SocialSecurityNumber);
        }

        [TestMethod]
        public async Task CreateUser_ValidationFailWhenCreate_ReturnsBadRequest()
        {
            var badCreateUserRequest = new CreateUser(
                 Gender: "Male",
                 Firstname: "John",
                 Lastname: "Doe",
                 Email: "",
                 Phonenumber: "123-456-7890",
                 SocialSecurityNumber: "19900101-1234"
            );

            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure("Email", "Email cannot be empty")
            };

            _validatorMock
                .Setup(v => v.ValidateAsync(badCreateUserRequest, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(validationFailures));

            var result = await _userController.CreateUser(badCreateUserRequest);

            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult, "Should return 400 Bad Request");
            Assert.AreEqual(400, badRequestResult.StatusCode);

        }
    }
}
