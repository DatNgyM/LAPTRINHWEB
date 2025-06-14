using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using LAPTRINHWEB.Data;
using LAPTRINHWEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace LAPTRINHWEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TourController : Controller
    {
        private readonly TourDbContext _context;

        public TourController(TourDbContext context)
        {
            _context = context;
        }


        // GET: Admin/Tour/Tours - Quản lý danh sách tour
        public async Task<IActionResult> Index(string search, string status, string sortBy, int page = 1, int pageSize = 20)
        {
            try
            {
                var query = _context.Tours
                    .Include(t => t.TourImages)
                    .AsQueryable();

                // Tìm kiếm
                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(t => t.Name_Tour.Contains(search) ||
                                           t.Start_Location.Contains(search) ||
                                           t.End_Location.Contains(search));
                }

                // Lọc theo trạng thái
                if (!string.IsNullOrEmpty(status))
                {
                    if (Enum.TryParse<TourStatus>(status, out var tourStatus))
                    {
                        query = query.Where(t => t.Status == tourStatus);
                    }
                }

                // Sắp xếp
                query = sortBy?.ToLower() switch
                {
                    "name" => query.OrderBy(t => t.Name_Tour),
                    "price" => query.OrderBy(t => t.Price),
                    "duration" => query.OrderBy(t => t.Duration),
                    _ => query.OrderByDescending(t => t.ID_Tour)
                };

                // Phân trang
                var totalTours = await query.CountAsync();
                var tours = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                ViewBag.CurrentSearch = search;
                ViewBag.CurrentStatus = status;
                ViewBag.CurrentSort = sortBy;
                ViewBag.CurrentPage = page;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalPages = (int)Math.Ceiling((double)totalTours / pageSize);
                ViewBag.TotalTours = totalTours;

                return View(tours);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Tours Management Error: {ex.Message}");
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tải danh sách tour.";
                return View(new List<Tour>());
            }
        }

        // GET: Admin/Tour/Add (hoặc CreateTour) - Thêm tour mới
        public IActionResult Add()
        {
            return View(new Tour());
        }

        // Alias cho Add action để match với view
        public IActionResult CreateTour()
        {
            return View("Add", new Tour());
        }

        // POST: Admin/Tour/Add - Xử lý thêm tour mới
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Tour tour, IFormFileCollection TourImages)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Set default values

                    if (tour.Status == 0) // Default to Active nếu không set
                        tour.Status = TourStatus.Active;

                    _context.Tours.Add(tour);
                    await _context.SaveChangesAsync();

                    // Xử lý upload hình ảnh
                    if (TourImages != null && TourImages.Count > 0)
                    {
                        await ProcessTourImages(tour.ID_Tour, TourImages);
                    }

                    TempData["SuccessMessage"] = $"Đã thêm tour '{tour.Name_Tour}' thành công!";
                    return RedirectToAction("Tours");
                }

                return View(tour);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Add Tour POST Error: {ex.Message}");
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi thêm tour.";
                return View(tour);
            }
        }

        // POST: Alias cho Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTour(Tour tour, IFormFileCollection TourImages)
        {
            return await Add(tour, TourImages);
        }

        // GET: Admin/Tour/Edit/5 - Chỉnh sửa tour
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var tour = await _context.Tours
                    .Include(t => t.TourImages)
                    .FirstOrDefaultAsync(t => t.ID_Tour == id);

                if (tour == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy tour.";
                    return RedirectToAction("Tours");
                }

                return View(tour);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Edit Tour GET Error: {ex.Message}");
                TempData["ErrorMessage"] = "Không thể tải thông tin tour.";
                return RedirectToAction("Tours");
            }
        }

        // Alias cho Edit action
        public async Task<IActionResult> EditTour(int id)
        {
            return await Edit(id);
        }

        // POST: Admin/Tour/Edit/5 - Xử lý chỉnh sửa tour
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Tour tour, IFormFileCollection TourImages, string DeletedImageIds)
        {
            if (id != tour.ID_Tour)
            {
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    // Cập nhật tour
                    _context.Update(tour);

                    // Xóa hình ảnh đã chọn xóa
                    if (!string.IsNullOrEmpty(DeletedImageIds))
                    {
                        var deletedIds = DeletedImageIds.Split(',')
                            .Where(id => int.TryParse(id, out _))
                            .Select(int.Parse)
                            .ToList();

                        if (deletedIds.Any())
                        {
                            var imagesToDelete = await _context.TourImages
                                .Where(ti => deletedIds.Contains(ti.ID_Image))
                                .ToListAsync();
                            _context.TourImages.RemoveRange(imagesToDelete);
                        }
                    }

                    // Thêm hình ảnh mới
                    if (TourImages != null && TourImages.Count > 0)
                    {
                        await ProcessTourImages(tour.ID_Tour, TourImages);
                    }

                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"Đã cập nhật tour '{tour.Name_Tour}' thành công!";
                    return RedirectToAction("Tours");
                }

                // Reload images nếu có lỗi
                tour.TourImages = await _context.TourImages
                    .Where(ti => ti.ID_Tour == tour.ID_Tour)
                    .ToListAsync();

                return View(tour);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Edit Tour POST Error: {ex.Message}");
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi cập nhật tour.";
                return View(tour);
            }
        }

        // POST: Alias cho Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTour(int id, Tour tour, IFormFileCollection TourImages, string DeletedImageIds)
        {
            return await Edit(id, tour, TourImages, DeletedImageIds);
        }

        // POST: Admin/Tour/DeleteTour - Xóa tour
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTour(int id)
        {
            try
            {
                var tour = await _context.Tours
                    .Include(t => t.TourImages)
                    .Include(t => t.Bookings)
                    .FirstOrDefaultAsync(t => t.ID_Tour == id);

                if (tour == null)
                {
                    TempData["ErrorMessage"] = "Tour không tồn tại.";
                    return RedirectToAction("Tours");
                }

                // Kiểm tra có booking không
                if (tour.Bookings?.Any() == true)
                {
                    TempData["ErrorMessage"] = "Không thể xóa tour đã có người đặt.";
                    return RedirectToAction("Tours");
                }

                // Xóa hình ảnh
                if (tour.TourImages?.Any() == true)
                {
                    _context.TourImages.RemoveRange(tour.TourImages);
                }

                // Xóa tour
                _context.Tours.Remove(tour);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Đã xóa tour '{tour.Name_Tour}' thành công!";
                return RedirectToAction("Tours");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Delete Tour Error: {ex.Message}");
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi xóa tour.";
                return RedirectToAction("Tours");
            }
        }

        // POST: Admin/Tour/ChangeStatus - Thay đổi trạng thái tour
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(int id, TourStatus status)
        {
            try
            {
                var tour = await _context.Tours.FindAsync(id);
                if (tour == null)
                {
                    return Json(new { success = false, message = "Tour không tồn tại." });
                }

                tour.Status = status;
                await _context.SaveChangesAsync();

                var statusText = status == TourStatus.Active ? "kích hoạt" : "tạm dừng";
                return Json(new { success = true, message = $"Đã {statusText} tour '{tour.Name_Tour}' thành công!" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Change Status Error: {ex.Message}");
                return Json(new { success = false, message = "Có lỗi xảy ra khi thay đổi trạng thái." });
            }
        }

        // POST: Admin/Tour/BulkAction - Thao tác hàng loạt
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BulkAction(string action, List<int> tourIds)
        {
            try
            {
                if (tourIds == null || !tourIds.Any())
                {
                    return Json(new { success = false, message = "Vui lòng chọn ít nhất một tour." });
                }

                var tours = await _context.Tours.Where(t => tourIds.Contains(t.ID_Tour)).ToListAsync();

                switch (action.ToLower())
                {
                    case "activate":
                        tours.ForEach(t => t.Status = TourStatus.Active);
                        await _context.SaveChangesAsync();
                        return Json(new { success = true, message = $"Đã kích hoạt {tours.Count} tour." });

                    case "deactivate":
                        tours.ForEach(t => t.Status = TourStatus.Inactive);
                        await _context.SaveChangesAsync();
                        return Json(new { success = true, message = $"Đã tạm dừng {tours.Count} tour." });

                    case "delete":
                        // Kiểm tra tour có booking không
                        var toursWithBookings = await _context.Tours
                            .Where(t => tourIds.Contains(t.ID_Tour) && t.Bookings.Any())
                            .CountAsync();

                        if (toursWithBookings > 0)
                        {
                            return Json(new { success = false, message = $"Có {toursWithBookings} tour đã có người đặt, không thể xóa." });
                        }

                        // Xóa hình ảnh
                        var imagesToDelete = await _context.TourImages
                            .Where(ti => tourIds.Contains(ti.ID_Tour))
                            .ToListAsync();
                        _context.TourImages.RemoveRange(imagesToDelete);

                        // Xóa tour
                        _context.Tours.RemoveRange(tours);
                        await _context.SaveChangesAsync();
                        return Json(new { success = true, message = $"Đã xóa {tours.Count} tour." });

                    default:
                        return Json(new { success = false, message = "Thao tác không hợp lệ." });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Bulk Action Error: {ex.Message}");
                return Json(new { success = false, message = "Có lỗi xảy ra khi thực hiện thao tác." });
            }
        }

        // Helper method để xử lý upload hình ảnh
        private async Task ProcessTourImages(int tourId, IFormFileCollection files)
        {
            try
            {
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        // Tạo tên file unique
                        var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                        var uploadPath = Path.Combine("wwwroot", "img", "tours");

                        // Tạo thư mục nếu chưa có
                        if (!Directory.Exists(uploadPath))
                        {
                            Directory.CreateDirectory(uploadPath);
                        }

                        var filePath = Path.Combine(uploadPath, fileName);

                        // Lưu file
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        // Lưu thông tin vào database
                        var tourImage = new TourImage
                        {
                            ID_Tour = tourId,
                            Image_URL = $"/img/tours/{fileName}",
                            Caption = file.FileName
                        };

                        _context.TourImages.Add(tourImage);
                    }
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Process Tour Images Error: {ex.Message}");
                // Không throw exception để không break main flow
            }
        }

        // Dummy actions cho navigation (có thể implement sau)

    }
}