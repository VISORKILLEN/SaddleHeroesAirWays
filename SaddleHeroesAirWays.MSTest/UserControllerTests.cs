using Azure.Core;
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
        private Mock<IUserService> _userServiceMock = null!;
        private Mock<IValidator<CreateUser>> _validatorMock = null!;
        private Mock<IValidator<UpdateUser>> _updateValidatorMock = null!;
        private UserController _userController = null!;

        [TestInitialize]
        public void Setup()
        {
            _validatorMock = new Mock<IValidator<CreateUser>>();
            _updateValidatorMock = new Mock<IValidator<UpdateUser>>();
            _userServiceMock = new Mock<IUserService>();
            _userController = new UserController(_userServiceMock.Object, _validatorMock.Object, _updateValidatorMock.Object);
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

            _userServiceMock.Verify(s => s.CreateUserAsync(createUserRequest), Times.Once);
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

            _userServiceMock
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
            var mockUsers = new List<UserResponse>();

            _userServiceMock
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
            _userServiceMock
                .Setup(s => s.DeleteUserAsync(1))
                .ReturnsAsync(true);

            var result = await _userController!.DeleteUser(1);

            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        // Edge case, user not found returns 404
        [TestMethod]
        public async Task DeleteUser_UserNotFound_ReturnsNotFound()
        {
            _userServiceMock
                .Setup(s => s.DeleteUserAsync(99))
                .ReturnsAsync(false);

            var result = await _userController!.DeleteUser(99);

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        // Happy path - 200 ok
        [TestMethod]
        public async Task GetUserById_ValidId_ReturnsOk()
        {
            var userResponse = new UserResponse(1, "Arthur", "Morgan", "arthur@test.com", "555-0101");

            _userServiceMock
                .Setup(s => s.GetUserByIdAsync(1))
                .ReturnsAsync(ServiceResult<UserResponse>.Ok(userResponse));

            var actual = await _userController.GetUserById(1);

            var okResult = actual.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        // User doesnt exist - 404
        [TestMethod]
        public async Task GetUserById_InvalidId_ReturnsNotFound()
        {
            _userServiceMock
                .Setup(s => s.GetUserByIdAsync(99))
                .ReturnsAsync(ServiceResult<UserResponse>.NotFound("Användare 99 hittades inte."));

            var actual = await _userController.GetUserById(99);

            var notFoundResult = actual.Result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        // Verify - the service is called only once
        [TestMethod]
        public async Task GetUserById_ValidId_VerifyServiceCalledOnce()
        {
            var id = 1;
            var userResponse = new UserResponse(id, "Arthur", "Morgan", "arthur@test.com", "555-0101");

            _userServiceMock
                .Setup(s => s.GetUserByIdAsync(id))
                .ReturnsAsync(ServiceResult<UserResponse>.Ok(userResponse));

            await _userController.GetUserById(1);

            _userServiceMock.Verify(s => s.GetUserByIdAsync(id), Times.Once);
        }

        // Happy path - valid update returns 200
        [TestMethod]
        public async Task UpdateUser_ValidId_ReturnsOk()
        {
            var request = new UpdateUser("John", "Doe", "john@test.com", null);
            var updatedUser = new User { Id = 1, Firstname = "John", Lastname = "Doe", Email = "john@test.com" };

            _updateValidatorMock
                .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _userServiceMock!
                .Setup(s => s.UpdateUserAsync(1, request))
                .ReturnsAsync(ServiceResult<User>.Ok(updatedUser));

            var result = await _userController!.UpdateUser(1, request);

            var ok = result as OkObjectResult;
            Assert.IsNotNull(ok);
            Assert.AreEqual(200, ok.StatusCode);
        }

        // Edge case - user not found returns 404
        [TestMethod]
        public async Task UpdateUser_UserNotFound_ReturnsNotFound()
        {
            var request = new UpdateUser("John", null, null, null);

            _updateValidatorMock
                .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _userServiceMock!
                .Setup(s => s.UpdateUserAsync(99, request))
                .ReturnsAsync(ServiceResult<User>.NotFound("User with id 99 was not found."));

            var result = await _userController!.UpdateUser(99, request);

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        // Edge case - invalid id returns 400 without calling service
        [TestMethod]
        public async Task UpdateUser_InvalidId_ReturnsBadRequest()
        {
            var request = new UpdateUser("John", null, null, null);

            var result = await _userController!.UpdateUser(-1, request);

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            _userServiceMock!.Verify(s => s.UpdateUserAsync(
                It.IsAny<int>(), It.IsAny<UpdateUser>()), Times.Never);
        }
    }
}