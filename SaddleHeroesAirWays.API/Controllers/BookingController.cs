using Microsoft.AspNetCore.Mvc;

namespace SaddleHeroesAirWays.API.Controllers
{
    public class BookingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
