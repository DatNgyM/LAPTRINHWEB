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
        public async Task<IActionResult> ListTour(string destination, DateTime? startDate, DateTime? endDate, int page = 1, int pageSize = 12)
        {
            try
            {
                var query = _context.Tours
                    .Include(t => t.TourImages)
                    .Where(t => t.Status == TourStatus.Active);

                // Lọc theo nơi đến nếu có nhập
                if (!string.IsNullOrWhiteSpace(destination))
                {
                    query = query.Where(t => t.End_Location.Contains(destination));
                }

                // Lọc theo ngày bắt đầu và ngày kết thúc nếu có nhập
                if (startDate.HasValue && endDate.HasValue)
                {
                    // Tour có ngày khởi hành nằm trong khoảng chọn
                    query = query.Where(t => t.DepartureDate >= startDate.Value && t.DepartureDate <= endDate.Value);
                }
                else if (startDate.HasValue)
                {
                    query = query.Where(t => t.DepartureDate >= startDate.Value);
                }
                else if (endDate.HasValue)
                {
                    query = query.Where(t => t.DepartureDate <= endDate.Value);
                }

                // Sắp xếp theo ngày khởi hành gần nhất
                query = query.OrderBy(t => t.DepartureDate);

                // Tổng số tour
                var totalTours = await query.CountAsync();

                // Lấy tour theo trang
                var tours = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                // Nếu có nhập ngày đi và ngày về, tính duration cho từng tour và truyền vào ViewBag
                if (startDate.HasValue && endDate.HasValue)
                {
                    var durations = tours.ToDictionary(
                        t => t.ID_Tour,
                        t => (endDate.Value - startDate.Value).Days + 1 // +1 để tính cả ngày đi và về
                    );
                    ViewBag.Durations = durations;
                }
                else
                {
                    ViewBag.Durations = null;
                }

                // ViewBag cho pagination
                ViewBag.CurrentPage = page;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalPages = (int)Math.Ceiling((double)totalTours / pageSize);
                ViewBag.TotalTours = totalTours;
                ViewBag.SearchDestination = destination;
                ViewBag.SearchStartDate = startDate;
                ViewBag.SearchEndDate = endDate;

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
                    .Include(t => t.Itineraries)
                        .ThenInclude(i => i.Details) // Thêm dòng này để lấy chi tiết hoạt động
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



