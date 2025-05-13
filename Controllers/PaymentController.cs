using Microsoft.AspNetCore.Mvc;

namespace LAPTRINHWEB.Controllers
{
    public class PaymentController : Controller
    {
        public IActionResult Payment()
        {
            return View();
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}