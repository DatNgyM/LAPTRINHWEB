using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LAPTRINHWEB.Models;

namespace LAPTRINHWEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AccommodationController : Controller
    {
        private readonly TourDbContext _context;
        public AccommodationController(TourDbContext context) => _context = context;

        // GET: Admin/Accommodation
        public async Task<IActionResult> Index(string searchString, AccommodationType? typeFilter, string sortOrder)
        {
            var query = _context.Accommodations.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
                query = query.Where(a => a.Name.Contains(searchString)
                                       || a.Address.Contains(searchString));

            if (typeFilter.HasValue)
                query = query.Where(a => a.Type == typeFilter.Value);

            ViewData["NameSort"] = sortOrder == "name_desc" ? "name" : "name_desc";
            ViewData["RatingSort"] = sortOrder == "rating_desc" ? "rating" : "rating_desc";

            query = sortOrder switch
            {
                "name_desc" => query.OrderByDescending(a => a.Name),
                "rating_desc" => query.OrderByDescending(a => a.Star_Rating),
                _ => query.OrderBy(a => a.Name),
            };

            var list = await query.AsNoTracking().ToListAsync();
            return View(list);
        }

        // GET: Admin/Accommodation/Add
        public IActionResult Add()
        {
            ViewData["TypeList"] = new SelectList(
                Enum.GetValues(typeof(AccommodationType))
                    .Cast<AccommodationType>()
                    .Select(e => new { Value = e, Text = e.ToString() }),
                "Value", "Text"
            );
            return View();
        }

        // POST: Admin/Accommodation/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add([Bind("Name,Type,Address,Phone,Star_Rating,Description")] Accommodation accommodation)
        {
            if (ModelState.IsValid)
            {
                _context.Accommodations.Add(accommodation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TypeList"] = new SelectList(
                Enum.GetValues(typeof(AccommodationType))
                    .Cast<AccommodationType>()
                    .Select(e => new { Value = e, Text = e.ToString() }),
                "Value", "Text", accommodation.Type
            );
            return View(accommodation);
        }

        // GET: Admin/Accommodation/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!id.HasValue) return NotFound();
            var accommodation = await _context.Accommodations.FindAsync(id.Value);
            if (accommodation == null) return NotFound();

            ViewData["TypeList"] = new SelectList(
                Enum.GetValues(typeof(AccommodationType))
                    .Cast<AccommodationType>()
                    .Select(e => new { Value = e, Text = e.ToString() }),
                "Value", "Text", accommodation.Type
            );
            return View(accommodation);
        }

        // POST: Admin/Accommodation/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_Accommodation,Name,Type,Address,Phone,Star_Rating,Description")] Accommodation accommodation)
        {
            if (id != accommodation.ID_Accommodation) return NotFound();
            if (!ModelState.IsValid)
            {
                ViewData["TypeList"] = new SelectList(
                    Enum.GetValues(typeof(AccommodationType))
                        .Cast<AccommodationType>()
                        .Select(e => new { Value = e, Text = e.ToString() }),
                    "Value", "Text", accommodation.Type
                );
                return View(accommodation);
            }

            try
            {
                _context.Update(accommodation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Accommodations.AnyAsync(a => a.ID_Accommodation == id))
                    return NotFound();
                throw;
            }
        }

        // GET: Admin/Accommodation/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!id.HasValue) return NotFound();
            var accommodation = await _context.Accommodations
                                              .AsNoTracking()
                                              .FirstOrDefaultAsync(a => a.ID_Accommodation == id.Value);
            if (accommodation == null) return NotFound();
            return View(accommodation);
        }

        // POST: Admin/Accommodation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var accommodation = await _context.Accommodations.FindAsync(id);
            if (accommodation != null)
            {
                _context.Accommodations.Remove(accommodation);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}