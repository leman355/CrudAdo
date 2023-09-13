using Microsoft.AspNetCore.Mvc;

namespace CarWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
