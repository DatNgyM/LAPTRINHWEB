using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LAPTRINHWEB.Models;
using LAPTRINHWEB.Data;
using Microsoft.AspNetCore.Authorization;

namespace LAPTRINHWEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TourGuideController : Controller
    {
        private readonly TourDbContext _context;
        public TourGuideController(TourDbContext context)
        {
            _context = context;
        }

        // Hiển thị danh sách
        public IActionResult Index()
        {
            var guides = _context.TourGuides.ToList();
            return View(guides);
        }

        // Thêm mới (GET)
        public IActionResult Add()
        {
            return View();
        }

        // Thêm mới (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(TourGuide tourGuide)
        {
            if (ModelState.IsValid)
            {
                _context.TourGuides.Add(tourGuide);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tourGuide);
        }

        // Sửa (GET)
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return BadRequest();
            var guide = _context.TourGuides.Find(id);
            if (guide == null)
                return NotFound();
            return View(guide);
        }

        // Sửa (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, TourGuide tourGuide)
        {
            if (id != tourGuide.ID_Guide)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tourGuide);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.TourGuides.Any(e => e.ID_Guide == tourGuide.ID_Guide))
                        return NotFound();
                    throw;
                }
                return RedirectToAction("Index");
            }
            return View(tourGuide);
        }

        // Xóa (GET)
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return BadRequest();
            var guide = _context.TourGuides.FirstOrDefault(m => m.ID_Guide == id);
            if (guide == null)
                return NotFound();
            return View(guide);
        }

        // Xóa (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var guide = _context.TourGuides.Find(id);
            if (guide == null)
                return NotFound();
            _context.TourGuides.Remove(guide);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}