using Microsoft.AspNetCore.Mvc;
using LAPTRINHWEB.Models;
using LAPTRINHWEB.Data;
using Microsoft.AspNetCore.Identity;
using System;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Linq;

namespace LAPTRINHWEB.Controllers
{
    public class PaymentController : Controller
    {
        private readonly TourDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PaymentController(TourDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Hiển thị trang thanh toán
        [HttpGet]
        public IActionResult Payment()
        {
            if (TempData["BookingInfo"] == null)
                return RedirectToAction("Index", "Booking");

            var bookingInfo = JsonConvert.DeserializeObject<dynamic>(TempData["BookingInfo"].ToString());
            TempData.Keep("BookingInfo"); // Giữ lại cho POST

            // Lấy thông tin tour
            int tourId = (int)bookingInfo.TourId;
            var tour = _context.Tours.FirstOrDefault(t => t.ID_Tour == tourId);
            ViewBag.Tour = tour;
            ViewBag.BookingInfo = bookingInfo;
            return View();
        }

        // Xử lý thanh toán và lưu booking vào DB
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Payment(string payment_method)
        {
            if (TempData["BookingInfo"] == null)
                return RedirectToAction("Index", "Booking");

            var bookingInfo = JsonConvert.DeserializeObject<dynamic>(TempData["BookingInfo"].ToString());

            // Xác định phương thức và trạng thái thanh toán
            PaymentMethod method;
            PaymentStatus paymentStatus;
            BookingStatus bookingStatus;
            switch (payment_method)
            {
                case "bank_transfer":
                    method = PaymentMethod.BankTransfer;
                    paymentStatus = PaymentStatus.Completed;
                    bookingStatus = BookingStatus.Completed;
                    break;
                case "credit_card":
                    method = PaymentMethod.CreditCard;
                    paymentStatus = PaymentStatus.Completed;
                    bookingStatus = BookingStatus.Completed;
                    break;
                case "momo":
                case "ewallet":
                    method = PaymentMethod.EWallet;
                    paymentStatus = PaymentStatus.Completed;
                    bookingStatus = BookingStatus.Completed;
                    break;
                default: // "cash"
                    method = PaymentMethod.Cash;
                    paymentStatus = PaymentStatus.Waiting;
                    bookingStatus = BookingStatus.Pending;
                    break;
            }

            // Tạo booking mới
            var booking = new Booking
            {
                ID_Tour = (int)bookingInfo.TourId,
                Booking_Date = DateTime.Now,
                Quantity = (int)bookingInfo.Quantity,
                Total_Price = (decimal)bookingInfo.TotalPrice,
                Status = bookingStatus,
                UserId = (string)bookingInfo.UserId,
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            var payment = new Payment
            {
                ID_Booking = booking.ID_Booking,
                Method = method,
                Paid_Amount = booking.Total_Price,
                Payment_Date = DateTime.Now,
                Status = paymentStatus
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            // Truyền mã booking qua TempData để hiển thị ở trang Success
            TempData["BookingId"] = booking.ID_Booking;

            return RedirectToAction("Success", new { area = "" });
        }

        // Trang xác nhận thành công
        public IActionResult Success()
        {
            int? bookingId = TempData["BookingId"] as int?;
            if (bookingId == null)
                return RedirectToAction("Index", "Home");

            var booking = _context.Bookings.FirstOrDefault(b => b.ID_Booking == bookingId);
            if (booking == null)
                return RedirectToAction("Index", "Home");

            var tour = _context.Tours.FirstOrDefault(t => t.ID_Tour == booking.ID_Tour);
            ViewBag.Tour = tour;
            return View(booking);
        }
    }
}