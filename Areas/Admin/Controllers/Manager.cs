using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using LAPTRINHWEB.Data;
using LAPTRINHWEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace LAPTRINHWEB.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class ManagerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ManagerController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // Dashboard
        public async Task<IActionResult> Index()
        {
            // Các thống kê cho trang dashboard
            ViewBag.TotalItineraries = await _context.Itineraries.CountAsync();
            ViewBag.TotalGuides = await _context.Guides.CountAsync();
            ViewBag.CurrentTours = await _context.Tours.Where(t => t.Status == "active").CountAsync();
            ViewBag.PendingAssignments = await _context.TourAssignments.Where(a => a.Status == "pending").CountAsync();

            // Lấy danh sách hoạt động gần đây
            var recentActivities = await _context.Activities
                .OrderByDescending(a => a.Timestamp)
                .Take(5)
                .ToListAsync();

            ViewBag.RecentActivities = recentActivities;

            // Lấy các cảnh báo
            ViewBag.UnassignedTours = await _context.Tours.Where(t => t.GuideId == null && t.Status == "active").CountAsync();
            ViewBag.OutdatedItineraries = await _context.Itineraries.Where(i => i.NeedsUpdate).CountAsync();
            ViewBag.PendingConfirmations = await _context.TourAssignments.Where(a => a.Status == "pending").CountAsync();

            return View();
        }

        // Quản lý lịch trình
        [HttpGet]
        public async Task<IActionResult> Itineraries()
        {
            var itineraries = await _context.Itineraries
                .Include(i => i.TourType)
                .OrderByDescending(i => i.CreatedDate)
                .ToListAsync();

            ViewBag.TotalCount = itineraries.Count;
            ViewBag.ActiveCount = itineraries.Count(i => i.Status == "active");
            ViewBag.InactiveCount = itineraries.Count(i => i.Status == "inactive");
            ViewBag.DraftCount = itineraries.Count(i => i.Status == "draft");
            ViewBag.TourTypes = await _context.TourTypes.ToListAsync();

            return View(itineraries);
        }

        [HttpGet]
        public async Task<IActionResult> CreateItinerary()
        {
            ViewBag.TourTypes = await _context.TourTypes.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateItinerary(Itinerary itinerary, IFormFile mainImage)
        {
            if (ModelState.IsValid)
            {
                // Upload image if provided
                if (mainImage != null)
                {
                    string uniqueFileName = ProcessUploadedFile(mainImage);
                    itinerary.ImageUrl = uniqueFileName;
                }

                itinerary.CreatedDate = DateTime.Now;
                itinerary.TourCode = GenerateUniqueTourCode(itinerary.Destination);

                _context.Itineraries.Add(itinerary);
                await _context.SaveChangesAsync();

                // Log activity
                await LogActivity("Tạo lịch trình mới", null, itinerary.Name);

                return RedirectToAction(nameof(Itineraries));
            }

            ViewBag.TourTypes = await _context.TourTypes.ToListAsync();
            return View(itinerary);
        }

        [HttpGet]
        public async Task<IActionResult> EditItinerary(int id)
        {
            var itinerary = await _context.Itineraries
                .Include(i => i.TourType)
                .Include(i => i.Days).ThenInclude(d => d.Activities)
                .Include(i => i.IncludedServices)
                .Include(i => i.ExcludedServices)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (itinerary == null)
            {
                return NotFound();
            }

            ViewBag.TourTypes = await _context.TourTypes.ToListAsync();
            return View(itinerary);
        }

        [HttpPost]
        public async Task<IActionResult> EditItinerary(Itinerary itinerary, IFormFile mainImage)
        {
            if (ModelState.IsValid)
            {
                // Upload image if provided
                if (mainImage != null)
                {
                    string uniqueFileName = ProcessUploadedFile(mainImage);
                    itinerary.ImageUrl = uniqueFileName;
                }

                itinerary.LastUpdated = DateTime.Now;

                _context.Entry(itinerary).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                // Log activity
                await LogActivity("Cập nhật lịch trình", null, itinerary.Name);

                return RedirectToAction(nameof(Itineraries));
            }

            ViewBag.TourTypes = await _context.TourTypes.ToListAsync();
            return View(itinerary);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteItinerary(int id)
        {
            var itinerary = await _context.Itineraries.FindAsync(id);
            if (itinerary == null)
            {
                return NotFound();
            }

            return View(itinerary);
        }

        [HttpPost, ActionName("DeleteItinerary")]
        public async Task<IActionResult> DeleteItineraryConfirmed(int id)
        {
            var itinerary = await _context.Itineraries.FindAsync(id);
            if (itinerary == null)
            {
                return NotFound();
            }

            string itineraryName = itinerary.Name;

            _context.Itineraries.Remove(itinerary);
            await _context.SaveChangesAsync();

            // Log activity
            await LogActivity("Xóa lịch trình", null, itineraryName);

            return RedirectToAction(nameof(Itineraries));
        }

        // Quản lý hướng dẫn viên
        public async Task<IActionResult> Guides()
        {
            var guides = await _context.Guides.ToListAsync();
            return View(guides);
        }

        [HttpGet]
        public IActionResult AddGuide()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddGuide(Guide guide)
        {
            if (ModelState.IsValid)
            {
                _context.Guides.Add(guide);
                await _context.SaveChangesAsync();

                // Log activity
                await LogActivity("Thêm hướng dẫn viên mới", guide.FullName, null);

                return RedirectToAction(nameof(Guides));
            }

            return View(guide);
        }

        [HttpGet]
        public async Task<IActionResult> EditGuide(int id)
        {
            var guide = await _context.Guides.FindAsync(id);
            if (guide == null)
            {
                return NotFound();
            }

            return View(guide);
        }

        [HttpPost]
        public async Task<IActionResult> EditGuide(Guide guide)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(guide).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                // Log activity
                await LogActivity("Cập nhật hướng dẫn viên", guide.FullName, null);

                return RedirectToAction(nameof(Guides));
            }

            return View(guide);
        }

        // Phân công tour
        public async Task<IActionResult> Assignments(string status = "", string tourType = "", string guideName = "")
        {
            var query = _context.TourAssignments
                .Include(a => a.Tour)
                .Include(a => a.Guide)
                .AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(a => a.Status == status);
            }

            if (!string.IsNullOrEmpty(tourType))
            {
                query = query.Where(a => a.Tour.TourType.Code == tourType);
            }

            if (!string.IsNullOrEmpty(guideName))
            {
                query = query.Where(a => a.Guide.FullName.Contains(guideName));
            }

            var assignments = await query.OrderByDescending(a => a.AssignmentDate).ToListAsync();

            ViewBag.Guides = await _context.Guides.ToListAsync();
            ViewBag.TourTypes = await _context.TourTypes.ToListAsync();

            return View(assignments);
        }

        [HttpGet]
        public async Task<IActionResult> AssignTour()
        {
            ViewBag.Guides = await _context.Guides.ToListAsync();
            ViewBag.AvailableTours = await _context.Tours
                .Where(t => t.GuideId == null && t.Status == "active")
                .ToListAsync();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AssignTour(TourAssignment assignment)
        {
            if (ModelState.IsValid)
            {
                assignment.AssignmentDate = DateTime.Now;
                assignment.Status = "pending";

                _context.TourAssignments.Add(assignment);
                await _context.SaveChangesAsync();

                // Update the tour with the guide ID
                var tour = await _context.Tours.FindAsync(assignment.TourId);
                if (tour != null)
                {
                    tour.GuideId = assignment.GuideId;
                    _context.Entry(tour).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }

                // Log activity
                var guide = await _context.Guides.FindAsync(assignment.GuideId);
                var tourName = tour?.Name ?? "Unknown";
                await LogActivity("Phân công mới", guide?.FullName ?? "Unknown", tourName);

                return RedirectToAction(nameof(Assignments));
            }

            ViewBag.Guides = await _context.Guides.ToListAsync();
            ViewBag.AvailableTours = await _context.Tours
                .Where(t => t.GuideId == null && t.Status == "active")
                .ToListAsync();

            return View(assignment);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteAssignment(int id)
        {
            var assignment = await _context.TourAssignments
                .Include(a => a.Tour)
                .Include(a => a.Guide)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (assignment == null)
            {
                return NotFound();
            }

            return View(assignment);
        }

        [HttpPost, ActionName("DeleteAssignment")]
        public async Task<IActionResult> DeleteAssignmentConfirmed(int id)
        {
            var assignment = await _context.TourAssignments
                .Include(a => a.Tour)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (assignment == null)
            {
                return NotFound();
            }

            // Update the tour to remove guide ID
            if (assignment.Tour != null)
            {
                assignment.Tour.GuideId = null;
                _context.Entry(assignment.Tour).State = EntityState.Modified;
            }

            _context.TourAssignments.Remove(assignment);
            await _context.SaveChangesAsync();

            // Log activity
            var guide = await _context.Guides.FindAsync(assignment.GuideId);
            var tourName = assignment.Tour?.Name ?? "Unknown";
            await LogActivity("Xóa phân công", guide?.FullName ?? "Unknown", tourName);

            return RedirectToAction(nameof(Assignments));
        }

        // Báo cáo
        public IActionResult Reports()
        {
            return View();
        }

        // Helper Methods
        private string ProcessUploadedFile(IFormFile file)
        {
            string uniqueFileName = null;
            if (file != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/tours");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }

        private string GenerateUniqueTourCode(string destination)
        {
            // Lấy 2 ký tự đầu tiên của điểm đến
            string prefix = destination.Length >= 2 ? destination.Substring(0, 2).ToUpper() : destination.ToUpper();

            // Đếm số lượng tour có cùng prefix để tạo số thứ tự
            int count = _context.Itineraries.Count(i => i.TourCode.StartsWith(prefix)) + 1;

            // Format: XX-NNN (XX là prefix, NNN là số thứ tự)
            return $"{prefix}-{count:D3}";
        }

        private async Task LogActivity(string action, string guide = null, string tour = null)
        {
            var activity = new Activity
            {
                ActionType = action,
                GuideName = guide,
                TourName = tour,
                Timestamp = DateTime.Now,
                UserId = User.Identity.Name
            };

            _context.Activities.Add(activity);
            await _context.SaveChangesAsync();
        }
    }

    // Các class model cần thiết 
    public class Activity
    {
        public int Id { get; set; }
        public string ActionType { get; set; }
        public string GuideName { get; set; }
        public string TourName { get; set; }
        public DateTime Timestamp { get; set; }
        public string UserId { get; set; }
    }

    public class Guide
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Language { get; set; }
        public List<TourAssignment> Assignments { get; set; }
    }

    public class Itinerary
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TourCode { get; set; }
        public string Destination { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Duration { get; set; }
        public int TourTypeId { get; set; }
        public TourType TourType { get; set; }
        public string Status { get; set; } // active, inactive, draft
        public string ImageUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastUpdated { get; set; }
        public bool NeedsUpdate { get; set; }
        public List<ItineraryDay> Days { get; set; }
        public List<IncludedService> IncludedServices { get; set; }
        public List<ExcludedService> ExcludedServices { get; set; }
    }

    public class ItineraryDay
    {
        public int Id { get; set; }
        public int ItineraryId { get; set; }
        public Itinerary Itinerary { get; set; }
        public int DayNumber { get; set; }
        public string Title { get; set; }
        public List<DayActivity> Activities { get; set; }
    }

    public class DayActivity
    {
        public int Id { get; set; }
        public int ItineraryDayId { get; set; }
        public ItineraryDay Day { get; set; }
        public TimeSpan Time { get; set; }
        public string Description { get; set; }
    }

    public class IncludedService
    {
        public int Id { get; set; }
        public int ItineraryId { get; set; }
        public Itinerary Itinerary { get; set; }
        public string Description { get; set; }
    }

    public class ExcludedService
    {
        public int Id { get; set; }
        public int ItineraryId { get; set; }
        public Itinerary Itinerary { get; set; }
        public string Description { get; set; }
    }

    public class Tour
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ItineraryId { get; set; }
        public Itinerary Itinerary { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public int? GuideId { get; set; }
        public Guide Guide { get; set; }
    }

    public class TourAssignment
    {
        public int Id { get; set; }
        public int TourId { get; set; }
        public Tour Tour { get; set; }
        public int GuideId { get; set; }
        public Guide Guide { get; set; }
        public DateTime AssignmentDate { get; set; }
        public string Status { get; set; } // pending, approved, completed
    }

    public class TourType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public List<Itinerary> Itineraries { get; set; }
    }
}