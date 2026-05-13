using Microsoft.AspNetCore.Mvc;

namespace SaddleHeroesAirWays.API.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
