using Microsoft.AspNetCore.Mvc;

namespace LAPTRINHWEB.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult HomePage()
        {
            return View();
        }
    }
}