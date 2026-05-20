using Moq;
using SaddleHeroesAirWays.API.Controllers;
using SaddleHeroesAirWays.API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaddleHeroesAirWays.MSTest
{
    [TestClass]
    public class UserControllerTests
    {
        private Mock<IUserService> _UserService;

        private UserController _UserController;

        public UserControllerTests()
        {
            _UserService = new Mock<IUserService>();
        }
    }
}
