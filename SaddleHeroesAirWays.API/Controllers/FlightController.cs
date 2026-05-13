using Microsoft.AspNetCore.Mvc;

namespace SaddleHeroesAirWays.API.Controllers
{
    public class FlightController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
