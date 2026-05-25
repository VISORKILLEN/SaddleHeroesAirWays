using Microsoft.EntityFrameworkCore;
using SaddleHeroesAirWays.API;
using SaddleHeroesAirWays.API.DTOs;
using SaddleHeroesAirWays.API.Services;
using SaddleHeroesAirWays.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaddleHeroesAirWays.MSTest
{
    [TestClass]
    public class UserServiceTests
    {
        private DbContextAPI CreateContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<DbContextAPI>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            return new DbContextAPI(options);
        }

        [TestMethod]
        public async Task CreateUserAsync_ShouldAddUserAndReturnIt()
        {
            using var context = CreateContext("CreateUserServiceTest");

            var service = new UserService(context);

            var request = new CreateUser(
                Gender: "Male",
                Firstname: "John",
                Lastname: "Doe",
                Email: "john.doe@example.com",
                Phonenumber: "123-456-7890",
                SocialSecurityNumber: "19900101-1234"
                );

            var result = await service.CreateUserAsync(request);

            Assert.IsNotNull(result);
            Assert.AreEqual("John", result.Firstname);
            Assert.AreEqual("Doe", result.Lastname);
            Assert.IsFalse(result.IsAdmin);

            var dbUsers = await context.User.ToListAsync();
            Assert.AreEqual(1, dbUsers.Count);
            Assert.AreEqual("john.doe@example.com", dbUsers.First().Email);
        }

        [TestMethod]
        public async Task GetAllUsersAlphabeticlyAsync_ShouldReturnUsersSortedByLastName()
        {
            using var context = CreateContext("GetUsersAlphabeticlyTest");
            var service = new UserService(context);

            await context.User.AddRangeAsync(
                new User { Firstname = "Alice", Lastname = "Zane", Email = "alice@example.com", SocialSecurityNumber = "1234567890" },
                new User { Firstname = "Bob", Lastname = "Adams", Email = "bob@example.com", SocialSecurityNumber = "2345678901" },
                new User { Firstname = "Charlie", Lastname = "Smith", Email = "charlie@example.com", SocialSecurityNumber = "3456789012" }
            );
            await context.SaveChangesAsync();

            var result = await service.GetAllUsersAlphabeticlyAsync();

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);

            Assert.AreEqual("Adams", result[0].Lastname);
            Assert.AreEqual("Smith", result[1].Lastname);
            Assert.AreEqual("Zane", result[2].Lastname);
        }
    }
}
