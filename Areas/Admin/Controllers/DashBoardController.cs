using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace LAPTRINHWEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashBoardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Users()
        {
            return View();
        }

        public IActionResult Transportations()
        {
            return View();
        }

        public IActionResult Accommodations()
        {
            return View();
        }

        public IActionResult Itineraries()
        {
            return View();
        }

        public IActionResult TourGuides()
        {
            return View();
        }
    }
}