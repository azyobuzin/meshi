using Microsoft.AspNetCore.Mvc;

namespace MeshiRoulette.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}