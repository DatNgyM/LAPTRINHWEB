using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LAPTRINHWEB.Data;
using LAPTRINHWEB.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace LAPTRINHWEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BookingController : Controller
    {
        private readonly TourDbContext _context;

        public BookingController(TourDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Booking
        public async Task<IActionResult> Index(int tourId = 0)
        {
            // Lấy danh sách tour cho dropdown
            var tours = await _context.Tours.OrderBy(t => t.Name_Tour).ToListAsync();
            ViewBag.Tours = tours;
            ViewBag.SelectedTourId = tourId;

            // Lấy danh sách booking theo tour (nếu chọn tour)
            IQueryable<Booking> bookings = _context.Bookings
                .Include(b => b.Tour)
                .Include(b => b.User)
                .OrderByDescending(b => b.Booking_Date);

            if (tourId > 0)
            {
                bookings = bookings.Where(b => b.ID_Tour == tourId);
            }

            return View(await bookings.ToListAsync());
        }

        // POST: Admin/Booking/Approve
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null && booking.Status == BookingStatus.Pending)
            {
                booking.Status = BookingStatus.Completed;
                await _context.SaveChangesAsync();
                TempData["Success"] = "Đã duyệt booking thành công!";
            }
            return RedirectToAction(nameof(Index), new { tourId = booking?.ID_Tour ?? 0 });
        }

        // POST: Admin/Booking/Cancel
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null && booking.Status == BookingStatus.Pending)
            {
                booking.Status = BookingStatus.Cancelled;

                // Huỷ payment liên quan nếu có
                var payment = await _context.Payments.FirstOrDefaultAsync(p => p.ID_Booking == booking.ID_Booking);
                if (payment != null)
                {
                    payment.Status = PaymentStatus.Cancelled;
                }

                await _context.SaveChangesAsync();
                TempData["Success"] = "Đã huỷ booking và payment thành công!";
            }
            return RedirectToAction(nameof(Index), new { tourId = booking?.ID_Tour ?? 0 });
        }
    }
}