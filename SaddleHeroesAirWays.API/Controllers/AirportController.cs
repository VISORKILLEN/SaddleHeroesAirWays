using Microsoft.AspNetCore.Mvc;

namespace SaddleHeroesAirWays.API.Controllers
{
    public class AirportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
