using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LAPTRINHWEB.Data;
using LAPTRINHWEB.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace LAPTRINHWEB.Controllers
{
    public class TourController : Controller
    {
        private readonly TourDbContext _context;

        public TourController(TourDbContext context)
        {
            _context = context;
        }

        // ListTour đơn giản - test từng bước
        public async Task<IActionResult> ListTour()
        {
            try
            {
                // Bước 1: Chỉ load Tours
                var tours = await _context.Tours
                    .Where(t => t.Status == TourStatus.Active)
                    .ToListAsync();

                return View(tours);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ListTour Error: {ex.Message}");
                return View(new List<Tour>());
            }
        }

        // Detail đơn giản - test từng bước
        public async Task<IActionResult> Detail(int id)
        {
            try
            {
                // Bước 1: Chỉ load Tour
                var tour = await _context.Tours
                    .FirstOrDefaultAsync(t => t.ID_Tour == id && t.Status == TourStatus.Active);

                if (tour == null)
                {
                    return NotFound();
                }

                // Bước 2: Load TourImages manually
                tour.TourImages = await _context.TourImages
                    .Where(ti => ti.ID_Tour == id)
                    .ToListAsync();

                // Bước 3: Load Itineraries manually
                tour.Itineraries = await _context.Itineraries
                    .Where(i => i.ID_Tour == id)
                    .OrderBy(i => i.Day_Number)
                    .ToListAsync();

                // ViewBag
                ViewBag.AverageRating = 4.5;
                ViewBag.TotalReviews = 0;
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

        // Test action để kiểm tra database
        public async Task<IActionResult> TestDb()
        {
            try
            {
                var toursCount = await _context.Tours.CountAsync();
                var imagesCount = await _context.TourImages.CountAsync();

                return Json(new
                {
                    ToursCount = toursCount,
                    ImagesCount = imagesCount,
                    Message = "Database OK"
                });
            }
            catch (Exception ex)
            {
                return Json(new { Error = ex.Message });
            }
        }

        public IActionResult Index()
        {
            return RedirectToAction("ListTour");
        }

        // Thêm các actions sau vào TourController

        [HttpGet]
        public IActionResult AddTour()
        {
            return View(new Tour());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTour(Tour tour, IFormFileCollection TourImages)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Thêm tour vào database
                    _context.Tours.Add(tour);
                    await _context.SaveChangesAsync();

                    // Xử lý upload hình ảnh (nếu có)
                    if (TourImages != null && TourImages.Count > 0)
                    {
                        await ProcessTourImages(tour.ID_Tour, TourImages);
                    }

                    TempData["SuccessMessage"] = "Thêm tour mới thành công!";
                    return RedirectToAction("Detail", new { id = tour.ID_Tour });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AddTour Error: {ex.Message}");
                ModelState.AddModelError("", "Có lỗi xảy ra khi thêm tour. Vui lòng thử lại.");
            }

            return View(tour);
        }

        private async Task ProcessTourImages(int tourId, IFormFileCollection images)
        {
            try
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "tours");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                foreach (var image in images)
                {
                    if (image.Length > 0 && image.Length <= 5 * 1024 * 1024) // Max 5MB
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                        var filePath = Path.Combine(uploadsFolder, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(stream);
                        }

                        var tourImage = new TourImage
                        {
                            ID_Tour = tourId,
                            Image_URL = $"/img/tours/{fileName}",
                            Caption = Path.GetFileNameWithoutExtension(image.FileName)
                        };

                        _context.TourImages.Add(tourImage);
                    }
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ProcessTourImages Error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var tour = await _context.Tours
                    .Include(t => t.TourImages)
                    .Include(t => t.Itineraries)
                    .Include(t => t.Bookings)
                    .FirstOrDefaultAsync(t => t.ID_Tour == id);

                if (tour == null)
                {
                    return NotFound();
                }

                return View(tour);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Delete GET Error: {ex.Message}");
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int ID_Tour)
        {
            try
            {
                var tour = await _context.Tours
                    .Include(t => t.TourImages)
                    .Include(t => t.Itineraries)
                    .Include(t => t.Bookings)
                    .FirstOrDefaultAsync(t => t.ID_Tour == ID_Tour);

                if (tour == null)
                {
                    return NotFound();
                }

                // Delete physical image files
                foreach (var image in tour.TourImages)
                {
                    DeleteImageFile(image.Image_URL);
                }

                // EF Core will handle cascading deletes based on your DbContext configuration
                _context.Tours.Remove(tour);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Đã xóa tour '{tour.Name_Tour}' thành công!";
                return RedirectToAction("ListTour");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DeleteConfirmed Error: {ex.Message}");
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi xóa tour. Vui lòng thử lại.";
                return RedirectToAction("Delete", new { id = ID_Tour });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var tour = await _context.Tours
                    .Include(t => t.TourImages)
                    .Include(t => t.Itineraries)
                    .FirstOrDefaultAsync(t => t.ID_Tour == id);

                if (tour == null)
                {
                    return NotFound();
                }

                return View(tour);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Edit GET Error: {ex.Message}");
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Tour tour, IFormFileCollection TourImages, string DeletedImageIds)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Update tour information
                    _context.Tours.Update(tour);

                    // Handle deleted images
                    if (!string.IsNullOrEmpty(DeletedImageIds))
                    {
                        var deletedIds = DeletedImageIds.Split(',')
                            .Where(id => int.TryParse(id, out _))
                            .Select(int.Parse);

                        foreach (var imageId in deletedIds)
                        {
                            var imageToDelete = await _context.TourImages
                                .FirstOrDefaultAsync(ti => ti.ID_Image == imageId);

                            if (imageToDelete != null)
                            {
                                DeleteImageFile(imageToDelete.Image_URL);
                                _context.TourImages.Remove(imageToDelete);
                            }
                        }
                    }

                    // Handle new images
                    if (TourImages != null && TourImages.Count > 0)
                    {
                        await ProcessTourImages(tour.ID_Tour, TourImages);
                    }

                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Cập nhật tour thành công!";
                    return RedirectToAction("Detail", new { id = tour.ID_Tour });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Edit POST Error: {ex.Message}");
                ModelState.AddModelError("", "Có lỗi xảy ra khi cập nhật tour. Vui lòng thử lại.");
            }

            // Reload tour data if validation fails
            var tourWithImages = await _context.Tours
                .Include(t => t.TourImages)
                .FirstOrDefaultAsync(t => t.ID_Tour == tour.ID_Tour);

            return View(tourWithImages ?? tour);
        }

        private void DeleteImageFile(string imageUrl)
        {
            try
            {
                if (!string.IsNullOrEmpty(imageUrl) && imageUrl.StartsWith("/img/tours/"))
                {
                    var fileName = Path.GetFileName(imageUrl);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "tours", fileName);

                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DeleteImageFile Error: {ex.Message}");
            }
        }

        // Thêm action này vào TourController

        [HttpPost]
        public async Task<IActionResult> BulkAction(string action, int[] tourIds)
        {
            try
            {
                if (tourIds == null || tourIds.Length == 0)
                {
                    return Json(new { success = false, message = "Không có tour nào được chọn" });
                }

                var tours = await _context.Tours
                    .Where(t => tourIds.Contains(t.ID_Tour))
                    .ToListAsync();

                if (!tours.Any())
                {
                    return Json(new { success = false, message = "Không tìm thấy tour nào" });
                }

                switch (action.ToLower())
                {
                    case "delete":
                        // Get tour images for deletion
                        var tourImages = await _context.TourImages
                            .Where(ti => tourIds.Contains(ti.ID_Tour))
                            .ToListAsync();

                        // Delete physical image files
                        foreach (var image in tourImages)
                        {
                            DeleteImageFile(image.Image_URL);
                        }

                        // Remove tours (cascading delete will handle related data)
                        _context.Tours.RemoveRange(tours);
                        await _context.SaveChangesAsync();

                        return Json(new
                        {
                            success = true,
                            message = $"Đã xóa thành công {tours.Count} tour"
                        });

                    case "activate":
                        foreach (var tour in tours)
                        {
                            tour.Status = TourStatus.Active;
                        }
                        await _context.SaveChangesAsync();

                        return Json(new
                        {
                            success = true,
                            message = $"Đã kích hoạt {tours.Count} tour"
                        });

                    case "deactivate":
                        foreach (var tour in tours)
                        {
                            tour.Status = TourStatus.Inactive;
                        }
                        await _context.SaveChangesAsync();

                        return Json(new
                        {
                            success = true,
                            message = $"Đã tạm dừng {tours.Count} tour"
                        });

                    default:
                        return Json(new { success = false, message = "Hành động không hợp lệ" });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"BulkAction Error: {ex.Message}");
                return Json(new
                {
                    success = false,
                    message = "Có lỗi xảy ra khi thực hiện thao tác"
                });
            }
        }
    }
}



