using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SaddleHeroesAirWays.API.Controllers;
using SaddleHeroesAirWays.API.DTOs;
using SaddleHeroesAirWays.API.Services;
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

            var createdUserResponse = new UserResponse(
                 1,
                 "John",
                 "Doe",
                 "john.doe@example.com",
                 "123-456-7890"
            );

            var serviceResult = ServiceResult<UserResponse>.Ok(createdUserResponse);

            _validatorMock
                .Setup(v => v.ValidateAsync(createUserRequest, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _userServiceMock
                .Setup(s => s.CreateUserAsync(createUserRequest))
                .ReturnsAsync(serviceResult);

            var result = await _userController.CreateUser(createUserRequest);

            var okResult = result.Result as OkObjectResult;

            Assert.IsNotNull(okResult, "Förväntad att controllern kommer returna OK() result :)");
            Assert.AreEqual(200, okResult.StatusCode);

            var returnedResult = okResult.Value as ServiceResult<UserResponse>;

            Assert.IsNotNull(returnedResult, "förväntat att värded som är returned är en ServiceResult<UserResponse>");
            Assert.IsNotNull(returnedResult.Data);

            Assert.AreEqual("john.doe@example.com", returnedResult.Data.Email);
            Assert.AreEqual("John", returnedResult.Data.Firstname);
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

        [TestMethod]
        public async Task GetAllUsersAlphabetical_ShouldReturnOkWithSortedUsers()
        {
            var mockUsers = new List<UserResponse>
            {
                new UserResponse(1, "Bob", "Adams", "bob@gmail.com", "0192334354"),
                new UserResponse(2, "Alice", "Smith", "alice@gmail.com", "0192334355"),
                new UserResponse(3, "Charlie", "Zane", "charlie@gmail.com", "0192334356")
            };

            _userServiceMock!
                .Setup(s => s.GetAllUsersAlphabeticlyAsync())
                .ReturnsAsync(mockUsers);

            var result = await _userController!.GetAllUsersAlphabetical();

            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult, "förväntad att returna ett Ok() result");
            Assert.AreEqual(200, okResult.StatusCode);

            var returnedUsers = okResult.Value as IEnumerable<UserResponse>;
            Assert.IsNotNull(returnedUsers, "Expected the returned value to be a collection of UserResponse");

            var returnedUserList = returnedUsers.ToList();
            Assert.AreEqual(3, returnedUserList.Count);

            Assert.AreEqual("Adams", returnedUserList[0].Lastname);
            Assert.AreEqual("Smith", returnedUserList[1].Lastname);
            Assert.AreEqual("Zane", returnedUserList[2].Lastname);

        }

        [TestMethod]
        public async Task GetAllUsersAlphabetical_ReturnAListOfNullUsers_ReturnsOk()
        {
            var mockUsers = new List<UserResponse>
            {
            };

            _userServiceMock!
                .Setup(s => s.GetAllUsersAlphabeticlyAsync())
                .ReturnsAsync(mockUsers);

            var result = await _userController!.GetAllUsersAlphabetical();

            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult, "förväntad att returna ett Ok() result");
            Assert.AreEqual(200, okResult.StatusCode);

            var returnedUsers = okResult.Value as IEnumerable<UserResponse>;
            Assert.IsNotNull(returnedUsers, "Expected the returned value to be a collection of UserResponse");

            var returnedUserList = returnedUsers.ToList();
            Assert.AreEqual(0, returnedUserList.Count);

        }

        // Happy path, valid id returns 204 NoContent
        [TestMethod]
        public async Task DeleteUser_ValidId_ReturnsNoContent()
        {
            _userServiceMock!
                .Setup(s => s.DeleteUserAsync(1))
                .ReturnsAsync(true);

            var result = await _userController!.DeleteUser(1);

            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        // Edge case, user not found returns 404
        [TestMethod]
        public async Task DeleteUser_UserNotFound_ReturnsNotFound()
        {
            _userServiceMock!
                .Setup(s => s.DeleteUserAsync(99))
                .ReturnsAsync(false);

            var result = await _userController!.DeleteUser(99);

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }
    }
}
