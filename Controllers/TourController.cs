using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LAPTRINHWEB.Data;
using LAPTRINHWEB.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace LAPTRINHWEB.Controllers
{
    public class TourController : Controller
    {
        private readonly TourDbContext _context;

        public TourController(TourDbContext context)
        {
            _context = context;
        }

        // Hiển thị danh sách tất cả tour với phân trang
        public async Task<IActionResult> ListTour(int page = 1, int pageSize = 12)
        {
            try
            {
                var query = _context.Tours
                    .Include(t => t.TourImages)
                    .Where(t => t.Status == TourStatus.Active)
                    .OrderBy(t => t.Name_Tour);

                // Tổng số tour
                var totalTours = await query.CountAsync();

                // Lấy tour theo trang
                var tours = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                // ViewBag cho pagination
                ViewBag.CurrentPage = page;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalPages = (int)Math.Ceiling((double)totalTours / pageSize);
                ViewBag.TotalTours = totalTours;

                return View(tours);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ListTour Error: {ex.Message}");
                return View(new List<Tour>());
            }
        }

        // Hiển thị chi tiết tour
        public async Task<IActionResult> Detail(int id)
        {
            try
            {
                var tour = await _context.Tours
                    .Include(t => t.TourImages)
                    .Include(t => t.Itineraries.OrderBy(i => i.Day_Number))
                        .ThenInclude(i => i.ItineraryLocations)
                            .ThenInclude(il => il.Location)
                    .FirstOrDefaultAsync(t => t.ID_Tour == id && t.Status == TourStatus.Active);

                if (tour == null)
                {
                    return NotFound("Tour không tồn tại hoặc đã bị ẩn.");
                }

                // Thông tin bổ sung cho view
                ViewBag.HasDiscount = tour.Discount.HasValue && tour.Discount > 0;
                ViewBag.FinalPrice = tour.FinalPrice;
                ViewBag.MainImage = tour.MainImageUrl;

                return View(tour);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Detail Error: {ex.Message}");
                return NotFound();
            }
        }

        // Redirect cho compatibility
        public IActionResult Index()
        {
            return RedirectToAction("ListTour");
        }
    }
}



