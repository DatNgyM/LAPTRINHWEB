using Microsoft.AspNetCore.Mvc;
using LAPTRINHWEB.Models;
using LAPTRINHWEB.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LAPTRINHWEB.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private readonly TourDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BookingController(TourDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Hiển thị form đặt tour
        [HttpGet]
        public async Task<IActionResult> Index(int? tourId)
        {
            if (tourId == null)
                return RedirectToAction("ListTour", "Tour");

            var tour = _context.Tours.FirstOrDefault(t => t.ID_Tour == tourId);
            if (tour == null)
                return NotFound();

            ViewBag.Tour = tour;

            // Lấy thông tin user đã đăng nhập
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                ViewBag.FullName = user.Full_Name;
                ViewBag.Email = user.Email;
                ViewBag.Phone = user.Phone;
                ViewBag.Address = user.Address;
                ViewBag.Gender = user.Gender;
            }

            return View();
        }

        // Xử lý đặt tour
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(int tourId, string fullName, string email, string phone, string address,
            DateTime departureDate, int quantity)
        {
            var tour = _context.Tours.FirstOrDefault(t => t.ID_Tour == tourId);
            if (tour == null)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);

            // Tính tổng tiền
            decimal totalPrice = tour.Price * quantity;

            // Lưu thông tin booking tạm vào TempData (dạng JSON)
            var bookingInfo = new
            {
                TourId = tourId,
                FullName = fullName,
                Email = email,
                Phone = phone,
                Address = address,
                DepartureDate = departureDate,
                Quantity = quantity,
                TotalPrice = totalPrice,
                UserId = user?.Id
            };
            TempData["BookingInfo"] = JsonConvert.SerializeObject(bookingInfo);

            // Chuyển sang trang thanh toán
            return RedirectToAction("Payment", "Payment", new { area = "" });
        }
    }
}