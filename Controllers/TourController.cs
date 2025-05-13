using Microsoft.AspNetCore.Mvc;

namespace LAPTRINHWEB.Controllers
{
    public class TourController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Detail()
        {
            return View();
        }
    }
}