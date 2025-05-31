using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LAPTRINHWEB.Data;
using LAPTRINHWEB.Models;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Collections.Generic;

namespace LAPTRINHWEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly TourDbContext _context;

        public HomeController(TourDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> HomePage()
        {
            try
            {
                // Lấy top 6 tour phổ biến (có thể dựa trên số lượng booking hoặc giá)
                var popularTours = await _context.Tours
                    .Where(t => t.Status == TourStatus.Active)
                    .Include(t => t.TourImages)
                    .Include(t => t.Bookings)
                    .OrderByDescending(t => t.Bookings.Count()) // Sắp xếp theo số lượng booking
                    .ThenBy(t => t.Price) // Sau đó theo giá
                    .Take(6)
                    .ToListAsync();

                // Lấy top 3 tour flash deals (có giảm giá cao nhất)
                var flashDealTours = await _context.Tours
                    .Where(t => t.Status == TourStatus.Active && t.Discount > 0)
                    .Include(t => t.TourImages)
                    .OrderByDescending(t => t.Discount)
                    .Take(3)
                    .ToListAsync();

                // Lấy danh sách các địa điểm phổ biến
                var popularLocations = await _context.Locations
                    .Include(l => l.ItineraryLocations)
                    .OrderByDescending(l => l.ItineraryLocations.Count())
                    .Take(4)
                    .ToListAsync();

                // Truyền dữ liệu qua ViewBag
                ViewBag.PopularTours = popularTours;
                ViewBag.FlashDealTours = flashDealTours;
                ViewBag.PopularLocations = popularLocations;

                return View();
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
                // _logger.LogError(ex, "Error loading home page data");

                // Trả về view với dữ liệu rỗng nếu có lỗi
                ViewBag.PopularTours = new List<Tour>();
                ViewBag.FlashDealTours = new List<Tour>();
                ViewBag.PopularLocations = new List<Location>();

                return View();
            }
        }
    }
}